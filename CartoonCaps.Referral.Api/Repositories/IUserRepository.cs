using CartoonCaps.Referral.Api.Models;

namespace CartoonCaps.Referral.Api.Repositories;

public interface IUserRepository
{
    Task<UserData?> GetUserAsync(string userId);
}