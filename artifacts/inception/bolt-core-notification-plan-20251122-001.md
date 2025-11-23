# Bolt Plan: CoreNotificationService Foundation

**Session ID**: 20251122-001
**Unit**: Unit 1 - CoreNotificationService
**Phase**: Construction
**Created**: 2025-11-22
**Status**: Planned

---

## Bolt Metadata

**Bolt Name**: Bolt 1 - CoreNotificationService Foundation
**Estimated Duration**: 2-3 days (16-24 hours)
**Developer**: TBD
**Complexity**: Medium
**Priority**: Critical (foundational)

---

## Objective

Implement the complete Windows toast notification engine that enables native Windows 10/11 toast notifications with configurable title, message, and duration. This bolt delivers a working, testable notification service that integrates with Windows Action Center and handles error scenarios gracefully.

**Deliverable**: Production-ready `INotificationService` implementation with comprehensive unit tests (80%+ coverage) that can display toast notifications in < 100ms with < 50ms initialization time.

---

## Scope

### In Scope
- `INotificationService` interface and abstractions
- `WindowsToastNotificationService` implementation using Microsoft.Toolkit.Uwp.Notifications
- `NotificationRequest` and `NotificationResult` domain models
- `ToastNotificationBuilder` fluent API for XML generation
- Error handling for disabled notification services
- Thread-safe concurrent notification support
- Unit tests with 80%+ coverage
- Windows 10 (1809+) and Windows 11 compatibility

### Out of Scope
- Audio support (Unit 3 - AudioNotificationSupport)
- CLI interface (Unit 2 - CommandLineInterface)
- Custom WPF windows (Unit 4 - CustomWindowUI)
- PowerShell integration (Unit 5)
- Build/packaging (Unit 6)

---

## Technical Approach

### Libraries & Frameworks
- **Microsoft.Toolkit.Uwp.Notifications** (v7.1.3): Windows toast notification APIs
- **.NET 10**: Target framework with C# 13 features
- **Windows.UI.Notifications**: Native Windows notification APIs (via toolkit)
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for tests
- **FluentAssertions**: Assertion library for readable tests

### Architecture Pattern
- **Interface-based design**: `INotificationService` abstraction for testability
- **Builder pattern**: `ToastNotificationBuilder` for fluent toast creation
- **Result pattern**: `NotificationResult` for error handling without exceptions
- **Immutable records**: `NotificationRequest` and `NotificationResult` as C# 13 records
- **Thread-safe singleton**: Service can handle concurrent requests

### Key Design Decisions
1. **Microsoft.Toolkit over raw APIs**: Abstracts complex XML toast templates
2. **Result pattern vs exceptions**: Performance-critical path returns status codes
3. **Fluent builder**: Simplifies toast configuration and validation
4. **Interface segregation**: Separate concerns for future audio/UI extensions
5. **System.Threading.Lock**: Use C# 13's new Lock type for thread synchronization

---

## Implementation Checklist

### Phase 1: Domain Models & Interfaces (4-6 hours)

- [ ] **Task 1.1**: Create project structure
  - New .NET 10 class library: `WindowsNotificationSystem.Core`
  - Configure .csproj for .NET 10, C# 13, nullable reference types
  - Add Microsoft.Toolkit.Uwp.Notifications NuGet package (7.1.3)
  - Estimated: 1 hour

- [ ] **Task 1.2**: Define `NotificationRequest` record
  - Properties: Title (required), Message (required), Duration (1-30s, default 5s)
  - Add validation logic (title/message not empty, duration in range)
  - Add `SilentMode` flag (bool, default false)
  - Use C# 13 record primary constructor syntax
  - Estimated: 1 hour

- [ ] **Task 1.3**: Define `NotificationResult` record
  - Properties: Success (bool), ErrorCode (enum), ErrorMessage (string?)
  - Create `NotificationErrorCode` enum (Success, Disabled, InvalidRequest, UnknownError)
  - Factory methods: `Success()`, `Failure(errorCode, message)`
  - Estimated: 1 hour

- [ ] **Task 1.4**: Define `INotificationService` interface
  - Method: `Task<NotificationResult> ShowAsync(NotificationRequest request)`
  - Method: `Task<bool> IsAvailableAsync()` (check if service enabled)
  - XML documentation comments for all members
  - Estimated: 1 hour

- [ ] **Task 1.5**: Create unit test project
  - New xUnit project: `WindowsNotificationSystem.Core.Tests`
  - Add references: xUnit, Moq, FluentAssertions
  - Project reference to Core library
  - Estimated: 1 hour

### Phase 2: Toast Builder Implementation (4-5 hours)

- [ ] **Task 2.1**: Implement `ToastNotificationBuilder` class
  - Fluent methods: `WithTitle(string)`, `WithMessage(string)`, `WithDuration(int)`
  - Method: `Build()` returns `ToastContent` (from Microsoft.Toolkit)
  - Internal validation before build
  - Estimated: 2 hours

- [ ] **Task 2.2**: Configure toast XML template
  - Use `ToastContentBuilder` from Microsoft.Toolkit
  - Set title as `AdaptiveText` with hint-maxLines
  - Set message as `AdaptiveText` with wrap
  - Configure duration as `ToastDuration` (short/long)
  - Estimated: 2 hours

- [ ] **Task 2.3**: Write unit tests for builder
  - Test valid toast creation
  - Test validation errors (empty title, invalid duration)
  - Test fluent API chaining
  - Test default values
  - Estimated: 1 hour

### Phase 3: Notification Service Implementation (6-8 hours)

- [ ] **Task 3.1**: Implement `WindowsToastNotificationService` class
  - Constructor: Initialize `ToastNotifierCompat.CreateToastNotifier()`
  - Implement `INotificationService` interface
  - Handle COM exceptions from Windows APIs
  - Add thread-safety using C# 13 `System.Threading.Lock`
  - Estimated: 3 hours

- [ ] **Task 3.2**: Implement `ShowAsync` method
  - Validate `NotificationRequest` using validation rules
  - Build toast using `ToastNotificationBuilder`
  - Show toast using `ToastNotifier.Show()`
  - Map duration to Windows `ToastDuration.Short` (< 10s) or `ToastDuration.Long`
  - Return `NotificationResult` with success/failure
  - Estimated: 2 hours

- [ ] **Task 3.3**: Implement `IsAvailableAsync` method
  - Check if notification service is enabled
  - Handle Windows API exceptions (service disabled scenario)
  - Return true/false based on availability
  - Estimated: 1 hour

- [ ] **Task 3.4**: Add error handling and logging
  - Catch `COMException` for disabled notifications
  - Catch `PlatformNotSupportedException` for non-Windows OS
  - Map exceptions to `NotificationErrorCode` enum
  - Add structured error messages
  - Estimated: 2 hours

### Phase 4: Integration & Testing (4-6 hours)

- [ ] **Task 4.1**: Write unit tests for `WindowsToastNotificationService`
  - Test successful notification display (mock Windows APIs if possible)
  - Test error handling for disabled service
  - Test thread-safety (concurrent calls)
  - Test validation failures return proper error codes
  - Estimated: 3 hours

- [ ] **Task 4.2**: Write integration tests
  - Create test helper that displays actual toast (manual verification)
  - Test with various title/message/duration combinations
  - Test duration edge cases (1s, 5s, 30s)
  - Test that toast appears in Action Center
  - Estimated: 2 hours

- [ ] **Task 4.3**: Performance testing
  - Measure initialization time (target: < 50ms)
  - Measure ShowAsync execution time (target: < 100ms)
  - Test concurrent notifications (10 simultaneous)
  - Verify no memory leaks
  - Estimated: 1 hour

### Phase 5: Documentation & Polish (2-3 hours)

- [ ] **Task 5.1**: Add XML documentation
  - Document all public interfaces, classes, methods
  - Add usage examples in XML comments
  - Document error codes and when they occur
  - Estimated: 1 hour

- [ ] **Task 5.2**: Create usage example
  - Simple console app demonstrating service usage
  - Example: `Examples/BasicNotification.cs`
  - Shows initialization, request creation, error handling
  - Estimated: 1 hour

- [ ] **Task 5.3**: Code review and refactoring
  - Ensure naming follows .NET conventions
  - Verify thread-safety implementation
  - Check for code smells and anti-patterns
  - Run code analysis (zero warnings)
  - Estimated: 1 hour

---

## Test Strategy

### Unit Test Coverage Goals
- **Target**: 80%+ line coverage
- **Focus Areas**:
  - `NotificationRequest` validation logic (100% coverage)
  - `ToastNotificationBuilder` fluent API (90%+ coverage)
  - `WindowsToastNotificationService` error handling (80%+ coverage)
  - Thread-safety scenarios (concurrent calls)

### Test Scenarios
1. **Happy Path**:
   - Valid request with title, message, default duration
   - Toast displays successfully
   - Result indicates success

2. **Validation Failures**:
   - Empty title returns InvalidRequest error
   - Empty message returns InvalidRequest error
   - Duration < 1 or > 30 returns InvalidRequest error

3. **Service Disabled**:
   - Windows notification service disabled
   - Returns NotificationErrorCode.Disabled
   - Error message explains how to enable

4. **Thread Safety**:
   - 10 concurrent ShowAsync calls
   - All succeed without race conditions
   - No exceptions thrown

5. **Performance**:
   - Initialization completes in < 50ms
   - ShowAsync completes in < 100ms
   - Memory usage stable (no leaks)

### Integration Test Approach
- **Manual Verification**: Run test app, visually confirm toast appears
- **Action Center Check**: Verify toast persists in Action Center
- **Multi-OS Testing**: Test on Windows 10 (1809+) and Windows 11
- **Accessibility**: Screen reader reads toast content correctly

### Mocking Strategy
- **Challenge**: Windows.UI.Notifications APIs are difficult to mock (COM interop)
- **Solution**: Extract Windows API calls to internal `IToastNotifierAdapter` interface
- **Benefit**: Can mock adapter for unit tests, use real adapter for integration tests
- **Tests**: 80% unit tests (mocked), 20% integration tests (real API)

---

## Acceptance Criteria (from Unit 1)

- [x] Display Windows toast notification with title and message
- [x] Support configurable duration (default 5 seconds, range 1-30 seconds)
- [x] Toast appears in Windows Action Center after display
- [x] Notification service initializes in < 50ms
- [x] Handle notification service errors gracefully (return error codes)
- [x] Support silent notifications (no sound) - via SilentMode flag
- [x] Unit tests achieve 80%+ coverage
- [x] Works on Windows 10 (1809+) and Windows 11

---

## Dependencies

### Blocks
- **Unit 2 (CommandLineInterface)**: Needs INotificationService to call
- **Unit 3 (AudioNotificationSupport)**: Extends INotificationService
- **Unit 4 (CustomWindowUI)**: Uses NotificationRequest model

### Blocked By
- None (foundational unit)

### External Dependencies
- **Microsoft.Toolkit.Uwp.Notifications** (7.1.3): Toast notification APIs
- **.NET 10 SDK**: Required for C# 13 features
- **Windows 10/11**: Platform requirement for toast APIs

---

## Risks & Mitigations

### Risk 1: COM Interop Complexity (Medium)
**Description**: Windows.UI.Notifications uses COM interop which is difficult to test and debug.

**Impact**: Testing challenges, potential runtime errors on different Windows versions.

**Mitigation**:
- Use proven Microsoft.Toolkit.Uwp.Notifications library (abstracts COM)
- Create adapter pattern for Windows API calls (enables mocking)
- Extensive integration testing on Windows 10 and 11
- Error handling for all COM exceptions

**Status**: Planned

### Risk 2: Notification Service Disabled (Low)
**Description**: Users may have Windows notifications disabled in system settings.

**Impact**: Notifications fail silently or throw exceptions.

**Mitigation**:
- Implement `IsAvailableAsync()` check before showing notifications
- Return clear error code (NotificationErrorCode.Disabled)
- Provide helpful error message with instructions to enable
- Document common causes and solutions

**Status**: Planned

### Risk 3: Thread Safety Issues (Medium)
**Description**: Concurrent notification requests may cause race conditions.

**Impact**: Crashes, duplicate notifications, or dropped notifications.

**Mitigation**:
- Use C# 13 `System.Threading.Lock` for synchronization
- Design stateless service (no shared mutable state)
- Comprehensive thread-safety tests (concurrent calls)
- Code review focused on concurrency

**Status**: Planned

### Risk 4: Performance Regression (Low)
**Description**: Initialization or notification display exceeds performance targets.

**Impact**: Poor user experience, slow automation scripts.

**Mitigation**:
- Lazy initialization of ToastNotifier (defer until first use)
- Async/await pattern prevents blocking
- Performance tests in CI/CD pipeline
- Profile with BenchmarkDotNet if issues found

**Status**: Planned

### Risk 5: Windows Version Compatibility (Low)
**Description**: Toast API differences between Windows 10 versions or Windows 11.

**Impact**: Notifications fail on specific Windows versions.

**Mitigation**:
- Microsoft.Toolkit abstracts version differences
- Test on Windows 10 1809, 21H2, and Windows 11
- Use stable APIs (avoid preview features)
- Document minimum Windows version (1809)

**Status**: Planned

---

## Definition of Done

### Code Quality
- [ ] All code follows .NET naming conventions and C# 13 best practices
- [ ] Zero compilation warnings (treat warnings as errors)
- [ ] All public APIs have XML documentation comments
- [ ] Code passes static analysis (SonarLint or similar)
- [ ] No code smells or anti-patterns identified

### Testing
- [ ] Unit tests achieve 80%+ line coverage
- [ ] All unit tests pass (xUnit test results green)
- [ ] Integration tests manually verified on Windows 10 and 11
- [ ] Thread-safety tests pass (no race conditions)
- [ ] Performance tests meet targets (< 50ms init, < 100ms display)

### Functionality
- [ ] Toast notifications display with title and message
- [ ] Duration configuration works (1-30 seconds)
- [ ] Toasts appear in Windows Action Center
- [ ] Silent mode suppresses sound (SilentMode flag)
- [ ] Error handling returns appropriate error codes
- [ ] Service initialization completes in < 50ms
- [ ] Notification display completes in < 100ms

### Documentation
- [ ] XML comments on all public interfaces/classes/methods
- [ ] Usage example created (Examples/BasicNotification.cs)
- [ ] README section drafted for CoreNotificationService
- [ ] Error codes documented with troubleshooting steps

### Integration
- [ ] NuGet package references added to .csproj
- [ ] Project builds successfully in Release configuration
- [ ] Project compatible with Windows 10 (1809+) and Windows 11
- [ ] No runtime dependencies beyond .NET 10 and Windows APIs

### Review
- [ ] Code peer reviewed by second developer (if available)
- [ ] Acceptance criteria verified against Unit 1 requirements
- [ ] Risk mitigations implemented and tested
- [ ] Ready for Unit 2 (CommandLineInterface) integration

---

## Estimated Task Breakdown

| Phase | Description | Estimated Time |
|-------|-------------|----------------|
| 1. Domain Models | Records, interfaces, test project | 4-6 hours |
| 2. Toast Builder | Fluent API and XML generation | 4-5 hours |
| 3. Service Implementation | Core service logic and error handling | 6-8 hours |
| 4. Testing | Unit, integration, performance tests | 4-6 hours |
| 5. Documentation | XML docs, examples, polish | 2-3 hours |
| **TOTAL** | **Full implementation** | **20-28 hours** |

**Estimated Duration**: 2.5-3.5 days (assuming 8-hour work days)

**Confidence**: High (well-defined requirements, proven libraries)

---

## Success Metrics

### Functional Success
- 100% of acceptance criteria met
- Toast notifications work on Windows 10 and 11
- Error handling covers all identified scenarios

### Quality Success
- Unit test coverage ≥ 80%
- Zero compilation warnings
- All tests pass (green build)

### Performance Success
- Initialization time < 50ms (measured)
- Notification display < 100ms (measured)
- No memory leaks (profiled)

### Integration Success
- Ready for Unit 2 (CLI) integration
- INotificationService interface stable
- NotificationRequest model extensible for Units 3-4

---

## Next Steps After Bolt Completion

1. **Retrospective**: Capture learnings, actual time vs estimate, blockers encountered
2. **Velocity Update**: Calculate velocity factor for remaining bolts
3. **Unit 2 Planning**: Create bolt plan for CommandLineInterface (depends on Unit 1)
4. **Risk Review**: Update risk register based on implementation experience
5. **Demo**: Show working toast notifications to stakeholders

---

## Notes

- **Priority**: This is the critical path bolt - all other units depend on it
- **Testing Focus**: Thread-safety and error handling are highest risk areas
- **Performance**: Use BenchmarkDotNet if initial profiling shows issues
- **Windows Versions**: Test on at least Windows 10 21H2 and Windows 11 22H2
- **Toolkit Version**: 7.1.3 is stable; avoid preview versions for reliability

---

**Bolt Status**: Ready for Development
**Next Action**: `/ai-dlc:next-bolt` to start implementation

