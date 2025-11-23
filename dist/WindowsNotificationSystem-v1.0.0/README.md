# Windows Notification System - Deployment Guide

## Overview

The Windows Notification System provides native Windows 10/11 toast notifications for PowerShell scripts and command-line automation. It consists of a self-contained executable (`NotifyUser.exe`) and a PowerShell module for easy integration.

## System Requirements

- **Operating System**: Windows 10 (version 1809 or later) OR Windows 11
- **Architecture**: x64 (64-bit)
- **PowerShell**: 5.1 or later (for PowerShell module)
- **.NET Runtime**: NOT required (self-contained executable)

## Package Contents

```
WindowsNotificationSystem/
├── bin/
│   └── NotifyUser.exe          # Self-contained executable
├── PowerShell/
│   ├── NotifyUser.psm1         # PowerShell module
│   └── NotifyUser.psd1         # Module manifest
├── README.md                   # This file
├── CHANGELOG.txt               # Version history
├── VERSION.txt                 # Build information
└── CHECKSUMS.txt               # SHA256 checksums
```

## Installation

### Option 1: Add to PATH (Recommended)

1. Extract the package to a permanent location (e.g., `C:\Tools\WindowsNotificationSystem`)
2. Add the `bin` directory to your system PATH:
   ```powershell
   # Add to User PATH (current user only)
   $userPath = [Environment]::GetEnvironmentVariable("Path", "User")
   [Environment]::SetEnvironmentVariable("Path", "$userPath;C:\Tools\WindowsNotificationSystem\bin", "User")
   ```
3. Open a new PowerShell window to use the updated PATH
4. Verify installation:
   ```powershell
   NotifyUser.exe --version
   ```

### Option 2: Use Absolute Path

Simply reference the executable with its full path:
```powershell
C:\Tools\WindowsNotificationSystem\bin\NotifyUser.exe -t "Hello" -m "World"
```

### Option 3: Copy to Existing PATH Location

Copy `NotifyUser.exe` to a directory already in your PATH (e.g., `C:\Windows\System32` or `C:\Windows`)

**Note**: Requires administrator privileges.

## PowerShell Module Setup

### Import Module

```powershell
# Import from package location
Import-Module C:\Tools\WindowsNotificationSystem\PowerShell\NotifyUser.psm1

# Or add to PowerShell profile for automatic loading
$profileDir = Split-Path $PROFILE
if (-not (Test-Path $profileDir)) {
    New-Item -Path $profileDir -ItemType Directory
}
Add-Content -Path $PROFILE -Value "Import-Module C:\Tools\WindowsNotificationSystem\PowerShell\NotifyUser.psm1"
```

### Install to PowerShell Modules Directory

For system-wide availability:

```powershell
# Copy to user modules directory
$modulePath = "$env:USERPROFILE\Documents\WindowsPowerShell\Modules\NotifyUser"
New-Item -Path $modulePath -ItemType Directory -Force
Copy-Item C:\Tools\WindowsNotificationSystem\PowerShell\* -Destination $modulePath -Recurse

# Copy NotifyUser.exe to module directory
Copy-Item C:\Tools\WindowsNotificationSystem\bin\NotifyUser.exe -Destination $modulePath

# Verify installation
Get-Module -ListAvailable NotifyUser

# Import module
Import-Module NotifyUser
```

## Usage

### Command-Line Interface

#### Basic Notification
```cmd
NotifyUser.exe -t "Build Complete" -m "The build finished successfully"
```

#### With Icon and Sound
```cmd
NotifyUser.exe -t "Error" -m "Build failed" --icon error --sound alarm
```

#### Interactive Buttons
```cmd
NotifyUser.exe -t "Confirm" -m "Proceed with deployment?" --button "Yes" --button "No" --duration sticky
```

#### All Options
```cmd
NotifyUser.exe -t "Title" -m "Message" ^
    --app-name "MyApp" ^
    --icon success ^
    --sound reminder ^
    --duration long ^
    --button "OK" ^
    --timeout 30
```

### Command-Line Parameters

| Parameter | Short | Description | Values |
|-----------|-------|-------------|--------|
| `--title` | `-t` | Notification title (required) | Any string |
| `--message` | `-m` | Notification message (required) | Any string |
| `--app-name` | | Application name shown in notification | Any string (default: "NotifyUser") |
| `--icon` | | Icon type | `info`, `success`, `warning`, `error`, `none` (default: `info`) |
| `--sound` | | Sound to play | `default`, `mail`, `reminder`, `sms`, `alarm`, `silent` (default: `default`) |
| `--duration` | | Display duration | `short` (5s), `long` (25s), `sticky` (until dismissed) (default: `short`) |
| `--button` | | Add button (repeatable, max 5) | Button label |
| `--timeout` | | Timeout in seconds for user response | Integer (0 = no timeout) |
| `--silent` | | Suppress sound | Flag |
| `--help` | `-h` | Show help | Flag |
| `--version` | `-v` | Show version | Flag |

### Exit Codes

| Code | Meaning |
|------|---------|
| 0 | Notification dismissed without button click |
| 1-5 | Button index clicked (1 = first button) |
| 10+ | Error occurred |

### PowerShell Module

#### Import Module
```powershell
Import-Module NotifyUser
```

#### Basic Usage
```powershell
# Simple notification
Show-Notification -Title "Build Complete" -Message "All tests passed"

# With icon and sound
Show-Notification -Title "Warning" -Message "Disk space low" -Icon warning -Sound reminder
```

#### Convenience Functions
```powershell
# Success notification (green checkmark)
Show-SuccessNotification -Title "Deployment Complete" -Message "All services running"

# Error notification (red X, alarm sound)
Show-ErrorNotification -Title "Build Failed" -Message "See logs for details"

# Warning notification (yellow exclamation)
Show-WarningNotification -Title "Disk Space" -Message "Only 5GB remaining"

# Info notification (blue info icon)
Show-InfoNotification -Title "Process Started" -Message "Background job running"
```

#### Confirmation Dialogs
```powershell
# Yes/No confirmation
$confirmed = Show-ConfirmNotification -Title "Confirm Action" -Message "Delete all files?"
if ($confirmed) {
    Write-Host "User clicked Yes"
} else {
    Write-Host "User clicked No or dismissed"
}

# With timeout (30 seconds)
$confirmed = Show-ConfirmNotification -Title "Confirm" -Message "Proceed?" -Timeout 30
```

#### Advanced Options
```powershell
# Custom buttons
Show-Notification -Title "Choose Action" -Message "Select an option" `
    -Buttons @("Option 1", "Option 2", "Cancel") `
    -Duration sticky `
    -Icon info

# Set default app name for all notifications
Set-DefaultAppName -AppName "My Application"
Show-Notification -Title "Hello" -Message "Now shows 'My Application'"

# Get path to executable
$exePath = Get-NotifyUserPath
Write-Host "NotifyUser.exe is at: $exePath"
```

## Integration Examples

### Build Script
```powershell
# build.ps1
Import-Module NotifyUser

try {
    dotnet build MySolution.sln
    if ($LASTEXITCODE -eq 0) {
        Show-SuccessNotification -Title "Build Complete" -Message "Build succeeded"
    } else {
        Show-ErrorNotification -Title "Build Failed" -Message "Exit code: $LASTEXITCODE"
    }
} catch {
    Show-ErrorNotification -Title "Build Error" -Message $_.Exception.Message
}
```

### Deployment Script
```powershell
# deploy.ps1
Import-Module NotifyUser

$confirmed = Show-ConfirmNotification -Title "Confirm Deployment" -Message "Deploy to production?"
if (-not $confirmed) {
    Write-Host "Deployment cancelled"
    exit 1
}

# Deploy...
Show-InfoNotification -Title "Deploying" -Message "Deployment in progress..." -Silent

# After deployment
Show-SuccessNotification -Title "Deployed" -Message "Application deployed successfully"
```

### Task Scheduler
```powershell
# scheduled-task.ps1
Import-Module NotifyUser

# Run backup
$result = Backup-MyData

if ($result.Success) {
    Show-SuccessNotification -Title "Backup Complete" -Message "Backed up $($result.FileCount) files"
} else {
    Show-ErrorNotification -Title "Backup Failed" -Message $result.ErrorMessage
}
```

### Long-Running Process
```cmd
REM long-running-task.bat
@echo off
echo Starting long task...
timeout /t 300 /nobreak

REM Notify when complete
NotifyUser.exe -t "Task Complete" -m "The long-running task has finished" --sound alarm
```

## Troubleshooting

### Notifications Not Appearing

1. **Check Windows notification settings**:
   - Open Settings > System > Notifications
   - Ensure notifications are enabled
   - Check "Do Not Disturb" mode is off

2. **Verify executable permissions**:
   ```powershell
   # Check if file is blocked
   Get-Item NotifyUser.exe | Unblock-File
   ```

3. **Test with simple notification**:
   ```powershell
   NotifyUser.exe -t "Test" -m "Testing" --silent
   ```

### PowerShell Module Not Loading

1. **Check execution policy**:
   ```powershell
   Get-ExecutionPolicy
   # If Restricted, change to RemoteSigned
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

2. **Verify module path**:
   ```powershell
   Get-Module -ListAvailable NotifyUser
   ```

3. **Import with absolute path**:
   ```powershell
   Import-Module C:\Full\Path\To\NotifyUser.psm1 -Force
   ```

### Executable Not Found

1. **Verify PATH**:
   ```powershell
   $env:Path -split ';' | Where-Object { $_ -match 'NotifyUser' }
   ```

2. **Use absolute path**:
   ```powershell
   & "C:\Tools\WindowsNotificationSystem\bin\NotifyUser.exe" --version
   ```

### Access Denied Errors

- Run PowerShell as Administrator
- Check antivirus/Windows Defender exclusions
- Verify file permissions on executable

### Slow Startup Time

- First run may be slower due to Windows security scanning
- Subsequent runs should be faster (target: < 100ms)
- Add to antivirus exclusions if performance is poor

## Performance Considerations

- **Startup Time**: < 100ms for subsequent executions
- **Memory Usage**: ~5-10 MB during execution
- **EXE Size**: ~20-30 MB (self-contained with .NET runtime)

## Security Notes

- The executable is digitally signed (if applicable)
- Verify checksums against `CHECKSUMS.txt` before deployment
- No network connectivity required
- No telemetry or data collection
- All processing is local to the machine

## Uninstallation

1. Remove from PATH (if added):
   ```powershell
   # Remove from User PATH
   $userPath = [Environment]::GetEnvironmentVariable("Path", "User")
   $newPath = ($userPath -split ';' | Where-Object { $_ -notmatch 'WindowsNotificationSystem' }) -join ';'
   [Environment]::SetEnvironmentVariable("Path", $newPath, "User")
   ```

2. Remove PowerShell module (if installed):
   ```powershell
   Remove-Module NotifyUser -ErrorAction SilentlyContinue
   Remove-Item "$env:USERPROFILE\Documents\WindowsPowerShell\Modules\NotifyUser" -Recurse -Force
   ```

3. Delete installation directory:
   ```powershell
   Remove-Item C:\Tools\WindowsNotificationSystem -Recurse -Force
   ```

## Support and Feedback

For issues, feature requests, or contributions:
- GitHub Issues: [Your Repository URL]
- Email: [Your Support Email]

## Version History

See `CHANGELOG.txt` for detailed version history.

## License

[Your License Here]

---

**Last Updated**: 2025-11-23
**Version**: 1.0.0
