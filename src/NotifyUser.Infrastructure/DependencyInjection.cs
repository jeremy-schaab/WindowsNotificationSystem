using Microsoft.Extensions.DependencyInjection;
using NotifyUser.Domain.Services;
using NotifyUser.Infrastructure.Services;

namespace NotifyUser.Infrastructure;

/// <summary>
/// Dependency injection registration for Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services with the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register domain service implementation
        services.AddSingleton<INotificationService, NotificationOrchestrationService>();

        // Register infrastructure adapters
        services.AddSingleton<IToastNotificationService, WindowsToastNotificationService>();
        services.AddSingleton<IAudioService, SystemAudioService>();

        return services;
    }
}
