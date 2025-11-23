using NotifyUser.Domain.Aggregates;
using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Domain.Services;

/// <summary>
/// Domain service that orchestrates notification display by coordinating
/// toast notifications and audio playback.
/// </summary>
public sealed class NotificationOrchestrationService : INotificationService
{
    private readonly IToastNotificationService _toastService;
    private readonly IAudioService _audioService;

    public NotificationOrchestrationService(
        IToastNotificationService toastService,
        IAudioService audioService)
    {
        _toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
    }

    public async Task<NotificationResult> DisplayAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Start audio playback asynchronously (fire-and-forget)
        if (request.Sound != SoundType.None && _audioService.IsAvailable())
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _audioService.PlaySoundAsync(request.Sound, cancellationToken);
                }
                catch
                {
                    // Audio failures are non-blocking
                }
            }, cancellationToken);
        }

        // Display notification based on channel
        return request.Channel switch
        {
            DeliveryChannel.Toast => await DisplayToastAsync(request, cancellationToken),
            DeliveryChannel.Window => throw new NotSupportedException("Window notifications not yet implemented"),
            DeliveryChannel.Balloon => throw new NotSupportedException("Balloon notifications not supported in this version"),
            _ => NotificationResult.Failure(
                request,
                $"Unsupported delivery channel: {request.Channel}",
                ExitCode.GeneralError)
        };
    }

    private async Task<NotificationResult> DisplayToastAsync(
        NotificationRequest request,
        CancellationToken cancellationToken)
    {
        if (!_toastService.IsSupported())
        {
            return NotificationResult.Failure(
                request,
                "Toast notifications are not supported on this system",
                ExitCode.NotificationServiceUnavailable);
        }

        return await _toastService.ShowToastAsync(request, cancellationToken);
    }
}
