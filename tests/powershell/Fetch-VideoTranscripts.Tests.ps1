Describe "Fetch-VideoTranscripts" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Fetch-VideoTranscripts.ps1"
        $vtt = @"
WEBVTT
Kind: captions
Language: en

NOTE this is a comment

00:00:01.000 --> 00:00:03.000 align:start position:0%
Hello <c>world</c> &amp; friends

00:00:03.000 --> 00:00:05.000
Hello <c>world</c> &amp; friends

00:00:05.000 --> 00:00:07.000
<i>Second</i> line &lt;ok&gt;
"@
    }

    BeforeEach {
        $state = [pscustomobject]@{
            Calls       = [System.Collections.Generic.List[hashtable]]::new()
            Urls        = @{ 'video-1' = 'https://www.youtube.com/watch?v=abc' }
            VttFiles    = @{ 'transcript.en.vtt' = $vtt }
            JobResponse = $null
            PostFails   = $false
            ExitCode    = 0
            HasExited   = $true
            Killed      = $false
            YtDlpArgs   = $null
            TempDir     = $null
            YtDlpMissing = $false
        }
        Mock yt-dlp { if ($state.YtDlpMissing) { throw 'not found' }; 'ytdlp-1.2.3' }
        Mock Start-Process {
            $state.YtDlpArgs = @($ArgumentList)
            $out = "$($ArgumentList[[array]::IndexOf($ArgumentList, '-o') + 1])"
            $state.TempDir = Split-Path $out
            foreach ($name in $state.VttFiles.Keys) { Set-Content -Path (Join-Path $state.TempDir $name) -Value $state.VttFiles[$name] }
            $p = [pscustomobject]@{ HasExited = $state.HasExited; ExitCode = $state.ExitCode }
            $p | Add-Member -MemberType ScriptMethod -Name WaitForExit -Value { param($ms) $true }
            $p | Add-Member -MemberType ScriptMethod -Name Kill -Value { $state.Killed = $true }
            $p
        }
        Mock Invoke-RestMethod {
            $state.Calls.Add(@{ Uri = $Uri.OriginalString; Method = [string]$Method; Headers = $Headers; Body = $Body })
            if ($Method -eq 'Post') {
                if ($state.PostFails) { throw 'HTTP 500' }
                return [pscustomobject]@{ title = 'Applied Title' }
            }
            if ($Uri.OriginalString -like '*processed-urls*') { return $state.JobResponse }
            $slug = [Uri]::UnescapeDataString(($Uri.OriginalString -split 'slug=')[-1])
            if ($state.Urls.ContainsKey($slug)) { return [pscustomobject]@{ externalUrl = $state.Urls[$slug] } }
            return [pscustomobject]@{ externalUrl = $null }
        }
    }

    Context "Parameters" {
        It "Uses separate parameter sets for Slug, Slugs and JobId" {
            $ast = [System.Management.Automation.Language.Parser]::ParseFile($scriptPath, [ref]$null, [ref]$null)
            $sets = foreach ($name in 'Slug', 'Slugs', 'JobId') {
                $p = Get-ScriptParameter $scriptPath $name
                ($p.Attributes | Where-Object { $_.TypeName.Name -eq 'Parameter' }).NamedArguments |
                    Where-Object { $_.ArgumentName -eq 'ParameterSetName' } | ForEach-Object { $_.Argument.Value }
            }
            $sets | Should -Be @('Slug', 'Slugs', 'Job')
        }

        It "Defaults to the local API without credentials and has a DryRun switch" {
            (Get-ScriptParameter $scriptPath 'ApiBaseUrl').DefaultValue.Value | Should -Be 'https://localhost:5001'
            (Get-ScriptParameter $scriptPath 'BearerToken').DefaultValue.Value | Should -Be ''
            (Get-ScriptParameter $scriptPath 'DryRun').StaticType.Name | Should -Be 'SwitchParameter'
        }

        It "Rejects combining Slug and JobId" {
            (Invoke-ScriptUnderTest $scriptPath @{ Slug = 'a'; JobId = 5 }).Error | Should -Not -BeNullOrEmpty
            $state.Calls | Should -HaveCount 0
        }
    }

    Context "Prerequisites and input" {
        It "Exits 1 when yt-dlp is not installed" {
            $state.YtDlpMissing = $true

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'yt-dlp is not installed'
            $state.Calls | Should -HaveCount 0
        }

        It "Exits 1 when no slug source is given" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Specify -Slug, -Slugs, or -JobId'
        }

        It "Exits 0 with a warning when the job contains no videos" {
            $state.JobResponse = [pscustomobject]@{ items = @(); totalCount = 0 }

            $result = Invoke-ScriptUnderTest $scriptPath @{ JobId = 7 }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'No slugs to process'
        }

        It "Processes a job that contains exactly one video" {
            $state.JobResponse = [pscustomobject]@{ items = @([pscustomobject]@{ slug = 'video-1' }); totalCount = 1 }

            $result = Invoke-ScriptUnderTest $scriptPath @{ JobId = 7 }

            $result.Output | Should -Match 'Found 1 video slug\(s\) in job 7'
            $result.Output | Should -Match 'Processing: video-1'
        }

        It "Accepts a Slugs value containing a single slug" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Slugs = 'video-1' }

            $result.Output | Should -Match 'Processing: video-1'
        }
    }

    Context "Slug resolution" {
        It "Splits, trims and de-blanks a comma-separated Slugs list" {
            $state.Urls = @{ 'a' = 'https://youtu.be/1'; 'b' = 'https://youtu.be/2'; 'c' = 'https://youtu.be/3' }

            Invoke-ScriptUnderTest $scriptPath @{ Slugs = ' a , b,, c ' } | Out-Null

            $lookups = @($state.Calls | Where-Object { $_.Method -eq 'Get' })
            $lookups | Should -HaveCount 3
            $lookups[0].Uri | Should -Match 'slug=a$'
            $lookups[1].Uri | Should -Match 'slug=b$'
            $lookups[2].Uri | Should -Match 'slug=c$'
        }

        It "Looks up all succeeded videos of a job (first page of 500) and processes each" {
            $state.Urls = @{ 'v1' = 'https://youtu.be/1'; 'v2' = 'https://youtu.be/2' }
            $state.JobResponse = [pscustomobject]@{ items = @([pscustomobject]@{ slug = 'v1' }, [pscustomobject]@{ slug = 'v2' }, [pscustomobject]@{ slug = $null }); totalCount = 2 }

            $result = Invoke-ScriptUnderTest $scriptPath @{ JobId = 42 }

            $state.Calls[0].Uri | Should -Be 'https://localhost:5001/api/admin/processed-urls?jobId=42&collectionName=videos&status=succeeded&pageSize=500'
            $result.Output | Should -Match 'Found 2 video slug\(s\) in job 42'
            @($state.Calls | Where-Object { $_.Uri -match 'edit-data' }) | Should -HaveCount 2
        }

        It "Warns when the job has more videos than one page returned" {
            $state.JobResponse = [pscustomobject]@{ items = @([pscustomobject]@{ slug = 'v1' }, [pscustomobject]@{ slug = 'v2' }); totalCount = 900 }

            $result = Invoke-ScriptUnderTest $scriptPath @{ JobId = 42 }

            $result.Output | Should -Match 'has 900 succeeded video\(s\) but only 2 were retrieved'
        }

        It "Escapes the slug in the edit-data URL and uses the ApiBaseUrl parameter" {
            $state.Urls = @{ 'a b&c' = 'https://youtu.be/1' }

            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'a b&c'; ApiBaseUrl = 'https://api.example.net' } | Out-Null

            $state.Calls[0].Uri | Should -Be 'https://api.example.net/api/admin/content-items/edit-data?collection=videos&slug=a%20b%26c'
        }
    }

    Context "Authentication" {
        It "Sends no Authorization header by default (local dev)" {
            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' } | Out-Null

            foreach ($call in $state.Calls) { $call.Headers.ContainsKey('Authorization') | Should -BeFalse }
        }

        It "Sends the bearer token on every call when provided" {
            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1'; BearerToken = 'tok-123' } | Out-Null

            $state.Calls.Count | Should -BeGreaterThan 0
            foreach ($call in $state.Calls) { $call.Headers['Authorization'] | Should -Be 'Bearer tok-123' }
        }
    }

    Context "Skipping" {
        It "Skips slugs without an external URL" {
            $state.Urls = @{}

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match "No YouTube URL found for slug 'video-1'"
            Should -Invoke Start-Process -Times 0 -Exactly
        }

        It "Skips non-YouTube URLs without running yt-dlp" {
            $state.Urls = @{ 'video-1' = 'https://vimeo.com/123' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.Output | Should -Match 'is not a YouTube URL'
            Should -Invoke Start-Process -Times 0 -Exactly
        }

        It "Accepts youtu.be short links" {
            $state.Urls = @{ 'video-1' = 'https://youtu.be/abc' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.Output | Should -Match 'Succeeded : 1'
        }

        It "Skips (without failing) when yt-dlp exits non-zero" {
            $state.ExitCode = 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'yt-dlp exited with code 1'
            @($state.Calls | Where-Object { $_.Method -eq 'Post' }) | Should -HaveCount 0
        }

        It "Skips when yt-dlp produced no subtitle file" {
            $state.VttFiles = @{}

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.Output | Should -Match 'No VTT subtitle file found'
            $result.Output | Should -Match 'Skipped   : 1'
        }

        It "Kills yt-dlp and skips when it does not finish in time" {
            $state.HasExited = $false

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $state.Killed | Should -BeTrue
            $result.Output | Should -Match 'yt-dlp timed out after 300s'
            @($state.Calls | Where-Object { $_.Method -eq 'Post' }) | Should -HaveCount 0
        }
    }

    Context "Transcript extraction" {
        It "Runs yt-dlp for subtitles only on a single video" {
            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' } | Out-Null

            $state.YtDlpArgs | Should -Contain '--skip-download'
            $state.YtDlpArgs | Should -Contain '--no-playlist'
            $state.YtDlpArgs | Should -Contain '--write-subs'
            $state.YtDlpArgs | Should -Contain '--write-auto-subs'
            $state.YtDlpArgs[-1] | Should -Be 'https://www.youtube.com/watch?v=abc'
        }

        It "Posts a transcript stripped of timestamps, tags, entities and repeated cues" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $post = @($state.Calls | Where-Object { $_.Method -eq 'Post' })
            $post | Should -HaveCount 1
            $post[0].Uri | Should -Be 'https://localhost:5001/api/admin/content-items/apply-transcript?collection=videos&slug=video-1'
            ($post[0].Body | ConvertFrom-Json).Transcript | Should -Be 'Hello world & friends Second line <ok>'
            $result.Output | Should -Match "Applied transcript for 'video-1' → title: Applied Title"
        }

        It "Prefers manual subtitles over auto-generated captions" {
            $state.VttFiles = @{
                'transcript.en-orig.vtt' = "WEBVTT`n`n00:00:01.000 --> 00:00:02.000`nauto caption`n"
                'transcript.en.vtt'      = "WEBVTT`n`n00:00:01.000 --> 00:00:02.000`nmanual caption`n"
            }

            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' } | Out-Null

            ($state.Calls | Where-Object { $_.Method -eq 'Post' }).Body | ConvertFrom-Json | ForEach-Object { $_.Transcript } | Should -Be 'manual caption'
        }

        It "Falls back to the auto-generated caption when no manual subtitle exists" {
            $state.VttFiles = @{ 'transcript.en-orig.vtt' = "WEBVTT`n`n00:00:01.000 --> 00:00:02.000`nauto caption`n" }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.Output | Should -Match 'No manual subtitles found; using auto-generated caption'
            ($state.Calls | Where-Object { $_.Method -eq 'Post' }).Body | ConvertFrom-Json | ForEach-Object { $_.Transcript } | Should -Be 'auto caption'
        }

        It "Skips a subtitle file that contains no cue text" {
            $state.VttFiles = @{ 'transcript.en.vtt' = "WEBVTT`n`n" }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.Output | Should -Match 'No transcript available'
            @($state.Calls | Where-Object { $_.Method -eq 'Post' }) | Should -HaveCount 0
        }

        It "Removes the temporary directory afterwards" {
            Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' } | Out-Null

            $state.TempDir | Should -Match 'techhub-transcript-'
            Test-Path $state.TempDir | Should -BeFalse
        }
    }

    Context "Applying transcripts" {
        It "DryRun fetches the transcript but never calls the apply-transcript API" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1'; DryRun = $true }

            $result.ExitCode | Should -Be 0
            @($state.Calls | Where-Object { $_.Method -eq 'Post' }) | Should -HaveCount 0
            $result.Output | Should -Match '\[DRY RUN\] Would call apply-transcript API'
            $result.Output | Should -Match 'Hello world & friends'
        }

        It "Counts API failures, keeps processing the other slugs and exits 1" {
            $state.Urls = @{ 'a' = 'https://youtu.be/1'; 'b' = 'https://youtu.be/2' }
            $state.PostFails = $true

            $result = Invoke-ScriptUnderTest $scriptPath @{ Slugs = 'a,b' }

            $result.ExitCode | Should -Be 1
            @($state.Calls | Where-Object { $_.Method -eq 'Post' }) | Should -HaveCount 2
            $result.Output | Should -Match 'Failed    : 2'
        }

        It "Exits 0 when everything succeeds" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Slug = 'video-1' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Succeeded : 1'
            $result.Output | Should -Match 'Failed    : 0'
        }
    }
}
