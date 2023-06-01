using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MixedDreams.Infrastructure.DeviceModels;
using MixedDreams.Infrastructure.Exceptions;
using MixedDreams.Infrastructure.Options;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Infrastructure.Hubs.Clients;
using MixedDreams.Infrastructure.Hubs;
using MixedDreams.Infrastructure.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    internal class DeviceService : IDisposable, IDeviceService
    {
        const string RequestProductConstraintsTopic = "mixedDreams/request_constraints";
        const string ResponseProductConstraintsTopic = "mixedDreams/response_constraints/";
        const string ProductReceivedTopic = "mixedDreams/product_received";
        const string lowWaterTopic = "mixedDreams/low_water";

        private IMqttClient? _client;
        private readonly HiveMQOptions _hiveOptions;
        private readonly IHubContext<NotificationHub, INotifyClient> _hubContext;
        private readonly IServiceProvider _services;

        public DeviceService(
            IOptions<HiveMQOptions> hiveOptions,
            IServiceProvider services,
            IHubContext<NotificationHub, INotifyClient> hubContext)
        {
            _hiveOptions = hiveOptions.Value;
            _services = services;
            _hubContext = hubContext;
        }

        public async Task ConnectAsync()
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


            _client.ApplicationMessageReceivedAsync += async e =>
            {
                switch (e.ApplicationMessage.Topic)
                {
                    case RequestProductConstraintsTopic:
                        await ProcessConstraintsRequest(e);
                        break;
                    case ProductReceivedTopic:
                        Console.WriteLine("Product is received.");
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
                        f.WithTopic(RequestProductConstraintsTopic).WithAtMostOnceQoS();
                    })
                .WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(ProductReceivedTopic).WithExactlyOnceQoS();
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

        public async Task ProcessConstraintsRequest(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine("Requested for product constraints.");
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            ConstraintsRequest constraintsRequest = JsonConvert.DeserializeObject<ConstraintsRequest>(payload)
                ?? throw new WrongMqttMessageSignatureException(e.ApplicationMessage.Topic, payload, nameof(ConstraintsRequest));

            using var scope = _services.CreateScope();
            IUnitOfWork unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            ProductConstraints productConstraints = await unitOfWork.ProductRepository.GetProductConstraints(Guid.Parse(constraintsRequest.ProductId));

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(ResponseProductConstraintsTopic + constraintsRequest.ClientId)
                .WithPayload(JsonConvert.SerializeObject(productConstraints))
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }

        public async Task NotifyAboutLowWater(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine("Low water.");
            string deviceId = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

            using var scope = _services.CreateScope();
            IUnitOfWork unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            string userId = await unitOfWork.DeviceRepository.GetUserId(deviceId) ?? throw new DeviceHaveNoCompanyException(deviceId);
            await _hubContext.Clients.Group(userId).LowWaterNotification(3);
        }

        public void Dispose()
        {
            _client?.DisconnectAsync();
        }
    }
}
