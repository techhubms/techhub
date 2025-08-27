function Get-WebResponseDetailsFromException {
    param(
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        $Exception
    )

    $statusCode = $null
    $statusDescription = $null
    $responseContent = $null
    $headers = $null

    try {
        # Extract webResponse from exception
        $webResponse = $null
        if ($Exception.PSObject.Properties.Name -contains "Response" -and $Exception.Response) {
            $webResponse = $Exception.Response
        }
        elseif ($Exception.PSObject.Properties.Name -contains "WebResponse" -and $Exception.WebResponse) {
            $webResponse = $Exception.WebResponse
        }

        if ($webResponse -and $webResponse.PSObject.Properties.Name -contains "StatusCode") {
            $statusCode = $webResponse.StatusCode
            if ($webResponse.PSObject.Properties.Name -contains "StatusDescription") {
                $statusDescription = $webResponse.StatusDescription
            }
            if ($webResponse.PSObject.Properties.Name -contains "Headers") {
                $headers = $webResponse.Headers
            }
        }
        
        # If no webResponse, try to extract status from exception message (for HttpRequestException)
        if (-not $webResponse -and $Exception.PSObject.Properties.Name -contains "Message") {
            $message = $Exception.Message
            if ($message -match "Response status code does not indicate success: (\d+) \(([^)]+)\)") {
                $statusCode = [int]$matches[1]
                $statusDescription = $matches[2]
            }
        }

        # Try to read response content
        try {
            if ($webResponse -and $webResponse.PSObject.Methods -and $webResponse.PSObject.Methods.Name -contains "GetResponseStream") {
                $stream = $webResponse.GetResponseStream()
                if ($stream) {
                    if ($stream.PSObject.Methods -and $stream.PSObject.Methods.Name -contains "ReadToEnd") {
                        $responseContent = $stream.ReadToEnd()
                    } else {
                        $reader = New-Object System.IO.StreamReader($stream)
                        $responseContent = $reader.ReadToEnd()
                        $reader.Close()
                        $stream.Close()
                    }
                }
            }
            elseif ($webResponse -and $webResponse.PSObject.Properties.Name -contains "Content") {
                $content = $webResponse.Content
                # Handle HttpConnectionResponseContent objects that need to be read asynchronously
                if ($content.GetType().Name -eq "HttpConnectionResponseContent") {
                    try {
                        # Try to read the content as a string
                        $responseContent = $content.ReadAsStringAsync().GetAwaiter().GetResult()
                    }
                    catch {
                        # Fallback to converting to string
                        $responseContent = $content.ToString()
                    }
                } else {
                    $responseContent = $content
                }
            }
            # Special handling for HttpConnectionResponseContent - try to get content from the error message
            elseif ($Exception.Message -and $Exception.Message.Contains("{")){
                # Extract JSON content from the error message if it's embedded
                $startIndex = $Exception.Message.IndexOf("{")
                if ($startIndex -ge 0) {
                    $jsonPart = $Exception.Message.Substring($startIndex)
                    # Clean up any trailing text that might not be part of the JSON
                    try {
                        $testJson = $jsonPart | ConvertFrom-Json
                        $responseContent = $jsonPart
                    }
                    catch {
                        # If the entire JSON doesn't parse, try to find just the error object
                        $responseContent = $jsonPart
                    }
                }
            }
        }
        catch {
            Write-ErrorDetails -ErrorRecord $_ -Context "Failed to get response content from web response"
        }
    }
    catch {
        Write-ErrorDetails -ErrorRecord $_ -Context "Failed to extract web response details"
    }

    return [PSCustomObject]@{
        StatusCode        = $statusCode
        StatusDescription = $statusDescription
        ResponseContent   = $responseContent
        Headers           = $headers
    }
}
