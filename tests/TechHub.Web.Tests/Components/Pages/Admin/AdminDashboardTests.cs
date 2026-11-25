using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminDashboard.razor — job history filters kept in the query string, prerender
/// restore, and the ordering and superseding of job history requests.
/// </summary>
public class AdminDashboardTests : AdminPageTestBase
{
    private static readonly ContentProcessingJobFilter _runningFilter = new(Status: ContentProcessingJobStatus.Running);
    private static readonly ContentProcessingJobFilter _failedFilter = new(Status: ContentProcessingJobStatus.Failed);

    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminDashboardTests()
    {
        Services.AddSingleton(_api.Object);
        _api.Setup(x => x.GetProcessingJobsAsync(It.IsAny<int>(), It.IsAny<ContentProcessingJobFilter?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    }

    [Fact]
    public void Navigate_ToDashboardWithoutQuery_ClearsActiveFilterAndReloads()
    {
        // Arrange
        var navigation = Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo("/admin?status=failed");
        _api.Setup(x => x.GetProcessingJobsAsync(20, It.Is<ContentProcessingJobFilter?>(f => f != null && !f.IsActive), It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateJob(1, ContentProcessingJobStatus.Completed)]);
        var cut = Render<AdminDashboard>();
        cut.Find("#jobStatusFilter").GetAttribute("value").Should().Be(ContentProcessingJobStatus.Failed);

        // Act
        navigation.NavigateTo("/admin");

        // Assert
        cut.WaitForAssertion(() =>
        {
            cut.Find("#jobStatusFilter").GetAttribute("value").Should().BeEmpty();
            cut.FindAll("tbody tr").Should().ContainSingle();
        });
        cut.FindAll(".admin-filter-actions").Should().BeEmpty();
    }

    [Fact]
    public void Render_RestoredFilteredState_MakesNoApiCalls()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin?status=failed");
        PersistentState.Persist<IReadOnlyList<ContentProcessingJob>>("admin-dashboard-jobs", [CreateJob(3, ContentProcessingJobStatus.Failed)]);
        PersistentState.Persist("admin-dashboard-has-running-job", false);

        // Act
        var cut = Render<AdminDashboard>();

        // Assert
        cut.FindAll("tbody tr").Should().ContainSingle();
        _api.Verify(
            x => x.GetProcessingJobsAsync(It.IsAny<int>(), It.IsAny<ContentProcessingJobFilter?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void Render_FilteredView_ChecksRunningJobsBeforeLoadingHistory()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin?status=failed");
        var calls = new List<ContentProcessingJobFilter?>();
        _api.Setup(x => x.GetProcessingJobsAsync(It.IsAny<int>(), It.IsAny<ContentProcessingJobFilter?>(), It.IsAny<CancellationToken>()))
            .Callback<int, ContentProcessingJobFilter?, CancellationToken>((_, filter, _) => calls.Add(filter))
            .ReturnsAsync([]);

        // Act
        Render<AdminDashboard>();

        // Assert
        calls.Should().Equal(_runningFilter, _failedFilter);
    }

    [Fact]
    public void Refresh_OlderResponseArrivesLast_KeepsNewestResults()
    {
        // Arrange
        var olderResponse = new TaskCompletionSource<IReadOnlyList<ContentProcessingJob>>();
        var newerResponse = new TaskCompletionSource<IReadOnlyList<ContentProcessingJob>>();
        _api.SetupSequence(x => x.GetProcessingJobsAsync(20, It.IsAny<ContentProcessingJobFilter?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([])
            .Returns(olderResponse.Task)
            .Returns(newerResponse.Task);
        var cut = Render<AdminDashboard>();

        // Act
        ClickRefresh(cut);
        ClickRefresh(cut);
        cut.InvokeAsync(() => newerResponse.SetResult([CreateJob(2, ContentProcessingJobStatus.Completed)]));
        cut.InvokeAsync(() => olderResponse.SetResult([CreateJob(1, ContentProcessingJobStatus.Failed), CreateJob(0, ContentProcessingJobStatus.Failed)]));

        // Assert
        cut.WaitForAssertion(() =>
        {
            var rows = cut.FindAll("tbody tr");
            rows.Should().ContainSingle();
            rows[0].TextContent.Should().Contain(ContentProcessingJobStatus.Completed);
        });
        cut.FindAll(".admin-loading").Should().BeEmpty();
    }

    private static void ClickRefresh(IRenderedComponent<AdminDashboard> cut) =>
        cut.FindAll("button").Single(b => b.TextContent.Contains("Refresh", StringComparison.Ordinal)).Click();

    private static ContentProcessingJob CreateJob(long id, string status) => new()
    {
        Id = id,
        Status = status,
        StartedAt = DateTimeOffset.UtcNow.AddMinutes(-id - 1),
        DurationMs = status == ContentProcessingJobStatus.Running ? null : 1000
    };
}
