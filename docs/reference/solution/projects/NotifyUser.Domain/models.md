# Models - NotifyUser.Domain

**Project**: NotifyUser.Domain
**Total Models**: 7

## Model List

### NotifyUser.Domain.Class1

**File**: `Class1.cs` (Line 3)

---

### NotifyUser.Domain.Aggregates.NotificationRequest

**File**: `NotificationRequest.cs` (Line 10)

**Documentation**:
<summary>
Aggregate root representing a notification request.
Immutable with built-in business rule validation.
</summary>

---

### NotifyUser.Domain.Aggregates.NotificationResult

**File**: `NotificationResult.cs` (Line 9)

**Documentation**:
<summary>
Represents the result of a notification display operation.
Immutable value object containing success/failure state and metadata.
</summary>

---

### NotifyUser.Domain.Common.Result

**File**: `Result.cs` (Line 7)

**Documentation**:
<summary>
Represents the result of an operation that can succeed or fail.
Implements the Result pattern for functional error handling.
</summary>

---

### NotifyUser.Domain.Common.Result

**File**: `Result.cs` (Line 7)

**Documentation**:
<summary>
Represents the result of an operation that can succeed or fail.
Implements the Result pattern for functional error handling.
</summary>

---

### NotifyUser.Domain.Services.NotificationOrchestrationService

**File**: `NotificationOrchestrationService.cs` (Line 10)

**Documentation**:
<summary>
Domain service that orchestrates notification display by coordinating
toast notifications and audio playback.
</summary>

**Inherits/Implements**: INotificationService

---

### NotifyUser.Domain.ValueObjects.Duration

**File**: `Duration.cs` (Line 9)

**Documentation**:
<summary>
Represents the display duration for a notification in seconds.
Immutable value object with built-in validation.
</summary>

---


