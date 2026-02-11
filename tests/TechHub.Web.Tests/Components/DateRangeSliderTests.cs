using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for DateRangeSlider.razor component
/// Validates rendering, defaults, presets, event callbacks, and parameter sync.
/// </summary>
public class DateRangeSliderTests : BunitContext
{
    public DateRangeSliderTests()
    {
        // DateRangeSlider imports a JS module for client-side clamping.
        // Use loose mode so the import call doesn't throw in test context.
        JSInterop.Mode = JSRuntimeMode.Loose;
    }
    [Fact]
    public void DateRangeSlider_RendersWithCorrectStructure()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - basic structure
        cut.Find("nav.sidebar-section").Should().NotBeNull();
        cut.Find("h2.sidebar-h2").TextContent.Should().Be("Date Range");
        cut.Find(".date-range-slider").Should().NotBeNull();
        cut.Find(".date-range-display").Should().NotBeNull();
        cut.Find(".slider-container").Should().NotBeNull();
    }

    [Fact]
    public void DateRangeSlider_RendersTwoSliderInputs()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - two range inputs with proper ARIA labels
        var sliders = cut.FindAll("input[type='range']");
        sliders.Should().HaveCount(2);

        var fromSlider = cut.Find(".slider-from");
        fromSlider.GetAttribute("aria-label").Should().Be("Start date");

        var toSlider = cut.Find(".slider-to");
        toSlider.GetAttribute("aria-label").Should().Be("End date");
    }

    [Fact]
    public void DateRangeSlider_RendersPresetButtons()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - five preset buttons
        var presets = cut.FindAll(".date-preset-button");
        presets.Should().HaveCount(5);
        presets[0].TextContent.Should().Be("7d");
        presets[1].TextContent.Should().Be("30d");
        presets[2].TextContent.Should().Be("90d");
        presets[3].TextContent.Should().Be("1y");
        presets[4].TextContent.Should().Be("All");
    }

    [Fact]
    public void DateRangeSlider_DefaultsTo90Days()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - 90d preset should be active by default
        var presets = cut.FindAll(".date-preset-button");
        presets[2].ClassList.Should().Contain("active", "90d should be active by default");

        // Verify the date display shows a range ending today
        var component = cut.Instance;
        component.CurrentToDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        component.CurrentFromDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(-90)));
    }

    [Fact]
    public void DateRangeSlider_RespectsCustomDefaultLastDays()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.DefaultLastDays, 30));

        // Assert - 30d preset should be active
        var presets = cut.FindAll(".date-preset-button");
        presets[1].ClassList.Should().Contain("active", "30d should be active when DefaultLastDays=30");

        var component = cut.Instance;
        component.CurrentFromDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(-30)));
    }

    [Fact]
    public void DateRangeSlider_RespectsFromDateParameter()
    {
        // Arrange
        var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-14));

        // Act
        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.FromDate, fromDate));

        // Assert
        var component = cut.Instance;
        component.CurrentFromDate.Should().Be(fromDate);
        component.CurrentToDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }

    [Fact]
    public void DateRangeSlider_RespectsToDateParameter()
    {
        // Arrange
        var toDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));

        // Act
        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.ToDate, toDate));

        // Assert
        var component = cut.Instance;
        component.CurrentToDate.Should().Be(toDate);
    }

    [Fact]
    public async Task DateRangeSlider_PresetClick_FiresDateRangeChanged()
    {
        // Arrange
        DateRangeChangedEventArgs? receivedArgs = null;

        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.OnDateRangeChanged, EventCallback.Factory.Create<DateRangeChangedEventArgs>(
                new object(), args => receivedArgs = args)));

        // Act - click the "7d" preset
        var sevenDayButton = cut.FindAll(".date-preset-button")[0];
        await cut.InvokeAsync(() => sevenDayButton.Click());

        // Assert
        receivedArgs.Should().NotBeNull();
        receivedArgs!.ToDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        receivedArgs.FromDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(-7)));
    }

    [Fact]
    public async Task DateRangeSlider_AllPreset_SetsFullRange()
    {
        // Arrange
        DateRangeChangedEventArgs? receivedArgs = null;

        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.OnDateRangeChanged, EventCallback.Factory.Create<DateRangeChangedEventArgs>(
                new object(), args => receivedArgs = args)));

        // Act - click the "All" preset
        var allButton = cut.FindAll(".date-preset-button")[4];
        await cut.InvokeAsync(() => allButton.Click());

        // Assert - "All" should set from to absolute min (3 years ago)
        receivedArgs.Should().NotBeNull();
        receivedArgs!.ToDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        receivedArgs.FromDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddYears(-3)));
    }

    [Fact]
    public void DateRangeSlider_HasAriaLiveRegion()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - date display has aria-live for accessibility
        var display = cut.Find(".date-range-display");
        display.GetAttribute("aria-live").Should().Be("polite");
    }

    [Fact]
    public void DateRangeSlider_HasNavigationLandmark()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - nav has aria-label
        var nav = cut.Find("nav.sidebar-section");
        nav.GetAttribute("aria-label").Should().Be("Filter by date range");
    }

    [Fact]
    public void DateRangeSlider_DisplaysMinMaxLabels()
    {
        // Arrange & Act
        var cut = Render<DateRangeSlider>();

        // Assert - min and max labels are displayed
        cut.Find(".slider-label-min").Should().NotBeNull();
        cut.Find(".slider-label-max").Should().NotBeNull();
    }

    [Fact]
    public async Task DateRangeSlider_PresetClick_HighlightsActivePreset()
    {
        // Arrange
        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.OnDateRangeChanged, EventCallback.Factory.Create<DateRangeChangedEventArgs>(
                new object(), _ => { })));

        // Act - click 30d preset
        var thirtyDayButton = cut.FindAll(".date-preset-button")[1];
        await cut.InvokeAsync(() => thirtyDayButton.Click());

        // Assert - 30d should now be active, 90d should not
        cut.WaitForAssertion(() =>
        {
            var presets = cut.FindAll(".date-preset-button");
            presets[1].ClassList.Should().Contain("active", "30d should be active after click");
            presets[2].ClassList.Should().NotContain("active", "90d should no longer be active");
        });
    }

    [Fact]
    public void DateRangeSlider_BothDateParameters_SetsCorrectRange()
    {
        // Arrange
        var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-14));
        var toDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

        // Act
        var cut = Render<DateRangeSlider>(parameters => parameters
            .Add(p => p.FromDate, fromDate)
            .Add(p => p.ToDate, toDate));

        // Assert
        var component = cut.Instance;
        component.CurrentFromDate.Should().Be(fromDate);
        component.CurrentToDate.Should().Be(toDate);
    }
}
