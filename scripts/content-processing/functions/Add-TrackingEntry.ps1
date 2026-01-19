function Add-TrackingEntry {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [string]$EntriesPath,
        [string]$ExternalUrl,
        [string]$Collection,
        [string]$Reason = ""
    )
    
    $entry = @{
        canonical_url = $ExternalUrl
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
        # DEBUG: Log all add operations to detect duplicates
        $hasReason = if ($entry.ContainsKey("reason")) { "WITH reason" } else { "WITHOUT reason" }
        Write-Host "🔍 DEBUG: Adding entry $hasReason to $(Split-Path $EntriesPath -Leaf): $($entry.canonical_url)" -ForegroundColor Cyan
        
        $entries | ConvertTo-Json -Depth 10 | Set-Content -Path $EntriesPath -Encoding UTF8 -Force
        #Write-Host "Added entry to $($EntriesPath): $($entry.external_url)"
    }
    else {
        Write-Host "What if: Would add entry to $EntriesPath for $($entry.canonical_url)"
    }
}
