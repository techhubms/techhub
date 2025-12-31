function Get-MainContentFromHtml {
    <#
    .SYNOPSIS
    Processes HTML content based on the source URL domain.
    
    .DESCRIPTION
    This function extracts specific content from HTML based on the source domain.
    If the expected content is not found, it throws an exception.
    
    .PARAMETER InputHtml
    The HTML content to process
    
    .PARAMETER SourceUrl
    The source URL to determine which processing logic to use
    
    .EXAMPLE
    $processedHtml = Get-MainContentFromHtml -InputHtml $htmlContent -SourceUrl "https://github.blog/some-article"
    
    .NOTES
    Throws an exception if the expected HTML content cannot be extracted.
    #>
    
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$InputHtml,
        
        [Parameter(Mandatory = $true)]
        [string]$SourceUrl
    )

    # Load the balanced HTML content extraction function
    . (Join-Path $PSScriptRoot "Get-BalancedHtmlContent.ps1")

    # Parse the source URL
    $sourceUri = [Uri]$SourceUrl
    $hostName = $sourceUri.Host.ToLower()
    
    try {
        # Determine processing logic based on domain patterns
        switch -Regex ($hostName) {
            "^github\.blog$" {
                # GitHub Blog processing
                Write-Verbose "Processing GitHub Blog HTML from: $SourceUrl"
                
                # Try new pattern first (current GitHub Blog structure as of Dec 2025)
                # Pattern: <section class="... post__content ...">
                $postContentPattern = '<section[^>]*\bpost__content\b[^>]*>'
                $balancedContent = Get-BalancedHtmlContent -InputHtml $InputHtml -TagName "section" -TagPattern $postContentPattern
                
                if (-not [string]::IsNullOrWhiteSpace($balancedContent)) {
                    Write-Verbose "Successfully extracted GitHub Blog content using post__content section pattern ($(($balancedContent.Length)) characters)"
                    return $balancedContent
                }
                
                # Fallback to legacy nested HTML structure (pre-Dec 2025)
                # Look for the pattern: <!DOCTYPE html PUBLIC...><html><body>...content...</body></html>
                $nestedHtmlPattern = '<!DOCTYPE html PUBLIC[^>]*>\s*<html>\s*<body>(.*?)</body>\s*</html>'
                $nestedMatch = [regex]::Match($InputHtml, $nestedHtmlPattern, [Text.RegularExpressions.RegexOptions]::Singleline)
                
                if ($nestedMatch.Success) {
                    $extractedContent = $nestedMatch.Groups[1].Value.Trim()
                    if ([string]::IsNullOrWhiteSpace($extractedContent)) {
                        throw "GitHub Blog nested content is empty"
                    }
                    Write-Verbose "Successfully extracted GitHub Blog content using legacy nested HTML pattern ($(($extractedContent.Length)) characters)"
                    return $extractedContent
                }
                
                throw "GitHub Blog content not found - tried post__content section and nested HTML patterns"
            }
            "\.microsoft\.com$" {
                # All Microsoft domains processing (including devblogs.microsoft.com, partner.microsoft.com)
                Write-Verbose "Processing Microsoft site HTML from: $SourceUrl"
                
                # Try different patterns in order of specificity (content-specific patterns beat generic semantic ones)
                $patterns = @(
                    # Highest priority: Semantic HTML elements with specific identifiers
                    @{ Pattern = '<article(?=[^>]*\sitemscope\b)(?=[^>]*\sitemtype\b)[^>]*>'; TagName = "article"; Name = "article with itemscope/itemtype (news.microsoft.com, unlocked.microsoft.com)" },
                    @{ Pattern = '<article[^>]*\s+id="post-[^"]*"[^>]*>'; TagName = "article"; Name = "article with post id" },
                    @{ Pattern = '<section[^>]*\s+class="[^"]*mssrc-block-content-block[^"]*"[^>]*>'; TagName = "section"; Name = "mssrc-block-content-block section" },
                    
                    # High priority: Content-specific classes (editorial intent)
                    @{ Pattern = '<div[^>]*\s+class="[^"]*page-content[^"]*"[^>]*>'; TagName = "div"; Name = "page-content class (unlocked.microsoft.com)" },
                    @{ Pattern = '<div[^>]*\s+class="[^"]*article-main-content[^"]*"[^>]*>'; TagName = "div"; Name = "article-main-content class (partner.microsoft.com)" },
                    @{ Pattern = '<div[^>]*\s+class="[^"]*blog-content[^"]*"[^>]*>'; TagName = "div"; Name = "blog-content class component" },
                    @{ Pattern = '<div[^>]*\s+class="[^"]*entry-content[^"]*"[^>]*>'; TagName = "div"; Name = "entry-content class" },
                    @{ Pattern = '<div[^>]*\s+class="[^"]*blog-post-content[^"]*"[^>]*>'; TagName = "div"; Name = "blog-post-content class" },
                    @{ Pattern = '<div[^>]*\s+class="[^"]*message-body[^"]*"[^>]*>'; TagName = "div"; Name = "message-body class" },
                    
                    # Medium priority: Content-specific IDs (more specific before generic)
                    @{ Pattern = '<div[^>]*\s+id="pressreleasecontent"[^>]*>'; TagName = "div"; Name = "pressreleasecontent id" },
                    @{ Pattern = '<div[^>]*\s+id="blogContent"[^>]*>'; TagName = "div"; Name = "blogContent id" },
                    
                    # Lower priority: Generic semantic markup
                    @{ Pattern = '<div[^>]*\s+itemprop\s*=\s*["'']articleBody["''][^>]*>'; TagName = "div"; Name = "articleBody itemprop" }
                )
                
                foreach ($patternConfig in $patterns) {
                    $balancedContent = Get-BalancedHtmlContent -InputHtml $InputHtml -TagName $patternConfig.TagName -TagPattern $patternConfig.Pattern
                    
                    if (-not [string]::IsNullOrWhiteSpace($balancedContent)) {
                        Write-Verbose "Successfully extracted Microsoft content using balanced $($patternConfig.Name) pattern ($(($balancedContent.Length)) characters)"
                        return $balancedContent
                    }
                }
                
                throw "*.microsoft.com main content extraction from HTML did not result in any content"
            }
            "^www\.youtube\.com$" {
                throw "YouTube HTML extraction not yet implemented"
            }
            default {
                Write-Verbose "No specific processor for domain: $hostName"
                # Return original HTML for unsupported domains
                return $InputHtml
            }
        }
    }
    catch {
        Write-Error "Error processing HTML from $SourceUrl`: $($_.Exception.Message)"
        throw
    }
}


