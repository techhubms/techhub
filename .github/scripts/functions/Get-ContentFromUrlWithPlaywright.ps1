function Get-ContentFromUrlWithPlaywright {
    <#
    .SYNOPSIS
    Fetches content from URLs using Playwright for JavaScript-enabled pages.
    
    .PARAMETER Urls
    The URLs to fetch content from. Can be a single URL string or an array of URLs.
    
    .PARAMETER TimeoutSeconds
    The timeout in seconds for each request.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Urls,
        
        [Parameter(Mandatory = $false)]
        [int]$TimeoutSeconds = 10
    )
    
    try {
        # Check if the E2E Playwright setup is available
        $sourceRoot = Get-SourceRoot
        $e2eDir = Join-Path $sourceRoot "spec/e2e"
        $fetchScript = Join-Path $e2eDir "fetch-content.js"
        
        if (-not (Test-Path $e2eDir)) {
            throw "E2E test directory not found at: $e2eDir. Playwright functionality requires the E2E test setup."
        }
        
        if (-not (Test-Path $fetchScript)) {
            throw "Playwright fetch script not found at: $fetchScript. Please ensure the E2E test setup is complete."
        }
        
        # Check if Playwright is available in the E2E installation
        $null = & npm --prefix $e2eDir exec playwright --version 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "Playwright is not installed in the E2E test setup. Please run the E2E test installation."
        }
        
        # Change to E2E directory and run the script with node directly
        $originalLocation = Get-Location
        try {
            Set-Location $e2eDir
            # Pass timeout and all URLs as arguments to the script
            $nodeArgs = @("fetch-content.js", $TimeoutSeconds) + $Urls
            $jsonOutput = & node $nodeArgs
        }
        finally {
            Set-Location $originalLocation
        }
        
        if ($LASTEXITCODE -ne 0) {
            throw "Playwright execution failed with exit code: $LASTEXITCODE"
        }
        
        if (-not $jsonOutput) {
            throw "No content received from Playwright"
        }
        
        # Join the output if it's an array (PowerShell splits lines into array)
        if ($jsonOutput -is [array]) {
            $jsonString = $jsonOutput -join ""
        } else {
            $jsonString = $jsonOutput
        }
        
        # Parse the JSON output to get array of objects with url and content
        try {
            $results = $jsonString | ConvertFrom-Json
            return $results
        }
        catch {
            throw "Failed to parse Playwright output as JSON: $($_.Exception.Message)"
        }
    }
    catch {
        throw "Error fetching content with Playwright from URLs [$($Urls -join ', ')]: $($_.Exception.Message)"
    }
}
