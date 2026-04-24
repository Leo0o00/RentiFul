using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace API;

public sealed class KestrelRuntimeSettings
{
    public string? BindAddress { get; init; }
    public int? GrpcPort { get; init; }
    public int? HttpPort { get; init; }
    public bool? GrpcUseHttps { get; init; }
    public bool? HttpUseHttps { get; init; }

    public static KestrelRuntimeSettings From(IConfiguration configuration)
    {
        var section = configuration.GetSection("KestrelRuntime");
        var runningInContainer = string.Equals(
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
            bool.TrueString,
            StringComparison.OrdinalIgnoreCase);

        return new KestrelRuntimeSettings
        {
            BindAddress = string.IsNullOrWhiteSpace(section["BindAddress"])
                ? (runningInContainer ? IPAddress.Any.ToString() : "localhost")
                : section["BindAddress"],
            GrpcPort = section.GetValue<int?>("GrpcPort") ?? 5001,
            HttpPort = section.GetValue<int?>("HttpPort") ?? 5168,
            GrpcUseHttps = section.GetValue<bool?>("GrpcUseHttps") ?? true,
            HttpUseHttps = section.GetValue<bool?>("HttpUseHttps") ?? true
        };
    }

    public void Configure(KestrelServerOptions options)
    {
        ConfigureListener(options, GrpcPort!.Value, HttpProtocols.Http2, GrpcUseHttps!.Value);
        ConfigureListener(options, HttpPort!.Value, HttpProtocols.Http1, HttpUseHttps!.Value);
    }

    private void ConfigureListener(
        KestrelServerOptions options,
        int port,
        HttpProtocols protocols,
        bool useHttps)
    {
        void ConfigureListenOptions(ListenOptions listenOptions)
        {
            if (useHttps)
            {
                listenOptions.UseHttps();
            }

            listenOptions.Protocols = protocols;
        }

        if (string.Equals(BindAddress, "localhost", StringComparison.OrdinalIgnoreCase))
        {
            options.ListenLocalhost(port, ConfigureListenOptions);
            return;
        }

        if (string.Equals(BindAddress, "any", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(BindAddress, "*", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(BindAddress, IPAddress.Any.ToString(), StringComparison.OrdinalIgnoreCase) ||
            string.Equals(BindAddress, IPAddress.IPv6Any.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            options.ListenAnyIP(port, ConfigureListenOptions);
            return;
        }

        if (!IPAddress.TryParse(BindAddress, out var ipAddress))
        {
            throw new InvalidOperationException(
                $"The configured KestrelRuntime:BindAddress value '{BindAddress}' is not valid.");
        }

        options.Listen(ipAddress, port, ConfigureListenOptions);
    }
}
