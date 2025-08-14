using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechHub.Data;
using TechHub.Models;
using TechHub.Services;

// Create a host builder for dependency injection and configuration
var builder = Host.CreateApplicationBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add Entity Framework with In-Memory database
builder.Services.AddDbContext<TechHubDbContext>(options =>
    options.UseInMemoryDatabase("TechHubInMemoryDb"));

// Register services
builder.Services.AddScoped<IFeedService, FeedService>();
builder.Services.AddScoped<IArticleService, ArticleService>();

// Build the host
using var host = builder.Build();

// Get services
using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<TechHubDbContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Starting TechHub application...");
    
    // Ensure database is created
    await dbContext.Database.EnsureCreatedAsync();
    logger.LogInformation("Database ensured to exist.");

    // Example: Add some sample data
    if (!await dbContext.Articles.AnyAsync())
    {
        logger.LogInformation("Adding sample data...");
        
        var aiTag = new Tag { Name = "AI", Description = "Artificial Intelligence" };
        var copilotTag = new Tag { Name = "GitHub Copilot", Description = "GitHub's AI-powered code assistant" };
        
        var article = new Article 
        { 
            Title = "Getting Started with Entity Framework Core",
            Content = "Entity Framework Core is a modern object-database mapper for .NET...",
            Author = "Tech Hub Team",
            PublishedDate = DateTime.UtcNow,
            Tags = new List<Tag> { aiTag, copilotTag }
        };

        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Sample data added successfully.");
    }

    // Example: Add sample feed data
    if (!await dbContext.Feeds.AnyAsync())
    {
        logger.LogInformation("Adding sample feed data...");
        
        var sampleFeed = new Feed
        {
            Title = "Sample Tech Article",
            Content = "This is a sample article content from an RSS feed...",
            Description = "A sample article to demonstrate Entity Framework integration",
            Author = "Sample Author",
            CanonicalUrl = "https://example.com/sample-article",
            ViewingMode = "external",
            FeedName = "Sample Tech Feed",
            FeedUrl = "https://example.com/feed",
            PublishedDate = DateTime.UtcNow,
            Permalink = "/sample-article.html",
            Page = "news",
            Categories = new List<string> { "AI", "Technology" },
            Tags = new List<string> { "Entity Framework", "Database", "Development" },
            TagsNormalized = new List<string> { "entity framework", "database", "development" },
            Layout = "post",
            ExcerptSeparator = "<!--excerpt_end-->"
        };

        dbContext.Feeds.Add(sampleFeed);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Sample feed data added successfully.");
    }

    // Example: Query data
    var articles = await dbContext.Articles
        .Include(a => a.Tags)
        .ToListAsync();

    var feeds = await dbContext.Feeds.ToListAsync();

    logger.LogInformation($"Found {articles.Count} articles and {feeds.Count} feeds in the database:");
    
    foreach (var article in articles)
    {
        logger.LogInformation($"- Article: {article.Title} by {article.Author}");
        logger.LogInformation($"  Tags: {string.Join(", ", article.Tags.Select(t => t.Name))}");
    }

    foreach (var feed in feeds)
    {
        logger.LogInformation($"- Feed: {feed.Title} by {feed.Author}");
        logger.LogInformation($"  Categories: {string.Join(", ", feed.Categories)}");
        logger.LogInformation($"  Tags: {string.Join(", ", feed.Tags)}");
    }

    Console.WriteLine("Entity Framework setup completed successfully!");
    Console.WriteLine($"Database contains {articles.Count} articles and {feeds.Count} feeds.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while running the application.");
    Console.WriteLine($"Error: {ex.Message}");
}

logger.LogInformation("TechHub application completed.");
