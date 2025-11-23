using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Domain.Services;

/// <summary>
/// Service interface for audio playback.
/// </summary>
public interface IAudioService
{
    /// <summary>
    /// Plays a system sound.
    /// </summary>
    Task PlaySoundAsync(
        SoundType soundType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if audio playback is available.
    /// </summary>
    bool IsAvailable();
}
