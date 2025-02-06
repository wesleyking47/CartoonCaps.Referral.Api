namespace CartoonCaps.Referral.Application.Dtos;

public class ReferralRecordRequest
{
    public int RefereeId { get; set; }
    public string ReferralCode { get; set; } = null!;
}