using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralRepository : IReferralRepository
{
    public Task<string?> GetCodeAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserIdByReferralCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveCodeAsync(string userId, string code)
    {
        throw new NotImplementedException();
    }

    public Task SaveReferralRecordAsync(ReferralRecord referralDataRecord)
    {
        throw new NotImplementedException();
    }
}