using CartoonCaps.Referral.Api.Models;

namespace CartoonCaps.Referral.Api.Services;

public interface IReferralService
{
    Task<string?> CreateCodeAsync(string userId);
    Task<string?> GetCodeAsync(string userId);
    Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(string userId);
    Task CreateReferralRecordAsync(CreateReferralRecordRequest record);
}
