using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class ReferralService(IReferralRepository referralRepository) : IReferralService
{
    private readonly IReferralRepository _referralRepository = referralRepository;

    public async Task<bool> CreateReferralRecordAsync(ReferralRecordRequest request)
    {
        var referrer = await _referralRepository.GetUserByReferralCodeAsync(request.ReferralCode);

        if (referrer == null)
        {
            return false;
        }

        var requestData = new ReferralRecord
        {
            RefereeId = request.RefereeId,
            ReferrerId = referrer.Id,
            ReferralStatus = "Pending"
        };

        return await _referralRepository.SaveReferralRecordAsync(requestData);
    }


    public async Task<ReferralRecordResponse> GetReferralRecordsAsync(int userId)
    {
        var recordsData = await _referralRepository.GetReferralRecordsAsync(userId);

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
}
