using System.Text.Json;

namespace Onboarding.Data.Models.Workflows;

public class WorkflowTemplate
{
    public int Id { get; set; }
    public int WorkflowTypeId { get; set; }
    public int CompanyId { get; set; }
    public string RequestJsonSchema { get; set; } = default!;
    public string WorkflowDefinitionId { get; set; } = default!;
    public WorkflowType WorkflowType { get; set; } = default!;
}
