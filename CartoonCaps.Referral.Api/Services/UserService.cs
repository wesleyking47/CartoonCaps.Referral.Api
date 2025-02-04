

using CartoonCaps.Referral.Api.Repositories;

namespace CartoonCaps.Referral.Api.Services;

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
