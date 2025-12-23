function Test-RateLimitInEffect {
    param()
    
    $sourceRoot = Get-SourceRoot
    $scriptsPath = Join-Path $sourceRoot "scripts"
    $rateLimitEndDatePath = Join-Path $scriptsPath "rate-limit-enddate.json"
    
    if (Test-Path $rateLimitEndDatePath) {
        try {
            $rateLimitData = Get-Content -Path $rateLimitEndDatePath -Raw | ConvertFrom-Json
            if ($rateLimitData.endDate) {
                $rateLimitEndUtc = [DateTime]::Parse($rateLimitData.endDate)
                $rateLimitEnd = [System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId($rateLimitEndUtc, 'Europe/Amsterdam')
                $currentTimeAmsterdam = [System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId((Get-Date), 'Europe/Amsterdam')
                if ($rateLimitEnd -gt $currentTimeAmsterdam) {
                    $waitSeconds = [int]($rateLimitEnd - $currentTimeAmsterdam).TotalSeconds
                    if ($waitSeconds -le 60) {
                        Write-Host "[RATE LIMIT] API rate limit in effect for $waitSeconds seconds. Waiting 60 seconds then continuing..."
                        Start-Sleep -Seconds 60
                        Write-Host "[RATE LIMIT] Wait complete. Removing rate-limit-enddate.json and continuing."
                        Remove-Item -Path $rateLimitEndDatePath -Force -ErrorAction SilentlyContinue
                        return $false
                    } else {
                        Write-Host "[RATE LIMIT] API rate limit in effect. Please wait $waitSeconds seconds (until $rateLimitEnd) before running again. Aborting."
                        return $true
                    }
                } else {
                    # End date has passed, remove the file
                    Write-Host "[RATE LIMIT] Rate limit has expired. Removing rate-limit-enddate.json."
                    Remove-Item -Path $rateLimitEndDatePath -Force -ErrorAction SilentlyContinue
                }
            } else {
                # No endDate property, remove the file
                Remove-Item -Path $rateLimitEndDatePath -Force -ErrorAction SilentlyContinue
            }
        } catch {
            Write-Host "[WARNING] Could not parse rate-limit-enddate.json. Removing file and continuing."
            Remove-Item -Path $rateLimitEndDatePath -Force -ErrorAction SilentlyContinue
        }
    }

    return $false
}
