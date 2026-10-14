using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminReviews.razor — review queue rendering, approve/reject flows, diff/preview modals
/// and the proposed-change editor.
/// </summary>
public class AdminReviewsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminReviewsTests()
    {
        ComponentFactories.AddStub<ContentItemEditorModal>();
        Services.AddSingleton(_api.Object);

        SetupReviews([]);
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
    }

    // ── Loading and rendering ───────────────────────────────────────────────

    [Fact]
    public void Render_Default_LoadsPendingReviewsAndSummary()
    {
        // Arrange
        SetupReviews([CreateReview(1), CreateReview(2, slug: "second")], new ContentReviewSummary { Pending = 2, Approved = 5, Rejected = 1 });

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        _api.Verify(x => x.GetContentReviewsAsync("pending", It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll("tbody tr").Should().HaveCount(2);
        cut.Find(".admin-result-summary").TextContent.Should().Contain("Showing 2 reviews");
        var badges = cut.Find(".review-summary-badges").TextContent;
        badges.Should().Contain("2 pending").And.Contain("5 approved").And.Contain("1 rejected");
    }

    [Fact]
    public void Render_NoReviews_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No content reviews match the current filter.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetContentReviewsAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("API down"));

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load reviews: API down");
        cut.FindAll("table").Should().BeEmpty();
    }

    [Fact]
    public void Render_SummaryFails_StillShowsReviews()
    {
        // Arrange
        SetupReviews([CreateReview(1)]);
        _api.Setup(x => x.GetContentReviewSummaryAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("summary down"));

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.FindAll("tbody tr").Should().HaveCount(1);
        cut.FindAll(".review-summary-badges").Should().BeEmpty();
    }

    [Fact]
    public void Render_StatusInQueryString_UsesItAsInitialFilter()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/reviews?status=approved");

        // Act
        Render<AdminReviews>();

        // Assert
        _api.Verify(x => x.GetContentReviewsAsync("approved", It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("approved", "approved")]
    [InlineData("rejected", "rejected")]
    [InlineData("", null)]
    public void StatusFilter_Changed_ReloadsWithNewFilter(string selected, string? expectedStatus)
    {
        // Arrange
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("#statusFilter").Change(selected);

        // Assert
        _api.Verify(x => x.GetContentReviewsAsync(expectedStatus, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Refresh_Click_ReloadsReviewsAndClearsMessage()
    {
        // Arrange
        SetupReviews([CreateReview(1)]);
        _api.Setup(x => x.ApproveContentReviewAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminReviews>();
        FindRowButtons(cut)[0].Click();
        cut.Find(".admin-alert").Should().NotBeNull();

        // Act
        cut.Find(".admin-actions .btn-secondary").Click();

        // Assert
        cut.FindAll(".admin-alert").Should().BeEmpty();
    }

    [Fact]
    public void Render_NonPendingReview_ShowsNoActionButtons()
    {
        // Arrange
        SetupReviews([CreateReview(1, status: ContentReviewStatus.Approved)]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        FindRowButtons(cut).Should().BeEmpty();
        cut.Find(".review-actions-cell").TextContent.Should().Contain("—");
        cut.Find("tbody .admin-status").ClassList.Should().Contain("admin-status-success");
    }

    [Fact]
    public void Render_LongSlug_IsTruncatedButLinkedInFull()
    {
        // Arrange
        var slug = new string('a', 60);
        SetupReviews([CreateReview(1, slug: slug)]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        var link = cut.Find(".review-article-link");
        link.TextContent.Trim().Should().HaveLength(38).And.EndWith("…");
        link.GetAttribute("href").Should().Be($"/admin/content-preview/blogs/{slug}");
    }

    // ── Change-type rendering ───────────────────────────────────────────────

    [Fact]
    public void Render_TagsReview_ShowsRemovedKeptAndAddedPills()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Tags, original: ",ai,azure,", fixedValue: ",ai,dotnet,")]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find(".review-tag-removed").TextContent.Should().Contain("azure");
        cut.Find(".review-tag-kept").TextContent.Should().Be("ai");
        cut.Find(".review-tag-added").TextContent.Should().Contain("dotnet");
    }

    [Fact]
    public void Render_AuthorReview_ShowsOriginalAndFixedAuthor()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Author, original: "jane doe", fixedValue: "Jane Doe")]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find(".review-author-removed").TextContent.Should().Be("jane doe");
        cut.Find(".review-author-added").TextContent.Should().Be("Jane Doe");
    }

    [Fact]
    public void Render_ValidationReview_TruncatesLongValuesAndKeepsFullTextInTitle()
    {
        // Arrange
        var longValue = new string('x', 100);
        SetupReviews([CreateReview(1, ContentReviewChangeType.Validation, original: longValue, fixedValue: "short")]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        var values = cut.FindAll(".review-value-text");
        values[0].TextContent.Should().HaveLength(78).And.EndWith("…");
        values[0].GetAttribute("title").Should().Be(longValue);
        values[1].ClassList.Should().Contain("review-value-warning");
    }

    // ── Approve / reject ────────────────────────────────────────────────────

    [Fact]
    public void Approve_Success_ShowsSuccessMessageAndReloads()
    {
        // Arrange
        SetupReviews([CreateReview(7)]);
        _api.Setup(x => x.ApproveContentReviewAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminReviews>();

        // Act
        FindRowButtons(cut)[0].Click();

        // Assert
        _api.Verify(x => x.ApproveContentReviewAsync(7, It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetContentReviewsAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        var alert = cut.Find(".admin-alert");
        alert.ClassList.Should().Contain("admin-alert-success");
        alert.TextContent.Should().Contain("Approved review for [blogs/my-article] (tags)");
    }

    [Fact]
    public void Approve_NotFound_ShowsErrorMessage()
    {
        // Arrange
        SetupReviews([CreateReview(7)]);
        _api.Setup(x => x.ApproveContentReviewAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var cut = Render<AdminReviews>();

        // Act
        FindRowButtons(cut)[0].Click();

        // Assert
        var alert = cut.Find(".admin-alert");
        alert.ClassList.Should().Contain("admin-alert-error");
        alert.TextContent.Should().Contain("Review 7 not found or already processed");
    }

    [Fact]
    public void Approve_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupReviews([CreateReview(7)]);
        _api.Setup(x => x.ApproveContentReviewAsync(7, It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminReviews>();

        // Act
        FindRowButtons(cut)[0].Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to approve: boom");
    }

    [Fact]
    public void Reject_Success_ShowsSuccessMessage()
    {
        // Arrange
        SetupReviews([CreateReview(8)]);
        _api.Setup(x => x.RejectContentReviewAsync(8, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminReviews>();

        // Act
        FindRowButtons(cut)[1].Click();

        // Assert
        _api.Verify(x => x.RejectContentReviewAsync(8, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Rejected review for [blogs/my-article] (tags)");
    }

    [Fact]
    public void Reject_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupReviews([CreateReview(8)]);
        _api.Setup(x => x.RejectContentReviewAsync(8, It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminReviews>();

        // Act
        FindRowButtons(cut)[1].Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to reject: boom");
    }

    [Fact]
    public void BulkButtons_ShowPendingCount()
    {
        // Arrange
        SetupReviews([CreateReview(1)], new ContentReviewSummary { Pending = 12 });

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find(".admin-actions .btn").TextContent.Should().Contain("Approve All (12)");
        cut.Find(".admin-actions .btn-danger").TextContent.Should().Contain("Reject All (12)");
    }

    [Fact]
    public void BulkButtons_NoPendingReviews_AreHidden()
    {
        // Arrange
        SetupReviews([CreateReview(1, status: ContentReviewStatus.Approved)], new ContentReviewSummary { Approved = 1 });

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.FindAll(".admin-actions .btn-danger").Should().BeEmpty();
        cut.FindAll(".admin-actions .btn").Should().BeEmpty();
    }

    [Fact]
    public void ApproveAll_Click_CallsApiAndShowsCount()
    {
        // Arrange
        SetupReviews([CreateReview(1)], new ContentReviewSummary { Pending = 3 });
        _api.Setup(x => x.ApproveAllContentReviewsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(3);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find(".admin-actions .btn").Click();

        // Assert
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Approved 3 reviews and applied changes");
    }

    [Fact]
    public void RejectAll_Click_CallsApiAndShowsCount()
    {
        // Arrange
        SetupReviews([CreateReview(1)], new ContentReviewSummary { Pending = 3 });
        _api.Setup(x => x.RejectAllContentReviewsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(3);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find(".admin-actions .btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Rejected 3 reviews");
    }

    [Fact]
    public void ApproveAll_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupReviews([CreateReview(1)], new ContentReviewSummary { Pending = 3 });
        _api.Setup(x => x.ApproveAllContentReviewsAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminReviews>();

        // Act
        cut.Find(".admin-actions .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to approve all: boom");
    }

    // ── Markdown preview and diff ───────────────────────────────────────────

    [Fact]
    public void Markdown_DiffButton_OpensDiffModalWithLineClasses()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown, original: "same\nold line", fixedValue: "same\nnew line")]);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='View diff']").Click();

        // Assert
        cut.Find(".admin-modal-title").TextContent.Should().Be("Diff — my-article");
        cut.FindAll(".diff-line-equal").Should().ContainSingle();
        cut.Find(".diff-line-delete").TextContent.Should().Contain("old line");
        cut.Find(".diff-line-insert").TextContent.Should().Contain("new line");
    }

    [Fact]
    public void Markdown_OriginalButton_ShowsRawMarkdownWithoutCallingApi()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown, original: "# Original <b>", fixedValue: "# Fixed")]);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='View original markdown']").Click();

        // Assert
        cut.Find("pre.review-raw-markdown").TextContent.Should().Be("# Original <b>");
        _api.Verify(x => x.PreviewMarkdownAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Markdown_FixedButton_RendersHtmlReturnedByApi()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown, original: "# Original", fixedValue: "# Fixed")]);
        _api.Setup(x => x.PreviewMarkdownAsync("# Fixed", It.IsAny<CancellationToken>())).ReturnsAsync("<h1>Fixed</h1>");
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='View rendered result']").Click();

        // Assert
        cut.Find(".admin-modal-title").TextContent.Should().Be("Edited preview — my-article");
        cut.Find("article.admin-preview-article h1").TextContent.Should().Be("Fixed");
    }

    [Fact]
    public void Markdown_FixedButton_PreviewFails_ShowsErrorAsRawText()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown, original: "# Original", fixedValue: "# Fixed")]);
        _api.Setup(x => x.PreviewMarkdownAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("render failed"));
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='View rendered result']").Click();

        // Assert
        cut.Find("pre.review-raw-markdown").TextContent.Should().Contain("Failed to render preview: render failed");
    }

    [Fact]
    public void PreviewModal_CloseButton_ClosesModal()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown)]);
        var cut = Render<AdminReviews>();
        cut.Find("button[title='View diff']").Click();

        // Act
        cut.Find(".admin-modal-footer .btn-secondary").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
    }

    [Fact]
    public void Markdown_PendingReview_ShowsNoContentItemEditorButtonInActions()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Markdown)]);

        // Act
        var cut = Render<AdminReviews>();

        // Assert
        FindRowButtons(cut).Should().HaveCount(2, "markdown reviews are edited through the proposed-change editor instead");
    }

    // ── Proposed-change editor ──────────────────────────────────────────────

    [Fact]
    public void EditProposedChange_Click_OpensEditorPrefilledWithFixedValue()
    {
        // Arrange
        SetupReviews([CreateReview(5, ContentReviewChangeType.Markdown, original: "old", fixedValue: "proposed")]);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='Edit proposed change']").Click();

        // Assert
        cut.Find(".admin-modal-title").TextContent.Should().Contain("Edit proposed change — my-article");
        GetTextAreaValue(cut.Find("textarea.review-textarea-code")).Should().Be("proposed");
    }

    [Fact]
    public void EditProposedChange_Save_SendsEditedTextAndClosesEditor()
    {
        // Arrange
        SetupReviews([CreateReview(5, ContentReviewChangeType.Markdown, original: "old", fixedValue: "proposed")]);
        _api.Setup(x => x.UpdateContentReviewFixedValueAsync(5, "edited", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminReviews>();
        cut.Find("button[title='Edit proposed change']").Click();
        cut.Find("textarea.review-textarea-code").Input("edited");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(x => x.UpdateContentReviewFixedValueAsync(5, "edited", It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".admin-modal").Should().BeEmpty();
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Updated proposed change for review 5.");
    }

    [Fact]
    public void EditProposedChange_SaveNotFound_KeepsEditorOpenAndShowsError()
    {
        // Arrange
        SetupReviews([CreateReview(5, ContentReviewChangeType.Markdown)]);
        _api.Setup(x => x.UpdateContentReviewFixedValueAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var cut = Render<AdminReviews>();
        cut.Find("button[title='Edit proposed change']").Click();

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Review 5 not found.");
        cut.FindAll(".admin-modal").Should().ContainSingle();
    }

    [Fact]
    public void EditProposedChange_Cancel_ClosesEditorWithoutSaving()
    {
        // Arrange
        SetupReviews([CreateReview(5, ContentReviewChangeType.Markdown)]);
        var cut = Render<AdminReviews>();
        cut.Find("button[title='Edit proposed change']").Click();

        // Act
        cut.Find(".admin-modal-footer .btn-secondary").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
        _api.Verify(x => x.UpdateContentReviewFixedValueAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Content item editor ─────────────────────────────────────────────────

    [Fact]
    public void EditContentItem_NotFound_ShowsErrorMessage()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Tags)]);
        _api.Setup(x => x.GetContentItemEditDataAsync("blogs", "my-article", It.IsAny<CancellationToken>())).ReturnsAsync((ContentItemEditData?)null);
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Content item not found.");
    }

    [Fact]
    public void EditContentItem_LoadsFeedNamesOnceAndEditData()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Tags), CreateReview(2, ContentReviewChangeType.Tags, slug: "other")]);
        _api.Setup(x => x.GetContentItemEditDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContentItemEditData
            {
                CollectionName = "blogs",
                Slug = "my-article",
                Title = "T",
                Author = "A",
                Excerpt = "E",
                Content = "C",
                PrimarySectionName = "ai",
                Tags = [],
                Sections = [],
            });
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FeedConfig { Id = 1, Name = "Z feed" }, new FeedConfig { Id = 2, Name = "A feed" }]);
        var cut = Render<AdminReviews>();

        // Act
        cut.FindAll("button[title='Edit content item']")[0].Click();
        cut.FindAll("button[title='Edit content item']")[1].Click();

        // Assert
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetContentItemEditDataAsync("blogs", "my-article", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetContentItemEditDataAsync("blogs", "other", It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".admin-alert").Should().BeEmpty();
    }

    [Fact]
    public void EditContentItem_ApiFails_ShowsErrorMessage()
    {
        // Arrange
        SetupReviews([CreateReview(1, ContentReviewChangeType.Tags)]);
        _api.Setup(x => x.GetContentItemEditDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminReviews>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load content item: boom");
    }

    private static IReadOnlyList<AngleSharp.Dom.IElement> FindRowButtons(IRenderedComponent<AdminReviews> cut) =>
        cut.FindAll(".review-actions-cell button").ToList();

    // Blazor renders a bound textarea value either as a value attribute or as text content depending on the renderer.
    private static string GetTextAreaValue(AngleSharp.Dom.IElement textarea) =>
        textarea.GetAttribute("value") ?? textarea.TextContent;

    private static ContentReview CreateReview(
        long id,
        string changeType = ContentReviewChangeType.Tags,
        string status = ContentReviewStatus.Pending,
        string slug = "my-article",
        string original = ",ai,",
        string fixedValue = ",ai,azure,") =>
        new()
        {
            Id = id,
            Slug = slug,
            CollectionName = "blogs",
            ChangeType = changeType,
            OriginalValue = original,
            FixedValue = fixedValue,
            Status = status,
            CreatedAt = new DateTimeOffset(2026, 1, 2, 3, 4, 0, TimeSpan.Zero),
        };

    private void SetupReviews(IReadOnlyList<ContentReview> reviews, ContentReviewSummary? summary = null)
    {
        _api.Setup(x => x.GetContentReviewsAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviews);
        _api.Setup(x => x.GetContentReviewSummaryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(summary ?? new ContentReviewSummary { Pending = reviews.Count(r => r.Status == ContentReviewStatus.Pending) });
    }
}
