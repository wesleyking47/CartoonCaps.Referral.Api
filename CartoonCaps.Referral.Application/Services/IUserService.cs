namespace CartoonCaps.Referral.Application.Services;

public interface IUserService
{
    Task<bool> ValidateUserIdAsync(string userId);
}
