using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Tests.Attributes;
using CartoonCaps.Referral.Application.Services;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Application.Services;

public class UserServiceTests
{
    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenValidateUserId_ThenReturnTrue(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        string userId,
        UserService service,
        User user
    )
    {
        userRepositoryMock.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(user);

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
        userRepositoryMock.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(null as User);

        var result = await service.ValidateUserIdAsync(userId);

        Assert.False(result);
    }
}