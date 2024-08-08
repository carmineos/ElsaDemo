using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data.Models.Configurations;

public class WorkflowTypeConfiguration : IEntityTypeConfiguration<WorkflowType>
{
    public void Configure(EntityTypeBuilder<WorkflowType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasData(Enum.GetValues<WorkflowTypes>()
            .Select(e => new WorkflowType()
            {
                Id = (int)e,
                Name = e.ToString()
            }));
    }
}

public class TaskTypeConfiguration : IEntityTypeConfiguration<TaskType>
{
    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasData(Enum.GetValues<TaskTypes>()
            .Select(e => new TaskType()
            {
                Id = (int)e,
                Name = e.ToString()
            }));
    }
}