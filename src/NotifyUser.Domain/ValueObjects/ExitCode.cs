namespace NotifyUser.Domain.ValueObjects;

/// <summary>
/// Standard exit codes for PowerShell integration.
/// </summary>
public enum ExitCode
{
    /// <summary>
    /// Notification displayed successfully (0)
    /// </summary>
    Success = 0,

    /// <summary>
    /// General error occurred (1)
    /// </summary>
    GeneralError = 1,

    /// <summary>
    /// Invalid command-line arguments (2)
    /// </summary>
    InvalidArguments = 2,

    /// <summary>
    /// Windows notification service unavailable (3)
    /// </summary>
    NotificationServiceUnavailable = 3,

    /// <summary>
    /// Audio playback failed (non-blocking, 4)
    /// </summary>
    AudioPlaybackFailed = 4,

    /// <summary>
    /// User cancelled notification (window only, 5)
    /// </summary>
    UserCancelled = 5
}
