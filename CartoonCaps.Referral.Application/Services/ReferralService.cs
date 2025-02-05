using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Utilities;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class ReferralService(IReferralCodeGenerator referralCodeGenerator, IReferralRepository referralRepository, IUserService userService) : IReferralService
{
    private readonly IReferralCodeGenerator _referralCodeGenerator = referralCodeGenerator;
    private readonly IReferralRepository _referralRepository = referralRepository;
    private readonly IUserService _userService = userService;

    public async Task<string?> CreateCodeAsync(string userId)
    {
        var isValidUserId = await _userService.ValidateUserIdAsync(userId);
        if (!isValidUserId)
        {
            throw new ArgumentException($"Invalid user id: {userId}");
        }

        var existingCode = await _referralRepository.GetCodeAsync(userId);
        if (!string.IsNullOrWhiteSpace(existingCode))
        {
            throw new InvalidOperationException($"User already has a code");
        }

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
        var referringUserId = await _referralRepository.GetUserIdByReferralCodeAsync(record.ReferralCode);
        if (referringUserId == null)
        {
            throw new ArgumentException($"Invalid referral code: {record.ReferralCode}");
        }

        var isValidUserId = await _userService.ValidateUserIdAsync(record.UserId);
        if (!isValidUserId)
        {
            throw new ArgumentException($"Invalid user id: {record.UserId}");
        }

        var requestData = new ReferralRecord
        {
            RefereeName = record.RefereeName,
            ReferralStatus = record.ReferralStatus,
            UserId = record.UserId
        };

        await _referralRepository.SaveReferralRecordAsync(requestData);
    }


    public async Task<IEnumerable<ReferralRecordDto>?> GetReferralRecordsAsync(string userId)
    {
        var recordsData = await _referralRepository.GetReferralRecordsAsync(userId);

        var records = recordsData?.Select(x =>
            new ReferralRecordDto
            {
                RefereeName = x.RefereeName,
                ReferralStatus = x.ReferralStatus
            }) ?? [];
        return records;
    }
}
