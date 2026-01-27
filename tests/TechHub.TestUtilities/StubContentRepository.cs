using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;

namespace TechHub.TestUtilities;

/// <summary>
/// Stub implementation of IContentRepository for integration tests.
/// Uses real markdown files from TestCollections directory instead of hardcoded data.
/// Inherits all filtering/search logic from FileBasedContentRepository.
/// Provides production-like test data from a curated subset of actual content files.
/// </summary>
public class StubContentRepository(
    IMarkdownService markdownService,
    ITagMatchingService tagMatchingService,
    IHostEnvironment environment,
    IMemoryCache cache) : FileBasedContentRepository(
        Options.Create(new AppSettings
        {
            Content = new ContentSettings
            {
                // Point to TestCollections directory in the TestUtilities assembly output
                CollectionsPath = Path.Combine(AppContext.BaseDirectory, "TestCollections"),
                Sections = [] // Empty sections - FileBasedContentRepository doesn't use this
            }
        }),
        markdownService,
        tagMatchingService,
        environment,
        cache)
{
    // No additional implementation needed - all logic is in base class
}
