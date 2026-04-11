namespace Properties;

public sealed class S3Settings
{
    public string Region { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretAccessKey { get; init; } = string.Empty;

}