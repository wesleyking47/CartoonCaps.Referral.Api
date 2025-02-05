using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Domain.Infra.Interfaces;

public interface IReferralRepository
{
    Task<bool> SaveCodeAsync(string userId, string code);
    Task<string?> GetCodeAsync(string userId);
    Task SaveReferralRecordAsync(ReferralRecord referralDataRecord);
    Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(string userId);
    Task<string?> GetUserIdByReferralCodeAsync(string code);
}