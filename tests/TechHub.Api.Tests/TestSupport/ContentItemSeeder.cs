using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace TechHub.Api.Tests.TestSupport;

/// <summary>
/// Inserts minimal content items directly into the integration test database.
/// Callers must pass unique slugs / URLs so tests never share rows.
/// </summary>
internal static class ContentItemSeeder
{
    public static async Task InsertAsync(
        IServiceProvider services,
        string collectionName,
        string slug,
        string externalUrl,
        string title = "Seeded Item",
        string author = "Test Author",
        long dateEpoch = 1700000000)
    {
        ArgumentNullException.ThrowIfNull(services);

        await using var scope = services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            @"INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name,
                 tags_csv, is_ai, sections_bitmask, content_hash)
              VALUES
                (@Slug, @Collection, @Title, '# Seeded', 'Seeded excerpt', @DateEpoch,
                 'ai', @ExternalUrl, @Author, 'Test Feed',
                 ',ai,', TRUE, 1, @ContentHash)",
            new
            {
                Slug = slug,
                Collection = collectionName,
                Title = title,
                DateEpoch = dateEpoch,
                ExternalUrl = externalUrl,
                Author = author,
                ContentHash = $"hash-{slug}"
            });
    }
}
