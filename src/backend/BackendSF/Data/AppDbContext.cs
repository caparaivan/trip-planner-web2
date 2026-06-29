using BackendSF.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendSF.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<TripPlanEntity> TripPlans => Set<TripPlanEntity>();

    public DbSet<DestinationEntity> Destinations => Set<DestinationEntity>();

    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();

    public DbSet<ExpenseEntity> Expenses => Set<ExpenseEntity>();

    public DbSet<ChecklistItemEntity> ChecklistItems => Set<ChecklistItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Name).HasMaxLength(120).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(180).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(40).IsRequired();
        });

        modelBuilder.Entity<TripPlanEntity>(entity =>
        {
            entity.HasKey(plan => plan.Id);
            entity.Property(plan => plan.Title).HasMaxLength(160).IsRequired();
            entity.Property(plan => plan.Description).HasMaxLength(1000);
            entity.Property(plan => plan.Notes).HasMaxLength(2000);
            entity.Property(plan => plan.PlannedBudget).HasPrecision(18, 2);

            entity.HasOne(plan => plan.User)
                .WithMany(user => user.TripPlans)
                .HasForeignKey(plan => plan.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DestinationEntity>(entity =>
        {
            entity.HasKey(destination => destination.Id);
            entity.Property(destination => destination.Name).HasMaxLength(160).IsRequired();
            entity.Property(destination => destination.Location).HasMaxLength(200).IsRequired();
            entity.Property(destination => destination.Description).HasMaxLength(1000);
            entity.HasOne(destination => destination.TripPlan)
                .WithMany(plan => plan.Destinations)
                .HasForeignKey(destination => destination.TripPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ActivityEntity>(entity =>
        {
            entity.HasKey(activity => activity.Id);
            entity.Property(activity => activity.Title).HasMaxLength(160).IsRequired();
            entity.Property(activity => activity.Location).HasMaxLength(200);
            entity.Property(activity => activity.Description).HasMaxLength(1000);
            entity.Property(activity => activity.Status).HasMaxLength(40).IsRequired();
            entity.Property(activity => activity.EstimatedCost).HasPrecision(18, 2);
            entity.HasOne(activity => activity.TripPlan)
                .WithMany(plan => plan.Activities)
                .HasForeignKey(activity => activity.TripPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExpenseEntity>(entity =>
        {
            entity.HasKey(expense => expense.Id);
            entity.Property(expense => expense.Title).HasMaxLength(160).IsRequired();
            entity.Property(expense => expense.Category).HasMaxLength(80).IsRequired();
            entity.Property(expense => expense.Amount).HasPrecision(18, 2);
            entity.Property(expense => expense.Description).HasMaxLength(1000);
            entity.HasOne(expense => expense.TripPlan)
                .WithMany(plan => plan.Expenses)
                .HasForeignKey(expense => expense.TripPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChecklistItemEntity>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Title).HasMaxLength(160).IsRequired();
            entity.HasOne(item => item.TripPlan)
                .WithMany(plan => plan.ChecklistItems)
                .HasForeignKey(item => item.TripPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
