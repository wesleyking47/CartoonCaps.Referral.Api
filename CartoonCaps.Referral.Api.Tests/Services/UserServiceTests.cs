using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Models;
using CartoonCaps.Referral.Api.Repositories;
using CartoonCaps.Referral.Api.Services;
using CartoonCaps.Referral.Api.Tests.Attributes;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Services;

public class UserServiceTests
{
    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenValidateUserId_ThenReturnTrue(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        string userId,
        UserService service,
        UserData userData
    )
    {
        userRepositoryMock.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(userData);

        var result = await service.ValidateUserIdAsync(userId);

        Assert.True(result);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenNullUser_WhenValidateUserId_ThenReturnFalse(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        string userId,
        UserService service
    )
    {
        userRepositoryMock.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(null as UserData);

        var result = await service.ValidateUserIdAsync(userId);

        Assert.False(result);
    }
}