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
using MixedDreams.Application.Features.AuthFeatures.Login;
using MixedDreams.Application.Extensions;

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

    builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(typeof(ModelValidationFilterAttribute));
    });
    //.AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling =
    //    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    //builder.Services.AddTransient<LoggingInterceptor>();

    //Custom services
    builder.Services.AddInternalServices(builder.Configuration);
    builder.Services.AddAndConfigureAutoMapper();
    builder.Services.AddMiddlewareServices();

    builder.Services.AddAndConfigureAuthentification(builder.Configuration);

    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
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
