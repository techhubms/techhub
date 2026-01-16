# Azure Infrastructure Architecture

## Overview

Defines Azure infrastructure for Tech Hub .NET deployment using Azure Container Apps, Bicep Infrastructure as Code, and Azure Application Insights monitoring.

## Constitution Alignment

- **Modern UX First**: Global CDN, auto-scaling, zero-downtime deployments
- **Configuration-Driven**: Environment-specific configurations via Azure App Configuration
- **Performance Excellence**: Auto-scaling, regional deployment, caching layers

## Target Architecture

**Hosting**: Azure Container Apps (serverless containers)  
**Infrastructure as Code**: Bicep  
**Monitoring**: Application Insights + OpenTelemetry  
**CDN**: Azure Front Door (planned)  
**Region**: West Europe (primary)

---

## Resource Overview

### Core Resources

| Resource | Type | Purpose |
|----------|------|---------|
| Container Apps Environment | `Microsoft.App/managedEnvironments` | Hosting environment for containers |
| TechHub API Container App | `Microsoft.App/containerApps` | REST API backend |
| TechHub Web Container App | `Microsoft.App/containerApps` | Blazor frontend |
| Application Insights | `Microsoft.Insights/components` | Monitoring and telemetry |
| Log Analytics Workspace | `Microsoft.OperationalInsights/workspaces` | Centralized logging |
| Container Registry | `Microsoft.ContainerRegistry/registries` | Docker image storage |

### Optional Resources (Future)

| Resource | Type | Purpose |
|----------|------|---------|
| Azure Front Door | `Microsoft.Network/frontDoors` | Global CDN and load balancing |
| Azure Cache for Redis | `Microsoft.Cache/redis` | Distributed caching |
| Virtual Network | `Microsoft.Network/virtualNetworks` | Network isolation |
| Storage Account | `Microsoft.Storage/storageAccounts` | Static content storage |

---

## Bicep Structure

**Location**: `/infra/`

**Organization**:

```text
infra/
├── main.bicep                    # Main orchestration
├── modules/
│   ├── containerApps.bicep       # Container Apps Environment
│   ├── api.bicep                 # TechHub.Api container
│   ├── web.bicep                 # TechHub.Web container
│   ├── monitoring.bicep          # Application Insights + Log Analytics
│   ├── registry.bicep            # Container Registry
│   └── networking.bicep          # VNet (optional)
├── parameters/
│   ├── dev.bicepparam            # Development environment
│   ├── staging.bicepparam        # Staging environment
│   └── prod.bicepparam           # Production environment
└── scripts/
    ├── deploy.ps1                # Deployment automation
    └── teardown.ps1              # Resource cleanup
```

---

## Main Bicep File

**File**: `/infra/main.bicep`

```bicep
targetScope = 'subscription'

@description('Azure region for resources')
param location string = 'westeurope'

@description('Environment name (dev, staging, prod)')
@allowed(['dev', 'staging', 'prod'])
param environmentName string

@description('Resource group name')
param resourceGroupName string = 'rg-techhub-${environmentName}'

@description('Application Insights name')
param appInsightsName string = 'appi-techhub-${environmentName}'

@description('Container Registry name (alphanumeric only)')
param containerRegistryName string = 'crtechhub${environmentName}'

@description('Container Apps Environment name')
param containerAppsEnvName string = 'cae-techhub-${environmentName}'

@description('API Container App name')
param apiAppName string = 'ca-techhub-api-${environmentName}'

@description('Web Container App name')
param webAppName string = 'ca-techhub-web-${environmentName}'

@description('API Docker image tag')
param apiImageTag string = 'latest'

@description('Web Docker image tag')
param webImageTag string = 'latest'

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Monitoring (Application Insights + Log Analytics)
module monitoring './modules/monitoring.bicep' = {
  scope: resourceGroup
  name: 'monitoring-deployment'
  params: {
    location: location
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: 'law-techhub-${environmentName}'
  }
}

// Container Registry
module registry './modules/registry.bicep' = {
  scope: resourceGroup
  name: 'registry-deployment'
  params: {
    location: location
    registryName: containerRegistryName
    sku: environmentName == 'prod' ? 'Premium' : 'Basic'
  }
}

// Container Apps Environment
module containerAppsEnv './modules/containerApps.bicep' = {
  scope: resourceGroup
  name: 'containerAppsEnv-deployment'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
}

// API Container App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-deployment'
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    imageTag: apiImageTag
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
  dependsOn: [
    registry
  ]
}

// Web Container App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-deployment'
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
  dependsOn: [
    registry
    apiApp
  ]
}

// Outputs
output resourceGroupName string = resourceGroup.name
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output appInsightsName string = monitoring.outputs.appInsightsName
output containerRegistryLoginServer string = registry.outputs.loginServer
```

---

## Container Apps Environment Module

**File**: `/infra/modules/containerApps.bicep`

```bicep
param location string
param environmentName string
param logAnalyticsWorkspaceId string
param appInsightsConnectionString string

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: environmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: reference(logAnalyticsWorkspaceId, '2023-09-01').customerId
        sharedKey: listKeys(logAnalyticsWorkspaceId, '2023-09-01').primarySharedKey
      }
    }
    openTelemetryConfiguration: {
      destinationsConfiguration: {
        otlpConfigurations: [
          {
            endpoint: 'https://otlp.applicationinsights.azure.com/${appInsightsConnectionString}'
            name: 'appInsights'
          }
        ]
      }
    }
  }
}

output environmentId string = containerAppsEnvironment.id
output defaultDomain string = containerAppsEnvironment.properties.defaultDomain
```

---

## API Container App Module

**File**: `/infra/modules/api.bicep`

```bicep
param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param imageTag string
param appInsightsConnectionString string

resource api 'Microsoft.App/containerApps@2024-03-01' = {
  name: containerAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http2'
        corsPolicy: {
          allowedOrigins: ['*'] // TODO: Restrict in production
          allowedMethods: ['GET', 'POST', 'PUT', 'DELETE']
          allowedHeaders: ['*']
          allowCredentials: false
        }
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: '${containerRegistryName}.azurecr.io/techhub-api:${imageTag}'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsightsConnectionString
            }
            {
              name: 'Content__SectionsJsonPath'
              value: '/app/data/sections.json'
            }
            {
              name: 'Content__CollectionsRootPath'
              value: '/app/data/collections'
            }
            {
              name: 'Content__Timezone'
              value: 'Europe/Brussels'
            }
            {
              name: 'Content__EnableCaching'
              value: 'true'
            }
            {
              name: 'Content__CacheExpirationMinutes'
              value: '60'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '100'
              }
            }
          }
        ]
      }
    }
  }
}

output fqdn string = api.properties.configuration.ingress.fqdn
output id string = api.id
```

---

## Web Container App Module

**File**: `/infra/modules/web.bicep`

```bicep
param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param imageTag string
param apiBaseUrl string
param appInsightsConnectionString string

resource web 'Microsoft.App/containerApps@2024-03-01' = {
  name: containerAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http'
        customDomains: [] // TODO: Add custom domain in production
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'web'
          image: '${containerRegistryName}.azurecr.io/techhub-web:${imageTag}'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsightsConnectionString
            }
            {
              name: 'ApiBaseUrl'
              value: 'https://${apiBaseUrl}'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 2
        maxReplicas: 20
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
}

output fqdn string = web.properties.configuration.ingress.fqdn
output id string = web.id
```

---

## Monitoring Module

**File**: `/infra/modules/monitoring.bicep`

```bicep
param location string
param appInsightsName string
param logAnalyticsWorkspaceName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    IngestionMode: 'LogAnalytics'
  }
}

output appInsightsName string = appInsights.name
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output appInsightsInstrumentationKey string = appInsights.properties.InstrumentationKey
output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
```

---

## Container Registry Module

**File**: `/infra/modules/registry.bicep`

```bicep
param location string
param registryName string

@allowed(['Basic', 'Standard', 'Premium'])
param sku string = 'Basic'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-11-01-preview' = {
  name: registryName
  location: location
  sku: {
    name: sku
  }
  properties: {
    adminUserEnabled: false
    publicNetworkAccess: 'Enabled'
  }
}

output loginServer string = containerRegistry.properties.loginServer
output name string = containerRegistry.name
```

---

## Environment Parameters

**File**: `/infra/parameters/prod.bicepparam`

```bicep
using '../main.bicep'

param location = 'westeurope'
param environmentName = 'prod'
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param containerRegistryName = 'crtechhubprod'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
param apiImageTag = 'v1.0.0' // Specific version for production
param webImageTag = 'v1.0.0'
```

**File**: `/infra/parameters/dev.bicepparam`

```bicep
using '../main.bicep'

param location = 'westeurope'
param environmentName = 'dev'
param resourceGroupName = 'rg-techhub-dev'
param appInsightsName = 'appi-techhub-dev'
param containerRegistryName = 'crtechhubdev'
param containerAppsEnvName = 'cae-techhub-dev'
param apiAppName = 'ca-techhub-api-dev'
param webAppName = 'ca-techhub-web-dev'
param apiImageTag = 'latest'
param webImageTag = 'latest'
```

---

## Deployment Scripts

**File**: `/infra/scripts/deploy.ps1`

```powershell
# !/usr/bin/env pwsh

param(
    [Parameter(Mandatory)]
    [ValidateSet('dev', 'staging', 'prod')]
    [string]$Environment,
    
    [string]$Location = 'westeurope',
    
    [switch]$WhatIf
)

$ErrorActionPreference = 'Stop'

Write-Host "🚀 Deploying Tech Hub to Azure ($Environment)" -ForegroundColor Cyan

# Validate Azure CLI login

$account = az account show 2>$null | ConvertFrom-Json
if (-not $account) {
    Write-Error "Not logged into Azure. Run: az login"
    exit 1
}

Write-Host "✅ Using Azure subscription: $($account.name)" -ForegroundColor Green

# Build parameter file path

$paramFile = Join-Path $PSScriptRoot ".." "parameters" "$Environment.bicepparam"

if (-not (Test-Path $paramFile)) {
    Write-Error "Parameter file not found: $paramFile"
    exit 1
}

# Deploy

$deployArgs = @(
    'deployment', 'sub', 'create'
    '--location', $Location
    '--template-file', (Join-Path $PSScriptRoot ".." "main.bicep")
    '--parameters', $paramFile
    '--name', "techhub-deployment-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
)

if ($WhatIf) {
    $deployArgs += '--what-if'
}

Write-Host "📦 Deploying infrastructure..." -ForegroundColor Yellow

$result = az @deployArgs | ConvertFrom-Json

if ($LASTEXITCODE -ne 0) {
    Write-Error "Deployment failed!"
    exit 1
}

Write-Host "✅ Deployment successful!" -ForegroundColor Green
Write-Host "   API URL: $($result.properties.outputs.apiUrl.value)" -ForegroundColor Cyan
Write-Host "   Web URL: $($result.properties.outputs.webUrl.value)" -ForegroundColor Cyan
Write-Host "   App Insights: $($result.properties.outputs.appInsightsName.value)" -ForegroundColor Cyan
```

---

## Scaling Configuration

**Auto-Scaling Rules**:

- **API**: 1-10 replicas, scale on 100 concurrent requests
- **Web**: 2-20 replicas, scale on 50 concurrent requests

**Resource Limits**:

- **API**: 0.5 vCPU, 1 GiB memory per replica
- **Web**: 0.5 vCPU, 1 GiB memory per replica

**Cost Optimization**:

- Development: Minimum replicas for cost savings
- Production: Higher minimum replicas for availability

---

## Networking (Future Enhancement)

**When to Add VNet**:

- Need private communication between services
- Integration with on-premises resources
- Advanced security requirements

**VNet Configuration**:

```bicep
resource vnet 'Microsoft.Network/virtualNetworks@2024-01-01' = {
  name: 'vnet-techhub-${environmentName}'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: ['10.0.0.0/16']
    }
    subnets: [
      {
        name: 'subnet-containerApps'
        properties: {
          addressPrefix: '10.0.1.0/24'
        }
      }
    ]
  }
}
```

---

## Domain & SSL Configuration

**Production Domain**: `tech.hub.ms`

**SSL Certificate**:

- **Strategy**: Azure-managed SSL certificates (automatic provisioning and renewal)
- **Provider**: Container Apps managed certificates
- **Renewal**: Automatic (no manual intervention required)
- **Configuration**: Custom domain bound to Container Apps with auto-SSL

**DNS Management**:

- **DNS Provider**: To be configured (Azure DNS or external provider)
- **Required Records**:
  - `A` or `CNAME` record pointing to Container Apps endpoint
  - `TXT` record for domain verification (Azure-provided)
- **TTL**: 3600 seconds (1 hour) for production records

**CDN Strategy**:

- **MVP**: Direct Container Apps access (no CDN)
- **Future Enhancement**: Azure Front Door for global CDN if traffic demands
- **Rationale**: Container Apps provides auto-scaling and global availability; CDN adds cost/complexity without proven need

---

## Monitoring & Alerts

**Key Metrics**:

- API response time (p50, p95, p99)
- Request rate and errors (5xx, 4xx)
- Container CPU and memory usage
- Auto-scaling events
- Site availability (uptime monitoring)

**Alert Rules** (Immediate Notification):

- **Critical (P0)**: Site down (availability < 100%) → Alert immediately
- **High (P1)**: Error rate > 5% for 5+ minutes → Alert within 5 minutes
- **Medium (P2)**: API response time > 1000ms (p95) for 10+ minutes → Alert within 15 minutes
- **Low (P3)**: CPU usage > 80% sustained → Daily summary

**Alert Channels**:

- Email notifications to site owner
- Optional: SMS for P0 alerts (site down)
- Azure Monitor action groups for alert routing

**Note**: This is a hobby project - alerts are for awareness, not SLA enforcement. No guaranteed response time for issues.

---

## Security Considerations

**Identity & Access**:

- Container Apps use System-Assigned Managed Identity
- Container Registry access via managed identity (no passwords)
- RBAC roles assigned at resource group level

**Network Security**:

- HTTPS only (enforced by Container Apps)
- CORS configured in API module
- Optional: VNet integration for private communication

**Secrets Management**:

- Application Insights connection string passed as environment variable
- Future: Azure Key Vault for sensitive configuration

---

## Cost Estimation

**Development Environment** (estimated):

- Container Apps: ~$20/month
- Application Insights: ~$5/month
- Container Registry (Basic): ~$5/month
- **Total**: ~$30/month

**Production Environment** (estimated):

- Container Apps: ~$150/month (higher scale)
- Application Insights: ~$30/month (more telemetry)
- Container Registry (Premium): ~$40/month
- **Total**: ~$220/month

*Note: Actual costs depend on traffic and usage patterns*

---

## References

- [Azure Container Apps Documentation](https://learn.microsoft.com/en-us/azure/container-apps/)
- [Bicep Documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Application Insights Documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- `/specs/cicd/github-actions.md` - CI/CD pipelines
- Future spec: monitoring.md - Detailed monitoring setup
- Future spec: networking.md - VNet configuration
