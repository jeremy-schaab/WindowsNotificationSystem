# Workflow Plan: Windows Notification System

**Workflow ID**: windows-notification-system
**Workflow File**: `artifacts/workflows/windows-notification-workflow.yaml`
**Intent**: Windows notification system that Claude Code can trigger via PowerShell
**Type**: Greenfield Utility Tool
**Created**: 2025-11-22
**Status**: Ready for Execution

---

## Executive Summary

This workflow creates a complete Windows desktop notification utility that Claude Code can invoke via PowerShell during long-running automation tasks. The result is a single-file, self-contained EXE that displays toast notifications, balloon tips, or desktop windows with configurable messages, titles, durations, and sounds.

### Key Deliverables

1. **NotifyUser.exe** - Single-file Windows executable (no dependencies)
2. **PowerShell Module** - Wrapper functions for easy integration
3. **Documentation** - README with usage examples and integration guide
4. **Source Code** - Full .NET 10 WPF application with tests
5. **Artifacts** - Intent, Units, Bolts, Domain Model, Logical Design

---

## Workflow Overview

| Metric | Value |
|--------|-------|
| **Total Phases** | 2 (Inception + Construction) |
| **Total Steps** | 29 |
| **Approval Gates** | 4 (1 manual, 3 automatic) |
| **Estimated Duration** | 45-60 minutes |
| **Complexity** | Medium |
| **Technology Stack** | .NET 10, C# 13, WPF, System.CommandLine |
| **Target Platform** | Windows 10/11 x64 |

---

## Phase Breakdown

### Phase 1: Inception (7 steps, ~15 minutes)

**Purpose**: Clarify requirements and plan development approach

#### Steps

1. **Clarify Intent** (approval required)
   - Create Intent document with detailed requirements
   - Define notification types, PowerShell integration, packaging strategy
   - Technology decision: WPF for rich UI, single-file EXE for distribution

2. **Decompose into Units**
   - Break down into 4 units: NotificationCore, CLI, NotificationUI, AudioSupport
   - Define unit boundaries and responsibilities

3. **Plan Core Notification Bolt**
   - Bolt plan for Windows notification API integration
   - Toast notifications and balloon tips

4. **Plan CLI Bolt**
   - Bolt plan for command-line argument parsing
   - PowerShell integration and exit codes

5. **Validate Inception Phase**
   - Verify all artifacts created and complete
   - Gate: Must pass before proceeding to Construction

**Artifacts Created**:
- `artifacts/inception/intent-windows-notification.md`
- `artifacts/inception/units-windows-notification.md`
- `artifacts/inception/bolt-notification-core-plan.md`
- `artifacts/inception/bolt-cli-plan.md`

---

### Phase 2: Construction (22 steps, ~30-45 minutes)

**Purpose**: Build, test, and package the notification system

#### Domain Modeling & Design (2 steps, ~5 minutes)

1. **Create Domain Model**
   - Simple object model for notifications
   - Bounded contexts: Notification, CLI, Audio
   - Output: `artifacts/construction/domain-model-notification-system.md`

2. **Create Logical Design**
   - Component architecture with Factory, Strategy, Command patterns
   - Technology decisions: WPF, System.CommandLine, Windows.UI.Notifications
   - Output: `artifacts/construction/logical-design-notification-system.md`

#### Project Setup (8 steps, ~10 minutes)

3. **Create .NET Solution**
   - Initialize WindowsNotificationSystem.sln

4. **Create WPF Application Project**
   - Main NotifyUser project targeting .NET 10

5. **Add Project to Solution**

6. **Create Test Project**
   - xUnit test project for unit tests

7. **Add Test Project to Solution**

8. **Add Project Reference**
   - Link test project to main project

9. **Install NuGet Packages**
   - System.CommandLine (CLI parsing)
   - Microsoft.Toolkit.Uwp.Notifications (Toast notifications)
   - NAudio (Audio playback)

10. **Install Test Dependencies**
    - FluentAssertions, Moq, coverlet.collector

#### Code Generation & Testing (7 steps, ~15-20 minutes)

11. **Generate Application Code** (with retry and fallback)
    - C# 13 code for notification system
    - Command-line parsing
    - Toast notifications, WPF windows, audio playback
    - Exit codes for PowerShell integration

12. **Generate Unit Tests**
    - Tests for NotificationService, CommandLineParser, NotificationFactory
    - Target: 70% code coverage

13. **Build Solution** (approval gate: automatic)
    - Compile in Release configuration
    - Must succeed or workflow rolls back

14. **Run Unit Tests** (approval gate: automatic)
    - Execute xUnit tests with code coverage
    - Must pass or workflow fails

15. **Verify Code Coverage**
    - Check 70% threshold met
    - Continue on failure (warning only)

16. **Publish Single-File EXE** (approval gate: automatic)
    - Self-contained Windows x64 executable
    - Single file with embedded runtime
    - Compressed for smaller size
    - Output: `publish/NotifyUser.exe`

17. **Test PowerShell Integration**
    - Verify --help works
    - Test basic notification
    - Test toast with duration

#### Documentation & Wrappers (3 steps, ~5 minutes)

18. **Validate Construction Phase**
    - Verify all artifacts and outputs present
    - Gate: Must pass to complete workflow

19. **Generate Usage Documentation**
    - README.md with overview, installation, usage examples
    - PowerShell integration guide
    - Command-line reference
    - Troubleshooting section

20. **Create PowerShell Wrapper Functions**
    - NotifyUser.psm1 module
    - Functions: Show-Notification, Show-ToastNotification, Show-SuccessNotification, Show-ErrorNotification

**Outputs**:
- `publish/NotifyUser.exe` (single-file executable)
- `NotifyUser.psm1` (PowerShell module)
- `README.md` (user documentation)
- `WindowsNotificationSystem.sln` (full source)
- Test results and coverage reports

---

## Approval Gates

### 1. Manual Approval: Intent Clarification (Step 1)

**Type**: Manual
**Timeout**: 60 minutes
**Required**: Yes

**Review Points**:
- Intent clearly defines notification types (toast, balloon, window)
- PowerShell integration approach approved
- Technology stack acceptable (WPF, .NET 10, single-file EXE)
- Packaging strategy (self-contained, no dependencies)

**On Timeout**: Workflow pauses, awaits manual restart

---

### 2. Automatic Approval: Build Success (Step 13)

**Type**: Automatic
**Condition**: Exit code == 0

**Validation**:
- Solution compiles without errors
- All projects build successfully

**On Failure**: Rollback workflow, delete generated files

---

### 3. Automatic Approval: Tests Pass (Step 14)

**Type**: Automatic
**Condition**: Exit code == 0 AND coverage >= 70%

**Validation**:
- All unit tests pass
- Code coverage meets threshold

**On Failure**: Workflow fails, no rollback (preserve for debugging)

---

### 4. Automatic Approval: Executable Created (Step 16)

**Type**: Automatic
**Condition**: File exists at publish/NotifyUser.exe

**Validation**:
- Executable published successfully
- File size reasonable (< 100 MB)

**On Failure**: Workflow fails

---

## Dependency Graph

```
Inception Phase:
start-intent (manual approval)
    |
    v
create-units
    |
    +-- plan-bolt-core
    |
    +-- plan-bolt-cli
    |
    v
validate-inception (gate)

Construction Phase:
validate-inception
    |
    v
domain-model
    |
    v
logical-design
    |
    +-- create-solution
    |       |
    |       +-- create-wpf-project --> add-project-to-solution --> install-nuget-packages
    |       |                                                              |
    |       +-- create-test-project --> add-test-project-to-solution       |
    |               |                                                      |
    |               v                                                      v
    |           add-project-reference --> install-test-packages --> generate-code
    |                                                                      |
    |                                                                      v
    |                                                              build-unit-tests
    |                                                                      |
    v                                                                      |
[logical-design] --------------------------------------------------------+
                                                                           |
                                                                           v
                                                                   build-solution (gate)
                                                                           |
                                                                           v
                                                                    run-unit-tests (gate)
                                                                           |
                                                                           v
                                                                  verify-code-coverage
                                                                           |
                                                                           v
                                                                 publish-executable (gate)
                                                                           |
                                                                           v
                                                              test-powershell-integration
                                                                           |
                                                                           v
                                                                 validate-construction
                                                                           |
                                                    +----------------------+----------------------+
                                                    |                                             |
                                                    v                                             v
                                        generate-usage-documentation            create-powershell-wrapper
```

---

## Error Handling Strategy

### Global Retry Strategy

- **Max Attempts**: 2
- **Backoff**: Linear (1.5x multiplier)
- **Continue on Warning**: Yes
- **Fail on Test Failure**: Yes

### Step-Specific Overrides

#### install-nuget-packages
- **Max Attempts**: 3
- **Backoff**: Exponential
- **Fallback**: Log warning, continue (packages may need manual installation)

#### generate-code
- **Max Attempts**: 2
- **Backoff**: Exponential
- **Fallback**: Use basic console app template if AI generation fails

### Rollback Strategy

**On Build Failure**:
- Delete generated code files
- Preserve artifacts (Intent, Units, etc.)
- Do NOT clear NuGet cache
- Allow user to fix and retry

**On Test Failure**:
- Preserve everything for debugging
- No automatic rollback
- Workflow stops for investigation

---

## Technology Recommendations

### Primary UI Framework: WPF

**Rationale**:
- Modern XAML-based UI with rich styling capabilities
- Native Windows 10/11 notification support via Windows.UI.Notifications
- Easy integration with Microsoft.Toolkit.Uwp.Notifications
- Better for toast and custom window notifications than WinForms
- Supports modern C# 13 features and MVVM patterns

**Alternatives Considered**:

1. **WinForms**
   - Pros: Simpler, lightweight, faster development
   - Cons: Legacy framework, limited styling, poor native toast support
   - Verdict: Not recommended for modern notifications

2. **Pure PowerShell Script**
   - Pros: No compilation, easy to modify
   - Cons: Limited UI capabilities, slower, less reliable
   - Verdict: Insufficient for rich notification features

3. **Console App with P/Invoke**
   - Pros: Small size, fast startup
   - Cons: Complex native API calls, more maintenance
   - Verdict: WPF with Toolkit is easier and more maintainable

---

### CLI Framework: System.CommandLine

**Rationale**:
- Modern .NET command-line parsing library
- Automatic help generation (--help)
- Type-safe argument binding
- Excellent PowerShell integration
- Built-in validation and error handling

**Example Usage**:
```powershell
.\NotifyUser.exe --title "Build Complete" --message "Success!" --type toast --duration 5 --sound success
```

---

### Packaging: Single-File Self-Contained EXE

**Rationale**:
- No .NET runtime dependency (works on any Windows 10/11)
- Easy to distribute (single file copy)
- Simple PowerShell integration (direct execution)
- Compressed for smaller size
- Professional deployment experience

**Publish Settings**:
```xml
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>true</SelfContained>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
<EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
```

**Expected Size**: ~50-80 MB (includes .NET 10 runtime)

---

## Usage Examples

### Basic Toast Notification
```powershell
.\NotifyUser.exe --title "Task Complete" --message "Your automation finished successfully"
```

### Toast with Custom Duration and Sound
```powershell
.\NotifyUser.exe --title "Build Status" --message "Build completed in 45 seconds" --duration 10 --sound success
```

### Desktop Window Notification
```powershell
.\NotifyUser.exe --type window --title "Review Required" --message "Please review the pull request"
```

### From PowerShell Module
```powershell
Import-Module .\NotifyUser.psm1

Show-Notification -Title "Deployment" -Message "Deploying to production..." -Type toast
Show-SuccessNotification -Message "All tests passed!"
Show-ErrorNotification -Message "Build failed - check logs"
```

### Integration with Claude Code Automation
```powershell
# In your automation script
Write-Host "Starting build..."
& "C:\Tools\NotifyUser.exe" --title "Build Started" --message "Building project..." --type toast

# Long-running operation
dotnet build

# Notify on completion
if ($LASTEXITCODE -eq 0) {
    & "C:\Tools\NotifyUser.exe" --title "Success" --message "Build completed" --sound success
} else {
    & "C:\Tools\NotifyUser.exe" --title "Build Failed" --message "Check console for errors" --sound error
}
```

---

## Execution Instructions

### Run the Workflow

```bash
/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml
```

### What Happens Next

1. **Inception Phase Starts**
   - Intent Clarification agent creates Intent document
   - You'll be prompted to approve the Intent (manual gate)
   - Units and Bolts are planned automatically

2. **Construction Phase Begins**
   - Domain Model and Logical Design generated
   - .NET solution and projects created
   - NuGet packages installed
   - Code generation (with fallback if needed)
   - Tests generated and executed
   - Single-file EXE published

3. **Documentation Created**
   - README.md with usage guide
   - PowerShell module with wrapper functions

4. **Workflow Complete**
   - Executable ready at: `publish/NotifyUser.exe`
   - Test it: `.\publish\NotifyUser.exe --help`

---

## Post-Workflow Next Steps

### 1. Test the Executable

```powershell
cd C:\Users\jschaab\source\repos\GitHub\WindowsNotificationSystem
.\publish\NotifyUser.exe --help
.\publish\NotifyUser.exe --title "Test" --message "Hello World"
```

### 2. Copy to Convenient Location

```powershell
# Add to PATH
Copy-Item .\publish\NotifyUser.exe C:\Tools\NotifyUser.exe

# Or add to project
Copy-Item .\publish\NotifyUser.exe C:\YourProject\tools\
```

### 3. Import PowerShell Module

```powershell
Import-Module .\NotifyUser.psm1

# Test wrapper functions
Show-Notification -Title "Test" -Message "Module loaded successfully"
```

### 4. Integrate with Claude Code

Add notification calls to your automation scripts:

```powershell
# Example: CI/CD script
.\build.ps1
if ($LASTEXITCODE -eq 0) {
    Show-SuccessNotification "Build complete"
} else {
    Show-ErrorNotification "Build failed"
}
```

### 5. Optional Enhancements

Consider adding these features later:

- **Custom Icons**: Show different icons for success/error/warning
- **Progress Bars**: Display progress during long operations
- **Interactive Buttons**: Add "View Logs" or "Retry" buttons to notifications
- **Notification History**: Log all notifications to a file
- **Configuration File**: Allow default settings in a config file
- **Dark Mode**: Respect Windows theme for notification styling

---

## Risk Mitigation

### Code Generation Failure
- **Risk**: AI code generation may fail or produce incomplete code
- **Mitigation**: Fallback to basic console app template, 2 retry attempts with exponential backoff
- **Recovery**: Manual code review and completion if needed

### NuGet Package Installation Issues
- **Risk**: Network issues or package conflicts
- **Mitigation**: 3 retry attempts with exponential backoff
- **Recovery**: Fallback logs warning, allows manual installation

### Test Failures
- **Risk**: Generated code may not pass all tests
- **Mitigation**: 70% coverage threshold (not 100%), preserves files for debugging
- **Recovery**: Workflow stops, user can fix code and re-run tests manually

### Build Failures
- **Risk**: Code may not compile
- **Mitigation**: Automatic rollback, preserves artifacts
- **Recovery**: Review compilation errors, fix code, re-run workflow from Construction phase

---

## Workflow Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `project_name` | WindowsNotificationSystem | Main project name |
| `dotnet_version` | 9.0 | Target .NET version |
| `ui_framework` | wpf | UI framework (wpf, winforms, console) |
| `notification_types` | [toast, balloon, window] | Notification types to support |
| `audio_support` | true | Enable sound notifications |
| `duration_configurable` | true | Allow custom display duration |
| `cli_arguments` | [--message, --title, etc.] | Command-line arguments |
| `single_file_executable` | true | Publish as single EXE |
| `self_contained` | true | Include .NET runtime |
| `test_framework` | xunit | Unit testing framework |
| `code_coverage_threshold` | 70 | Minimum coverage percentage |

### Customization

To customize the workflow, edit `artifacts/workflows/windows-notification-workflow.yaml` and modify variables in the `variables:` section.

---

## Success Criteria

The workflow succeeds when:

1. All Inception artifacts created and validated
2. .NET solution builds without errors
3. All unit tests pass
4. Code coverage meets 70% threshold
5. Single-file EXE published successfully
6. PowerShell integration tests pass
7. Documentation generated
8. PowerShell module created

**Final Deliverable**: Self-contained Windows executable that displays notifications when called from PowerShell, ready for integration with Claude Code automation workflows.

---

## Support & Troubleshooting

### Build Fails

**Check**:
- .NET 10 SDK installed
- Windows SDK installed (for WPF)
- NuGet packages restored correctly

**Fix**:
```powershell
dotnet restore
dotnet build --verbosity detailed
```

### Tests Fail

**Check**:
- Test project references main project
- All test dependencies installed
- Code coverage tools present

**Fix**:
```powershell
dotnet test --verbosity detailed
```

### Executable Doesn't Run

**Check**:
- Windows 10/11 x64 platform
- Antivirus not blocking
- File not corrupted

**Fix**:
```powershell
# Try running in verbose mode
.\NotifyUser.exe --help

# Check file size (should be 50-80 MB)
Get-Item .\NotifyUser.exe | Select-Object Length
```

### PowerShell Integration Issues

**Check**:
- Execution policy allows scripts
- Path to executable is correct
- Arguments properly quoted

**Fix**:
```powershell
# Set execution policy
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Test with full path
& "C:\full\path\to\NotifyUser.exe" --title "Test" --message "Hello"
```

---

## Workflow Metrics

| Metric | Value |
|--------|-------|
| Total Steps | 29 |
| Inception Steps | 7 |
| Construction Steps | 22 |
| Manual Approvals | 1 |
| Automatic Gates | 3 |
| Estimated Duration | 45-60 minutes |
| Complexity Score | 6/10 (Medium) |
| Retry-Enabled Steps | 4 |
| Fallback-Enabled Steps | 2 |
| Artifacts Generated | 6 |
| Code Files Generated | 15-20 (estimated) |
| Test Files Generated | 5-10 (estimated) |
| Documentation Files | 2 |

---

## Version History

- **1.0.0** (2025-11-22): Initial workflow for Windows Notification System
  - Inception + Construction phases
  - WPF-based notification system
  - PowerShell integration
  - Single-file EXE packaging
  - 70% code coverage target

---

**Ready to Execute**: This workflow is complete and ready to run. Execute with `/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml` to begin.
