using System.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Tests.Services;

public class StartupBackgroundServiceTests
{
    [Fact]
    public async Task StartupBackgroundService_WhenMigrationsFail_ShouldStopApplication()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockHostLifetime = new Mock<IHostApplicationLifetime>();
        var mockStartupState = new StartupStateService();
        var mockLogger = new Mock<ILogger<StartupBackgroundService>>();

        var mockMigrationRunner = new Mock<IMigrationRunner>();
        mockMigrationRunner.Setup(m => m.RunMigrationsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Migration failed"));

        var mockConnectionFactory = new Mock<IDbConnectionFactory>();
        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(Mock.Of<IDbConnection>());

        services.AddSingleton(mockMigrationRunner.Object);
        services.AddSingleton(mockConnectionFactory.Object);
        services.AddSingleton(mockHostLifetime.Object);
        services.AddSingleton(mockStartupState);

        var serviceProvider = services.BuildServiceProvider();

        var backgroundService = new StartupBackgroundService(
            serviceProvider,
            mockStartupState,
            mockHostLifetime.Object,
            mockLogger.Object);

        // Act
        await backgroundService.StartAsync(CancellationToken.None);
        await Task.Delay(500, TestContext.Current.CancellationToken);

        // Assert
        mockHostLifetime.Verify(l => l.StopApplication(), Times.Once);
        mockStartupState.IsStartupCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task StartupBackgroundService_WhenSuccessful_ShouldNotStopApplication()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockHostLifetime = new Mock<IHostApplicationLifetime>();
        var mockStartupState = new StartupStateService();
        var mockLogger = new Mock<ILogger<StartupBackgroundService>>();

        var mockMigrationRunner = new Mock<IMigrationRunner>();
        mockMigrationRunner.Setup(m => m.RunMigrationsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();
        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(mockConnection.Object);

        // Mock for LogDatabaseRecordCounts - uses IDbCommand.ExecuteScalar
        mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
        {
            var mockCommand = new Mock<IDbCommand>();
            mockCommand.SetupProperty(cmd => cmd.CommandText);
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(100);
            return mockCommand.Object;
        });

        var mockJobRepo = new Mock<IContentProcessingJobRepository>();
        mockJobRepo.Setup(r => r.AbortRunningJobsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var mockCustomPageRepo = new Mock<ICustomPageDataRepository>();
        mockCustomPageRepo.Setup(r => r.UpsertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var customDir = Path.Combine(Path.GetTempPath(), "_custom");
        Directory.CreateDirectory(customDir);
        var appSettings = Options.Create(new AppSettings
        {
            BaseUrl = "https://localhost",
            Content = new ContentSettings
            {
                CollectionsPath = Path.GetTempPath(),
                Sections = []
            }
        });

        services.AddSingleton(mockMigrationRunner.Object);
        services.AddSingleton(mockConnectionFactory.Object);
        services.AddSingleton(mockJobRepo.Object);
        services.AddSingleton(mockCustomPageRepo.Object);
        services.AddSingleton<IOptions<AppSettings>>(appSettings);
        services.AddSingleton(mockHostLifetime.Object);
        services.AddSingleton(mockStartupState);

        var serviceProvider = services.BuildServiceProvider();

        var backgroundService = new StartupBackgroundService(
            serviceProvider,
            mockStartupState,
            mockHostLifetime.Object,
            mockLogger.Object);

        // Act
        await backgroundService.StartAsync(CancellationToken.None);
        await Task.Delay(500, TestContext.Current.CancellationToken);

        // Assert
        mockHostLifetime.Verify(l => l.StopApplication(), Times.Never);
        mockStartupState.IsStartupCompleted.Should().BeTrue();
        mockJobRepo.Verify(r => r.AbortRunningJobsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

