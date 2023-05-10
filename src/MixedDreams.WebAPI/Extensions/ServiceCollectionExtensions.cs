using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MixedDreams.WebAPI.Middlewares;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using MixedDreams.Application.Exceptions;

namespace MixedDreams.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
                options.AddPolicy(name: config["Cors:Policy:Name"] ?? throw new InternalServerErrorException("Cors policy name isn't specified or options path is incorrect"),
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            return services;
        }

        public static IServiceCollection AddAndConfigureAuthentification(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"] ?? throw new InternalServerErrorException("Jwt Issuer key isn't specified or options path is incorrect"),
                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"] ?? throw new InternalServerErrorException("Jwt Audience isn't specified or options path is incorrect"),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            config["Jwt:SigningKey"] ?? 
                            throw new InternalServerErrorException("Jwt signing key isn't specified or options path is incorrect")))
                };
            });
            return services;
        }

        public static IServiceCollection AddAndConfigureSwagger(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                //options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }
    }
}
