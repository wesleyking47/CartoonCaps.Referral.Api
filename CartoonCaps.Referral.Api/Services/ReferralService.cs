using CartoonCaps.Referral.Api.Models;
using CartoonCaps.Referral.Api.Models.Data;
using CartoonCaps.Referral.Api.Repositories;
using CartoonCaps.Referral.Api.Utilities;

namespace CartoonCaps.Referral.Api.Services;

public class ReferralService(IReferralCodeGenerator referralCodeGenerator, IReferralRepository referralRepository, IUserService userService) : IReferralService
{
    private readonly IReferralCodeGenerator _referralCodeGenerator = referralCodeGenerator;
    private readonly IReferralRepository _referralRepository = referralRepository;
    private readonly IUserService _userService = userService;

    public async Task<string> CreateCodeAsync(string userId)
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

    public async Task CreateReferralRecordAsync(CreateReferralRecordRequest record)
    {
        var referringUserId = await _userService.GetUserIdByReferralCodeAsync(record.ReferralCode);
        if (referringUserId == null)
        {
            throw new ArgumentException($"Invalid referral code: {record.ReferralCode}");
        }

        var isValidUserId = await _userService.ValidateUserIdAsync(record.UserId);
        if (!isValidUserId)
        {
            throw new ArgumentException($"Invalid user id: {record.UserId}");
        }

        var requestData = new CreateReferralRecordDataRequest
        {
            ReferringUserId = referringUserId,
            ReferredUserId = record.UserId
        };

        await _referralRepository.SaveReferralRecordAsync(requestData);
    }


    public async Task<IEnumerable<ReferralRecord>?> GetReferralRecordsAsync(string userId)
    {
        var recordsData = await _referralRepository.GetReferralRecordsAsync(userId);

        var records = recordsData?.Select(x =>
            new ReferralRecord
            {
                ReferryName = x.ReferryName,
                ReferralStatus = x.ReferralStatus
            }) ?? [];
        return records;
    }
}
