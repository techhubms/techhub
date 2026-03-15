function Invoke-AiApiCall {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Token,
        [Parameter(Mandatory = $true)]
        [ValidateSet('staging', 'prod')]
        [string]$Environment,
        [Parameter(Mandatory = $true)]
        [string]$SystemMessage,
        [Parameter(Mandatory = $true)]
        [string]$UserMessage,
        [Parameter(Mandatory = $false)]
        [int]$MaxRetries = 3,
        [Parameter(Mandatory = $false)]
        [int]$RateLimitPreventionDelay = 15
    )

    # Get endpoint and model from centralized config based on environment
    $endpoint = Get-AzureOpenAIEndpoint -Environment $Environment
    $model = Get-AzureOpenAIModelName

    $messages = @(
        @{
            "role"    = "system"
            "content" = $SystemMessage
        },
        @{
            "role"    = "user"
            "content" = $UserMessage
        }
    )

    $apiRequestBody = @{
        "model"    = $model
        "messages" = $messages
    } | ConvertTo-Json -Depth 10 -Compress

    $headers = @{
        "Content-Type" = "application/json"
    }
    
    # Determine authentication and URL based on endpoint
    $uri = $endpoint
    if ($endpoint -like "*.azure.com*") {
        $headers["api-key"] = $Token
        
        # Add API version if not already in URL
        if ($Endpoint -notlike "*api-version=*") {
            if ($Endpoint.Contains("?")) {
                $uri = $Endpoint + "&api-version=2024-05-01-preview"
            }
            else {
                $uri = $Endpoint + "?api-version=2024-05-01-preview"
            }
        }
    }
    else {
        throw "Unsupported endpoint: $endpoint. Only Azure AI Foundry endpoints (*.azure.com) are supported."
    }

    if ($PSCmdlet.ShouldProcess("AI API", "Send chat completion request")) {
        Write-Host "Sending request to: $uri" -ForegroundColor Cyan
        Write-Host "Model: $model" -ForegroundColor Cyan
        Write-Host "Request body length: $($apiRequestBody.Length) characters" -ForegroundColor Cyan
        
        $retryCount = 0
        $success = $false
        $response = $null
            
        while ($retryCount -le $MaxRetries -and -not $success) {
            try {
                if ($retryCount -gt 0) {
                    Write-Host "⚠️ Retry $($retryCount)/$($MaxRetries) for AI API call" -ForegroundColor Yellow
                }
                    
                $response = Invoke-WebRequest -Uri $uri `
                    -Method Post `
                    -Headers $headers `
                    -Body $apiRequestBody `
                    -ContentType "application/json"
                    
                # Always wait 15 seconds after API call to respect rate limits
                Write-Host "API call successful, waiting $RateLimitPreventionDelay seconds before processing response to prevent ratelimits..."
                Start-Sleep -Seconds $RateLimitPreventionDelay

                $success = $true
            }
            catch {
                # Always wait 15 seconds after API call, even on failure, this will give us a delay seconds on the first retry
                Write-ErrorDetails -ErrorRecord $_ -Context "AI API call, waiting $RateLimitPreventionDelay seconds before processing failure..."
                Start-Sleep -Seconds $RateLimitPreventionDelay

                $webResponseDetails = Get-WebResponseDetailsFromException -Exception $_.Exception
                $statusCode = $webResponseDetails.StatusCode
                $statusDescription = $webResponseDetails.StatusDescription
                $responseHeaders = $webResponseDetails.Headers
                $responseContent = $webResponseDetails.ResponseContent
            
                if ($statusCode) {
                    Write-Host "Response StatusCode: $statusCode"
                }

                if ($statusDescription) {
                    Write-Host "Response StatusDescription: $statusDescription"
                }

                # Log response content for 400 errors to help debug
                if ($statusCode -eq 400) {
                    Write-Host "Response Content for 400 error:" -ForegroundColor Yellow
                    if ($responseContent) {
                        Write-Host $responseContent -ForegroundColor Yellow
                    } else {
                        Write-Host "No response content available" -ForegroundColor Yellow
                    }
                }

                if ($statusCode -eq 413 -or $statusDescription -eq 'Request Entity Too Large') {
                    return ([PSCustomObject]@{ Error = $true; Type = "RequestEntityTooLarge"; Message = "Request entity too large" } | ConvertTo-Json -Compress)
                }
                
                # Check for rate limit immediately
                if ($statusCode -eq 429) {
                    # Process headers and try to get rate limit wait time
                    $rateLimitSeconds = $null
                    if ($responseHeaders) {
                        Write-Host "Response Headers:"
                        foreach ($header in $responseHeaders.GetEnumerator()) {
                            # Handle both HttpResponseHeaders (.GetValues method) and Dictionary/Hashtable (.Value property)
                            if ($responseHeaders.PSObject.Methods['GetValues']) {
                                $headerValues = @($responseHeaders.GetValues($header.Key))
                            }
                            else {
                                # For Dictionary/Hashtable in tests, use the Value directly
                                $headerValues = @($header.Value)
                            }
                            $headerValue = $headerValues -join ", "

                            Write-Host " - $($header.Key): $headerValue"
                
                            # Check for rate limit headers
                            if ($headerValue -match '^\d+$') {
                                if ($header.Key -eq "x-ratelimit-timeremaining") {
                                    $rateLimitSeconds = $headerValue
                                }
                                elseif ($header.Key -eq "retry-after" -and -not $rateLimitSeconds) {
                                    $rateLimitSeconds = $headerValue
                                }
                            }
                        }
                    }
                    else {
                        Write-Host "No response headers but statuscode is 429, setting rate limit seconds to 300"
                        $rateLimitSeconds = 300
                    }

                    if ($rateLimitSeconds) {
                        # If rate limit is 60 seconds or less, wait and retry
                        if ([int]$rateLimitSeconds -le 60) {
                            Write-Host "⏱️  Rate limit detected: $rateLimitSeconds seconds. Will wait 60 seconds and retry..." -ForegroundColor Yellow
                            Start-Sleep -Seconds 60
                            $retryCount++
                            continue # Go to next retry iteration
                        }
                        else {
                            # For longer rate limits, save to file and return error as before
                            $sourceRoot = Get-SourceRoot
                            $scriptsPath = Join-Path $sourceRoot "scripts"
                            $rateLimitEndDatePath = Join-Path $scriptsPath "rate-limit-enddate.json"
                            $endDate = (Get-Date).AddSeconds([int]$rateLimitSeconds)
                            $json = @{ endDate = $endDate.ToString("o") } | ConvertTo-Json
                            $json | Out-File -FilePath $rateLimitEndDatePath -Force
                            return ([PSCustomObject]@{ Error = $true; Type = "RateLimit"; RateLimitSeconds = $rateLimitSeconds; Message = "Rate limit exceeded, wait $rateLimitSeconds seconds" } | ConvertTo-Json -Compress)
                        }
                    }
                }
        
                try {
                    # Try to read response content for content filter detection
                    if ($responseContent) {
                        $contentToCheck = $responseContent
                    } else {
                        # If responseContent is not available, try to extract from exception message
                        $contentToCheck = $_.Exception.Message
                    }
                    
                    if ($contentToCheck) {
                        $errorPatterns = @(
                            "content.*filter",
                            "content.*policy",
                            "inappropriate.*content",
                            "content.*violation",
                            "blocked.*content",
                            "safety.*filter",
                            "content.*moderation",
                            "jailbreak.*filtered",
                            "ResponsibleAIPolicyViolation",
                            "content_filter"
                        )
                
                        # Convert to string first, then to lowercase for pattern matching
                        $errorText = $contentToCheck.ToString().ToLower()
                        foreach ($pattern in $errorPatterns) {
                            if ($errorText -match $pattern) {
                                Write-Host "❌ Content filter violation detected. Pattern: $pattern" -ForegroundColor Red
                                return ([PSCustomObject]@{ Error = $true; Type = "ContentFilter"; Pattern = $pattern; Message = "Content filter violation: $pattern"; ResponseContent = $contentToCheck.ToString() } | ConvertTo-Json -Compress)
                            }
                        }
                    }
                }
                catch {
                    Write-ErrorDetails -ErrorRecord $_ -Context "Failed to process web response details and look for rate limit or content filter errors"
                }

                $retryCount++
                    
                if ($retryCount -le $MaxRetries) {
                    # Calculate exponential backoff delay: rateLimitDelay * 2^retryCount
                    $delaySeconds = [Math]::Pow(2, $retryCount) * $RateLimitPreventionDelay
                    Write-Host "⚠️ API attempt $retryCount failed: $($_.Exception.Message)" -ForegroundColor Yellow
                    Write-Host "⏳ Additional retry delay: $delaySeconds seconds..." -ForegroundColor Yellow
                    Start-Sleep -Seconds $delaySeconds
                }
                else {
                    throw
                }
            }
        }
            
        # Check if response is null (all retries failed)
        if ($null -eq $response) {
            Write-Host "❌ All API call retries failed - no response received" -ForegroundColor Red
            return ([PSCustomObject]@{ 
                Error = $true 
                Type = "AllRetriesFailed" 
                Message = "All API call retries failed - no response received"
            } | ConvertTo-Json -Compress)
        }
            
        # Process the successful response and extract content
        $responseJson = $null
        try {
            $responseJson = $response.Content | ConvertFrom-Json
            $contentString = $responseJson.choices[0].message.content
            
            # Repair invalid JSON escape sequences from AI responses (e.g., \s, \d, \w)
            # AI models sometimes generate invalid escapes that break ConvertFrom-Json
            $contentString = Repair-JsonResponse -JsonString $contentString
            
            return $contentString
        }
        catch {
            Write-ErrorDetails -ErrorRecord $_ -Context "Failed to parse AI model response"
            
            return ([PSCustomObject]@{ 
                Error           = $true 
                Type            = "ResponseParseError" 
                Message         = "Failed to parse AI model response: $($_.Exception.Message)"
                ResponseContent = if ($null -ne $responseJson) { $responseJson } elseif ($response -and $response.Content) { $response.Content } else { $null }
            } | ConvertTo-Json -Compress)
        }
    }
    else {
        Write-Host "What if: Would send API request to AI model"
        Write-Host "What if: Model: $Model"
        Write-Host "What if: SystemMessage length: $($SystemMessage.Length) characters"
        Write-Host "What if: UserMessage length: $($UserMessage.Length) characters"
        
        # Return a dummy content string that can be parsed as JSON by the calling function
        return '{"title":"Sample Article Title","description":"This is a sample description generated in What if mode for testing purposes. The content would normally be processed by the AI model to create meaningful summaries.","excerpt":"This is a sample excerpt generated in What if mode for testing purposes. The content would normally be processed by the AI model to create meaningful summaries.","content":"# Sample Article Content\\n\\nThis is sample content that would be generated by the AI model in a real scenario. The actual content would be based on the RSS feed item provided.\\n\\n## Key Points\\n\\n- Point 1 from the article\\n- Point 2 from the article\\n- Point 3 from the article","tags":["AI","Testing","Sample","GitHub Copilot"],"categories":["AI"]}'
    }
}
