namespace Hospital.Infrastructure.Services;

/// <summary>
/// Configuration for the National Registry external API.
/// Sensitive values should come from user-secrets or a vault — never hardcoded.
/// </summary>
public class NationalRegistryOptions
{
    public const string SectionName = "NationalRegistry";

    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
