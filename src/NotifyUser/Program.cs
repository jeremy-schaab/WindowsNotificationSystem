using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyUser.Application;
using NotifyUser.Application.Services;
using NotifyUser.Domain.ValueObjects;
using NotifyUser.Infrastructure;

namespace NotifyUser;

/// <summary>
/// Main entry point for NotifyUser CLI application.
/// </summary>
internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Build dependency injection container
        var host = CreateHostBuilder(args).Build();
        var serviceProvider = host.Services;

        // Get application service
        var notificationService = serviceProvider.GetRequiredService<NotificationApplicationService>();
        var logger = serviceProvider.GetRequiredService<ILogger<NotificationApplicationService>>();

        // Parse command-line arguments
        var rootCommand = BuildRootCommand(notificationService, logger);

        // Execute command
        return await rootCommand.InvokeAsync(args);
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register application and infrastructure services
                services.AddApplicationServices();
                services.AddInfrastructureServices();
            })
            .ConfigureLogging(logging =>
            {
                // Configure logging (minimal console output for CLI)
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            });
    }

    private static RootCommand BuildRootCommand(
        NotificationApplicationService notificationService,
        ILogger logger)
    {
        // Define command-line options
        var titleOption = new Option<string>(
            aliases: new[] { "--title", "-t" },
            description: "Notification title (required, max 100 characters)")
        {
            IsRequired = true
        };

        var messageOption = new Option<string>(
            aliases: new[] { "--message", "-m" },
            description: "Notification message (required, max 500 characters)")
        {
            IsRequired = true
        };

        var durationOption = new Option<int>(
            aliases: new[] { "--duration", "-d" },
            getDefaultValue: () => 5,
            description: "Display duration in seconds (1-60, default 5)");

        var typeOption = new Option<NotificationType>(
            aliases: new[] { "--type" },
            getDefaultValue: () => NotificationType.Info,
            description: "Notification type: Info, Success, Warning, Error");

        var soundOption = new Option<SoundType>(
            aliases: new[] { "--sound", "-s" },
            getDefaultValue: () => SoundType.Default,
            description: "Sound type: None, Default, Success, Error, Warning, Info");

        var channelOption = new Option<DeliveryChannel>(
            aliases: new[] { "--channel", "-c" },
            getDefaultValue: () => DeliveryChannel.Toast,
            description: "Delivery channel: Toast, Window, Balloon");

        // Build root command
        var rootCommand = new RootCommand("Display Windows notifications from PowerShell scripts")
        {
            titleOption,
            messageOption,
            durationOption,
            typeOption,
            soundOption,
            channelOption
        };

        // Set command handler
        rootCommand.SetHandler(async (context) =>
        {
            var title = context.ParseResult.GetValueForOption(titleOption)!;
            var message = context.ParseResult.GetValueForOption(messageOption)!;
            var duration = context.ParseResult.GetValueForOption(durationOption);
            var type = context.ParseResult.GetValueForOption(typeOption);
            var sound = context.ParseResult.GetValueForOption(soundOption);
            var channel = context.ParseResult.GetValueForOption(channelOption);

            // Display notification
            var result = await notificationService.DisplayNotificationAsync(
                title: title,
                message: message,
                durationSeconds: duration,
                type: type,
                sound: sound,
                channel: channel,
                cancellationToken: context.GetCancellationToken());

            // Set exit code based on result
            context.ExitCode = (int)result.ExitCode;

            // Output error message if failed
            if (!result.IsSuccess)
            {
                Console.Error.WriteLine($"Error: {result.ErrorMessage}");
            }
        });

        return rootCommand;
    }
}
