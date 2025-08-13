function Get-FrontmatterDate {
    <#
    .SYNOPSIS
    Extracts and parses the date from markdown frontmatter.
    
    .DESCRIPTION
    Parses the 'date' field from markdown frontmatter and returns a DateTime object.
    Returns DateTime.MinValue if the date cannot be parsed.
    
    .PARAMETER Content
    The content of the markdown file including frontmatter.
    
    .OUTPUTS
    System.DateTime
    
    .EXAMPLE
    $content = Get-Content "post.md" -Raw
    $date = Get-FrontmatterDate -Content $content
    #>
    
    param(
        [Parameter(Mandatory=$true)]
        [string]$Content
    )
    
    if ($Content -match 'date:\s*([^\r\n]+)') {
        $dateString = $matches[1].Trim()
        try {
            return [DateTime]::Parse($dateString)
        }
        catch {
            return [DateTime]::MinValue
        }
    }
    return [DateTime]::MinValue
}
