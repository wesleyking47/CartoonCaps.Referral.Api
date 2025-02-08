using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class ReferralRepository(ReferralContext context) : IReferralRepository
{
    private readonly ReferralContext _context = context;

    public async Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(int userId, bool includeReferees = false, bool includeReferrers = false)
    {
        var records = _context.ReferralRecords
            .Where(x => x.ReferrerId == userId);

        if (includeReferees)
        {
            records = records.Include(x => x.Referee);
        }

        if (includeReferrers)
        {
            records = records.Include(x => x.Referrer);
        }

        return await records.ToListAsync();
    }

    public async Task<User?> GetUserByReferralCodeAsync(string code)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.ReferralCode == code);

        return user;
    }

    public async Task<bool> SaveReferralRecordAsync(ReferralRecord referralDataRecord)
    {
        _context.ReferralRecords.Add(referralDataRecord);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task UpdateReferralRecordAsync(ReferralRecord record)
    {
        _context.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReferralRecordAsync(ReferralRecord record)
    {
        _context.Remove(record);
        await _context.SaveChangesAsync();
    }

    public async Task<ReferralRecord?> GetReferralRecordByRefereeIdAsync(int refereeId)
    {
        return await _context.ReferralRecords.SingleOrDefaultAsync(x => x.RefereeId == refereeId);
    }
}