using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="FeatureToggleRepository"/>. The seeded <c>timeline</c> toggle is only read;
/// the tests that change a toggle use a row of their own.
/// </summary>
public class FeatureToggleRepositoryTests
    : IClassFixture<DatabaseFixture<FeatureToggleRepositoryTests>>
{
    private readonly DatabaseFixture<FeatureToggleRepositoryTests> _fixture;
    private readonly FeatureToggleRepository _repository;

    public FeatureToggleRepositoryTests(DatabaseFixture<FeatureToggleRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _repository = new FeatureToggleRepository(
            fixture.Connection,
            NullLogger<FeatureToggleRepository>.Instance);
    }

    [Fact]
    public async Task GetAllAsync_SeededDatabase_ContainsTimelineForAdmins()
    {
        // Act
        var toggles = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        var timeline = toggles.Should().ContainSingle(t => t.Key == FeatureKeys.Timeline).Subject;
        timeline.Mode.Should().Be(FeatureToggleMode.Admins);
        timeline.Description.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(FeatureToggleMode.Disabled)]
    [InlineData(FeatureToggleMode.Everyone)]
    [InlineData(FeatureToggleMode.Admins)]
    public async Task SetModeAsync_ExistingFeature_StoresTheMode(FeatureToggleMode mode)
    {
        // Arrange
        var key = await InsertFeatureAsync();

        // Act
        var updated = await _repository.SetModeAsync(key, mode, TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeTrue();
        var toggles = await _repository.GetAllAsync(TestContext.Current.CancellationToken);
        toggles.Single(t => t.Key == key).Mode.Should().Be(mode);
    }

    [Fact]
    public async Task SetModeAsync_UnknownFeature_ReturnsFalse()
    {
        // Act
        var updated = await _repository.SetModeAsync("no-such-feature", FeatureToggleMode.Everyone, TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeFalse();
    }

    [Fact]
    public async Task SetModeAsync_ChangesOnlyTheGivenFeature()
    {
        // Arrange
        var first = await InsertFeatureAsync();
        var second = await InsertFeatureAsync();

        // Act
        await _repository.SetModeAsync(first, FeatureToggleMode.Everyone, TestContext.Current.CancellationToken);

        // Assert
        var toggles = await _repository.GetAllAsync(TestContext.Current.CancellationToken);
        toggles.Single(t => t.Key == second).Mode.Should().Be(FeatureToggleMode.Disabled);
        toggles.Single(t => t.Key == FeatureKeys.Timeline).Mode.Should().Be(FeatureToggleMode.Admins);
    }

    [Fact]
    public async Task Table_RejectsAModeThatIsNotKnown()
    {
        // Act
        var act = async () => await _fixture.Connection.ExecuteAsync(
            "INSERT INTO feature_toggles (feature_key, mode) VALUES ('bad-mode', 'sometimes')");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    private async Task<string> InsertFeatureAsync()
    {
        var key = $"test-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO feature_toggles (feature_key, description) VALUES (@Key, 'Test feature')",
            new { Key = key });
        return key;
    }
}
