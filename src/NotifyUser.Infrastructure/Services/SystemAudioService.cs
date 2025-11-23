using System.Media;
using Microsoft.Extensions.Logging;
using NotifyUser.Domain.Services;
using NotifyUser.Domain.ValueObjects;

namespace NotifyUser.Infrastructure.Services;

/// <summary>
/// Infrastructure adapter for system audio playback using System.Media.
/// </summary>
public sealed class SystemAudioService : IAudioService
{
    private readonly ILogger<SystemAudioService> _logger;

    public SystemAudioService(ILogger<SystemAudioService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool IsAvailable()
    {
        // System sounds are available on Windows platforms
        return OperatingSystem.IsWindows();
    }

    public async Task PlaySoundAsync(
        SoundType soundType,
        CancellationToken cancellationToken = default)
    {
        if (soundType == SoundType.None)
        {
            return; // No sound requested
        }

        try
        {
            // Map SoundType to SystemSound
            var systemSound = MapToSystemSound(soundType);

            if (systemSound != null)
            {
                // Play system sound (fire-and-forget, non-blocking)
                await Task.Run(() => systemSound.Play(), cancellationToken);

                _logger.LogDebug("Played system sound: {SoundType}", soundType);
            }
            else
            {
                _logger.LogWarning("Unknown sound type: {SoundType}", soundType);
            }
        }
        catch (Exception ex)
        {
            // Audio failures are non-critical - log and continue
            _logger.LogWarning(
                ex,
                "Failed to play system sound: {SoundType}",
                soundType);
        }
    }

    private static SystemSound? MapToSystemSound(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.Default => SystemSounds.Asterisk,
            SoundType.Success => SystemSounds.Asterisk,
            SoundType.Error => SystemSounds.Hand,
            SoundType.Warning => SystemSounds.Exclamation,
            SoundType.Info => SystemSounds.Asterisk,
            SoundType.None => null,
            _ => null
        };
    }
}
