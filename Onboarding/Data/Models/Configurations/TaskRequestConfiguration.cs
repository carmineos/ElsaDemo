using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data.Models.Configurations;

public class TaskRequestConfiguration : IEntityTypeConfiguration<TaskRequest>
{
    public void Configure(EntityTypeBuilder<TaskRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.TaskType)
            .WithMany()
            .HasForeignKey(x => x.TaskTypeId)
            .IsRequired();

        builder.Property(x => x.ExternalTaskId)
            .IsRequired();

        builder.Property(x => x.IsCompleted)
            .IsRequired();

        builder.Property(x => x.CompletedBy);

        builder.Property(x => x.CompletedAt);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.WorkflowRequest)
            .WithMany(x => x.TaskRequests)
            .HasForeignKey(x => x.WorkflowRequestId)
            .IsRequired();
    }
}
