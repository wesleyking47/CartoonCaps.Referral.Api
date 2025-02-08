using System.ComponentModel.DataAnnotations;

namespace CartoonCaps.Referral.Application.Dtos;

public class UpdateReferralRecordRequest
{
    [Required]
    public int RefereeId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Status { get; set; } = null!;
}