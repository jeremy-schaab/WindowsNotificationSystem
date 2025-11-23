using Microsoft.Extensions.DependencyInjection;
using NotifyUser.Application.Services;

namespace NotifyUser.Application;

/// <summary>
/// Dependency injection registration for Application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services with the dependency injection container.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application service as singleton (stateless)
        services.AddSingleton<NotificationApplicationService>();

        return services;
    }
}
