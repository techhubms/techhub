function Get-BalancedHtmlContent {
    <#
    .SYNOPSIS
    Extracts content from HTML using balanced tag matching to handle nested structures.
    
    .DESCRIPTION
    This function finds HTML content within a specific tag using balanced tag counting
    to properly handle nested tags of the same type. It uses IndexOf for performance
    and supports case-insensitive matching.
    
    .PARAMETER InputHtml
    The HTML content to process
    
    .PARAMETER TagName
    The HTML tag name to extract content from (e.g., "div", "article", "section")
    
    .PARAMETER TagPattern
    The regex pattern to match the opening tag (including attributes)
    
    .EXAMPLE
    $content = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+itemprop=["'']articleBody["''][^>]*>'
    
    .EXAMPLE
    $content = Get-BalancedHtmlContent -InputHtml $html -TagName "article" -TagPattern '<article[^>]*\s+id="post-[^"]*"[^>]*>'
    
    .EXAMPLE
    $content = Get-BalancedHtmlContent -InputHtml $html -TagName "section" -TagPattern '<section[^>]*\s+class="[^"]*content[^"]*"[^>]*>'
    
    .NOTES
    Uses balanced tag counting with IndexOf for performance. Handles nested tags correctly.
    Returns empty string if no matching content is found.
    #>
    
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$InputHtml,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$TagName,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$TagPattern
    )

    try {
        # Handle empty or null inputs early
        if ([string]::IsNullOrEmpty($InputHtml) -or [string]::IsNullOrEmpty($TagName) -or [string]::IsNullOrEmpty($TagPattern)) {
            Write-Verbose "Empty input detected - returning empty string"
            return ""
        }
        
        # Find the opening tag using the provided pattern
        $openingMatch = [regex]::Match($InputHtml, $TagPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
        
        if (-not $openingMatch.Success) {
            Write-Verbose "Opening tag pattern not found: $TagPattern"
            return ""
        }
        
        # Start position is after the opening tag
        $startPos = $openingMatch.Index + $openingMatch.Length
        $currentPos = $startPos
        $tagCount = 0
        
        # Prepare case-insensitive search strings
        $openingTagSearch = "<$TagName"
        $closingTagSearch = "</$TagName>"
        
        Write-Verbose "Starting balanced extraction for tag '$TagName' at position $startPos"
        
        while ($currentPos -lt $InputHtml.Length) {
            # Find next opening and closing tag positions (case-insensitive)
            $nextOpenTag = $InputHtml.IndexOf($openingTagSearch, $currentPos, [StringComparison]::OrdinalIgnoreCase)
            $nextCloseTag = $InputHtml.IndexOf($closingTagSearch, $currentPos, [StringComparison]::OrdinalIgnoreCase)
            
            # Handle end-of-string cases
            if ($nextOpenTag -eq -1) { $nextOpenTag = $InputHtml.Length }
            if ($nextCloseTag -eq -1) { $nextCloseTag = $InputHtml.Length }
            
            # Process whichever comes first
            if ($nextOpenTag -lt $nextCloseTag) {
                # Found opening tag first - validate it's a real tag
                if ($nextOpenTag -lt $InputHtml.Length) {
                    $charAfterTag = if ($nextOpenTag + $openingTagSearch.Length -lt $InputHtml.Length) { 
                        $InputHtml[$nextOpenTag + $openingTagSearch.Length] 
                    } else { 
                        ' ' 
                    }
                    
                    # Validate that this is actually a tag (followed by space, >, /, tab, newline, or carriage return)
                    if ($charAfterTag -in @(' ', '>', '/', "`t", "`n", "`r")) {
                        $tagCount++
                        Write-Verbose "Found opening tag at position $nextOpenTag, count: $tagCount"
                        $currentPos = $nextOpenTag + $openingTagSearch.Length
                    } else {
                        # Not a real tag, skip this occurrence
                        $currentPos = $nextOpenTag + $openingTagSearch.Length
                        Write-Verbose "Skipped false positive opening tag at position $nextOpenTag"
                    }
                } else {
                    break
                }
            }
            elseif ($nextCloseTag -lt $nextOpenTag) {
                # Found closing tag first
                $tagCount--
                Write-Verbose "Found closing tag at position $nextCloseTag, count: $tagCount"
                
                if ($tagCount -eq -1) {
                    # Found our matching closing tag!
                    $extractedContent = $InputHtml.Substring($startPos, $nextCloseTag - $startPos).Trim()
                    Write-Verbose "Successfully extracted balanced content ($(($extractedContent.Length)) characters)"
                    return $extractedContent
                }
                
                $currentPos = $nextCloseTag + $closingTagSearch.Length
            }
            else {
                # Both are at end of string or equal positions
                break
            }
        }
        
        # If we reach here, we didn't find a balanced closing tag
        Write-Warning "Could not find balanced closing tag for '$TagName' - HTML may be malformed"
        return ""
        
    }
    catch {
        Write-Error "Error extracting balanced HTML content for tag '$TagName': $($_.Exception.Message)"
        return ""
    }
}
