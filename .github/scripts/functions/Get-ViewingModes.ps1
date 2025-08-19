function Get-ViewingModes {
    <#
    .SYNOPSIS
    Returns the viewing modes configuration for different content directories.
    
    .DESCRIPTION
    This function returns a hashtable mapping content directories to their viewing modes.
    This configuration is used to determine how different types of content should be displayed.
    
    .OUTPUTS
    [hashtable] A hashtable with directory names as keys and viewing modes as values.
    
    .EXAMPLE
    $viewingModes = Get-ViewingModes
    $viewingModes["_videos"]  # Returns "internal"
    #>
    
    return @{
        "_videos"    = "internal"
        "_posts"     = "external"
        "_news"      = "external"
        "_community" = "external"
        "_events"    = "internal"
        "_roundups"  = "internal"
    }
}
