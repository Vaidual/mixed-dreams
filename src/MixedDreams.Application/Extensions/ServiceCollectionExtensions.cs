using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Infrastructure.Hubs.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
