function Get-ContentFromUrl {
    <#
    .SYNOPSIS
    Fetches content from a URL and converts it to markdown.
    
    .PARAMETER Url
    The URL to fetch content from.
    
    .PARAMETER TimeoutSeconds
    The timeout in seconds for the request.
    
    .PARAMETER MaxRetries
    Maximum number of retry attempts. Default is 3.
    
    .PARAMETER BaseDelayMs
    Base delay in milliseconds for exponential backoff. Default is 1000.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Url,
        
        [Parameter(Mandatory = $false)]
        [int]$TimeoutSeconds = 10,
        
        [Parameter(Mandatory = $false)]
        [int]$MaxRetries = 3,
        
        [Parameter(Mandatory = $false)]
        [int]$BaseDelayMs = 1000
    )
    
    
    $retryCount = 0
    $success = $false
    $html = ""
    
    while ($retryCount -le $MaxRetries -and -not $success) {
        try {
            if ($retryCount -gt 0) {
                Write-Host "⚠️ Retry $($retryCount)/$($MaxRetries) for URL: $Url" -ForegroundColor Yellow
            }
            
            $webRequest = [System.Net.WebRequest]::Create($Url)
            $webRequest.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36"
            $webRequest.Timeout = $TimeoutSeconds * 1000
            $webRequest.Method = "GET"
            $webRequest.Headers.Add("Accept", "application/rss+xml, application/atom+xml, application/xml, text/xml, */*")
            $webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.9")
            $webRequest.Headers.Add("Accept-Encoding", "gzip, deflate")
            $webRequest.AutomaticDecompression = [System.Net.DecompressionMethods]::GZip -bor [System.Net.DecompressionMethods]::Deflate
            
            # Enable automatic redirect following (handles 301, 302, 307, 308)
            if ($webRequest -is [System.Net.HttpWebRequest]) {
                $webRequest.AllowAutoRedirect = $true
                $webRequest.MaximumAutomaticRedirections = 10
            }
            $response = $webRequest.GetResponse()

            if ($response.StatusCode -ne 200) {
                throw "Failed to fetch content from $Url. Status code: $($response.StatusCode)"
            }
            
            $responseStream = $response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($responseStream)
            $html = $reader.ReadToEnd()
            $reader.Close()
            $response.Close()
            
            if (-not $html) {
                throw "No content received from URL"
            }

            $success = $true
        }
        catch {
            $retryCount++
            $errorMessage = $_.Exception.Message
            
            if ($retryCount -le $MaxRetries) {
                # Calculate exponential backoff delay: baseDelay * 2^(retryCount-1)
                $delayMs = $BaseDelayMs * [Math]::Pow(2, $retryCount - 1)
                Write-Host "⚠️ Attempt $retryCount failed for $Url`: $errorMessage" -ForegroundColor Yellow
                Write-Host "⏳ Waiting $($delayMs)ms before retry..." -ForegroundColor Yellow
                Start-Sleep -Milliseconds $delayMs
            }
            else {
                throw "Error fetching content from $Url after $MaxRetries retries: $errorMessage"
            }
        }
    }

    return $html
}
