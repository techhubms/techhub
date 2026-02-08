<#
.SYNOPSIS
    Generates a comprehensive index of all documentation headers.

.DESCRIPTION
    Scans AGENTS.md, README.md, and all markdown files in the docs/ directory.
    Extracts H1 (#) and H2 (##) headers.
    Compiles them into a single 'documentation-index.md' file in the docs/ directory.
    Each file is listed on a single line with headers separated by semicolons.

    Filters out:
    - "Overview" sections (should be directly under H1)
    - "Related Documentation" / "Additional Resources" sections (always last, low value in index)

    Output format:
    - [filepath](link): "H1": "H2": "H3", "H3"; "H2"; "H2"
    Where:
    - `:` goes a level deeper (H1 -> H2 -> H3)
    - `,` separates items at the same level
    - `;` goes back up a level

.EXAMPLE
    ./scripts/Generate-DocumentationIndex.ps1
#>

[CmdletBinding()]
param()

$Root = (Get-Location).Path
$OutputFile = Join-Path $Root "docs/documentation-index.md"

Write-Host "Generating documentation index..." -ForegroundColor Cyan

# Helper function to format a header section
function Format-HeaderSection {
    param($H1, $H2s, $H3s)
    
    # Attach any remaining H3s to the last H2
    if ($H2s.Count -gt 0 -and $H3s.Count -gt 0) {
        $LastH2Index = $H2s.Count - 1
        $H2s[$LastH2Index] = "$($H2s[$LastH2Index]): $($H3s -join ', ')"
    }
    
    if ($H2s.Count -gt 0) {
        return "$H1`: $($H2s -join '; ')"
    } else {
        return $H1
    }
}

# Headers to filter out from the index (case-insensitive)
$FilteredHeaders = @(
    "Overview",
    "Related Documentation",
    "Additional Resources"
)

# Define the files to scan
$FilesToScan = @(
    "AGENTS.md",
    "README.md"
)

Write-Host "Finding docs files..."
$DocsFiles = Get-ChildItem -Path (Join-Path $Root "docs") -Filter "*.md" | Where-Object { $_.Name -ne "documentation-index.md" }
foreach ($Doc in $DocsFiles) {
    $FilesToScan += "docs/$($Doc.Name)"
}

Write-Host "Finding AGENTS.md files..."
# Targeted search instead of full root recursion
$SearchPaths = @("src", "collections", "scripts", "tests")
foreach ($Path in $SearchPaths) {
    $FullPath = Join-Path $Root $Path
    if (Test-Path $FullPath) {
        Write-Host "  Scanning $Path..."
        try {
            $AgentsFiles = Get-ChildItem -Path $FullPath -Recurse -Filter "AGENTS.md" -ErrorAction SilentlyContinue
            foreach ($Agent in $AgentsFiles) {
                 # Get relative path
                $RelPath = [System.IO.Path]::GetRelativePath($Root, $Agent.FullName)
                # Convert backslashes to forward slashes for consistency
                $RelPath = $RelPath -replace "\\", "/"
                $FilesToScan += $RelPath
            }
        } catch {
            Write-Warning "Failed to scan $Path : $_"
        }
    }
}

Write-Host "Files found: $($FilesToScan.Count). Processing..."

# Initialize output content
$OutputContent = @(
    "# Documentation Index",
    "",
    "> **Auto-generated file**. Do not edit manually. Run ``scripts/Generate-DocumentationIndex.ps1`` to update.",
    "",
    "This index maps the structure of all documentation files in the project.",
    "",
    "## Syntax",
    "",
    "Output format: ``- [filepath](link): `"H1`": `"H2`": `"H3`", `"H3`"; `"H2`"; `"H2`"``",
    "",
    "- ``:`` goes a level deeper (H1 -> H2 -> H3)",
    "- ``,`` separates items at the same level",
    "- ``;`` goes back up a level",
    "",
    "## Index",
    ""
)

foreach ($FilePath in $FilesToScan) {
    $FullPath = Join-Path $Root $FilePath
    
    if (-not (Test-Path $FullPath)) {
        Write-Warning "File not found: $FilePath"
        continue
    }

    # Convert path to be relative from docs/ directory
    if ($FilePath.StartsWith("docs/")) {
        # Files in docs/ - remove the docs/ prefix
        $LinkPath = $FilePath.Substring(5)
    } else {
        # Files outside docs/ - add ../ prefix
        $LinkPath = "../$FilePath"
    }

    # Get filename for display
    $FileName = [System.IO.Path]::GetFileName($FilePath)

    $Lines = Get-Content -Path $FullPath
    $InCodeBlock = $false
    $H1Sections = @()
    $CurrentH1 = $null
    $CurrentH2s = @()
    $CurrentH3s = @()

    foreach ($Line in $Lines) {
        # Skip code blocks to avoid capturing comments as headers
        if ($Line.Trim().StartsWith('```')) {
            $InCodeBlock = -not $InCodeBlock
            continue
        }
        if ($InCodeBlock) { continue }

        # Match H1 headers (single #)
        if ($Line -match '^#\s+(.+)$' -and $Line -notmatch '^##') {
            $HeaderText = $matches[1].Trim()
            # Filter out unwanted headers
            if ($FilteredHeaders -notcontains $HeaderText) {
                # Save previous H2 with its H3s before moving to new H1
                if ($null -ne $CurrentH1) {
                    $H1Sections += (Format-HeaderSection $CurrentH1 $CurrentH2s $CurrentH3s)
                }
                $CurrentH1 = $HeaderText
                $CurrentH2s = @()
                $CurrentH3s = @()
            }
        }
        # Match H2 headers (double ##)
        elseif ($Line -match '^##\s+(.+)$' -and $Line -notmatch '^###') {
            $HeaderText = $matches[1].Trim()
            # Filter out unwanted headers
            if ($FilteredHeaders -notcontains $HeaderText) {
                # Save previous H2 with its H3s
                if ($CurrentH2s.Count -gt 0 -and $CurrentH3s.Count -gt 0) {
                    $LastH2Index = $CurrentH2s.Count - 1
                    $CurrentH2s[$LastH2Index] = "$($CurrentH2s[$LastH2Index]): $($CurrentH3s -join ', ')"
                }
                $CurrentH2s += $HeaderText
                $CurrentH3s = @()
            }
        }
        # Match H3 headers (triple ###)
        elseif ($Line -match '^###\s+(.+)$' -and $Line -notmatch '^####') {
            $HeaderText = $matches[1].Trim()
            # Filter out unwanted headers
            if ($FilteredHeaders -notcontains $HeaderText) {
                $CurrentH3s += $HeaderText
            }
        }
    }
    
    # Don't forget the last H1 section
    if ($null -ne $CurrentH1) {
        # Attach any remaining H3s to the last H2
        if ($CurrentH2s.Count -gt 0 -and $CurrentH3s.Count -gt 0) {
            $LastH2Index = $CurrentH2s.Count - 1
            $CurrentH2s[$LastH2Index] = "$($CurrentH2s[$LastH2Index]): $($CurrentH3s -join ', ')"
        }
        if ($CurrentH2s.Count -gt 0) {
            $H1Sections += "$CurrentH1`: $($CurrentH2s -join '; ')"
        } else {
            $H1Sections += $CurrentH1
        }
    }
    
    # Build single line output: - [filepath](link): "H1": "H2": "H3", "H3"; "H2"; "H2"
    $HeadersString = $H1Sections -join "; "
    $OutputContent += "- [$FilePath]($LinkPath): `"$($HeadersString -replace ': ', '": "' -replace '; ', '"; "' -replace ', ', '", "')`""
}

# Write to file
$OutputContent | Set-Content -Path $OutputFile -Encoding UTF8
Write-Host "Index generated at: $OutputFile" -ForegroundColor Green
