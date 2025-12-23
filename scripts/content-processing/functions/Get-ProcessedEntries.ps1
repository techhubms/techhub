function Get-ProcessedEntries {
    param([string]$ProcessedEntriesPath)
    return Get-Entries -EntriesPath $ProcessedEntriesPath
}
