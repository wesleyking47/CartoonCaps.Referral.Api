using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Application.Dtos;

public class ReferralRecordDto
{
    public ReferralRecordDto() { }

    public ReferralRecordDto(ReferralRecord referralRecord)
    {
        UserId = referralRecord.UserId;
        RefereeName = referralRecord.RefereeName;
        ReferralStatus = referralRecord.ReferralStatus;
    }

    public string UserId { get; set; } = string.Empty;
    public string RefereeName { get; set; } = string.Empty;
    public string ReferralStatus { get; set; } = string.Empty;
    public string ReferralCode { get; set; } = string.Empty;
}