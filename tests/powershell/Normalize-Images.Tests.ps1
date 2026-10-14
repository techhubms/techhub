Describe "Normalize-Images" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Normalize-Images.ps1"
        $realGetCommand = Get-Command -Name Get-Command -CommandType Cmdlet
        # The script caches the sRGB profile at a fixed path; make sure it exists so nothing is downloaded.
        $iccPath = '/tmp/sRGB_v4_ICC.icc'
        $createdIcc = -not (Test-Path $iccPath)
        if ($createdIcc) { Set-Content -Path $iccPath -Value 'icc' }
    }

    AfterAll {
        if ($createdIcc) { Remove-Item $iccPath -Force -ErrorAction SilentlyContinue }
    }

    BeforeEach {
        $cli = New-CliFake
        $state = [pscustomobject]@{ Missing = @(); RepoRoot = $null }
        $images = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-images-$([guid]::NewGuid().ToString('N'))"
        New-Item -ItemType Directory -Force -Path (Join-Path $images 'section-backgrounds') | Out-Null
        foreach ($name in 'a.jpg', 'b.jpeg', 'c.png', 'd.webp', 'e.jxl', 'f.gif', 'notes.txt', 'section-backgrounds/ai-bg.jpg') {
            Set-Content -Path (Join-Path $images $name) -Value "content-$name"
        }
        $before = Get-ChildItem $images -Recurse -File | ForEach-Object { "$($_.FullName)=$((Get-FileHash $_.FullName).Hash)" }

        Mock Get-Command {
            if ($Name -in $state.Missing) { return $null }
            & $realGetCommand @PesterBoundParameters
        }
        Mock git { $state.RepoRoot }
        Mock exiftool { Invoke-CliFake -Fake $cli -Command 'exiftool' -Arguments $args }
        Mock sudo { Invoke-CliFake -Fake $cli -Command 'sudo' -Arguments $args }
        Mock curl { Invoke-CliFake -Fake $cli -Command 'curl' -Arguments $args }
        # webpmux writes the -o target like the real tool so the script can move it over the image.
        Mock webpmux {
            Invoke-CliFake -Fake $cli -Command 'webpmux' -Arguments $args
            $out = $args[[array]::IndexOf(@($args | ForEach-Object { "$_" }), '-o') + 1]
            Set-Content -Path $out -Value 'webp-with-icc'
        }
    }

    AfterEach {
        Remove-Item $images -Recurse -Force -ErrorAction SilentlyContinue
    }

    Context "Parameters" {
        It "Has optional ImagePath, Artist and Copyright and a DryRun switch" {
            foreach ($name in 'ImagePath', 'Artist', 'Copyright', 'DryRun') {
                Test-ParameterMandatory (Get-ScriptParameter $scriptPath $name) | Should -BeFalse
            }
            (Get-ScriptParameter $scriptPath 'Artist').DefaultValue.Value | Should -Be 'Reinier van Maanen'
            (Get-ScriptParameter $scriptPath 'DryRun').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Input validation" {
        It "Fails when the image path does not exist" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = '/nonexistent/techhub-images'; DryRun = $true }

            # Write-Error with ErrorActionPreference=Stop surfaces as an exception or, depending on the host, as error output + exit 1
            $message = if ($result.Error) { $result.Error.Exception.Message } else { $result.Output }
            ($result.Error -or $result.ExitCode -eq 1) | Should -BeTrue
            $message | Should -Match 'Image path not found: /nonexistent/techhub-images'
            @(Get-CliCalls $cli '^exiftool') | Should -BeNullOrEmpty
        }

        It "Defaults to wwwroot/images below the git repository root" {
            $state.RepoRoot = '/nonexistent/repo'

            $result = Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true }

            $message = if ($result.Error) { $result.Error.Exception.Message } else { $result.Output }
            $message | Should -Match 'Image path not found: /nonexistent/repo/src/TechHub.Web/wwwroot/images'
        }

        It "Exits 0 without touching anything when the directory has no supported images" {
            $empty = Join-Path $images 'empty'
            New-Item -ItemType Directory -Path $empty | Out-Null
            Set-Content (Join-Path $empty 'x.gif') 'gif'

            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $empty }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'No images found'
            @(Get-CliCalls $cli '^(exiftool|webpmux)') | Should -BeNullOrEmpty
        }
    }

    Context "DryRun" {
        It "Lists all supported images but leaves files and external tools untouched" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images; DryRun = $true }

            $result.Error | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Found 6 images'
            $result.Output | Should -Match 'Total processed: 6'
            $result.Output | Should -Match 'Succeeded:\s+6'
            $result.Output | Should -Match 'DRY RUN - No changes were made'
            @(Get-CliCalls $cli '^(exiftool|webpmux|sudo|curl)') | Should -BeNullOrEmpty
            $after = Get-ChildItem $images -Recurse -File | ForEach-Object { "$($_.FullName)=$((Get-FileHash $_.FullName).Hash)" }
            $after | Should -Be $before
        }

        It "Reports missing packages without installing them" {
            $state.Missing = @('exiftool', 'webpmux')

            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images; DryRun = $true }

            $result.Output | Should -Match 'Would run: sudo apt-get install -y libimage-exiftool-perl webp'
            @(Get-CliCalls $cli '^sudo') | Should -BeNullOrEmpty
        }
    }

    Context "Normalization" {
        It "Installs missing packages with apt-get" {
            $state.Missing = @('exiftool')

            Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images } | Out-Null

            @(Get-CliCalls $cli '^sudo apt-get install -y -qq libimage-exiftool-perl') | Should -HaveCount 1
        }

        It "Does not install anything when all tools are present" {
            Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images } | Out-Null

            @(Get-CliCalls $cli '^sudo') | Should -BeNullOrEmpty
        }

        It "Strips metadata, embeds the sRGB profile and sets the configured Artist and Copyright on JPEGs" {
            $jpg = Join-Path $images 'a.jpg'
            $only = Join-Path $images 'only'
            New-Item -ItemType Directory -Path $only | Out-Null
            Copy-Item $jpg $only

            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $only; Artist = 'Jane Doe'; Copyright = '(c) Test' }

            $result.Error | Should -BeNullOrEmpty
            $calls = @(Get-CliCalls $cli '^exiftool')
            $calls | Should -HaveCount 3
            $calls[0] | Should -Match '-overwrite_original -all= '
            $calls[1] | Should -Match '-ICC_Profile<=/tmp/sRGB_v4_ICC.icc '
            $calls[2] | Should -Match '-Artist=Jane Doe -Copyright=\(c\) Test -Software=TechHub Image Pipeline -ImageDescription=TechHub image: a -ColorSpace=sRGB -XResolution=96'
        }

        It "Defaults the copyright to the current year" {
            $only = Join-Path $images 'only'
            New-Item -ItemType Directory -Path $only | Out-Null
            Copy-Item (Join-Path $images 'c.png') $only

            Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $only } | Out-Null

            @(Get-CliCalls $cli '^exiftool')[-1] | Should -Match "-Copyright=© $((Get-Date).Year) TechHub. All rights reserved."
        }

        It "Derives the description from the sub-folder and file name" {
            Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images } | Out-Null

            @(Get-CliCalls $cli '^exiftool') -join "`n" | Should -Match 'TechHub section backgrounds: ai bg'
        }

        It "Writes PNG descriptions through -Description (not -ImageDescription)" {
            $only = Join-Path $images 'only'
            New-Item -ItemType Directory -Path $only | Out-Null
            Copy-Item (Join-Path $images 'c.png') $only

            Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $only } | Out-Null

            $last = @(Get-CliCalls $cli '^exiftool')[-1]
            $last | Should -Match '-Description=TechHub image: c'
            $last | Should -Not -Match '-ImageDescription'
        }

        It "Embeds the ICC profile into WebP files with webpmux" {
            $only = Join-Path $images 'only'
            New-Item -ItemType Directory -Path $only | Out-Null
            Copy-Item (Join-Path $images 'd.webp') $only

            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $only }

            $result.Error | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^webpmux -set icc /tmp/sRGB_v4_ICC.icc ') | Should -HaveCount 2
            Get-Content (Join-Path $only 'd.webp') | Should -Be 'webp-with-icc'
        }

        It "Counts a WebP file as failed when webpmux produces no output" {
            Mock webpmux { Invoke-CliFake -Fake $cli -Command 'webpmux' -Arguments $args }
            $only = Join-Path $images 'only'
            New-Item -ItemType Directory -Path $only | Out-Null
            Copy-Item (Join-Path $images 'd.webp') $only
            Remove-Item "/tmp/d.webp" -Force -ErrorAction SilentlyContinue

            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $only }

            $result.Output | Should -Match 'Failed:\s+1'
            $result.Output | Should -Match 'webpmux failed for d.webp'
        }

        It "Processes every supported extension (jpg, jpeg, png, webp, jxl) and ignores other files" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ ImagePath = $images }

            $result.Output | Should -Match 'Total processed: 6'
            $result.Output | Should -Match 'Succeeded:\s+6'
            @(Get-CliCalls $cli '^exiftool') -join "`n" | Should -Not -Match 'f\.gif|notes\.txt'
        }
    }
}
