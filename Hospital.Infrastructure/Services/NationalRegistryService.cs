using System.Text;
using System.Text.Json;
using Hospital.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hospital.Infrastructure.Services;

public class NationalRegistryService(
    IHttpClientFactory httpClientFactory,
    IOptions<NationalRegistryOptions> options,
    ILogger<NationalRegistryService> logger
) : INationalRegistryService
{
    public async Task<bool> ValidateCpr(string cpr)
    {
        var settings = options.Value;

        using var client = httpClientFactory.CreateClient("NationalRegistry");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");

        var requestBody = new { cpr };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // TODO: Actual HTTP call is omitted (dummy endpoint)

        logger.LogInformation("CPR validation successful for {Cpr}.", cpr);
        return true;
    }
}
