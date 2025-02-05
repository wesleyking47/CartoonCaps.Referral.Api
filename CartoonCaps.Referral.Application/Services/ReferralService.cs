using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Utilities;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class ReferralService(IReferralCodeGenerator referralCodeGenerator, IReferralRepository referralRepository) : IReferralService
{
    private readonly IReferralCodeGenerator _referralCodeGenerator = referralCodeGenerator;
    private readonly IReferralRepository _referralRepository = referralRepository;

    public async Task<string?> CreateCodeAsync(string userId)
    {
        var code = _referralCodeGenerator.GenerateCode();

        await _referralRepository.SaveCodeAsync(userId, code);

        return code;
    }

    public Task<string?> GetCodeAsync(string userId)
    {
        var code = _referralRepository.GetCodeAsync(userId);
        return code;
    }

    public async Task CreateReferralRecordAsync(ReferralRecordDto record)
    {
        var requestData = new ReferralRecord(record.UserId, record.RefereeName, record.ReferralStatus);

        await _referralRepository.SaveReferralRecordAsync(requestData);
    }


    public async Task<IEnumerable<ReferralRecordDto>?> GetReferralRecordsAsync(string userId)
    {
        var recordsData = await _referralRepository.GetReferralRecordsAsync(userId);

        var records = recordsData?.Select(x =>
            new ReferralRecordDto(x)) ?? [];
        return records;
    }
}
