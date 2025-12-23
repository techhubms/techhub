function Save-AiApiResult {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [hashtable] $InputData,
        [Parameter(Mandatory = $true)]
        [object]$Response,
        [Parameter(Mandatory = $true)]
        [string]$Url,
        [Parameter(Mandatory = $true)]
        [string]$Model,
        [Parameter(Mandatory = $true)]
        [DateTime]$PubDate,
        [Parameter(Mandatory = $true)]
        [string]$AiResultsPath
    )

        # Ensure airesults directory exists
    if (-not (Test-Path $AiResultsPath)) {
        New-Item -ItemType Directory -Path $AiResultsPath | Out-Null
    }

    # Format the publication date for filename usage
    $pubDateForFilename = $PubDate.ToString("yyyy-MM-dd")

    # Determine the title to use for filename generation
    $titleForFilename = $null
    if ($Response -and $Response.PSObject.Properties.Name -contains 'title' -and $Response.title) {
        # Use response title if available (successful AI processing)
        $titleForFilename = $Response.title
    }
    elseif ($InputData -and $InputData.ContainsKey('title') -and $InputData.title) {
        # Fall back to input title if response title not available (error cases)
        $titleForFilename = $InputData.title
    }
    else {
        # Last resort: use a generic title with timestamp
        $titleForFilename = "Unknown-Title-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
    }

    # Generate safe filename
    $titleSlug = ConvertTo-SafeFilename -Title $titleForFilename -MaxLength 200
    $baseFilename = "$pubDateForFilename-$titleSlug"

    # Create AI result object
    $aiResultObj = @{ 
        inputData = $InputData
        response  = $Response
        url       = $Url
        model     = $Model 
    } | ConvertTo-Json -Depth 10

    # Generate AI result filename
    $aiResultFile = Join-Path $AiResultsPath ("airesult-$baseFilename.json")
        
    # Save AI result to file
    Set-Content -Path $aiResultFile -Value $aiResultObj -Encoding UTF8 -Force
        
    Write-Host "Saved AI result to: $aiResultFile"
}
