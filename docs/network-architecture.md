# Network Architecture

Tech Hub uses a **single VNet** in the production resource group. Key Vault, AI Foundry, and
PostgreSQL are all reached over **private endpoints** in a dedicated subnet — Container Apps
traffic to these services never touches the public internet. Admin access to each service goes
through public access with IP-based firewall rules instead.

## Topology

```text
Internet
    │
    ├── Web frontend (ca-techhub-web-prod) — public ingress (external: true)
    │       HTTPS on tech.hub.ms, tech.xebia.ms (wildcard TLS from kv-techhub-prod)
    │
    └── Admin IP (allowlisted for Key Vault, PostgreSQL, and AI Foundry)

Prod VNet — vnet-techhub-prod (10.2.0.0/16) [rg-techhub-prod]
    ├── snet-container-apps (10.2.0.0/23) — Container Apps Environment (internal: false)
    │    │
    │    ├── ca-techhub-web-prod   [external: true]  ← reachable from internet
    │    │
    │    └── ca-techhub-api-prod   [external: false] ← internal only, NOT reachable from internet
    │            (Web frontend calls API over the internal Container Apps environment network)
    │
    └── snet-private-endpoints (10.2.2.0/27) — private endpoint NICs only
         │
         ├── pe-psql-techhub-prod → psql-techhub-prod
         │       → Container Apps reach PostgreSQL over the VNet via
         │         privatelink.postgres.database.azure.com (Private DNS Zone)
         │
         ├── pe-kv-techhub-prod → kv-techhub-prod
         │       → Container Apps reach Key Vault over the VNet via
         │         privatelink.vaultcore.azure.net (Private DNS Zone)
         │
         └── pe-oai-techhub-prod → oai-techhub-prod
                 → Container Apps reach AI Foundry over the VNet via
                   privatelink.cognitiveservices.azure.com (Private DNS Zone)
```

## Address Spaces

| VNet | CIDR | Resource Group | Purpose |
|------|------|----------------|---------|
| `vnet-techhub-prod` | `10.2.0.0/16` | `rg-techhub-prod` | Container Apps + private endpoints (prod + PR previews) |

There are two subnets:

- `snet-container-apps` (`10.2.0.0/23`), delegated to `Microsoft.App/environments`
- `snet-private-endpoints` (`10.2.2.0/27`), hosts the PostgreSQL, Key Vault, and AI Foundry
  private endpoint NICs — private endpoints cannot share a subnet delegated to
  `Microsoft.App/environments`

No hub-spoke peering.

## Container Apps Ingress

The Container Apps Environment (`cae-techhub-prod`) is deployed with `internal: false`, meaning
it has a public IP. However, each Container App controls its own ingress independently.

| App | Ingress | Reachable from internet | Notes |
|-----|---------|------------------------|-------|
| `ca-techhub-web-prod` | `external: true` | **Yes** | Custom domains (`tech.hub.ms`, `tech.xebia.ms`); wildcard TLS from Key Vault |
| `ca-techhub-api-prod` | `external: false` | **No** | Internal only; the Web frontend calls the API over the internal Container Apps environment network |

The API backend is intentionally not publicly accessible. No path to the API exists from the
internet — not via the custom domains, not via the default Container Apps FQDN. The Web Blazor
frontend calls the API exclusively over the internal environment network (server-side rendering
and SSR API calls stay within the Container Apps environment).

CORS policy is configured on the API app and restricts allowed origins to the configured
`primaryHosts` (i.e. `tech.hub.ms`, `tech.xebia.ms`), so even if the API were made external,
cross-origin browser requests from unexpected origins would be blocked.

Admin access to Azure resources is controlled via per-resource IP firewall rules using the
`ADMIN_IP_ADDRESSES` environment variable (supports multiple comma-separated IPs).

| Resource | Firewall Mechanism | Access |
|----------|-------------------|--------|
| Key Vault | Private endpoint (VNet traffic) + `networkAcls.ipRules` (admin traffic) | Container Apps via private endpoint; admin IPs via public access; default deny, no trusted-services bypass |
| PostgreSQL | Private endpoint (VNet traffic) + per-IP/range firewall rules (admin traffic) | Container Apps via private endpoint; admin IPs via public access; default deny otherwise |
| AI Foundry | Private endpoint (VNet traffic) + `networkAcls.ipRules` (admin traffic) | Container Apps via private endpoint; admin IPs via public access; default deny, no trusted-services bypass |
| Log Analytics | Public ingestion + query enabled | RBAC-protected |
| App Insights | Public ingestion + query enabled | RBAC-protected; browser JS SDK over public internet |

## Key Vault

The production Key Vault (`kv-techhub-prod`) stores:

- Wildcard TLS certificates for `*.hub.ms` and `*.xebia.ms`
- AAD client secret (`techhub-prod-aad-client-secret`) for the admin dashboard
- GitHub registry token (`techhub-github-registry-token`) for Container Apps pulling from ghcr.io
- PostgreSQL admin password (`techhub-prod-postgres-admin-password`) for infrastructure management
- Newsletter ACS endpoint URL (`techhub-prod-newsletter-acs-endpoint`) — written automatically by `Sync-KeyVaultSecrets.ps1` from infra deployment outputs
- Newsletter ACS sender address (`techhub-prod-acs-sender-address`) — written directly by Bicep from the ACS domain output
- Newsletter unsubscribe HMAC key (`techhub-prod-newsletter-unsubscribe-secret`) for signing unsubscribe/confirm links

Security:

- **Public access**: Enabled only when admin IPs are configured; admin IPs allowlisted via
  `networkAcls.ipRules`; default deny; `bypass: 'None'` (no trusted Microsoft services bypass)
- **Private endpoint**: `pe-kv-techhub-prod` in `snet-private-endpoints`, resolved via the
  `privatelink.vaultcore.azure.net` Private DNS Zone linked to the VNet — Container Apps reach
  Key Vault entirely over the VNet, replacing the previous VNet service endpoint
- **Authorization**: RBAC (Key Vault Administrator role for admins; Key Vault Secrets User for
  the managed identity used by Container Apps)

> **Note:** PostgreSQL connection strings and AI Foundry API keys are no longer stored in Key
> Vault. The application uses managed identity token auth for both services (see below).

## Container Registry (ghcr.io)

Docker images are hosted on **GitHub Container Registry** (`ghcr.io`) as private packages.

- **Push**: GitHub Actions uses `GITHUB_TOKEN` with `packages:write` permission
- **Pull**: Container Apps use a GitHub PAT (`read:packages` scope) stored in Key Vault as
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
- **Container Apps** reach PostgreSQL over the **private endpoint** — traffic stays on the VNet
  and never touches the public internet. This replaced a NAT Gateway (`natgw-techhub-prod`) that
  previously gave Container Apps a single stable outbound public IP for firewall allowlisting;
  the private endpoint removes both the NAT Gateway cost and the public-internet hop entirely.
- **Admin** reaches PostgreSQL via IP-allowlisted public access

> **Authentication**: The `id-techhub-prod` user-assigned managed identity is registered as the
> Entra administrator on `psql-techhub-prod`. The Container App sets `Database:UseEntraAuth=true`
> and acquires Azure AD tokens at runtime via `DefaultAzureCredential` —
> no password in the connection string or Key Vault.
>
> **PR isolation**: All PR environments share one managed identity (`id-techhub-pr`), created
> once by `infrastructure.bicep` and registered as the Entra admin on each ephemeral PITR server.
> A PR container cannot authenticate against `psql-techhub-prod` because that server's Entra
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
PostgreSQL — no public access without an allowlisted admin IP, and Container Apps reach it over
a private endpoint.

- **Public access**: Enabled only when admin IPs are configured; admin IPs allowlisted via
  `networkAcls.ipRules`; default deny; `bypass: 'None'` (no trusted Microsoft services bypass)
- **Private endpoint**: `pe-oai-techhub-prod` in `snet-private-endpoints`, resolved via the
  `privatelink.cognitiveservices.azure.com` Private DNS Zone linked to the VNet — Container Apps
  reach AI Foundry entirely over the VNet
- **Authentication**: RBAC — `Cognitive Services OpenAI User` role (`5e0bd9bd-7b93-4f28-af87-19fc36ad61bd`)
  assigned to `id-techhub-prod`. No API key is used; the application acquires an Entra token with
  `DefaultAzureCredential` and the `https://cognitiveservices.azure.com/.default` scope.

**Local development**: After `az login`, `DefaultAzureCredential` uses your user token
automatically, provided you have the `Cognitive Services OpenAI User` RBAC role and connect from
an allowlisted admin IP (`ADMIN_IP_ADDRESSES`) — AI Foundry no longer accepts unrestricted public
traffic. Find your object ID with: `az ad signed-in-user show --query id -o tsv`

## Deploy Order

1. **Production** (`rg-techhub-prod`): VNet, monitoring, Key Vault, AI Foundry, Container Apps
   Environment, wildcard certificates, PostgreSQL, API Container App, Web Container App,
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
