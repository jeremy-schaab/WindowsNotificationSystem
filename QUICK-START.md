# Windows Notification System - Quick Start

## Execute the Workflow

```bash
/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml
```

## What You'll Get

After 45-60 minutes of automated development:

1. **NotifyUser.exe** - Single-file Windows executable at `publish/NotifyUser.exe`
2. **PowerShell Module** - `NotifyUser.psm1` for easy scripting
3. **Full Source Code** - .NET 10 WPF application with tests
4. **Documentation** - README.md with usage examples

## Quick Test After Completion

```powershell
cd C:\Users\jschaab\source\repos\GitHub\WindowsNotificationSystem
.\publish\NotifyUser.exe --title "Test" --message "Hello World!"
```

## PowerShell Integration Examples

### Basic Usage
```powershell
.\NotifyUser.exe --title "Build Complete" --message "Your automation finished"
```

### With Sound and Duration
```powershell
.\NotifyUser.exe --title "Tests Passed" --message "All 47 tests passed" --duration 10 --sound success
```

### Using PowerShell Module
```powershell
Import-Module .\NotifyUser.psm1

Show-Notification -Title "Deployment" -Message "Deploying to production..."
Show-SuccessNotification -Message "Build succeeded!"
Show-ErrorNotification -Message "Build failed - check logs"
```

### Claude Code Automation Example
```powershell
# In your automation script
Write-Host "Building project..."
& "C:\Tools\NotifyUser.exe" --title "Build Started" --message "Compiling..." --type toast

dotnet build

if ($LASTEXITCODE -eq 0) {
    & "C:\Tools\NotifyUser.exe" --title "Success" --message "Build completed" --sound success
} else {
    & "C:\Tools\NotifyUser.exe" --title "Failed" --message "Build errors detected" --sound error
}
```

## Workflow Phases

### Phase 1: Inception (~15 min)
- Clarify Intent (you'll approve)
- Create Units and Bolts
- Validate inception artifacts

### Phase 2: Construction (~30-45 min)
- Generate domain model and logical design
- Create .NET solution with WPF project
- Generate C# code
- Build and test
- Publish single-file EXE
- Create documentation

## Manual Approval Required

You'll be prompted once at the start to approve the Intent:

**Review**:
- Notification types: Toast, Balloon, Desktop Window
- Technology: .NET 10, WPF, System.CommandLine
- Packaging: Single-file self-contained EXE
- PowerShell integration via command-line arguments

**Approve to continue the automated workflow.**

## Technology Stack

- **.NET 10** - Latest .NET with C# 13 features
- **WPF** - Rich Windows UI framework
- **System.CommandLine** - Modern CLI parsing
- **Microsoft.Toolkit.Uwp.Notifications** - Native Windows toast notifications
- **NAudio** - Audio playback support
- **xUnit** - Unit testing framework

## Output Files

```
WindowsNotificationSystem/
├── publish/
│   └── NotifyUser.exe              ← Main executable (50-80 MB)
├── src/NotifyUser/                 ← Source code
│   ├── NotifyUser.csproj
│   ├── Program.cs
│   ├── Services/
│   ├── Models/
│   └── ...
├── tests/NotifyUser.Tests/         ← Unit tests
├── artifacts/
│   ├── inception/                  ← Intent, Units, Bolts
│   ├── construction/               ← Domain Model, Logical Design
│   └── workflows/                  ← This workflow
├── NotifyUser.psm1                 ← PowerShell module
├── README.md                       ← Full documentation
└── WindowsNotificationSystem.sln   ← Visual Studio solution
```

## Next Steps After Workflow Completes

1. **Test the executable**
   ```powershell
   .\publish\NotifyUser.exe --help
   .\publish\NotifyUser.exe --title "Test" --message "It works!"
   ```

2. **Copy to convenient location**
   ```powershell
   Copy-Item .\publish\NotifyUser.exe C:\Tools\
   ```

3. **Add to your PATH** (optional)
   ```powershell
   $env:PATH += ";C:\Tools"
   ```

4. **Import PowerShell module**
   ```powershell
   Import-Module .\NotifyUser.psm1
   Show-Notification -Title "Module Ready" -Message "PowerShell integration loaded"
   ```

5. **Integrate with Claude Code automation**
   - Add notification calls to your PowerShell scripts
   - Notify on build completion, test results, deployment status, etc.

## Troubleshooting

### Workflow fails at build step
- Ensure .NET 10 SDK is installed
- Check Windows SDK for WPF support
- Run `dotnet restore` manually

### Executable doesn't run
- Verify Windows 10/11 x64
- Check antivirus settings
- Try running as administrator

### PowerShell can't execute
- Set execution policy: `Set-ExecutionPolicy RemoteSigned -Scope CurrentUser`
- Use full path to executable
- Quote paths with spaces

## Customization

To modify the workflow before execution, edit:
`artifacts/workflows/windows-notification-workflow.yaml`

Change variables like:
- `ui_framework` - Switch to "winforms" or "console"
- `notification_types` - Add/remove notification types
- `audio_support` - Disable sound if not needed
- `code_coverage_threshold` - Adjust test coverage requirement

## Support

For detailed information, see:
- **Workflow Plan**: `artifacts/workflows/WORKFLOW-PLAN-SUMMARY.md`
- **Full Workflow**: `artifacts/workflows/windows-notification-workflow.yaml`

---

**Ready?** Run the workflow now:

```bash
/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml
```
