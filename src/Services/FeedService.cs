using Microsoft.EntityFrameworkCore;
using TechHub.Data;
using TechHub.Models;

namespace TechHub.Services;

public interface IFeedService
{
    Task<IEnumerable<Feed>> GetAllFeedsAsync();
    Task<Feed?> GetFeedByIdAsync(int id);
    Task<Feed> CreateFeedAsync(Feed feed);
    Task<Feed?> UpdateFeedAsync(int id, Feed feed);
    Task<bool> DeleteFeedAsync(int id);
    Task<IEnumerable<Feed>> SearchFeedsByTagAsync(string tag);
}

public class FeedService : IFeedService
{
    private readonly TechHubDbContext _context;

    public FeedService(TechHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Feed>> GetAllFeedsAsync()
    {
        return await _context.Feeds
            .OrderByDescending(f => f.PublishedDate)
            .ToListAsync();
    }

    public async Task<Feed?> GetFeedByIdAsync(int id)
    {
        return await _context.Feeds.FindAsync(id);
    }

    public async Task<Feed> CreateFeedAsync(Feed feed)
    {
        _context.Feeds.Add(feed);
        await _context.SaveChangesAsync();
        return feed;
    }

    public async Task<Feed?> UpdateFeedAsync(int id, Feed feed)
    {
        var existingFeed = await _context.Feeds.FindAsync(id);
        if (existingFeed == null)
            return null;

        existingFeed.Title = feed.Title;
        existingFeed.Content = feed.Content;
        existingFeed.Description = feed.Description;
        existingFeed.Author = feed.Author;
        existingFeed.Categories = feed.Categories;
        existingFeed.Tags = feed.Tags;
        existingFeed.TagsNormalized = feed.TagsNormalized;
        existingFeed.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingFeed;
    }

    public async Task<bool> DeleteFeedAsync(int id)
    {
        var feed = await _context.Feeds.FindAsync(id);
        if (feed == null)
            return false;

        _context.Feeds.Remove(feed);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Feed>> SearchFeedsByTagAsync(string tag)
    {
        return await _context.Feeds
            .Where(f => f.Tags.Contains(tag) || f.TagsNormalized.Contains(tag.ToLower()))
            .OrderByDescending(f => f.PublishedDate)
            .ToListAsync();
    }
}
