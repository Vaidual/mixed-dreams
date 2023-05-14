using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MixedDreams.Application.ServicesInterfaces;
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
        /// Defines how often in minutes database backup must be executed
        /// </summary>
        public int ReplicationFrequency { get; private set; } = 60 * 24;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

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
