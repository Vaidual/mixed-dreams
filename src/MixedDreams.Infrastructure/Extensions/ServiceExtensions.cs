using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.IdentitySetup;
using MixedDreams.Infrastructure.Repositories;
using MixedDreams.Infrastructure.Services;
using MixedDreams.Infrastructure.Tenants;
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
        public static void AddInternalServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITenantService, TenantService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAndConfigureIdentity();
            services.AddAndConfigureDbContext(config);
            services.AddAndConfigureBackblaze(config);

        }

        private static IServiceCollection AddAndConfigureIdentity(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredUniqueChars = 4;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // MixedDreamsClaimsFactory is used with SignInManager
            //services.AddScoped<IUserClaimsPrincipalFactory<Customer>, MixedDreamsClaimsFactory>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            return services;
        }

        private static IServiceCollection AddAndConfigureDbContext(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                    dbContextOptions => dbContextOptions.MigrationsAssembly("MixedDreams.Infrastructure"));
            });
            return services;
        }

        private static IServiceCollection AddAndConfigureBackblaze(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddBackblazeAgent(options =>
            {
                options.KeyId = config["Backblaze:WriteKey:KeyId"];
                options.ApplicationKey = config["Backblaze:WriteKey:ApplicationKey"];
            });
            return services;
        }
    }
}
