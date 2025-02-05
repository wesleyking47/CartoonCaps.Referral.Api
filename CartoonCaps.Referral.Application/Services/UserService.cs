using CartoonCaps.Referral.Domain.Infra.Interfaces;

namespace CartoonCaps.Referral.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> ValidateUserIdAsync(string userId)
    {
        var user = await _userRepository.GetUserAsync(userId);

        var userExists = user != null;
        return userExists;
    }
}
