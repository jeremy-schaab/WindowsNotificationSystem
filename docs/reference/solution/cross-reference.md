# Cross-Reference Documentation

**Solution**: WindowsNotificationSystem.sln
**Generated**: 2025-11-23

## Project Dependencies

This section maps out how projects reference each other, enabling understanding of the architectural layers.

### NotifyUser (Presentation Layer)
**References**:
- NotifyUser.Application
- NotifyUser.Infrastructure

**Purpose**: Entry point console application that wires up dependency injection and handles command-line parsing.

### NotifyUser.Application (Application Layer)
**References**:
- NotifyUser.Domain (implicit from dependency analysis)

**Purpose**: Application services that orchestrate use cases and coordinate domain logic.

### NotifyUser.Infrastructure (Infrastructure Layer)
**References**:
- NotifyUser.Domain

**Purpose**: Implements domain service interfaces for Windows notifications and audio playback using external libraries.

### NotifyUser.Domain (Domain Layer)
**References**: None (pure domain layer)

**Purpose**: Core business logic, aggregates, value objects, and domain service interfaces.

## Interface Implementation Map

This section shows which projects implement interfaces defined in the domain.

### IAudioService
- **Defined in**: NotifyUser.Domain
- **Implemented in**: NotifyUser.Infrastructure (assumed)
- **Purpose**: Audio playback for notification sounds

### INotificationService
- **Defined in**: NotifyUser.Domain
- **Implemented by**: NotifyUser.Domain.Services.NotificationOrchestrationService
- **Purpose**: Main orchestration service for displaying notifications

### IToastNotificationService
- **Defined in**: NotifyUser.Domain
- **Implemented in**: NotifyUser.Infrastructure (assumed)
- **Purpose**: Windows toast notification display

## Key Models by Layer

### Domain Models (NotifyUser.Domain)
- **NotificationRequest**: Aggregate root representing notification data
- **NotificationResult**: Result of notification display operation
- **Duration**: Value object for notification display duration
- **Result**: Functional error handling pattern
- **NotificationOrchestrationService**: Domain service implementation

### Application Models (NotifyUser.Application)
- **NotificationApplicationService**: Application service coordinating use cases

### Infrastructure Models (NotifyUser.Infrastructure)
- Various implementations of domain service interfaces

## Technology Stack

### Core Frameworks
- **.NET 10**: All projects target .NET 10
- **C# 13**: Latest language features

### Key Dependencies
- **System.CommandLine** (2.0.0-beta4): Command-line parsing
- **Microsoft.Toolkit.Uwp.Notifications** (7.1.3): Windows toast notifications
- **Microsoft.Extensions.DependencyInjection** (10.0.0): Dependency injection container
- **Microsoft.Extensions.Hosting** (10.0.0): Generic host for console apps
- **Microsoft.Extensions.Logging** (10.0.0): Logging infrastructure
- **System.Windows.Extensions** (10.0.0): Windows-specific extensions

## Architecture Pattern

This solution follows **Domain-Driven Design (DDD)** with **Clean Architecture** principles:

1. **Domain Layer** (NotifyUser.Domain)
   - No external dependencies
   - Contains business logic, aggregates, value objects
   - Defines service interfaces

2. **Application Layer** (NotifyUser.Application)
   - Depends only on Domain
   - Orchestrates use cases
   - Coordinates domain objects

3. **Infrastructure Layer** (NotifyUser.Infrastructure)
   - Depends on Domain
   - Implements domain service interfaces
   - Handles external dependencies (Windows APIs, audio)

4. **Presentation Layer** (NotifyUser)
   - Depends on Application and Infrastructure
   - Entry point with DI configuration
   - Command-line interface

## Recommended Exploration Paths

### Understanding Core Functionality
1. Start with `NotifyUser.Domain\Aggregates\NotificationRequest.cs`
2. Review `NotifyUser.Domain\Services\INotificationService.cs`
3. Check `NotifyUser.Application\Services\NotificationApplicationService.cs`
4. Examine entry point in `NotifyUser\Program.cs`

### Understanding Notification Display
1. Review `NotifyUser.Domain\Services\IToastNotificationService.cs`
2. Check infrastructure implementation (search for toast notification classes)

### Understanding Architecture
1. Read `NotifyUser.Domain\Common\Result.cs` (functional error handling)
2. Review `NotifyUser.Domain\ValueObjects\Duration.cs` (value object pattern)
3. Examine `NotifyUser.Domain\Aggregates\NotificationRequest.cs` (aggregate pattern)
