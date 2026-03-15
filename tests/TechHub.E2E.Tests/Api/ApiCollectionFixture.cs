using TechHub.TestUtilities;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// Collection fixture for sharing WebApplicationFactory across all API E2E test classes.
/// This significantly reduces overhead by creating the factory once instead of per test class.
/// Performance impact: Reduces startup time from ~4s per test class to ~4s total for all API tests.
/// </summary>
public class ApiCollectionFixture : IDisposable
{
    public TechHubE2ETestApiFactory Factory { get; }

    public ApiCollectionFixture()
    {
        Factory = new TechHubE2ETestApiFactory();
    }

    public void Dispose()
    {
        Factory.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Collection definition for API E2E tests.
/// All API test classes decorated with [Collection("API E2E Tests")] will share the same factory instance.
/// </summary>
[CollectionDefinition("API E2E Tests")]
public class ApiCollection : ICollectionFixture<ApiCollectionFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
