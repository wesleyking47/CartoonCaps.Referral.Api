using System.ComponentModel.DataAnnotations;

namespace CartoonCaps.Referral.Application.Dtos;

public class DeleteReferralRecordRequest
{
    [Required]
    public int RefereeId { get; set; }
}