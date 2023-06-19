using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MixedDreams.Application.DeviceModels;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Options;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Application.Hubs;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System.Text;
using MixedDreams.Application.DeviceModels;
using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using MQTTnet.Server;

namespace MixedDreams.Application.Services
{
    internal class DeviceService : IDisposable, IDeviceService
    {
        const string RequestProductConstraintsTopic = "mixedDreams/request_constraints";
        const string ResponseProductConstraintsTopic = "mixedDreams/response_constraints/";
        const string OrderReceivedTopic = "mixedDreams/order_received";
        const string lowWaterTopic = "mixedDreams/low_water";

        private IMqttClient? _client;
        private readonly HiveMQOptions _hiveOptions;
        private readonly IHubContext<NotificationHub, INotifyClient> _hubContext;
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;

        public DeviceService(
            IOptions<HiveMQOptions> hiveOptions,
            IServiceProvider services,
            IHubContext<NotificationHub, INotifyClient> hubContext,
            ILoggerFactory loggerFactory)
        {
            _hiveOptions = hiveOptions.Value;
            _services = services;
            _hubContext = hubContext;
            _logger = loggerFactory.CreateLogger<DeviceService>();
        }

        public async Task ConnectAsync()
        {
            try
            {
                var mqttFactory = new MqttFactory();

                _client = mqttFactory.CreateMqttClient();

                MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithClientId(_hiveOptions.ClientId)
                    .WithTcpServer(_hiveOptions.Server)
                    .WithCredentials(_hiveOptions.Username, _hiveOptions.Password)
                    .WithTls()
                    .WithCleanSession()
                    .Build();

                var response = await _client.ConnectAsync(mqttClientOptions, CancellationToken.None);

                _client.DisconnectedAsync += async e =>
                {
                    if (e.ClientWasConnected)
                    {
                        // Use the current options as the new options.
                        await _client.ConnectAsync(mqttClientOptions);
                    }
                };

                _client.ApplicationMessageReceivedAsync += async e =>
                {
                    switch (e.ApplicationMessage.Topic)
                    {
                        case RequestProductConstraintsTopic:
                            await ProcessConstraintsRequest(e);
                            break;
                        case OrderReceivedTopic:
                            await SetOrderReceivedAsync(e);
                            break;
                        case lowWaterTopic:
                            await NotifyAboutLowWater(e);
                            break;
                        default:
                            break;
                    }
                };

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(RequestProductConstraintsTopic).WithAtLeastOnceQoS();
                        })
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(OrderReceivedTopic).WithExactlyOnceQoS();
                        })
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(lowWaterTopic);
                        })
                    .Build();

                await _client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to topic {0}", RequestProductConstraintsTopic);
            }
            catch (BaseException ex)
            {
                _logger.Log(ex.LogLevel, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task ProcessConstraintsRequest(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine("Requested for product constraints.");
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            ConstraintsRequest constraintsRequest = JsonConvert.DeserializeObject<ConstraintsRequest>(payload)
                ?? throw new WrongMqttMessageSignatureException(e.ApplicationMessage.Topic, payload, nameof(ConstraintsRequest));

            using var scope = _services.CreateScope();
            IUnitOfWork unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.OrderRepository.SetOrderPrepared(Guid.Parse(constraintsRequest.OrderId));
            await unitOfWork.SaveAsync();

            ProductConstraints productConstraints = await unitOfWork.OrderRepository.GetOrderProductConstraints(Guid.Parse(constraintsRequest.OrderId));

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(ResponseProductConstraintsTopic + constraintsRequest.DeviceId)
                .WithPayload(JsonConvert.SerializeObject(productConstraints))
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }

        private async Task NotifyAboutLowWater(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine("Low water.");
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            LowWaterNotify lowWaterNotify = JsonConvert.DeserializeObject<LowWaterNotify>(payload)
                ?? throw new WrongMqttMessageSignatureException(e.ApplicationMessage.Topic, payload, nameof(LowWaterNotify));

            using var scope = _services.CreateScope();
            IUnitOfWork unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            string userId = await unitOfWork.DeviceRepository.GetUserId(lowWaterNotify.DeviceId) ?? throw new DeviceHaveNoCompanyException(lowWaterNotify.DeviceId);
            await _hubContext.Clients.Group(userId).LowWaterNotification(lowWaterNotify.WaterLevel);
        }

        private async Task SetOrderReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            OrderReceived orderReceived = JsonConvert.DeserializeObject<OrderReceived>(payload) ??
                throw new WrongMqttMessageSignatureException(e.ApplicationMessage.Topic, payload, nameof(OrderReceived));

            Console.WriteLine($"Order ${orderReceived.OrderId} is received.");

            using var scope = _services.CreateScope();
            IUnitOfWork unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.OrderRepository.SetOrderReceived(Guid.Parse(orderReceived.OrderId));
            await unitOfWork.SaveAsync();
        }

        public void Dispose()
        {
            _client?.DisconnectAsync();
        }
    }
}
