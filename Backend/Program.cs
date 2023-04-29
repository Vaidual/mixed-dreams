using Backend;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using MixedDreams.WebAPI.IdentitySetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Http;
using Serilog;
using MixedDreams.WebAPI.Extensions;
using Serilog.Formatting.Json;
using MixedDreams.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

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

    builder.Services.AddAndConfigureCors(builder.Configuration);

    builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
    builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(typeof(ModelValidationFilterAttribute));
    });
    //.AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling =
    //    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    //builder.Services.AddTransient<LoggingInterceptor>();
    builder.Services.AddAndConfigureDbContext(builder.Configuration);

    builder.Services.AddAndConfigureIdentity();
    builder.Services.AddAndConfigureAuthentification(builder.Configuration);

    //Custom services
    builder.Services.AddDataAccessServices();
    builder.Services.AddMiddlewareServices();

    builder.Services.AddAutoMapper(typeof(Program));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAndConfigureSwagger();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MigrateDatabase();

    app.UseExceptionMiddleware();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseCors(builder.Configuration["Cors:Policy:Name"]);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
