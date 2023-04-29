using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MixedDreams.Core.Services;
using MixedDreams.Core.ServicesInterfaces;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.Entities;
using MixedDreams.WebAPI.IdentitySetup;
using MixedDreams.WebAPI.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace MixedDreams.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            return services;
        }

        public static IServiceCollection AddMiddlewareServices(
            this IServiceCollection services)
        {
            services.AddTransient<ExceptionMiddleware>();
            return services;
        }

        public static IServiceCollection AddAndConfigureCors(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: config["Cors:Policy:Name"],
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            return services;
        }

        public static IServiceCollection AddAndConfigureIdentity(
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

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MixedDreamsClaimsFactory>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            return services;
        }

        public static IServiceCollection AddAndConfigureAuthentification(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JwtToken:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["JwtToken:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:SigningKey"]))
                };
            });
            return services;
        }

        public static IServiceCollection AddAndConfigureSwagger(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "oauth2", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddAndConfigureDbContext(
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
    }
}
