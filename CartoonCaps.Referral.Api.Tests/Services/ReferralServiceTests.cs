using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Models;
using CartoonCaps.Referral.Api.Models.Data;
using CartoonCaps.Referral.Api.Repositories;
using CartoonCaps.Referral.Api.Services;
using CartoonCaps.Referral.Api.Tests.Attributes;
using CartoonCaps.Referral.Api.Utilities;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Services;

public class ReferralServiceTests
{
    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenCreateCode_ThenReturnCode(
        [Frozen] Mock<IUserService> userServiceMock,
        [Frozen] Mock<IReferralCodeGenerator> referralCodeGeneratorMock,
        string userId,
        ReferralService service,
        string code
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(userId)).ReturnsAsync(true);
        referralCodeGeneratorMock.Setup(x => x.GenerateCode()).Returns(code);

        var result = await service.CreateCodeAsync(userId);

        Assert.Equal(code, result);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenAUserIdAndCode_WhenCreateCode_ThenSaveCode(
        [Frozen] Mock<IUserService> userServiceMock,
        [Frozen] Mock<IReferralCodeGenerator> referralCodeGeneratorMock,
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        string code,
        ReferralService service
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(userId)).ReturnsAsync(true);
        referralCodeGeneratorMock.Setup(x => x.GenerateCode()).Returns(code);

        await service.CreateCodeAsync(userId);

        referralRepositoryMock.Verify(x => x.SaveCodeAsync(userId, code), Times.Once);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenUserAlreadyHasCode_WhenCreateCode_ThenThrowInvalidOperationException(
        [Frozen] Mock<IUserService> userServiceMock,
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        ReferralService service,
        string code
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(userId)).ReturnsAsync(true);
        referralRepositoryMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(code);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateCodeAsync(userId));
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenInvalidUserId_WhenCreateCode_ThenThrowArgumentException(
        [Frozen] Mock<IUserService> userServiceMock,
        string userId,
        ReferralService service
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(userId)).ReturnsAsync(false);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateCodeAsync(userId));
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenGetCode_ThenReturnCode(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        string code,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(code);

        var result = await service.GetCodeAsync(userId);

        Assert.Equal(code, result);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenNoCode_WhenGetCode_ThenReturnNull(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetCodeAsync(userId)).ReturnsAsync(null as string);

        var result = await service.GetCodeAsync(userId);

        Assert.Null(result);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenValidReferralCodeAndValidReferredUserId_WhenCreateReferralRecord_ThenSaveRecord(
        [Frozen] Mock<IUserService> userServiceMock,
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest request,
        ReferralService service,
        string referringUserId
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(request.UserId)).ReturnsAsync(true);
        referralRepositoryMock.Setup(x => x.GetUserIdByReferralCodeAsync(request.ReferralCode)).ReturnsAsync(referringUserId);

        await service.CreateReferralRecordAsync(request);

        var expectedCreateReferralRecordDataRequest = new CreateReferralRecordDataRequest
        {
            ReferringUserId = referringUserId,
            ReferredUserId = request.UserId
        };

        referralRepositoryMock.Verify(x => x.SaveReferralRecordAsync(
            It.Is<CreateReferralRecordDataRequest>(r =>
                r.ReferringUserId == expectedCreateReferralRecordDataRequest.ReferringUserId
                && r.ReferredUserId == expectedCreateReferralRecordDataRequest.ReferredUserId)
        ), Times.Once);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenInvalidReferralCode_WhenCreateReferralRecord_ThenThrowArgumentException(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest request,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetUserIdByReferralCodeAsync(request.ReferralCode)).ReturnsAsync(null as string);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateReferralRecordAsync(request));
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenAValidReferralCodeAndAnInvalidReferredUserId_WhenCreateReferralRecord_ThenThrowArgumentException(
        [Frozen] Mock<IUserService> userServiceMock,
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest request,
        ReferralService service,
        string referringUserId
    )
    {
        referralRepositoryMock.Setup(x => x.GetUserIdByReferralCodeAsync(request.ReferralCode)).ReturnsAsync(referringUserId);
        userServiceMock.Setup(x => x.ValidateUserIdAsync(request.UserId)).ReturnsAsync(false);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateReferralRecordAsync(request));
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnRecords(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        ReferralService service,
        IEnumerable<ReferralDataRecord> records
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(records);

        var result = await service.GetReferralRecordsAsync(userId);

        var expectedReferralRecords = records.Select(x =>
            new ReferralRecord
            {
                ReferralStatus = x.ReferralStatus,
                RefereeName = x.RefereeName
            });
        Assert.Equivalent(expectedReferralRecords, result);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenNoRecords_WhenGetReferralRecords_ThenAnEmptyList(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralDataRecord>);

        var result = await service.GetReferralRecordsAsync(userId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}