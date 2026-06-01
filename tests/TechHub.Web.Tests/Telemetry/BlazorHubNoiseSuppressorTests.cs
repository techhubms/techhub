using System.Diagnostics;
using FluentAssertions;
using TechHub.Web.Telemetry;

namespace TechHub.Web.Tests.Telemetry;

public class BlazorHubNoiseSuppressorTests
{
    // ─── Failed ComponentHub activities are suppressed ───────────────────────

    [Fact]
    public void OnEnd_UpdateRootComponentsActivity_ErrorStatus_ClearsRecordedFlag()
    {
        var suppressor = new BlazorHubNoiseSuppressor();
        using var activity = CreateActivity(
            "Microsoft.AspNetCore.Components.Server.ComponentHub/UpdateRootComponents",
            ActivityStatusCode.Error);

        suppressor.OnEnd(activity);

        activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded)
            .Should().BeFalse("failed ComponentHub activities must not be exported to App Insights");
    }

    [Theory]
    [InlineData("Microsoft.AspNetCore.Components.Server.ComponentHub/UpdateRootComponents")]
    [InlineData("Microsoft.AspNetCore.Components.Server.ComponentHub/BeginInvokeDotNetFromJS")]
    [InlineData("Microsoft.AspNetCore.Components.Server.ComponentHub/EndInvokeJSFromDotNet")]
    [InlineData("Microsoft.AspNetCore.Components.Server.ComponentHub/DispatchBrowserEvent")]
    [InlineData("Microsoft.AspNetCore.Components.Server.ComponentHub")]
    public void OnEnd_AnyComponentHubActivity_ErrorStatus_ClearsRecordedFlag(string displayName)
    {
        var suppressor = new BlazorHubNoiseSuppressor();
        using var activity = CreateActivity(displayName, ActivityStatusCode.Error);

        suppressor.OnEnd(activity);

        activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded)
            .Should().BeFalse($"failed activity '{displayName}' must have Recorded flag cleared");
    }

    // ─── Successful ComponentHub activities pass through untouched ────────────

    [Theory]
    [InlineData(ActivityStatusCode.Ok)]
    [InlineData(ActivityStatusCode.Unset)]
    public void OnEnd_ComponentHubActivity_NonErrorStatus_LeavesRecordedFlagUnchanged(ActivityStatusCode statusCode)
    {
        var suppressor = new BlazorHubNoiseSuppressor();
        using var activity = CreateActivity(
            "Microsoft.AspNetCore.Components.Server.ComponentHub/UpdateRootComponents",
            statusCode);

        suppressor.OnEnd(activity);

        activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded)
            .Should().BeTrue("successful hub invocations must remain visible in App Insights");
    }

    // ─── Non-ComponentHub activities are left untouched ──────────────────────

    [Theory]
    [InlineData("GET /ai/videos/my-article")]
    [InlineData("POST /admin/login")]
    [InlineData("Microsoft.AspNetCore.Hosting.HttpRequestIn")]
    [InlineData("Microsoft.AspNetCore.SignalR.HubInvocation")]
    [InlineData("SomeOtherHub/UpdateRootComponents")]
    public void OnEnd_NonComponentHubActivity_LeavesRecordedFlagUnchanged(string displayName)
    {
        var suppressor = new BlazorHubNoiseSuppressor();
        using var activity = CreateActivity(displayName, ActivityStatusCode.Error); // even with Error status

        suppressor.OnEnd(activity);

        activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded)
            .Should().BeTrue($"activity '{displayName}' must not have its Recorded flag cleared");
    }

    [Fact]
    public void OnEnd_ActivityAlreadyNotRecorded_RemainsNotRecorded()
    {
        // Verifies the bitwise operation does not inadvertently flip flags on unrecorded
        // ComponentHub activities that were already suppressed upstream.
        var suppressor = new BlazorHubNoiseSuppressor();
        using var activity = new Activity(
            "Microsoft.AspNetCore.Components.Server.ComponentHub/UpdateRootComponents");

        activity.SetStatus(ActivityStatusCode.Error);
        activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded; // already not recorded

        suppressor.OnEnd(activity);

        activity.ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded).Should().BeFalse();
    }

    // ─── Helper ───────────────────────────────────────────────────────────────

    private static Activity CreateActivity(string displayName, ActivityStatusCode statusCode = ActivityStatusCode.Unset)
    {
        var activity = new Activity(displayName);
        activity.ActivityTraceFlags |= ActivityTraceFlags.Recorded;
        activity.Start();
        activity.SetStatus(statusCode);
        return activity;
    }
}
