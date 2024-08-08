using System.Text.Json;

namespace Onboarding.Data.Models.Workflows;

public class WorkflowRequest
{
    public Guid Id { get; set; }
    public int WorkflowTemplateId { get; set; }
    public string RequestJsonData { get; set; } = default!;
    public string? WorkflowInstanceId { get; set; }
    public Guid CreatorId { get; set; }
    public Guid RequestorId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset CompletedAtUtc { get; set; }

    public WorkflowTemplate WorkflowTemplate { get; set; } = default!;

    public List<TaskRequest> TaskRequests { get; set; } = new List<TaskRequest>();
}
