#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Generates ASP.NET Core development certificate for HTTPS in Docker containers.

.DESCRIPTION
    Creates a self-signed certificate for local HTTPS development with docker-compose.
    The certificate is saved to ~/.aspnet/https/ and mounted into containers.

.EXAMPLE
    ./Generate-DevCertificate.ps1
#>

$ErrorActionPreference = "Stop"

$httpsDir = Join-Path $HOME ".aspnet" "https"
$certPath = Join-Path $httpsDir "aspnetapp.pfx"
$password = "devpass"

Write-Host "🔐 Generating ASP.NET Core development certificate for Docker..." -ForegroundColor Cyan

# Create directory if it doesn't exist
if (-not (Test-Path $httpsDir)) {
    New-Item -ItemType Directory -Path $httpsDir -Force | Out-Null
    Write-Host "  ✅ Created directory: $httpsDir" -ForegroundColor Green
}

# Check if certificate already exists
if (Test-Path $certPath) {
    $response = Read-Host "Certificate already exists at $certPath. Regenerate? (y/N)"
    if ($response -ne "y" -and $response -ne "Y") {
        Write-Host "  ℹ️  Using existing certificate" -ForegroundColor Yellow
        exit 0
    }
    Remove-Item $certPath -Force
}

# Generate certificate using dotnet dev-certs
Write-Host "  🔨 Generating certificate..." -ForegroundColor Cyan
dotnet dev-certs https -ep $certPath -p $password --trust

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ Certificate generated and trusted!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Certificate Details:" -ForegroundColor Cyan
    Write-Host "  Path:     $certPath" -ForegroundColor White
    Write-Host "  Password: $password" -ForegroundColor White
    Write-Host ""
    Write-Host "You can now run: docker-compose up" -ForegroundColor Green
} else {
    Write-Host "  ❌ Failed to generate certificate" -ForegroundColor Red
    exit 1
}
