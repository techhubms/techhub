namespace TechHub.Web.Configuration;

/// <summary>
/// Configuration for the newsletter subscription modal.
/// Maps to the "Newsletter" section in appsettings.json.
/// </summary>
public sealed class NewsletterSettings
{
    public const string SectionName = "Newsletter";

    /// <summary>
    /// When false (default), the modal displays a "coming soon" message instead of the subscription form.
    /// Set to true once the Mailchimp account and interest groups are configured.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Mailchimp form POST URL.
    /// Example: https://yourdomain.us1.list-manage.com/subscribe/post
    /// </summary>
    public string FormActionUrl { get; init; } = string.Empty;

    /// <summary>
    /// Mailchimp audience user ID (the 'u' parameter in the embedded form).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Mailchimp audience list ID (the 'id' parameter in the embedded form).
    /// </summary>
    public string ListId { get; init; } = string.Empty;

    /// <summary>
    /// Per-topic interest group configuration.
    /// Key is the section slug (e.g. "ai", "github-copilot").
    /// </summary>
    public Dictionary<string, NewsletterTopic> Topics { get; init; } = [];
}

/// <summary>
/// Configuration for a single newsletter topic / Mailchimp interest group.
/// </summary>
public sealed class NewsletterTopic
{
    /// <summary>
    /// Human-readable label shown in the modal checkbox (e.g. "AI", "GitHub Copilot").
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Mailchimp interest group ID. Used as the checkbox input name: group[GroupId][GroupValue].
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Mailchimp interest group value. Used as the checkbox input value.
    /// </summary>
    public string GroupValue { get; init; } = string.Empty;
}
