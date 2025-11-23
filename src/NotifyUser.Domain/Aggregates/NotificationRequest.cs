using NotifyUser.Domain.Common;
using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Domain.Aggregates;

/// <summary>
/// Aggregate root representing a notification request.
/// Immutable with built-in business rule validation.
/// </summary>
public sealed record NotificationRequest
{
    public required Guid RequestId { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
    public required Duration Duration { get; init; }
    public required NotificationType Type { get; init; }
    public required SoundType Sound { get; init; }
    public required DeliveryChannel Channel { get; init; }
    public DateTime CreatedAt { get; init; }

    private NotificationRequest() { }

    /// <summary>
    /// Factory method to create a NotificationRequest with validation.
    /// </summary>
    public static Result<NotificationRequest> Create(
        string title,
        string message,
        int durationSeconds = 5,
        NotificationType type = NotificationType.Info,
        SoundType sound = SoundType.Default,
        DeliveryChannel channel = DeliveryChannel.Toast)
    {
        // Validate title
        if (string.IsNullOrWhiteSpace(title))
            return Result<NotificationRequest>.Failure("Title is required");

        if (title.Length > 100)
            return Result<NotificationRequest>.Failure("Title cannot exceed 100 characters");

        // Validate message
        if (string.IsNullOrWhiteSpace(message))
            return Result<NotificationRequest>.Failure("Message is required");

        if (message.Length > 500)
            return Result<NotificationRequest>.Failure("Message cannot exceed 500 characters");

        // Create duration
        var durationResult = Duration.Create(durationSeconds);
        if (durationResult.IsFailure)
            return Result<NotificationRequest>.Failure(durationResult.Error);

        // Create request
        var request = new NotificationRequest
        {
            RequestId = Guid.NewGuid(),
            Title = title.Trim(),
            Message = message.Trim(),
            Duration = durationResult.Value,
            Type = type,
            Sound = sound,
            Channel = channel,
            CreatedAt = DateTime.UtcNow
        };

        return Result<NotificationRequest>.Success(request);
    }
}
