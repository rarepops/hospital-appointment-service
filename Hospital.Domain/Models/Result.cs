namespace Hospital.Domain.Results;

/// <summary>
/// Represents the outcome of an operation, carrying success/failure and an optional error message.
/// </summary>
public record Result(bool IsSuccess, string? ErrorMessage = null)
{
    public static Result Success() => new(true);

    public static Result Failure(string errorMessage) => new(false, errorMessage);
}

/// <summary>
/// Represents the outcome of an operation that returns a value on success.
/// </summary>
public record Result<T>(bool IsSuccess, T? Data = default, string? ErrorMessage = null)
{
    public static Result<T> Success(T data) => new(true, data);

    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
}
