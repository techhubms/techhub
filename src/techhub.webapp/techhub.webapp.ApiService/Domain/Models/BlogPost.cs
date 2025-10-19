namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents a blog post or technical article.
/// </summary>
public class BlogPost : ContentItem
{
    public BlogPost()
    {
        CollectionType = "posts";
    }
}
