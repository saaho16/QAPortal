namespace QAPortal.Data.EntityConfigs;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using QAPortal.Data.Entities;

public class QuestionsEntityConfig : IEntityTypeConfiguration<QuestionsEntity>
{
    public void Configure(EntityTypeBuilder<QuestionsEntity> builder)
    {
        builder.ToTable("Questions");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(q => q.Body)
            .IsRequired();
        builder.Property(q => q.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        // builder.Property(q => q.UpdatedAt)
        //      .HasDefaultValueSql("GETDATE()");



        
        builder.HasMany(q => q.Answers)
               .WithOne(a => a.Question)
               .HasForeignKey(a => a.QuestionId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);

      
        builder.HasOne(q => q.CreatedUser)
               .WithMany()
               .HasForeignKey(q => q.CreatedBy)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
        

        
    
    }
}