using CartoonCaps.Referral.Api.Models.Data;

namespace CartoonCaps.Referral.Api.Repositories;

public interface IReferralRepository
{
    Task<bool> SaveCodeAsync(string userId, string code);
    Task<string?> GetCodeAsync(string userId);
    Task SaveReferralRecordAsync(CreateReferralRecordDataRequest createReferralRecordDataRequest);
    Task<IEnumerable<ReferralDataRecord>?> GetReferralRecordsAsync(string userId);
    Task<string?> GetUserIdByReferralCodeAsync(string code);
}