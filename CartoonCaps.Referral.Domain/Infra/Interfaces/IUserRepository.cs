using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Domain.Infra.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string userId);
}