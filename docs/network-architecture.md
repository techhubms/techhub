# Network Architecture

Tech Hub uses a **single VNet** in the production resource group. All application services
(Container Apps, PostgreSQL, Key Vault, AI Foundry) communicate over the public internet where
needed, secured by API keys, managed identity, IP firewall rules, and a Key Vault VNet service
endpoint.

## Topology

```text
Admin IP (firewall allowlisted)
    │
    ▼
Prod VNet — vnet-techhub-prod (10.2.0.0/16) [rg-techhub-prod]
    └── snet-container-apps (10.2.0.0/23) — Container Apps Environment (prod + PR previews)
         │
         ├── Key Vault service endpoint (Microsoft.KeyVault)
         │       → Container Apps reach kv-techhub-prod over Microsoft backbone
         │
         ├── PostgreSQL firewall rule (10.2.0.0–10.2.1.255)
         │       → Container Apps reach psql-techhub-prod over public internet
         │
         └── AI Foundry — open public access, API key authentication
```

## Address Spaces

| VNet | CIDR | Resource Group | Purpose |
|------|------|----------------|---------|
| `vnet-techhub-prod` | `10.2.0.0/16` | `rg-techhub-prod` | Container Apps (prod + PR previews) |

There is a single subnet: `snet-container-apps` (`10.2.0.0/23`), delegated to
`Microsoft.App/environments`. No private endpoints or hub-spoke peering.

## IP Firewall Rules

Admin access to Azure resources is controlled via per-resource IP firewall rules using the
`ADMIN_IP_ADDRESSES` environment variable (supports multiple comma-separated IPs).

| Resource | Firewall Mechanism | Access |
|----------|-------------------|--------|
| Key Vault | `networkAcls.ipRules` + VNet service endpoint | Admin IPs + Container Apps subnet; default deny |
| PostgreSQL | Per-IP/range firewall rules | Admin IPs + Container Apps subnet (10.2.0.0–10.2.1.255); default deny |
| Log Analytics | Public ingestion + query enabled | RBAC-protected |
| App Insights | Public ingestion + query enabled | RBAC-protected; browser JS SDK over public internet |
| AI Foundry | Public access open | API key authentication; no IP restriction needed |

## Key Vault

The production Key Vault (`kv-techhub-prod`) stores:

- Wildcard TLS certificates for `*.hub.ms` and `*.xebia.ms`
- Application secrets (PostgreSQL connection string, AI API key, AAD client secret)
- GitHub registry token (`techhub-github-registry-token`) for Container Apps pulling from ghcr.io

Security:

- **Public access**: Enabled (admin IPs allowlisted via `networkAcls.ipRules`; default deny)
- **VNet service endpoint**: `Microsoft.KeyVault` on the Container Apps subnet allows Container
  Apps to reach Key Vault over the Microsoft backbone without a private endpoint
- **Authorization**: RBAC (Key Vault Administrator role for admins; Key Vault Secrets User for
  the managed identity used by Container Apps)

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

- **Production**: `psql-techhub-prod` — permanent, public access with firewall rules; password auth
- **PR environments**: `psql-techhub-pr-{N}` — ephemeral, created via Point-in-Time Restore; Entra-only auth
- **Public access**: Enabled with firewall rules for admin IPs and Container Apps subnet
- **Firewall**: Admin IP rules + Container Apps subnet range (10.2.0.0–10.2.1.255)
- **Container Apps** reach PostgreSQL over the public internet (source IP is in the CA subnet range)
- **Admin** reaches PostgreSQL via IP-allowlisted public access

> **PR isolation**: Each PR gets a dedicated user-assigned managed identity
> (`id-techhub-pr-{N}`) that is registered as the sole Entra admin on its own PITR server.
> Password authentication is disabled on PR servers. A PR container cannot authenticate against
> `psql-techhub-prod` because that server's Entra admin is `id-techhub-prod` — a completely
> separate identity. No shared credentials exist.

## AI Foundry (OpenAI)

The production AI Foundry account (`oai-techhub-prod`) is publicly accessible.

- **Public access**: Enabled with `defaultAction: Allow` — Container Apps and GitHub Actions
  runners both reach the endpoint over the public internet
- **Authentication**: API key stored in Key Vault

## Deploy Order

1. **Production** (`rg-techhub-prod`): VNet, monitoring, Key Vault, AI Foundry, Container Apps
   Environment, wildcard certificates, PostgreSQL, API Container App, Web Container App,
   action group, ACME DNS zone, budget, policy

No shared or staging resource groups. PR preview environments are created on-demand within
`rg-techhub-prod` by `scripts/Deploy-PrPreview.ps1`.

## Implementation Reference

- Spoke VNet: [infra/modules/network.bicep](../infra/modules/network.bicep)
- Key Vault: [infra/modules/keyVault.bicep](../infra/modules/keyVault.bicep)
- Log Analytics (per-env): [infra/modules/monitoring.bicep](../infra/modules/monitoring.bicep)
- PostgreSQL: [infra/modules/postgres.bicep](../infra/modules/postgres.bicep)
- Action Group: [infra/modules/actionGroup.bicep](../infra/modules/actionGroup.bicep)
- AI Foundry: [infra/modules/openai.bicep](../infra/modules/openai.bicep)
- Environment orchestration: [infra/main.bicep](../infra/main.bicep)
