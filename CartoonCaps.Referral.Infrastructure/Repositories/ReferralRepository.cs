using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralRepository(ReferralContext context) : IReferralRepository
{
    private readonly ReferralContext _context = context;

    public Task<string?> GetCodeAsync(int userId)
    {
        return Task.FromResult<string?>(userId.ToString());
    }

    public async Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(int userId)
    {
        var records = await _context.ReferralRecords.Where(x => x.ReferrerId == userId).ToListAsync();

        return records;
    }

    public Task<User> GetUserByReferralCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveCodeAsync(int userId, string code)
    {
        throw new NotImplementedException();
    }

    public Task SaveReferralRecordAsync(ReferralRecord referralDataRecord)
    {
        throw new NotImplementedException();
    }
}