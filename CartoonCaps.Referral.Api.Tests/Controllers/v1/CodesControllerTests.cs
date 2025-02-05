using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Controllers.v1;
using CartoonCaps.Referral.Api.Services;
using CartoonCaps.Referral.Api.Tests.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Controllers.v1;

public class CodesControllerTests
{
    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAUserId_WhenPostAsync_ThenReturnCreated(
                [Frozen] Mock<IReferralService> referralServiceMock,
                string userId,
                CodesController controller,
                string code
            )
    {
        referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).ReturnsAsync(code);

        var result = await controller.PostAsync(userId);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetAsync", createdResult.ActionName);
        Assert.Equal(userId, createdResult.RouteValues?["userId"]);
        Assert.Equal(code, createdResult.Value);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenNullCode_WhenPostAsync_ThenReturnBadRequest(
        [Frozen] Mock<IReferralService> referralServiceMock,
        string userId,
        CodesController controller
    )
    {
        referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).ReturnsAsync(null as string);

        var result = await controller.PostAsync(userId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Code not created.", badRequestResult.Value);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenNullCode_WhenGetAsync_ThenReturnNotFound(
        [Frozen] Mock<IReferralService> referralServiceMock,
        string userId,
        CodesController controller
    )
    {
        referralServiceMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(null as string);

        var result = await controller.GetAsync(userId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Referral code not found.", notFoundResult.Value);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenCodeExists_WhenGetAsync_ThenReturnOk(
        [Frozen] Mock<IReferralService> referralServiceMock,
        string userId,
        CodesController controller,
        string code
    )
    {
        referralServiceMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(code);

        var result = await controller.GetAsync(userId);

        Assert.Equal(code, result.Value);
    }
}