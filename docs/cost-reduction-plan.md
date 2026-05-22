# Cost Reduction Plan

**Goal**: Cut Azure costs significantly without impacting production availability.

---

## Estimated Monthly Savings

| Change | Saves |
|---|---|
| Remove 5 private endpoints (KV, AMPLS, OpenAI×2, PostgreSQL) | ~€40/month |
| Replace ACR Standard with ghcr.io (€0) | ~€20/month |
| Remove staging environment (monitoring, OpenAI) | ~€10/month |
| Remove AMPLS | included in PE savings above |
| Remove Hub VNet | €0 (VNets are free; savings come from removing attached resources) |
| **Total** | **~€70/month** |

---

## Architecture Changes

### Before

```text
rg-techhub-shared (West Europe)
├── crtechhubms (ACR Standard) ← €20/month
├── kv-techhub-shared (Key Vault)
│   └── pe-kv-techhub ← ~€8/month
├── vnet-techhub-hub (Hub VNet)
│   └── ampls-techhub (AMPLS + PE) ← ~€8/month
└── privatelink.postgres.database.azure.com (private DNS zone)

rg-techhub-staging (Sweden Central)
├── vnet-techhub-staging
├── cae-techhub-staging (Container Apps Env — hosts PR previews)
├── oai-techhub-staging (OpenAI, pay-per-token)
├── pe-oai-techhub-staging ← ~€8/month
└── appi/law-techhub-staging (monitoring)

rg-techhub-prod (Sweden Central)
├── vnet-techhub-prod (VNet with 2 subnets)
├── cae-techhub-prod + ca-techhub-api/web-prod
├── psql-techhub-prod + pe-psql-techhub-prod ← ~€8/month
├── oai-techhub-prod + pe-oai-techhub-prod ← ~€8/month
└── appi/law-techhub-prod (monitoring)
```

### After

```text
rg-techhub-prod (Sweden Central)
├── kv-techhub-prod (Key Vault — moved from shared, new name due to soft-delete)
│   ├── IP rules: admin IPs + Container Apps subnet CIDR
│   └── VNet service endpoint: snet-container-apps
├── acme.hub.ms (ACME DNS zone — moved from shared)
├── vnet-techhub-prod (VNet, Container Apps subnet only — no PE subnet)
├── cae-techhub-prod (hosts prod apps AND PR preview apps)
├── ca-techhub-api/web-prod + ca-techhub-api/web-pr-{N} (ephemeral PRs)
├── psql-techhub-prod (public endpoint, subnet CIDR firewall rule)
├── psql-techhub-pr-{N} (PR PITR clones, ephemeral, public access)
├── oai-techhub-prod (public endpoint, IP rules — no private endpoint)
├── ag-techhub-ops (action group — moved from shared)
└── appi/law-techhub-prod (monitoring)

No shared resource group.
No staging resource group.
No ACR — images on ghcr.io (private packages, registry credentials in Key Vault).
```

---

## Files to Delete

### Bicep modules (no longer needed)

| File | Reason |
|---|---|
| `infra/shared.bicep` | Eliminated — contents absorbed into main.bicep or removed |
| `infra/parameters/shared.bicepparam` | No more shared deployment |
| `infra/parameters/staging.bicepparam` | No more staging environment |
| `infra/modules/registry.bicep` | No more ACR |
| `infra/modules/acrRoleAssignment.bicep` | No more ACR pull |
| `infra/modules/hubNetwork.bicep` | No more Hub VNet |
| `infra/modules/monitorPrivateLink.bicep` | No more AMPLS |
| `infra/modules/amplsScope.bicep` | No more AMPLS |
| `infra/modules/amplsSpokeLinks.bicep` | No more AMPLS |
| `infra/modules/keyVaultPrivateEndpoint.bicep` | No more KV PE |
| `infra/modules/postgresPrivateEndpoint.bicep` | No more PostgreSQL PE |
| `infra/modules/openAiPrivateEndpoint.bicep` | No more OpenAI PE |
| `infra/modules/postgresDnsZone.bicep` | No more private DNS zones |
| `infra/modules/privateDnsZoneLink.bicep` | No more private DNS zone links |
| `infra/modules/vnetPeering.bicep` | No more VNet peering |

---

## Files to Modify

### `infra/main.bicep` — Major rewrite

**Remove params:**

- `sharedResourceGroupName` — shared RG is gone
- `containerRegistryName` — no more ACR
- `hubVnetId`, `hubVnetName` — no Hub VNet
- `privateEndpointsSubnetPrefix` — no PE subnet needed

**Add params (absorb from shared.bicep):**

- `keyVaultName` (default: `kv-techhub-prod`)
- `keyVaultAdminObjectIds` (array)
- `acmeDnsZoneName` (default: `acme.hub.ms`)
- `acmeDelegatedDomains`
- `alertEmailAddress`
- `githubOrg` (default: `techhubms`) — for ghcr.io image names

**Remove module calls:**

- `acrRoleAssignment` — no ACR
- `network` (subnet 2 — private endpoints subnet)
- `openAiPrivateEndpoint`
- `postgresPrivateEndpoint`
- `postgresDnsLink`
- `kvDnsLink`
- `amplsScope`
- `peeringSpokeToHub`, `peeringHubToSpoke`

**Add module calls (absorb from shared.bicep):**

- `keyVault` — deployed in prod RG (new name `kv-techhub-prod`)
- `acmeDnsZone` — deployed in prod RG
- `actionGroup` — deployed in prod RG
- `budget` — subscription-scoped (was in shared)
- `policy` — subscription-scoped (was in shared)

**Change logic:**

- Remove `deployApplications` conditional — always deploy everything (prod only)
- Remove `sharedResourceGroup` existing reference
- Remove `environmentName` param entirely — hardcode `'prod'` in resource names (single-environment now)
- Update `sharedKeyVaultUri` → `keyVaultUri` (local KV, same deployment)
- Update `wildcardCerts` module: KV is now in same RG (remove `sharedResourceGroupName`)
- Update `actionGroupId`: now from local `actionGroup.outputs.actionGroupId`
- Pass `containerAppsStartIp` / `containerAppsEndIp` to `postgres` module for subnet firewall rule
- Pass `containerAppsSubnetId` to `keyVault` module for VNet service endpoint

---

### `infra/parameters/prod-infrastructure.bicepparam`

**Remove:**

- `environmentName` (no longer parameterized)
- `hubVnetId`, `hubVnetName`
- `sharedResourceGroupName`
- `containerRegistryName`
- `privateEndpointsSubnetPrefix`

**Add:**

- `keyVaultName = 'kv-techhub-prod'`
- `keyVaultAdminObjectIds = []` (populate with object IDs)
- `alertEmailAddress = 'reinier.vanmaanen@xebia.com'`
- `acmeDnsZoneName = 'acme.hub.ms'`
- `acmeDelegatedDomains = ['hub.ms', 'xebia.ms']`
- `monthlyBudgetAmount = 180` (current spend minus ~€70 savings; keep margin above expected ~€110)
- `budgetStartDate = '2026-04-01'`
- `containerAppsStartIp = '10.2.0.0'`
- `containerAppsEndIp = '10.2.1.255'`
- `githubOrg = 'techhubms'`

---

### `infra/modules/network.bicep`

**Remove:**

- `privateEndpointsSubnetName` and `privateEndpointsSubnetPrefix` params
- `privateEndpointsNsg` NSG resource
- Private endpoints subnet from VNet definition
- `privateEndpointsSubnetId` output

**Add:**

- `serviceEndpoints: [{ service: 'Microsoft.KeyVault' }]` on the Container Apps subnet
  (required so the Key Vault VNet rule can trust traffic from this subnet)

> **Verified:** Service endpoints are fully supported on subnets with `Microsoft.App/environments` delegation. The delegation restricts what resource types can be *deployed into* the subnet; it does not restrict service endpoint configuration. Container Apps pods egress from subnet IPs, and Key Vault identifies traffic by subnet identity through the Azure fabric — independent of internet-bound NAT.

---

### `infra/modules/postgres.bicep`

**Add params:**

- `containerAppsStartIp string = ''` — e.g. `'10.2.0.0'`
- `containerAppsEndIp string = ''` — e.g. `'10.2.1.255'`

**Add firewall rule:**

```bicep
resource containerAppsFirewallRule 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2024-08-01' = if (!empty(containerAppsStartIp)) {
  parent: postgresServer
  name: 'allow-container-apps-subnet'
  properties: {
    startIpAddress: containerAppsStartIp
    endIpAddress: containerAppsEndIp
  }
}
```

Use explicit start/end IP params rather than CIDR parsing — Bicep has no native CIDR-to-range conversion. For `10.2.0.0/23`: start = `10.2.0.0`, end = `10.2.1.255`.

> **Security note:** This grants network access to all Container Apps in the environment (including PR previews). This is acceptable because PR previews use their own PITR-cloned database — they never have credentials for `psql-techhub-prod`. The firewall rule provides network-level access, but authentication still requires the correct connection string (stored only in Key Vault secrets accessible to the prod Container App).

---

### `infra/modules/keyVault.bicep`

**Add param:**

- `containerAppsSubnetId string = ''`

**Add to `networkAcls`:**

```bicep
virtualNetworkRules: !empty(containerAppsSubnetId) ? [
  {
    id: containerAppsSubnetId
    ignoreMissingVnetServiceEndpoint: false
  }
] : []
```

---

### `infra/modules/api.bicep`

**Remove params:**

- `containerRegistryName`
- `acrPullIdentityId` → rename to `identityId` (still used for KV access)

**Add param:**

- `githubOrg string` — e.g. `'techhubms'`

**Change:**

- `imageReference`: `'ghcr.io/${githubOrg}/techhub-api:${imageTag}'`
- `registries`: replace ACR config with ghcr.io credentials (username from param, password from Key Vault secret `techhub-github-registry-token`)
- Identity in resource: keep `identityId` for KV secret access
- All `identity: acrPullIdentityId` secret refs → `identity: identityId`

---

### `infra/modules/web.bicep`

Same changes as `api.bicep` (same pattern — ghcr.io with registry credentials).

---

### `infra/modules/wildcardCertificates.bicep`

**Remove params:**

- `sharedResourceGroupName`

**Change:**

- `kvSecretsRole` module: remove `scope: resourceGroup(sharedResourceGroupName)` — KV is now in the same RG, no cross-RG scope needed.

---

## Scripts to Modify

### `scripts/Deploy-Infrastructure.ps1`

**Remove:**

- `shared` and `staging` entries from `$envConfig`
- Staging validation for `POSTGRES_ADMIN_PASSWORD` (staging never had it)
- Action group resolution from shared RG (action group now in prod RG)
- `AZURE_AD_CLIENT_SECRET` staging skip

**Change:**

- Rename `Environment` valid values from `['shared', 'staging', 'production']` → `['production']`
- Action group resolution: look in `rg-techhub-prod` (same RG as deployment target)

---

### `scripts/Deploy-Application.ps1`

**Remove params:**

- `RegistryName` (no more ACR)

**Change:**

- `$registryServer`: `'ghcr.io'`
- `$apiImage`: `'ghcr.io/techhubms/techhub-api'`
- `$webImage`: `'ghcr.io/techhubms/techhub-web'`
- Registry login: replace `az acr login --name $RegistryName` with `docker login ghcr.io -u $env:GITHUB_ACTOR -p $env:GITHUB_TOKEN`
- This means `GITHUB_TOKEN` (or a PAT) must be set when pushing — in CI this is automatic; locally a PAT is needed.

---

### `scripts/Deploy-PrPreview.ps1`

This script changes most significantly.

**Change targets:**

- `$stagingRG = 'rg-techhub-staging'` → `$prodRG = 'rg-techhub-prod'`
- `$stagingEnvName = 'cae-techhub-staging'` → `$prodEnvName = 'cae-techhub-prod'`
- `$stagingIdentityName = 'id-techhub-staging'` → `$prodIdentityName = 'id-techhub-prod'`
- `$registryServer = 'crtechhubms.azurecr.io'` → `$registryServer = 'ghcr.io'`
- Image refs: `ghcr.io/techhubms/techhub-api:$Tag`

**Remove from deploy action:**

- Private endpoint creation (`az network private-endpoint create` + DNS zone group)

**Change PostgreSQL provisioning:**

- PITR restore still works (same approach)
- After PITR, enable public access: `az postgres flexible-server update --public-access Enabled`
- Add firewall rule for Container Apps subnet CIDR instead of private endpoint:

  ```powershell
  az postgres flexible-server firewall-rule create `
      --name $prPostgresServer --resource-group $prodRG `
      --rule-name allow-container-apps `
      --start-ip-address 10.2.0.0 --end-ip-address 10.2.1.255
  ```

**Remove from teardown action:**

- Private endpoint deletion steps
- DNS A record cleanup
- ACR image cleanup (replace with ghcr.io image cleanup using `gh api` or `docker manifest`)

**Note:** PITR from prod still works — the PITR target is now in `rg-techhub-prod` instead of `rg-techhub-staging`.

---

## CI/CD Workflow Changes

### `.github/workflows/cd.yml`

**Add permission:**

- `packages: write` (for ghcr.io push)

**Remove jobs:**

- `deploy-shared-infra`
- `deploy-staging-infra`

**Change `build-and-push` job:**

- Remove: Azure Login (no ACR needed), or keep for production deployment
- Add: `docker login ghcr.io -u ${{ github.actor }} -p ${{ secrets.GITHUB_TOKEN }}`
- Update `Deploy-Application.ps1` call (no `-RegistryName` param)

**Change `deploy-production` job:**

- Remove `needs: deploy-staging-infra` dependency
- Keep `needs: build-and-push` only

---

### `.github/workflows/ci.yml`

**Add permission:**

- `packages: write` (for ghcr.io push in PR preview)

**Change `pr-preview-build-and-push` job:**

- Replace ACR auth with ghcr.io auth: `docker login ghcr.io -u ${{ github.actor }} -p ${{ secrets.GITHUB_TOKEN }}`
- Update `Deploy-Application.ps1` call (no `-RegistryName` param)

**Change `pr-preview-deploy` job:**

- Update environment URL to reference prod CAE domain (not staging)
- PR comment: update note (now deploys to prod RG/CAE, still isolated database)

**Change PR teardown job (closed PR):**

- Same resource group change (`rg-techhub-prod`)
- ghcr.io image cleanup instead of ACR

---

## Key Vault Migration (Manual Step Required)

The existing `kv-techhub-shared` has purge protection enabled — it **cannot** be deleted for 90 days after removal and the name is locked. The new deployment creates `kv-techhub-prod`.

**Secrets to migrate from `kv-techhub-shared` to `kv-techhub-prod`:**

| Secret name | Description |
|---|---|
| `techhub-prod-db-connection-string` | PostgreSQL connection string |
| `techhub-prod-ai-api-key` | Azure OpenAI API key |
| `techhub-prod-aad-client-secret` | Azure AD client secret |
| `techhub-github-registry-token` | GitHub PAT for ghcr.io pull (new — create before migration) |
| `wildcard-hub-ms` | Wildcard TLS certificate for `*.hub.ms` |
| `wildcard-xebia-ms` | Wildcard TLS certificate for `*.xebia.ms` |

A helper script can be added (e.g., `scripts/Migrate-KeyVaultSecrets.ps1`) to copy secrets between vaults.

---

## Networking: How Container Apps reach PostgreSQL and Key Vault

Without private endpoints, connectivity is as follows:

### PostgreSQL

- PostgreSQL Flexible Server: `publicNetworkAccess: Enabled`
- Firewall rules: admin IPs + Container Apps subnet CIDR (`10.2.0.0` → `10.2.1.255`)
- Container App pods have IPs within the Container Apps subnet range
- No DNS change needed — uses the public FQDN `psql-techhub-prod.postgres.database.azure.com`

### Key Vault

- Key Vault: `publicNetworkAccess: Enabled`, `defaultAction: Deny`
- Firewall rules: admin IPs
- VNet service endpoint: `Microsoft.KeyVault` on Container Apps subnet
- VNet rule: Container Apps subnet ID → Key Vault allows traffic from that subnet
- Container Apps still use the public FQDN `kv-techhub-prod.vault.azure.net`

### Azure OpenAI

- Already configured with public endpoint + IP rules
- Remove private endpoint only (no other changes)

---

## ghcr.io Image Strategy

Images are pushed to:

- `ghcr.io/techhubms/techhub-api:{tag}`
- `ghcr.io/techhubms/techhub-web:{tag}`

**Visibility: Private** (default)

- GitHub Actions push using built-in `GITHUB_TOKEN` with `packages: write`
- Container Apps pull using a GitHub PAT with `read:packages` scope, stored in Key Vault as `techhub-github-registry-token`
- Container Apps `registries` config references the Key Vault secret for authentication
- No manual visibility change needed — packages are private by default

This keeps images private while avoiding ACR costs. The PAT needs the `read:packages` scope only and should be a fine-grained token scoped to the `techhubms` org.

---

## Deployment Order After Changes

1. Push images to ghcr.io (first time — creates the packages)
2. Create a GitHub PAT with `read:packages` scope and store in `kv-techhub-shared` temporarily
3. Deploy prod infrastructure (creates new `kv-techhub-prod`, ACR gone)
4. Run `Migrate-KeyVaultSecrets.ps1` to copy secrets from `kv-techhub-shared` → `kv-techhub-prod` (including `techhub-github-registry-token`)
5. Deploy prod application (Container Apps now pull from ghcr.io with registry credentials)
6. Verify production is healthy
7. **Verify DNS resolution**: Confirm Container Apps connect to PostgreSQL and Key Vault via public FQDNs (not stale private DNS records from the old `privatelink.*` zones)
8. Tear down staging: `az group delete --name rg-techhub-staging`
9. Tear down shared: `az group delete --name rg-techhub-shared` (soft-delete KV stays 90 days but costs nothing)
10. Mark `crtechhubms` ACR for deletion (or let it be deleted with the RG)

---

## What Does NOT Change

- Production Container Apps (API, Web) — same names, same config
- PostgreSQL (`psql-techhub-prod`) — same server, same data, same SKU
- Azure OpenAI (`oai-techhub-prod`) — same resource, just no private endpoint
- Custom domains (`tech.hub.ms`, `tech.xebia.ms`) — unchanged
- Wildcard certificates — same certs, just stored in the new KV
- Monitoring (App Insights, Log Analytics) — unchanged
- CI/CD quality gates — unchanged
- E2E tests — unchanged
- Prod deployment approval flow — unchanged

---

## Summary of New GitHub Secrets Needed

| Secret | Notes |
|---|---|
| `GITHUB_TOKEN` | Auto-provided by GitHub Actions — no action needed |
| `POSTGRES_ADMIN_PASSWORD` | Unchanged |
| `ADMIN_IP_ADDRESSES` | Unchanged |
| `AZURE_AD_CLIENT_SECRET` | Unchanged |
| `GHCR_READ_PAT` | GitHub PAT with `read:packages` scope — stored in Key Vault for Container Apps registry pull |
| ~~`CONTAINER_REGISTRY_*`~~ | Not needed — no ACR |

---

## Files to Check for ACR References

Grep for `crtechhubms` and `azurecr.io` in:

- `.github/workflows/cd.yml`
- `.github/workflows/ci.yml`
- `scripts/Deploy-Application.ps1`
- `scripts/Deploy-PrPreview.ps1`
- `docker-compose.yml` (if any)

All references must be updated to `ghcr.io/techhubms/` equivalents.

---

## DNS Transition: Avoiding Stale Private DNS Resolution

When private endpoints are removed, the `privatelink.postgres.database.azure.com` and `privatelink.vaultcore.azure.net` DNS zones (linked to the VNet) may still resolve to old private IPs. To prevent connectivity failures during transition:

1. **Before removing private endpoints**: Remove the DNS zone VNet links from the prod VNet
2. **Verify**: From a Container App, confirm `psql-techhub-prod.postgres.database.azure.com` resolves to the public IP
3. **Then**: Delete the private endpoints and DNS zones (with the shared/staging RGs)

Alternatively, deploy the infra changes (which remove VNet links and private endpoints simultaneously) and perform a zero-downtime restart of the Container Apps to refresh DNS caches.
