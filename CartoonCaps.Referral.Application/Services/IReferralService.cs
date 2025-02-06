using CartoonCaps.Referral.Application.Dtos;

namespace CartoonCaps.Referral.Application.Services;

public interface IReferralService
{
    Task<string?> CreateCodeAsync(int userId);
    Task<string?> GetCodeAsync(int userId);
    Task<ReferralRecordResponse> GetReferralRecordsAsync(int userId);
    Task CreateReferralRecordAsync(ReferralRecordRequest request);
}
