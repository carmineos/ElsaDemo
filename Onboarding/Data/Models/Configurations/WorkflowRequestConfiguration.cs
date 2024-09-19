using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;

namespace Onboarding.Data.Models.Configurations;

public class WorkflowRequestConfiguration : IEntityTypeConfiguration<WorkflowRequest>
{
    public void Configure(EntityTypeBuilder<WorkflowRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedBy)
            .IsRequired();
    }
}
