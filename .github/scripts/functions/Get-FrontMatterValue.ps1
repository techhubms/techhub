function Get-FrontMatterValue {
    param(
        [Parameter(Mandatory=$true)]
        [AllowEmptyString()]
        [string]$Content,
        
        [Parameter(Mandatory=$true)]
        [AllowEmptyString()]
        [string]$Key
    )
    
    # Normalize all line endings to `n (PowerShell's newline)
    $Content = $Content -replace "\r\n", "`n" -replace "\r", "`n"
    $linesArr = $Content -split "`n", 0
    $found_front_matter_start = $false
    $found_front_matter_end = $false
    
    foreach ($line in $linesArr) {
        # Check for front matter start
        if (-not $found_front_matter_start -and $line -match '^---\s*$') {
            $found_front_matter_start = $true
            continue
        }
        
        # Check for front matter end
        if ($found_front_matter_start -and $line -match '^---\s*$') {
            $found_front_matter_end = $true
            break
        }
        
        # If we're in front matter, check if this line contains our key
        if ($found_front_matter_start -and -not $found_front_matter_end) {
            if ($line -match "^$Key\s*:\s*(.*)$") {
                $value = $matches[1].Trim()
                
                # Handle array values (enclosed in brackets)
                if ($value -match '^\[(.*)\]$') {
                    $arrayContent = $matches[1].Trim()
                    if ($arrayContent) {
                        # Split by comma and clean up each item
                        $items = $arrayContent -split ',' | ForEach-Object {
                            $_.Trim() -replace '^"(.*)"$', '$1'
                        } | Where-Object { $_ -ne '' }
                        return @($items)
                    } else {
                        return @()
                    }
                } else {
                    # Handle single values
                    $cleanValue = $value -replace '^["\s]*|["\s]*$', ''
                    
                    # For array-type keys, always return as array even for single values
                    if ($Key -in @('tags', 'tags_normalized', 'categories')) {
                        return @($cleanValue)
                    } else {
                        return $cleanValue
                    }
                }
            }
        }
    }
    
    # Key not found
    return $null
}