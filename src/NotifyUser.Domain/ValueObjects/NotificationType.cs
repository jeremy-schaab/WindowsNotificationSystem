namespace NotifyUser.Domain.ValueObjects;

/// <summary>
/// Defines the visual type and urgency level of a notification.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Informational notification (blue/neutral styling)
    /// </summary>
    Info = 0,

    /// <summary>
    /// Success notification (green/positive styling)
    /// </summary>
    Success = 1,

    /// <summary>
    /// Warning notification (yellow/amber styling)
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Error notification (red/negative styling)
    /// </summary>
    Error = 3
}
