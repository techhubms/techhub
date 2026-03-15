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

        # Build character map using sequential assignment to avoid duplicate key parsing issues
        $charMap = @{}
        # Vowels lowercase
        $charMap['à']='a'; $charMap['á']='a'; $charMap['â']='a'; $charMap['ã']='a'; $charMap['ä']='a'; $charMap['å']='a'; $charMap['ā']='a'; $charMap['ă']='a'; $charMap['ą']='a'; $charMap['ǎ']='a'; $charMap['ǟ']='a'; $charMap['ǡ']='a'; $charMap['ǻ']='a'; $charMap['ȁ']='a'; $charMap['ȃ']='a'; $charMap['ȧ']='a'
        $charMap['è']='e'; $charMap['é']='e'; $charMap['ê']='e'; $charMap['ë']='e'; $charMap['ē']='e'; $charMap['ĕ']='e'; $charMap['ė']='e'; $charMap['ę']='e'; $charMap['ě']='e'; $charMap['ȅ']='e'; $charMap['ȇ']='e'; $charMap['ȩ']='e'
        $charMap['ì']='i'; $charMap['í']='i'; $charMap['î']='i'; $charMap['ï']='i'; $charMap['ī']='i'; $charMap['ĭ']='i'; $charMap['į']='i'; $charMap['ǐ']='i'; $charMap['ȉ']='i'; $charMap['ȋ']='i'
        $charMap['ò']='o'; $charMap['ó']='o'; $charMap['ô']='o'; $charMap['õ']='o'; $charMap['ö']='o'; $charMap['ø']='o'; $charMap['ō']='o'; $charMap['ŏ']='o'; $charMap['ő']='o'; $charMap['œ']='o'; $charMap['ǒ']='o'; $charMap['ǫ']='o'; $charMap['ǭ']='o'; $charMap['ȍ']='o'; $charMap['ȏ']='o'; $charMap['ȫ']='o'; $charMap['ȭ']='o'; $charMap['ȯ']='o'; $charMap['ȱ']='o'
        $charMap['ù']='u'; $charMap['ú']='u'; $charMap['û']='u'; $charMap['ü']='u'; $charMap['ū']='u'; $charMap['ŭ']='u'; $charMap['ů']='u'; $charMap['ű']='u'; $charMap['ų']='u'; $charMap['ǔ']='u'; $charMap['ǖ']='u'; $charMap['ǘ']='u'; $charMap['ǚ']='u'; $charMap['ǜ']='u'; $charMap['ȕ']='u'; $charMap['ȗ']='u'
        $charMap['ý']='y'; $charMap['ÿ']='y'; $charMap['ȳ']='y'
        # Vowels uppercase
        $charMap['À']='A'; $charMap['Á']='A'; $charMap['Â']='A'; $charMap['Ã']='A'; $charMap['Ä']='A'; $charMap['Å']='A'; $charMap['Ā']='A'; $charMap['Ă']='A'; $charMap['Ą']='A'; $charMap['Ǎ']='A'; $charMap['Ǟ']='A'; $charMap['Ǡ']='A'; $charMap['Ǻ']='A'; $charMap['Ȁ']='A'; $charMap['Ȃ']='A'; $charMap['Ȧ']='A'
        $charMap['È']='E'; $charMap['É']='E'; $charMap['Ê']='E'; $charMap['Ë']='E'; $charMap['Ē']='E'; $charMap['Ĕ']='E'; $charMap['Ė']='E'; $charMap['Ę']='E'; $charMap['Ě']='E'; $charMap['Ȅ']='E'; $charMap['Ȇ']='E'; $charMap['Ȩ']='E'
        $charMap['Ì']='I'; $charMap['Í']='I'; $charMap['Î']='I'; $charMap['Ï']='I'; $charMap['Ī']='I'; $charMap['Ĭ']='I'; $charMap['Į']='I'; $charMap['Ǐ']='I'; $charMap['Ȉ']='I'; $charMap['Ȋ']='I'
        $charMap['Ò']='O'; $charMap['Ó']='O'; $charMap['Ô']='O'; $charMap['Õ']='O'; $charMap['Ö']='O'; $charMap['Ø']='O'; $charMap['Ō']='O'; $charMap['Ŏ']='O'; $charMap['Ő']='O'; $charMap['Œ']='O'; $charMap['Ǒ']='O'; $charMap['Ǫ']='O'; $charMap['Ǭ']='O'; $charMap['Ȍ']='O'; $charMap['Ȏ']='O'; $charMap['Ȫ']='O'; $charMap['Ȭ']='O'; $charMap['Ȯ']='O'; $charMap['Ȱ']='O'
        $charMap['Ù']='U'; $charMap['Ú']='U'; $charMap['Û']='U'; $charMap['Ü']='U'; $charMap['Ū']='U'; $charMap['Ŭ']='U'; $charMap['Ů']='U'; $charMap['Ű']='U'; $charMap['Ų']='U'; $charMap['Ǔ']='U'; $charMap['Ǖ']='U'; $charMap['Ǘ']='U'; $charMap['Ǚ']='U'; $charMap['Ǜ']='U'; $charMap['Ȕ']='U'; $charMap['Ȗ']='U'
        $charMap['Ý']='Y'; $charMap['Ÿ']='Y'; $charMap['Ȳ']='Y'
        # Consonants lowercase
        $charMap['ñ']='n'; $charMap['ń']='n'; $charMap['ņ']='n'; $charMap['ň']='n'; $charMap['ŉ']='n'; $charMap['ŋ']='n'
        $charMap['ç']='c'; $charMap['ć']='c'; $charMap['ĉ']='c'; $charMap['ċ']='c'; $charMap['č']='c'
        $charMap['ď']='d'; $charMap['đ']='d'
        $charMap['ĝ']='g'; $charMap['ğ']='g'; $charMap['ġ']='g'; $charMap['ģ']='g'
        $charMap['ĥ']='h'; $charMap['ħ']='h'
        $charMap['ĵ']='j'
        $charMap['ķ']='k'; $charMap['ĸ']='k'
        $charMap['ĺ']='l'; $charMap['ļ']='l'; $charMap['ľ']='l'; $charMap['ŀ']='l'; $charMap['ł']='l'
        $charMap['ŕ']='r'; $charMap['ŗ']='r'; $charMap['ř']='r'
        $charMap['ś']='s'; $charMap['ŝ']='s'; $charMap['ş']='s'; $charMap['š']='s'; $charMap['ș']='s'
        $charMap['ţ']='t'; $charMap['ť']='t'; $charMap['ŧ']='t'; $charMap['ț']='t'
        $charMap['ŵ']='w'
        $charMap['ŷ']='y'
        $charMap['ź']='z'; $charMap['ż']='z'; $charMap['ž']='z'
        # Consonants uppercase
        $charMap['Ñ']='N'; $charMap['Ń']='N'; $charMap['Ņ']='N'; $charMap['Ň']='N'; $charMap['Ŋ']='N'
        $charMap['Ç']='C'; $charMap['Ć']='C'; $charMap['Ĉ']='C'; $charMap['Ċ']='C'; $charMap['Č']='C'
        $charMap['Ď']='D'; $charMap['Đ']='D'
        $charMap['Ĝ']='G'; $charMap['Ğ']='G'; $charMap['Ġ']='G'; $charMap['Ģ']='G'
        $charMap['Ĥ']='H'; $charMap['Ħ']='H'
        $charMap['Ĵ']='J'
        $charMap['Ķ']='K'
        $charMap['Ĺ']='L'; $charMap['Ļ']='L'; $charMap['Ľ']='L'; $charMap['Ŀ']='L'; $charMap['Ł']='L'
        $charMap['Ŕ']='R'; $charMap['Ŗ']='R'; $charMap['Ř']='R'
        $charMap['Ś']='S'; $charMap['Ŝ']='S'; $charMap['Ş']='S'; $charMap['Š']='S'; $charMap['Ș']='S'
        $charMap['Ţ']='T'; $charMap['Ť']='T'; $charMap['Ŧ']='T'; $charMap['Ț']='T'
        $charMap['Ŵ']='W'
        $charMap['Ŷ']='Y'
        $charMap['Ź']='Z'; $charMap['Ż']='Z'; $charMap['Ž']='Z'
        # Multi-character replacements
        $charMap['ß']='ss'; $charMap['ĳ']='ij'; $charMap['Ĳ']='IJ'; $charMap['æ']='ae'; $charMap['Æ']='AE'; $charMap['ð']='dh'; $charMap['Ð']='DH'; $charMap['þ']='th'; $charMap['Þ']='TH'
        # Quote characters (using char codes)
        $charMap[[char]0x201C] = '"'  # Left double quote
        $charMap[[char]0x201D] = '"'  # Right double quote
        $charMap[[char]0x2018] = "'"  # Left single quote
        $charMap[[char]0x2019] = "'"  # Right single quote

        # Build result using StringBuilder for performance
        $sb = [System.Text.StringBuilder]::new($Title.Length)
        foreach ($char in $Title.ToCharArray()) {
            $charStr = [string]$char
            if ($charMap.ContainsKey($charStr)) {
                [void]$sb.Append($charMap[$charStr])
            }
            else {
                [void]$sb.Append($char)
            }
        }
        $titleSlug = $sb.ToString()

        # Remaining processing with fewer regex operations
        $titleSlug = $titleSlug -replace '[<>:"/\\|?*\[\]{}();,!@#$%^+=`~''""]', ''
        $titleSlug = $titleSlug -replace '\s+&\s+', ' and '
        $titleSlug = $titleSlug -replace '&', 'and'
        $titleSlug = $titleSlug -replace '\s+', '-'
        $titleSlug = $titleSlug -replace '[^\w-]', ''
        $titleSlug = $titleSlug -replace '-+', '-'
        $titleSlug = $titleSlug.Trim('-').Trim()

        if ([string]::IsNullOrWhiteSpace($titleSlug)) {
            throw "Title contains no valid filename characters after processing"
        }

        if ($titleSlug.Length -gt $MaxLength) {
            $titleSlug = $titleSlug.Substring(0, $MaxLength).Trim('-')
        }

        $titleSlug = $titleSlug.TrimEnd('.')
        return $titleSlug
    }
}
