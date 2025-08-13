function Get-YouTubeVideoId {
    <#
    .SYNOPSIS
    Extracts YouTube video ID from various YouTube URL formats.

    .DESCRIPTION
    Parses different YouTube URL formats and extracts the video ID.
    Supports standard watch URLs, shortened URLs, embed URLs, older embed URLs, and Shorts URLs.

    .PARAMETER Url
    The YouTube URL to parse for video ID.

    .EXAMPLE
    Get-YouTubeVideoId -Url "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
    Returns: "dQw4w9WgXcQ"

    .EXAMPLE
    Get-YouTubeVideoId -Url "https://youtu.be/dQw4w9WgXcQ"
    Returns: "dQw4w9WgXcQ"

    .EXAMPLE
    Get-YouTubeVideoId -Url "https://www.youtube.com/embed/dQw4w9WgXcQ"
    Returns: "dQw4w9WgXcQ"

    .EXAMPLE
    Get-YouTubeVideoId -Url "https://www.youtube.com/shorts/dQw4w9WgXcQ"
    Returns: "dQw4w9WgXcQ"

    .OUTPUTS
    String. The YouTube video ID, or empty string if no valid ID is found.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$Url
    )

    if ([string]::IsNullOrWhiteSpace($Url)) {
        throw "URL parameter cannot be null or empty"
    }

    try {
        $uri = [System.Uri]$Url
    }
    catch {
        throw "Invalid URL format: $Url"
    }

    # Validate that we have a valid URI object
    if (-not $uri -or -not $uri.Host) {
        throw "Invalid URL format: $Url"
    }

    # YouTube video ID pattern (11 characters: letters, numbers, hyphens, underscores)
    $videoIdPattern = '^[a-zA-Z0-9_-]{11}$'

    switch -Regex ($uri.Host.ToLower()) {
        '^(www\.)?youtube\.com$' {
            # Handle different YouTube.com paths
            $lowerPath = $uri.AbsolutePath.ToLower()
            switch -Regex ($lowerPath) {
                '^/watch$' {
                    # Standard URL: https://www.youtube.com/watch?v=VIDEO_ID
                    $queryParams = [System.Web.HttpUtility]::ParseQueryString($uri.Query)
                    $videoId = $queryParams["v"]
                    if ($videoId -and $videoId -match $videoIdPattern) {
                        return $videoId
                    }
                }
                '^/embed/(.+)$' {
                    # Embed URL: https://www.youtube.com/embed/VIDEO_ID
                    # Extract from original path to preserve case
                    if ($uri.AbsolutePath -match '^/embed/(.+)$') {
                        $videoId = $matches[1]
                        # Remove any additional parameters (e.g., ?start=30)
                        if ($videoId.Contains('?')) {
                            $videoId = $videoId.Split('?')[0]
                        }
                        if ($videoId -match $videoIdPattern) {
                            return $videoId
                        }
                    }
                }
                '^/v/(.+)$' {
                    # Older embed URL: https://www.youtube.com/v/VIDEO_ID
                    # Extract from original path to preserve case
                    if ($uri.AbsolutePath -match '^/v/(.+)$') {
                        $videoId = $matches[1]
                        # Remove any additional parameters
                        if ($videoId.Contains('?')) {
                            $videoId = $videoId.Split('?')[0]
                        }
                        if ($videoId -match $videoIdPattern) {
                            return $videoId
                        }
                    }
                }
                '^/shorts/(.+)$' {
                    # Shorts URL: https://www.youtube.com/shorts/VIDEO_ID
                    # Extract from original path to preserve case
                    if ($uri.AbsolutePath -match '^/shorts/(.+)$') {
                        $videoId = $matches[1]
                        # Remove any additional parameters
                        if ($videoId.Contains('?')) {
                            $videoId = $videoId.Split('?')[0]
                        }
                        if ($videoId -match $videoIdPattern) {
                            return $videoId
                        }
                    }
                }
            }
        }
        '^youtu\.be$' {
            # Shortened URL: https://youtu.be/VIDEO_ID
            $videoId = $uri.AbsolutePath.TrimStart('/')
            # Remove any additional parameters
            if ($videoId.Contains('?')) {
                $videoId = $videoId.Split('?')[0]
            }
            if ($videoId -match $videoIdPattern) {
                return $videoId
            }
        }
    }

    # If no valid video ID found, return empty string
    return ''
}
