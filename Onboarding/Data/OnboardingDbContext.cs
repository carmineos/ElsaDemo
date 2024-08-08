using Microsoft.EntityFrameworkCore;
using Onboarding.Data.Models.Workflows;
using Onboarding.Entities;

namespace Onboarding.Data;

public class OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : DbContext(options)
{
    public DbSet<OnboardingTask> Tasks { get; set; } = default!;


    public DbSet<WorkflowType> WorkflowTypes { get; set; } = default!;
    public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; } = default!;
    public DbSet<WorkflowRequest> WorkflowRequests { get; set; } = default!;
}
