using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MixedDreams.Infrastructure.Hubs.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MixedDreams.Infrastructure.Services
{
    public class ReplicationService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ReplicationService(IServiceProvider services)
        {
            _services = services;
        }

        /// <summary>
        /// Defines how often database backup must be executed.
        /// </summary>
        public TimeSpan ReplicationFrequency { get; private set; } = TimeSpan.FromDays(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(ReplicationFrequency);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _services.CreateScope();
                var backupService =
                    scope.ServiceProvider
                        .GetRequiredService<IBackupService>();

                await backupService.CreateBackupAsync();
            }
        }
    }
}
