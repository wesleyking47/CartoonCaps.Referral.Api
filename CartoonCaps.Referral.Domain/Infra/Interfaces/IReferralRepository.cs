using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Domain.Infra.Interfaces;

public interface IReferralRepository
{
    Task<bool> SaveReferralRecordAsync(ReferralRecord referralDataRecord);
    Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(int userId, bool includeReferees = false, bool includeReferrers = false);
    Task UpdateReferralRecordAsync(ReferralRecord record);
    Task DeleteReferralRecordAsync(ReferralRecord record);
    Task<User?> GetUserByReferralCodeAsync(string code);
    Task<ReferralRecord?> GetReferralRecordByRefereeIdAsync(int refereeId);
}