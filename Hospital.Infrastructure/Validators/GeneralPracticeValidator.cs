using Hospital.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hospital.Infrastructure.Validators;

/// <summary>
/// Validates General Practice appointments.
/// Rule: patients can only book with their assigned GP.
/// </summary>
public class GeneralPracticeValidator(ILogger<GeneralPracticeValidator> logger) : IDepartmentValidator
{
    public string Department => "General Practice";

    public Task<DepartmentValidationResult> ValidateAsync(string cpr, string doctorName)
    {
        if (!IsAssignedToGP(cpr, doctorName))
        {
            return Task.FromResult(
                DepartmentValidationResult.Failure("Patients can only book appointments with their assigned GP.")
            );
        }

        return Task.FromResult(DepartmentValidationResult.Success());
    }

    private bool IsAssignedToGP(string cpr, string doctorName)
    {
        logger.LogInformation("Checking GP assignment for {Cpr} with {DoctorName}.", cpr, doctorName);
        // TODO: Placeholder, implement actual logic
        return true;
    }
}
