using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;
using API;
using Applications;
using Ardalis.GuardClauses;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Leases;
using Managers;
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
builder.Services.AddProblemDetails();

var kestrelRuntimeSettings = KestrelRuntimeSettings.From(builder.Configuration);
builder.WebHost.ConfigureKestrel(kestrelRuntimeSettings.Configure);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["AllowedOrigin"]!);
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
// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        var httpContext = context.HttpContext;
        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        var detail = "Too many requests. Please try again later.";

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
        {
            httpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds}";
            detail = $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.";
        }

        var problemDetailsService =
            httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Too Many Requests",
                Detail = detail
            }
        });
    };

    options.AddPolicy("per-user", httpContext =>
    {
        // Revisar cual es la key que pertenece al id del usuario
        string? userId = httpContext.User.FindFirstValue("sub");

        if (!string.IsNullOrWhiteSpace(userId))
        {
            return RateLimitPartition.GetTokenBucketLimiter(
                userId,
                _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 25,
                    TokensPerPeriod = 2,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1)
                });
        }

        return RateLimitPartition.GetFixedWindowLimiter(
            "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            });
    });
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

app.UseRateLimiter();

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

await app.MigrateApplicationsDbAsync();
await app.MigrateLeasesDbAsync();
await app.MigrateManagersDbAsync();
await app.MigratePaymentsDbAsync();
await app.MigratePropertiesDbAsync();
await app.MigrateTenantsDbAsync();

app.Run();
            
public partial class Program { }
