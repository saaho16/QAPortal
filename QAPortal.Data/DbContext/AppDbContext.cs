using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;


namespace QAPortal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {

    //     base.OnConfiguring(optionsBuilder);
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ApprovalEntity> Approvals { get; set; }

    public DbSet<QuestionsEntity> Questions { get; set; }
    public DbSet<AnswersEntity> Answers { get; set; }


}

