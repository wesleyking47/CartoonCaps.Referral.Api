using System.Data.Common;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace CartoonCaps.Referral.Tests.Infrastructure.Repositories;

public class ReferralRepositoryTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ReferralContext> _contextOptions;

    public ReferralRepositoryTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Unittests");

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<ReferralContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new ReferralContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW Referrals AS
                SELECT RefereeId, ReferrerId, ReferralStatus
                FROM ReferralRecords";
            viewCommand.ExecuteNonQuery();
        }

        context.Users.AddRange(
            new User { Id = 1, Name = "John Doe", ReferralCode = "A" },
            new User { Id = 2, Name = "Jane Doe", ReferralCode = "B" },
            new User { Id = 3, Name = "Bob Smith", ReferralCode = "C" },
            new User { Id = 4, Name = "Alice Smith", ReferralCode = "D" }
        );

        context.ReferralRecords.AddRange(
            new ReferralRecord { ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" },
            new ReferralRecord { ReferrerId = 1, RefereeId = 3, ReferralStatus = "Pending" },
            new ReferralRecord { ReferrerId = 1, RefereeId = 4, ReferralStatus = "Pending" }
        );
        context.SaveChanges();
    }

    ReferralContext CreateContext() => new(_contextOptions);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }
    }

    [Fact]
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnRecords()
    {
        var userId = 1;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordsAsync(userId);

        var expectedReferralRecords = new List<ReferralRecord>
        {
            new() { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" },
            new() { Id = 2, ReferrerId = 1, RefereeId = 3, ReferralStatus = "Pending" },
            new() { Id = 3, ReferrerId = 1, RefereeId = 4, ReferralStatus = "Pending" }
        };
        result.ShouldNotBeNull();
        var resultList = result.ToList();
        resultList.Count.ShouldBe(3);
        foreach (var record in resultList)
        {
            record.ReferrerId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferrerId);
            record.RefereeId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].RefereeId);
            record.ReferralStatus.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferralStatus);
            record.Id.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].Id);
        }
    }

    [Fact]
    public async Task GivenAUserIdAndIncludeRefereesTrue_WhenGetReferralRecords_ThenReturnRecords()
    {
        var userId = 1;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordsAsync(userId, true);

        var expectedReferralRecords = new List<ReferralRecord>
        {
            new() { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" },
            new() { Id = 2, ReferrerId = 1, RefereeId = 3, ReferralStatus = "Pending" },
            new() { Id = 3, ReferrerId = 1, RefereeId = 4, ReferralStatus = "Pending" }
        };
        result.ShouldNotBeNull();
        var resultList = result.ToList();
        resultList.Count.ShouldBe(3);
        foreach (var record in resultList)
        {
            record.ReferrerId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferrerId);
            record.RefereeId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].RefereeId);
            record.ReferralStatus.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferralStatus);
            record.Id.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].Id);
            record.Referee.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task GivenAUserIdAndIncludeReferrersTrue_WhenGetReferralRecords_ThenReturnRecords()
    {
        var userId = 1;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordsAsync(userId, false, true);

        var expectedReferralRecords = new List<ReferralRecord>
        {
            new() { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" },
            new() { Id = 2, ReferrerId = 1, RefereeId = 3, ReferralStatus = "Pending" },
            new() { Id = 3, ReferrerId = 1, RefereeId = 4, ReferralStatus = "Pending" }
        };
        result.ShouldNotBeNull();
        var resultList = result.ToList();
        resultList.Count.ShouldBe(3);
        foreach (var record in resultList)
        {
            record.ReferrerId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferrerId);
            record.RefereeId.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].RefereeId);
            record.ReferralStatus.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].ReferralStatus);
            record.Id.ShouldBe(expectedReferralRecords[resultList.IndexOf(record)].Id);
            record.Referrer.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task GivenAUserId_WhenGetReferralRecords_ThenReturnEmpty()
    {
        var userId = 5;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordsAsync(userId);

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GivenAnExistingCode_WhenGetUserByReferralCode_ThenReturnUser()
    {
        var code = "A";
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetUserByReferralCodeAsync(code);

        result.ShouldNotBeNull();
        var expectedUser = new User
        {
            Id = 1,
            ReferralCode = "A",
            Name = "John Doe"
        };
        result.ShouldBeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GivenNonExistentCode_WhenGetUserByReferralCode_ThenReturnNull()
    {
        var code = "Z";
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetUserByReferralCodeAsync(code);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GivenReferralRecord_WhenSaveReferralRecord_ThenReferralRecordSaved()
    {
        var newUser = new User { Id = 5, Name = "New User", ReferralCode = "E" };
        var record = new ReferralRecord { ReferrerId = 1, RefereeId = 5, ReferralStatus = "Pending" };
        using var context = CreateContext();
        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        var repository = new ReferralRepository(context);

        var result = await repository.SaveReferralRecordAsync(record);

        result.ShouldBeTrue();
        var savedRecord = await context.ReferralRecords.FirstOrDefaultAsync(x => x.RefereeId == record.RefereeId);
        savedRecord.ShouldNotBeNull();
        savedRecord.Id.ShouldBe(4);
        savedRecord.ReferrerId.ShouldBe(record.ReferrerId);
        savedRecord.RefereeId.ShouldBe(record.RefereeId);
        savedRecord.ReferralStatus.ShouldBe(record.ReferralStatus);
    }

    [Fact]
    public async Task GivenReferralRecord_WhenUpdateReferralRecord_ThenReferralRecordUpdated()
    {
        var record = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Accepted" };
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        await repository.UpdateReferralRecordAsync(record);

        var expectedReferralRecord = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Accepted" };
        var updatedRecord = await context.ReferralRecords.FirstOrDefaultAsync(x => x.Id == record.Id);
        updatedRecord.ShouldBeEquivalentTo(expectedReferralRecord);
    }

    [Fact]
    public async Task GivenNoChangesForReferralRecord_WhenUpdateReferralRecord_ThenReturnFalse()
    {
        var record = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" };
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        await repository.UpdateReferralRecordAsync(record);

        var expectedReferralRecord = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" };
        var updatedRecord = await context.ReferralRecords.FirstOrDefaultAsync(x => x.Id == record.Id);
        updatedRecord.ShouldBeEquivalentTo(expectedReferralRecord);
    }

    [Fact]
    public async Task GivenReferralRecord_WhenDeleteReferralRecord_ThenReferralRecordDeleted()
    {
        var record = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" };
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        await repository.DeleteReferralRecordAsync(record);

        var deletedRecord = await context.ReferralRecords.FirstOrDefaultAsync(x => x.Id == record.Id);
        deletedRecord.ShouldBeNull();
    }

    [Fact]
    public async Task GivenARefereeId_WhenGetReferralRecordByRefereeId_ThenReturnReferralRecord()
    {
        var refereeId = 2;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordByRefereeIdAsync(refereeId);

        var expectedReferralRecord = new ReferralRecord { Id = 1, ReferrerId = 1, RefereeId = 2, ReferralStatus = "Pending" };
        result.ShouldBeEquivalentTo(expectedReferralRecord);
    }

    [Fact]
    public async Task GivenANonExistentRefereeId_WhenGetReferralRecordByRefereeId_ThenReturnNull()
    {
        var refereeId = 5;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetReferralRecordByRefereeIdAsync(refereeId);

        result.ShouldBeNull();
    }
}