function Get-Entries {
    param([string]$EntriesPath)
    if (Test-Path $EntriesPath) {
        $content = Get-Content $EntriesPath -Raw | ConvertFrom-Json
        return @($content)
    }
    return @()
}
