using System.Data.Common;
using CartoonCaps.Referral.Domain.Entities;
using CartoonCaps.Referral.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace CartoonCaps.Referral.Tests.Application.Infrastructure.Repositories;

public class ReferralRepositoryTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ReferralContext> _contextOptions;

    public ReferralRepositoryTests()
    {
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
        result.ShouldBeEquivalentTo(expectedReferralRecords);
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
    public async Task GivenAUserId_WhenGetCode_ThenReturnCode()
    {
        var userId = 1;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetCodeAsync(userId);

        result.ShouldBe("A");
    }

    [Fact]
    public async Task GivenAUserId_WhenGetCode_ThenReturnNull()
    {
        var userId = 5;
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.GetCodeAsync(userId);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GivenAnExistentUserIdAndNewCode_WhenSaveCode_ThenSaveCode()
    {
        var userId = 1;
        var code = "Z";
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.SaveCodeAsync(userId, code);

        result.ShouldBeTrue();
        var user = await context.Users.SingleOrDefaultAsync(x => x.Id == userId);
        user.ShouldNotBeNull();
        user.ReferralCode.ShouldBe(code);
    }

    [Fact]
    public async Task GivenNonExistentAUserIdAndCode_WhenSaveCode_ThenReturnFalse()
    {
        var userId = 10;
        var code = "Z";
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.SaveCodeAsync(userId, code);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenAUserIdAndSameCode_WhenSaveCode_ThenReturnFalse()
    {
        var userId = 1;
        var code = "A";
        using var context = CreateContext();
        var repository = new ReferralRepository(context);

        var result = await repository.SaveCodeAsync(userId, code);

        result.ShouldBeFalse();
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
}