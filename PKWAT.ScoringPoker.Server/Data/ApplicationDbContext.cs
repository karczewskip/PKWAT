﻿namespace PKWAT.ScoringPoker.Server.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {
        public DbSet<ScoringTask> ScoringTasks { get; set; }
        public DbSet<ScoringTaskStatus> ScoringTaskStatuses { get; set; }
        public DbSet<EstimationMethod> EstimationMethods { get; set; }
        public DbSet<UserEstimation> UserEstimations { get; set; }
        public DbSet<EstimationMethodPossibleValue> EstimationMethodPossibleValues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ScoringTask>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Name)
                    .HasConversion(x => x.Name, x => ScoringTaskName.Create(x))
                    .HasMaxLength(ScoringTaskName.MaxLength);

                b.HasOne(x => x.EstimationMethod)
                    .WithMany(x => x.ScoringTasks)
                    .HasForeignKey(x => x.EstimationMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.UserEstimations)
                    .WithOne()
                    .HasForeignKey(x => x.ScoringTaskId);

                b.Property(x => x.FinalEstimationMethodName)
                    .HasConversion(x => x.Value, x => EstimationMethodName.Create(x))
                    .HasMaxLength(EstimationMethodName.MaxLength);

                b.Property(x => x.FinalEstimationValue)
                    .HasConversion(x => x.Value, x => EstimationMethodValue.Create(x))
                    .HasMaxLength(EstimationMethodValue.MaxLength);
            });

            builder.Entity<ScoringTaskStatus>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Name)
                    .HasMaxLength(Enum.GetValues(typeof(ScoringTaskStatusId))
                    .Cast<ScoringTaskStatusId>()
                    .Max(x => x.ToFriendlyString().Length));

                b.HasData(
                    Enum.GetValues(typeof(ScoringTaskStatusId))
                    .Cast<ScoringTaskStatusId>()
                    .Select(ScoringTaskStatus.Create));
            });

            builder.Entity<EstimationMethod>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasMany(x => x.PossibleValues)
                .WithOne()
                .HasForeignKey(x => x.EstimationMethodId);

                b.Property(x => x.Name)
                    .HasConversion(x => x.Value, x => EstimationMethodName.Create(x))
                    .HasMaxLength(EstimationMethodName.MaxLength);
            });

            builder.Entity<UserEstimation>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.EstimationMethodName)
                    .HasConversion(x => x.Value, x => EstimationMethodName.Create(x))
                    .HasMaxLength(EstimationMethodName.MaxLength);

                b.Property(x => x.Value)
                    .HasConversion(x => x.Value, x => EstimationMethodValue.Create(x))
                    .HasMaxLength(EstimationMethodValue.MaxLength);
            });

            builder.Entity<EstimationMethodPossibleValue>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.EstimationMethodValue)
                    .HasConversion(x => x.Value, x => EstimationMethodValue.Create(x))
                    .HasMaxLength(EstimationMethodValue.MaxLength);
            });
        }
    }
}
