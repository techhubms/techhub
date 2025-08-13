function Add-TrackingEntry {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [string]$EntriesPath,
        [string]$CanonicalUrl,
        [string]$Collection,
        [string]$Reason = ""
    )
    
    $entry = @{
        canonical_url = $CanonicalUrl
        collection    = $Collection
        timestamp     = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss zzz")
    }
    
    if (-not [string]::IsNullOrWhiteSpace($Reason)) {
        $entry.reason = $Reason
    }
    
    $entries = @(Get-Entries -EntriesPath $EntriesPath)
    $entries += $entry
    $entries = @($entries)
    if ($PSCmdlet.ShouldProcess($EntriesPath, "Add entry to tracking file")) {
        $entries | ConvertTo-Json -Depth 10 | Set-Content -Path $EntriesPath -Encoding UTF8 -Force
        #Write-Host "Added entry to $($EntriesPath): $($entry.canonical_url)"
    }
    else {
        Write-Host "What if: Would add entry to $EntriesPath for $($entry.canonical_url)"
    }
}
