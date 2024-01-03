namespace PKWAT.ScoringPoker.Server.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {
        public DbSet<ScoringTask> ScoringTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ScoringTask>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name)
                    .HasConversion(x => x.Name, x => ScoringTaskName.Create(x))
                    .HasMaxLength(ScoringTaskName.MaxLength);
            });

            //builder.Entity<ScoringTaskUser>()
            //    .HasKey(x => new { x.ScoringTaskId, x.UserId });
            //builder.Entity<ScoringTaskUserVote>()
            //    .HasKey(x => new { x.ScoringTaskUserId, x.ScoringTaskId, x.UserId });
            //builder.Entity<ScoringTaskUserVote>()
            //    .HasOne(x => x.ScoringTaskUser)
            //    .WithMany(x => x.Votes)
            //    .HasForeignKey(x => new { x.ScoringTaskUserId, x.ScoringTaskId });
            //builder.Entity<ScoringTaskUserVote>()
            //    .HasOne(x => x.User)
            //    .WithMany(x => x.Votes)
            //    .HasForeignKey(x => x.UserId);
        }
    }
}
