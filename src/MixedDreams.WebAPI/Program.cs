using MixedDreams.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Formatting.Json;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.WebAPI.Filters;
using MixedDreams.WebAPI.Extensions;
using MixedDreams.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.Extensions.Options;
using MixedDreams.Infrastructure.Features.AuthFeatures.Login;
using MixedDreams.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MixedDreams.WebAPI;
using Bytewizer.Backblaze.Client;
using MixedDreams.Infrastructure.Exceptions;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using MixedDreams.Infrastructure.Hubs.Clients;
using MixedDreams.Infrastructure.Services;
using MixedDreams.Infrastructure.Exceptions.InternalServerError;
using MixedDreams.WebAPI.Middlewares;
using System.Net;
using MixedDreams.Infrastructure.Hubs;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);

    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddScoped<TenantMiddleware>();

    builder.Services.AddAndConfigureCors(builder.Configuration);

    builder.Services.AddSignalR();

    builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(typeof(ModelValidationFilterAttribute));
    });
    //.AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling =
    //    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    //builder.Services.AddTransient<LoggingInterceptor>();

    //builder.Services.AddHttpsRedirection(options =>
    //{
    //    options.HttpsPort = 7239;
    //});

    //Custom services
    builder.Services.AddInternalServices(builder.Configuration);
    builder.Services.AddAndConfigureAutoMapper();
    builder.Services.AddMiddlewareServices();

    builder.Services.AddAndConfigureAuthentification(builder.Configuration);

    //builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
    builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAndConfigureSwagger();
    builder.Services.AddMemoryCache();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.MigrateDatabase();

    app.UseExceptionMiddleware();

    await app.Services.GetRequiredService<IStorageClient>().ConnectAsync();
    await app.Services.GetRequiredService<IDeviceService>().ConnectAsync();

    app.UseHttpsRedirection();

    app.UseCors(builder.Configuration["Cors:Policy:Name"] ?? throw new MissingConfigurationOptionsException("Cors:Policy:Name"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<TenantMiddleware>();

    app.MapControllers();
    app.MapHub<NotificationHub>("/notificationHub");

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
