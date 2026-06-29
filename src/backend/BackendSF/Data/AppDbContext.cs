using BackendSF.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendSF.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<PlanPutovanjaEntity> PlanoviPutovanja => Set<PlanPutovanjaEntity>();

    public DbSet<DestinacijaEntity> Destinacije => Set<DestinacijaEntity>();

    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();

    public DbSet<ExpenseEntity> Expenses => Set<ExpenseEntity>();

    public DbSet<ChecklistItemEntity> ChecklistItems => Set<ChecklistItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(korisnik =>
        {
            korisnik.HasKey(k => k.Id);
            korisnik.Property(k => k.Name).HasMaxLength(120).IsRequired();
            korisnik.Property(k => k.Email).HasMaxLength(180).IsRequired();
            korisnik.HasIndex(k => k.Email).IsUnique();
            korisnik.Property(k => k.PasswordHash).HasMaxLength(500).IsRequired();
            korisnik.Property(k => k.Role).HasMaxLength(40).IsRequired();
        });

        modelBuilder.Entity<PlanPutovanjaEntity>(planPutovanja =>
        {
            planPutovanja.ToTable("PlanoviPutovanja");
            planPutovanja.HasKey(plan => plan.Id);
            planPutovanja.Property(plan => plan.Naziv).HasMaxLength(160).IsRequired();
            planPutovanja.Property(plan => plan.KratakOpis).HasMaxLength(1000);
            planPutovanja.Property(plan => plan.OpsteNapomene).HasMaxLength(2000);
            planPutovanja.Property(plan => plan.PlaniraniBudzet).HasPrecision(18, 2);

            planPutovanja.HasOne(plan => plan.User)
                .WithMany(korisnik => korisnik.PlanoviPutovanja)
                .HasForeignKey(plan => plan.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DestinacijaEntity>(destinacija =>
        {
            destinacija.ToTable("Destinacije");
            destinacija.HasKey(d => d.Id);
            destinacija.Property(d => d.Naziv).HasMaxLength(160).IsRequired();
            destinacija.Property(d => d.Lokacija).HasMaxLength(200).IsRequired();
            destinacija.Property(d => d.KratakOpis).HasMaxLength(1000);
            destinacija.HasOne(d => d.PlanPutovanja)
                .WithMany(plan => plan.Destinacije)
                .HasForeignKey(d => d.PlanPutovanjaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ActivityEntity>(aktivnost =>
        {
            aktivnost.HasKey(a => a.Id);
            aktivnost.Property(a => a.Title).HasMaxLength(160).IsRequired();
            aktivnost.Property(a => a.Location).HasMaxLength(200);
            aktivnost.Property(a => a.Description).HasMaxLength(1000);
            aktivnost.Property(a => a.Status).HasMaxLength(40).IsRequired();
            aktivnost.Property(a => a.EstimatedCost).HasPrecision(18, 2);
            aktivnost.HasOne(a => a.PlanPutovanja)
                .WithMany(plan => plan.Aktivnosti)
                .HasForeignKey(a => a.PlanPutovanjaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExpenseEntity>(trosak =>
        {
            trosak.HasKey(t => t.Id);
            trosak.Property(t => t.Title).HasMaxLength(160).IsRequired();
            trosak.Property(t => t.Category).HasMaxLength(80).IsRequired();
            trosak.Property(t => t.Amount).HasPrecision(18, 2);
            trosak.Property(t => t.Description).HasMaxLength(1000);
            trosak.HasOne(t => t.PlanPutovanja)
                .WithMany(plan => plan.Troskovi)
                .HasForeignKey(t => t.PlanPutovanjaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChecklistItemEntity>(stavka =>
        {
            stavka.HasKey(s => s.Id);
            stavka.Property(s => s.Title).HasMaxLength(160).IsRequired();
            stavka.HasOne(s => s.PlanPutovanja)
                .WithMany(plan => plan.StavkeCheckListe)
                .HasForeignKey(s => s.PlanPutovanjaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
