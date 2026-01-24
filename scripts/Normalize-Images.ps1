<#
.SYNOPSIS
    Normalizes all images in wwwroot with consistent metadata and color profiles.

.DESCRIPTION
    This script processes all images in the wwwroot directory to ensure consistent:
    - sRGB ICC color profile embedded
    - Clean EXIF data (removes all existing, sets only essential fields)
    - Proper color space declaration
    
    Supports: WebP, JPG/JPEG, JXL, PNG

.PARAMETER ImagePath
    Path to image directory. Defaults to src/TechHub.Web/wwwroot/images

.PARAMETER Artist
    Artist name for EXIF data. Defaults to "Reinier van Maanen"

.PARAMETER Copyright
    Copyright notice. Defaults to "© <year> TechHub. All rights reserved."

.PARAMETER DryRun
    Show what would be done without making changes

.EXAMPLE
    ./scripts/Normalize-Images.ps1
    
.EXAMPLE
    ./scripts/Normalize-Images.ps1 -DryRun
    
.EXAMPLE
    ./scripts/Normalize-Images.ps1 -ImagePath "./src/TechHub.Web/wwwroot/images/section-backgrounds"
#>

[CmdletBinding()]
param(
    [string]$ImagePath = "",
    [string]$Artist = "Reinier van Maanen",
    [string]$Copyright = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

# =============================================================================
# Configuration
# =============================================================================

$script:RepoRoot = git rev-parse --show-toplevel 2>$null
if (-not $script:RepoRoot) {
    $script:RepoRoot = "/workspaces/techhub"
}

if (-not $ImagePath) {
    $ImagePath = Join-Path $script:RepoRoot "src/TechHub.Web/wwwroot/images"
}

if (-not $Copyright) {
    $year = (Get-Date).Year
    $Copyright = "© $year TechHub. All rights reserved."
}

$script:IccProfileUrl = "https://www.color.org/sRGB_v4_ICC_preference.icc"
$script:IccProfilePath = "/tmp/sRGB_v4_ICC.icc"
$script:Software = "TechHub Image Pipeline"

# =============================================================================
# Helper Functions
# =============================================================================

function Write-Step {
    param([string]$Message)
    Write-Host "  → $Message" -ForegroundColor Gray
}

function Write-Success {
    param([string]$Message)
    Write-Host "  ✓ $Message" -ForegroundColor Green
}

function Write-Warning {
    param([string]$Message)
    Write-Host "  ⚠ $Message" -ForegroundColor Yellow
}

function Write-Failure {
    param([string]$Message)
    Write-Host "  ✗ $Message" -ForegroundColor Red
}

function Install-RequiredPackages {
    Write-Host "`n📦 Checking required packages..." -ForegroundColor Cyan
    
    $packages = @(
        @{ Name = "exiftool"; Command = "exiftool"; Package = "libimage-exiftool-perl" },
        @{ Name = "webp tools"; Command = "webpmux"; Package = "webp" },
        @{ Name = "file"; Command = "file"; Package = "file" }
    )
    
    $needsInstall = @()
    
    foreach ($pkg in $packages) {
        $exists = Get-Command $pkg.Command -ErrorAction SilentlyContinue
        if ($exists) {
            Write-Step "$($pkg.Name) already installed"
        }
        else {
            $needsInstall += $pkg.Package
            Write-Step "$($pkg.Name) needs installation"
        }
    }
    
    if ($needsInstall.Count -gt 0) {
        Write-Host "`n📥 Installing packages: $($needsInstall -join ', ')" -ForegroundColor Yellow
        
        if ($DryRun) {
            Write-Host "   [DRY RUN] Would run: sudo apt-get install -y $($needsInstall -join ' ')" -ForegroundColor Magenta
        }
        else {
            sudo apt-get update -qq 2>&1 | Out-Null
            sudo apt-get install -y -qq $needsInstall 2>&1 | Out-Null
            Write-Success "Packages installed"
        }
    }
}

function Get-IccProfile {
    Write-Host "`n🎨 Downloading sRGB ICC profile..." -ForegroundColor Cyan
    
    if (Test-Path $script:IccProfilePath) {
        Write-Step "ICC profile already cached"
        return $true
    }
    
    if ($DryRun) {
        Write-Host "   [DRY RUN] Would download ICC profile from $script:IccProfileUrl" -ForegroundColor Magenta
        return $true
    }
    
    try {
        curl -sL -o $script:IccProfilePath $script:IccProfileUrl 2>&1 | Out-Null
        if (Test-Path $script:IccProfilePath) {
            $size = (Get-Item $script:IccProfilePath).Length
            Write-Success "Downloaded sRGB ICC profile ($size bytes)"
            return $true
        }
    }
    catch {
        Write-Failure "Failed to download ICC profile: $_"
        return $false
    }
    
    return $false
}

function Get-ImageDescription {
    param([System.IO.FileInfo]$File)
    
    # Generate description from path relative to images folder
    $relativePath = $File.FullName -replace [regex]::Escape($ImagePath), ""
    $relativePath = $relativePath.TrimStart("/", "\")
    
    # Extract meaningful name from path
    $pathParts = $relativePath -split "[/\\]"
    $folder = if ($pathParts.Count -gt 1) { $pathParts[0] } else { "" }
    $name = $File.BaseName -replace "[-_]", " "
    
    if ($folder) {
        $folder = $folder -replace "[-_]", " "
        return "TechHub $folder`: $name"
    }
    return "TechHub image: $name"
}

function Process-WebPImage {
    param([System.IO.FileInfo]$File)
    
    $tempFile = "/tmp/$($File.Name)"
    
    # Step 1: Add ICC profile using webpmux
    Write-Step "Adding ICC profile..."
    if (-not $DryRun) {
        $result = webpmux -set icc $script:IccProfilePath $File.FullName -o $tempFile 2>&1
        if (Test-Path $tempFile) {
            Move-Item $tempFile $File.FullName -Force
        }
        else {
            Write-Warning "webpmux failed for $($File.Name)"
            return $false
        }
    }
    
    # Step 2: Strip all EXIF and re-add minimal clean data
    Write-Step "Setting clean EXIF metadata..."
    $description = Get-ImageDescription -File $File
    
    if (-not $DryRun) {
        # First strip all existing metadata
        exiftool -overwrite_original -all= $File.FullName 2>&1 | Out-Null
        
        # Re-add ICC profile (exiftool -all= removes it)
        $result = webpmux -set icc $script:IccProfilePath $File.FullName -o $tempFile 2>&1
        if (Test-Path $tempFile) {
            Move-Item $tempFile $File.FullName -Force
        }
        
        # Add clean EXIF data
        exiftool -overwrite_original `
            "-Artist=$Artist" `
            "-Copyright=$Copyright" `
            "-Software=$script:Software" `
            "-ImageDescription=$description" `
            "-ColorSpace=sRGB" `
            $File.FullName 2>&1 | Out-Null
    }
    
    return $true
}

function Process-JpgImage {
    param([System.IO.FileInfo]$File)
    
    # Step 1: Strip all metadata
    Write-Step "Stripping existing metadata..."
    if (-not $DryRun) {
        exiftool -overwrite_original -all= $File.FullName 2>&1 | Out-Null
    }
    
    # Step 2: Add ICC profile
    Write-Step "Adding ICC profile..."
    if (-not $DryRun) {
        exiftool -overwrite_original "-ICC_Profile<=$script:IccProfilePath" $File.FullName 2>&1 | Out-Null
    }
    
    # Step 3: Add clean EXIF data
    Write-Step "Setting clean EXIF metadata..."
    $description = Get-ImageDescription -File $File
    
    if (-not $DryRun) {
        exiftool -overwrite_original `
            "-Artist=$Artist" `
            "-Copyright=$Copyright" `
            "-Software=$script:Software" `
            "-ImageDescription=$description" `
            "-ColorSpace=sRGB" `
            "-XResolution=96" `
            "-YResolution=96" `
            "-ResolutionUnit=inches" `
            $File.FullName 2>&1 | Out-Null
    }
    
    return $true
}

function Process-JxlImage {
    param([System.IO.FileInfo]$File)
    
    # JXL handling via exiftool
    Write-Step "Stripping existing metadata..."
    if (-not $DryRun) {
        exiftool -overwrite_original -all= $File.FullName 2>&1 | Out-Null
    }
    
    Write-Step "Adding ICC profile..."
    if (-not $DryRun) {
        exiftool -overwrite_original "-ICC_Profile<=$script:IccProfilePath" $File.FullName 2>&1 | Out-Null
    }
    
    Write-Step "Setting clean EXIF metadata..."
    $description = Get-ImageDescription -File $File
    
    if (-not $DryRun) {
        exiftool -overwrite_original `
            "-Artist=$Artist" `
            "-Copyright=$Copyright" `
            "-Software=$script:Software" `
            "-ImageDescription=$description" `
            "-ColorSpace=sRGB" `
            $File.FullName 2>&1 | Out-Null
    }
    
    return $true
}

function Process-PngImage {
    param([System.IO.FileInfo]$File)
    
    Write-Step "Stripping existing metadata..."
    if (-not $DryRun) {
        exiftool -overwrite_original -all= $File.FullName 2>&1 | Out-Null
    }
    
    Write-Step "Adding ICC profile..."
    if (-not $DryRun) {
        exiftool -overwrite_original "-ICC_Profile<=$script:IccProfilePath" $File.FullName 2>&1 | Out-Null
    }
    
    Write-Step "Setting clean metadata..."
    $description = Get-ImageDescription -File $File
    
    if (-not $DryRun) {
        exiftool -overwrite_original `
            "-Artist=$Artist" `
            "-Copyright=$Copyright" `
            "-Software=$script:Software" `
            "-Description=$description" `
            $File.FullName 2>&1 | Out-Null
    }
    
    return $true
}

function Process-Image {
    param([System.IO.FileInfo]$File)
    
    $extension = $File.Extension.ToLower()
    
    switch ($extension) {
        ".webp" { return Process-WebPImage -File $File }
        ".jpg" { return Process-JpgImage -File $File }
        ".jpeg" { return Process-JpgImage -File $File }
        ".jxl" { return Process-JxlImage -File $File }
        ".png" { return Process-PngImage -File $File }
        default {
            Write-Warning "Unsupported format: $extension"
            return $false
        }
    }
}

function Show-Summary {
    param(
        [int]$Processed,
        [int]$Succeeded,
        [int]$Failed
    )
    
    Write-Host "`n" + "=" * 60 -ForegroundColor Cyan
    Write-Host "📊 Summary" -ForegroundColor Cyan
    Write-Host "=" * 60 -ForegroundColor Cyan
    Write-Host "  Total processed: $Processed"
    Write-Host "  Succeeded:       $Succeeded" -ForegroundColor Green
    if ($Failed -gt 0) {
        Write-Host "  Failed:          $Failed" -ForegroundColor Red
    }
    
    if ($DryRun) {
        Write-Host "`n  ⚠️  DRY RUN - No changes were made" -ForegroundColor Yellow
    }
}

# =============================================================================
# Main Script
# =============================================================================

Write-Host "`n" + "=" * 60 -ForegroundColor Cyan
Write-Host "🖼️  TechHub Image Normalizer" -ForegroundColor Cyan
Write-Host "=" * 60 -ForegroundColor Cyan

if ($DryRun) {
    Write-Host "🔍 DRY RUN MODE - No changes will be made" -ForegroundColor Magenta
}

Write-Host "`nConfiguration:"
Write-Host "  Image path: $ImagePath"
Write-Host "  Artist:     $Artist"
Write-Host "  Copyright:  $Copyright"

# Check image path exists
if (-not (Test-Path $ImagePath)) {
    Write-Error "Image path not found: $ImagePath"
    exit 1
}

# Install required packages
Install-RequiredPackages

# Download ICC profile
if (-not (Get-IccProfile)) {
    Write-Error "Failed to obtain ICC profile"
    exit 1
}

# Find all supported images
Write-Host "`n🔍 Scanning for images..." -ForegroundColor Cyan
$images = Get-ChildItem -Path $ImagePath -Recurse -Include "*.webp", "*.jpg", "*.jpeg", "*.jxl", "*.png"

if ($images.Count -eq 0) {
    Write-Host "  No images found in $ImagePath" -ForegroundColor Yellow
    exit 0
}

Write-Host "  Found $($images.Count) images" -ForegroundColor Green

# Group by extension for summary
$byExtension = $images | Group-Object Extension | ForEach-Object {
    Write-Host "    $($_.Name): $($_.Count)"
}

# Process each image
Write-Host "`n🔧 Processing images..." -ForegroundColor Cyan

$processed = 0
$succeeded = 0
$failed = 0

foreach ($image in $images) {
    $processed++
    $relativePath = $image.FullName -replace [regex]::Escape($script:RepoRoot), ""
    Write-Host "`n[$processed/$($images.Count)] $relativePath" -ForegroundColor Yellow
    
    $originalSize = $image.Length
    
    if (Process-Image -File $image) {
        $newSize = (Get-Item $image.FullName).Length
        $sizeDiff = $newSize - $originalSize
        $sizeStr = if ($sizeDiff -gt 0) { "+$sizeDiff" } elseif ($sizeDiff -lt 0) { "$sizeDiff" } else { "±0" }
        Write-Success "Done ($sizeStr bytes)"
        $succeeded++
    }
    else {
        Write-Failure "Failed to process"
        $failed++
    }
}

# Show summary
Show-Summary -Processed $processed -Succeeded $succeeded -Failed $failed

# Cleanup
if (-not $DryRun -and (Test-Path $script:IccProfilePath)) {
    # Keep the ICC profile cached for future runs
    # Remove-Item $script:IccProfilePath -Force
}

Write-Host "`n✅ Done!" -ForegroundColor Green
