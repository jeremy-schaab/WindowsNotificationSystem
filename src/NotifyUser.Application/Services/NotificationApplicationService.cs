using Microsoft.Extensions.Logging;
using NotifyUser.Domain.Aggregates;
using NotifyUser.Domain.Services;
using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Application.Services;

/// <summary>
/// Application service that coordinates notification display use cases.
/// Orchestrates domain model validation, notification display, and logging.
/// </summary>
public sealed class NotificationApplicationService
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationApplicationService> _logger;

    public NotificationApplicationService(
        INotificationService notificationService,
        ILogger<NotificationApplicationService> logger)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Displays a notification with the specified parameters.
    /// </summary>
    /// <param name="title">Notification title (required, max 100 characters)</param>
    /// <param name="message">Notification message (required, max 500 characters)</param>
    /// <param name="durationSeconds">Display duration in seconds (1-60, default 5)</param>
    /// <param name="type">Notification visual type (Info, Success, Warning, Error)</param>
    /// <param name="sound">Sound to play (None, Default, Success, Error, Warning, Info)</param>
    /// <param name="channel">Delivery channel (Toast, Window, Balloon)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>NotificationResult indicating success or failure with exit code</returns>
    public async Task<NotificationResult> DisplayNotificationAsync(
        string title,
        string message,
        int durationSeconds = 5,
        NotificationType type = NotificationType.Info,
        SoundType sound = SoundType.Default,
        DeliveryChannel channel = DeliveryChannel.Toast,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Displaying {Channel} notification: Title='{Title}', Type={Type}, Duration={Duration}s",
            channel, title, type, durationSeconds);

        // Create and validate notification request using domain model
        var requestResult = NotificationRequest.Create(
            title: title,
            message: message,
            durationSeconds: durationSeconds,
            type: type,
            sound: sound,
            channel: channel);

        // If validation failed, return failure result
        if (requestResult.IsFailure)
        {
            _logger.LogWarning("Notification request validation failed: {Error}", requestResult.Error);

            // Create a minimal valid request for error reporting
            // Use "Error" as fallback title/message to satisfy validation
            var fallbackResult = NotificationRequest.Create(
                title: string.IsNullOrWhiteSpace(title) ? "Error" : title.Length <= 100 ? title : title[..100],
                message: string.IsNullOrWhiteSpace(message) ? requestResult.Error : message.Length <= 500 ? message : message[..500],
                durationSeconds: 5,
                type: type,
                sound: SoundType.None,
                channel: channel);

            // This should always succeed since we're using valid values
            var fallbackRequest = fallbackResult.IsSuccess
                ? fallbackResult.Value
                : throw new InvalidOperationException("Fallback request creation failed");

            return NotificationResult.Failure(
                fallbackRequest,
                requestResult.Error,
                ExitCode.InvalidArguments);
        }

        var request = requestResult.Value;

        try
        {
            // Display notification using domain service
            var result = await _notificationService.DisplayAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    "Notification displayed successfully in {Latency}ms (RequestId={RequestId})",
                    result.DisplayLatency.TotalMilliseconds,
                    request.RequestId);
            }
            else
            {
                _logger.LogError(
                    "Notification display failed: {Error} (RequestId={RequestId}, ExitCode={ExitCode})",
                    result.ErrorMessage,
                    request.RequestId,
                    result.ExitCode);
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Notification display cancelled (RequestId={RequestId})", request.RequestId);

            return NotificationResult.Failure(
                request,
                "Notification display was cancelled by the user",
                ExitCode.UserCancelled);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error displaying notification (RequestId={RequestId})",
                request.RequestId);

            return NotificationResult.Failure(
                request,
                $"Unexpected error: {ex.Message}",
                ExitCode.GeneralError);
        }
    }
}
