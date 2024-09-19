using System.Text.Json;

namespace Onboarding.Data.Models.Workflows;

public class WorkflowRequest
{
    public Guid Id { get; set; }
    public string WorkflowDefinitionId { get; set; } = default!;
    public string? WorkflowInstanceId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset CompletedAtUtc { get; set; }

    public List<TaskRequest> TaskRequests { get; set; } = new List<TaskRequest>();
}
