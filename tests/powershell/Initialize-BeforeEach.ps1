# Disable progress bars for performance
$ProgressPreference = 'SilentlyContinue'

if (Test-Path $global:TempPath) {
    Remove-Item -Path $global:TempPath -Recurse -Force -ErrorAction SilentlyContinue
}
New-Item -ItemType Directory -Path $global:TempPath -Force | Out-Null

Mock Start-Sleep { }
Mock Invoke-WebRequest { throw "Should not make HTTP calls" }
Mock Invoke-RestMethod { throw "Should not make HTTP calls" }
Mock Write-ErrorDetails { }
Mock Get-ContentFromUrl { }
