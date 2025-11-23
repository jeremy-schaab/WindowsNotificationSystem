<#
.SYNOPSIS
    PowerShell module for Windows Notification System

.DESCRIPTION
    Provides PowerShell cmdlets for displaying Windows toast notifications
    using the NotifyUser.exe executable.

.NOTES
    Module Name: NotifyUser
    Author: Windows Notification System
    Version: 1.0.0
#>

# Module-level variables
$script:NotifyUserExePath = $null
$script:DefaultAppName = "PowerShell"

<#
.SYNOPSIS
    Finds the NotifyUser.exe executable.

.DESCRIPTION
    Searches for NotifyUser.exe in the following locations:
    1. Same directory as the module
    2. ../bin directory (package structure)
    3. System PATH

.OUTPUTS
    String path to NotifyUser.exe or $null if not found
#>
function Find-NotifyUserExe {
    [CmdletBinding()]
    param()

    # Check if already cached
    if ($script:NotifyUserExePath -and (Test-Path $script:NotifyUserExePath)) {
        return $script:NotifyUserExePath
    }

    # Search locations
    $searchPaths = @(
        # Same directory as module
        (Join-Path $PSScriptRoot "NotifyUser.exe"),
        # Parent bin directory (package structure)
        (Join-Path (Split-Path -Parent $PSScriptRoot) "bin\NotifyUser.exe"),
        # Two levels up + publish (development structure)
        (Join-Path (Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $PSScriptRoot))) "publish\NotifyUser.exe")
    )

    foreach ($path in $searchPaths) {
        if (Test-Path $path) {
            $script:NotifyUserExePath = $path
            Write-Verbose "Found NotifyUser.exe at: $path"
            return $path
        }
    }

    # Try PATH
    $pathExe = Get-Command NotifyUser.exe -ErrorAction SilentlyContinue
    if ($pathExe) {
        $script:NotifyUserExePath = $pathExe.Source
        Write-Verbose "Found NotifyUser.exe in PATH: $($pathExe.Source)"
        return $pathExe.Source
    }

    Write-Error "NotifyUser.exe not found. Please ensure it's in the module directory, ../bin, or in your PATH."
    return $null
}

<#
.SYNOPSIS
    Displays a Windows toast notification.

.DESCRIPTION
    Shows a native Windows 10/11 toast notification with customizable title,
    message, icon, and sound.

.PARAMETER Title
    The title/header of the notification (required)

.PARAMETER Message
    The main message body of the notification (required)

.PARAMETER AppName
    The application name shown in the notification. Defaults to "PowerShell"

.PARAMETER Icon
    Icon type: info, success, warning, error, or none. Defaults to "info"

.PARAMETER Sound
    Sound to play: default, mail, reminder, sms, alarm, or silent

.PARAMETER Duration
    How long the notification stays on screen: short (5s), long (25s), or sticky (until dismissed)

.PARAMETER Silent
    Suppress sound when displaying the notification

.PARAMETER Buttons
    Array of button labels (max 5)

.PARAMETER Timeout
    Maximum time to wait for user interaction (in seconds)

.EXAMPLE
    Show-Notification -Title "Build Complete" -Message "The build finished successfully"

.EXAMPLE
    Show-Notification -Title "Error" -Message "Build failed" -Icon error -Sound alarm

.EXAMPLE
    Show-Notification -Title "Confirm" -Message "Proceed?" -Buttons @("Yes", "No") -Duration sticky

.OUTPUTS
    Exit code from NotifyUser.exe (0 = success, button index, or error code)
#>
function Show-Notification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [string]$Title,

        [Parameter(Mandatory = $true, Position = 1)]
        [string]$Message,

        [Parameter(Position = 2)]
        [string]$AppName = $script:DefaultAppName,

        [Parameter()]
        [ValidateSet("info", "success", "warning", "error", "none")]
        [string]$Icon = "info",

        [Parameter()]
        [ValidateSet("default", "mail", "reminder", "sms", "alarm", "silent")]
        [string]$Sound = "default",

        [Parameter()]
        [ValidateSet("short", "long", "sticky")]
        [string]$Duration = "short",

        [Parameter()]
        [switch]$Silent,

        [Parameter()]
        [string[]]$Buttons = @(),

        [Parameter()]
        [int]$Timeout = 0
    )

    $exePath = Find-NotifyUserExe
    if (-not $exePath) {
        return 1
    }

    # Build arguments
    $arguments = @(
        "-t", $Title,
        "-m", $Message,
        "--app-name", $AppName,
        "--icon", $Icon,
        "--duration", $Duration
    )

    if ($Silent) {
        $arguments += "--silent"
    } elseif ($Sound -ne "default") {
        $arguments += "--sound", $Sound
    }

    if ($Buttons.Count -gt 0) {
        foreach ($button in $Buttons) {
            $arguments += "--button", $button
        }
    }

    if ($Timeout -gt 0) {
        $arguments += "--timeout", $Timeout
    }

    # Execute
    try {
        $process = Start-Process -FilePath $exePath -ArgumentList $arguments -NoNewWindow -Wait -PassThru
        return $process.ExitCode
    } catch {
        Write-Error "Failed to execute NotifyUser.exe: $_"
        return 1
    }
}

<#
.SYNOPSIS
    Displays a success notification.

.DESCRIPTION
    Convenience function for showing success notifications with a green checkmark icon.

.PARAMETER Title
    The notification title

.PARAMETER Message
    The notification message

.PARAMETER Silent
    Suppress sound

.EXAMPLE
    Show-SuccessNotification -Title "Deployment Complete" -Message "All services are running"
#>
function Show-SuccessNotification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$Message,

        [switch]$Silent
    )

    Show-Notification -Title $Title -Message $Message -Icon success -Sound $(if ($Silent) { "silent" } else { "default" })
}

<#
.SYNOPSIS
    Displays an error notification.

.DESCRIPTION
    Convenience function for showing error notifications with a red X icon and alarm sound.

.PARAMETER Title
    The notification title

.PARAMETER Message
    The notification message

.PARAMETER Silent
    Suppress sound

.EXAMPLE
    Show-ErrorNotification -Title "Deployment Failed" -Message "Check the logs for details"
#>
function Show-ErrorNotification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$Message,

        [switch]$Silent
    )

    Show-Notification -Title $Title -Message $Message -Icon error -Sound $(if ($Silent) { "alarm" } else { "silent" })
}

<#
.SYNOPSIS
    Displays a warning notification.

.DESCRIPTION
    Convenience function for showing warning notifications with a yellow exclamation icon.

.PARAMETER Title
    The notification title

.PARAMETER Message
    The notification message

.PARAMETER Silent
    Suppress sound

.EXAMPLE
    Show-WarningNotification -Title "Low Disk Space" -Message "Only 5GB remaining"
#>
function Show-WarningNotification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$Message,

        [switch]$Silent
    )

    Show-Notification -Title $Title -Message $Message -Icon warning -Sound $(if ($Silent) { "silent" } else { "reminder" })
}

<#
.SYNOPSIS
    Displays an informational notification.

.DESCRIPTION
    Convenience function for showing informational notifications with a blue info icon.

.PARAMETER Title
    The notification title

.PARAMETER Message
    The notification message

.PARAMETER Silent
    Suppress sound

.EXAMPLE
    Show-InfoNotification -Title "Process Started" -Message "Background job is running"
#>
function Show-InfoNotification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$Message,

        [switch]$Silent
    )

    Show-Notification -Title $Title -Message $Message -Icon info -Sound $(if ($Silent) { "silent" } else { "mail" })
}

<#
.SYNOPSIS
    Displays a notification with user confirmation buttons.

.DESCRIPTION
    Shows a notification with Yes/No buttons and waits for user response.

.PARAMETER Title
    The notification title

.PARAMETER Message
    The notification message

.PARAMETER Timeout
    Maximum time to wait for response (seconds). 0 = wait indefinitely

.OUTPUTS
    Boolean: $true if Yes clicked, $false if No clicked or timeout

.EXAMPLE
    $confirmed = Show-ConfirmNotification -Title "Confirm Action" -Message "Delete all files?"
    if ($confirmed) {
        Remove-Item .\* -Recurse
    }
#>
function Show-ConfirmNotification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$Message,

        [int]$Timeout = 0
    )

    $exitCode = Show-Notification -Title $Title -Message $Message -Icon warning -Buttons @("Yes", "No") -Duration sticky -Timeout $Timeout

    # Exit code 0 = dismissed, 1 = Yes, 2 = No, 3+ = error/timeout
    return ($exitCode -eq 1)
}

<#
.SYNOPSIS
    Sets the default application name for notifications.

.DESCRIPTION
    Changes the default app name used when AppName is not specified.

.PARAMETER AppName
    The new default application name

.EXAMPLE
    Set-DefaultAppName -AppName "My Application"
#>
function Set-DefaultAppName {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$AppName
    )

    $script:DefaultAppName = $AppName
    Write-Verbose "Default app name set to: $AppName"
}

<#
.SYNOPSIS
    Gets the path to the NotifyUser.exe executable.

.DESCRIPTION
    Returns the path to NotifyUser.exe if found, otherwise $null.

.EXAMPLE
    $exePath = Get-NotifyUserPath
    if ($exePath) {
        Write-Host "NotifyUser.exe found at: $exePath"
    }
#>
function Get-NotifyUserPath {
    [CmdletBinding()]
    param()

    return Find-NotifyUserExe
}

# Export module members
Export-ModuleMember -Function @(
    "Show-Notification",
    "Show-SuccessNotification",
    "Show-ErrorNotification",
    "Show-WarningNotification",
    "Show-InfoNotification",
    "Show-ConfirmNotification",
    "Set-DefaultAppName",
    "Get-NotifyUserPath"
)
