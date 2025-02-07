namespace CartoonCaps.Referral.Application.Dtos;

public class UpdateReferralRecordRequest
{
    public int RefereeId { get; set; }
    public string Status { get; set; } = null!;
}