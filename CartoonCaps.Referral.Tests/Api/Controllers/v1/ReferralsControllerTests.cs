using CartoonCaps.Referral.Api.Controllers.v1;
using Moq;
using CartoonCaps.Referral.Api.Tests.Attributes;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Services;


namespace CartoonCaps.Referral.Api.Tests.Controllers.v1;

public class ReferralsControllerTests
{
    [Theory]
    [AutoControllerDomainData]
    public async Task GivenModelStateInvalid_WhenPost_ThenReturnBadRequest(
        ReferralRecordDto referralRecord,
        ReferralsController controller,
        string modelErrorKey,
        string modelErrorMessage
    )
    {
        controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

        var result = await controller.PostAsync(referralRecord);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult);
        var errors = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.NotNull(errors);
        Assert.Equal(new[] { modelErrorMessage }, errors[modelErrorKey]);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenValidModelState_WhenPost_ThenReturnCreated(
        [Frozen] Mock<IReferralService> referralServiceMock,
        ReferralRecordDto referralRecord,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.CreateReferralRecordAsync(referralRecord)).Returns(Task.CompletedTask);

        var result = await controller.PostAsync(referralRecord);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.NotNull(createdResult);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenNullRecords_WhenGet_ThenReturnNotFound(
        [Frozen] Mock<IReferralService> referralServiceMock,
        string userId,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralRecordDto>);

        var result = await controller.GetAsync(userId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Referral details not found.", notFoundResult.Value);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenRecords_WhenGet_ThenReturnOk(
        [Frozen] Mock<IReferralService> referralServiceMock,
        string userId,
        ReferralsController controller,
        IEnumerable<ReferralRecordDto> referralRecords
    )
    {
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecords);

        var result = await controller.GetAsync(userId);

        Assert.NotNull(result.Value);
        Assert.Equal(referralRecords, result.Value);
    }
}
