# Network Architecture

Tech Hub uses a **hub-spoke VNet topology** with private endpoints for all data services. Admin access is controlled via Network Security Perimeter (NSP) with IP allowlisting and per-resource firewall rules.

## Topology

```text
Admin IP (NSP allowlisted)
    │
    ▼
Hub VNet — vnet-techhub-hub (10.100.0.0/16) [rg-techhub-shared]
    │   └── snet-private-endpoints (10.100.1.0/24) — Key Vault PE, AMPLS PE
    │
    ├── Peering ──► Staging Spoke VNet — vnet-techhub-staging (10.1.0.0/16) [rg-techhub-staging]
    │                   ├── snet-container-apps (10.1.0.0/23) — Container Apps Environment
    │                   └── snet-private-endpoints (10.1.2.0/24) — PostgreSQL PE, AI Foundry PE
    │
    └── Peering ──► Prod Spoke VNet — vnet-techhub-prod (10.2.0.0/16) [rg-techhub-prod]
                        ├── snet-container-apps (10.2.0.0/23) — Container Apps Environment
                        └── snet-private-endpoints (10.2.2.0/24) — PostgreSQL PE, AI Foundry PE
```

## Address Spaces

| VNet | CIDR | Resource Group | Purpose |
|------|------|----------------|---------|
| `vnet-techhub-hub` | `10.100.0.0/16` | `rg-techhub-shared` | Key Vault PE, AMPLS PE |
| `vnet-techhub-staging` | `10.1.0.0/16` | `rg-techhub-staging` | Container Apps, PostgreSQL PE, AI Foundry PE |
| `vnet-techhub-prod` | `10.2.0.0/16` | `rg-techhub-prod` | Container Apps, PostgreSQL PE, AI Foundry PE |

Address spaces are deliberately non-overlapping to support VNet peering.

## VNet Peering

Each spoke VNet has bidirectional peering with the hub:

- **Hub → Spoke**: `allowGatewayTransit: false` — peering for private endpoint resolution
- **Spoke → Hub**: `useRemoteGateways: false` — no gateway in hub

The spoke-to-hub peering depends on hub-to-spoke being established first.

## Network Security Perimeter (NSP)

A single NSP (`nsp-techhub`) in the shared resource group controls public access to NSP-supported resources. Admin access is via IP allowlisting; same-subscription Azure services are allowed implicitly.

```text
NSP: nsp-techhub (rg-techhub-shared)
└── Profile: profile-techhub
    ├── Inbound: allow-admin-ip  → addressPrefixes: ["<ADMIN_IP>/32"]
    └── Inbound: allow-subscription → subscriptions: [{id: "<subscription-id>"}]

Associated resources:
  - Key Vault (shared)
  - App Insights (shared, staging, prod)
  - Log Analytics (shared, staging, prod)
  - AI Foundry (staging, prod)
```

Resources that do **not** support NSP (PostgreSQL) use per-resource IP firewall rules instead.

## Azure Monitor Private Link Scope (AMPLS)

AMPLS routes app telemetry privately through the hub VNet. All Application Insights and Log Analytics workspaces (shared, staging, prod) are scoped to `ampls-techhub`.

- **Access mode**: Open (allows both private and public ingestion/query)
- **Private endpoint**: In hub VNet `snet-private-endpoints` subnet
- **DNS zones**: 5 AMPLS-specific private DNS zones linked to hub VNet

## Private Endpoints

Data services use private endpoints. Key Vault and AI Foundry use NSP for public admin access; PostgreSQL uses IP firewall rules.

| Resource | PE Location | DNS Zone | Linked VNets |
|----------|-------------|----------|--------------|
| Key Vault (`kv-techhub-shared`) | Hub VNet | `privatelink.vaultcore.azure.net` | Hub + all spokes |
| PostgreSQL Staging (`psql-techhub-staging`) | Staging VNet | `privatelink.postgres.database.azure.com` | Staging + Hub |
| PostgreSQL Prod (`psql-techhub-prod`) | Prod VNet | `privatelink.postgres.database.azure.com` | Prod + Hub |
| AI Foundry Staging (`oai-techhub-staging`) | Staging VNet | `privatelink.cognitiveservices.azure.com`, `privatelink.openai.azure.com`, `privatelink.services.ai.azure.com` | Staging |
| AI Foundry Prod (`oai-techhub-prod`) | Prod VNet | (same 3 zones) | Prod |
| AMPLS (`ampls-techhub`) | Hub VNet | 5 monitor DNS zones | Hub |

Private DNS zones are linked to the appropriate VNets for name resolution.

## Key Vault

The shared Key Vault stores wildcard TLS certificates used by both staging and production Container Apps.

- **Public access**: Enabled (controlled by NSP — only admin IP allowed)
- **Network ACLs**: Default allow, bypass Azure services
- **Access**: Private endpoint in hub VNet + NSP-controlled public access
- **Authorization**: RBAC (Key Vault Administrator role assigned to specific Azure AD object IDs)
- **Certificates**: Wildcard certs for `*.hub.ms` and `*.xebia.ms` — see [wildcard-certificates.md](wildcard-certificates.md)

## ACME DNS Zone

A public Azure DNS zone (`acme.hub.ms`) is used for automated wildcard certificate renewal via `certbot-dns-azure`. External DNS (GoDaddy) delegates `_acme-challenge` CNAMEs to this zone so certbot can create/delete TXT records during renewal without touching GoDaddy DNS. See [wildcard-certificates.md](wildcard-certificates.md) for details.

## PostgreSQL

Each environment has its own PostgreSQL Flexible Server.

- **Public access**: Enabled with admin IP firewall rule (PostgreSQL does not support NSP)
- **Firewall**: Single admin IP rule — all other public access denied
- **Access**: Private endpoint in the environment's spoke VNet
- **Container Apps** reach PostgreSQL through the spoke VNet private endpoint
- **Admin** reaches PostgreSQL via IP-allowlisted public access

## AI Foundry (OpenAI)

Each environment has its own AI Foundry (Cognitive Services) account.

- **Public access**: Enabled (controlled by NSP — only admin IP allowed)
- **Private endpoint**: In each spoke VNet for Container Apps to use a private path
- **DNS zones**: 3 zones per spoke (`privatelink.cognitiveservices.azure.com`, `privatelink.openai.azure.com`, `privatelink.services.ai.azure.com`)

## DNS Resolution

Private DNS zones ensure all consumers can resolve private endpoint IPs:

| Zone | Created By | Linked To |
|------|-----------|-----------|
| `privatelink.vaultcore.azure.net` | KV PE module (shared RG) | Hub VNet + each spoke VNet |
| `privatelink.postgres.database.azure.com` | Postgres DNS zone module (shared RG) | Hub VNet + each spoke VNet |
| `privatelink.cognitiveservices.azure.com` | AI Foundry PE module (per env RG) | Spoke VNet |
| `privatelink.openai.azure.com` | AI Foundry PE module (per env RG) | Spoke VNet |
| `privatelink.services.ai.azure.com` | AI Foundry PE module (per env RG) | Spoke VNet |
| `privatelink.monitor.azure.com` | AMPLS module (shared RG) | Hub VNet |
| `privatelink.oms.opinsights.azure.com` | AMPLS module (shared RG) | Hub VNet |
| `privatelink.ods.opinsights.azure.com` | AMPLS module (shared RG) | Hub VNet |
| `privatelink.agentsvc.azure-automation.net` | AMPLS module (shared RG) | Hub VNet |
| `privatelink.blob.core.windows.net` | AMPLS module (shared RG) | Hub VNet |

## Deploy Order

1. **Shared** (`rg-techhub-shared`): ACR, Key Vault, Hub VNet, KV Private Endpoint, ACME DNS Zone, PostgreSQL Private DNS Zone, NSP, AMPLS
2. **Staging/Production** (`rg-techhub-staging`, `rg-techhub-prod`): VNet, peering, Container Apps, PostgreSQL, PostgreSQL PE, AI Foundry PE, KV DNS zone link, PostgreSQL DNS zone link, NSP associations, AMPLS scoping

Shared must be deployed first — spoke deployments reference the hub VNet ID for peering.

## Implementation Reference

- Hub VNet: [infra/modules/hubNetwork.bicep](../infra/modules/hubNetwork.bicep)
- Spoke VNet: [infra/modules/network.bicep](../infra/modules/network.bicep)
- VNet Peering: [infra/modules/vnetPeering.bicep](../infra/modules/vnetPeering.bicep)
- Key Vault: [infra/modules/keyVault.bicep](../infra/modules/keyVault.bicep)
- Key Vault PE: [infra/modules/keyVaultPrivateEndpoint.bicep](../infra/modules/keyVaultPrivateEndpoint.bicep)
- PostgreSQL: [infra/modules/postgres.bicep](../infra/modules/postgres.bicep)
- PostgreSQL PE: [infra/modules/postgresPrivateEndpoint.bicep](../infra/modules/postgresPrivateEndpoint.bicep)
- PostgreSQL DNS Zone: [infra/modules/postgresDnsZone.bicep](../infra/modules/postgresDnsZone.bicep)
- DNS Zone Link: [infra/modules/privateDnsZoneLink.bicep](../infra/modules/privateDnsZoneLink.bicep)
- NSP: [infra/modules/networkSecurityPerimeter.bicep](../infra/modules/networkSecurityPerimeter.bicep)
- NSP Association: [infra/modules/nspAssociation.bicep](../infra/modules/nspAssociation.bicep)
- AMPLS: [infra/modules/monitorPrivateLink.bicep](../infra/modules/monitorPrivateLink.bicep)
- AMPLS Scope: [infra/modules/amplsScope.bicep](../infra/modules/amplsScope.bicep)
- AI Foundry PE: [infra/modules/openAiPrivateEndpoint.bicep](../infra/modules/openAiPrivateEndpoint.bicep)
- Shared orchestration: [infra/shared.bicep](../infra/shared.bicep)
- Environment orchestration: [infra/main.bicep](../infra/main.bicep)
