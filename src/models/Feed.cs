using System;
using System.Collections.Generic;

namespace TechHub.Models;

public class Feed
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required string Description { get; set; }
    public required string Author { get; set; }
    public required string CanonicalUrl { get; set; }
    public required string ViewingMode { get; set; }
    public required string FeedName { get; set; }
    public required string FeedUrl { get; set; }
    public DateTime PublishedDate { get; set; }
    public required string Permalink { get; set; }
    public required string Page { get; set; }
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Tags { get; set; } = new List<string>();
    public List<string> TagsNormalized { get; set; } = new List<string>();
    public required string Layout { get; set; }
    public required string ExcerptSeparator { get; set; }
}