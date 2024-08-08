using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data.Models.Configurations;

public class WorkflowRequestConfiguration : IEntityTypeConfiguration<WorkflowRequest>
{
    public void Configure(EntityTypeBuilder<WorkflowRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.WorkflowTemplate)
            .WithMany()
            .HasForeignKey(x => x.WorkflowTemplateId)
            .IsRequired();

        builder.Property(x => x.CreatorId)
            .IsRequired();

        builder.Property(x => x.RequestorId)
            .IsRequired();

        builder.Property(x => x.RequestJsonData)
            .IsRequired();

        builder.HasMany(x => x.TaskRequests)
            .WithOne(x => x.WorkflowRequest)
            .HasForeignKey(x => x.WorklowRequestId);
    }
}
