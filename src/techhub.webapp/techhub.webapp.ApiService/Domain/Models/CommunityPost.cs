namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents community posts and XPRT Magazine editions.
/// </summary>
public class CommunityPost : ContentItem
{
    public CommunityPost()
    {
        CollectionType = "community";
    }

    /// <summary>
    /// AI-generated magazine description (optional).
    /// </summary>
    public string? MagazineDescriptionAi { get; set; }

    /// <summary>
    /// PDF download link for magazines (optional).
    /// </summary>
    public string? DownloadUrl { get; set; }
}
