# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an **AI-DLC (AI-Driven Development Lifecycle)** managed project for building a **Windows Notification System** - a PowerShell-callable executable for providing native Windows notifications during Claude Code automation tasks.

The project follows AI-DLC methodology with three phases:
- **Inception**: Intent clarification, Unit/Bolt planning
- **Construction**: Domain modeling, logical design, code generation, testing
- **Operations**: Deployment, monitoring, incident response

## AI-DLC Commands

### Starting Development

Execute the pre-configured workflow to build the complete system:
```bash
/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml
```

See `QUICK-START.md` for full workflow details and expected output.

### Phase Management

```bash
# Check current phase and project status
/ai-dlc:status

# Transition to next phase with validation
/ai-dlc:next-phase

# Start a new intent
/ai-dlc:start-intent "Your feature description"

# Create units from current intent
/ai-dlc:create-units artifacts/inception/intent-20251122-001.md

# Plan bolts for a unit
/ai-dlc:plan-bolt <unit-name>

# Complete current bolt
/ai-dlc:complete-bolt <bolt-name>
```

### Construction Phase

```bash
# Generate domain model
/ai-dlc:domain-model <unit-name>

# Create logical design
/ai-dlc:logical-design artifacts/construction/<domain-model>.md

# Generate code from design
/ai-dlc:generate-code artifacts/construction/<logical-design>.md

# Build and run tests
/ai-dlc:build-tests

# Create UI tests
/ai-dlc:implement-ui-tests
```

### Quality & Review

```bash
# Review artifacts for quality
/ai-dlc:review-artifacts

# Code review
/ai-dlc:review-code

# Security audit
/ai-dlc:security-audit full

# Assess risks
/ai-dlc:assess-risks project
```

### Documentation

```bash
# Generate documentation
/ai-dlc:generate-docs <component-name>

# Create diagrams
/ai-dlc:create-diagram <unit-name>

# Create ADR
Skill: adr-generation
```

## Specialized Skills

This repository includes 15+ AI-DLC skills accessible via the `Skill:` command:

### .NET Development
- **dotnet-development**: Build, test, format .NET projects
- **dotnet-project-scaffold**: Create .NET 10 projects with CPM
- **csharp-refactor-advisor**: Apply SOLID principles, detect code smells
- **blazor-component-generator**: Scaffold Blazor components (Syncfusion/Telerik/MudBlazor)

### Testing
- **playwright-test-builder**: Generate E2E tests for Blazor apps
- **test-suite-builder**: Create xUnit tests with Moq/FluentAssertions

### Design & Architecture
- **ddd-modeling**: Domain-Driven Design patterns
- **logical-design**: Create architecture diagrams
- **api-contract-designer**: OpenAPI/Swagger specifications
- **ux-design**: UI/UX design and accessibility audits

### Project Management
- **azure-devops-story**: Create/sync Azure DevOps work items (PAT configured locally)
- **story-tracker**: Track implementation progress
- **bolt-estimator**: Estimate work in Bolts (1-3 day increments)
- **mob-elaboration**: Facilitate collaborative requirements gathering
- **nfr-elicitation**: Elicit Non-Functional Requirements

### Documentation & Planning
- **adr-generation**: Create Architecture Decision Records
- **prp-generator**: Generate Product Requirement Prompts
- **risk-assessment**: Identify and mitigate project risks

### Skills
- **skill-creator**: Create new Claude Code skills
- **skill-reviewer**: Review skill quality

## Directory Structure

```
WindowsNotificationSystem/
├── artifacts/                    # AI-DLC artifacts (tracked in git)
│   ├── inception/               # Intent, Units, Bolts
│   ├── construction/            # Domain Models, Logical Designs
│   ├── operations/              # Deployment manifests, runbooks
│   └── workflows/               # Workflow YAML files
├── .ai-dlc/                     # AI-DLC framework
│   ├── session-state.json       # Current session tracking
│   └── workflows/               # Workflow engine
├── .claude/                     # Claude Code configuration
│   ├── commands/                # 42 slash commands
│   └── skills/                  # 29 reusable skills
├── src/                         # Source code (generated during Construction)
├── tests/                       # Test projects
└── publish/                     # Build output
```

### Critical: artifacts/ Folder

The `artifacts/` folder is **always tracked in git** (via `!artifacts/` in .gitignore). This contains:
- Intent statements
- Unit and Bolt definitions
- Domain models
- Logical designs
- Workflow definitions

These artifacts drive the entire AI-DLC process and must be version controlled.

## Technology Stack

**Target**: .NET 10, C# 13, WPF for Windows desktop
**CLI Parsing**: System.CommandLine
**Notifications**: Microsoft.Toolkit.Uwp.Notifications
**Audio**: NAudio
**Testing**: xUnit, Moq, FluentAssertions, bUnit
**Packaging**: Single-file self-contained executable

## Workflow Orchestration

The primary workflow is pre-configured in `artifacts/workflows/windows-notification-workflow.yaml`.

**Execution**: Uses the workflow orchestrator (`workflow_orchestrator.py`) to execute steps sequentially with validation gates.

**Human Approval Points**:
- Intent review (Inception phase)
- Phase transitions (Inception → Construction → Operations)

**Automatic Steps**:
- Domain modeling
- Code generation
- Building and testing
- Publishing
- Documentation generation

## Azure DevOps Integration

**Organization**: fyisoft
**Default Project**: suitefyi

Use CURL with the PAT for Azure DevOps operations:
```bash
curl -u :$AZURE_DEVOPS_PAT \
  https://dev.azure.com/fyisoft/suitefyi/_apis/...
```

Set the PAT as an environment variable:
```powershell
$env:AZURE_DEVOPS_PAT = "your-pat-here"
```

Or use the `azure-devops-story` skill for work item management.

## AI-DLC Core Concepts

### Intent
High-level business objective that describes WHAT to build and WHY, not HOW.
Located in: `artifacts/inception/intent-*.md`

### Units
Logical groupings of work representing a cohesive feature or capability. Each Unit consists of 1-3 Bolts.

### Bolts
1-3 day development increments that are independently testable and deployable.

### Artifacts
Concrete deliverables at each phase:
- **Inception**: Intent, Units, Bolts, NFRs, Risk Register
- **Construction**: Domain Model, Logical Design, Code, Tests, Docs
- **Operations**: Deployment Manifests, Runbooks, Monitoring Configs

### Sessions
Work sessions are tracked in `.ai-dlc/session-state.json` with unique IDs (YYYYMMDD-NNN format).

## Development Workflow

1. **Review Current State**: `/ai-dlc:status`
2. **Execute Pre-Configured Workflow**: `/ai-dlc:run-workflow artifacts/workflows/windows-notification-workflow.yaml`
3. **Monitor Progress**: Workflow reports progress at each step
4. **Approve Gates**: Approve Intent and phase transitions when prompted
5. **Validate Output**: After completion, executable will be in `publish/NotifyUser.exe`

## Python Scripts

The workflow engine uses Python for orchestration:
- `workflow_orchestrator.py`: Main workflow execution engine
- `template-validator.py`: Validates workflow YAML structure
- Various skill scripts in `.claude/skills/*/scripts/`

## Key Files

- **QUICK-START.md**: Complete workflow execution guide
- **artifacts/workflows/windows-notification-workflow.yaml**: Main automated workflow
- **artifacts/inception/intent-20251122-001.md**: Project intent
- **.ai-dlc/session-state.json**: Current session and phase tracking
- **.claude/commands/**: 42 AI-DLC slash commands

## Best Practices

- Always review artifacts before proceeding to next phase
- Use `/ai-dlc:review-artifacts` as quality gate before phase transitions
- Keep Intent updated as requirements evolve
- Track all design decisions in ADRs (use `adr-generation` skill)
- Run security audits before Operations phase
- Document cross-cutting concerns in Risk Register
