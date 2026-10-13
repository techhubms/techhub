Describe "Wait-ForLiveVersion" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Wait-ForLiveVersion.ps1"

        # Responses are produced by $http.Handler(<uri>); it may return a response object or throw.
        function New-HttpResponse {
            param([int]$StatusCode = 200, [string]$Content = '')
            [pscustomobject]@{ StatusCode = $StatusCode; Content = $Content }
        }
    }

    BeforeEach {
        $http = [pscustomobject]@{
            Uris    = [System.Collections.Generic.List[string]]::new()
            Handler = { param($uri) throw "unexpected request: $uri" }
        }
        $sleeps = [System.Collections.Generic.List[int]]::new()

        Mock Invoke-WebRequest {
            $http.Uris.Add([string]$Uri)
            & $http.Handler ([string]$Uri)
        }
        Mock Start-Sleep { $sleeps.Add([int]$Seconds) }
    }

    Context "Parameters" {
        It "Requires WebFqdn and Tag" {
            (Test-ParameterMandatory (Get-ScriptParameter $scriptPath 'WebFqdn')) | Should -BeTrue
            (Test-ParameterMandatory (Get-ScriptParameter $scriptPath 'Tag')) | Should -BeTrue
        }

        It "Defaults MaxWaitSeconds to 300 and makes SkipSmokeTests a switch" {
            (Get-ScriptParameter $scriptPath 'MaxWaitSeconds').DefaultValue.Value | Should -Be 300
            (Get-ScriptParameter $scriptPath 'SkipSmokeTests').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Version polling" {
        It "Succeeds immediately when /version already reports the expected tag" {
            $http.Handler = {
                param($uri)
                if ($uri -like '*/version*') { New-HttpResponse -Content '{"tag":"20260509120000"}' }
                else { New-HttpResponse -Content 'ok' }
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = '20260509120000' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Version 20260509120000 is live after 5s'
            @($http.Uris | Where-Object { $_ -like '*/version*' }).Count | Should -Be 1
            $http.Uris[0] | Should -Be 'https://web.example.net/version?t=20260509120000'
        }

        It "Keeps polling while the old version is served, then succeeds" {
            $http.Handler = {
                param($uri)
                $tag = if ($http.Uris.Count -lt 3) { 'old-tag' } else { 'new-tag' }
                New-HttpResponse -Content "{`"tag`":`"$tag`"}"
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 'new-tag'; SkipSmokeTests = $true }

            $result.ExitCode | Should -Be 0
            $http.Uris.Count | Should -Be 3
            $sleeps.Count | Should -Be 2
            $result.Output | Should -Match "Still on 'old-tag'"
        }

        It "Treats connection errors as not-ready and keeps retrying" {
            $http.Handler = {
                param($uri)
                if ($http.Uris.Count -lt 3) { throw "connection refused" }
                New-HttpResponse -Content '{"tag":"t1"}'
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1'; SkipSmokeTests = $true }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Not yet responding'
        }

        It "Fails with exit code 1 once MaxWaitSeconds is exhausted (one attempt per 5 seconds)" {
            $http.Handler = { param($uri) New-HttpResponse -Content '{"tag":"old-tag"}' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 'new-tag'; MaxWaitSeconds = 20 }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'did not become live within 20s'
            $http.Uris.Count | Should -Be 4
            $sleeps | ForEach-Object { $_ | Should -Be 5 }
        }

        It "Rounds the attempt count up when MaxWaitSeconds is not a multiple of 5" {
            $http.Handler = { param($uri) throw "down" }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 'x'; MaxWaitSeconds = 12 }

            $result.ExitCode | Should -Be 1
            $http.Uris.Count | Should -Be 3
        }

        It "Does not treat a non-200 /version response as live" {
            $http.Handler = { param($uri) New-HttpResponse -StatusCode 204 -Content '{"tag":"t1"}' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1'; MaxWaitSeconds = 10 }

            $result.ExitCode | Should -Be 1
        }

        It "Sends a no-cache header and a cache-busting query string" {
            $http.Handler = { param($uri) New-HttpResponse -Content '{"tag":"t1"}' }

            Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1'; SkipSmokeTests = $true } | Out-Null

            Should -Invoke Invoke-WebRequest -Times 1 -Exactly -ParameterFilter {
                $Headers['Cache-Control'] -eq 'no-cache' -and $Uri -match '\?t=t1$'
            }
        }
    }

    Context "Smoke tests" {
        It "Skips health and homepage checks when SkipSmokeTests is set" {
            $http.Handler = { param($uri) New-HttpResponse -Content '{"tag":"t1"}' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1'; SkipSmokeTests = $true }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Smoke tests skipped'
            $http.Uris | Should -Not -Contain 'https://web.example.net/health'
        }

        It "Passes when /health and / both return 200" {
            $http.Handler = {
                param($uri)
                if ($uri -like '*/version*') { New-HttpResponse -Content '{"tag":"t1"}' } else { New-HttpResponse }
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1' }

            $result.ExitCode | Should -Be 0
            $http.Uris | Should -Contain 'https://web.example.net/health'
            $http.Uris | Should -Contain 'https://web.example.net/'
            $result.Output | Should -Match 'All smoke tests passed'
        }

        It "Fails when /health is unhealthy even if the homepage works" {
            $http.Handler = {
                param($uri)
                if ($uri -like '*/version*') { New-HttpResponse -Content '{"tag":"t1"}' }
                elseif ($uri -like '*/health') { throw "503" }
                else { New-HttpResponse }
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Health check failed'
            $result.Output | Should -Not -Match 'Homepage not accessible'
        }

        It "Fails when the homepage returns a non-200 status" {
            $http.Handler = {
                param($uri)
                if ($uri -like '*/version*') { New-HttpResponse -Content '{"tag":"t1"}' }
                elseif ($uri -like '*/health') { New-HttpResponse }
                else { New-HttpResponse -StatusCode 500 }
            }

            $result = Invoke-ScriptUnderTest $scriptPath @{ WebFqdn = 'web.example.net'; Tag = 't1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Homepage not accessible'
        }
    }
}
