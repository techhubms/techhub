using FluentAssertions;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Infrastructure.Tests.Services;

public class NewsletterTemplateResourceTests
{
    [Fact]
    public void NewsletterRoundupTemplate_ShouldContainResponsiveLayoutMarkers()
    {
        const string resourceFileName = "newsletter-roundup-template.html";
        var resourceName = typeof(NewsletterService)
            .Assembly
            .GetManifestResourceNames()
            .Single(name => name.EndsWith(resourceFileName, StringComparison.Ordinal));

        using var stream = typeof(NewsletterService).Assembly.GetManifestResourceStream(resourceName);
        stream.Should().NotBeNull();

        using var reader = new StreamReader(stream!);
        var template = reader.ReadToEnd();

        template.Should().MatchRegex("<meta(?=[^>]*name=\"viewport\")(?=[^>]*content=\"width=device-width, initial-scale=1.0\")[^>]*>");
        template.Should().MatchRegex("<table(?=[^>]*role=\"presentation\")(?=[^>]*width=\"100%\")[^>]*>");
        template.Should().Contain("max-width:900px");
    }
}
