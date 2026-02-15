using Hospital.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hospital.Infrastructure.Validators;

/// <summary>
/// Validates Surgery appointments.
/// Rule: requires a specialist referral.
/// </summary>
public class SurgeryValidator(ILogger<SurgeryValidator> logger) : IDepartmentValidator
{
    public string Department => "Surgery";

    public Task<DepartmentValidationResult> ValidateAsync(string cpr, string doctorName)
    {
        if (!HasValidReferral(cpr))
        {
            return Task.FromResult(DepartmentValidationResult.Failure("Surgery requires a specialist referral."));
        }

        return Task.FromResult(DepartmentValidationResult.Success());
    }

    private bool HasValidReferral(string cpr)
    {
        logger.LogInformation("Checking specialist referral for {Cpr} in Surgery.", cpr);
        // TODO: Placeholder
        return true;
    }
}
