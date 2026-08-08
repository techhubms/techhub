# Network Architecture

Tech Hub uses a **single VNet** in the production resource group. Key Vault, AI Foundry, and
PostgreSQL are all reached over **private endpoints** in a dedicated subnet — App Service
traffic to these services never touches the public internet. Admin access to each service goes
through public access with IP-based firewall rules instead.

## Topology

```text
Internet
    │
    ├── Web site (app-techhub-web-prod) — public, https://tech.hub.ms, https://tech.xebia.ms
    │       HTTPS on tech.hub.ms, tech.xebia.ms (wildcard TLS from kv-techhub-prod)
    │
    └── Admin IP (allowlisted for Key Vault, PostgreSQL, and AI Foundry)

Prod VNet — vnet-techhub-prod (10.2.0.0/16) [rg-techhub-prod]
    ├── snet-app-service (10.2.0.0/23) — Regional VNet Integration for asp-techhub-prod (Basic B1)
    │    │
    │    ├── app-techhub-web-prod   — public HTTPS ingress, reachable from internet
    │    │
    │    └── app-techhub-api-prod   — IP-restricted to the Web site's VNet-integration outbound
    │            address only; NOT reachable from the internet (Web calls API over the VNet)
    │
    ├── snet-app-service-pr (10.2.4.0/23) — Regional VNet Integration for asp-techhub-pr,
    │    │                                   the shared PR-preview App Service Plan
    │    │
    │    └── app-techhub-{api,web}-pr-{N} — same public/VNet-restricted split as prod, per PR
    │
    └── snet-private-endpoints (10.2.2.0/27) — private endpoint NICs only
         │
         ├── pe-psql-techhub-prod → psql-techhub-prod
         │       → App Service sites reach PostgreSQL over the VNet via
         │         privatelink.postgres.database.azure.com (Private DNS Zone)
         │
         ├── pe-kv-techhub-prod → kv-techhub-prod
         │       → App Service sites reach Key Vault over the VNet via
         │         privatelink.vaultcore.azure.net (Private DNS Zone)
         │
         └── pe-oai-techhub-prod → oai-techhub-prod
                 → App Service sites reach AI Foundry over the VNet via
                   privatelink.openai.azure.com (Private DNS Zone, app-facing endpoint),
                   privatelink.cognitiveservices.azure.com (Private DNS Zone, other data-plane APIs), and
                   privatelink.services.ai.azure.com (Private DNS Zone, AI Foundry project/agent APIs)
```

## Address Spaces

| VNet | CIDR | Resource Group | Purpose |
|------|------|----------------|---------|
| `vnet-techhub-prod` | `10.2.0.0/16` | `rg-techhub-prod` | App Service VNet integration + private endpoints (prod + PR previews) |

There are three subnets:

- `snet-app-service` (`10.2.0.0/23`), delegated to `Microsoft.Web/serverFarms` — Regional VNet
  Integration for the production App Service Plan (`asp-techhub-prod`)
- `snet-app-service-pr` (`10.2.4.0/23`), delegated to `Microsoft.Web/serverFarms` — Regional VNet
  Integration for the shared PR-preview App Service Plan (`asp-techhub-pr`). Regional VNet
  Integration is strictly 1 subnet : 1 App Service Plan, so the PR-preview Plan needs its own
  dedicated subnet even though it is deployed and reused persistently (not created/torn down per PR)
- `snet-private-endpoints` (`10.2.2.0/27`), hosts the PostgreSQL, Key Vault, and AI Foundry
  private endpoint NICs — private endpoints cannot share a subnet delegated to
  `Microsoft.Web/serverFarms`

No hub-spoke peering.

## App Service Ingress

Both API and Web sites run on the same **Basic B1 App Service Plan** (`asp-techhub-prod`), each
as a separate Web App for Containers with its own VNet-integrated outbound traffic and its own
inbound access rules.

| App | Ingress | Reachable from internet | Notes |
|-----|---------|------------------------|-------|
| `app-techhub-web-prod` | Public | **Yes** | Custom domains (`tech.hub.ms`, `tech.xebia.ms`); wildcard TLS from Key Vault |
| `app-techhub-api-prod` | `ipSecurityRestrictions` (deny-by-default, allow only the Web site's VNet-integration subnet) | **No** | Internal only; the Web frontend calls the API over the VNet |

The API backend is intentionally not publicly accessible. No path to the API exists from the
internet — not via the custom domains, not via its default `*.azurewebsites.net` hostname. The Web
Blazor frontend calls the API using its `ApiBaseUrl` app setting; with `vnetRouteAllEnabled: true`
on the Web App, all outbound traffic (including API calls) is sourced from the Web site's VNet
integration subnet. The API's `ipSecurityRestrictions` allow only that subnet and deny everything
else by default — so while the API still has a public `*.azurewebsites.net` hostname (there is no
private endpoint on the API), the IP restriction policy effectively blocks all public access.

CORS policy is configured on the API app and restricts allowed origins to the configured
`primaryHosts` (i.e. `tech.hub.ms`, `tech.xebia.ms`), so even if the API were made public,
cross-origin browser requests from unexpected origins would be blocked.

Admin access to Azure resources is controlled via per-resource IP firewall rules using the
`ADMIN_IP_ADDRESSES` environment variable (supports multiple comma-separated IPs).

| Resource | Firewall Mechanism | Access |
|----------|-------------------|--------|
| Key Vault | Private endpoint (VNet traffic) + `networkAcls.ipRules` (admin traffic) | App Service sites via private endpoint; admin IPs via public access; default deny, no trusted-services bypass |
| PostgreSQL | Private endpoint (VNet traffic) + per-IP/range firewall rules (admin traffic) | App Service sites via private endpoint; admin IPs via public access; default deny otherwise |
| AI Foundry | Private endpoint (VNet traffic) + `networkAcls.ipRules` (admin traffic) | App Service sites via private endpoint; admin IPs via public access; default deny, no trusted-services bypass |
| Log Analytics | Public ingestion + query enabled | RBAC-protected |
| App Insights | Public ingestion + query enabled | RBAC-protected; browser JS SDK over public internet |

## Key Vault

The production Key Vault (`kv-techhub-prod`) stores:

- Wildcard TLS certificates for `*.hub.ms` and `*.xebia.ms`
- AAD client secret (`techhub-prod-aad-client-secret`) for the admin dashboard
- GitHub registry token (`techhub-github-registry-token`) for App Service sites pulling from ghcr.io
- PostgreSQL admin password (`techhub-prod-postgres-admin-password`) for infrastructure management
- Newsletter ACS endpoint URL (`techhub-prod-newsletter-acs-endpoint`) — written automatically by `Sync-KeyVaultSecrets.ps1` from infra deployment outputs
- Newsletter ACS sender address (`techhub-prod-acs-sender-address`) — written directly by Bicep from the ACS domain output
- Newsletter unsubscribe HMAC key (`techhub-prod-newsletter-unsubscribe-secret`) for signing unsubscribe/confirm links

Security:

- **Public access**: Enabled only when admin IPs are configured; admin IPs allowlisted via
  `networkAcls.ipRules`; default deny; `bypass: 'None'` (no trusted Microsoft services bypass)
- **Private endpoint**: `pe-kv-techhub-prod` in `snet-private-endpoints`, resolved via the
  `privatelink.vaultcore.azure.net` Private DNS Zone linked to the VNet — App Service sites reach
  Key Vault entirely over the VNet, replacing the previous VNet service endpoint
- **Authorization**: RBAC (Key Vault Administrator role for admins; Key Vault Secrets User for
  the managed identity used by App Service sites)

> **Note:** PostgreSQL connection strings and AI Foundry API keys are no longer stored in Key
> Vault. The application uses managed identity token auth for both services (see below).

## Container Registry (ghcr.io)

Docker images are hosted on **GitHub Container Registry** (`ghcr.io`) as private packages.

- **Push**: GitHub Actions uses `GITHUB_TOKEN` with `packages:write` permission
- **Pull**: App Service sites use a GitHub PAT (`read:packages` scope) stored in Key Vault as
  `techhub-github-registry-token`

This replaces the previous Azure Container Registry (ACR Standard) at a saving of ~€20/month.

## ACME DNS Zone

A public Azure DNS zone (`acme.hub.ms`) is used for automated wildcard certificate renewal via
`certbot-dns-azure`. External DNS (GoDaddy) delegates `_acme-challenge` CNAMEs to this zone so
certbot can create/delete TXT records during renewal without touching GoDaddy DNS.

See [wildcard-certificates.md](wildcard-certificates.md) for details.

## PostgreSQL

Production has a permanent PostgreSQL Flexible Server. PR environments get ephemeral servers
created via PITR from the production backup — both in `rg-techhub-prod`.

- **Production**: `psql-techhub-prod` — permanent, public access with firewall rules; both password auth (for admin/emergency use) and Entra ID auth enabled
- **PR environments**: `psql-techhub-pr-{N}` — ephemeral, created via Point-in-Time Restore; Entra-only auth
- **Public access**: Enabled with firewall rules for admin IPs only
- **Private endpoint**: `pe-psql-techhub-prod` (and `pe-psql-techhub-pr-{N}` for PR servers) in
  `snet-private-endpoints`, resolved via the `privatelink.postgres.database.azure.com` Private
  DNS Zone linked to the VNet
- **App Service sites** reach PostgreSQL over the **private endpoint** — traffic stays on the VNet
  and never touches the public internet. This replaced a NAT Gateway (`natgw-techhub-prod`) that
  previously gave the compute layer a single stable outbound public IP for firewall allowlisting;
  the private endpoint removes both the NAT Gateway cost and the public-internet hop entirely.
- **Admin** reaches PostgreSQL via IP-allowlisted public access

> **Authentication**: The `id-techhub-prod` user-assigned managed identity is registered as the
> Entra administrator on `psql-techhub-prod`. The API site sets `Database:UseEntraAuth=true`
> and acquires Azure AD tokens at runtime via `DefaultAzureCredential` —
> no password in the connection string or Key Vault.
>
> **PR isolation**: All PR environments share one managed identity (`id-techhub-pr`), created
> once by `infrastructure.bicep` and registered as the Entra admin on each ephemeral PITR server.
> A PR site cannot authenticate against `psql-techhub-prod` because that server's Entra
> admin is `id-techhub-prod` — a completely separate identity. No shared credentials exist.

## Azure Communication Services (ACS)

The production ACS account (`acs-techhub-prod`) and email service (`eml-techhub-prod`) handle
outbound newsletter email delivery.

- **Location**: global (data residency: Europe)
- **Domain**: Azure-managed domain (`AzureManagedDomain`) — sender address is auto-generated by ACS (e.g. `DoNotReply@{guid}.azurecomm.net`)
- **Authentication**: RBAC — `ACS Data Contributor` role assigned to `id-techhub-prod`;
  the API acquires the token via `DefaultAzureCredential`. No ACS connection string or key is used.
- **Endpoint URL**: Stored in Key Vault as `techhub-prod-newsletter-acs-endpoint`. The URL is
  captured from the infrastructure Bicep output and synced to Key Vault by `Deploy-Infrastructure.ps1`
  via `Sync-KeyVaultSecrets.ps1` after every infrastructure deployment.
- **Sender address**: Stored in Key Vault as `techhub-prod-acs-sender-address`. Written directly
  by the Bicep template from the ACS domain output — no manual input required.

## AI Foundry (OpenAI)

The production AI Foundry account (`oai-techhub-prod`) is secured the same way as Key Vault and
PostgreSQL — no public access without an allowlisted admin IP, and App Service sites reach it over
a private endpoint.

- **Public access**: Enabled only when admin IPs are configured; admin IPs allowlisted via
  `networkAcls.ipRules`; default deny; `bypass: 'None'` (no trusted Microsoft services bypass)
- **Private endpoint**: `pe-oai-techhub-prod` in `snet-private-endpoints`. The account is kind
  `AIServices` (multi-service — "Foundry Tools" in Microsoft's private endpoint DNS reference),
  and the application calls the endpoint at `<account>.openai.azure.com` (see
  `AiCategorizationOptions.Endpoint`). Per Microsoft's documented DNS zone mapping for
  `Microsoft.CognitiveServices/accounts` (subresource `account`), the private endpoint's DNS zone
  group registers all three zones: `privatelink.openai.azure.com` (required — resolves the domain
  the app actually calls), `privatelink.cognitiveservices.azure.com`, and
  `privatelink.services.ai.azure.com` (both recommended, for other Cognitive Services/AI Foundry
  data-plane calls) — all linked to the VNet so App Service sites reach AI Foundry entirely over the
  VNet
- **Authentication**: RBAC — `Cognitive Services OpenAI User` role (`5e0bd9bd-7b93-4f28-af87-19fc36ad61bd`)
  assigned to `id-techhub-prod`. No API key is used; the application acquires an Entra token with
  `DefaultAzureCredential` and the `https://cognitiveservices.azure.com/.default` scope.

**Local development**: After `az login`, `DefaultAzureCredential` uses your user token
automatically, provided you have the `Cognitive Services OpenAI User` RBAC role and connect from
an allowlisted admin IP (`ADMIN_IP_ADDRESSES`) — AI Foundry no longer accepts unrestricted public
traffic. Find your object ID with: `az ad signed-in-user show --query id -o tsv`

## Monitoring (Application Insights / Log Analytics)

Unlike Key Vault, PostgreSQL, and AI Foundry, **Application Insights and Log Analytics have no
private endpoint / Azure Monitor Private Link Scope (AMPLS)**. Both `publicNetworkAccessForIngestion`
and `publicNetworkAccessForQuery` are left `Enabled`. This is deliberate, not an oversight:

- **Browser (RUM/client-side) telemetry requires public ingestion.** The App Insights JS SDK sends
  telemetry directly from end-user browsers, which cannot reach our VNet. An AMPLS cannot close
  this path, so ingestion has to stay public regardless of any other configuration.
- **Server-side telemetry gets little benefit from an AMPLS here.** App Service telemetry could
  move to a private path via AMPLS, but since ingestion must stay public anyway for the browser
  path above, the security improvement is marginal (only slightly less public egress) relative to
  the added cost/complexity (a private endpoint + DNS zones + AMPLS resource, ~$7-8/month).
- **Query access is intentionally public too**, protected by RBAC, so portal/admin users can query
  without VPN/jumpbox access. An AMPLS would only add value here if we also wanted to force query
  access through the VNet.

If this trade-off changes (e.g. compliance requirements demand private query, or admin access moves
behind a VPN), revisit adding an AMPLS following the same private-endpoint pattern used for Key
Vault/PostgreSQL/AI Foundry.

## Deploy Order

1. **Production** (`rg-techhub-prod`): VNet, monitoring, Key Vault, AI Foundry, App Service Plans
   (prod + PR-preview), wildcard certificates, PostgreSQL, API App Service site, Web App Service site,
   action group, ACME DNS zone, budget, policy

No shared or staging resource groups. PR preview environments are created on-demand within
`rg-techhub-prod` by `scripts/Deploy-PrPreview.ps1`.

## Implementation Reference

- Spoke VNet + private endpoint subnet/DNS zones: [infra/modules/network.bicep](../infra/modules/network.bicep)
- Key Vault: [infra/modules/keyVault.bicep](../infra/modules/keyVault.bicep)
- Log Analytics: [infra/modules/monitoring.bicep](../infra/modules/monitoring.bicep)
- PostgreSQL: [infra/modules/postgres.bicep](../infra/modules/postgres.bicep)
- Action Group: [infra/modules/actionGroup.bicep](../infra/modules/actionGroup.bicep)
- AI Foundry: [infra/modules/openai.bicep](../infra/modules/openai.bicep)
- Azure Communication Services: [infra/modules/communication.bicep](../infra/modules/communication.bicep)
- Infrastructure orchestration (Phase 1): [infra/infrastructure.bicep](../infra/infrastructure.bicep)
- Application orchestration (Phase 2): [infra/applications.bicep](../infra/applications.bicep)
