using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data.Models.Configurations;

public class TaskRequestConfiguration : IEntityTypeConfiguration<TaskRequest>
{
    public void Configure(EntityTypeBuilder<TaskRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Description)
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
            .HasForeignKey(x => x.WorklowRequestId)
            .IsRequired();
    }
}
