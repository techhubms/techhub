function Repair-MarkdownJekyll {
    <#
    SYNOPSIS
        Fix Jekyll-specific markdown issues including frontmatter processing
    .DESCRIPTION
        This function handles Jekyll-specific markdown processing:
        - Frontmatter validation and repair
        - Tag processing and normalization
        - Date format standardization  
        - Viewing mode insertion
        - Canonical URL tracking
        - File renaming based on date/permalink
        - YAML list conversion
        
        Note: Generic markdown formatting (headings, lists, code blocks, etc.) is handled by Repair-MarkdownFormatting
    .PARAMETER Path
        The root directory containing Jekyll markdown files
    .PARAMETER FilePath
        Specific file path to process (for testing purposes)
    .PARAMETER ViewingModes
        Hashtable mapping directory names to viewing modes
    #>
    param(
        [Parameter(Mandatory = $true, ParameterSetName = "Directory")]
        [Parameter(Mandatory = $false, ParameterSetName = "SingleFile")]
        [string]$Path,
        
        [Parameter(Mandatory = $true, ParameterSetName = "SingleFile")]
        [string]$FilePath
    )
    
    if ($PSCmdlet.ParameterSetName -eq "SingleFile") {
        if (-not (Test-Path $FilePath)) {
            throw "File not found: $FilePath"
        }
        $markdownFiles = @(Get-Item $FilePath)
        # Set Path if not provided - derive from FilePath
        if (-not $Path) { 
            $Path = Split-Path (Split-Path $FilePath -Parent) -Parent 
        }
    }
    else {
        $markdownFiles = Get-MarkdownFiles -Root $Path -IncludeDirectoryPatterns @('collections/*') -ExcludeFilePatterns @('*/AGENTS.md', 'AGENTS.md', '*-guidelines.md')
    }

    # Validate that we have files to process
    if (-not $markdownFiles -or $markdownFiles.Count -eq 0) {
        throw "No markdown files found to process. Check the path and file patterns."
    }

    # Keys that should not be quoted in frontmatter YAML
    # These keys have special handling or should remain unquoted for proper Jekyll processing
    $frontmatterSkipKeys = @('excerpt_separator', 'tags', 'categories', 'date')

    $processedFile = Join-Path (Get-SourceRoot) "scripts/data/processed-entries.json"
    
    $processed = @()
    if ($processedFile -and (Test-Path $processedFile)) {
        $processed = @(Get-Content $processedFile | ConvertFrom-Json)
    }

    $newCanonicalEntries = @()
    $fixedFilesCount = 0
    
    foreach ($file in $markdownFiles) {
        #Write-Host "Processing file: $($file.Name)"

        # Check file size first to catch truly empty files
        $fileInfo = Get-Item $file.FullName
        if ($fileInfo.Length -eq 0) {
            throw "File is empty or cannot be read: $($file.FullName)"
        }
        
        $content = Get-Content $file.FullName -Raw
        if (-not $content -or $content.Length -eq 0 -or [string]::IsNullOrWhiteSpace($content)) { 
            throw "File is empty or cannot be read: $($file.FullName)"
        }

        # Normalize all line endings to `n (PowerShell's newline)
        $content = $content -replace "\r\n", "`n" -replace "\r", "`n"
        $linesArr = $content -split "`n", 0
        
        # Additional safety check after splitting
        if (-not $linesArr -or $linesArr.Count -eq 0 -or ($linesArr.Count -eq 1 -and [string]::IsNullOrWhiteSpace($linesArr[0]))) {
            throw "File is empty or cannot be read: $($file.FullName)"
        }
        
        $new_lines = @()
        $i = 0

        # Prepare for duplicate front matter key check
        $fm_keys = @{}

        # Remove any blank lines or whitespace at the start of the file
        while ($linesArr.Count -gt 0 -and $linesArr[0] -match '^[\s]*$') {
            $linesArr = $linesArr[1..($linesArr.Count - 1)]
        }

        # Insert or update viewing_mode if this file is in a known directory
        $viewingModes = Get-ViewingModes
        $viewing_mode = $null
        
        # Determine viewing mode from file path - look for _collection pattern in the full path
        $full_path_for_viewing = $file.FullName -replace '\\', '/'
        $path_segments_for_viewing = $full_path_for_viewing.Split('/')
        foreach ($segment in $path_segments_for_viewing) {
            if ($segment -match '^_(.+)$') {
                $collection_dir = "_$($matches[1])"
                if ($viewingModes.ContainsKey($collection_dir)) {
                    $viewing_mode = $viewingModes[$collection_dir]
                    break
                }
            }
        }

        if ($null -eq $viewing_mode) {
            throw "No viewingmode found for '$($file.FullName)'"
        }

        $in_front_matter = $false
        $dash_count = 0
        $found_viewing_mode = $false
        $layout_value = $null
        $date_value = $null
        $current_permalink = $null
        $categories_value = @()
        $collection_value = $null
        $found_tags = $false
        $original_tags = @()
        $tags_normalized_lines = @()
        
        # Determine collection from file path - look for _collection pattern in the full path
        $full_path = $file.FullName -replace '\\', '/'
        $path_segments = $full_path.Split('/')
        foreach ($segment in $path_segments) {
            if ($segment -match '^_(.+)$') {
                $collection_value = $matches[1]
                break
            }
        }
        
        # If not found in full path, try the relative path (for backwards compatibility)
        if (-not $collection_value -and $first_segment -match '^_(.+)$') {
            $collection_value = $matches[1]
        }
        
        # First pass: extract categories from frontmatter
        $categories_value = @(Get-FrontMatterValue -Content $content -Key "categories")
        if (-not $categories_value -or $categories_value.Count -eq 0) {
            throw "No categories found in file $($file.FullName)"
        }

        while ($i -lt $linesArr.Count) {
            $line = $linesArr[$i]

            # Detect start of front-matter (YAML '---' at file start)
            if ($i -eq 0 -and $line.Trim() -eq '---') {
                $in_front_matter = $true
                $dash_count = 1
                $new_lines += $line
                $i++
                continue
            }

            # Detect end of front-matter (second YAML '---')
            if ($in_front_matter -and $line.Trim() -eq '---') {
                $dash_count++
                if ($dash_count -eq 2) {
                    $in_front_matter = $false
                }

                # If viewing_mode not found, insert before closing front-matter
                if ($viewing_mode -and -not $found_viewing_mode) {
                    $formattedViewingMode = Format-FrontMatterValue -Value $viewing_mode
                    $new_lines += "viewing_mode: $formattedViewingMode"
                }
                
                # Process tags at the end of frontmatter
                if ($found_tags -and $original_tags -and @($original_tags).Count -gt 0) {
                    if (-not $collection_value) {
                        throw "Collection has not been determined yet"
                    }

                    $tagResult = (Get-FilteredTags -Tags $original_tags -Categories $categories_value -Collection $collection_value)
                    $frontMatterTags = Format-FrontMatterValue -Value $tagResult.tags
                    $frontMatterNormalizedTags = Format-FrontMatterValue -Value $tagResult.tags_normalized
                    $new_lines += "tags: $frontMatterTags"
                    $new_lines += "tags_normalized: $frontMatterNormalizedTags"
                }
                # Don't add empty tags frontmatter if there are no tags
                
                # Cleanup action: Remove blank lines from frontmatter
                # Process the collected frontmatter lines and remove any blank lines
                $cleaned_frontmatter = @()
                $frontmatter_started = $false
                
                foreach ($fm_line in $new_lines) {
                    if ($fm_line.Trim() -eq '---') {
                        if (-not $frontmatter_started) {
                            $frontmatter_started = $true
                            $cleaned_frontmatter += $fm_line
                        }
                        else {
                            # This is the closing ---
                            $cleaned_frontmatter += $fm_line
                            break
                        }
                    }
                    elseif ($frontmatter_started) {
                        # Only add non-blank lines within frontmatter
                        if ($fm_line.Trim() -ne '') {
                            $cleaned_frontmatter += $fm_line
                        }
                    }
                    else {
                        # Before frontmatter starts
                        $cleaned_frontmatter += $fm_line
                    }
                }
                
                # Replace the collected lines with cleaned frontmatter
                $new_lines = $cleaned_frontmatter
                
                $new_lines += $line
                $i++
                continue
            }

            if ($in_front_matter) {
                # Check for duplicate front matter keys as we process each line
                $trimmed_line = $line.Trim()
                if ($trimmed_line -match '^([a-zA-Z0-9_-]+):') {
                    $key = $matches[1]
                    # Special handling for tags and tags_normalized - we handle them specially
                    if ($key -ne 'tags_normalized' -and $key -ne 'tags') {
                        if ($fm_keys.ContainsKey($key)) {
                            throw "Duplicate front matter key '$key' detected in $($file.FullName)"
                        }
                        else {
                            $fm_keys[$key] = $true
                        }
                    }
                }

                # Update or insert viewing_mode in front matter
                if ($line -match '^viewing_mode:\s*"?([^"\s]+)"?') {
                    $current_vm = $matches[1]
                    if ($viewing_mode -and $current_vm -ne $viewing_mode) {
                        $formattedViewingMode = Format-FrontMatterValue -Value $viewing_mode
                        $new_lines += "viewing_mode: $formattedViewingMode"
                    }
                    else {
                        $formattedCurrentVm = Format-FrontMatterValue -Value $current_vm
                        $new_lines += "viewing_mode: $formattedCurrentVm"
                    }
                    $found_viewing_mode = $true
                    $i++
                    continue
                }

                # Capture layout, date, and permalink values for filename/permalink processing
                if ($line -match '^layout:\s*"?([^"\s]+)"?') {
                    $layout_value = $matches[1]
                }
                if ($line -match '^date:\s*(.+)') {
                    $date_value = $matches[1].Trim() -replace '^["\''`]', '' -replace '["\''`]$', ''
                }
                if ($line -match '^permalink:\s*"?([^"\s]+)"?') {
                    $current_permalink = $matches[1]
                }
                
                # Capture and format categories
                if ($line -match '^categories:\s*\[([^\]]*)\]') {
                    # Format categories using Format-FrontMatterValue
                    $formattedCategories = Format-FrontMatterValue -Value $categories_value
                    $line = "categories: $formattedCategories"
                }

                # Check if tags_normalized already exists
                if ($line -match '^tags_normalized:\s*') {
                    $tags_normalized_lines += $i
                    # Skip this line - we'll add the corrected version later
                    $i++
                    continue
                }

                # Process tags: collect tags but don't add to output yet
                if ($line -match '^[\s]*tags:[\s]*\[([^\]]*)\]') {
                    $found_tags = $true
                    $tagListRaw = $matches[1]
                    $tagArray = $tagListRaw -split ',' | ForEach-Object { ($_.Trim(' "')) }
                    $original_tags = $tagArray | Where-Object { $_ -ne '' }
                    # Skip this line - we'll add the corrected version later
                    $i++
                    continue
                }

                # Multi-line YAML list conversion: convert multi-line lists to array format
                if ($line -match '^([a-zA-Z0-9_-]+):\s*$') {
                    $key = $matches[1]
                    $listItems = @()
                    $nextLineIndex = $i + 1
                    
                    # Check if next non-empty line starts with '- ' (indicating a list)
                    while ($nextLineIndex -lt $linesArr.Count) {
                        $nextLine = $linesArr[$nextLineIndex]
                        if ($nextLine -match '^[\s]*$') {
                            # Skip empty lines
                            $nextLineIndex++
                            continue
                        }
                        if ($nextLine -match '^[\s]*-[\s]+(.+)$') {
                            # Found a list item
                            $listItems += $matches[1].Trim()
                            $nextLineIndex++
                        }
                        else {
                            # Not a list item, stop collecting
                            break
                        }
                    }
                    
                    # If we found list items, convert to array format
                    if ($listItems.Count -gt 0) {
                        if ($key -eq 'tags') {
                            # Collect tags from multi-line format but don't add to output yet
                            $found_tags = $true
                            $original_tags = $listItems
                            # Skip the processed list items
                            $i = $nextLineIndex - 1
                            $i++
                            continue
                        }
                        elseif ($key -eq 'categories') {
                            # For categories, format properly and update categories_value
                            $categories_value = $listItems
                            $formattedItems = Format-FrontMatterValue -Value $listItems
                            $line = "$key`: $formattedItems"
                            # Skip the processed list items
                            $i = $nextLineIndex - 1
                        }
                        else {
                            # For other keys, use Format-FrontMatterValue to format properly
                            $formattedItems = Format-FrontMatterValue -Value $listItems
                            $line = "$key`: $formattedItems"
                            # Skip the processed list items
                            $i = $nextLineIndex - 1
                        }
                    }
                }

                # Canonical URL extraction: collect new canonical_url entries for processed-entries.json
                if ($line -match '^canonical_url:[\s]*"([^"]+)"') {
                    $url = $matches[1]
                    if (-not ($processed | Where-Object { $_.canonical_url -eq $url }) -and -not ($newCanonicalEntries | Where-Object { $_.canonical_url -eq $url })) {
                        $entry = @{ 
                            canonical_url = $url
                            collection = $collection_value
                            timestamp = ((Get-Date).ToString("yyyy-MM-dd HH:mm:ss zzz"))
                        }
                        $newCanonicalEntries += $entry
                    }
                }

                # Remove quotes from skipKeys entries: ensure certain keys are never quoted
                if ($line -match '^([a-zA-Z0-9_-]+):\s*(.+)$') {
                    $key = $matches[1]
                    $value = $matches[2]
                    
                    # Remove quotes if this key is in the skip list
                    if ($key -in $frontmatterSkipKeys) {
                        # Remove existing quotes/backticks if present
                        $cleanValue = $value -replace '^["\''`]', '' -replace '["\''`]$', ''
                        $cleanValue = $cleanValue.Trim()
                        if ($cleanValue -ne $value) {
                            $line = "$key`: $cleanValue"
                        }
                    }
                }

                # Date frontmatter repair: standardize date formats
                if ($line -match '^date:[\s]*(.+)') {
                    $dateValue = $matches[1].Trim()
                    $repairedDate = $null
                    
                    # Case 1: YYYY-MM-DD format - convert to full datetime
                    if ($dateValue -match '^\d{4}-\d{2}-\d{2}$') {
                        try {
                            $parsedDate = [DateTime]::ParseExact($dateValue, 'yyyy-MM-dd', $null)
                            $repairedDate = $parsedDate.ToString('yyyy-MM-dd HH:mm:ss +00:00')
                        }
                        catch {
                            Write-Warning "Failed to parse date '$dateValue' in $($file.FullName): $_"
                        }
                    }
                    # Case 2: YYYY-MM-DD HH:mm:ss +0000 (without colon) - add colon
                    elseif ($dateValue -match '^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \+(\d{4})$') {
                        $datePart = $matches[1]
                        $timezonePart = $matches[2]
                        $repairedDate = "$datePart +$($timezonePart.Substring(0,2)):$($timezonePart.Substring(2,2))"
                    }
                    # Case 3: Already in correct format YYYY-MM-DD HH:mm:ss +00:00 - no change needed
                    elseif ($dateValue -match '^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} \+\d{2}:\d{2}$') {
                        $repairedDate = $dateValue
                    }
                    # Case 4: Try generic parsing for other formats before giving up
                    else {
                        try {
                            $parsedDate = [DateTime]::Parse($dateValue)
                            $repairedDate = $parsedDate.ToString('yyyy-MM-dd HH:mm:ss +00:00')
                        }
                        catch {
                            Write-Warning "Unrecognized date format '$dateValue' in $($file.FullName). Expected formats: YYYY-MM-DD, YYYY-MM-DD HH:mm:ss +00:00, or YYYY-MM-DD HH:mm:ss +0000"
                            $repairedDate = $dateValue
                        }
                    }
                    
                    # Update the line if repair was successful and different
                    if ($repairedDate -and $repairedDate -ne $dateValue) {
                        $line = "date: $repairedDate"
                    }
                }

                # Frontmatter value formatting: ensure all values are properly formatted except skipKeys
                # This handles both single-line and multi-line values
                if ($line -match '^([a-zA-Z0-9_-]+):\s*(.+)$') {
                    $key = $matches[1]
                    $value = $matches[2]
                    
                    # Skip if key is in the skip list
                    if ($key -notin $frontmatterSkipKeys) {
                        # Skip if value is already a list format [...]
                        if ($value -notmatch '^\[.*\]$') {
                            # Check for continuation lines
                            $nextLineIndex = $i + 1
                            $continuationLines = @()
                            
                            # Look ahead to find continuation lines
                            while ($nextLineIndex -lt $linesArr.Count) {
                                $nextLine = $linesArr[$nextLineIndex]
                                # Check if this is a continuation line (starts with spaces but not a new key)
                                if ($nextLine -match '^[\s]+(.+)$' -and $nextLine -notmatch '^[\s]*[a-zA-Z0-9_-]+:') {
                                    $continuationLines += $nextLine.Trim()
                                    $nextLineIndex++
                                }
                                else {
                                    break
                                }
                            }
                            
                            # Process the value (single line or multi-line)
                            if ($continuationLines.Count -gt 0) {
                                # Multi-line value: combine all lines
                                $fullValue = $value + " " + ($continuationLines -join " ")
                                # Use Format-FrontMatterValue for proper escaping
                                $formattedValue = Format-FrontMatterValue -Value $fullValue
                                $line = "$key`: $formattedValue"
                                # Skip the continuation lines we've processed
                                $i = $nextLineIndex - 1
                            }
                            # Single line value
                            else {
                                $formattedValue = Format-FrontMatterValue -Value $value
                                $line = "$key`: $formattedValue"
                            }
                        }
                    }
                }

                $new_lines += $line
                $i++
                continue
            }

            $new_lines += $line
            $i++
        }

        # Filename and permalink processing for layout: "post" files
        $new_filename = $null
        $expected_permalink = $null
        if ($layout_value -eq "post" -and $date_value) {
            try {
                # Parse the date from frontmatter
                $parsed_date = $null
                if ($date_value -match '^\d{4}-\d{2}-\d{2}') {
                    $date_part = $matches[0]
                    $parsed_date = [DateTime]::ParseExact($date_part, 'yyyy-MM-dd', $null)
                }
                
                if ($parsed_date) {
                    $current_filename = [System.IO.Path]::GetFileNameWithoutExtension($file.FullName)
                    $current_extension = [System.IO.Path]::GetExtension($file.FullName)
                    $current_dir = [System.IO.Path]::GetDirectoryName($file.FullName)
                    
                    # Extract title from filename (everything after date)
                    $title_part = ""
                    if ($current_filename -match '^\d{4}-\d{2}-\d{2}-(.+)$') {
                        $title_part = $matches[1]
                    }
                    elseif ($current_filename -notmatch '^\d{4}-\d{2}-\d{2}') {
                        # If filename doesn't start with date, use the whole filename as title
                        $title_part = $current_filename
                    }
                    
                    # Create expected filename based on frontmatter date
                    $expected_filename = $parsed_date.ToString('yyyy-MM-dd') + "-" + $title_part
                    $expected_permalink = "/$expected_filename.html"
                    
                    # Check if filename needs to be updated
                    if ($current_filename -ne $expected_filename) {
                        $new_filename = Join-Path $current_dir ($expected_filename + $current_extension)
                        Write-Host "Filename mismatch detected: $($file.FullName) -> $new_filename"
                    }
                    
                    # Update permalink in frontmatter if it's different from expected
                    if ($current_permalink -ne $expected_permalink) {
                        for ($j = 0; $j -lt $new_lines.Count; $j++) {
                            if ($new_lines[$j] -match '^permalink:\s*') {
                                $formattedPermalink = Format-FrontMatterValue -Value $expected_permalink
                                $new_lines[$j] = "permalink: $formattedPermalink"
                                break
                            }
                        }
                        # If no permalink found, we'll add it before the closing ---
                        if ($null -eq $current_permalink) {
                            for ($j = $new_lines.Count - 1; $j -ge 0; $j--) {
                                if ($new_lines[$j] -eq '---') {
                                    $formattedPermalink = Format-FrontMatterValue -Value $expected_permalink
                                    $new_lines = $new_lines[0..($j - 1)] + @("permalink: $formattedPermalink") + $new_lines[$j..($new_lines.Count - 1)]
                                    break
                                }
                            }
                        }
                    }
                }
            }
            catch {
                Write-Warning "Failed to process filename/permalink for $($file.FullName): $_"
            }
        }

        # Join lines with proper line endings
        $final_content = $new_lines -join "`n"
        if ($final_content -ne "" -and -not $final_content.EndsWith("`n")) {
            $final_content += "`n"
        }
        $original_content = Get-Content -Path $file.FullName -Raw
        if ($final_content -cne $original_content) {
            # Write with LF line endings and UTF8 encoding without BOM
            $final_content_bytes = [System.Text.Encoding]::UTF8.GetBytes($final_content)
            [System.IO.File]::WriteAllBytes($file.FullName, $final_content_bytes)
            Write-Host "Fixed Jekyll frontmatter in $($file.FullName)"
            $fixedFilesCount++
        }
        
        # Rename file if filename needs to be updated
        if ($new_filename -and $new_filename -ne $file.FullName) {
            try {
                Move-Item -Path $file.FullName -Destination $new_filename -Force
                Write-Host "Renamed file $($file.FullName) to $new_filename"
            }
            catch {
                Write-Warning "Failed to rename file $($file.FullName) to $new_filename`: $_"
            }
        }
    }

    $newCanonicalCount = $newCanonicalEntries.Count
    if ($newCanonicalCount -gt 0 -and $processedFile -and $processedFile.Trim() -ne "") {
        $processed += $newCanonicalEntries
        Set-Content -Path $processedFile -Value ($processed | ConvertTo-Json -Depth 5)
        Write-Host "Added $newCanonicalCount new entries to processed-entries."
    }

    Write-Host "Fixed Jekyll frontmatter in $fixedFilesCount file(s) out of $($markdownFiles.Count) total files."    
}