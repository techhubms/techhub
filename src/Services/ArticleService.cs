using Microsoft.EntityFrameworkCore;
using TechHub.Data;
using TechHub.Models;

namespace TechHub.Services;

public interface IArticleService
{
    Task<IEnumerable<Article>> GetAllArticlesAsync();
    Task<Article?> GetArticleByIdAsync(int id);
    Task<Article> CreateArticleAsync(Article article);
    Task<Article?> UpdateArticleAsync(int id, Article article);
    Task<bool> DeleteArticleAsync(int id);
    Task<IEnumerable<Article>> GetArticlesByTagAsync(string tagName);
}

public class ArticleService : IArticleService
{
    private readonly TechHubDbContext _context;

    public ArticleService(TechHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        return await _context.Articles
            .Include(a => a.Tags)
            .OrderByDescending(a => a.PublishedDate)
            .ToListAsync();
    }

    public async Task<Article?> GetArticleByIdAsync(int id)
    {
        return await _context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Article> CreateArticleAsync(Article article)
    {
        // Handle existing tags
        var existingTags = new List<Tag>();
        foreach (var tag in article.Tags)
        {
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == tag.Name);
            
            if (existingTag != null)
            {
                existingTags.Add(existingTag);
            }
            else
            {
                existingTags.Add(tag);
            }
        }
        
        article.Tags = existingTags;
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<Article?> UpdateArticleAsync(int id, Article article)
    {
        var existingArticle = await _context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (existingArticle == null)
            return null;

        existingArticle.Title = article.Title;
        existingArticle.Content = article.Content;
        existingArticle.Author = article.Author;
        existingArticle.PublishedDate = article.PublishedDate;
        existingArticle.UpdatedDate = DateTime.UtcNow;

        // Update tags
        existingArticle.Tags.Clear();
        foreach (var tag in article.Tags)
        {
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == tag.Name);
            
            existingArticle.Tags.Add(existingTag ?? tag);
        }

        await _context.SaveChangesAsync();
        return existingArticle;
    }

    public async Task<bool> DeleteArticleAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return false;

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Article>> GetArticlesByTagAsync(string tagName)
    {
        return await _context.Articles
            .Include(a => a.Tags)
            .Where(a => a.Tags.Any(t => t.Name == tagName))
            .OrderByDescending(a => a.PublishedDate)
            .ToListAsync();
    }
}
