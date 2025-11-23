using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Domain.Aggregates;

/// <summary>
/// Represents the result of a notification display operation.
/// Immutable value object containing success/failure state and metadata.
/// </summary>
public sealed record NotificationResult
{
    public required NotificationRequest Request { get; init; }
    public required bool IsSuccess { get; init; }
    public required ExitCode ExitCode { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTime DisplayedAt { get; init; }
    public TimeSpan DisplayLatency { get; init; }

    public bool IsFailure => !IsSuccess;

    private NotificationResult() { }

    /// <summary>
    /// Creates a successful notification result.
    /// </summary>
    public static NotificationResult Success(
        NotificationRequest request,
        DateTime displayedAt,
        TimeSpan displayLatency)
    {
        return new NotificationResult
        {
            Request = request,
            IsSuccess = true,
            ExitCode = ExitCode.Success,
            ErrorMessage = null,
            DisplayedAt = displayedAt,
            DisplayLatency = displayLatency
        };
    }

    /// <summary>
    /// Creates a failed notification result.
    /// </summary>
    public static NotificationResult Failure(
        NotificationRequest request,
        string errorMessage,
        ExitCode exitCode)
    {
        return new NotificationResult
        {
            Request = request,
            IsSuccess = false,
            ExitCode = exitCode,
            ErrorMessage = errorMessage,
            DisplayedAt = DateTime.UtcNow,
            DisplayLatency = TimeSpan.Zero
        };
    }
}
