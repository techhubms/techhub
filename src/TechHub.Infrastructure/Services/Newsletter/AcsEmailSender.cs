using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.Newsletter;

/// <summary>
/// ACS-backed email sender. Returns false (and logs a warning) when configuration is missing
/// or when ACS reports a non-success status.
/// </summary>
public sealed class AcsEmailSender : IEmailSender
{
    private readonly EmailClient _emailClient;
    private readonly NewsletterOptions _options;
    private readonly ILogger<AcsEmailSender> _logger;

    public AcsEmailSender(EmailClient emailClient, IOptions<NewsletterOptions> options, ILogger<AcsEmailSender> logger)
    {
        ArgumentNullException.ThrowIfNull(emailClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _emailClient = emailClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<bool> SendAsync(string recipientEmail, string subject, string html, string plainText, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Endpoint) || string.IsNullOrWhiteSpace(_options.SenderAddress))
        {
            _logger.LogWarning("Newsletter email skipped because Newsletter:Endpoint or Newsletter:SenderAddress is not configured");
            return false;
        }

        var emailMessage = new EmailMessage(
            senderAddress: _options.SenderAddress,
            content: new EmailContent(subject)
            {
                Html = html,
                PlainText = plainText
            },
            recipients: new EmailRecipients([new EmailAddress(recipientEmail)]));

        EmailSendOperation operation;
        try
        {
            operation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage, ct);
        }
        catch (RequestFailedException ex) when (ex.Status == 429)
        {
            _logger.LogWarning(ex, "Newsletter email send rate-limited by ACS (429)");
            return false;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Newsletter email send failed with ACS error {Status}", ex.Status);
            return false;
        }

        if (operation.Value.Status != EmailSendStatus.Succeeded)
        {
            _logger.LogWarning("Newsletter email delivery completed with non-success status {Status}", operation.Value.Status);
            return false;
        }

        if (_options.SendDelayMs > 0)
        {
            await Task.Delay(_options.SendDelayMs, ct);
        }

        return true;
    }
}
