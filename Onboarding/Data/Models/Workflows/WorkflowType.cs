namespace Onboarding.Data.Models.Workflows;

public enum WorkflowTypes
{
    AbsenceRequest = 1,
}

public class WorkflowType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
