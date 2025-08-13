$functionsPath = Join-Path $PSScriptRoot "../../.github/scripts/functions"

. (Join-Path $functionsPath "Write-ErrorDetails.ps1")
    
Get-ChildItem -Path $functionsPath -Filter "*.ps1" | 
Where-Object { $_.Name -ne "Write-ErrorDetails.ps1" } |
ForEach-Object { . $_.FullName }

$tempBase = if ($env:TEMP) { $env:TEMP } elseif ($env:TMP) { $env:TMP } elseif ($env:TMPDIR) { $env:TMPDIR } else { "/tmp" }
$tempPath = Join-Path $tempBase "pwshtests"

if (Test-Path $tempPath) {
    Remove-Item $tempPath -Recurse -Force
}
New-Item -ItemType Directory -Path $tempPath -Force | Out-Null

$script:TempPath = $tempPath