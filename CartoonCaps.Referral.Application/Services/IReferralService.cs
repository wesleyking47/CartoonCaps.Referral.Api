using CartoonCaps.Referral.Application.Dtos;

namespace CartoonCaps.Referral.Application.Services;

public interface IReferralService
{
    Task<ReferralRecordResponse> GetReferralRecordsAsync(int userId);
    Task<string?> CreateReferralRecordAsync(CreateReferralRecordRequest request);
    Task<string?> UpdateReferralRecordAsync(UpdateReferralRecordRequest request);
    Task<string?> DeleteReferralRecordAsync(DeleteReferralRecordRequest request);
}
