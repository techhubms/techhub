using Azure;
using Azure.Communication.Email;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Infrastructure.Tests.Services;

public class AcsEmailSenderTests
{
    private const string Endpoint = "https://example.communication.azure.com/";
    private const string SenderAddress = "DoNotReply@example.azurecomm.net";

    // ── Constructor guards ─────────────────────────────────────────────────

    [Fact]
    public void Constructor_NullEmailClient_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new AcsEmailSender(null!, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("emailClient");
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new AcsEmailSender(new Mock<EmailClient>().Object, null!, NullLogger<AcsEmailSender>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("options");
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new AcsEmailSender(new Mock<EmailClient>().Object, CreateOptions(), null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    // ── Configuration guard ────────────────────────────────────────────────

    [Theory]
    [InlineData("", SenderAddress)]
    [InlineData("   ", SenderAddress)]
    [InlineData(Endpoint, "")]
    [InlineData(Endpoint, "   ")]
    public async Task SendAsync_EndpointOrSenderNotConfigured_ReturnsFalseWithoutCallingAcs(string endpoint, string senderAddress)
    {
        // Arrange
        var client = new Mock<EmailClient>(MockBehavior.Strict);
        var sut = new AcsEmailSender(client.Object, CreateOptions(endpoint, senderAddress), NullLogger<AcsEmailSender>.Instance);

        // Act
        var result = await sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        client.VerifyNoOtherCalls();
    }

    // ── Success path ───────────────────────────────────────────────────────

    [Fact]
    public async Task SendAsync_AcsReportsSucceeded_ReturnsTrueAndSendsExpectedMessage()
    {
        // Arrange
        EmailMessage? captured = null;
        var client = CreateClient(EmailSendStatus.Succeeded, message => captured = message);
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        var result = await sut.SendAsync("to@example.com", "Weekly digest", "<p>Hello</p>", "Hello", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        captured.Should().NotBeNull();
        captured!.SenderAddress.Should().Be(SenderAddress);
        captured.Content.Subject.Should().Be("Weekly digest");
        captured.Content.Html.Should().Be("<p>Hello</p>");
        captured.Content.PlainText.Should().Be("Hello");
        captured.Recipients.To.Should().ContainSingle().Which.Address.Should().Be("to@example.com");
    }

    [Fact]
    public async Task SendAsync_AcsReportsSucceeded_WaitsForCompletion()
    {
        // Arrange
        var client = CreateClient(EmailSendStatus.Succeeded);
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        await sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        client.Verify(
            c => c.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ── Non-success delivery status ────────────────────────────────────────

    [Theory]
    [InlineData("Failed")]
    [InlineData("Canceled")]
    [InlineData("NotStarted")]
    [InlineData("Running")]
    public async Task SendAsync_AcsReportsNonSuccessStatus_ReturnsFalse(string statusName)
    {
        // Arrange
        var client = CreateClient(ParseStatus(statusName));
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        var result = await sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    // ── ACS errors ─────────────────────────────────────────────────────────

    [Fact]
    public async Task SendAsync_AcsRateLimited_ReturnsFalse()
    {
        // Arrange
        var client = CreateFailingClient(new RequestFailedException(status: 429, message: "Too many requests"));
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        var result = await sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(500)]
    [InlineData(503)]
    public async Task SendAsync_AcsRequestFailed_ReturnsFalse(int status)
    {
        // Arrange
        var client = CreateFailingClient(new RequestFailedException(status, "ACS failure"));
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        var result = await sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_UnexpectedException_Propagates()
    {
        // Arrange
        var client = CreateFailingClient(new InvalidOperationException("boom"));
        var sut = new AcsEmailSender(client.Object, CreateOptions(), NullLogger<AcsEmailSender>.Instance);

        // Act
        var act = () => sut.SendAsync("to@example.com", "Subject", "<p>Html</p>", "Text", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private static OptionsWrapper<NewsletterOptions> CreateOptions(string endpoint = Endpoint, string senderAddress = SenderAddress) =>
        new(new NewsletterOptions
        {
            Endpoint = endpoint,
            SenderAddress = senderAddress,
            SendDelayMs = 0 // avoid the post-send throttle delay in tests
        });

    private static EmailSendStatus ParseStatus(string statusName) => statusName switch
    {
        "Failed" => EmailSendStatus.Failed,
        "Canceled" => EmailSendStatus.Canceled,
        "NotStarted" => EmailSendStatus.NotStarted,
        "Running" => EmailSendStatus.Running,
        "Succeeded" => EmailSendStatus.Succeeded,
        _ => throw new ArgumentOutOfRangeException(nameof(statusName), statusName, "Unknown status")
    };

    private static Mock<EmailClient> CreateClient(EmailSendStatus status, Action<EmailMessage>? onSend = null)
    {
        var operation = new Mock<EmailSendOperation>();
        operation.Setup(o => o.Value).Returns(EmailModelFactory.EmailSendResult("operation-id", status));

        var client = new Mock<EmailClient>();
        client
            .Setup(c => c.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .Callback<WaitUntil, EmailMessage, CancellationToken>((_, message, _) => onSend?.Invoke(message))
            .ReturnsAsync(operation.Object);
        return client;
    }

    private static Mock<EmailClient> CreateFailingClient(Exception exception)
    {
        var client = new Mock<EmailClient>();
        client
            .Setup(c => c.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        return client;
    }
}
