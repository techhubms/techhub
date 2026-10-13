# Infrastructure Deployment Rules

**This file is for AI coding agents.** Read this before modifying or deploying infrastructure.

## Two-Phase Deployment Architecture

The infrastructure is deployed in **two strictly separated phases**:

### Phase 1: Base Infrastructure (`infrastructure.bicep`)

**Contains ONLY platform-level resources:**

- Networking (VNet, subnets, private endpoints, DNS zones)
- Identity (managed identities for production and PR environments)
- Key Vault (with admin IP firewall rules)
- PostgreSQL Flexible Server
- Azure AI Foundry (OpenAI)
- Log Analytics + Application Insights
- App Service Plan (Basic B1, no sites yet!)
- Azure Communication Services (ACS) for email delivery, using a customer-managed email domain (`mail.hub.ms`)
- Monitoring (action groups, alerts, availability tests)
- Governance (budgets, policies)
- ACME DNS zone (`acme.hub.ms`) for wildcard certificate renewal

**Key principle**: Phase 1 creates the **Key Vault first**, then stores all application secrets in it.

**ACS configuration pattern (managed identity):**

- RBAC role (`acsDataContributorRole.bicep`) assigned to the API managed identity in Phase 1
- Sender address stored in Key Vault secret `techhub-prod-acs-sender-address`, written directly by Bicep in Phase 1
- Endpoint is a Phase 1 Bicep output (`acsEndpoint`). `Deploy-Infrastructure.ps1` exports it as `NEWSLETTER_ACS_ENDPOINT` and `Sync-KeyVaultSecrets.ps1` stores it in Key Vault secret `techhub-prod-newsletter-acs-endpoint`. Phase 2 only knows the secret name (`acsEndpointSecretName`)

**ACS custom email domain (verify, then link):** the domain must have verified DNS records before it can be linked to the Communication Service. The first deploy creates the domain without linking it (`linkEmailDomain = false`). After the DNS records are added and verified in ACS, redeploy with `Deploy-Infrastructure.ps1 -LinkEmailDomain` (or `linkEmailDomain = true` in the parameter file). Every PUT of the domain resets its verification, so `Deploy-Infrastructure.ps1` sets `createEmailDomain = false` once the domain exists. See [docs/network-architecture.md](../docs/network-architecture.md#azure-communication-services-acs).

### Phase 2: App Service Sites (`applications.bicep`)

**Contains ONLY application deployments:**

- API app (Web App for Containers, VNet-integrated; not publicly accessible — access restrictions allow inbound calls only from the Web app's VNet integration subnet)
- Web app (Web App for Containers, public-facing)

**No infrastructure resources.** Phase 2 reads from Key Vault secrets created in Phase 1.

### PR previews (`pr-applications.bicep`)

`pr-applications.bicep` deploys the per-PR API + Web sites onto the shared, **ephemeral** PR-preview App Service Plan (`asp-techhub-pr`). Phase 1 deliberately does not create that Plan: `Deploy-PrPreview.ps1` creates it on the first PR preview deploy and deletes it after the last teardown. Phase 1 only creates the PR subnet and the PR managed identity (`id-techhub-pr`). See [docs/ci-cd-pipeline.md](../docs/ci-cd-pipeline.md#pr-preview-environments).

### Parameters and compiled output

- `parameters/` holds the production parameter files (`prod-infrastructure.bicepparam`, `prod-applications.bicepparam`). `readEnvironmentVariable()` is used for values that must not be committed (admin IPs, the PostgreSQL password) or that change per deploy (image tags)
- Compiled ARM templates (`infra/**/*.json`) are build output and gitignored. Edit the Bicep files only

## Secret Flow Pattern

```text
Phase 1 (infrastructure.bicep)
  ↓
  Creates ACS resources + RBAC role assignment
  ↓
  Writes sender address to Key Vault secret (Bicep)
  ↓
  Outputs endpoint URL (not secret) to deployment output
  ↓
Deploy-Infrastructure.ps1 → Sync-KeyVaultSecrets.ps1
  ↓
  Writes endpoint URL to Key Vault secret (techhub-prod-newsletter-acs-endpoint)
  ↓
Phase 2 (applications.bicep, standalone)
  ↓
  Receives only the Key Vault secret NAMES as parameters
  ↓
  App settings are Key Vault references resolved by App Service at runtime
  ↓
  App Service site uses managed identity to authenticate to ACS
```

**Key insight**: With managed identity, no ACS key or connection string exists. The endpoint and sender address travel through Key Vault, not through Bicep outputs or parameters, so Phase 2 stays fully standalone (`Deploy-Applications.ps1` needs only an image tag). Authentication happens automatically via Entra ID.

## Rules for Adding New Infrastructure

### Adding a new shared service (like ACS)

1. ✅ Add to `infrastructure.bicep` (Phase 1)
2. ✅ Store values Phase 2 needs in Key Vault secrets (written by Bicep, or by `Sync-KeyVaultSecrets.ps1` for deployment outputs)
3. ✅ Reference in `applications.bicep` (Phase 2) via Key Vault secret names
4. ❌ **Never** pass Phase 1 outputs to Phase 2 as Bicep outputs or parameters (scripts may read outputs only to write them to Key Vault)

### Adding a new application secret

1. ✅ Add to Key Vault in Phase 1 (or via `Sync-KeyVaultSecrets.ps1` between phases)
2. ✅ Reference in Phase 2 via a Key Vault reference app setting (`@Microsoft.KeyVault(SecretUri=...)`) in `applications.bicep`
3. ❌ **Never** pass secrets as plain Bicep parameters in Phase 2

## Why This Separation?

**Problem**: You can't deploy "just infrastructure" without also deploying application sites.

**Before** (wrong):

- Phase 1: Base infrastructure
- Phase 2: ACS + application sites (tightly coupled!)
- **Issue**: Can't add ACS without providing container image tags

**After** (correct):

- Phase 1: Base infrastructure + ACS (no application sites!)
- Key Vault secrets bridge the gap
- Phase 2: Application sites only (reads from Key Vault)
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
| `Deploy-Infrastructure.ps1` | Phase 1 | `infrastructure.bicep` + secret sync (`-LinkEmailDomain` links the verified ACS email domain) | **No** |
| `Deploy-Applications.ps1` | Phase 2 | `applications.bicep` (API + Web App Service sites) | **Yes** |
| `Build-Images.ps1` | Build | Docker build + push to ghcr.io | **Yes** |
| `Deploy-Application.ps1` | Fast path | `az webapp config set` (image swap only) | **Yes** |
| `Deploy-PrPreview.ps1` | PR preview | `pr-applications.bicep` + ephemeral PR App Service Plan + PITR PostgreSQL server (`-Action deploy` or `teardown`) | **Yes** (`-Tag`) |
| `Teardown-PrEnvironments.ps1` | PR preview | Tears down every active PR preview (nightly workflow) | No |
| `Sync-KeyVaultSecrets.ps1` | Secrets | Writes application secrets from env vars to Key Vault. Called by `Deploy-Infrastructure.ps1` | No |
| `Renew-WildcardCertificates.ps1` | Certificates | Standalone `modules/wildcardCert.bicep` after renewing the PFX in Key Vault | No |
| `Setup-OidcAuthentication.ps1` | One-time setup | GitHub Actions OIDC federated credentials and repository variables (no Bicep) | No |

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

- **PostgreSQL** - App Service sites use `DefaultAzureCredential` with passwordless connection string
- **Azure AI Foundry (OpenAI)** - Apps use Entra token auth instead of API keys
- **Key Vault** - App Service sites reference secrets via Key Vault reference app settings (`@Microsoft.KeyVault(SecretUri=...)`) with managed identity
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
module serviceRole './modules/acsDataContributorRole.bicep' = {
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

- ❌ Add application sites to `infrastructure.bicep`
- ❌ Add infrastructure resources (storage, databases, message queues, etc.) to `applications.bicep`
- ❌ Pass secrets as plain Bicep parameters (always use Key Vault + `keyVaultUri`)
- ❌ Reference outputs directly between phases (use Key Vault as the bridge)
- ❌ **Use connection strings or API keys when managed identity is available**
