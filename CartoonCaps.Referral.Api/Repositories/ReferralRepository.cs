using CartoonCaps.Referral.Api.Models.Data;

namespace CartoonCaps.Referral.Api.Repositories;

public class ReferralRepository : IReferralRepository
{
    public Task<string?> GetCodeAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ReferralDataRecord>?> GetReferralRecordsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveCodeAsync(string userId, string code)
    {
        throw new NotImplementedException();
    }

    public Task SaveReferralRecordAsync(CreateReferralRecordDataRequest createReferralRecordDataRequest)
    {
        throw new NotImplementedException();
    }
}