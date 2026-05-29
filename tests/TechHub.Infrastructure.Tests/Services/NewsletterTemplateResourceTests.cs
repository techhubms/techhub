using FluentAssertions;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Infrastructure.Tests.Services;

public class NewsletterTemplateResourceTests
{
    [Fact]
    public void NewsletterRoundupTemplate_ShouldContainResponsiveLayoutMarkers()
    {
        const string ResourceFileName = "newsletter-roundup-template.html";
        var resourceName = typeof(NewsletterService)
            .Assembly
            .GetManifestResourceNames()
            .Single(name => name.EndsWith(ResourceFileName, StringComparison.Ordinal));

        using var stream = typeof(NewsletterService).Assembly.GetManifestResourceStream(resourceName);
        stream.Should().NotBeNull();

        using var reader = new StreamReader(stream!);
        var template = reader.ReadToEnd();

        template.Should().Contain("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
        template.Should().Contain("<table role=\"presentation\" width=\"100%\"");
        template.Should().Contain("style=\"max-width:900px");
    }
}
