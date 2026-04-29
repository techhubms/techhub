using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for YtDlpTranscriptService VTT parsing logic.
/// Tests the internal ParseVttToPlainText method which extracts plain text
/// from WebVTT subtitle files downloaded by yt-dlp.
/// </summary>
public class YtDlpTranscriptServiceTests
{
    #region ParseVttToPlainText Tests

    [Fact]
    public void ParseVttToPlainText_ValidVtt_ExtractsTextWithoutTimecodes()
    {
        // Arrange
        var vtt = """
            WEBVTT
            Kind: captions
            Language: en

            00:00:00.320 --> 00:00:05.790 align:start position:0%
            Hello world

            00:00:05.790 --> 00:00:10.800 align:start position:0%
            This is a test

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().Be("Hello world This is a test");
    }

    [Fact]
    public void ParseVttToPlainText_VttWithInlineTimingTags_StripsHtmlTags()
    {
        // Arrange - yt-dlp auto-generated captions contain inline timing tags like <00:00:19.039><c> no</c>
        var vtt = """
            WEBVTT
            Kind: captions
            Language: en

            00:00:18.800 --> 00:00:21.790 align:start position:0%
            We're<00:00:19.039><c> no</c><00:00:19.359><c> strangers</c><00:00:19.840><c> to</c>

            00:00:21.800 --> 00:00:25.950 align:start position:0%
            love.<00:00:22.800><c> You</c><00:00:23.039><c> know</c>

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().Be("We're no strangers to love. You know");
    }

    [Fact]
    public void ParseVttToPlainText_DuplicateLines_RemovesDuplicates()
    {
        // Arrange - auto-generated VTT often duplicates lines across cue boundaries
        var vtt = """
            WEBVTT
            Kind: captions
            Language: en

            00:00:00.000 --> 00:00:05.000
            Hello world

            00:00:05.000 --> 00:00:05.010
            Hello world

            00:00:05.010 --> 00:00:10.000
            Hello world
            This is new text

            00:00:10.000 --> 00:00:15.000
            This is new text

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().Be("Hello world This is new text");
    }

    [Fact]
    public void ParseVttToPlainText_EmptyVtt_ReturnsEmpty()
    {
        // Arrange
        var vtt = """
            WEBVTT
            Kind: captions
            Language: en

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseVttToPlainText_OnlyWhitespaceLines_ReturnsEmpty()
    {
        // Arrange
        var vtt = """
            WEBVTT

            00:00:00.000 --> 00:00:05.000
             

            00:00:05.000 --> 00:00:10.000
             

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseVttToPlainText_MusicAndSoundEffectMarkers_StripsMarkers()
    {
        // Arrange - [Music] and similar markers in brackets should be stripped
        var vtt = """
            WEBVTT

            00:00:00.000 --> 00:00:05.000
            [Music]

            00:00:05.000 --> 00:00:10.000
            Hello world

            00:00:10.000 --> 00:00:15.000
            [Applause]

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().Be("Hello world");
    }

    [Fact]
    public void ParseVttToPlainText_MultilineCaption_JoinsWithSpace()
    {
        // Arrange
        var vtt = """
            WEBVTT

            00:00:00.000 --> 00:00:05.000
            First line
            Second line

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt);

        // Assert
        result.Should().Be("First line Second line");
    }

    [Fact]
    public void ParseVttToPlainText_TruncatesAtMaxLength()
    {
        // Arrange - create VTT with text exceeding max length
        var longText = new string('a', 100);
        var vtt = $"""
            WEBVTT

            00:00:00.000 --> 00:00:05.000
            {longText}

            00:00:05.000 --> 00:00:10.000
            {longText}

            """;

        // Act
        var result = YtDlpTranscriptService.ParseVttToPlainText(vtt, maxLength: 150);

        // Assert
        result.Length.Should().BeLessThanOrEqualTo(150);
    }

    [Fact]
    public void ParseVttToPlainText_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = () => YtDlpTranscriptService.ParseVttToPlainText(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region BuildYtDlpArguments Tests

    [Fact]
    public void BuildYtDlpArguments_StandardVideoId_ProducesCorrectArguments()
    {
        // Arrange
        var videoUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        var outputDir = "/tmp/test";

        // Act
        var args = YtDlpTranscriptService.BuildYtDlpArguments(videoUrl, outputDir, timeoutSeconds: 30);

        // Assert
        args.Should().Contain("--write-sub");
        args.Should().Contain("--write-auto-sub");
        args.Should().Contain("--skip-download");
        args.Should().Contain("--sub-lang");
        args.Should().Contain("en");
        args.Should().Contain("--sub-format");
        args.Should().Contain("vtt");
        args.Should().Contain(videoUrl);
        args.Should().Contain("-o");
    }

    [Fact]
    public void BuildYtDlpArguments_NullVideoUrl_ThrowsArgumentNullException()
    {
        // Act
        var act = () => YtDlpTranscriptService.BuildYtDlpArguments(null!, "/tmp", 30);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void BuildYtDlpArguments_NullOutputDir_ThrowsArgumentNullException()
    {
        // Act
        var act = () => YtDlpTranscriptService.BuildYtDlpArguments("https://youtube.com/watch?v=test", null!, 30);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}
