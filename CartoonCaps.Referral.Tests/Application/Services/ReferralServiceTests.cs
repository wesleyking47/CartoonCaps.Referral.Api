using AutoFixture.Xunit2;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Services;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using CartoonCaps.Referral.Tests.Attributes;
using Moq;
using Shouldly;

namespace CartoonCaps.Referral.Tests.Application.Services;

public class ReferralServiceTests
{
    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenValidReferralCodeAndValidReferredUserId_WhenCreateReferralRecord_ThenSaveRecord(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralService service,
        User user
    )
    {
        referralRepositoryMock.Setup(x => x.GetUserByReferralCodeAsync(referralRecordRequest.ReferralCode)).ReturnsAsync(user);

        await service.CreateReferralRecordAsync(referralRecordRequest);

        var expectedReferralRecord = new ReferralRecord
        {
            RefereeId = referralRecordRequest.RefereeId,
            ReferrerId = user.Id,
            ReferralStatus = "Pending"
        };

        referralRepositoryMock.Verify(x => x.SaveReferralRecordAsync(
            It.Is<ReferralRecord>(r =>
                r.RefereeId == expectedReferralRecord.RefereeId &&
                r.ReferrerId == expectedReferralRecord.ReferrerId &&
                r.ReferralStatus == expectedReferralRecord.ReferralStatus)
        ), Times.Once);
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenNullUser_WhenCreateReferralRecord_ThenReturnErrorMessage(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetUserByReferralCodeAsync(referralRecordRequest.ReferralCode)).ReturnsAsync(null as User);

        var result = await service.CreateReferralRecordAsync(referralRecordRequest);

        result.ShouldBe("Invalid Referral Code");
        referralRepositoryMock.Verify(x => x.SaveReferralRecordAsync(It.IsAny<ReferralRecord>()), Times.Never);
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenNotSaved_WhenCreateReferralRecord_ThenReturnErrorMessage(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.SaveReferralRecordAsync(It.IsAny<ReferralRecord>())).ReturnsAsync(false);

        var result = await service.CreateReferralRecordAsync(referralRecordRequest);

        result.ShouldBe("No changes made");
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenRecordSaved_WhenCreateReferralRecord_ThenReturnNullErrorMessage(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        CreateReferralRecordRequest referralRecordRequest,
        ReferralService service,
        User referrer
    )
    {
        referralRepositoryMock.Setup(x => x.GetUserByReferralCodeAsync(referralRecordRequest.ReferralCode)).ReturnsAsync(referrer);
        referralRepositoryMock.Setup(x => x.SaveReferralRecordAsync(It.Is<ReferralRecord>(r =>
            r.RefereeId == referralRecordRequest.RefereeId
            && r.ReferrerId == referrer.Id
            && r.ReferralStatus == "Pending")))
            .ReturnsAsync(true);

        var result = await service.CreateReferralRecordAsync(referralRecordRequest);

        result.ShouldBeNull();
    }


    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnRecords(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        int userId,
        ReferralService service,
        IEnumerable<ReferralRecord> referralRecords
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId, true, false)).ReturnsAsync(referralRecords);

        var result = await service.GetReferralRecordsAsync(userId);

        var expectedReferralRecordResponse = new ReferralRecordResponse
        {
            ReferralRecords = [.. referralRecords.Select(x =>
                new ReferralRecordDto
                {
                    ReferralStatus = x.ReferralStatus,
                    RefereeName = x.Referee.Name
                }).ToList()]
        };
        result.ReferralRecords.ShouldBeEquivalentTo(expectedReferralRecordResponse.ReferralRecords);
    }

    [Theory]
    [AutoDomainData]
    public async Task GivenNoRecords_WhenGetReferralRecords_ThenAnEmptyList(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        int userId,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId, false, false)).ReturnsAsync(null as IEnumerable<ReferralRecord>);

        var result = await service.GetReferralRecordsAsync(userId);

        result.ShouldNotBeNull();
        result.ReferralRecords.ShouldBeEmpty();
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenAReferralRecordRequest_WhenUpdateReferralRecord_ThenCallUpdateReferralRecordAsync(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        UpdateReferralRecordRequest referralRecordRequest,
        ReferralService service,
        ReferralRecord referralRecord
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordByRefereeIdAsync(referralRecordRequest.RefereeId)).ReturnsAsync(referralRecord);

        var result = await service.UpdateReferralRecordAsync(referralRecordRequest);

        result.ShouldBeNull();
        referralRepositoryMock.Verify(x => x.UpdateReferralRecordAsync(It.Is<ReferralRecord>(r =>
            r.Id == referralRecord.Id
            && r.ReferralStatus == referralRecordRequest.Status
            && r.RefereeId == referralRecord.RefereeId
            && r.ReferrerId == referralRecord.ReferrerId)),
            Times.Once);
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenANullReferralRecord_WhenUpdateReferralRecord_ThenReturnErrorMessage(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        UpdateReferralRecordRequest referralRecordRequest,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordByRefereeIdAsync(referralRecordRequest.RefereeId)).ReturnsAsync(null as ReferralRecord);

        var result = await service.UpdateReferralRecordAsync(referralRecordRequest);

        result.ShouldBe("Invalid Referee Id");
        referralRepositoryMock.Verify(x => x.UpdateReferralRecordAsync(It.IsAny<ReferralRecord>()), Times.Never);
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenAReferralRecord_WhenDeleteReferralRecord_ThenCallDeleteReferralRecordAsync(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        DeleteReferralRecordRequest referralRecordRequest,
        ReferralService service,
        ReferralRecord referralRecord
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordByRefereeIdAsync(referralRecordRequest.RefereeId)).ReturnsAsync(referralRecord);

        await service.DeleteReferralRecordAsync(referralRecordRequest);

        referralRepositoryMock.Verify(x => x.DeleteReferralRecordAsync(It.Is<ReferralRecord>(r =>
            r.Id == referralRecord.Id
            && r.RefereeId == referralRecord.RefereeId
            && r.ReferrerId == referralRecord.ReferrerId)),
            Times.Once);
    }

    [Theory]
    [AutoOmitRecursionDomainData]
    public async Task GivenANullReferralRecord_WhenDeleteReferralRecord_ThenReturnFalse(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        DeleteReferralRecordRequest referralRecordRequest,
        ReferralService service
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordByRefereeIdAsync(referralRecordRequest.RefereeId)).ReturnsAsync(null as ReferralRecord);

        var result = await service.DeleteReferralRecordAsync(referralRecordRequest);

        result.ShouldBe("Invalid Referee Id");
        referralRepositoryMock.Verify(x => x.DeleteReferralRecordAsync(It.IsAny<ReferralRecord>()), Times.Never);
    }
}