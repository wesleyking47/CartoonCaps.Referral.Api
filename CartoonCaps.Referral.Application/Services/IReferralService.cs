using CartoonCaps.Referral.Application.Dtos;

namespace CartoonCaps.Referral.Application.Services;

public interface IReferralService
{
    Task<string?> CreateCodeAsync(string userId);
    Task<string?> GetCodeAsync(string userId);
    Task<IEnumerable<ReferralRecordDto>?> GetReferralRecordsAsync(string userId);
    Task CreateReferralRecordAsync(ReferralRecordDto record);
}
