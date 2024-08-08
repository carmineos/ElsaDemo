using Microsoft.EntityFrameworkCore;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data;

public class OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : DbContext(options)
{
    public DbSet<WorkflowType> WorkflowTypes { get; set; } = default!;
    public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; } = default!;
    public DbSet<WorkflowRequest> WorkflowRequests { get; set; } = default!;

    public DbSet<TaskRequest> TaskRequests { get; set; } = default!;
}
