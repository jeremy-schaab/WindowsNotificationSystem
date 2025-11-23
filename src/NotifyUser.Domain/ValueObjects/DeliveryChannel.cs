namespace NotifyUser.Domain.ValueObjects;

/// <summary>
/// Defines the delivery mechanism for displaying notifications.
/// </summary>
public enum DeliveryChannel
{
    /// <summary>
    /// Windows toast notification (action center)
    /// </summary>
    Toast = 0,

    /// <summary>
    /// Custom WPF window overlay (if enabled)
    /// </summary>
    Window = 1,

    /// <summary>
    /// System tray balloon tip (legacy, Windows 10 only)
    /// </summary>
    Balloon = 2
}
