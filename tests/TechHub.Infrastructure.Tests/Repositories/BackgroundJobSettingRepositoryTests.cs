using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="BackgroundJobSettingRepository"/>.
/// Seeded jobs (from migrations) are only read; toggling tests use their own uniquely named rows.
/// </summary>
public class BackgroundJobSettingRepositoryTests
    : IClassFixture<DatabaseFixture<BackgroundJobSettingRepositoryTests>>
{
    private readonly DatabaseFixture<BackgroundJobSettingRepositoryTests> _fixture;
    private readonly BackgroundJobSettingRepository _repository;

    public BackgroundJobSettingRepositoryTests(DatabaseFixture<BackgroundJobSettingRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _repository = new BackgroundJobSettingRepository(
            fixture.Connection,
            NullLogger<BackgroundJobSettingRepository>.Instance);
    }

    // ── GetAllAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_SeededJobs_AreReturnedOrderedByName()
    {
        // Arrange
        string[] seededJobs = ["ContentProcessor", "Newsletter", "RoundupGenerator"];

        // Act
        var settings = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        settings.Where(s => seededJobs.Contains(s.JobName)).Select(s => s.JobName).Should().Equal(seededJobs);
        settings.Should().NotContain(s => s.JobName == "ContentCleanup");
    }

    [Fact]
    public async Task GetAllAsync_AddedJob_IsIncludedWithAllFields()
    {
        // Arrange
        var jobName = await InsertJobAsync(enabled: true, description: "Included job");

        // Act
        var settings = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        var job = settings.Should().ContainSingle(s => s.JobName == jobName).Subject;
        job.Enabled.Should().BeTrue();
        job.Description.Should().Be("Included job");
    }

    // ── GetByNameAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetByNameAsync_SeededJob_ReturnsItsSettings()
    {
        // Act
        var setting = await _repository.GetByNameAsync("Newsletter", TestContext.Current.CancellationToken);

        // Assert
        setting.Should().NotBeNull();
        setting!.JobName.Should().Be("Newsletter");
        setting.Description.Should().Contain("Newsletter sending");
    }

    [Fact]
    public async Task GetByNameAsync_UnknownJob_ReturnsNull()
    {
        // Act
        var setting = await _repository.GetByNameAsync("NoSuchJob", TestContext.Current.CancellationToken);

        // Assert
        setting.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_DifferentCase_ReturnsNull()
    {
        // Act
        var setting = await _repository.GetByNameAsync("newsletter", TestContext.Current.CancellationToken);

        // Assert
        setting.Should().BeNull("job names are matched exactly");
    }

    // ── IsEnabledAsync ─────────────────────────────────────────────────────

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task IsEnabledAsync_ExistingJob_ReturnsStoredValue(bool enabled)
    {
        // Arrange
        var jobName = await InsertJobAsync(enabled);

        // Act
        var result = await _repository.IsEnabledAsync(jobName, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(enabled);
    }

    [Fact]
    public async Task IsEnabledAsync_UnknownJob_ReturnsFalse()
    {
        // Act
        var result = await _repository.IsEnabledAsync("NoSuchJob", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse("unknown jobs must never run");
    }

    // ── SetEnabledAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task SetEnabledAsync_DisabledJob_EnablesItAndReturnsTrue()
    {
        // Arrange
        var jobName = await InsertJobAsync(enabled: false, description: "Toggle me");

        // Act
        var changed = await _repository.SetEnabledAsync(jobName, true, TestContext.Current.CancellationToken);

        // Assert
        changed.Should().BeTrue();
        var setting = await _repository.GetByNameAsync(jobName, TestContext.Current.CancellationToken);
        setting!.Enabled.Should().BeTrue();
        setting.Description.Should().Be("Toggle me");
    }

    [Fact]
    public async Task SetEnabledAsync_EnabledJob_DisablesIt()
    {
        // Arrange
        var jobName = await InsertJobAsync(enabled: true);

        // Act
        var changed = await _repository.SetEnabledAsync(jobName, false, TestContext.Current.CancellationToken);

        // Assert
        changed.Should().BeTrue();
        (await _repository.IsEnabledAsync(jobName, TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task SetEnabledAsync_ValueAlreadySet_StillReturnsTrue()
    {
        // Arrange
        var jobName = await InsertJobAsync(enabled: true);

        // Act
        var changed = await _repository.SetEnabledAsync(jobName, true, TestContext.Current.CancellationToken);

        // Assert
        changed.Should().BeTrue("the job exists, so the update matches a row");
        (await _repository.IsEnabledAsync(jobName, TestContext.Current.CancellationToken)).Should().BeTrue();
    }

    [Fact]
    public async Task SetEnabledAsync_UnknownJob_ReturnsFalseAndDoesNotCreateRow()
    {
        // Act
        var changed = await _repository.SetEnabledAsync("NoSuchJob", true, TestContext.Current.CancellationToken);

        // Assert
        changed.Should().BeFalse();
        (await _repository.GetByNameAsync("NoSuchJob", TestContext.Current.CancellationToken)).Should().BeNull();
    }

    [Fact]
    public async Task SetEnabledAsync_OneJob_DoesNotAffectOtherJobs()
    {
        // Arrange
        var target = await InsertJobAsync(enabled: false);
        var other = await InsertJobAsync(enabled: false);

        // Act
        await _repository.SetEnabledAsync(target, true, TestContext.Current.CancellationToken);

        // Assert
        (await _repository.IsEnabledAsync(target, TestContext.Current.CancellationToken)).Should().BeTrue();
        (await _repository.IsEnabledAsync(other, TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private async Task<string> InsertJobAsync(bool enabled, string description = "Test job")
    {
        var jobName = $"TestJob-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO background_job_settings (job_name, enabled, description) VALUES (@JobName, @Enabled, @Description)",
            new { JobName = jobName, Enabled = enabled, Description = description });
        return jobName;
    }
}
