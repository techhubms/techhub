using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for FileBasedContentRepository - loading and querying content from markdown files.
/// Uses TestCollections directory for all test data - NO manual file creation.
/// Inherits all common tests from BaseContentRepositoryTests.
/// </summary>
public class FileBasedContentRepositoryTests : BaseContentRepositoryTests
{
    private readonly FileBasedContentRepository _repository;

    protected override IContentRepository Repository => _repository;

    public FileBasedContentRepositoryTests()
    {
        // Use TestCollections directory for all test data
        var testCollectionsPath = "/workspaces/techhub/tests/TechHub.TestUtilities/TestCollections";

        // Load real AppSettings from appsettings.json and override collections path
        var settings = ConfigurationHelper.LoadAppSettings(overrideCollectionsPath: testCollectionsPath);

        // Setup: Create mock IHostEnvironment
        var mockEnvironment = new Mock<IHostEnvironment>();
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(testCollectionsPath);

        // Setup: Create MemoryCache for caching
        var cache = new MemoryCache(new MemoryCacheOptions());

        // Setup: Create FileBasedContentRepository pointing to TestCollections
        var markdownService = new MarkdownService();
        _repository = new FileBasedContentRepository(
            Options.Create(settings),
            markdownService,
            mockEnvironment.Object,
            cache
        );
    }

    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
