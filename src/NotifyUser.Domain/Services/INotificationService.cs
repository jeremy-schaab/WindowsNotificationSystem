using NotifyUser.Domain.Aggregates;

namespace NotifyUser.Domain.Services;

/// <summary>
/// Domain service interface for displaying notifications.
/// Main entry point for notification operations.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Displays a notification using the appropriate channel and returns the result.
    /// </summary>
    Task<NotificationResult> DisplayAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default);
}
