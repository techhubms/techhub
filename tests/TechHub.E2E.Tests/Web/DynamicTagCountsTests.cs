using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for dynamic tag count functionality.
/// Tests that tag counts update based on filter state to prevent empty result sets.
/// Based on spec: /workspaces/techhub/specs/001a-tag-counting/spec.md
/// </summary>
public class DynamicTagCountsTests : PlaywrightTestBase
{
    public DynamicTagCountsTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task TagCounts_WhenNoFilters_ShowTotalItemsWithTag()
    {
        // Arrange - Navigate to section page with no filters
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Assert - All tags should show counts > 0 (formatted with thousand separator if applicable)
        var tagButtons = await Page.Locator(".tag-cloud-item").AllAsync();
        tagButtons.Should().NotBeEmpty("Tag cloud should display tags");

        foreach (var tag in tagButtons)
        {
            var text = await tag.TextContentAsync();
            text.Should().NotBeNull();

            // Verify count format: "TagName (count)" where count is a number with optional comma separator
            text.Should().MatchRegex(@".+\s\(\d{1,3}(,\d{3})*\)", $"Tag '{text}' should display count in format 'TagName (123)' or 'TagName (1,234)'");
        }
    }

    [Fact]
    public async Task TagCounts_WhenTagSelected_ShowIntersectionCounts()
    {
        // Arrange - Navigate to section page
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Get initial counts for all tags
        var initialCounts = await GetTagCountsAsync();
        initialCounts.Should().NotBeEmpty("Initial tag cloud should have tags");

        // Act - Select first tag (that has a count > 0)
        var firstTag = Page.Locator(".tag-cloud-item").First;
        await firstTag.ClickBlazorElementAsync();

        // Wait for URL to update
        await Page.WaitForURLAsync(url => url.Contains("tags="));

        // Wait for tag cloud to reload with new counts
        await WaitForTagCloudReadyAsync();

        // Assert - Tag counts should update to show intersection
        var updatedCounts = await GetTagCountsAsync();

        // At least one tag should have different count (showing intersection)
        // OR at least one tag should now be disabled (count = 0)
        var hasCountChanges = initialCounts.Any(kvp =>
            updatedCounts.ContainsKey(kvp.Key) && updatedCounts[kvp.Key] != kvp.Value);

        var hasDisabledTags = await Page.Locator(".tag-cloud-item.disabled").CountAsync() > 0;

        (hasCountChanges || hasDisabledTags).Should().BeTrue(
            "Tag counts should update to show intersection after selecting a tag, " +
            $"or some tags should become disabled. Initial counts: {string.Join(", ", initialCounts.Take(3).Select(kvp => $"{kvp.Key}={kvp.Value}"))}");
    }

    [Fact]
    public async Task TagCounts_WhenCountIsZero_TagBecomesDisabled()
    {
        // Arrange - Navigate to section page
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Act - Select multiple tags until we get a disabled tag
        // (In real data, selecting incompatible tags should result in count=0 tags)
        var attempts = 0;
        var maxAttempts = 5;
        var hasDisabledTag = false;

        while (attempts < maxAttempts)
        {
            // Get enabled tags that are NOT already selected (to avoid deselecting)
            var enabledTags = await Page.Locator(".tag-cloud-item:not(.disabled):not(.selected)").AllAsync();
            if (enabledTags.Count == 0)
            {
                break;
            }

            // Select a tag
            await enabledTags[0].ClickBlazorElementAsync();
            await Page.WaitForURLAsync(url => url.Contains("tags="));
            await WaitForTagCloudReadyAsync();

            // Check if any tags are now disabled
            var disabledCount = await Page.Locator(".tag-cloud-item.disabled").CountAsync();
            if (disabledCount > 0)
            {
                hasDisabledTag = true;
                break;
            }

            attempts++;
        }

        // Assert - If we found a disabled tag, verify it meets requirements
        if (hasDisabledTag)
        {
            var disabledTag = Page.Locator(".tag-cloud-item.disabled").First;

            // Verify disabled tag has count of 0
            var disabledText = await disabledTag.TextContentAsync();
            disabledText.Should().Contain("(0)", "Disabled tag should show count of 0");

            // Verify disabled tag cannot be clicked
            var isDisabled = await disabledTag.GetAttributeAsync("disabled");
            isDisabled.Should().NotBeNull("Disabled tag should have disabled attribute");

            var ariaDisabled = await disabledTag.GetAttributeAsync("aria-disabled");
            ariaDisabled.Should().Be("true", "Disabled tag should have aria-disabled=true");

            // Verify CSS class
            var classList = await disabledTag.GetAttributeAsync("class");
            classList.Should().Contain("disabled", "Disabled tag should have 'disabled' CSS class");
        }
    }

    [Fact]
    public async Task TagCounts_WhenTagDeselected_CountsRecalculate()
    {
        // Arrange - Navigate to section page and select a tag
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        var firstTag = Page.Locator(".tag-cloud-item").First;
        await firstTag.ClickBlazorElementAsync();
        await Page.WaitForURLAsync(url => url.Contains("tags="));
        await WaitForTagCloudReadyAsync();

        // Get counts with one tag selected
        var countsWithFilter = await GetTagCountsAsync();

        // Act - Deselect the tag
        var selectedTag = Page.Locator(".tag-cloud-item.selected").First;
        await selectedTag.ClickBlazorElementAsync();

        // Wait for URL to update (tags parameter should be removed)
        await Page.WaitForURLAsync(url => !url.Contains("tags="));
        await WaitForTagCloudReadyAsync();

        // Assert - Counts should recalculate to show totals again
        var countsWithoutFilter = await GetTagCountsAsync();

        // At least one tag should have higher count without filter
        // (because we're no longer filtering to intersection)
        countsWithFilter.Any(kvp =>
            countsWithoutFilter.ContainsKey(kvp.Key) &&
            countsWithoutFilter[kvp.Key] > kvp.Value)
            .Should().BeTrue(
                "After deselecting tag, at least one tag should have higher count " +
                $"(showing totals instead of intersection). " +
                $"Sample with filter: {string.Join(", ", countsWithFilter.Take(3).Select(kvp => $"{kvp.Key}={kvp.Value}"))}");
    }

    [Fact]
    public async Task TagCounts_DisplaysThousandSeparators()
    {
        // Arrange - Navigate to a page that likely has tags with high counts
        await Page.GotoRelativeAsync("/all");
        await WaitForTagCloudReadyAsync();

        // Act - Get all tag texts
        var tagTexts = await Page.Locator(".tag-cloud-item").AllTextContentsAsync();

        // Assert - If any tag has count >= 1000, verify it uses comma separator
        var hasHighCountTags = tagTexts.Any(text =>
        {
            // Extract count from "TagName (1,234)" format
            var match = System.Text.RegularExpressions.Regex.Match(text, @"\((\d+(?:,\d{3})*)\)");
            if (match.Success)
            {
                var countStr = match.Groups[1].Value.Replace(",", "");
                if (int.TryParse(countStr, out var count))
                {
                    return count >= 1000;
                }
            }

            return false;
        });

        if (hasHighCountTags)
        {
            // Verify that counts >= 1000 use comma separator
            var highCountTagsWithComma = tagTexts.Count(text =>
                System.Text.RegularExpressions.Regex.IsMatch(text, @"\(\d{1,3},\d{3}(?:,\d{3})*\)"));

            highCountTagsWithComma.Should().BeGreaterThan(0,
                "Tags with counts >= 1,000 should display thousand separators (e.g., '1,234')");
        }
    }

    [Fact]
    public async Task DisabledTag_CannotBeClicked()
    {
        // Arrange - Navigate and select tags until we get a disabled tag
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Select multiple tags to create disabled state
        var attempts = 0;
        var maxAttempts = 5;

        while (attempts < maxAttempts)
        {
            // Get enabled tags that are NOT already selected (to avoid deselecting)
            var enabledTags = await Page.Locator(".tag-cloud-item:not(.disabled):not(.selected)").AllAsync();
            if (enabledTags.Count == 0)
            {
                break;
            }

            await enabledTags[0].ClickBlazorElementAsync();
            await Page.WaitForURLAsync(url => url.Contains("tags="));
            await WaitForTagCloudReadyAsync();

            var disabledCount = await Page.Locator(".tag-cloud-item.disabled").CountAsync();
            if (disabledCount > 0)
            {
                break;
            }

            attempts++;
        }

        var disabledTagExists = await Page.Locator(".tag-cloud-item.disabled").CountAsync() > 0;

        if (disabledTagExists)
        {
            // Act - Try to click disabled tag
            var disabledTag = Page.Locator(".tag-cloud-item.disabled").First;
            var urlBeforeClick = Page.Url;

            // Use the shared helper but disable URL wait since we expect NO change
            await disabledTag.ClickBlazorElementAsync(
                timeoutMs: BlazorHelpers.DefaultAssertionTimeout,
                waitForUrlChange: false);

            // Assert - URL should not change (tag should not be selected)
            Page.Url.Should().Be(urlBeforeClick, "Clicking disabled tag should not change URL or filter state");

            // CSS class should still be disabled
            var isStillDisabled = await disabledTag.EvaluateAsync<bool>("el => el.classList.contains('disabled')");
            isStillDisabled.Should().BeTrue("Tag should remain in disabled state after click attempt");
        }
    }

    /// <summary>
    /// Waits for the tag cloud component to finish loading and render tags
    /// </summary>
    private async Task WaitForTagCloudReadyAsync()
    {
        // Wait for tag cloud to finish loading (tags become visible)
        await Page.WaitForSelectorAsync(".tag-cloud-item", new() { State = WaitForSelectorState.Visible });

        // Wait for Blazor Server interactivity to be ready
        await Page.WaitForBlazorReadyAsync();
    }

    /// <summary>
    /// Extracts tag names and their counts from the current page
    /// </summary>
    private async Task<Dictionary<string, int>> GetTagCountsAsync()
    {
        var tagElements = await Page.Locator(".tag-cloud-item").AllAsync();
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var tagElement in tagElements)
        {
            var text = await tagElement.TextContentAsync();
            if (text == null)
            {
                continue;
            }

            // Parse "TagName (123)" or "TagName (1,234)"
            var match = System.Text.RegularExpressions.Regex.Match(text, @"(.+?)\s\((\d+(?:,\d{3})*)\)");
            if (match.Success)
            {
                var tagName = match.Groups[1].Value.Trim();
                var countStr = match.Groups[2].Value.Replace(",", "");
                if (int.TryParse(countStr, out var count))
                {
                    counts[tagName] = count;
                }
            }
        }

        return counts;
    }
}
