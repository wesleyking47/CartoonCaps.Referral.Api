using CartoonCaps.Referral.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralContext : DbContext
{
    public ReferralContext() { }

    public ReferralContext(DbContextOptions<ReferralContext> options) : base(options) { }

    public DbSet<ReferralRecord> ReferralRecords { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(x => x.ReferralCode)
                .IsUnique();
        });

        modelBuilder.Entity<ReferralRecord>(entity =>
        {
            entity.HasIndex(x => new { x.RefereeId, x.ReferrerId })
                .IsUnique();
            entity.HasOne(x => x.Referee)
                .WithOne(x => x.RefereeRecord)
                .HasForeignKey<ReferralRecord>(x => x.RefereeId);
            entity.HasOne(x => x.Referrer)
                .WithMany(x => x.ReferrerRecords)
                .HasForeignKey(x => x.ReferrerId);
        });

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "John Doe", ReferralCode = "ABC123" },
            new User { Id = 2, Name = "Jane Smith", ReferralCode = "XYZ789" },
            new User { Id = 3, Name = "Bob Johnson", ReferralCode = "DEF456" },
            new User { Id = 4, Name = "Alice Williams", ReferralCode = "GHI789" },
            new User { Id = 5, Name = "Tom Brown", ReferralCode = "JKL123" },
            new User { Id = 6, Name = "Emily Davis", ReferralCode = "MNO456" },
            new User { Id = 7, Name = "David Wilson", ReferralCode = "PQR789" },
            new User { Id = 8, Name = "Olivia Taylor", ReferralCode = "STU123" },
            new User { Id = 9, Name = "Jack Anderson", ReferralCode = "VWX456" },
            new User { Id = 10, Name = "Grace Thomas", ReferralCode = "YZA789" }
        );

        modelBuilder.Entity<ReferralRecord>().HasData(
            new ReferralRecord
            {
                Id = 1,
                RefereeId = 2,
                ReferrerId = 1,
                ReferralStatus = "Pending"
            },
            new ReferralRecord
            {
                Id = 2,
                RefereeId = 3,
                ReferrerId = 1,
                ReferralStatus = "Pending"
            },
            new ReferralRecord
            {
                Id = 3,
                RefereeId = 4,
                ReferrerId = 1,
                ReferralStatus = "Pending"
            }
        );
    }
}