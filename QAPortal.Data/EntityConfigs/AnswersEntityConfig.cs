namespace QAPortal.Data.EntityConfigs;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using QAPortal.Data.Entities;

public class AnswersEntityConfig : IEntityTypeConfiguration<AnswersEntity>
{
    public void Configure(EntityTypeBuilder<AnswersEntity> builder)
    {
        builder.ToTable("Answers");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Body)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(a => a.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        builder.Property(a => a.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");


        builder.HasOne(a => a.Question)
               .WithMany(q => q.Answers)
               .HasForeignKey(a => a.QuestionId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.CreatedUser)
               .WithMany()
               .HasForeignKey(a => a.CreatedBy)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);




    }
}