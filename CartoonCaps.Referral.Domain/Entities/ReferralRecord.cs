namespace CartoonCaps.Referral.Domain.Entities;

public class ReferralRecord(string userId, string refereeName, string referralStatus)
{
    public string UserId { get; set; } = userId;
    public string RefereeName { get; set; } = refereeName;
    public string ReferralStatus { get; set; } = referralStatus;
}