using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using API;
using Applications;
using Ardalis.GuardClauses;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Leases;
using Managers;
// using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Payments;
using Properties;
using Serilog;
using SharedKernel;
using Tenants;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHttpLogging(o => { });

// For RPC endpoints
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    options.ListenLocalhost(5168, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

// CORS
// Todo: Configurar esto correctamente y en base a las variables de entorno
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = builder.Configuration["Authentication:Audience"];
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
            ValidateIssuerSigningKey = true
        };
        o.Events = new JwtBearerEvents()
        {
            OnTokenValidated = async context =>
            {
                var userRole = context.Principal!.Claims.SingleOrDefault(c => c.Type == "custom:role")?.Value;
                Guard.Against.NullOrEmpty(userRole);
                var userClaims = new Claim(ClaimTypes.Role, userRole);

                ((ClaimsIdentity)context.Principal!.Identity!).AddClaim(userClaims);
            }
        };
    });

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Add Module Services
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddTenantsModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddManagersModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddPropertiesModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddLeasesModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddPaymentsModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddApplicationsModule(builder.Configuration, logger, mediatRAssemblies);

// Set up Mediator (source generator based)
builder.Services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
builder.Services.AddMediatRLoggingBehavior();
builder.Services.AddMediatRFluentValidationBehavior();

// Set up MassTransit
builder.Services.AddMassTransitConfig(builder.Configuration);

var app = builder.Build();
app.UseCors();
app.UseHttpLogging();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication()
    .UseAuthorization();

app.MapPropertyModule();
app.MapLeaseModule();
app.MapTenantModule();
app.MapManagerModule();

app.UseFastEndpoints(
        c =>
        {
            c.Errors.UseProblemDetails();
        })
    .UseSwaggerGen();

app.Run();
            
public partial class Program { }