namespace CartoonCaps.Referral.Domain.Entities;

public class ReferralRecord
{
    public int Id { get; set; }
    public int RefereeId { get; set; }
    public int ReferrerId { get; set; }
    public string ReferralStatus { get; set; } = null!;

    public User Referee { get; set; } = null!;
    public User Referrer { get; set; } = null!;
}