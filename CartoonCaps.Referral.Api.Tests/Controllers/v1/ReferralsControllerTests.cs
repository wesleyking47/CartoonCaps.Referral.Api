using CartoonCaps.Referral.Api.Controllers.v1;
using CartoonCaps.Referral.Api.Services;
using Moq;
using CartoonCaps.Referral.Api.Tests.Attributes;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CartoonCaps.Referral.Api.Models;


namespace CartoonCaps.Referral.Api.Tests.Controllers.v1;

public class ReferralsControllerTests
{
    [Theory]
    [AutoControllerDomainData]
    public async Task GivenModelStateInvalid_WhenPost_ThenReturnBadRequest(
        CreateReferralRecordRequest createReferralRecordRequest,
        ReferralsController controller,
        string modelErrorKey,
        string modelErrorMessage
    )
    {
        controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

        var result = await controller.PostAsync(createReferralRecordRequest);

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
        CreateReferralRecordRequest createReferralRecordRequest,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.CreateReferralRecordAsync(createReferralRecordRequest)).Returns(Task.CompletedTask);

        var result = await controller.PostAsync(createReferralRecordRequest);

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
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralRecord>);

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
        IEnumerable<ReferralRecord> referralRecords
    )
    {
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecords);

        var result = await controller.GetAsync(userId);

        Assert.NotNull(result.Value);
        Assert.Equal(referralRecords, result.Value);
    }
}
