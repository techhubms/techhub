using System.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Tests.Services;

public class StartupBackgroundServiceTests
{
    [Fact]
    public async Task StartupBackgroundService_WhenContentSyncFails_ShouldStopApplication()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockHostLifetime = new Mock<IHostApplicationLifetime>();
        var mockStartupState = new StartupStateService();
        var mockLogger = new Mock<ILogger<StartupBackgroundService>>();

        var mockMigrationRunner = new Mock<IMigrationRunner>();
        mockMigrationRunner.Setup(m => m.RunMigrationsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockContentSync = new Mock<IContentSyncService>();
        mockContentSync.Setup(s => s.SyncAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Content sync failed"));

        var mockConnectionFactory = new Mock<IDbConnectionFactory>();
        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(Mock.Of<IDbConnection>());

        services.AddSingleton(mockMigrationRunner.Object);
        services.AddSingleton(mockContentSync.Object);
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
        await Task.Delay(500);

        // Assert
        mockHostLifetime.Verify(l => l.StopApplication(), Times.Once);
        mockStartupState.IsMigrationsCompleted.Should().BeTrue();
        mockStartupState.IsContentSyncCompleted.Should().BeFalse();
    }

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

        var mockContentSync = new Mock<IContentSyncService>();
        var mockConnectionFactory = new Mock<IDbConnectionFactory>();
        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(Mock.Of<IDbConnection>());

        services.AddSingleton(mockMigrationRunner.Object);
        services.AddSingleton(mockContentSync.Object);
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
        await Task.Delay(500);

        // Assert
        mockHostLifetime.Verify(l => l.StopApplication(), Times.Once);
        mockStartupState.IsMigrationsCompleted.Should().BeFalse();
        mockStartupState.IsContentSyncCompleted.Should().BeFalse();
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

        var mockContentSync = new Mock<IContentSyncService>();
        mockContentSync.Setup(s => s.SyncAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SyncResult(5, 2, 0, 10, TimeSpan.FromSeconds(1)));

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

        services.AddSingleton(mockMigrationRunner.Object);
        services.AddSingleton(mockContentSync.Object);
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
        await Task.Delay(500);

        // Assert
        mockHostLifetime.Verify(l => l.StopApplication(), Times.Never);
        mockStartupState.IsMigrationsCompleted.Should().BeTrue();
        mockStartupState.IsContentSyncCompleted.Should().BeTrue();
    }
}

