
namespace QAPortal.Data.EntityConfigs;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using QAPortal.Data.Entities;

public class ApprovalEntityConfig : IEntityTypeConfiguration<ApprovalEntity>
{
    public void Configure(EntityTypeBuilder<ApprovalEntity> builder)
    {
        builder.ToTable("Approvals");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.IsApproved)
            .IsRequired().HasDefaultValue(false);
        builder.Property(a => a.ApprovedBy)
            .IsRequired();
        builder.Property(a => a.ApprovalFor)
            .IsRequired();
            
        builder.HasOne(a => a.RequestedUser)
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);

    }
}