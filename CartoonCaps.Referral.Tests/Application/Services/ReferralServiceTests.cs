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
        ReferralRecordRequest referralRecordRequest,
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
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnRecords(
        [Frozen] Mock<IReferralRepository> referralRepositoryMock,
        int userId,
        ReferralService service,
        IEnumerable<ReferralRecord> referralRecords
    )
    {
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(referralRecords);

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
        referralRepositoryMock.Setup(x => x.GetReferralRecordsAsync(userId)).ReturnsAsync(null as IEnumerable<ReferralRecord>);

        var result = await service.GetReferralRecordsAsync(userId);

        result.ShouldNotBeNull();
        result.ReferralRecords.ShouldBeEmpty();
    }
}