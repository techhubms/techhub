namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents a news article or announcement.
/// </summary>
public class NewsArticle : ContentItem
{
    public NewsArticle()
    {
        CollectionType = "news";
    }
}
