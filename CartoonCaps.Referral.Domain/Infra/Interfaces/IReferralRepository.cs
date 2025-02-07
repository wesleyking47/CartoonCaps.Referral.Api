using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Domain.Infra.Interfaces;

public interface IReferralRepository
{
    Task<bool> SaveCodeAsync(int userId, string code);
    Task<string?> GetCodeAsync(int userId);
    Task SaveReferralRecordAsync(ReferralRecord referralDataRecord);
    Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(int userId);
    Task<User?> GetUserByReferralCodeAsync(string code);
}