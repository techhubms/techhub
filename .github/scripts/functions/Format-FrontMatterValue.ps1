function Format-FrontMatterValue {
    <#
    .SYNOPSIS
        Formats values for YAML frontmatter usage, handling both single values and arrays
    .DESCRIPTION
        This function processes values by:
        1. Detecting if input is an array or single value
        2. Cleaning up content while preserving as much original formatting as possible
        3. Properly escaping quotes for YAML compatibility
        4. Returning properly formatted YAML array syntax or quoted string
    .PARAMETER Value
        String or array of values to format for YAML frontmatter
    #>
    param([object]$Value)
    
    if ($null -eq $Value) {
        return $Value
    }
    
    # Helper function to clean and escape a single value for YAML
    function Format-SingleValue {
        param([string]$SingleValue)
        
        if (-not $SingleValue -or $SingleValue.Trim() -eq '') {
            return '""'
        }
        
        # Clean up the value
        $cleanValue = $SingleValue -replace '<[^>]+>', ''           # Remove HTML tags
        $cleanValue = $cleanValue -replace '[\r\n]+', ' '           # Replace line breaks with spaces
        $cleanValue = $cleanValue -replace '\s+', ' '               # Replace multiple spaces with single space
        $cleanValue = $cleanValue.Trim()                            # Remove leading/trailing whitespace
        
        # Remove surrounding quotes (any type) to work with clean content
        if ($cleanValue.Length -ge 2) {
            $firstChar = $cleanValue[0]
            $lastChar = $cleanValue[$cleanValue.Length - 1]
            
            if (($firstChar -eq '"' -and $lastChar -eq '"') -or 
                ($firstChar -eq "'" -and $lastChar -eq "'") -or
                ($firstChar -eq '`' -and $lastChar -eq '`')) {
                $cleanValue = $cleanValue.Substring(1, $cleanValue.Length - 2)
            }
        }
        
        # Special case: if the content is only quote characters or empty, treat as empty
        if ([string]::IsNullOrWhiteSpace($cleanValue) -or $cleanValue -match '^["''`]+$') {
            return '""'
        }
        
        # Minimal YAML escaping: add one backslash where needed for validity
        $result = $cleanValue
        
        # Escape single backslashes (but not those already part of escape sequences like \" or \\)
        $result = $result -replace '(?<!\\)\\(?![\\"])', '\\'
        
        # Escape unescaped double quotes (quotes that don't have a backslash immediately before them)
        # NOTE: This preserves already escaped quotes \" as-is, not double-escaping them
        $result = $result -replace '(?<!\\)"', '\"'
        
        # Wrap in double quotes for YAML
        return '"' + $result + '"'
    }
    
    # Check if Value is an array or can be treated as one
    if ($Value -is [Array] -or $Value -is [System.Collections.IEnumerable] -and $Value -isnot [string]) {
        # Handle as array
        $formattedItems = @()
        foreach ($item in $Value) {
            if ($null -ne $item) {
                $formattedItems += Format-SingleValue -SingleValue $item.ToString()
            }
        }
        
        if ($formattedItems.Count -eq 0) {
            return '[]'
        }
        
        # Return as YAML array syntax
        return '[' + ($formattedItems -join ', ') + ']'
    }
    else {
        # Handle as single value
        return Format-SingleValue -SingleValue $Value.ToString()
    }
}
