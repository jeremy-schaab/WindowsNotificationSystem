# Windows Notification System

A lightweight, self-contained Windows notification utility designed for PowerShell automation and script integration. Display native Windows toast notifications with customizable messages, durations, and sound effects.

## Features

- **Native Windows Notifications**: Uses Windows 10/11 toast notification system
- **PowerShell Integration**: Call from PowerShell scripts with simple command-line interface
- **Self-Contained Executable**: Single-file deployment with no dependencies
- **Customizable Duration**: Control notification display time (1-30 seconds)
- **Sound Support**: Built-in notification sounds (success, error, warning, info, silent)
- **Action Center Integration**: Notifications persist in Windows Action Center
- **Lightweight**: Optimized for fast startup and minimal memory footprint

## Quick Start

### Basic Usage

```powershell
# Simple notification
NotifyUser.exe --title "Build Complete" --message "Your build finished successfully"

# With duration and sound
NotifyUser.exe -t "Deployment" -m "Application deployed to production" -d 10 -s success

# Error notification
NotifyUser.exe -t "Error" -m "Build failed with 3 errors" -s error
```

### PowerShell Module

```powershell
# Import module
Import-Module .\NotifyUser.psm1

# Use helper cmdlets
Show-SuccessNotification -Title "Test Passed" -Message "All 47 tests passed"
Show-ErrorNotification -Title "Backup Failed" -Message "Unable to connect to backup server"
Show-WarningNotification -Title "Disk Space" -Message "Less than 10% free space remaining"
```

## Installation

### Download Pre-Built Executable

1. Download `NotifyUser.exe` from the [Releases](https://github.com/jeremy-schaab/WindowsNotificationSystem/releases) page
2. Place in a directory (e.g., `C:\Tools\NotifyUser\`)
3. Add directory to PATH or call with full path

### Build from Source

```powershell
# Clone repository
git clone https://github.com/jeremy-schaab/WindowsNotificationSystem.git
cd WindowsNotificationSystem

# Build and publish
dotnet publish src/NotifyUser/NotifyUser.csproj -c Release -r win-x64 --self-contained

# Output: publish/NotifyUser.exe
```

## Command-Line Options

| Option | Short | Description | Default |
|--------|-------|-------------|---------|
| `--title` | `-t` | Notification title (required) | - |
| `--message` | `-m` | Notification message (required) | - |
| `--duration` | `-d` | Display duration in seconds (1-30) | 5 |
| `--sound` | `-s` | Sound type: `success`, `error`, `warning`, `info`, `none` | `none` |
| `--help` | `-h` | Display help information | - |

## PowerShell Module Cmdlets

| Cmdlet | Description |
|--------|-------------|
| `Show-Notification` | General notification with full parameter control |
| `Show-SuccessNotification` | Green notification with success sound |
| `Show-ErrorNotification` | Red notification with error sound |
| `Show-WarningNotification` | Yellow notification with warning sound |
| `Show-InfoNotification` | Blue notification with info sound |
| `Show-ConfirmNotification` | Confirmation dialog with Yes/No buttons |
| `Set-DefaultAppName` | Configure default application name |
| `Get-NotifyUserPath` | Get path to NotifyUser.exe |

## Examples

### Build Automation

```powershell
# In your build script
dotnet build MyProject.sln
if ($LASTEXITCODE -eq 0) {
    NotifyUser.exe -t "Build Success" -m "MyProject built successfully" -s success
} else {
    NotifyUser.exe -t "Build Failed" -m "Build failed with errors" -s error
}
```

### Long-Running Tasks

```powershell
# Start deployment
Write-Host "Deploying application..."
Start-Sleep -Seconds 60

# Notify when complete
NotifyUser.exe -t "Deployment Complete" -m "Application deployed to staging" -d 10
```

### Test Results

```powershell
# Run tests
$testResults = dotnet test --no-build
$exitCode = $LASTEXITCODE

if ($exitCode -eq 0) {
    Show-SuccessNotification -Title "Tests Passed" -Message "All tests completed successfully"
} else {
    Show-ErrorNotification -Title "Tests Failed" -Message "Some tests failed - check output"
}
```

### Scheduled Tasks

```powershell
# Daily backup notification
$backupResult = Invoke-BackupScript
if ($backupResult.Success) {
    Show-SuccessNotification -Title "Backup Complete" -Message "Daily backup finished at $(Get-Date)"
}
```

## Architecture

This project follows **Domain-Driven Design (DDD)** with **Clean Architecture** principles:

### Project Structure

```
WindowsNotificationSystem/
├── src/
│   ├── NotifyUser/                    # Console application (Presentation)
│   ├── NotifyUser.Domain/             # Domain layer (core business logic)
│   ├── NotifyUser.Application/        # Application layer (use cases)
│   └── NotifyUser.Infrastructure/     # Infrastructure (Windows APIs, audio)
├── tests/
│   ├── NotifyUser.Domain.Tests/       # Domain unit tests
│   ├── NotifyUser.Application.Tests/  # Application unit tests
│   └── NotifyUser.Integration.Tests/  # Integration tests
├── deployment/                         # Deployment scripts and packages
├── artifacts/                          # AI-DLC artifacts (intent, units, bolts)
└── docs/                              # Documentation
```

### Technology Stack

- **.NET 10** with C# 13
- **System.CommandLine** for CLI parsing
- **Microsoft.Toolkit.Uwp.Notifications** for Windows toast notifications
- **System.Media.SoundPlayer** for audio playback
- **xUnit**, **Moq**, **FluentAssertions** for testing

## Development

### Prerequisites

- .NET 10 SDK
- Windows 10 version 1809+ or Windows 11
- Visual Studio 2022 or VS Code

### Build

```powershell
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Publish single-file executable
dotnet publish src/NotifyUser/NotifyUser.csproj -c Release -r win-x64 --self-contained -o publish
```

### Testing

```powershell
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=cobertura

# Run specific test project
dotnet test tests/NotifyUser.Domain.Tests
```

## Documentation

- **[Quick Start Guide](QUICK-START.md)**: Complete workflow execution guide
- **[Solution Documentation](docs/reference/solution/README.md)**: Comprehensive API documentation
- **[AI-DLC Artifacts](artifacts/)**: Intent, Units, Bolts, and workflow definitions

### Querying the Codebase

This repository includes AI-optimized documentation for fast codebase exploration:

```bash
/docs:ask-codebase "Which project contains INotificationService?"
/docs:ask-codebase "What models are in NotifyUser.Domain?"
/docs:ask-codebase "Show me all dependencies of NotifyUser.Infrastructure"
```

## Performance

- **Startup Time**: < 100ms on modern hardware
- **Memory Usage**: < 50 MB peak
- **Executable Size**: ~58 MB (self-contained with .NET runtime)
- **Notification Display**: < 100ms latency

## Compatibility

- **Operating Systems**: Windows 10 (1809+), Windows 11
- **PowerShell**: PowerShell 5.1, PowerShell 7+
- **.NET Runtime**: Self-contained (no installation required)

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Support

- **Issues**: [GitHub Issues](https://github.com/jeremy-schaab/WindowsNotificationSystem/issues)
- **Documentation**: [Project Wiki](https://github.com/jeremy-schaab/WindowsNotificationSystem/wiki)

## Roadmap

- [ ] WPF-based custom notification windows with button support
- [ ] Configuration file support for default settings
- [ ] Notification templates for common scenarios
- [ ] System tray integration for persistent notifications
- [ ] Cross-platform support (Windows, macOS, Linux)

## Acknowledgments

- Built using [AI-DLC (AI-Driven Development Lifecycle)](https://ai-dlc.dev) methodology
- Uses [Microsoft.Toolkit.Uwp.Notifications](https://github.com/CommunityToolkit/WindowsCommunityToolkit) for Windows toast APIs
- Inspired by the need for better PowerShell automation feedback

---

**Made with ❤️ for PowerShell automation enthusiasts**
