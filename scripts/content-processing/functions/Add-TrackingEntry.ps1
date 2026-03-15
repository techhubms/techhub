function Add-TrackingEntry {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [string]$EntriesPath,
        [string]$ExternalUrl,
        [string]$Collection,
        [string]$Reason = ""
    )
    
    # Create entry with canonical_url for JSON storage
    $entry = @{
        canonical_url = $ExternalUrl
        collection    = $Collection
        timestamp     = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss zzz")
    }
    
    if (-not [string]::IsNullOrWhiteSpace($Reason)) {
        $entry.reason = $Reason
    }
    
    # Get existing entries (they come back with external_url from Get-Entries)
    $existingEntries = @(Get-Entries -EntriesPath $EntriesPath)
    
    # Convert existing entries back to canonical_url format for JSON storage
    $jsonEntries = @($existingEntries | ForEach-Object {
        $jsonEntry = @{
            canonical_url = $_.external_url
            collection    = $_.collection
            timestamp     = $_.timestamp
        }
        if ($_.PSObject.Properties.Name -contains 'reason') {
            $jsonEntry.reason = $_.reason
        }
        $jsonEntry
    })
    
    # Add new entry
    $jsonEntries += $entry
    $jsonEntries = @($jsonEntries)
    
    if ($PSCmdlet.ShouldProcess($EntriesPath, "Add entry to tracking file")) {
        $jsonEntries | ConvertTo-Json -Depth 10 | Set-Content -Path $EntriesPath -Encoding UTF8 -Force
    }
    else {
        Write-Host "What if: Would add entry to $EntriesPath for $ExternalUrl"
    }
}
