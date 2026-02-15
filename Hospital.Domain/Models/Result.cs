namespace Hospital.Domain.Results;

/// <summary>
/// Represents the outcome of an operation, carrying success/failure and an optional error message.
/// </summary>
public record Result(bool IsSuccess, string? ErrorMessage = null)
{
    public static Result Success() => new(true);

    public static Result Failure(string errorMessage) => new(false, errorMessage);
}
