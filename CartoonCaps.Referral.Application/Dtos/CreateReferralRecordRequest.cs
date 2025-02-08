using System.ComponentModel.DataAnnotations;

namespace CartoonCaps.Referral.Application.Dtos;

public class CreateReferralRecordRequest
{
    [Required]
    public int RefereeId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string ReferralCode { get; set; } = null!;
}