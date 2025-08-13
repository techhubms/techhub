function ConvertTo-SafeFilename {
    <#
    .SYNOPSIS
    Converts a title string into a filesystem-safe filename slug.

    .DESCRIPTION
    This function takes a title string and converts it into a safe filename by:
    - Converting international characters to ASCII equivalents (é→e, ñ→n, etc.)
    - Removing problematic filesystem characters
    - Converting spaces to hyphens
    - Normalizing multiple hyphens to single hyphens
    - Ensuring reasonable length limits for cross-platform compatibility

    .PARAMETER Title
    The title string to convert to a safe filename.

    .PARAMETER MaxLength
    Maximum length for the resulting filename slug. Default is 200 characters.

    .EXAMPLE
    ConvertTo-SafeFilename -Title "Café & Résumé: A Guide"
    Returns: "Cafe-and-Resume-A-Guide"

    .EXAMPLE
    ConvertTo-SafeFilename -Title "AI/ML: The Future?" -MaxLength 20
    Returns: "AIML-The-Future"

    .NOTES
    This function ensures cross-platform compatibility between Windows and Linux filesystems.
    It maintains readability while removing problematic characters.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$Title,
        
        [Parameter(Mandatory = $false)]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$MaxLength = 200
    )

    process {
        if ([string]::IsNullOrWhiteSpace($Title)) {
            throw "Title parameter cannot be null, empty, or whitespace"
        }

        $titleSlug = $Title

        # Convert international characters to ASCII equivalents
        # Vowels with diacritics
        $titleSlug = $titleSlug -replace '[àáâãäåāăąǎǟǡǻȁȃȧ]', 'a'
        $titleSlug = $titleSlug -replace '[ÀÁÂÃÄÅĀĂĄǍǞǠǺȀȂȦ]', 'A'
        $titleSlug = $titleSlug -replace '[èéêëēĕėęěȅȇȩ]', 'e'
        $titleSlug = $titleSlug -replace '[ÈÉÊËĒĔĖĘĚȄȆȨ]', 'E'
        $titleSlug = $titleSlug -replace '[ìíîïīĭįǐȉȋ]', 'i'
        $titleSlug = $titleSlug -replace '[ÌÍÎÏĪĬĮǏȈȊ]', 'I'
        $titleSlug = $titleSlug -replace '[òóôõöøōŏőœǒǫǭȍȏȫȭȯȱ]', 'o'
        $titleSlug = $titleSlug -replace '[ÒÓÔÕÖØŌŎŐŒǑǪǬȌȎȪȬȮȰ]', 'O'
        $titleSlug = $titleSlug -replace '[ùúûüūŭůűųǔǖǘǚǜȕȗ]', 'u'
        $titleSlug = $titleSlug -replace '[ÙÚÛÜŪŬŮŰŲǓǕǗǙǛȔȖ]', 'U'
        $titleSlug = $titleSlug -replace '[ýÿȳ]', 'y'
        $titleSlug = $titleSlug -replace '[ÝŸȲ]', 'Y'

        # Consonants with diacritics
        $titleSlug = $titleSlug -replace '[ñńņňŉŋ]', 'n'
        $titleSlug = $titleSlug -replace '[ÑŃŅŇŊ]', 'N'
        $titleSlug = $titleSlug -replace '[çćĉċč]', 'c'
        $titleSlug = $titleSlug -replace '[ÇĆĈĊČ]', 'C'
        $titleSlug = $titleSlug -replace '[ďđ]', 'd'
        $titleSlug = $titleSlug -replace '[ĎĐ]', 'D'
        $titleSlug = $titleSlug -replace '[ĝğġģ]', 'g'
        $titleSlug = $titleSlug -replace '[ĜĞĠĢ]', 'G'
        $titleSlug = $titleSlug -replace '[ĥħ]', 'h'
        $titleSlug = $titleSlug -replace '[ĤĦ]', 'H'
        $titleSlug = $titleSlug -replace '[ĵ]', 'j'
        $titleSlug = $titleSlug -replace '[Ĵ]', 'J'
        $titleSlug = $titleSlug -replace '[ķĸ]', 'k'
        $titleSlug = $titleSlug -replace '[Ķ]', 'K'
        $titleSlug = $titleSlug -replace '[ĺļľŀł]', 'l'
        $titleSlug = $titleSlug -replace '[ĹĻĽĿŁ]', 'L'
        $titleSlug = $titleSlug -replace '[ŕŗř]', 'r'
        $titleSlug = $titleSlug -replace '[ŔŖŘ]', 'R'
        $titleSlug = $titleSlug -replace '[śŝşšș]', 's'
        $titleSlug = $titleSlug -replace '[ŚŜŞŠȘ]', 'S'
        $titleSlug = $titleSlug -replace '[ţťŧț]', 't'
        $titleSlug = $titleSlug -replace '[ŢŤŦȚ]', 'T'
        $titleSlug = $titleSlug -replace '[ŵ]', 'w'
        $titleSlug = $titleSlug -replace '[Ŵ]', 'W'
        $titleSlug = $titleSlug -replace '[ŷ]', 'y'
        $titleSlug = $titleSlug -replace '[Ŷ]', 'Y'
        $titleSlug = $titleSlug -replace '[źżž]', 'z'
        $titleSlug = $titleSlug -replace '[ŹŻŽ]', 'Z'

        # Special character replacements
        $titleSlug = $titleSlug -replace '[ß]', 'ss'
        $titleSlug = $titleSlug -replace '[ĳ]', 'ij'
        $titleSlug = $titleSlug -replace '[Ĳ]', 'IJ'
        $titleSlug = $titleSlug -replace '[æ]', 'ae'
        $titleSlug = $titleSlug -replace '[Æ]', 'AE'
        $titleSlug = $titleSlug -replace '[ð]', 'dh'
        $titleSlug = $titleSlug -replace '[Ð]', 'DH'
        $titleSlug = $titleSlug -replace '[þ]', 'th'
        $titleSlug = $titleSlug -replace '[Þ]', 'TH'

        # Convert various quote characters to regular quotes, then remove them
        $titleSlug = $titleSlug -replace '[""]', '"'
        $titleSlug = $titleSlug -replace "['']", "'"

        # Remove problematic filesystem characters first
        # Windows: < > : " / \ | ? *
        # Additional safety: remove other control and special characters
        $titleSlug = $titleSlug -replace '[<>:"/\\|?*\[\]{}();,!@#$%^+=`~''""]', ''
        
        # Convert ampersand to "and" for readability (preserve spacing context)
        # Handle spaced ampersands: " & " -> " and "
        $titleSlug = $titleSlug -replace '\s+&\s+', ' and '
        # Handle non-spaced ampersands: "&" -> "and"
        $titleSlug = $titleSlug -replace '&', 'and'

        # Convert to slug format: normalize whitespace, convert to hyphens
        $titleSlug = $titleSlug -replace '\s+', '-'
        
        # Remove remaining non-word characters except hyphens
        $titleSlug = $titleSlug -replace '[^\w-]', ''
        
        # Normalize multiple hyphens to single hyphens
        $titleSlug = $titleSlug -replace '-+', '-'
        
        # Trim hyphens and whitespace from start and end
        $titleSlug = $titleSlug.Trim('-').Trim()

        # Ensure the result is not empty - throw if no valid content remains
        if ([string]::IsNullOrWhiteSpace($titleSlug)) {
            throw "Title contains no valid filename characters after processing"
        }

        # Ensure reasonable length (leave room for date prefix and extension)
        if ($titleSlug.Length -gt $MaxLength) {
            $titleSlug = $titleSlug.Substring(0, $MaxLength).Trim('-')
        }

        # Final safety check - ensure we don't end with a dot (problematic on Windows)
        $titleSlug = $titleSlug.TrimEnd('.')

        return $titleSlug
    }
}
