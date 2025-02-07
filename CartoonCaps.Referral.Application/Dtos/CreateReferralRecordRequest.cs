namespace CartoonCaps.Referral.Application.Dtos;

public class CreateReferralRecordRequest
{
    public int RefereeId { get; set; }
    public string ReferralCode { get; set; } = null!;
}