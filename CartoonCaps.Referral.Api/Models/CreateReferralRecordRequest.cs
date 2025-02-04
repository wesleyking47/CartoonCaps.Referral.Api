using System.ComponentModel.DataAnnotations;

namespace CartoonCaps.Referral.Api.Models;

public class CreateReferralRecordRequest
{
    [Required]
    public string ReferralCode { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;
}