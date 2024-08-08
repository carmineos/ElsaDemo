using Onboarding.Data.Models.Workflows;

namespace Onboarding.Views.Home;

public class IndexViewModel(ICollection<TaskRequest> tasks)
{
    public ICollection<TaskRequest> Tasks { get; set; } = tasks;
}