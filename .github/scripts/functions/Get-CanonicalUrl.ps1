function Get-CanonicalUrl {
    <#
    .SYNOPSIS
    Extracts the canonical URL from markdown frontmatter.
    
    .DESCRIPTION
    Parses the 'canonical_url' field from markdown frontmatter and returns the URL string.
    Returns null if no canonical URL is found.
    
    .PARAMETER Content
    The content of the markdown file including frontmatter.
    
    .OUTPUTS
    System.String or $null
    
    .EXAMPLE
    $content = Get-Content "post.md" -Raw
    $url = Get-CanonicalUrl -Content $content
    #>
    
    param(
        [Parameter(Mandatory=$true)]
        [string]$Content
    )
    
    if ($Content -match 'canonical_url:\s*"([^"]+)"') {
        return $matches[1]
    }
    return $null
}
