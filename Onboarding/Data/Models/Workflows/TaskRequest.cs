namespace Onboarding.Data.Models.Workflows;

/// <summary>
/// A task that needs to be completed by the user.
/// </summary>
public class TaskRequest
{
    /// <summary>
    /// The ID of the task.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// An external ID that can be used to reference the task.
    /// </summary>
    public string ExternalTaskId { get; set; } = default!;

    /// <summary>
    /// The ID of the onboarding process that the task belongs to.
    /// </summary>
    public Guid WorklowRequestId { get; set; } = default!;


    /// <summary>
    /// The name of the task.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The task description.
    /// </summary>
    public string Description { get; set; } = default!;

    public Guid? CompletedBy { get; set; } = default!;

    /// <summary>
    /// Whether the task has been completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// The date and time when the task was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the task was completed.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    public WorkflowRequest WorkflowRequest { get; set; } = default!;
}
