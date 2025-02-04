namespace CartoonCaps.Referral.Api.Models;

public class CreateReferralRecordRequest
{
    public string ReferralCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}