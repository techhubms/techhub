<#
.SYNOPSIS
    Generates a comprehensive index of all documentation headers.

.DESCRIPTION
    Scans AGENTS.md, README.md, and all markdown files in the docs/ directory.
    Extracts H1 (#), H2 (##), and H3 (###) headers.
    Compiles them into a single 'documentation-index.md' file in the docs/ directory.
    This index serves as a searchable map of all documentation topics.

    Filters out:
    - "Overview" sections (should be directly under H1)
    - "Related Documentation" / "Additional Resources" sections (always last, low value in index)

.EXAMPLE
    ./scripts/Generate-DocumentationIndex.ps1
#>

[CmdletBinding()]
param()

$Root = (Get-Location).Path
$OutputFile = Join-Path $Root "docs/documentation-index.md"

Write-Host "Generating documentation index..." -ForegroundColor Cyan

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

    $OutputContent += "## File: [$FilePath]($LinkPath)"
    $OutputContent += ""

    $Lines = Get-Content -Path $FullPath
    $InCodeBlock = $false

    foreach ($Line in $Lines) {
        # Skip code blocks to avoid capturing comments as headers
        if ($Line.Trim().StartsWith('```')) {
            $InCodeBlock = -not $InCodeBlock
            continue
        }
        if ($InCodeBlock) { continue }

        # Match headers and filter unwanted ones
        if ($Line -match '^#\s+(.+)') {
            $HeaderText = $matches[1].Trim()
            $OutputContent += "- $HeaderText"
        }
        elseif ($Line -match '^##\s+(.+)') {
            $HeaderText = $matches[1].Trim()
            # Filter out unwanted H2 headers
            if ($FilteredHeaders -notcontains $HeaderText) {
                $OutputContent += "  - $HeaderText"
            }
        }
        # Disabled h3 headers for now as they add a lot of noise and often contain low-value content (e.g. "Parameters", "Returns", "Examples" in code docs)
        # elseif ($Line -match '^###\s+(.+)') {
        #     $HeaderText = $matches[1].Trim()
        #     # Filter out unwanted H3 headers
        #     if ($FilteredHeaders -notcontains $HeaderText) {
        #         $OutputContent += "    - $HeaderText"
        #     }
        # }
    }
    
    $OutputContent += ""
}

# Write to file
$OutputContent | Set-Content -Path $OutputFile -Encoding UTF8
Write-Host "Index generated at: $OutputFile" -ForegroundColor Green
