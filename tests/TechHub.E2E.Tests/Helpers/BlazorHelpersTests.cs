using FluentAssertions;
using Microsoft.Playwright;
using Moq;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Helpers;

public class BlazorHelpersTests
{
    [Fact]
    public async Task GotoAndWaitForBlazorAsync_RetriesOnce_WhenCircuitReadinessTimesOut()
    {
        // Arrange
        var page = new Mock<IPage>();
        var context = new Mock<IBrowserContext>();
        var readyHandle = new Mock<IJSHandle>();
        page.SetupGet(p => p.Context).Returns(context.Object);
        page.Setup(p => p.GotoAsync(It.IsAny<string>(), It.IsAny<PageGotoOptions?>()))
            .ReturnsAsync((IResponse?)null);
        page.SetupSequence(p => p.WaitForFunctionAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<PageWaitForFunctionOptions?>()))
            .ThrowsAsync(new TimeoutException("The circuit did not start"))
            .ReturnsAsync(readyHandle.Object);

        // Act
        await page.Object.GotoAndWaitForBlazorAsync("https://example.test");

        // Assert
        page.Verify(p => p.GotoAsync("https://example.test", It.IsAny<PageGotoOptions?>()), Times.Exactly(2));
        page.Verify(p => p.WaitForFunctionAsync(
            It.IsAny<string>(),
            It.IsAny<object?>(),
            It.IsAny<PageWaitForFunctionOptions?>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GotoAndWaitForBlazorAsync_DoesNotRetryForever_WhenCircuitNeverStarts()
    {
        // Arrange
        var page = new Mock<IPage>();
        var context = new Mock<IBrowserContext>();
        page.SetupGet(p => p.Context).Returns(context.Object);
        page.Setup(p => p.GotoAsync(It.IsAny<string>(), It.IsAny<PageGotoOptions?>()))
            .ReturnsAsync((IResponse?)null);
        page.Setup(p => p.WaitForFunctionAsync(
                It.IsAny<string>(),
                It.IsAny<object?>(),
                It.IsAny<PageWaitForFunctionOptions?>()))
            .ThrowsAsync(new TimeoutException("The circuit did not start"));

        // Act
        var act = () => page.Object.GotoAndWaitForBlazorAsync("https://example.test");

        // Assert
        await act.Should().ThrowAsync<TimeoutException>().WithMessage("The circuit did not start*");
        page.Verify(p => p.GotoAsync("https://example.test", It.IsAny<PageGotoOptions?>()), Times.Exactly(2));
    }
}
