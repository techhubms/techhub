function Get-MarkdownFromHtml {
    <#
    .SYNOPSIS
    Extracts main content from HTML and converts it to markdown.
    
    .PARAMETER HtmlContent
    The HTML content to process.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$HtmlContent
    )
    
    try {
        # Handle empty or null input
        if ([string]::IsNullOrWhiteSpace($HtmlContent)) {
            return $null
        }
        
        # Check if body tag is found early in the content (first 200 characters)
        # This helps distinguish between actual document structure and embedded HTML in code blocks
        $firstPart = $HtmlContent.Substring(0, [Math]::Min(200, $HtmlContent.Length))
        $bodyTagEarly = $firstPart -match '(?i)<body[^>]*>'
        
        # Extract body content from HTML (case-insensitive) only if body tag is found early
        if ($bodyTagEarly -and $HtmlContent -match '(?si)<body[^>]*>(.*?)</body>') {
            $bodyContent = $matches[1]
        } else {
            # If no body tag found early or extraction fails, use the entire content
            $bodyContent = $HtmlContent
        }

        # Remove everything that is not content-related
        # Remove script, style, nav, header, footer, aside, and other non-content elements
        # Use (?s) flag to make . match newlines (multiline script/style blocks)
        
        # Remove script tags in multiple passes to catch nested or complex cases
        $bodyContent = $bodyContent -replace '(?s)<script[^>]*>.*?</script>', ''
        $bodyContent = $bodyContent -replace '<script[^>]*/>', ''  # Self-closing script tags
        $bodyContent = $bodyContent -replace '(?s)<noscript[^>]*>.*?</noscript>', ''  # NoScript tags
        
        # Remove style tags
        $bodyContent = $bodyContent -replace '(?s)<style[^>]*>.*?</style>', ''
        $bodyContent = $bodyContent -replace '<style[^>]*/>', ''   # Self-closing style tags
        
        # Remove link tags (CSS, preload, etc.)
        $bodyContent = $bodyContent -replace '<link[^>]*>', ''     # Link tags (usually self-closing)
        $bodyContent = $bodyContent -replace '<link[^>]*/>', ''    # Explicitly self-closing link tags
        
        # Remove meta tags and other head elements that might leak into body
        $bodyContent = $bodyContent -replace '<meta[^>]*>', ''     # Meta tags
        $bodyContent = $bodyContent -replace '<meta[^>]*/>', ''    # Self-closing meta tags
        $bodyContent = $bodyContent -replace '(?s)<title[^>]*>.*?</title>', ''  # Title tags
        
        # Remove navigation and layout elements
        $bodyContent = $bodyContent -replace '(?s)<nav[^>]*>.*?</nav>', ''
        $bodyContent = $bodyContent -replace '(?s)<header[^>]*>.*?</header>', ''
        $bodyContent = $bodyContent -replace '(?s)<footer[^>]*>.*?</footer>', ''
        $bodyContent = $bodyContent -replace '(?s)<aside[^>]*>.*?</aside>', ''
        $bodyContent = $bodyContent -replace '(?s)<iframe[^>]*>.*?</iframe>', ''
        $bodyContent = $bodyContent -replace '<iframe[^>]*/?>', ''
        
        # Remove forms and interactive elements (usually not content) - but keep form content
        $bodyContent = $bodyContent -replace '(?s)<button[^>]*>.*?</button>', ''
        $bodyContent = $bodyContent -replace '<button[^>]*/>', ''
        $bodyContent = $bodyContent -replace '(?s)<select[^>]*>.*?</select>', ''
        
        # Remove comment blocks
        $bodyContent = $bodyContent -replace '(?s)<!--.*?-->', ''
        
        # Remove specific input elements that are not content (hidden inputs, form controls)
        $bodyContent = $bodyContent -replace '<input[^>]*type\s*=\s*["`'']hidden["`''][^>]*>', ''
        $bodyContent = $bodyContent -replace '<input[^>]*type\s*=\s*["`'']submit["`''][^>]*>', ''
        $bodyContent = $bodyContent -replace '<input[^>]*type\s*=\s*["`'']button["`''][^>]*>', ''
        
        # Keep only content-related tags and remove everything else
        # This regex keeps: a, p, h1-h6, ul, ol, li, strong, em, b, i, br, img, blockquote, pre, code, highlight, div, span, table, tr, td, th, form
        $allowedTags = 'a|p|h[1-6]|ul|ol|li|strong|em|b|i|br|img|blockquote|pre|code|highlight|div|span|table|tr|td|th|thead|tbody|tfoot|form'
        $bodyContent = $bodyContent -replace "(?s)<(?!/?($allowedTags)\b)[^>]*>", ''
        
        # Remove all attributes from HTML tags to clean up the markup, but preserve important ones
        # This converts <h1 class="something" id="test"> to <h1> and </h1>
        # But preserves href in links, src in images, and class attributes in code tags for language specification
        $bodyContent = $bodyContent -replace '<(?!a\b|img\b|code\b)([a-zA-Z][a-zA-Z0-9]*)[^>]*>', '<$1>'
        
        # Clean up link tags - keep only href attribute
        $bodyContent = $bodyContent -replace '<a[^>]*href\s*=\s*["`'']([^"`'']*)["`''][^>]*>', '<a href="$1">'
        # Remove anchor tags that don't have href attributes (but not the ones that do)
        $bodyContent = $bodyContent -replace '<a(?![^>]*href=)[^>]*>', '<a>'
        
        # Clean up img tags - preserve src, alt, title, aria-*, width, height and other useful attributes
        # Instead of trying to preserve specific attributes, let's just ensure src is preserved and clean broken img tags
        $bodyContent = $bodyContent -replace '<img(?![^>]*src=)[^>]*>', '<img>'  # Remove img tags without src
        
        # Format HTML before converting to markdown
        # Add line breaks after block-level elements for better readability
        $bodyContent = $bodyContent -replace '</p>', "</p>`n"
        $bodyContent = $bodyContent -replace '</div>', "</div>`n"
        $bodyContent = $bodyContent -replace '</h[1-6]>', "$&`n"
        $bodyContent = $bodyContent -replace '</li>', "</li>`n"
        $bodyContent = $bodyContent -replace '</ul>', "</ul>`n`n"
        $bodyContent = $bodyContent -replace '</ol>', "</ol>`n`n"
        $bodyContent = $bodyContent -replace '</blockquote>', "</blockquote>`n`n"
        $bodyContent = $bodyContent -replace '</pre>', "</pre>`n`n"
        $bodyContent = $bodyContent -replace '</code>', "</code>`n"
        $bodyContent = $bodyContent -replace '</highlight>', "</highlight>`n"
        $bodyContent = $bodyContent -replace '</table>', "</table>`n`n"
        
        # Clean up excessive whitespace but preserve intentional line breaks
        $bodyContent = $bodyContent -replace '[ \t]+', ' '  # Multiple spaces/tabs to single space
        $bodyContent = $bodyContent -replace '\r\n', "`n"   # Normalize line endings
        $bodyContent = $bodyContent -replace '\n{3,}', "`n"    # Multiple newlines to single newline
        $bodyContent = $bodyContent.Trim()

        # Add logging before HTML to markdown conversion
        if ([string]::IsNullOrWhiteSpace($bodyContent)) {
            Write-Warning "Body content is null or empty before HTML to markdown conversion"
            return $null
        }
        
        # Convert HTML to markdown using GitHub-flavored markdown for proper code block formatting
        try {
            $markdownContent = $bodyContent | Convert-HtmlToMarkdown -GithubFlavored -UnknownTags Bypass
        }
        catch {
            Write-Warning "Convert-HtmlToMarkdown failed: $($_.Exception.Message)"
            return $bodyContent
        }
        if($null -eq $markdownContent) {
            Write-Host "Unable to convert HTML to markdown. Returning body content with cleaned up HTML."
            return $bodyContent
        }
        $markdownContent = $markdownContent -replace '[ \t]+', ' '  # Multiple spaces to single space
        $markdownContent = $markdownContent -replace '\r\n', "`n"   # Normalize line endings
        
        # Remove unwanted line breaks within sentences while preserving paragraph breaks
        # Split into lines and process each group
        $lines = $markdownContent -split '\n'
        $processedLines = @()
        $currentParagraph = @()
        
        foreach ($line in $lines) {
            $trimmedLine = $line.Trim()
            
            # If it's an empty line, process current paragraph and add break
            if ([string]::IsNullOrWhiteSpace($trimmedLine)) {
                if ($currentParagraph.Count -gt 0) {
                    # Join current paragraph lines with spaces, removing unwanted breaks
                    $paragraphText = ($currentParagraph -join ' ').Trim()
                    if ($paragraphText) {
                        $processedLines += $paragraphText
                    }
                    $currentParagraph = @()
                }
                $processedLines += ''  # Preserve paragraph break
            }
            # If it starts with markdown formatting (headers, lists, etc.), process current paragraph first
            elseif ($trimmedLine -match '^(#{1,6}\s|[-\*\+]\s|\d+\.\s|>\s)') {
                if ($currentParagraph.Count -gt 0) {
                    $paragraphText = ($currentParagraph -join ' ').Trim()
                    if ($paragraphText) {
                        $processedLines += $paragraphText
                    }
                    $currentParagraph = @()
                }
                $processedLines += $trimmedLine  # Keep formatted line as-is
            }
            # Otherwise, add to current paragraph
            else {
                $currentParagraph += $trimmedLine
            }
        }
        
        # Process any remaining paragraph
        if ($currentParagraph.Count -gt 0) {
            $paragraphText = ($currentParagraph -join ' ').Trim()
            if ($paragraphText) {
                $processedLines += $paragraphText
            }
        }
        
        # Join all processed lines
        $markdownContent = ($processedLines -join "`n")
        
        # Clean up excessive newlines
        $markdownContent = $markdownContent -replace '\n{3,}', "`n`n"  # Excessive newlines to double newline
        $markdownContent = $markdownContent.Trim()
        
        return $markdownContent
    }
    catch {
        Write-Warning "Failed to extract markdown from HTML: $($_.Exception.Message)"
        return $null
    }
}
