using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;
using YoutubeExplode.Videos.ClosedCaptions;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for YouTubeTranscriptService caption track selection logic.
/// Tests the internal FindBestTrack method which determines which closed caption
/// track to use when multiple tracks are available.
/// </summary>
public class YouTubeTranscriptServiceTests
{
    private static Language Lang(string code, string name) => new(code, name);

    private static ClosedCaptionTrackInfo Track(string langCode, string langName, bool auto) =>
        new("https://example.com/track", Lang(langCode, langName), auto);

    #region FindBestTrack Tests

    [Fact]
    public void FindBestTrack_EmptyTracks_ReturnsNull()
    {
        // Arrange
        var manifest = new ClosedCaptionManifest(Array.Empty<ClosedCaptionTrackInfo>());

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void FindBestTrack_SingleEnglishManualTrack_ReturnsThatTrack()
    {
        // Arrange
        var track = Track("en", "English", auto: false);
        var manifest = new ClosedCaptionManifest([track]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(track);
    }

    [Fact]
    public void FindBestTrack_PrefersEnglishManualOverEnglishAuto()
    {
        // Arrange
        var auto = Track("en", "English", auto: true);
        var manual = Track("en", "English", auto: false);
        var manifest = new ClosedCaptionManifest([auto, manual]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(manual);
    }

    [Fact]
    public void FindBestTrack_PrefersEnglishAutoOverNonEnglish()
    {
        // Arrange
        var frenchManual = Track("fr", "French", auto: false);
        var englishAuto = Track("en-US", "English (auto)", auto: true);
        var manifest = new ClosedCaptionManifest([frenchManual, englishAuto]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(englishAuto);
    }

    [Fact]
    public void FindBestTrack_EnglishVariants_MatchesOnPrefix()
    {
        // Arrange - en-US, en-GB should match as English
        var enUs = Track("en-US", "English (United States)", auto: false);
        var manifest = new ClosedCaptionManifest([enUs]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(enUs);
    }

    [Fact]
    public void FindBestTrack_NoEnglish_PrefersManualOverAuto()
    {
        // Arrange
        var frenchAuto = Track("fr", "French", auto: true);
        var germanManual = Track("de", "German", auto: false);
        var manifest = new ClosedCaptionManifest([frenchAuto, germanManual]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(germanManual);
    }

    [Fact]
    public void FindBestTrack_OnlyAutoTracks_ReturnsFirst()
    {
        // Arrange
        var french = Track("fr", "French", auto: true);
        var german = Track("de", "German", auto: true);
        var manifest = new ClosedCaptionManifest([french, german]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert
        result.Should().BeSameAs(french);
    }

    [Fact]
    public void FindBestTrack_NullManifest_ThrowsArgumentNullException()
    {
        // Act
        var act = () => YouTubeTranscriptService.FindBestTrack(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FindBestTrack_FullPriorityOrder_SelectsCorrectly()
    {
        // Arrange: all four categories present
        var frenchAuto = Track("fr", "French", auto: true);
        var germanManual = Track("de", "German", auto: false);
        var englishAuto = Track("en", "English", auto: true);
        var englishManual = Track("en", "English", auto: false);

        // Scrambled order to ensure priority logic works regardless of order
        var manifest = new ClosedCaptionManifest([frenchAuto, englishAuto, germanManual, englishManual]);

        // Act
        var result = YouTubeTranscriptService.FindBestTrack(manifest);

        // Assert: English manual has highest priority
        result.Should().BeSameAs(englishManual);
    }

    #endregion

    #region ParseCookies Tests

    [Fact]
    public void ParseCookies_ValidEntries_ReturnsCookies()
    {
        // Act
        var result = YouTubeTranscriptService.ParseCookies("CONSENT=YES+42;LOGIN_INFO=abc123");

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("CONSENT");
        result[0].Value.Should().Be("YES+42");
        result[0].Domain.Should().Be(".youtube.com");
        result[1].Name.Should().Be("LOGIN_INFO");
        result[1].Value.Should().Be("abc123");
    }

    [Fact]
    public void ParseCookies_EmptyString_ReturnsEmpty()
    {
        // Act
        var result = YouTubeTranscriptService.ParseCookies("");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseCookies_NullString_ReturnsEmpty()
    {
        // Act
        var result = YouTubeTranscriptService.ParseCookies(null!);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseCookies_SkipsBlankAndMalformedEntries()
    {
        // Act
        var result = YouTubeTranscriptService.ParseCookies(";;no-equals-sign;VALID=value;  ");

        // Assert
        result.Should().ContainSingle();
        result[0].Name.Should().Be("VALID");
    }

    [Fact]
    public void ParseCookies_ValueContainsEqualsSign_PreservesFullValue()
    {
        // Arrange — values like base64 can contain '='
        // Act
        var result = YouTubeTranscriptService.ParseCookies("TOKEN=abc=def==");

        // Assert
        result.Should().ContainSingle();
        result[0].Name.Should().Be("TOKEN");
        result[0].Value.Should().Be("abc=def==");
    }

    [Fact]
    public void ParseCookies_TrimsWhitespace()
    {
        // Act
        var result = YouTubeTranscriptService.ParseCookies("  NAME  =  value  ");

        // Assert
        result.Should().ContainSingle();
        result[0].Name.Should().Be("NAME");
        result[0].Value.Should().Be("value");
    }

    #endregion
}
