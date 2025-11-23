using NotifyUser.Domain.Aggregates;

namespace NotifyUser.Domain.Services;

/// <summary>
/// Service interface for Windows toast notifications.
/// </summary>
public interface IToastNotificationService
{
    /// <summary>
    /// Displays a Windows toast notification.
    /// </summary>
    Task<NotificationResult> ShowToastAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if toast notifications are supported on this system.
    /// </summary>
    bool IsSupported();
}
