using AngleSharp.Html.Dom;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechHub.Web.Components;
using TechHub.Web.Configuration;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for NewsletterModal.razor component.
///
/// Covers:
/// - "Coming soon" notice when newsletter is disabled
/// - Topic checkboxes rendered when newsletter is enabled
/// - "Subscribe to all" toggles all topic checkboxes
/// - Deselecting a topic unchecks "subscribe to all"
/// - Submit button disabled when email or topics are missing
/// - Submit button enabled when email and at least one topic are provided
/// - Pre-selected topic is checked when the modal opens
/// - Closing via × or backdrop clears the modal
/// </summary>
public class NewsletterModalTests : BunitContext
{
    private static readonly NewsletterSettings DisabledSettings = new()
    {
        Enabled = false
    };

    private static readonly NewsletterSettings EnabledSettings = new()
    {
        Enabled = true,
        FormActionUrl = "https://example.us1.list-manage.com/subscribe/post",
        UserId = "test_user",
        ListId = "test_list",
        Topics = new Dictionary<string, NewsletterTopic>
        {
            ["ai"]      = new() { Label = "AI",     GroupId = "100", GroupValue = "1" },
            ["azure"]   = new() { Label = "Azure",  GroupId = "100", GroupValue = "2" },
            ["dotnet"]  = new() { Label = ".NET",   GroupId = "100", GroupValue = "4" }
        }
    };

    private void RegisterSettings(NewsletterSettings settings) =>
        Services.AddSingleton<IOptions<NewsletterSettings>>(Options.Create(settings));

    // ─── Coming-soon notice ───────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_WhenDisabled_ShowsComingSoonNotice()
    {
        RegisterSettings(DisabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll(".newsletter-coming-soon").Count > 0);

        cut.Find(".newsletter-coming-soon").Should().NotBeNull();
        cut.Find(".newsletter-coming-soon-title").TextContent.ToLowerInvariant().Should()
            .Contain("coming soon");
    }

    [Fact]
    public async Task NewsletterModal_WhenDisabled_DoesNotRenderForm()
    {
        RegisterSettings(DisabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll(".newsletter-coming-soon").Count > 0);

        cut.FindAll("form").Should().BeEmpty("form must not be rendered when newsletter is disabled");
    }

    // ─── Topic checkboxes ────────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_WhenEnabled_RendersTopicCheckboxes()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // 3 topic checkboxes + 1 "subscribe to all" = 4 total
        var checkboxes = cut.FindAll("input[type='checkbox']");
        checkboxes.Count.Should().Be(EnabledSettings.Topics.Count + 1,
            "one checkbox per topic plus one for subscribe-to-all");
    }

    [Fact]
    public async Task NewsletterModal_WhenEnabled_RendersTopicLabels()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        var markup = cut.Markup;
        markup.Should().Contain("AI");
        markup.Should().Contain("Azure");
        markup.Should().Contain(".NET");
    }

    // ─── Subscribe to all ─────────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_SubscribeToAll_ChecksAllTopicCheckboxes()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // The first checkbox is "subscribe to all"; click it
        var subscribeToAll = cut.Find(".newsletter-topic-all input[type='checkbox']");
        subscribeToAll.Change(true);

        // All individual topic checkboxes should now be checked
        var topicCheckboxes = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1) // skip subscribe-to-all
            .ToList();

        topicCheckboxes.Should().AllSatisfy(cb =>
            ((IHtmlInputElement)cb).IsChecked.Should().BeTrue("all topics should be checked after subscribe-to-all"));
    }

    [Fact]
    public async Task NewsletterModal_UnchecksSubscribeToAll_WhenTopicDeselected()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // Check "subscribe to all"
        var subscribeToAll = cut.Find(".newsletter-topic-all input[type='checkbox']");
        subscribeToAll.Change(true);

        // Now uncheck one topic
        var firstTopicCheckbox = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1)
            .First();
        firstTopicCheckbox.Change(false);

        // "Subscribe to all" should be unchecked
        ((IHtmlInputElement)subscribeToAll).IsChecked.Should().BeFalse(
            "subscribe-to-all must uncheck when any individual topic is deselected");
    }

    // ─── Submit button state ─────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_SubmitButton_DisabledWhenNoEmail()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // Select a topic but leave email empty
        var firstTopic = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1)
            .First();
        firstTopic.Change(true);

        var submitButton = cut.Find("button[type='submit']");
        submitButton.HasAttribute("disabled").Should().BeTrue("submit must be disabled without a valid email");
    }

    [Fact]
    public async Task NewsletterModal_SubmitButton_DisabledWhenNoTopicSelected()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // Provide an email but select no topic
        var emailInput = cut.Find("input[type='email']");
        emailInput.Input("user@example.com");

        var submitButton = cut.Find("button[type='submit']");
        submitButton.HasAttribute("disabled").Should().BeTrue("submit must be disabled without any topic selected");
    }

    [Fact]
    public async Task NewsletterModal_SubmitButton_EnabledWhenEmailAndTopicProvided()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // Provide email
        var emailInput = cut.Find("input[type='email']");
        emailInput.Input("user@example.com");

        // Select a topic
        var firstTopic = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1)
            .First();
        firstTopic.Change(true);

        var submitButton = cut.Find("button[type='submit']");
        submitButton.HasAttribute("disabled").Should().BeFalse("submit must be enabled when email and at least one topic are provided");
    }

    // ─── Pre-selected topic ──────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_WithPreSelectedTopic_ChecksMatchingCheckbox()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>(p => p.Add(m => m.PreSelectedTopic, "azure"));

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        // The Azure checkbox should be checked; AI and .NET should not
        var checkboxes = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1) // skip subscribe-to-all
            .ToList();

        // Exactly one checkbox should be checked (Azure)
        var checkedCount = checkboxes.Count(cb => ((IHtmlInputElement)cb).IsChecked);
        checkedCount.Should().Be(1, "only the pre-selected topic should be checked");
    }

    [Fact]
    public async Task NewsletterModal_WithUnknownPreSelectedTopic_ChecksNothing()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>(p => p.Add(m => m.PreSelectedTopic, "nonexistent"));

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        var topicCheckboxes = cut.FindAll(".newsletter-topics-fieldset input[type='checkbox']")
            .Skip(1)
            .ToList();

        topicCheckboxes.Should().AllSatisfy(cb =>
            ((IHtmlInputElement)cb).IsChecked.Should().BeFalse("unknown topic should not check any checkbox"));
    }

    // ─── Close behaviour ─────────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_CloseButton_HidesModal()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll(".newsletter-modal").Count > 0);

        cut.Find(".newsletter-modal-close").Click();

        cut.FindAll(".newsletter-modal").Should().BeEmpty("modal must be hidden after close button click");
    }

    [Fact]
    public async Task NewsletterModal_Backdrop_Click_HidesModal()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll(".newsletter-backdrop").Count > 0);

        cut.Find(".newsletter-backdrop").Click();

        cut.FindAll(".newsletter-backdrop").Should().BeEmpty("modal must be hidden after backdrop click");
    }

    // ─── Mailchimp field names ────────────────────────────────────────────────

    [Fact]
    public async Task NewsletterModal_Form_ContainsHiddenMailchimpFields()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        cut.Find("input[name='u']").Should().NotBeNull("Mailchimp user ID hidden field must be present");
        cut.Find("input[name='id']").Should().NotBeNull("Mailchimp list ID hidden field must be present");
    }

    [Fact]
    public async Task NewsletterModal_Form_PostsToConfiguredActionUrl()
    {
        RegisterSettings(EnabledSettings);
        var cut = Render<NewsletterModal>();

        await cut.InvokeAsync(() => cut.Instance.Open());
        cut.WaitForState(() => cut.FindAll("form").Count > 0);

        var form = cut.Find("form");
        form.GetAttribute("action").Should().Be(EnabledSettings.FormActionUrl);
        form.GetAttribute("method").ToLowerInvariant().Should().Be("post");
        form.GetAttribute("target").Should().Be("_blank");
    }
}
