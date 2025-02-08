using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class ReferralService(IReferralRepository referralRepository) : IReferralService
{
    private readonly IReferralRepository _referralRepository = referralRepository;

    public async Task<string?> CreateReferralRecordAsync(CreateReferralRecordRequest request)
    {
        var referrer = await _referralRepository.GetUserByReferralCodeAsync(request.ReferralCode);

        if (referrer == null)
        {
            return "Invalid Referral Code";
        }

        var requestData = new ReferralRecord
        {
            RefereeId = request.RefereeId,
            ReferrerId = referrer.Id,
            ReferralStatus = "Pending"
        };

        var saved = await _referralRepository.SaveReferralRecordAsync(requestData);
        if (!saved)
        {
            return "No changes made";
        }

        return null;
    }

    public async Task<string?> DeleteReferralRecordAsync(DeleteReferralRecordRequest request)
    {
        var record = await _referralRepository.GetReferralRecordByRefereeIdAsync(request.RefereeId);
        if (record == null)
        {
            return "Invalid Referee Id";
        }

        await _referralRepository.DeleteReferralRecordAsync(record);
        return null;
    }

    public async Task<ReferralRecordResponse> GetReferralRecordsAsync(int userId)
    {
        var recordsData = await _referralRepository.GetReferralRecordsAsync(userId, true);

        var records = recordsData?.Select(x =>
            new ReferralRecordDto
            {
                ReferralStatus = x.ReferralStatus,
                RefereeName = x.Referee.Name
            }) ?? [];
        var response = new ReferralRecordResponse
        {
            ReferralRecords = [.. records.ToList()]
        };
        return response;
    }

    public async Task<string?> UpdateReferralRecordAsync(UpdateReferralRecordRequest request)
    {
        var record = await _referralRepository.GetReferralRecordByRefereeIdAsync(request.RefereeId);
        if (record == null)
        {
            return "Invalid Referee Id";
        }

        record.ReferralStatus = request.Status;
        await _referralRepository.UpdateReferralRecordAsync(record);
        return null;
    }
}
