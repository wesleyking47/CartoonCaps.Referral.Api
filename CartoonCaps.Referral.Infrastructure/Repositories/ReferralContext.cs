using CartoonCaps.Referral.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralContext : DbContext
{
    public DbSet<ReferralRecord> ReferralRecords { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(x => x.ReferralCode)
                .IsUnique();
            entity.HasMany<ReferralRecord>()
                .WithOne(x => x.Referrer);
        });

        modelBuilder.Entity<ReferralRecord>(entity =>
        {
            entity.HasIndex(x => new { x.RefereeId, x.ReferrerId })
                .IsUnique();
            entity.HasOne(x => x.Referee)
                .WithMany(x => x.ReferralRecords)
                .HasForeignKey(x => x.RefereeId);
            entity.HasOne(x => x.Referrer)
                .WithMany(x => x.ReferralRecords)
                .HasForeignKey(x => x.ReferrerId);
        });
    }
}