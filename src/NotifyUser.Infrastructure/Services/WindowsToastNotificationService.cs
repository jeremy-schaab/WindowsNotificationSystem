using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using NotifyUser.Domain.Aggregates;
using NotifyUser.Domain.Services;
using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Infrastructure.Services;

/// <summary>
/// Infrastructure adapter for Windows toast notifications using UWP notification APIs.
/// </summary>
public sealed class WindowsToastNotificationService : IToastNotificationService
{
    private readonly ILogger<WindowsToastNotificationService> _logger;

    public WindowsToastNotificationService(ILogger<WindowsToastNotificationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool IsSupported()
    {
        // Toast notifications require Windows 10+ and desktop mode
        return OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10240);
    }

    public async Task<NotificationResult> ShowToastAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var startTime = Stopwatch.GetTimestamp();

        try
        {
            // Build and show toast using fluent builder
            // Note: No activation handler needed for simple notifications
            new ToastContentBuilder()
                .AddText(request.Title)
                .AddText(request.Message)
                .SetToastDuration(ToastDuration.Short)
                .Show();

            var displayLatency = Stopwatch.GetElapsedTime(startTime);
            var displayedAt = DateTime.UtcNow;

            _logger.LogDebug(
                "Toast notification displayed in {Latency}ms (RequestId={RequestId})",
                displayLatency.TotalMilliseconds,
                request.RequestId);

            // Return immediately - toast is fire-and-forget
            return NotificationResult.Success(request, displayedAt, displayLatency);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to display toast notification (RequestId={RequestId})",
                request.RequestId);

            var displayLatency = Stopwatch.GetElapsedTime(startTime);

            return NotificationResult.Failure(
                request,
                $"Toast notification failed: {ex.Message}",
                ExitCode.NotificationServiceUnavailable);
        }
    }

}
