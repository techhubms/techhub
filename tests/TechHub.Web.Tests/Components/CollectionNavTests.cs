using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Moq;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using TechHub.Web.Services;
using Xunit;

namespace TechHub.Web.Tests.Components;

public class CollectionNavTests : TestContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;
    private readonly Mock<ILogger<CollectionNav>> _mockLogger;
    private readonly Mock<IJSRuntime> _mockJsRuntime;

    public CollectionNavTests()
    {
        _mockApiClient = new Mock<TechHubApiClient>(MockBehavior.Loose, new HttpClient(), Mock.Of<ILogger<TechHubApiClient>>());
        _mockLogger = new Mock<ILogger<CollectionNav>>();
        _mockJsRuntime = new Mock<IJSRuntime>();
        
        Services.AddSingleton(_mockApiClient.Object);
        Services.AddSingleton(_mockLogger.Object);
        Services.AddSingleton(_mockJsRuntime.Object);
    }

    [Fact]
    public void Component_ShowsSkeleton_WhenLoading()
    {
        // Arrange
        var taskCompletionSource = new TaskCompletionSource<SectionDto?>();
        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .Returns(taskCompletionSource.Task);

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        // Assert
        var skeletonItems = cut.FindAll(".skeleton-nav-item");
        Assert.Equal(3, skeletonItems.Count); // 3 skeleton placeholders
    }

    [Fact]
    public async Task Component_DisplaysCollections_WhenLoaded()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "news", Collection = "News", Url = "null", Description = "" },
            new CollectionReferenceDto { Title = "videos", Collection = "Videos", Url = "null", Description = "" },
            new CollectionReferenceDto { Title = "blogs", Collection = "Blogs", Url = "null", Description = "" }
        };

        var sectionDto = new SectionDto { Id = "ai", Title = "AI", Description = "AI Description", Url = "/`ai", Category = "AI", BackgroundImage = "/images/ai-bg.jpg", Collections = collections
         };

        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        // Wait for rendering
        await Task.Delay(100);

        // Assert
        var buttons = cut.FindAll("button");
        Assert.Equal(4, buttons.Count); // "All" + 3 collections
        
        Assert.Contains("All", buttons[0].TextContent);
        Assert.Contains("News", buttons[1].TextContent);
        Assert.Contains("Videos", buttons[2].TextContent);
        Assert.Contains("Blogs", buttons[3].TextContent);
    }

    [Fact]
    public async Task Component_HighlightsSelectedCollection()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "news", Collection = "News", Url = "null", Description = "" }
        };

        var sectionDto = new SectionDto { Id = "ai", Title = "AI", Description = "AI Description", Url = "/`ai", Category = "AI", BackgroundImage = "/images/ai-bg.jpg", Collections = collections
         };

        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "news")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        await Task.Delay(100);

        // Assert
        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        Assert.Contains("active", newsButton.ClassName);
    }

    [Fact]
    public async Task Component_InvokesCallback_WhenCollectionSelected()
    {
        // Arrange
        string? selectedCollection = null;
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "news", Collection = "News", Url = "null", Description = "" }
        };

        var sectionDto = new SectionDto { Id = "ai", Title = "AI", Description = "AI Description", Url = "/`ai", Category = "AI", BackgroundImage = "/images/ai-bg.jpg", Collections = collections
         };

        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);

        _mockJsRuntime
            .Setup(x => x.InvokeAsync<object>(
                "history.pushState",
                It.IsAny<object[]>()))
            .ReturnsAsync(Mock.Of<object>());

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, value => selectedCollection = value)));

        await Task.Delay(100);

        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        newsButton.Click();

        // Assert
        Assert.Equal("news", selectedCollection);
    }

    [Fact]
    public async Task Component_UpdatesUrl_WhenCollectionSelected()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "news", Collection = "News", Url = "null", Description = "" }
        };

        var sectionDto = new SectionDto { Id = "ai", Title = "AI", Description = "AI Description", Url = "/`ai", Category = "AI", BackgroundImage = "/images/ai-bg.jpg", Collections = collections
         };

        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);

        _mockJsRuntime
            .Setup(x => x.InvokeAsync<object>(
                "history.pushState",
                It.Is<object[]>(args => 
                    args.Length == 3 && 
                    (string)args[2] == "/ai/news")))
            .ReturnsAsync(Mock.Of<object>())
            .Verifiable();

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        await Task.Delay(100);

        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        newsButton.Click();

        // Assert
        _mockJsRuntime.Verify();
    }

    [Fact]
    public async Task Component_DisablesButtons_WhenContentIsLoading()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "news", Collection = "News", Url = "null", Description = "" }
        };

        var sectionDto = new SectionDto { Id = "ai", Title = "AI", Description = "AI Description", Url = "/`ai", Category = "AI", BackgroundImage = "/images/ai-bg.jpg", Collections = collections
         };

        _mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, true)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        await Task.Delay(100);

        // Assert
        var buttons = cut.FindAll("button");
        Assert.All(buttons, button => Assert.True(button.HasAttribute("disabled")));
    }
}
