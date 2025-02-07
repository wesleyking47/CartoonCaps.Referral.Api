using CartoonCaps.Referral.Application.Dtos;

namespace CartoonCaps.Referral.Application.Services;

public interface IReferralService
{
    Task<ReferralRecordResponse> GetReferralRecordsAsync(int userId);
    Task CreateReferralRecordAsync(ReferralRecordRequest request);
}
