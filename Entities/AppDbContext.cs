using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using onilne_service.Model;

namespace onilne_service.Entities;

public partial class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccountHistory> BankAccountHistories { get; set; }

    public virtual DbSet<BankCard> BankCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // ⚠️ Required for Identity tables

        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BankAccountHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("BankAccountHistory");

            entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.ArchivedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.BankAccountId).HasMaxLength(450);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.CardHolderName).HasMaxLength(100);
            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.MobileNumber).HasMaxLength(15);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<BankCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(15)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
