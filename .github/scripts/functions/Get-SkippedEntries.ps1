function Get-SkippedEntries {
    param([string]$SkippedEntriesPath)
    return Get-Entries -EntriesPath $SkippedEntriesPath
}
