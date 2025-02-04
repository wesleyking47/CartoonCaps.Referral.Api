using CartoonCaps.Referral.Api.Controllers.v1;
using CartoonCaps.Referral.Api.Services;
using Moq;
using CartoonCaps.Referral.Api.Tests.Attributes;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CartoonCaps.Referral.Api.Models;


namespace CartoonCaps.Referral.Api.Tests.Controllers.v1
{
    public class ReferralsControllerTests
    {
        [Theory]
        [AutoControllerDomainData]
        public void GivenUserDoesNotExist_WhenCreateCode_ThenReturnBadRequest(
            [Frozen] Mock<IUserService> userServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            userServiceMock.Setup(x => x.Exists(userId)).Returns(false);

            var result = controller.CreateCode(userId);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("User does not exist.", badRequestResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenUserExistsAndCodeExists_WhenCreateCode_ThenReturnConflict(
            [Frozen] Mock<IUserService> userServiceMock,
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string code
        )
        {
            userServiceMock.Setup(x => x.Exists(userId)).Returns(true);
            referralServiceMock.Setup(x => x.GetCode(userId)).Returns(code);

            var result = controller.CreateCode(userId);

            Assert.IsType<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.Equal("Referral code already exists.", conflictResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenUserExistsAndCodeDoesNotExist_WhenCreateCode_ThenReturnCreated(
            [Frozen] Mock<IUserService> userServiceMock,
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string code
        )
        {
            userServiceMock.Setup(x => x.Exists(userId)).Returns(true);
            referralServiceMock.Setup(x => x.GetCode(userId)).Returns(null as string);
            referralServiceMock.Setup(x => x.CreateCode(userId)).Returns(code);

            var result = controller.CreateCode(userId);

            Assert.IsType<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal("GetCode", createdResult.ActionName);
            Assert.Equal(userId, createdResult.RouteValues?["userId"]);
            Assert.Equal(code, createdResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenCodeDoesNotExist_WhenGetCode_ThenReturnNotFound(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetCode(userId)).Returns(null as string);

            var result = controller.GetCode(userId);

            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Referral code not found.", notFoundResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenCodeExists_WhenGetCode_ThenReturnOk(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            string code
        )
        {
            referralServiceMock.Setup(x => x.GetCode(userId)).Returns(code);

            var result = controller.GetCode(userId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(code, okResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenExceptionThrown_WhenGetCode_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetCode(userId)).Throws<Exception>();

            var result = controller.GetCode(userId);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occured while getting the referral code.", objectResult.Value);
        }


        [Theory]
        [AutoControllerDomainData]
        public void GivenNullDetails_WhenGetDetails_ThenReturnNotFound(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetDetails(userId)).Returns(null as IEnumerable<ReferralDetails>);

            var result = controller.GetReferralDetails(userId);

            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Referral details not found.", notFoundResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenDetails_WhenGetStatus_ThenReturnOk(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller,
            IEnumerable<ReferralDetails> referralDetails
        )
        {
            referralServiceMock.Setup(x => x.GetDetails(userId)).Returns(referralDetails);

            var result = controller.GetReferralDetails(userId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(referralDetails, okResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenExceptionThrown_WhenGetDetails_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            string userId,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.GetDetails(userId)).Throws<Exception>();

            var result = controller.GetReferralDetails(userId);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occured while getting the referral details.", objectResult.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenInvalidCode_WhenCreateReferralRecord_ThenReturnNotFound(
            [Frozen] Mock<IReferralService> referralServiceMock,
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.ValidateCode(createReferralRecordRequest.ReferralCode)).Returns(false);

            var result = controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<BadRequestObjectResult>(result);
            var notFoundResult = result as BadRequestObjectResult;
            Assert.Equal("Invalid referral code.", notFoundResult?.Value);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenValidCode_WhenCreateReferralRecord_ThenReturnCreated(
            [Frozen] Mock<IReferralService> referralServiceMock,
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.ValidateCode(createReferralRecordRequest.ReferralCode)).Returns(true);

            var result = controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<CreatedResult>(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
        }

        [Theory]
        [AutoControllerDomainData]
        public void GivenExceptionThrown_WhenCreateReferralRecord_ThenReturnInternalServerError(
            [Frozen] Mock<IReferralService> referralServiceMock,
            CreateReferralRecordRequest createReferralRecordRequest,
            ReferralsController controller
        )
        {
            referralServiceMock.Setup(x => x.ValidateCode(createReferralRecordRequest.ReferralCode)).Throws<Exception>();

            var result = controller.CreateReferralRecord(createReferralRecordRequest);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occured while creating the referral record.", objectResult.Value);
        }
    }
}