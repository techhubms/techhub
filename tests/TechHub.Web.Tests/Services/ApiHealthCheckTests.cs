using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

public class ApiHealthCheckTests
{
    [Fact]
    public async Task CheckHealthAsync_ReturnsHealthy_WhenApiAliveEndpointReturnsSuccess()
    {
        using var httpClient = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)))
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        var sut = new ApiHealthCheck(httpClient, NullLogger<ApiHealthCheck>.Instance);

        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsUnhealthy_WhenApiReturnsNonSuccessStatusCode()
    {
        using var httpClient = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.Forbidden)))
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        var sut = new ApiHealthCheck(httpClient, NullLogger<ApiHealthCheck>.Instance);

        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Contain("403");
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsUnhealthy_WhenApiIsUnreachable()
    {
        using var httpClient = new HttpClient(new StubHandler(_ => throw new HttpRequestException("Connection refused")))
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        var sut = new ApiHealthCheck(httpClient, NullLogger<ApiHealthCheck>.Instance);

        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Exception.Should().BeOfType<HttpRequestException>();
    }

    [Fact]
    public async Task CheckHealthAsync_Propagates_WhenCallerCancels()
    {
        using var cts = new CancellationTokenSource();
        using var httpClient = new HttpClient(new StubHandler(_ =>
        {
            cts.Cancel();
            throw new TaskCanceledException();
        }))
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        var sut = new ApiHealthCheck(httpClient, NullLogger<ApiHealthCheck>.Instance);

        var act = () => sut.CheckHealthAsync(new HealthCheckContext(), cts.Token);

        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    private sealed class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responder(request));
        }
    }
}
