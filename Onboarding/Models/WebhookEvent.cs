using System.Text.Json;

namespace Onboarding.Models;

public record WebhookEvent(string EventType, RunTaskWebhook Payload, DateTimeOffset Timestamp);

public record RunTaskWebhook(string WorkflowInstanceId, string TaskId, string TaskName, TaskPayload TaskPayload);

public record TaskPayload(JsonDocument? Request, Guid WorkflowRequestId, string Description);
