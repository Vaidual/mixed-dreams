using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.IdentitySetup;
using MixedDreams.Infrastructure.Options;
using MixedDreams.Infrastructure.Repositories;
using MixedDreams.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddInternalOptions(config);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IBackblazeService, BackblazeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddHostedServices();

            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITenantGetter>(provider => provider.GetService<ITenantService>() ?? throw new ServiceNotRegisteredException(nameof(ITenantService)));
            services.AddScoped<ITenantSetter>(provider => provider.GetService<ITenantService>() ?? throw new ServiceNotRegisteredException(nameof(ITenantService)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAndConfigureIdentity();
            services.AddAndConfigureDbContext(config);
            services.AddAndConfigureBackblaze(config);

            return services;
        }

        private static IServiceCollection AddAndConfigureIdentity(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // MixedDreamsClaimsFactory is used with SignInManager
            //services.AddScoped<IUserClaimsPrincipalFactory<Customer>, MixedDreamsClaimsFactory>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<IUserStore<ApplicationUser>, AppUserStore>();

            return services;
        }

        private static IServiceCollection AddAndConfigureDbContext(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection" ?? throw new InternalServerErrorException("Default connection string isn't specified or options path is incorrect")),
                    dbContextOptions => dbContextOptions.MigrationsAssembly("MixedDreams.Infrastructure"));
            });

            return services;
        }

        private static IServiceCollection AddInternalOptions(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddOptions<JwtOptions>()
                .Bind(config.GetSection(JwtOptions.Jwt))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<BackblazeOptions>()
                .Bind(config.GetSection(BackblazeOptions.Backblaze))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<BackupOptions>()
                .Bind(config.GetSection(BackupOptions.Backup))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        private static IServiceCollection AddAndConfigureBackblaze(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddBackblazeAgent(options =>
            {
                options.KeyId = config["Backblaze:WriteKey:KeyId"] ?? throw new InternalServerErrorException("Backblaze-WriteKey KeyId isn't specified or options path is incorrect");
                options.ApplicationKey = config["Backblaze:WriteKey:ApplicationKey"] ?? throw new InternalServerErrorException("Backblaze-WriteKey ApplicationKey isn't specified or options path is incorrect");
            });

            return services;
        }

        private static IServiceCollection AddHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<ReplicationService>();

            return services;
        }
    }
}
