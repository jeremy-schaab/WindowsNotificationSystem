namespace NotifyUser.Domain.ValueObjects;

/// <summary>
/// Defines the system sound to play with a notification.
/// </summary>
public enum SoundType
{
    /// <summary>
    /// No sound (silent notification)
    /// </summary>
    None = 0,

    /// <summary>
    /// Default system beep
    /// </summary>
    Default = 1,

    /// <summary>
    /// Success/asterisk sound
    /// </summary>
    Success = 2,

    /// <summary>
    /// Error/hand sound
    /// </summary>
    Error = 3,

    /// <summary>
    /// Warning/exclamation sound
    /// </summary>
    Warning = 4,

    /// <summary>
    /// Info beep sound
    /// </summary>
    Info = 5
}
