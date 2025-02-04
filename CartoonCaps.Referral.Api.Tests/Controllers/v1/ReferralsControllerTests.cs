using CartoonCaps.Referral.Api.Controllers.v1;
using CartoonCaps.Referral.Api.Services;
using Moq;
using CartoonCaps.Referral.Api.Tests.Attributes;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CartoonCaps.Referral.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace CartoonCaps.Referral.Api.Tests.Controllers.v1
{
    public class ReferralsControllerTests
    {
        [Theory]
        [AutoControllerDomainData]
        public async Task GivenAUserId_WhenCreateCode_ThenReturnCreated(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string code
        )
        {
            referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).ReturnsAsync(code);

            var result = await controller.CreateCode(userId);

            Assert.IsType<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal("GetCode", createdResult.ActionName);
            Assert.Equal(userId, createdResult.RouteValues?["userId"]);
            Assert.Equal(code, createdResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenArgumentException_WhenCreateCode_ThenReturnBadRequest(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string exceptionMessage
        )
        {
            referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).Throws(new ArgumentException(exceptionMessage));

            var result = await controller.CreateCode(userId);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenInvalidOperationException_WhenCreateCode_ThenReturnConflict(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string exceptionMessage
        )
        {
            referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).Throws(new InvalidOperationException(exceptionMessage));

            var result = await controller.CreateCode(userId);

            Assert.IsType<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(exceptionMessage, conflictResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenException_WhenCreateCode_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.CreateCodeAsync(userId)).Throws<Exception>();

            var result = await controller.CreateCode(userId);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while creating the referral code.", objectResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenNullCode_WhenGetCode_ThenReturnNotFound(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(null as string);

            var result = await controller.GetCode(userId);

            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Referral code not found.", notFoundResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenCodeExists_WhenGetCode_ThenReturnOk(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string code
        )
        {
            referralServiceMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(code);

            var result = await controller.GetCode(userId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(code, okResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenExceptionThrown_WhenGetCode_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetCodeAsync(userId)).Throws<Exception>();

            var result = await controller.GetCode(userId);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while getting the referral code.", objectResult.Value);
        }


        [Theory]
        [AutoControllerDomainData]
        public async Task GivenNullRecords_WhenGetReferralRecords_ThenReturnNotFound(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralRecord>);

            var result = await controller.GetReferralRecords(userId);

            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Referral details not found.", notFoundResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenRecords_WhenGetReferralRecords_ThenReturnOk(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            IEnumerable<ReferralRecord> referralRecords
        )
        {
            referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecords);

            var result = await controller.GetReferralRecords(userId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equivalent(referralRecords, okResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenExceptionThrown_WhenGetReferralRecords_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetReferralRecordsAsync(userId)).Throws<Exception>();

            var result = await controller.GetReferralRecords(userId);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while getting the referral details.", objectResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenModelStateInvalid_WhenCreateReferralRecord_ThenReturnBadRequest(
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller,
            string modelErrorKey,
            string modelErrorMessage
        )
        {
            controller.ModelState.AddModelError(modelErrorKey, modelErrorMessage);

            var result = await controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            var errors = badRequestResult.Value as SerializableError;
            Assert.NotNull(errors);
            Assert.Equal(new[] { modelErrorMessage }, errors[modelErrorKey]);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenValidModelState_WhenCreateReferralRecord_ThenReturnCreated(
            [Frozen] Mock<IReferralService> referralServiceMock,
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.CreateReferralRecordAsync(createReferralRecordRequest)).Returns(Task.CompletedTask);

            var result = await controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<CreatedResult>(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
        }

        [Theory]
        [AutoControllerDomainData]
        public async Task GivenExceptionThrown_WhenCreateReferralRecord_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.CreateReferralRecordAsync(createReferralRecordRequest)).Throws<Exception>();

            var result = await controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while creating the referral record.", objectResult.Value);
        }

    }
}