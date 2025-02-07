using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralRepository(ReferralContext context) : IReferralRepository
{
    private readonly ReferralContext _context = context;

    public async Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(int userId)
    {
        var records = await _context.ReferralRecords.Where(x => x.ReferrerId == userId).ToListAsync();

        return records;
    }

    public async Task<User?> GetUserByReferralCodeAsync(string code)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.ReferralCode == code);

        return user;
    }

    public async Task SaveReferralRecordAsync(ReferralRecord referralDataRecord)
    {
        _context.ReferralRecords.Add(referralDataRecord);

        await _context.SaveChangesAsync();
    }
}