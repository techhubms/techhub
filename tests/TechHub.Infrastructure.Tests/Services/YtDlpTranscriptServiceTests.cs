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
    public void BuildYtDlpArguments_WithCookieFilePath_IncludesCookiesFlag()
    {
        // Arrange
        var videoUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        var outputDir = "/tmp/test";
        var cookieFilePath = "/tmp/yt-dlp-cookies-abc.txt";

        // Act
        var args = YtDlpTranscriptService.BuildYtDlpArguments(videoUrl, outputDir, timeoutSeconds: 30, cookieFilePath: cookieFilePath);

        // Assert
        args.Should().Contain("--cookies");
        args.Should().Contain(cookieFilePath);
    }

    [Fact]
    public void BuildYtDlpArguments_WithoutCookieFilePath_OmitsCookiesFlag()
    {
        // Arrange
        var videoUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        var outputDir = "/tmp/test";

        // Act
        var args = YtDlpTranscriptService.BuildYtDlpArguments(videoUrl, outputDir, timeoutSeconds: 30);

        // Assert
        args.Should().NotContain("--cookies");
    }

    [Fact]
    public void BuildYtDlpArguments_NullCookieFilePath_OmitsCookiesFlag()
    {
        // Arrange
        var videoUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        var outputDir = "/tmp/test";

        // Act
        var args = YtDlpTranscriptService.BuildYtDlpArguments(videoUrl, outputDir, timeoutSeconds: 30, cookieFilePath: null);

        // Assert
        args.Should().NotContain("--cookies");
    }

    #endregion

    #region WriteCookiesFile Tests

    [Fact]
    public void WriteCookiesFile_ValidCookieString_WritesNetscapeHeader()
    {
        // Arrange
        var cookieString = "PREF=f4=4000000;SOCS=CAITest";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: must start with Netscape HTTP Cookie File header
            content.Should().StartWith("# Netscape HTTP Cookie File");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_RegularCookie_WritesDotYoutubeDomainWithSubdomains()
    {
        // Arrange
        var cookieString = "PREF=f4=4000000";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: regular cookies use .youtube.com domain with subdomain flag TRUE
            content.Should().Contain(".youtube.com\tTRUE\t/\tFALSE\t0\tPREF\tf4=4000000");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_SecurePrefixedCookie_WritesSecureTrueFlag()
    {
        // Arrange
        var cookieString = "__Secure-ROLLOUT_TOKEN=testvalue";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: __Secure- cookies get secure=TRUE
            content.Should().Contain(".youtube.com\tTRUE\t/\tTRUE\t0\t__Secure-ROLLOUT_TOKEN\ttestvalue");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_HostPrefixedGapsCookie_WritesGoogleAccountsDomain()
    {
        // Arrange — __Host-GAPS is a Google Accounts cookie, not a YouTube cookie
        var cookieString = "__Host-GAPS=1:abc:def";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: __Host-GAPS must use accounts.google.com, not youtube.com
            content.Should().Contain("accounts.google.com\tFALSE\t/\tTRUE\t0\t__Host-GAPS\t1:abc:def");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_OtherHostPrefixedCookie_WritesHostOnlyYoutubeDomain()
    {
        // Arrange — other __Host- cookies (not GAPS) belong to youtube.com
        var cookieString = "__Host-SomeOther=value123";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: non-GAPS __Host- cookies use youtube.com (no dot), subdomains=FALSE, secure=TRUE
            content.Should().Contain("youtube.com\tFALSE\t/\tTRUE\t0\t__Host-SomeOther\tvalue123");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_CookieWithValueContainingEquals_PreservesFullValue()
    {
        // Arrange — base64 values contain '='
        var cookieString = "VISITOR_PRIVACY_METADATA=CgJOT%3D%3D";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert: value after the first '=' is preserved verbatim
            content.Should().Contain("VISITOR_PRIVACY_METADATA\tCgJOT%3D%3D");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_MultipleCookies_WritesAllCookies()
    {
        // Arrange
        var cookieString = "PREF=value1;SOCS=value2;YSC=value3";

        // Act
        var path = YtDlpTranscriptService.WriteCookiesFile(cookieString);

        try
        {
            var content = File.ReadAllText(path);

            // Assert
            content.Should().Contain("PREF");
            content.Should().Contain("SOCS");
            content.Should().Contain("YSC");
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WriteCookiesFile_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = () => YtDlpTranscriptService.WriteCookiesFile(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}
