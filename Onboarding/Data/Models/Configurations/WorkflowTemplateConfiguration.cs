using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboarding.Data.Models.Workflows;
using System.Text.Json;

namespace Onboarding.Data.Models.Configurations;

public class WorkflowTemplateConfiguration : IEntityTypeConfiguration<WorkflowTemplate>
{
    public void Configure(EntityTypeBuilder<WorkflowTemplate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.WorkflowType)
            .WithMany()
            .HasForeignKey(x => x.WorkflowTypeId)
            .IsRequired();

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.RequestJsonSchema)
            .IsRequired();

        builder.Property(x => x.WorkflowDefinitionId)
            .IsRequired();
    }
}
