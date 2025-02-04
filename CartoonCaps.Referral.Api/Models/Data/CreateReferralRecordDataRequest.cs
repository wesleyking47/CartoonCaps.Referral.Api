namespace CartoonCaps.Referral.Api.Models.Data;

public class CreateReferralRecordDataRequest
{
    public string ReferredUserId { get; set; } = string.Empty;

    public string ReferringUserId { get; set; } = string.Empty;
}