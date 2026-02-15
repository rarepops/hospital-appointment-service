namespace Hospital.Domain.Interfaces;

/// <summary>
/// Defines department-specific validation rules for appointment scheduling.
/// Each department implements this interface with its own booking requirements.
/// To add a new department, create a new implementation — no changes to core logic required.
/// </summary>
public interface IDepartmentValidator
{
    /// <summary>
    /// The department name this validator handles (e.g., "General Practice", "Surgery").
    /// </summary>
    string Department { get; }

    /// <summary>
    /// Validates whether the appointment can be scheduled based on department-specific rules.
    /// </summary>
    /// <param name="cpr">Patient CPR number.</param>
    /// <param name="doctorName">Name of the doctor for the appointment.</param>
    /// <returns>A result indicating success or failure with an error message.</returns>
    Task<DepartmentValidationResult> ValidateAsync(string cpr, string doctorName);
}

public record DepartmentValidationResult(bool IsValid, string? ErrorMessage = null)
{
    public static DepartmentValidationResult Success() => new(true);

    public static DepartmentValidationResult Failure(string errorMessage) => new(false, errorMessage);
}
