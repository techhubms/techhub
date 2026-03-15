function Set-FrontMatterValue {
    param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$Content,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$Key,
        
        [Parameter(Mandatory = $true)]
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
    $frontmatterSkipKeys = @('date')
    # Keys that should always be formatted as arrays, even with single values
    # All other keys with single values will be formatted inline
    $frontmatterArrayKeys = @('tags', 'section_names', 'categories', 'category')
    
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
                    $new_lines += "$Key`: $formattedValue"
                }
                elseif ($Value.Count -eq 0) {
                    # Empty array
                    $new_lines += "$Key`: []"
                }
                elseif (($Key -in $frontmatterArrayKeys) -or ($Value.Count -gt 1)) {
                    # Format as block sequence array
                    $new_lines += "$Key`:"
                    foreach ($item in $Value) {
                        $new_lines += "  - `"$item`""
                    }
                }
                else {
                    # Single scalar value - inline format
                    $formattedValue = "`"$($Value[0])`""
                    $new_lines += "$Key`: $formattedValue"
                }
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
                    $new_lines += "$Key`: $formattedValue"
                }
                elseif ($Value.Count -eq 0) {
                    # Empty array
                    $new_lines += "$Key`: []"
                }
                elseif (($Key -in $frontmatterArrayKeys) -or ($Value.Count -gt 1)) {
                    # Format as block sequence array
                    $new_lines += "$Key`:"
                    foreach ($item in $Value) {
                        $new_lines += "  - `"$item`""
                    }
                    # Skip any subsequent lines that are part of the old array
                    $i++
                    while ($i -lt $linesArr.Count -and $linesArr[$i] -match '^\s+-\s') {
                        $i++
                    }
                    continue
                }
                else {
                    # Single scalar value - inline format
                    $formattedValue = "`"$($Value[0])`""
                    $new_lines += "$Key`: $formattedValue"
                }
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
            $front_matter_lines += "$Key`: $formattedValue"
        }
        elseif ($Value.Count -eq 0) {
            # Empty array
            $front_matter_lines += "$Key`: []"
        }
        elseif (($Key -in $frontmatterArrayKeys) -or ($Value.Count -gt 1)) {
            # Format as block sequence array
            $front_matter_lines += "$Key`:"
            foreach ($item in $Value) {
                $front_matter_lines += "  - `"$item`""
            }
        }
        else {
            # Single scalar value - inline format
            $formattedValue = "`"$($Value[0])`""
            $front_matter_lines += "$Key`: $formattedValue"
        }
        $front_matter_lines += "---"
        $front_matter_lines += ""
        
        $new_lines = $front_matter_lines + $new_lines
    }
    
    return ($new_lines -join "`n")
}
