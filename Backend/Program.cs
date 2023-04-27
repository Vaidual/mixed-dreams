using Backend;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.Models;
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

const string PolicyName = "MixedDreamsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers();
    //.AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling =
    //    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        dbContextOptions => dbContextOptions.MigrationsAssembly("MixedDreams.Infrastructure"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
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

builder.Services.AddAuthentication(auth =>
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
        ValidIssuer = builder.Configuration["JwtToken:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtToken:Audience"],
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:SigningKey"]))
    };
});

// Cookies scheme
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "auth/login";
//        options.LogoutPath = "auth/logout";
//        options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
//        options.SlidingExpiration = true;
//        options.Cookie.Name = "Session";
//        options.Cookie.SameSite = SameSiteMode.Lax;
//        options.Cookie.HttpOnly = true;
//        options.Cookie.MaxAge = TimeSpan.FromDays(180);

//        options.Events.OnValidatePrincipal = context =>
//        {
//            if (context.Properties.Items.TryGetValue("LastLoginTime", out string lastLoginTimeString)
//                && DateTime.TryParse(lastLoginTimeString, out DateTime lastLoginTime))
//            {
//                var currentTime = DateTimeOffset.UtcNow;
//                var elapsedDays = (int)(currentTime - lastLoginTime).TotalSeconds;

//                if (elapsedDays >= 180)
//                {
//                    context.RejectPrincipal();
//                }
//            }
//            return Task.CompletedTask;
//        };
//    });

//Custom services
builder.Services.AddDataAccessServices();
builder.Services.AddIdentityServices();
builder.Services.AddMiddlewareServices();

builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors(PolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
