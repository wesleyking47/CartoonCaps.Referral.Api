using CartoonCaps.Referral.Api.Models;

namespace CartoonCaps.Referral.Api.Repositories;

public class UserRepository : IUserRepository
{
    public Task<UserData?> GetUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}