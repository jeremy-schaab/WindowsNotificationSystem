namespace NotifyUser.Domain.Common;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// Implements the Result pattern for functional error handling.
/// </summary>
public sealed record Result<T>
{
    public T Value { get; init; }
    public string Error { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;

    private Result(T value, string error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
    }

    public static Result<T> Success(T value) =>
        new(value, string.Empty, true);

    public static Result<T> Failure(string error) =>
        new(default!, error, false);

    public static Result<T> Failure(T value, string error) =>
        new(value, error, false);
}

/// <summary>
/// Represents the result of an operation without a return value.
/// </summary>
public sealed record Result
{
    public string Error { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;

    private Result(string error, bool isSuccess)
    {
        Error = error;
        IsSuccess = isSuccess;
    }

    public static Result Success() =>
        new(string.Empty, true);

    public static Result Failure(string error) =>
        new(error, false);
}
