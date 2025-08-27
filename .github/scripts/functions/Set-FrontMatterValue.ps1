function Set-FrontMatterValue {
    param(
        [Parameter(Mandatory=$true)]
        [AllowEmptyString()]
        [string]$Content,
        
        [Parameter(Mandatory=$true)]
        [AllowEmptyString()]
        [string]$Key,
        
        [Parameter(Mandatory=$true)]
        [array]$Value
    )
    
    # Normalize all line endings to `n (PowerShell's newline)
    $Content = $Content -replace "\r\n", "`n" -replace "\r", "`n"
    $linesArr = $Content -split "`n", 0
    $new_lines = @()
    $i = 0
    $found_front_matter_start = $false
    $found_front_matter_end = $false
    $key_updated = $false
    
    # Keys that should not be quoted in frontmatter YAML (but their array values should still be quoted)
    $frontmatterSkipKeys = @('excerpt_separator', 'date')
    # Keys that should always be formatted as arrays, even with single values
    $frontmatterArrayKeys = @('tags', 'categories', 'tags_normalized', 'category')
    
    while ($i -lt $linesArr.Count) {
        $line = $linesArr[$i]
        
        # Check for front matter start
        if (-not $found_front_matter_start -and $line -match '^---\s*$') {
            $found_front_matter_start = $true
            $new_lines += $line
            $i++
            continue
        }
        
        # Check for front matter end
        if ($found_front_matter_start -and -not $found_front_matter_end -and $line -match '^---\s*$') {
            # If we haven't updated the key yet, add it before the closing ---
            if (-not $key_updated) {
                if ($Key -in $frontmatterSkipKeys) {
                    $formattedValue = "[$($Value -join ', ')]"
                } elseif ($Key -in $frontmatterArrayKeys -or $Value.Count -gt 1) {
                    # Always format as array for array keys or multiple values
                    $quotedValues = $Value | ForEach-Object { "`"$_`"" }
                    $formattedValue = "[$($quotedValues -join ', ')]"
                } else {
                    # Single value for non-array key
                    $formattedValue = "`"$($Value[0])`""
                }
                $new_lines += "$Key`: $formattedValue"
                $key_updated = $true
            }
            $found_front_matter_end = $true
            $new_lines += $line
            $i++
            continue
        }
        
        # If we're in front matter, check if this line contains our key
        if ($found_front_matter_start -and -not $found_front_matter_end) {
            if ($line -match "^$Key\s*:") {
                # Replace the existing key with new value
                if ($Key -in $frontmatterSkipKeys) {
                    $formattedValue = "[$($Value -join ', ')]"
                } elseif ($Key -in $frontmatterArrayKeys -or $Value.Count -gt 1) {
                    # Always format as array for array keys or multiple values
                    $quotedValues = $Value | ForEach-Object { "`"$_`"" }
                    $formattedValue = "[$($quotedValues -join ', ')]"
                } else {
                    # Single value for non-array key
                    $formattedValue = "`"$($Value[0])`""
                }
                $new_lines += "$Key`: $formattedValue"
                $key_updated = $true
                $i++
                continue
            }
        }
        
        # Add the line as-is
        $new_lines += $line
        $i++
    }
    
    # If no front matter was found, create it
    if (-not $found_front_matter_start) {
        $front_matter_lines = @()
        $front_matter_lines += "---"
        if ($Key -in $frontmatterSkipKeys) {
            $formattedValue = "[$($Value -join ', ')]"
        } elseif ($Key -in $frontmatterArrayKeys -or $Value.Count -gt 1) {
            # Always format as array for array keys or multiple values
            $quotedValues = $Value | ForEach-Object { "`"$_`"" }
            $formattedValue = "[$($quotedValues -join ', ')]"
        } else {
            # Single value for non-array key
            $formattedValue = "`"$($Value[0])`""
        }
        $front_matter_lines += "$Key`: $formattedValue"
        $front_matter_lines += "---"
        $front_matter_lines += ""
        
        $new_lines = $front_matter_lines + $new_lines
    }
    
    return ($new_lines -join "`n")
}
