
namespace API;

public sealed class JWTBearerSettings
{
    public string Authority { get; init; } = string.Empty;
    public string MetadataAddress { get; init; } = string.Empty;
    public bool IncludeErrorDetails { get; init; }
    public bool RequireHttpsMetadata { get; init; }
    public TokenValidationParametersSettings TokenValidationParameters { get; init; }
}

public sealed class TokenValidationParametersSettings
{
    public bool ValidateIssuer { get; init; }
    public bool ValidateAudience { get; init; }
    public bool ValidateIssuerSigningKey { get; init; }
}