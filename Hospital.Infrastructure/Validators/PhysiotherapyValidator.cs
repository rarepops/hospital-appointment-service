using Hospital.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hospital.Infrastructure.Validators;

/// <summary>
/// Validates Physiotherapy appointments.
/// Rules: requires a doctor's referral AND valid insurance approval.
/// </summary>
public class PhysiotherapyValidator(ILogger<PhysiotherapyValidator> logger) : IDepartmentValidator
{
    public string Department => "Physiotherapy";

    public Task<DepartmentValidationResult> ValidateAsync(string cpr, string doctorName)
    {
        if (!HasValidReferral(cpr))
        {
            return Task.FromResult(DepartmentValidationResult.Failure("Physiotherapy requires a doctor's referral."));
        }

        if (!HasValidInsuranceApproval(cpr))
        {
            return Task.FromResult(
                DepartmentValidationResult.Failure("Physiotherapy requires valid insurance approval.")
            );
        }

        return Task.FromResult(DepartmentValidationResult.Success());
    }

    private bool HasValidReferral(string cpr)
    {
        logger.LogDebug("Checking referral for {Cpr} in Physiotherapy.", cpr);
        // TODO: Placeholder
        return true;
    }

    private bool HasValidInsuranceApproval(string cpr)
    {
        logger.LogDebug("Checking insurance approval for {Cpr} in Physiotherapy.", cpr);
        // TODO: Placeholder
        return true;
    }
}
