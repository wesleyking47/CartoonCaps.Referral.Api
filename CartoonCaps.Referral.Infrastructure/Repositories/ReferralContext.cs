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
            optionsBuilder.UseNpgsql();
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
    }
}