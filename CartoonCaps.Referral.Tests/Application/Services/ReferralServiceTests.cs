using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Tests.Attributes;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Services;
using CartoonCaps.Referral.Application.Utilities;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Application.Services;

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
        ReferralRecordDto referralRecord,
        ReferralService service,
        string referringUserId
    )
    {
        userServiceMock.Setup(x => x.ValidateUserIdAsync(referralRecord.UserId)).ReturnsAsync(true);
        referralRepositoryMock.Setup(x => x.GetUserIdByReferralCodeAsync(referralRecord.ReferralCode)).ReturnsAsync(referringUserId);

        await service.CreateReferralRecordAsync(referralRecord);

        var expectedReferralRecordDto = new ReferralRecordDto
        {
            RefereeName = referralRecord.RefereeName,
            ReferralStatus = referralRecord.ReferralStatus,
            UserId = referralRecord.UserId
        };

        referralRepositoryMock.Verify(x => x.SaveReferralRecordAsync(
            It.Is<ReferralRecord>(r =>
                r.RefereeName == expectedReferralRecordDto.RefereeName
                && r.ReferralStatus == expectedReferralRecordDto.ReferralStatus
                && r.UserId == expectedReferralRecordDto.UserId)
        ), Times.Once);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnRecords(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        string userId,
        ReferralService service,
        IEnumerable<ReferralRecord> referralRecords
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecords);

        var result = await service.GetReferralRecordsAsync(userId);

        var expectedReferralRecords = referralRecords.Select(x =>
            new ReferralRecord(x.UserId, x.RefereeName, x.ReferralStatus));
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
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralRecord>);

        var result = await service.GetReferralRecordsAsync(userId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}