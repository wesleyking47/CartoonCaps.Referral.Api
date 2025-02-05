
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User?> GetUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}