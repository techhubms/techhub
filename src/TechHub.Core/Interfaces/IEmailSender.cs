namespace TechHub.Core.Interfaces;

/// <summary>
/// Abstraction over the ACS email delivery so that newsletter delivery can be tested without outbound network calls.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends a single email message.
    /// Returns <c>true</c> when the provider reports success, <c>false</c> otherwise.
    /// </summary>
    Task<bool> SendAsync(string recipientEmail, string subject, string html, string plainText, CancellationToken ct = default);
}
