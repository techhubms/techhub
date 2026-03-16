function Test-AiResponseFormat {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Response,
        [Parameter(Mandatory = $true)]
        [string]$StepName
    )
    
    # Check if response indicates an error
    if ($Response -like "*I cannot*" -or $Response -like "*I'm unable*" -or $Response -like "*I don't have*") {
        return @{
            IsValid      = $false
            ErrorMessage = "AI indicated it cannot complete the request: $($Response.Substring(0, [Math]::Min(200, $Response.Length)))"
        }
    }
    
    # Check if response contains error JSON (handles both compressed and non-compressed JSON)
    if ($Response -like "*`"Error`":true*" -or $Response -like "*`"Error`": true*") {
        try {
            $errorObj = $Response | ConvertFrom-Json
            Write-Host "❌ $StepName failed with error type: $($errorObj.Type)" -ForegroundColor Red
            if ($errorObj.PSObject.Properties.Name -contains "ResponseContent" -and $errorObj.ResponseContent) {
                Write-Host "Response content: $($errorObj.ResponseContent)" -ForegroundColor Red
            }
            return @{ IsValid = $false; ErrorType = $errorObj.Type; ErrorMessage = "$StepName failed: $($errorObj.Type)" }
        }
        catch {
            Write-Host "❌ $StepName failed but could not parse error response" -ForegroundColor Red
            Write-Host "Raw response: $Response" -ForegroundColor Red
            return @{ IsValid = $false; ErrorType = "UnparseableError"; ErrorMessage = "$StepName failed with unparseable error response" }
        }
    }
    
    # Check if response is empty or too short to be useful
    if ([string]::IsNullOrWhiteSpace($Response) -or $Response.Length -lt 10) {
        Write-Host "❌ $StepName returned empty or too short response" -ForegroundColor Red
        Write-Host "Response length: $($Response.Length)" -ForegroundColor Red
        return @{ IsValid = $false; ErrorType = "EmptyResponse"; ErrorMessage = "$StepName returned empty or too short response" }
    }
    
    # Response appears to be successful
    return @{ IsValid = $true; ErrorType = $null; ErrorMessage = $null }
}
