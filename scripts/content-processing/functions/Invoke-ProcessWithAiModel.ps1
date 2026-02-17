function Invoke-ProcessWithAiModel {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Token,
        [Parameter(Mandatory = $true)]
        [ValidateSet('staging', 'prod')]
        [string]$Environment,
        [Parameter(Mandatory = $true)]
        [hashtable]$InputData,
        [Parameter(Mandatory = $false)]
        [int]$MaxRetries = 3,
        [Parameter(Mandatory = $false)]
        [int]$RateLimitPreventionDelay = 15
    )

    $sourceRoot = Get-SourceRoot
    $contentProcessingPath = Join-Path $sourceRoot "scripts/content-processing"
    $systemMessagePath = Join-Path $contentProcessingPath "system-message.md"
    $systemMessage = Get-Content -Path $systemMessagePath -Raw

    # Convert the input data hashtable to JSON for the API call
    $inputDataJson = $InputData | ConvertTo-Json -Depth 10 -Compress

    if ($PSCmdlet.ShouldProcess("AI Model", "Process RSS content with AI model")) {
        # Call the generic AI API function with individual messages
        $response = Invoke-AiApiCall `
            -Token $Token `
            -Environment $Environment `
            -SystemMessage $systemMessage `
            -UserMessage $inputDataJson `
            -MaxRetries $MaxRetries `
            -RateLimitPreventionDelay $RateLimitPreventionDelay

        # Parse the response as JSON first (since Invoke-AiApiCall content could be invalid JSON)
        try {
            $responseObject = $response | ConvertFrom-Json
            return $responseObject
        }
        catch {
            Write-ErrorDetails -ErrorRecord $_ -Context "Failed to parse AI model response content as JSON"
        
            return ([PSCustomObject]@{ 
                Error           = $true 
                Type            = "JsonParseError" 
                ResponseContent = $response
            } | ConvertTo-Json -Compress)
        }
    }
    else {
        Write-Host "What if: Would process RSS content with AI model"
        Write-Host "What if: Environment: $Environment"
        Write-Host "What if: InputData keys: $($InputData.Keys -join ', ')"
        
        # Return a dummy response object for WhatIf mode
        return [PSCustomObject]@{
            title = "Sample RSS Article Title"
            description = "This is a sample description generated in What if mode for testing purposes."
            excerpt = "This is a sample excerpt generated in What if mode."
            content = "# Sample RSS Content`n`nThis would be the processed content from the RSS item."
            tags = @("AI", "Testing", "Sample", "RSS")
            categories = @("AI")
        }
    }
}