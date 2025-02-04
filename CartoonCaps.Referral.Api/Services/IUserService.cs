namespace CartoonCaps.Referral.Api.Services;

public interface IUserService
{
    Task<bool> ValidateUserIdAsync(string userId);
    Task<string?> GetUserIdByReferralCodeAsync(string code);
}
