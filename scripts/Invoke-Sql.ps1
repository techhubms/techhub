param(
    [Parameter(Mandatory = $true)]
    [string]$Query
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$env:PGPASSWORD = 'localdev'
$env:PAGER = ''

psql -h localhost -p 5432 -d techhub -U techhub --no-psqlrc -A -F "|" -c $Query
