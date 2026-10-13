using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminNewsletter.razor — subscriber list and filters, edit/save, delete, send log
/// and test-send triggering.
/// </summary>
public class AdminNewsletterTests : AdminPageTestBase
{
    private const string DefaultTestEmail = "whistler112@gmail.com";

    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminNewsletterTests()
    {
        Services.AddSingleton(_api.Object);

        SetupData(
            subscribers: [],
            sendLog: [],
            sections: [CreateSection("ai", "AI"), CreateSection("azure", "Azure")]);
    }

    // ── List ────────────────────────────────────────────────────────────────

    [Fact]
    public void Render_WithSubscribers_ShowsOneRowPerSubscriberWithDetails()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "jane@example.com", displayName: "Jane", confirmed: true, weekly: ["ai", "azure"], daily: ["ai"]),
            CreateSubscriber(2, "bob@example.com", displayName: null, confirmed: false)
        ]);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        var rows = SubscriberRows(cut);
        rows.Should().HaveCount(2);
        var janeCells = rows[0].QuerySelectorAll("td");
        janeCells[0].TextContent.Should().Be("jane@example.com");
        janeCells[1].TextContent.Should().Be("Jane");
        janeCells[2].TextContent.Should().Be("ai, azure");
        janeCells[3].TextContent.Should().Be("ai");
        janeCells[4].TextContent.Should().Be("✓");
        var bobCells = rows[1].QuerySelectorAll("td");
        bobCells[1].TextContent.Should().BeEmpty();
        bobCells[4].TextContent.Should().Be("✗");
    }

    [Fact]
    public void Render_WithSubscribers_RequestsFirstPageOfFiveHundredAndTheSendLog()
    {
        // Act
        Render<AdminNewsletter>();

        // Assert
        _api.Verify(x => x.GetNewsletterSubscribersAsync(1, 500, null, null, It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetNewsletterSendLogAsync(100, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_WithSubscribers_StatsReflectConfirmedAndUnconfirmedCounts()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "a@example.com", confirmed: true),
            CreateSubscriber(2, "b@example.com", confirmed: true),
            CreateSubscriber(3, "c@example.com", confirmed: false)
        ]);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        Stats(cut).Should().Equal("3", "2", "1");
    }

    [Fact]
    public void Render_NoSubscribers_ShowsEmptyTableAndZeroStats()
    {
        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        SubscriberRows(cut).Should().BeEmpty();
        Stats(cut).Should().Equal("0", "0", "0");
    }

    [Fact]
    public void Render_SubscriberSendStatus_ShowsDashForNeverSentAndMarksSuccessAndFailure()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(
                1,
                "a@example.com",
                lastDailySentAt: new DateTimeOffset(2026, 7, 15, 15, 30, 45, TimeSpan.Zero),
                lastDailySucceeded: true,
                lastWeeklySentAt: new DateTimeOffset(2026, 1, 15, 15, 30, 45, TimeSpan.Zero),
                lastWeeklySucceeded: false),
            CreateSubscriber(2, "b@example.com")
        ]);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        var rows = SubscriberRows(cut);
        var sent = rows[0].QuerySelectorAll(".admin-cell-send-status");
        sent[0].TextContent.Should().Be("✓ 2026-07-15 17:30:45");
        sent[1].TextContent.Should().Be("✗ 2026-01-15 16:30:45");
        var never = rows[1].QuerySelectorAll(".admin-cell-send-status");
        never[0].TextContent.Should().Be("—");
        never[1].TextContent.Should().Be("—");
    }

    [Fact]
    public void Render_SubscriberSentWithoutOutcome_IsMarkedAsFailed()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "a@example.com", lastDailySentAt: new DateTimeOffset(2026, 7, 15, 15, 30, 45, TimeSpan.Zero), lastDailySucceeded: null)
        ]);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        SubscriberRows(cut)[0].QuerySelector(".admin-cell-send-status")!.TextContent.Should().StartWith("✗");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorWithMessageAndNoRows()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to refresh newsletter data: down");
        SubscriberRows(cut).Should().BeEmpty();
    }

    [Fact]
    public void Refresh_AfterEarlierFailure_ClearsErrorLoadsDataAndLoadsSectionsOnce()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));
        var cut = Render<AdminNewsletter>();
        cut.FindAll(".admin-alert-error").Should().ContainSingle();
        SetupData(subscribers: [CreateSubscriber(1, "jane@example.com")]);

        // Act
        cut.Find(".admin-actions button").Click();

        // Assert
        cut.FindAll(".admin-alert").Should().BeEmpty();
        SubscriberRows(cut).Should().ContainSingle();
        _api.Verify(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Refresh_Click_ReloadsSubscribersButNotSections()
    {
        // Arrange
        var cut = Render<AdminNewsletter>();
        SetupData(subscribers: [CreateSubscriber(1, "new@example.com")]);

        // Act
        cut.Find(".admin-actions button").Click();

        // Assert
        SubscriberRows(cut).Should().ContainSingle().Which.TextContent.Should().Contain("new@example.com");
        _api.Verify(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _api.Verify(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Filters ─────────────────────────────────────────────────────────────

    [Fact]
    public void Search_ByEmailFragment_FiltersCaseInsensitively()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "jane@example.com"), CreateSubscriber(2, "bob@example.com")]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#subscriberSearch").Input("JANE");

        // Assert
        SubscriberRows(cut).Should().ContainSingle().Which.TextContent.Should().Contain("jane@example.com");
        Stats(cut)[0].Should().Be("1");
    }

    [Fact]
    public void Search_ByDisplayName_MatchesSubscribersWithThatName()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "a@example.com", displayName: "Alice Wonder"),
            CreateSubscriber(2, "b@example.com", displayName: null),
            CreateSubscriber(3, "c@example.com", displayName: "Charlie")
        ]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#subscriberSearch").Input("wonder");

        // Assert
        SubscriberRows(cut).Should().ContainSingle().Which.TextContent.Should().Contain("a@example.com");
    }

    [Fact]
    public void Search_NoMatch_ShowsNoRows()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "a@example.com")]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#subscriberSearch").Input("zzz");

        // Assert
        SubscriberRows(cut).Should().BeEmpty();
        Stats(cut).Should().Equal("0", "0", "0");
    }

    [Fact]
    public void Search_Whitespace_DoesNotFilter()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "a@example.com"), CreateSubscriber(2, "b@example.com")]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#subscriberSearch").Input("   ");

        // Assert
        SubscriberRows(cut).Should().HaveCount(2);
    }

    [Theory]
    [InlineData("true", new[] { "a@example.com", "c@example.com" })]
    [InlineData("false", new[] { "b@example.com" })]
    [InlineData("", new[] { "a@example.com", "b@example.com", "c@example.com" })]
    public void ConfirmedFilter_Selection_ShowsMatchingSubscribers(string filter, string[] expectedEmails)
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "a@example.com", confirmed: true),
            CreateSubscriber(2, "b@example.com", confirmed: false),
            CreateSubscriber(3, "c@example.com", confirmed: true)
        ]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#confirmedFilter").Change(filter);

        // Assert
        SubscriberRows(cut).Select(r => r.QuerySelector("td")!.TextContent).Should().Equal(expectedEmails);
    }

    [Fact]
    public void Filters_SearchAndConfirmedCombined_AppliesBoth()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "jane@example.com", confirmed: true),
            CreateSubscriber(2, "jane.doe@example.com", confirmed: false),
            CreateSubscriber(3, "bob@example.com", confirmed: false)
        ]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#subscriberSearch").Input("jane");
        cut.Find("#confirmedFilter").Change("false");

        // Assert
        SubscriberRows(cut).Should().ContainSingle().Which.TextContent.Should().Contain("jane.doe@example.com");
        Stats(cut).Should().Equal("1", "0", "1");
    }

    // ── Edit / save ─────────────────────────────────────────────────────────

    [Fact]
    public void Edit_Click_OpensModalPrefilledFromSubscriber()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "jane@example.com", displayName: "Jane", weekly: ["ai", "azure"], daily: ["ai"])]);
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("button.admin-link-btn:not(.admin-link-btn-danger)").Click();

        // Assert
        cut.Find(".admin-modal-title").TextContent.Should().Be("Edit subscriber");
        var inputs = cut.FindAll(".admin-modal input");
        inputs[0].GetAttribute("value").Should().Be("Jane");
        inputs[1].GetAttribute("value").Should().Be("ai, azure");
        inputs[2].GetAttribute("value").Should().Be("ai");
    }

    [Fact]
    public void Render_Initially_ModalIsNotShown()
    {
        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
    }

    [Fact]
    public void Save_EditedValues_SendsNormalizedSectionsClosesModalAndReloads()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com", displayName: "Jane")]);
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);
        cut.FindAll(".admin-modal input")[0].Change("Jane Doe");
        cut.FindAll(".admin-modal input")[1].Change(" AI, Azure ,ai,, ");
        cut.FindAll(".admin-modal input")[2].Change("GitHub-Copilot");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(x => x.UpdateNewsletterSubscriberAsync(
            7,
            "Jane Doe",
            It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "ai", "azure" })),
            It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "github-copilot" })),
            It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".admin-modal").Should().BeEmpty();
        cut.Find(".admin-alert-success").TextContent.Should().Be("Subscriber updated.");
        _api.Verify(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public void Save_BlankSectionFields_SendsEmptyLists()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com", weekly: ["ai"], daily: ["azure"])]);
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);
        cut.FindAll(".admin-modal input")[1].Change("");
        cut.FindAll(".admin-modal input")[2].Change("   ");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(x => x.UpdateNewsletterSubscriberAsync(
            7,
            It.IsAny<string?>(),
            It.Is<IReadOnlyList<string>>(l => l.Count == 0),
            It.Is<IReadOnlyList<string>>(l => l.Count == 0),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Save_ApiFails_KeepsModalOpenAndShowsError()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com")]);
        _api.Setup(x => x.UpdateNewsletterSubscriberAsync(It.IsAny<long>(), It.IsAny<string?>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to update subscriber: boom");
        cut.FindAll(".admin-modal").Should().ContainSingle();
    }

    [Fact]
    public void Edit_CancelButton_ClosesModalWithoutCallingApi()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com")]);
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);

        // Act
        cut.Find(".admin-modal-footer .btn-secondary").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
        VerifyNoUpdate();
    }

    [Fact]
    public void Edit_CloseButton_ClosesModalWithoutCallingApi()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com")]);
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);

        // Act
        cut.Find(".admin-modal-close").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
        VerifyNoUpdate();
    }

    [Fact]
    public void Edit_BackdropClick_ClosesModal()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(7, "jane@example.com")]);
        var cut = Render<AdminNewsletter>();
        OpenEditModal(cut);

        // Act
        cut.Find(".admin-modal-backdrop").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
    }

    [Fact]
    public void Edit_CancelThenEditOtherSubscriber_PrefillsFromTheNewSubscriber()
    {
        // Arrange
        SetupData(subscribers:
        [
            CreateSubscriber(1, "a@example.com", displayName: "Alice"),
            CreateSubscriber(2, "b@example.com", displayName: "Bob")
        ]);
        var cut = Render<AdminNewsletter>();
        EditButtons(cut)[0].Click();
        cut.Find(".admin-modal-footer .btn-secondary").Click();

        // Act
        EditButtons(cut)[1].Click();

        // Assert
        cut.FindAll(".admin-modal input")[0].GetAttribute("value").Should().Be("Bob");
    }

    // ── Delete ──────────────────────────────────────────────────────────────

    [Fact]
    public void Delete_Click_DeletesSubscriberReloadsAndShowsSuccess()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "a@example.com"), CreateSubscriber(2, "b@example.com")]);
        var cut = Render<AdminNewsletter>();
        SetupData(subscribers: [CreateSubscriber(2, "b@example.com")]);

        // Act
        cut.FindAll("button.admin-link-btn-danger")[0].Click();

        // Assert
        _api.Verify(x => x.DeleteNewsletterSubscriberAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        SubscriberRows(cut).Should().ContainSingle().Which.TextContent.Should().Contain("b@example.com");
        var alert = cut.Find(".admin-alert-success");
        alert.TextContent.Should().Be("Subscriber deleted.");
    }

    [Fact]
    public void Delete_ApiFails_ShowsErrorAndKeepsRows()
    {
        // Arrange
        SetupData(subscribers: [CreateSubscriber(1, "a@example.com")]);
        _api.Setup(x => x.DeleteNewsletterSubscriberAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("cannot delete"));
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("button.admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Be("cannot delete");
        SubscriberRows(cut).Should().ContainSingle();
        _api.Verify(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Send log ────────────────────────────────────────────────────────────

    [Fact]
    public void Render_WithSendLog_ShowsOneRowPerEntryWithFormattedValues()
    {
        // Arrange
        SetupData(sendLog:
        [
            new NewsletterSendLogEntry
            {
                Id = 1,
                SendKind = "weekly",
                TargetKey = "2026-W28",
                SentAt = new DateTimeOffset(2026, 7, 15, 15, 30, 45, TimeSpan.Zero),
                RecipientCount = 120,
                FailedCount = 3,
                Status = "partial"
            },
            new NewsletterSendLogEntry
            {
                Id = 2,
                SendKind = "daily",
                TargetKey = "2026-07-14",
                SentAt = new DateTimeOffset(2026, 7, 14, 6, 0, 0, TimeSpan.Zero),
                RecipientCount = 5,
                FailedCount = 0,
                Status = "ok"
            }
        ]);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        var rows = SendLogRows(cut);
        rows.Should().HaveCount(2);
        rows[0].QuerySelectorAll("td").Select(c => c.TextContent)
            .Should().Equal("2026-07-15 17:30:45", "weekly", "2026-W28", "120", "3", "partial");
        rows[1].QuerySelectorAll("td")[4].TextContent.Should().Be("—", "no failures are shown as a dash");
    }

    [Fact]
    public void Render_EmptySendLog_ShowsHeadingWithNoRows()
    {
        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.Find(".admin-newsletter-send-log-heading").TextContent.Should().Be("Send log");
        SendLogRows(cut).Should().BeEmpty();
    }

    // ── Test send ───────────────────────────────────────────────────────────

    [Fact]
    public void Render_WithSections_ShowsOneCheckedCheckboxPerSection()
    {
        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        var items = cut.FindAll(".admin-checkbox-item");
        items.Select(i => i.TextContent.Trim()).Should().Equal("AI", "Azure");
        items.Should().OnlyContain(i => i.QuerySelector("input")!.HasAttribute("checked"));
    }

    [Fact]
    public void Render_NoSections_HidesSectionPickerAndDisablesTestButtons()
    {
        // Arrange
        SetupData(sections: []);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.FindAll(".admin-checkbox-list").Should().BeEmpty();
        TestButton(cut, "Test Weekly").HasAttribute("disabled").Should().BeTrue();
        TestButton(cut, "Test Daily").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Render_SectionsApiReturnsNull_TreatsAsNoSections()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync((IReadOnlyList<Section>?)null);

        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.FindAll(".admin-checkbox-list").Should().BeEmpty();
        TestButton(cut, "Test Weekly").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Render_Defaults_TestButtonsEnabledWithPrefilledEmail()
    {
        // Act
        var cut = Render<AdminNewsletter>();

        // Assert
        cut.Find("#testEmail").GetAttribute("value").Should().Be(DefaultTestEmail);
        TestButton(cut, "Test Weekly").HasAttribute("disabled").Should().BeFalse();
        TestButton(cut, "Test Daily").HasAttribute("disabled").Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TestEmail_BlankValue_DisablesTestButtons(string email)
    {
        // Arrange
        var cut = Render<AdminNewsletter>();

        // Act
        cut.Find("#testEmail").Change(email);

        // Assert
        TestButton(cut, "Test Weekly").HasAttribute("disabled").Should().BeTrue();
        TestButton(cut, "Test Daily").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void TestSections_AllUnchecked_DisablesTestButtons()
    {
        // Arrange
        var cut = Render<AdminNewsletter>();

        // Act
        var checkboxCount = cut.FindAll(".admin-checkbox-item input").Count;
        for (var i = 0; i < checkboxCount; i++)
        {
            cut.FindAll(".admin-checkbox-item input")[i].Change(false);
        }

        // Assert
        TestButton(cut, "Test Weekly").HasAttribute("disabled").Should().BeTrue();
        TestButton(cut, "Test Daily").HasAttribute("disabled").Should().BeTrue();
    }

    [Theory]
    [InlineData("Test Weekly", "weekly")]
    [InlineData("Test Daily", "daily")]
    public void TestSend_Click_SendsTrimmedEmailAllSectionsAndKind(string buttonText, string expectedKind)
    {
        // Arrange
        var cut = Render<AdminNewsletter>();
        cut.Find("#testEmail").Change("  me@example.com  ");

        // Act
        TestButton(cut, buttonText).Click();

        // Assert
        _api.Verify(x => x.TriggerNewsletterTestSendAsync(
            "me@example.com",
            It.Is<IReadOnlyList<string>>(l => l.OrderBy(s => s).SequenceEqual(new[] { "ai", "azure" })),
            expectedKind,
            It.IsAny<CancellationToken>()), Times.Once);
        var alert = cut.Find(".admin-alert-success");
        alert.TextContent.Should().Be($"Test {expectedKind} send triggered.");
    }

    [Fact]
    public void TestSend_SectionUnchecked_SendsOnlyRemainingSections()
    {
        // Arrange
        var cut = Render<AdminNewsletter>();
        cut.FindAll(".admin-checkbox-item input")[0].Change(false);

        // Act
        TestButton(cut, "Test Weekly").Click();

        // Assert
        _api.Verify(x => x.TriggerNewsletterTestSendAsync(
            DefaultTestEmail,
            It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "azure" })),
            "weekly",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void TestSend_SectionRecheckedAfterUncheck_IsIncludedAgain()
    {
        // Arrange
        var cut = Render<AdminNewsletter>();
        cut.FindAll(".admin-checkbox-item input")[0].Change(false);
        cut.FindAll(".admin-checkbox-item input")[0].Change(true);

        // Act
        TestButton(cut, "Test Daily").Click();

        // Assert
        _api.Verify(x => x.TriggerNewsletterTestSendAsync(
            It.IsAny<string>(),
            It.Is<IReadOnlyList<string>>(l => l.OrderBy(s => s).SequenceEqual(new[] { "ai", "azure" })),
            "daily",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void TestSend_ApiFails_ShowsErrorMessage()
    {
        // Arrange
        _api.Setup(x => x.TriggerNewsletterTestSendAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("smtp unavailable"));
        var cut = Render<AdminNewsletter>();

        // Act
        TestButton(cut, "Test Weekly").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Be("smtp unavailable");
        cut.FindAll(".admin-alert-success").Should().BeEmpty();
    }

    [Fact]
    public void TestSend_SuccessAfterFailure_ReplacesErrorWithSuccess()
    {
        // Arrange
        _api.Setup(x => x.TriggerNewsletterTestSendAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("smtp unavailable"));
        var cut = Render<AdminNewsletter>();
        TestButton(cut, "Test Weekly").Click();
        cut.FindAll(".admin-alert-error").Should().ContainSingle();
        _api.Setup(x => x.TriggerNewsletterTestSendAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        TestButton(cut, "Test Weekly").Click();

        // Assert
        cut.FindAll(".admin-alert-error").Should().BeEmpty();
        cut.Find(".admin-alert-success").TextContent.Should().Be("Test weekly send triggered.");
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static List<IElement> SubscriberRows(IRenderedComponent<AdminNewsletter> cut) =>
        cut.FindAll(".admin-table")[0].QuerySelectorAll("tbody tr").ToList();

    private static List<IElement> SendLogRows(IRenderedComponent<AdminNewsletter> cut) =>
        cut.FindAll(".admin-table")[1].QuerySelectorAll("tbody tr").ToList();

    private static List<string> Stats(IRenderedComponent<AdminNewsletter> cut) =>
        cut.FindAll(".admin-stat-value").Select(e => e.TextContent).ToList();

    private static IReadOnlyList<IElement> EditButtons(IRenderedComponent<AdminNewsletter> cut) =>
        cut.FindAll("button.admin-link-btn:not(.admin-link-btn-danger)");

    private static void OpenEditModal(IRenderedComponent<AdminNewsletter> cut) =>
        EditButtons(cut)[0].Click();

    private static IElement TestButton(IRenderedComponent<AdminNewsletter> cut, string text) =>
        cut.FindAll(".admin-filters button.btn").Single(b => b.TextContent.Contains(text, StringComparison.Ordinal));

    private void VerifyNoUpdate() =>
        _api.Verify(x => x.UpdateNewsletterSubscriberAsync(It.IsAny<long>(), It.IsAny<string?>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<CancellationToken>()), Times.Never);

    private static Section CreateSection(string name, string title) =>
        A.Section.WithName(name).WithTitle(title).Build();

    private static NewsletterSubscriber CreateSubscriber(
        long id,
        string email,
        string? displayName = null,
        bool confirmed = true,
        string[]? weekly = null,
        string[]? daily = null,
        DateTimeOffset? lastDailySentAt = null,
        bool? lastDailySucceeded = null,
        DateTimeOffset? lastWeeklySentAt = null,
        bool? lastWeeklySucceeded = null) =>
        new()
        {
            Id = id,
            Email = email,
            DisplayName = displayName,
            IsConfirmed = confirmed,
            WeeklySections = weekly ?? [],
            DailySections = daily ?? [],
            LastDailySentAt = lastDailySentAt,
            LastDailySucceeded = lastDailySucceeded,
            LastWeeklySentAt = lastWeeklySentAt,
            LastWeeklySucceeded = lastWeeklySucceeded
        };

    /// <summary>Configures the API mock; arguments left <c>null</c> keep their previously configured value.</summary>
    private void SetupData(
        IReadOnlyList<NewsletterSubscriber>? subscribers = null,
        IReadOnlyList<NewsletterSendLogEntry>? sendLog = null,
        IReadOnlyList<Section>? sections = null)
    {
        if (subscribers is not null)
        {
            _api.Setup(x => x.GetNewsletterSubscribersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscribers);
        }

        if (sendLog is not null)
        {
            _api.Setup(x => x.GetNewsletterSendLogAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(sendLog);
        }

        if (sections is not null)
        {
            _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(sections);
        }
    }
}
