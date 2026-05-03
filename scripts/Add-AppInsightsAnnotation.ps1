param(
    [Parameter(Mandatory = $true)]
    [string]$SubscriptionId,

    [Parameter(Mandatory = $true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]$AppInsightsName,

    [Parameter(Mandatory = $true)]
    [string]$AnnotationId,

    [Parameter(Mandatory = $true)]
    [string]$ReleaseName,

    [Parameter(Mandatory = $true)]
    [string]$Commit,

    [Parameter(Mandatory = $true)]
    [string]$DeployedBy,

    [Parameter(Mandatory = $true)]
    [string]$RunUrl,

    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

try {
    Write-Host "Creating Application Insights release annotation..."
    Write-Host "  App Insights: $AppInsightsName"
    Write-Host "  Release:      $ReleaseName"
    Write-Host "  Commit:       $Commit"

    # The 2015-05-01 API requires Properties to be a JSON-encoded string, not a nested object.
    # It also requires a non-null Category field.
    $propertiesObject = [ordered]@{
        ReleaseName = $ReleaseName
        Commit      = $Commit
        DeployedBy  = $DeployedBy
        RunUrl      = $RunUrl
    }
    $propertiesJson = $propertiesObject | ConvertTo-Json -Compress

    $eventTime = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.0000000Z")

    $annotationBody = [ordered]@{
        Id             = $AnnotationId
        AnnotationName = "Release $ReleaseName"
        EventTime      = $eventTime
        Category       = "Deployment"
        Properties     = $propertiesJson
    }
    $annotationBodyJson = $annotationBody | ConvertTo-Json -Compress

    $url = "https://management.azure.com/subscriptions/$SubscriptionId/resourceGroups/$ResourceGroupName/providers/Microsoft.Insights/components/$AppInsightsName/Annotations/$($AnnotationId)?api-version=2015-05-01"

    az rest --method put `
        --headers "Content-Type=application/json" `
        --url $url `
        --body $annotationBodyJson

    if ($LASTEXITCODE -ne 0) {
        throw "az rest failed with exit code $LASTEXITCODE"
    }

    Write-Host "Release annotation created successfully."
}
catch {
    Write-Error "Failed to create Application Insights release annotation: $_"
    exit 1
}
