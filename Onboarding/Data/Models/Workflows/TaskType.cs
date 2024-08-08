namespace Onboarding.Data.Models.Workflows;

public enum TaskTypes
{
    LineManagerReview = 1,
    HRReview = 2,
    HRManagerReview = 3,
}

public class TaskType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}