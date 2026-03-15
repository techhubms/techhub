function Get-ExternalUrl {
    <#
    .SYNOPSIS
    Extracts the external URL from markdown frontmatter.
    
    .DESCRIPTION
    Parses the 'external_url' field from markdown frontmatter and returns the URL string.
    Returns null if no external URL is found.
    
    .PARAMETER Content
    The content of the markdown file including frontmatter.
    
    .OUTPUTS
    System.String or $null
    
    .EXAMPLE
    $content = Get-Content "item.md" -Raw
    $url = Get-ExternalUrl -Content $content
    #>
    
    param(
        [Parameter(Mandatory = $true)]
        [string]$Content
    )
    
    if ($Content -match 'external_url:\s*"([^"]+)"') {
        return $matches[1]
    }
    return $null
}
