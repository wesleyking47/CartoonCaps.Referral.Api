using CartoonCaps.Referral.Api.Controllers.v1;
using Moq;
using CartoonCaps.Referral.Api.Tests.Attributes;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Services;
using Shouldly;

namespace CartoonCaps.Referral.Tests.Api.Controllers.v1;

public class ReferralsControllerTests
{
    [Theory]
    [AutoControllerDomainData]
    public async Task GivenModelStateInvalid_WhenPost_ThenReturnBadRequest(
        CreateReferralRecordRequest referralRecordRequest,
        ReferralsController controller,
        string modelErrorKey,
        string modelErrorMessage
    )
    {
        controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

        var result = await controller.PostAsync(referralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.ShouldNotBeNull();
        var errors = badRequestResult.Value.ShouldBeOfType<SerializableError>();
        errors.ShouldNotBeNull();
        errors[modelErrorKey].ShouldBe(new[] { modelErrorMessage });
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenValidModelState_WhenPost_ThenReturnCreated(
        [Frozen] Mock<IReferralService> referralServiceMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.CreateReferralRecordAsync(referralRecordRequest)).ReturnsAsync(null as string);

        var result = await controller.PostAsync(referralRecordRequest);

        var createdResult = result.ShouldBeOfType<CreatedResult>();
        createdResult.ShouldNotBeNull();
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAnErrorMessage_WhenPost_ThenReturnBadRequest(
        [Frozen] Mock<IReferralService> referralServiceMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralsController controller,
        string errorMessage
    )
    {
        referralServiceMock.Setup(x => x.CreateReferralRecordAsync(referralRecordRequest)).ReturnsAsync(errorMessage);

        var result = await controller.PostAsync(referralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.Value.ShouldBe(errorMessage);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenNullRecords_WhenGet_ThenReturnNotFound(
        [Frozen] Mock<IReferralService> referralServiceMock,
        int userId,
        ReferralsController controller,
        ReferralRecordResponse referralRecordResponse
    )
    {
        referralRecordResponse.ReferralRecords = null;
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecordResponse);

        var result = await controller.GetAsync(userId);

        var notFoundResult = result.Result.ShouldBeOfType<NotFoundObjectResult>();
        notFoundResult.Value.ShouldBe("Referral details not found.");
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenRecords_WhenGet_ThenReturnOk(
        [Frozen] Mock<IReferralService> referralServiceMock,
        int userId,
        ReferralsController controller,
        ReferralRecordResponse referralRecordResponse
    )
    {
        referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecordResponse);

        var result = await controller.GetAsync(userId);

        result.Value.ShouldNotBeNull();
        result.Value.ReferralRecords.ShouldBe(referralRecordResponse.ReferralRecords);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenValidUpdateRequest_WhenPut_ThenReturnNoContent(
        [Frozen] Mock<IReferralService> referralServiceMock,
        UpdateReferralRecordRequest updateReferralRecordRequest,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.UpdateReferralRecordAsync(updateReferralRecordRequest)).ReturnsAsync(null as string);

        var result = await controller.PutAsync(updateReferralRecordRequest);

        var noContentResult = result.ShouldBeOfType<NoContentResult>();
        noContentResult.ShouldNotBeNull();
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAnInvalidModelState_WhenPut_ThenReturnBadRequest(
        UpdateReferralRecordRequest updateReferralRecordRequest,
        ReferralsController controller,
        string modelErrorKey,
        string modelErrorMessage
    )
    {
        controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

        var result = await controller.PutAsync(updateReferralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        var errors = badRequestResult.Value.ShouldBeOfType<SerializableError>();
        errors[modelErrorKey].ShouldBe(new[] { modelErrorMessage });
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAnInvalidUpdateRequest_WhenPut_ThenReturnBadRequest(
        [Frozen] Mock<IReferralService> referralServiceMock,
        UpdateReferralRecordRequest updateReferralRecordRequest,
        ReferralsController controller,
        string errorMessage
    )
    {
        referralServiceMock.Setup(x => x.UpdateReferralRecordAsync(updateReferralRecordRequest)).ReturnsAsync(errorMessage);

        var result = await controller.PutAsync(updateReferralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.Value.ShouldBe(errorMessage);
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenADeleteRequest_WhenDelete_ThenReturnNoContent(
        [Frozen] Mock<IReferralService> referralServiceMock,
        DeleteReferralRecordRequest deleteReferralRecordRequest,
        ReferralsController controller
    )
    {
        referralServiceMock.Setup(x => x.DeleteReferralRecordAsync(deleteReferralRecordRequest)).ReturnsAsync(null as string);

        var result = await controller.DeleteAsync(deleteReferralRecordRequest);

        var noContentResult = result.ShouldBeOfType<NoContentResult>();
        noContentResult.ShouldNotBeNull();
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAnInvalidModelState_WhenDelete_ThenReturnBadRequest(
        DeleteReferralRecordRequest deleteReferralRecordRequest,
        ReferralsController controller,
        string modelErrorKey,
        string modelErrorMessage
    )
    {
        controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

        var result = await controller.DeleteAsync(deleteReferralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        var errors = badRequestResult.Value.ShouldBeOfType<SerializableError>();
        errors[modelErrorKey].ShouldBe(new[] { modelErrorMessage });
    }

    [Theory]
    [AutoControllerDomainData]
    public async Task GivenAnInvalidDeleteRequest_WhenDelete_ThenReturnBadRequest(
        [Frozen] Mock<IReferralService> referralServiceMock,
        DeleteReferralRecordRequest deleteReferralRecordRequest,
        ReferralsController controller,
        string errorMessage
    )
    {
        referralServiceMock.Setup(x => x.DeleteReferralRecordAsync(deleteReferralRecordRequest)).ReturnsAsync(errorMessage);

        var result = await controller.DeleteAsync(deleteReferralRecordRequest);

        var badRequestResult = result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.Value.ShouldBe(errorMessage);
    }
}