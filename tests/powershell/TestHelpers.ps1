# Shared helpers for behavioural Pester tests. Dot-source inside BeforeAll: . $PSScriptRoot/TestHelpers.ps1
# Provides: external command stubs (az, gh, ...), a recording CLI fake, a script sandbox and a script runner.

# Stubs so Pester `Mock` can replace commands that may not exist on the machine running the tests.
function az { }
function gh { }
function docker { }
function certbot { }
function openssl { }
function chmod { }
function git { }
function yt-dlp { }
function dotnet { }
function psql { }
function exiftool { }
function webpmux { }
function sudo { }
function curl { }
function file { }

# Az PowerShell stubs (only the parameters the deployment scripts use) so the Az module is not required.
function Get-AzContext { [CmdletBinding()] param() }
function Test-AzDeployment { [CmdletBinding()] param($Location, $TemplateFile, $TemplateParameterFile, [switch]$SkipTemplateParameterPrompt) }
function New-AzDeployment { [CmdletBinding(SupportsShouldProcess)] param($Name, $Location, $TemplateFile, $TemplateParameterFile, [switch]$SkipTemplateParameterPrompt) }
function Get-AzDeployment { [CmdletBinding()] param($Name) }
function Get-AzDeploymentOperation { [CmdletBinding()] param($DeploymentName) }
function Get-AzResourceGroupDeploymentOperation { [CmdletBinding()] param($ResourceGroupName, $DeploymentName) }
function Get-AzResource { [CmdletBinding()] param($ResourceGroupName, $ResourceType, $Name, $ResourceName) }
function Remove-AzResource { [CmdletBinding()] param($ResourceId, [switch]$Force) }
function New-AzRoleAssignment { [CmdletBinding()] param($ObjectId, $ObjectType, $RoleDefinitionId, $Scope) }
function New-AzResourceGroupDeployment { [CmdletBinding()] param($ResourceGroupName, $TemplateFile, $location, $appServicePlanId, $certResourceName, $keyVaultResourceId, $keyVaultSecretName) }
function Get-AzPostgreSqlFlexibleServerFirewallRule { [CmdletBinding()] param($ResourceGroupName, $ServerName, $Name) }
function Remove-AzPostgreSqlFlexibleServerFirewallRule { [CmdletBinding()] param($ResourceGroupName, $ServerName, $Name) }
function Get-AzNatGateway { [CmdletBinding()] param($ResourceGroupName, $Name) }
function Remove-AzNatGateway { [CmdletBinding()] param($ResourceGroupName, $Name, [switch]$Force) }
function Get-AzPublicIpAddress { [CmdletBinding()] param($ResourceGroupName, $Name) }
function Remove-AzPublicIpAddress { [CmdletBinding()] param($ResourceGroupName, $Name, [switch]$Force) }

function New-CliFake {
    # State object shared (by reference) between a test and the mocks it registers.
    [pscustomobject]@{
        Calls = [System.Collections.Generic.List[string]]::new()
        Rules = [System.Collections.Generic.List[hashtable]]::new()
    }
}

function Add-CliRule {
    # Match is a regex on "<command> <args joined by space>". First matching rule wins;
    # -First inserts the rule ahead of earlier ones so a test can override default rules.
    param(
        [Parameter(Mandatory)] $Fake,
        [Parameter(Mandatory)] [string]$Command,
        [Parameter(Mandatory)] [string]$Match,
        $Output = $null,
        [int]$ExitCode = 0,
        [scriptblock]$Action = $null,
        [switch]$First
    )
    $rule = @{ Command = $Command; Match = $Match; Output = $Output; ExitCode = $ExitCode; Action = $Action }
    if ($First) { $Fake.Rules.Insert(0, $rule) } else { $Fake.Rules.Add($rule) }
}

function Invoke-CliFake {
    param(
        [Parameter(Mandatory)] $Fake,
        [Parameter(Mandatory)] [string]$Command,
        [object[]]$Arguments
    )
    $line = ("$Command " + (@($Arguments | ForEach-Object { "$_" }) -join ' ')).Trim()
    $Fake.Calls.Add($line)
    foreach ($rule in $Fake.Rules) {
        if ($rule.Command -eq $Command -and $line -match $rule.Match) {
            $global:LASTEXITCODE = $rule.ExitCode
            if ($rule.Action) { & $rule.Action $Arguments | Out-Null }
            return $rule.Output
        }
    }
    $global:LASTEXITCODE = 0
    return $null
}

function Get-CliCalls {
    # Calls whose text matches the regex, in call order.
    param([Parameter(Mandatory)] $Fake, [Parameter(Mandatory)] [string]$Match)
    @($Fake.Calls | Where-Object { $_ -match $Match })
}

function New-ScriptSandbox {
    # Copies scripts from scripts/ into <tmp>/scripts and adds stub scripts/files so a script that
    # calls sibling scripts (via $PSScriptRoot) can run without touching the real repository.
    param(
        [string[]]$Scripts = @(),
        [hashtable]$StubScripts = @{},
        [hashtable]$Files = @{}
    )
    $realScripts = (Resolve-Path (Join-Path $PSScriptRoot '../../scripts')).Path
    $root = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-pester-$([guid]::NewGuid().ToString('N'))"
    New-Item -ItemType Directory -Force -Path (Join-Path $root 'scripts') | Out-Null
    foreach ($name in $Scripts) {
        Copy-Item -Path (Join-Path $realScripts $name) -Destination (Join-Path $root "scripts/$name")
    }
    foreach ($name in $StubScripts.Keys) {
        Set-Content -Path (Join-Path $root "scripts/$name") -Value $StubScripts[$name]
    }
    foreach ($relative in $Files.Keys) {
        $target = Join-Path $root $relative
        New-Item -ItemType Directory -Force -Path (Split-Path $target) | Out-Null
        Set-Content -Path $target -Value $Files[$relative]
    }
    $root
}

function New-RecordingStubScript {
    # Stub script that appends "<name> <args>" to stub-calls.log next to itself and exits with ExitCode.
    param([int]$ExitCode = 0)
    @"
`$log = Join-Path `$PSScriptRoot 'stub-calls.log'
Add-Content -Path `$log -Value ("{0} {1}" -f `$MyInvocation.MyCommand.Name, (`$args -join ' '))
exit $ExitCode
"@
}

function Get-StubScriptCalls {
    param([Parameter(Mandatory)] [string]$SandboxRoot)
    $log = Join-Path $SandboxRoot 'scripts/stub-calls.log'
    if (Test-Path $log) { @(Get-Content $log) } else { @() }
}

function Remove-ScriptSandbox {
    param([string]$SandboxRoot)
    if ($SandboxRoot -and (Test-Path $SandboxRoot)) {
        Remove-Item -Path $SandboxRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}

function Invoke-ScriptUnderTest {
    # Runs a script in a child scope and captures exit code, all output streams and any terminating error.
    # `exit N` inside a script invoked with & ends only that script and sets $LASTEXITCODE.
    param(
        [Parameter(Mandatory)] [string]$Path,
        [hashtable]$Parameters = @{}
    )
    $global:LASTEXITCODE = 0
    $failure = $null
    $lines = [System.Collections.Generic.List[object]]::new()
    try {
        # Scripts that do not set StrictMode themselves must not inherit the StrictMode used by the tests.
        & { Set-StrictMode -Off; & $Path @Parameters } *>&1 | ForEach-Object { $lines.Add($_) }
    }
    catch {
        $failure = $_
    }
    [pscustomobject]@{
        ExitCode = $global:LASTEXITCODE
        Output   = ($lines | Out-String)
        Error    = $failure
    }
}

function Save-EnvironmentVariable {
    # Snapshot of environment variables ($null = unset) to hand to Restore-EnvironmentVariable.
    param([Parameter(Mandatory)] [string[]]$Name)
    $saved = @{}
    foreach ($n in $Name) { $saved[$n] = [Environment]::GetEnvironmentVariable($n) }
    $saved
}

function Restore-EnvironmentVariable {
    param([Parameter(Mandatory)] [hashtable]$Saved)
    foreach ($n in $Saved.Keys) { [Environment]::SetEnvironmentVariable($n, $Saved[$n]) }
}

function Get-ScriptParameter {
    # Returns the ParameterAst for a script's param block entry.
    param([Parameter(Mandatory)] [string]$Path, [Parameter(Mandatory)] [string]$Name)
    $ast = [System.Management.Automation.Language.Parser]::ParseFile($Path, [ref]$null, [ref]$null)
    $ast.ParamBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq $Name }
}

function Test-ParameterMandatory {
    param([Parameter(Mandatory)] $Parameter)
    $found = $Parameter.Attributes | Where-Object {
        $_.TypeName.Name -eq 'Parameter' -and
        ($_.NamedArguments | Where-Object { $_.ArgumentName -eq 'Mandatory' -and $_.Argument.Extent.Text -eq '$true' })
    }
    [bool]$found
}

function Get-ParameterValidateSet {
    param([Parameter(Mandatory)] $Parameter)
    $set = $Parameter.Attributes | Where-Object { $_.TypeName.Name -eq 'ValidateSet' }
    if ($set) { @($set.PositionalArguments | ForEach-Object { $_.Value }) } else { @() }
}
