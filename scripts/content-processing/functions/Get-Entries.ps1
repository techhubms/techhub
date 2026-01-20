function Get-Entries {
    param([string]$EntriesPath)
    if (Test-Path $EntriesPath) {
        $content = Get-Content $EntriesPath -Raw | ConvertFrom-Json
        # Convert canonical_url from JSON to external_url for internal use
        $entries = @($content) | ForEach-Object {
            $entry = $_
            # Create new object with external_url instead of canonical_url
            $newEntry = [PSCustomObject]@{
                external_url = $entry.canonical_url
                collection   = $entry.collection
                timestamp    = $entry.timestamp
            }
            # Add reason if it exists
            if ($entry.PSObject.Properties.Name -contains 'reason') {
                $newEntry | Add-Member -NotePropertyName 'reason' -NotePropertyValue $entry.reason
            }
            $newEntry
        }
        return $entries
    }
    return @()
}

