Describe "Build-Images" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Build-Images.ps1"
        $envNames = @('GITHUB_ACTIONS', 'GITHUB_TOKEN', 'GHCR_PAT', 'GITHUB_ACTOR', 'GITHUB_OUTPUT')
    }

    BeforeEach {
        $savedEnv = Save-EnvironmentVariable $envNames
        foreach ($n in $envNames) { Remove-Item -Path "Env:$n" -ErrorAction SilentlyContinue }
        $env:GHCR_PAT = 'pat-value-123'
        $cli = New-CliFake
        $piped = [System.Collections.Generic.List[string]]::new()
        Mock Start-Sleep { }
        # A real function (not a Mock) so stdin piped to `docker login` can be captured via $input.
        function docker { foreach ($i in $input) { $piped.Add([string]$i) }; Invoke-CliFake -Fake $cli -Command 'docker' -Arguments $args }
    }

    AfterEach {
        Restore-EnvironmentVariable $savedEnv
    }

    Context "Parameters" {
        It "Has no mandatory parameters and defaults the registry owner to techhubms" {
            foreach ($name in 'Tag', 'GithubRegistryUsername', 'GithubRegistryAuthUsername', 'SkipPush') {
                Test-ParameterMandatory (Get-ScriptParameter $scriptPath $name) | Should -BeFalse
            }
            (Get-ScriptParameter $scriptPath 'GithubRegistryUsername').DefaultValue.Value | Should -Be 'techhubms'
            (Get-ScriptParameter $scriptPath 'SkipPush').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Tagging" {
        It "Defaults the tag to a yyyyMMddHHmmss timestamp" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ SkipPush = $true }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $result.Output -match 'Tag\s+: (\d{14})' | Should -BeTrue
            @(Get-CliCalls $cli '^docker build')[0] | Should -Match "-t ghcr\.io/techhubms/techhub-api:$($Matches[1])"
        }

        It "Tags both images with the requested tag under the registry owner" {
            Invoke-ScriptUnderTest $scriptPath @{ SkipPush = $true; Tag = 't-1'; GithubRegistryUsername = 'someone' } | Out-Null

            $builds = @(Get-CliCalls $cli '^docker build')
            $builds | Should -HaveCount 2
            $builds[0] | Should -Be 'docker build -f src/TechHub.Api/Dockerfile -t ghcr.io/someone/techhub-api:t-1 .'
            $builds[1] | Should -Be 'docker build -f src/TechHub.Web/Dockerfile -t ghcr.io/someone/techhub-web:t-1 .'
        }
    }

    Context "SkipPush" {
        It "Builds but neither logs in nor pushes, and needs no credentials" {
            $env:GHCR_PAT = $null

            $result = Invoke-ScriptUnderTest $scriptPath @{ SkipPush = $true; Tag = 't-1' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^docker build') | Should -HaveCount 2
            @(Get-CliCalls $cli '^docker (login|push)') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Skipping push'
        }
    }

    Context "Authentication" {
        It "Fails before building when neither GITHUB_TOKEN nor GHCR_PAT is set" {
            $env:GHCR_PAT = $null

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Neither GITHUB_TOKEN nor GHCR_PAT is set'
            @(Get-CliCalls $cli '^docker') | Should -BeNullOrEmpty
        }

        It "Fails when no login username can be determined locally" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Cannot determine ghcr.io login username'
            @(Get-CliCalls $cli '^docker') | Should -BeNullOrEmpty
        }

        It "Logs in with the PAT on stdin (never as an argument) and the auth username parameter" {
            Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' } | Out-Null

            $login = @(Get-CliCalls $cli '^docker login')
            $login | Should -HaveCount 1
            $login[0] | Should -Be 'docker login ghcr.io --username me --password-stdin'
            $piped | Should -Contain 'pat-value-123'
            $login[0] | Should -Not -Match 'pat-value-123'
        }

        It "Prefers GITHUB_TOKEN over GHCR_PAT and GITHUB_ACTOR over the auth username parameter" {
            $env:GITHUB_TOKEN = 'ci-token'
            $env:GITHUB_ACTOR = 'ci-actor'

            Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' } | Out-Null

            $piped | Should -Contain 'ci-token'
            @(Get-CliCalls $cli '^docker login')[0] | Should -Match '--username ci-actor'
        }

        It "Exits 1 without building when the login fails" {
            Add-CliRule $cli 'docker' '^docker login' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to authenticate'
            @(Get-CliCalls $cli '^docker build') | Should -BeNullOrEmpty
        }
    }

    Context "Build and push" {
        It "Builds both images, then pushes API before Web" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' }

            $result.ExitCode | Should -Be 0
            $pushes = @(Get-CliCalls $cli '^docker push')
            $pushes | Should -Be @('docker push ghcr.io/techhubms/techhub-api:t-1', 'docker push ghcr.io/techhubms/techhub-web:t-1')
            $lastBuild = $cli.Calls.FindLastIndex({ param($c) $c -match '^docker build' })
            $firstPush = $cli.Calls.FindIndex({ param($c) $c -match '^docker push' })
            $lastBuild | Should -BeLessThan $firstPush
        }

        It "Exits 1 without building Web when the API build fails" {
            Add-CliRule $cli 'docker' '^docker build .*TechHub.Api' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; SkipPush = $true }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to build API image'
            @(Get-CliCalls $cli '^docker build') | Should -HaveCount 1
        }

        It "Exits 1 when the Web build fails" {
            Add-CliRule $cli 'docker' '^docker build .*TechHub.Web' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; SkipPush = $true }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to build Web image'
        }

        It "Retries a failing push up to 3 times and succeeds on the last attempt" {
            $pushAttempts = [pscustomobject]@{ Count = 0 }
            Add-CliRule $cli 'docker' '^docker push .*techhub-api' '' -Action { $pushAttempts.Count++; $global:LASTEXITCODE = $(if ($pushAttempts.Count -lt 3) { 1 } else { 0 }) }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' }

            @(Get-CliCalls $cli '^docker push .*techhub-api') | Should -HaveCount 3
            Should -Invoke Start-Sleep -Times 2 -Exactly -ParameterFilter { $Seconds -eq 15 }
            $result.Output | Should -Match 'Image Build Complete'
        }

        It "Exits 1 after 3 failed push attempts and does not push Web" {
            Add-CliRule $cli 'docker' '^docker push .*techhub-api' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Tag = 't-1'; GithubRegistryAuthUsername = 'me' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^docker push .*techhub-api') | Should -HaveCount 3
            @(Get-CliCalls $cli '^docker push .*techhub-web') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Failed to push API image after 3 attempts'
        }
    }

    Context "CI output" {
        It "Writes image-tag to GITHUB_OUTPUT when running in GitHub Actions" {
            $outputFile = Join-Path ([System.IO.Path]::GetTempPath()) "gh-output-$([guid]::NewGuid().ToString('N')).txt"
            try {
                $env:GITHUB_ACTIONS = 'true'
                $env:GITHUB_OUTPUT = $outputFile

                Invoke-ScriptUnderTest $scriptPath @{ Tag = 'ci-tag'; SkipPush = $true } | Out-Null

                (Get-Content $outputFile) | Should -Contain 'image-tag=ci-tag'
            }
            finally {
                Remove-Item $outputFile -Force -ErrorAction SilentlyContinue
            }
        }
    }
}
