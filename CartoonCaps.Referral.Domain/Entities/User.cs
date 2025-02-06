namespace CartoonCaps.Referral.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ReferralCode { get; set; } = null!;

    public ICollection<ReferralRecord>? ReferrerRecords { get; set; }
    public ReferralRecord? RefereeRecord { get; set; }
}