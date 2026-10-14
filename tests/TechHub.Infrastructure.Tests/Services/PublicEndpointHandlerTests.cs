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
    [InlineData("192.0.2.0")]
    [InlineData("192.0.2.255")]
    [InlineData("198.51.100.0")]
    [InlineData("198.51.100.255")]
    [InlineData("203.0.113.0")]
    [InlineData("203.0.113.255")]
    [InlineData("192.88.99.0")]
    [InlineData("192.88.99.255")]
    [InlineData("240.0.0.0")]
    [InlineData("255.255.255.254")]
    [InlineData("::1")]
    [InlineData("::")]
    [InlineData("fe80::1")]
    [InlineData("fc00::1")]
    [InlineData("fd12:3456:789a::1")]
    [InlineData("ff02::1")]
    [InlineData("2001:db8::1")]
    [InlineData("2001:db8:ffff:ffff:ffff:ffff:ffff:ffff")]
    [InlineData("2001:0:ffff:ffff:ffff:ffff:ffff:ffff")]
    [InlineData("2001:1ff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [InlineData("2002::1")]
    [InlineData("2002:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [InlineData("3fff::1")]
    [InlineData("3fff:f::1")]
    [InlineData("5f00::1")]
    [InlineData("5f00:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [InlineData("100::1")]
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
    [InlineData("192.0.1.0")]
    [InlineData("192.0.3.0")]
    [InlineData("192.88.98.255")]
    [InlineData("192.88.100.0")]
    [InlineData("2606:4700:4700::1111")]
    [InlineData("2001:200::1")]
    [InlineData("2001:db9::1")]
    [InlineData("2003::1")]
    [InlineData("3fff:1000::1")]
    [InlineData("5f01::1")]
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

    [Fact]
    public void Create_DisablesProxy()
    {
        // Act
        using var handler = PublicEndpointHandler.Create();

        // Assert
        handler.UseProxy.Should().BeFalse();
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
