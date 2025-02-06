using System.Collections.ObjectModel;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Utilities;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class ReferralService(IReferralCodeGenerator referralCodeGenerator, IReferralRepository referralRepository) : IReferralService
{
    private readonly IReferralCodeGenerator _referralCodeGenerator = referralCodeGenerator;
    private readonly IReferralRepository _referralRepository = referralRepository;

    public async Task<string?> CreateCodeAsync(int userId)
    {
        var code = _referralCodeGenerator.GenerateCode();

        await _referralRepository.SaveCodeAsync(userId, code);

        return code;
    }

    public Task<string?> GetCodeAsync(int userId)
    {
        var code = _referralRepository.GetCodeAsync(userId);
        return code;
    }

    public async Task CreateReferralRecordAsync(ReferralRecordRequest request)
    {
        var referrer = await _referralRepository.GetUserByReferralCodeAsync(request.ReferralCode);

        var requestData = new ReferralRecord
        {
            RefereeId = request.RefereeId,
            ReferrerId = referrer.Id,
            ReferralStatus = "Pending"
        };

        await _referralRepository.SaveReferralRecordAsync(requestData);
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
