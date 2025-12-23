function Repair-MarkdownFormatting {
    <#
    .SYNOPSIS
        Fix generic markdown formatting issues (excludes Jekyll-specific and frontmatter processing)
    .DESCRIPTION
        This function fixes common markdown formatting issues including:
        - MD009: Remove trailing spaces
        - MD018: Add space after hash in headings
        - MD022: Add blank lines around headings
        - MD029: Fix ordered list numbering
        - MD031: Add blank lines around fenced code blocks
        - MD032: Add blank lines around lists
        - Remove ending colons from headings
        - Remove blank lines immediately after opening code fences and before closing code fences
        - Collapsing multiple consecutive blank lines
        - Ensuring proper line endings
        - Cleaning up trailing whitespace
        - Ensuring files end with exactly one newline
        
        Excludes: MD013 (line-length) and MD040 (fenced-code-language)
    .PARAMETER Path
        The directory path containing markdown files to process
    .PARAMETER FilePath
        Specific file path to process (for testing purposes)
    .EXAMPLE
        Repair-MarkdownFormatting -Path "docs"
    .EXAMPLE
        Repair-MarkdownFormatting -FilePath "/path/to/specific/file.md"
    #>
    param(
        [Parameter(Mandatory = $true, ParameterSetName = "Directory")]
        [string]$Path,
        
        [Parameter(Mandatory = $true, ParameterSetName = "SingleFile")]
        [string]$FilePath
    )
        
    if ($PSCmdlet.ParameterSetName -eq "SingleFile") {
        if (-not (Test-Path $FilePath)) {
            Write-Host "File not found: $FilePath" -ForegroundColor Red
            return
        }
        $markdownFiles = @(Get-Item $FilePath)
        $Path = Split-Path $FilePath -Parent
    }
    else {
        $markdownFiles = Get-MarkdownFiles -Root $Path
    }

    # Validate that we have files to process
    if (-not $markdownFiles -or $markdownFiles.Count -eq 0) {
        throw "No markdown files found to process. Check the path and file patterns."
    }

    $fixedCount = 0

    foreach ($file in $markdownFiles) {
        #Write-Host "Processing file: $($file.Name)"

        $content = Get-Content $file.FullName -Raw
        if (-not $content) { 
            throw "File is empty or cannot be read: $($file.FullName)"
        }

        # Normalize all line endings to `n (PowerShell's newline)
        $content = $content -replace "\r\n", "`n" -replace "\r", "`n"
        $linesArr = $content -split "`n", 0
        $new_lines = @()

        # Detect frontmatter boundaries
        $frontmatter_start = -1
        $frontmatter_end = -1
        if ($linesArr.Count -gt 0 -and $linesArr[0].Trim() -eq '---') {
            $frontmatter_start = 0
            for ($i = 1; $i -lt $linesArr.Count; $i++) {
                if ($linesArr[$i].Trim() -eq '---') {
                    $frontmatter_end = $i
                    break
                }
            }
        }

        # PHASE 1: Identify list blocks and their boundaries
        $listBlocks = @()
        $inListBlock = $false
        $currentListStart = -1
        $inCodeBlock = $false
        
        for ($i = 0; $i -lt $linesArr.Count; $i++) {
            $line = $linesArr[$i]
            
            # Skip frontmatter
            if ($frontmatter_start -ne -1 -and $frontmatter_end -ne -1 -and $i -ge $frontmatter_start -and $i -le $frontmatter_end) {
                continue
            }
            
            # Track code blocks
            if ($line -match '^\s*```') {
                $inCodeBlock = !$inCodeBlock
                if ($inListBlock) {
                    # End current list block before code block
                    $listBlocks += @{ Start = $currentListStart; End = $i - 1 }
                    $inListBlock = $false
                }
                continue
            }
            
            # Skip processing inside code blocks
            if ($inCodeBlock) {
                continue
            }
            
            # Check if this line is a list item (ordered or unordered)
            $orderedListPattern = '^(\s*)\d+\.\s+'
            $unorderedListPattern = '^(\s*)[-*+]\s+'
            $isOrderedList = $line -match $orderedListPattern
            $isUnorderedList = $line -match $unorderedListPattern
            $isListItem = $isOrderedList -or $isUnorderedList
            
            # Check if this line is indented content (continuation of list items)
            $indentedContentPattern = '^\s+\S'
            
            # Check if this line definitely breaks a list
            # A list is broken by:
            # 1. Headings
            # 2. Horizontal rules  
            # 3. Code blocks
            # 4. Text that starts at column 0 (left margin) that isn't a list item
            $nonWhitespaceAtStart = '^\S'
            $isListBreaker = ($line -match '^#{1,6}\s+') -or ($line -match '^---') -or ($line -match '^\s*```') -or ($line -match $nonWhitespaceAtStart -and -not $isListItem)
            
            # Special handling for blank lines: check if next non-blank line would break the list
            $isBlank = $line -match '^\s*$'
            if ($isBlank -and $inListBlock) {
                # Look ahead to find the next non-blank line
                for ($j = $i + 1; $j -lt $linesArr.Count; $j++) {
                    $nextNonBlankLine = $linesArr[$j]
                    if ($nextNonBlankLine -notmatch '^\s*$') {
                        # Check if this next non-blank line would break the list
                        $nextIsListItem = ($nextNonBlankLine -match $orderedListPattern) -or ($nextNonBlankLine -match $unorderedListPattern)
                        $nextIsIndented = $nextNonBlankLine -match $indentedContentPattern -and -not $nextIsListItem
                        $nextIsListBreaker = ($nextNonBlankLine -match '^#{1,6}\s+') -or ($nextNonBlankLine -match '^---') -or ($nextNonBlankLine -match '^\s*```') -or ($nextNonBlankLine -match $nonWhitespaceAtStart -and -not $nextIsListItem)
                        
                        if ($nextIsListBreaker -and -not $nextIsIndented) {
                            # The next non-blank line breaks the list, so end the current list block before this blank line
                            $listBlocks += @{ Start = $currentListStart; End = $i - 1 }
                            $inListBlock = $false
                        }
                        break
                    }
                }
            }
            
            if ($isListItem) {
                if (-not $inListBlock) {
                    # Start a new list block
                    $inListBlock = $true
                    $currentListStart = $i
                }
            }
            elseif ($isListBreaker) {
                if ($inListBlock) {
                    # End current list block
                    $listBlocks += @{ Start = $currentListStart; End = $i - 1 }
                    $inListBlock = $false
                }
            }
            # Other blank lines and indented content don't break list blocks
        }
        
        # Handle case where file ends with a list
        if ($inListBlock) {
            $listBlocks += @{ Start = $currentListStart; End = $linesArr.Count - 1 }
        }
        
        # Create a lookup table for original line numbers to list block mapping
        $lineToListBlock = @{}
        for ($blockIndex = 0; $blockIndex -lt $listBlocks.Count; $blockIndex++) {
            $block = $listBlocks[$blockIndex]
            for ($lineNum = $block.Start; $lineNum -le $block.End; $lineNum++) {
                $lineToListBlock[$lineNum] = $blockIndex
            }
        }
        
        # PHASE 2: Process each line for markdown fixes with list context
        $inCodeBlock = $false
        $new_lines = @()
        $originalLineMapping = @()  # Track which original line each new line came from
        
        for ($i = 0; $i -lt $linesArr.Count; $i++) {
            $currentLine = $linesArr[$i]
            $prevLine = if ($i -gt 0) { $linesArr[$i - 1] } else { "" }
            $nextLine = if ($i -lt $linesArr.Count - 1) { $linesArr[$i + 1] } else { "" }
            
            # Skip frontmatter processing - it's handled by Jekyll-specific function
            if ($frontmatter_start -ne -1 -and $frontmatter_end -ne -1 -and $i -ge $frontmatter_start -and $i -le $frontmatter_end) {
                $new_lines += $currentLine
                $originalLineMapping += $i
                continue
            }
            
            # Check if we're inside a list block
            $insideListBlock = $lineToListBlock.ContainsKey($i)
            
            # Check for list patterns (any level of indentation)
            $unorderedListPattern = '^(\s*)[-*+]\s+'
            $orderedListPattern = '^(\s*)\d+\.\s+'
            $currentIsList = $currentLine -match $unorderedListPattern -or $currentLine -match $orderedListPattern
            $prevIsList = $prevLine -match $unorderedListPattern -or $prevLine -match $orderedListPattern
            $nextIsList = $nextLine -match $unorderedListPattern -or $nextLine -match $orderedListPattern
            
            # Skip EXCESSIVE blank lines within list blocks, but preserve single blank lines for readability
            # Only remove blank lines if there are multiple consecutive blanks within a list
            if ($currentLine -match "^\s*$" -and $insideListBlock) {
                # Check if the previous line was also blank
                if ($prevLine -match "^\s*$") {
                    continue  # Skip this additional blank line
                }
                # Otherwise, keep the single blank line for readability
            }
            
            # MD009: Remove trailing spaces (except intentional 2-space line breaks)
            $trimmedLine = $currentLine
            if ($currentLine -match "\s+$" -and -not ($currentLine -match "\s{2}$")) {
                $trimmedLine = $currentLine.TrimEnd()
            }
            
            # MD018: Add space after hash in headings
            if ($trimmedLine -match "^(#{1,6})([^#\s].*)") {
                $trimmedLine = $matches[1] + " " + $matches[2]
            }
            
            # Fix numbered lists without space after period (e.g., "1.First" becomes "1. First")
            if ($trimmedLine -match "^(\s*)(\d+)\.([^\s].*)") {
                $trimmedLine = $matches[1] + $matches[2] + ". " + $matches[3]
            }
            
            # Remove ending colon from headings (e.g., "#### Key Points:" becomes "#### Key Points")
            if ($trimmedLine -match '^(#+\s+.*):$') {
                $trimmedLine = $trimmedLine -replace ':$', ''
            }
            
            # MD022: Add blank line before headings (except first line)
            if ($trimmedLine -match "^#{1,6}\s+" -and $i -gt 0 -and $prevLine.Trim() -ne "") {
                $new_lines += ""
            }
            
            # MD031: Add blank line before fenced code blocks (opening ```) - but respect list context
            if ($trimmedLine -match '^\s*```' -and -not $inCodeBlock) {
                # Only add blank line if previous line is not empty AND we're not inside a list block
                # Inside list blocks, preserve the original spacing to maintain list item structure
                if ($prevLine.Trim() -ne "" -and -not $insideListBlock) {
                    $new_lines += ""
                }
            }
            
            # MD032: Add blank line before lists (when previous line is not empty and we're starting a list block)
            if ($currentIsList -and $prevLine.Trim() -ne "" -and -not $prevIsList -and $insideListBlock) {
                # Check if this is the start of a list block
                $isListBlockStart = $false
                foreach ($block in $listBlocks) {
                    if ($i -eq $block.Start) {
                        $isListBlockStart = $true
                        break
                    }
                }
                if ($isListBlockStart) {
                    $new_lines += ""
                }
            }
            
            # Add current line (using trimmed version)
            $new_lines += $trimmedLine
            $originalLineMapping += $i
            
            # Track if we're in a code block (after adding the line)
            if ($trimmedLine -match '^\s*```') {
                $inCodeBlock = !$inCodeBlock
            }
            
            # MD022: Add blank line after headings
            if ($trimmedLine -match "^#{1,6}\s+" -and $nextLine.Trim() -ne "" -and $nextLine -notmatch "^#{1,6}\s+") {
                $new_lines += ""
            }
            
            # MD031: Add blank line after fenced code blocks (closing ```) - but respect list context
            if ($trimmedLine -match '^\s*```' -and -not $inCodeBlock) {
                # Only add blank line if next line is not empty AND we're not inside a list block
                # Inside list blocks, preserve the original spacing to maintain list item structure
                if ($nextLine.Trim() -ne "" -and $i -lt $linesArr.Count - 1 -and -not $insideListBlock) {
                    $new_lines += ""
                }
            }
            
            # MD032: Add blank line after lists (when we're ending a list block)
            if ($currentIsList -and $nextLine.Trim() -ne "" -and -not $nextIsList -and $insideListBlock) {
                # Check if this is the end of a list block
                $isListBlockEnd = $false
                foreach ($block in $listBlocks) {
                    if ($i -eq $block.End) {
                        $isListBlockEnd = $true
                        break
                    }
                }
                if ($isListBlockEnd) {
                    $new_lines += ""
                }
            }
        }

        # MD029: Fix ordered list numbering - CONSERVATIVE approach
        # Only fix lists that are obviously broken (like 1,1,2,3 or 1,3,4,5)
        # Don't try to renumber correctly formatted lists
        $numbered_lines = @()
        $lastOrderedNumber = 0
        $inOrderedList = $false
        $listIndentLevel = ""
        
        for ($i = 0; $i -lt $new_lines.Count; $i++) {
            $line = $new_lines[$i]
            
            # Check if this is an ordered list item
            if ($line -match "^(\s*)(\d+)\.\s+(.*)$") {
                $indent = $matches[1]
                $number = [int]$matches[2]
                $content = $matches[3]
                
                # Check if this is a continuation of the current list or a new list
                if (-not $inOrderedList -or $indent -ne $listIndentLevel) {
                    # Starting a new list or changing indent level
                    $inOrderedList = $true
                    $listIndentLevel = $indent
                    
                    # If the first item is not 1, fix it
                    if ($number -ne 1) {
                        $numbered_lines += "$indent" + "1. $content"
                        $lastOrderedNumber = 1
                    } else {
                        $numbered_lines += $line
                        $lastOrderedNumber = 1
                    }
                } else {
                    # Continuing current list
                    $expectedNumber = $lastOrderedNumber + 1
                    
                    # Only fix if the numbering is wrong
                    if ($number -ne $expectedNumber) {
                        $numbered_lines += "$indent$expectedNumber. $content"
                    } else {
                        $numbered_lines += $line
                    }
                    $lastOrderedNumber = $expectedNumber
                }
            }
            else {
                # Not an ordered list item
                $numbered_lines += $line
                
                # Check if this line breaks the ordered list sequence
                if ($line -notmatch "^\s*$" -and $line -notmatch "^(\s*)[-*+]\s+" -and $line -notmatch "^\s+\S") {
                    # Non-blank, non-list, non-indented content breaks the sequence
                    if ($line -match '^#{1,6}\s+' -or $line -match '^---' -or $line -match '^\s*```' -or $line -match '^[^\s]') {
                        $inOrderedList = $false
                        $lastOrderedNumber = 0
                        $listIndentLevel = ""
                    }
                }
            }
        }
        
        $new_lines = $numbered_lines

        # Remove blank lines immediately after opening code fences and immediately before closing code fences
        $code_fence_lines = @()
        $inCodeFence = $false
        
        for ($i = 0; $i -lt $new_lines.Count; $i++) {
            $line = $new_lines[$i]
            $prevLine = if ($i -gt 0) { $new_lines[$i - 1] } else { "" }
            $nextLine = if ($i -lt $new_lines.Count - 1) { $new_lines[$i + 1] } else { "" }
            
            # Check if this line is a code fence
            if ($line -match '^\s*```') {
                if (-not $inCodeFence) {
                    # Opening code fence
                    $inCodeFence = $true
                    $code_fence_lines += $line
                    
                    # Skip the next line if it's blank
                    if ($i + 1 -lt $new_lines.Count -and $new_lines[$i + 1] -match '^\s*$') {
                        $i++ # Skip the blank line
                    }
                } else {
                    # Closing code fence
                    $inCodeFence = $false
                    
                    # Remove the previous line if it's blank (already added)
                    if ($code_fence_lines.Count -gt 0 -and $code_fence_lines[-1] -match '^\s*$') {
                        $code_fence_lines = $code_fence_lines[0..($code_fence_lines.Count - 2)]
                    }
                    
                    $code_fence_lines += $line
                }
            } else {
                $code_fence_lines += $line
            }
        }
        
        $new_lines = $code_fence_lines

        # Collapse multiple consecutive blank lines into a single blank line
        $collapsed_lines = @()
        $blank_count = 0
        foreach ($l in $new_lines) {
            if ($l -match '^[\s]*$') {
                $blank_count++
            }
            else {
                $blank_count = 0
            }
            if ($blank_count -le 1) {
                $collapsed_lines += $l
            }
        }

        # Final step: ensure file ends with exactly one newline after the last non-whitespace character
        if ($collapsed_lines.Count -gt 0) {
            # Remove all trailing blank lines and whitespace lines
            while ($collapsed_lines.Count -gt 0 -and $collapsed_lines[-1] -match '^[\s]*$') {
                if ($collapsed_lines.Count -eq 1) {
                    $collapsed_lines = @()
                }
                else {
                    $collapsed_lines = $collapsed_lines[0..($collapsed_lines.Count - 2)]
                }
            }
            # Trim trailing whitespace from the last line
            if ($collapsed_lines.Count -gt 0) {
                $collapsed_lines[-1] = $collapsed_lines[-1].TrimEnd()
            }
        }

        # Join lines and ensure exactly one newline at the end
        $final_content = $collapsed_lines -join "`n"
        if ($final_content -ne "" -and -not $final_content.EndsWith("`n")) {
            $final_content += "`n"
        }

        # Compare with original content
        $original_content = Get-Content -Path $file.FullName -Raw
        if ($final_content -cne $original_content) {
            # Write with LF line endings and UTF8 encoding without BOM
            $final_content_bytes = [System.Text.Encoding]::UTF8.GetBytes($final_content)
            [System.IO.File]::WriteAllBytes($file.FullName, $final_content_bytes)

            $fixedCount++
            Write-Host "Fixed markdown formatting in $($file.FullName)"
        }
    }

    Write-Host "Fixed markdown formatting in $fixedCount file(s) out of $($markdownFiles.Count) total files."
}