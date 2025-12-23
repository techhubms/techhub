function Get-CollectionPriorityOrder {
    <#
    .SYNOPSIS
    Returns the collection priority order for RSS feed processing.

    .DESCRIPTION
    Returns a hashtable defining the priority order for different collection types.
    Lower numbers indicate higher priority and will be processed first.

    .OUTPUTS
    Hashtable with collection names as keys and priority numbers as values.

    .EXAMPLE
    $collectionOrder = Get-CollectionPriorityOrder
    $priority = $collectionOrder["_news"]  # Returns 10
    #>

    return @{
        "_news" = 10
        "_posts" = 20
        "_videos" = 30
        "_events" = 40
        "_community" = 90
        "_roundups" = 100
    }
}
