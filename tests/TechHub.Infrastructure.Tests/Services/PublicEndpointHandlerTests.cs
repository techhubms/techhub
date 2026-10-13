using System.Net;
using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

public class PublicEndpointHandlerTests
{
    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("127.255.255.254")]
    [InlineData("10.0.0.5")]
    [InlineData("172.16.0.1")]
    [InlineData("172.31.255.255")]
    [InlineData("192.168.1.1")]
    [InlineData("169.254.169.254")]
    [InlineData("100.64.0.1")]
    [InlineData("0.0.0.0")]
    [InlineData("224.0.0.1")]
    [InlineData("255.255.255.255")]
    [InlineData("::1")]
    [InlineData("::")]
    [InlineData("fe80::1")]
    [InlineData("fc00::1")]
    [InlineData("fd12:3456:789a::1")]
    [InlineData("ff02::1")]
    [InlineData("::ffff:127.0.0.1")]
    [InlineData("::ffff:169.254.169.254")]
    public void IsPublicAddress_NonPublicAddress_ReturnsFalse(string address)
    {
        // Arrange
        var ip = IPAddress.Parse(address);

        // Act
        var result = PublicEndpointHandler.IsPublicAddress(ip);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("8.8.8.8")]
    [InlineData("1.1.1.1")]
    [InlineData("172.32.0.1")]
    [InlineData("100.128.0.1")]
    [InlineData("2606:4700:4700::1111")]
    [InlineData("::ffff:8.8.8.8")]
    public void IsPublicAddress_PublicAddress_ReturnsTrue(string address)
    {
        // Arrange
        var ip = IPAddress.Parse(address);

        // Act
        var result = PublicEndpointHandler.IsPublicAddress(ip);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("http://127.0.0.1:9/")]
    [InlineData("http://[::1]:9/")]
    [InlineData("http://169.254.169.254/latest/meta-data/")]
    [InlineData("http://10.0.0.1/")]
    [InlineData("http://localhost:9/")]
    public async Task Handler_RequestToNonPublicHost_IsBlocked(string url)
    {
        // Arrange
        using var handler = PublicEndpointHandler.Create();
        using var client = new HttpClient(handler);

        // Act
        var act = () => client.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.Which.InnerException?.Message.Should().Contain("Blocked connection");
    }
}
