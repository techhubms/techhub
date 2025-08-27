if (Test-Path $script:TempPath) {
    Remove-Item -Path $script:TempPath -Recurse -Force -ErrorAction SilentlyContinue
}
New-Item -ItemType Directory -Path $script:TempPath -Force | Out-Null

Mock Start-Sleep { }
Mock Invoke-WebRequest { throw "Should not make HTTP calls" }
Mock Invoke-RestMethod { throw "Should not make HTTP calls" }
Mock Write-ErrorDetails { }
Mock Get-ContentFromUrl { }
Mock Get-ContentFromUrlWithPlaywright { }
