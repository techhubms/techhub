# Infrastructure Deployment Rules

**This file is for AI coding agents.** Read this before modifying or deploying infrastructure.

## Two-Phase Deployment Architecture

The infrastructure is deployed in **two strictly separated phases**:

### Phase 1: Base Infrastructure (`infrastructure.bicep`)

**Contains ONLY platform-level resources:**

- Networking (VNet, NAT Gateway, subnets, DNS zones)
- Identity (managed identities for production and PR environments)
- Key Vault (with admin IP firewall rules)
- PostgreSQL Flexible Server
- Azure AI Foundry (OpenAI)
- Log Analytics + Application Insights
- Container Apps Environment (no apps yet!)
- Azure Communication Services (ACS) for email delivery
- Monitoring (action groups, alerts, availability tests)
- Governance (budgets, policies)

**Key principle**: Phase 1 creates the **Key Vault first**, then stores all application secrets in it.

**ACS configuration pattern (managed identity):**

- Endpoint passed via Bicep output from Phase 1 to Phase 2
- RBAC role assigned to API managed identity in Phase 1
- Only sender address stored in Key Vault secret: `techhub-prod-acs-sender-address`

### Phase 2: Container Apps (`applications.bicep`)

**Contains ONLY application deployments:**

- API Container App
- Web Container App

**No infrastructure resources.** Phase 2 reads from Key Vault secrets created in Phase 1.

## Secret Flow Pattern

```text
Phase 1 (infrastructure.bicep)
  ↓
  Creates ACS resources + RBAC role assignment
  ↓
  Writes sender address to Key Vault secret
  ↓
  Outputs endpoint URL (not secret) to deployment output
  ↓
Phase 2 (applications.bicep)
  ↓
  Receives ACS endpoint as parameter
  ↓
  Reads sender address secret name from Key Vault
  ↓
  Container App uses managed identity to authenticate to ACS
```

**Key insight**: With managed identity, only the sender address needs Key Vault storage. The endpoint is public and passed via Bicep parameters. Authentication happens automatically via Entra ID.

## Rules for Adding New Infrastructure

### Adding a new shared service (like ACS)

1. ✅ Add to `infrastructure.bicep` (Phase 1)
2. ✅ Store outputs in Key Vault secrets
3. ✅ Reference in `applications.bicep` (Phase 2) via Key Vault secret names
4. ❌ **Never** pass outputs directly from Phase 1 to Phase 2 via Bicep outputs

### Adding a new application secret

1. ✅ Add to Key Vault in Phase 1 (or via `Sync-KeyVaultSecrets.ps1` between phases)
2. ✅ Reference in Phase 2 via `keyVaultUrl` in Container App secrets
3. ❌ **Never** pass secrets as plain Bicep parameters in Phase 2

## Why This Separation?

**Problem**: You can't deploy "just infrastructure" without also deploying Container Apps.

**Before** (wrong):

- Phase 1: Base infrastructure
- Phase 2: ACS + Container Apps (tightly coupled!)
- **Issue**: Can't add ACS without providing container image tags

**After** (correct):

- Phase 1: Base infrastructure + ACS (no container apps!)
- Key Vault secrets bridge the gap
- Phase 2: Container Apps only (reads from Key Vault)
- **Benefit**: Can deploy Phase 1 independently for local development

## Local Development Workflow

For testing features that require new infrastructure (like ACS):

1. Deploy Phase 1 only: Creates infrastructure + writes secrets to Key Vault
2. Run `Setup-UserSecrets.ps1`: Fetches secrets from Key Vault to local user secrets
3. Run `Run` locally: Uses local user secrets (no container deployment needed!)

## Deployment Scripts

Infrastructure and applications are deployed by **separate scripts**:

| Script | Phase | What it deploys | Image tag needed? |
|--------|-------|-----------------|-------------------|
| `Deploy-Infrastructure.ps1` | Phase 1 | `infrastructure.bicep` + secret sync | **No** |
| `Deploy-Applications.ps1` | Phase 2 | `applications.bicep` (Container Apps) | **Yes** |
| `Build-Images.ps1` | Build | Docker build + push to ghcr.io | **Yes** |
| `Deploy-Application.ps1` | Fast path | `az containerapp update` (image swap only) | **Yes** |

### CD Pipeline (automatic)

The pipeline detects whether `infra/` or `scripts/` changed since last successful deployment:

- **Infra changed** → `Deploy-Infrastructure.ps1` + `Deploy-Applications.ps1` (~10 min)
- **Code only** → `Deploy-Application.ps1 -Tag <tag>` (~1 min)

### Local development (Phase 1 only)

```powershell
$env:ADMIN_IP_ADDRESSES = "<your-ip>"
$env:POSTGRES_ADMIN_PASSWORD = "<password>"
./scripts/Deploy-Infrastructure.ps1 -Mode deploy
```

### Full manual deploy (both phases)

```powershell
./scripts/Deploy-Infrastructure.ps1 -Mode deploy
./scripts/Deploy-Applications.ps1 -Mode deploy -ImageTag "20260501120000"
```

## Authentication Pattern: Always Use Managed Identities

**CRITICAL RULE**: Always use managed identities for Azure service authentication. Never use connection strings, API keys, or passwords when managed identity is available.

### ✅ Services Using Managed Identity (Current)

- **PostgreSQL** - Container Apps use `DefaultAzureCredential` with passwordless connection string
- **Azure AI Foundry (OpenAI)** - Apps use Entra token auth instead of API keys
- **Key Vault** - Container Apps reference secrets via `keyVaultUrl` with managed identity
- **Azure Communication Services** - EmailClient uses managed identity instead of connection string

### 🔧 When Adding New Azure Services

1. **Check if managed identity is supported** (most Azure services support it)
2. **Add RBAC role assignment** in `infrastructure.bicep` granting API's managed identity the required role
3. **Pass endpoint/resource ID** instead of connection strings
4. **Update app code** to use `DefaultAzureCredential` or equivalent
5. **Only use Key Vault secrets** for truly secret values (e.g., sender email address, unsubscribe tokens)

### Example Pattern

```bicep
// Phase 1: Grant managed identity access to service
module serviceRole './modules/serviceDataContributorRole.bicep' = {
  scope: resourceGroup
  params: {
    serviceName: serviceName
    principalId: identity.outputs.identityPrincipalId
    principalType: 'ServicePrincipal'
  }
}

// Pass endpoint, NOT connection string
output serviceEndpoint string = 'https://${service.name}.azure.com/'
```

```csharp
// App code: Use DefaultAzureCredential
var client = new ServiceClient(
    new Uri(options.Endpoint),
    new DefaultAzureCredential());
```

### Why Managed Identity?

- ✅ **No secrets to manage** - Azure handles credential lifecycle
- ✅ **Automatic rotation** - No manual key updates required
- ✅ **Better security** - No string-based credentials to leak
- ✅ **Audit trail** - Azure tracks all access via Entra ID
- ✅ **Zero-trust** - Aligns with modern security architecture

## Never Do

- ❌ Add Container Apps to `infrastructure.bicep`
- ❌ Add infrastructure resources (storage, databases, message queues, etc.) to `applications.bicep`
- ❌ Pass secrets as plain Bicep parameters (always use Key Vault + `keyVaultUrl`)
- ❌ Reference outputs directly between phases (use Key Vault as the bridge)
- ❌ **Use connection strings or API keys when managed identity is available**
