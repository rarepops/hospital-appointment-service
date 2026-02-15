using Hospital.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hospital.Infrastructure.Validators;

/// <summary>
/// Validates Radiology appointments.
/// Rules: requires a doctor's referral and may require financial approval.
/// </summary>
public class RadiologyValidator(ILogger<RadiologyValidator> logger) : IDepartmentValidator
{
    public string Department => "Radiology";

    public Task<DepartmentValidationResult> ValidateAsync(string cpr, string doctorName)
    {
        if (!HasValidReferral(cpr))
        {
            return Task.FromResult(DepartmentValidationResult.Failure("Radiology requires a doctor's referral."));
        }

        if (!HasValidFinancialApproval(cpr))
        {
            return Task.FromResult(
                DepartmentValidationResult.Failure("Radiology procedures require financial approval.")
            );
        }

        return Task.FromResult(DepartmentValidationResult.Success());
    }

    private bool HasValidReferral(string cpr)
    {
        logger.LogDebug("Checking referral for {Cpr} in Radiology.", cpr);
        // TODO: Placeholder
        return true;
    }

    private bool HasValidFinancialApproval(string cpr)
    {
        logger.LogDebug("Checking financial approval for {Cpr} in Radiology.", cpr);
        // TODO: Placeholder
        return true;
    }
}
