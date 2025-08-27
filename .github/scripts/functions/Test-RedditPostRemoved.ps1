function Test-RedditPostRemoved {
    <#
    .SYNOPSIS
    Checks if a Reddit post has been removed, deleted, or banned.
    
    .DESCRIPTION
    Uses Reddit's JSON API to check the status of a Reddit post and determine
    if it has been removed, deleted, banned, or marked as spam.
    
    .PARAMETER RedditUrl
    The Reddit URL to check (will be converted to JSON API URL).
    
    .PARAMETER FilePath
    The file path for logging purposes (optional).
    
    .OUTPUTS
    System.Collections.Hashtable with properties:
    - IsRemoved: Boolean indicating if the post is removed
    - Error: Any error that occurred during checking
    - Details: Description of the removal reason or status
    - PostData: The raw post data from Reddit API
    
    .EXAMPLE
    $result = Test-RedditPostRemoved -RedditUrl "https://reddit.com/r/programming/comments/abc123/title"
    if ($result.IsRemoved) {
        Write-Host "Post removed: $($result.Details)"
    }
    #>
    
    param(
        [Parameter(Mandatory=$true)]
        [string]$RedditUrl,
        
        [Parameter(Mandatory=$false)]
        [string]$FilePath
    )
    
    $result = @{
        IsRemoved = $false
        Error     = $null
        Details   = $null
        PostData  = $null
    }
    
    try {
        # Convert to JSON API URL
        $jsonUrl = $RedditUrl.TrimEnd('/') + '.json'
        
        $response = Get-ContentFromUrl -Url $jsonUrl
        $jsonData = $response | ConvertFrom-Json
        
        # Extract post data
        if ($jsonData -and $jsonData.Count -gt 0 -and 
            $jsonData[0].data -and $jsonData[0].data.children -and 
            $jsonData[0].data.children.Count -gt 0) {
            
            $postData = $jsonData[0].data.children[0].data
            $result.PostData = $postData
            
            # Check various removal indicators
            $removalReasons = @()
            
            # Note: Reddit doesn't use a simple .removed boolean property
            # Instead, check the removal-related properties for non-null/non-empty values
            if ($postData.PSObject.Properties.Name -contains 'removed' -and $postData.removed -eq $true) {
                $removalReasons += "Post marked as removed"
                $result.IsRemoved = $true
            }
            
            if ($postData.removed_by -and $null -ne $postData.removed_by -and $postData.removed_by -ne "") {
                $removalReasons += "Removed by: $($postData.removed_by)"
                $result.IsRemoved = $true
            }
            
            if ($postData.removed_by_category -and $null -ne $postData.removed_by_category -and $postData.removed_by_category -ne "") {
                $removalReasons += "Removed by category: $($postData.removed_by_category)"
                $result.IsRemoved = $true
            }
            
            if ($postData.removal_reason -and $null -ne $postData.removal_reason -and $postData.removal_reason -ne "") {
                $removalReasons += "Removal reason: $($postData.removal_reason)"
                $result.IsRemoved = $true
            }
            
            if ($postData.banned_by -and $null -ne $postData.banned_by -and $postData.banned_by -ne "") {
                $removalReasons += "Banned by: $($postData.banned_by)"
                $result.IsRemoved = $true
            }
            
            if ($postData.banned_at_utc -and $null -ne $postData.banned_at_utc -and $postData.banned_at_utc -ne "") {
                $removalReasons += "Banned at: $($postData.banned_at_utc)"
                $result.IsRemoved = $true
            }
            
            if ($postData.author -eq "[deleted]") {
                $removalReasons += "Author deleted account"
                $result.IsRemoved = $true
            }
            
            if ($postData.selftext -eq "[removed]") {
                $removalReasons += "Content removed"
                $result.IsRemoved = $true
            }
            
            if ($postData.PSObject.Properties.Name -contains 'spam' -and $postData.spam -eq $true) {
                $removalReasons += "Marked as spam"
                $result.IsRemoved = $true
            }
            
            # Additional checks for common removal indicators
            if ($postData.title -and $postData.title -match '\[removed\]|\[deleted\]') {
                $removalReasons += "Title indicates removal"
                $result.IsRemoved = $true
            }
            
            if ($result.IsRemoved) {
                $result.Details = $removalReasons -join "; "
            }
            else {
                $result.Details = "Post is active"
                Write-Host "      ✅ Post is active" -ForegroundColor Green
            }
        }
        else {
            # No data returned - likely deleted
            $result.IsRemoved = $true
            $result.Details = "No post data returned from Reddit API"
        }
        
    }
    catch {
        $result.Error = $_.Exception.Message
        Write-Host "      ⚠️  Error checking post: $($_.Exception.Message)" -ForegroundColor Yellow
    }
    
    return $result
}
