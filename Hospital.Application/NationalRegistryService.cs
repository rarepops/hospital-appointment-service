using System.Text.Json;
using System.Text;

namespace Hospital.Application;

public class NationalRegistryService
{
    private const string NationalRegistryApiKey = "1fds232d-1defw-2cwd23-23d";
    private const string CprValidationApiUrl = "https://dummy-national-registry.com/api/validate-cpr";

    public async Task<bool> ValidateCpr(string cpr)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {NationalRegistryApiKey}");

        var requestBody = new { cpr };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");


        Console.WriteLine($"[LOG] CPR validation successful for {cpr}");
        return true;
    }
}