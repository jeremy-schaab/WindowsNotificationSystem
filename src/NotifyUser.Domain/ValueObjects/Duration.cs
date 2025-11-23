using NotifyUser.Domain.Common;

namespace NotifyUser.Domain.ValueObjects;

/// <summary>
/// Represents the display duration for a notification in seconds.
/// Immutable value object with built-in validation.
/// </summary>
public sealed record Duration
{
    private const int MinSeconds = 1;
    private const int MaxSeconds = 60;

    public int Seconds { get; init; }
    public TimeSpan TimeSpan => TimeSpan.FromSeconds(Seconds);

    private Duration(int seconds)
    {
        Seconds = seconds;
    }

    /// <summary>
    /// Creates a Duration with validation.
    /// </summary>
    public static Result<Duration> Create(int seconds)
    {
        if (seconds < MinSeconds)
            return Result<Duration>.Failure($"Duration must be at least {MinSeconds} second");

        if (seconds > MaxSeconds)
            return Result<Duration>.Failure($"Duration cannot exceed {MaxSeconds} seconds");

        return Result<Duration>.Success(new Duration(seconds));
    }

    /// <summary>
    /// Default duration (5 seconds)
    /// </summary>
    public static Duration Default => new(5);

    /// <summary>
    /// Short duration (3 seconds)
    /// </summary>
    public static Duration Short => new(3);

    /// <summary>
    /// Long duration (10 seconds)
    /// </summary>
    public static Duration Long => new(10);
}
