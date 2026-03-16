# Network Architecture

Tech Hub uses a **hub-spoke VNet topology** with private endpoints for all data services. No databases or secrets are exposed to the public internet.

## Topology

```text
VPN Client (172.16.0.0/24)
    │
    ▼
Hub VNet — vnet-techhub-hub (10.100.0.0/16) [rg-techhub-shared]
    │   ├── GatewaySubnet (10.100.0.0/24) — VPN Gateway (P2S, Azure AD auth)
    │   └── snet-private-endpoints (10.100.1.0/24) — Key Vault PE
    │
    ├── Peering ──► Staging Spoke VNet — vnet-techhub-staging (10.1.0.0/16) [rg-techhub-staging]
    │                   ├── snet-container-apps (10.1.0.0/23) — Container Apps Environment
    │                   └── snet-private-endpoints (10.1.2.0/24) — PostgreSQL PE
    │
    └── Peering ──► Prod Spoke VNet — vnet-techhub-prod (10.2.0.0/16) [rg-techhub-prod]
                        ├── snet-container-apps (10.2.0.0/23) — Container Apps Environment
                        └── snet-private-endpoints (10.2.2.0/24) — PostgreSQL PE
```

## Address Spaces

| VNet | CIDR | Resource Group | Purpose |
|------|------|----------------|---------|
| `vnet-techhub-hub` | `10.100.0.0/16` | `rg-techhub-shared` | VPN Gateway, Key Vault PE |
| `vnet-techhub-staging` | `10.1.0.0/16` | `rg-techhub-staging` | Container Apps, PostgreSQL PE |
| `vnet-techhub-prod` | `10.2.0.0/16` | `rg-techhub-prod` | Container Apps, PostgreSQL PE |
| VPN clients | `172.16.0.0/24` | — | P2S address pool |

Address spaces are deliberately non-overlapping to support VNet peering.

## VNet Peering

Each spoke VNet has bidirectional peering with the hub:

- **Hub → Spoke**: `allowGatewayTransit: true` — lets VPN traffic flow to spoke resources
- **Spoke → Hub**: `useRemoteGateways: true` — routes VPN client traffic through the hub gateway

The spoke-to-hub peering depends on hub-to-spoke being established first.

## Private Endpoints

All data services use private endpoints with `publicNetworkAccess: Disabled`.

| Resource | PE Location | DNS Zone | Linked VNets |
|----------|-------------|----------|--------------|
| Key Vault (`kv-techhub-shared`) | Hub VNet | `privatelink.vaultcore.azure.net` | Hub + all spokes |
| PostgreSQL Staging (`psql-techhub-staging`) | Staging VNet | `privatelink.postgres.database.azure.com` | Staging + Hub |
| PostgreSQL Prod (`psql-techhub-prod`) | Prod VNet | `privatelink.postgres.database.azure.com` | Prod + Hub |

Private DNS zones are linked to both the spoke VNet (for Container Apps access) and the hub VNet (for VPN client access).

## VPN Gateway

A Point-to-Site VPN gateway in the hub VNet provides admin access to all private resources.

- **Authentication**: Microsoft Entra ID using the Microsoft-registered App ID (no manual app registration needed)
- **Protocol**: OpenVPN
- **SKU**: VpnGw2 (Generation2)
- **Client platforms**: Windows, macOS, Linux (all supported via Azure VPN Client)

From the VPN, traffic reaches spoke resources via hub-spoke peering.

## Key Vault

The shared Key Vault stores wildcard TLS certificates used by both staging and production Container Apps.

- **Public access**: Disabled
- **Network ACLs**: Default deny, bypass none
- **Access**: Private endpoint in hub VNet only
- **Authorization**: RBAC (Key Vault Administrator role assigned to specific Azure AD object IDs)
- **Certificates**: Wildcard certs for `*.hub.ms` and `*.xebia.ms` — see [wildcard-certificates.md](wildcard-certificates.md)

## ACME DNS Zone

A public Azure DNS zone (`acme.hub.ms`) is used for automated wildcard certificate renewal via `certbot-dns-azure`. External DNS (GoDaddy) delegates `_acme-challenge` CNAMEs to this zone so certbot can create/delete TXT records during renewal without touching GoDaddy DNS. See [wildcard-certificates.md](wildcard-certificates.md) for details.

## PostgreSQL

Each environment has its own PostgreSQL Flexible Server.

- **Public access**: Disabled
- **Access**: Private endpoint in the environment's spoke VNet
- **Container Apps** reach PostgreSQL through the shared spoke VNet
- **VPN clients** reach PostgreSQL through hub → spoke peering with DNS zone linked to hub

## DNS Resolution

Private DNS zones ensure all consumers can resolve private endpoint IPs:

| Zone | Created By | Linked To |
|------|-----------|-----------|
| `privatelink.vaultcore.azure.net` | KV PE module (shared RG) | Hub VNet + each spoke VNet |
| `privatelink.postgres.database.azure.com` | Postgres DNS zone module (shared RG) | Hub VNet + each spoke VNet |

This means both Container Apps (in spokes) and VPN clients (via hub) resolve the correct private IPs.

## Deploy Order

1. **Shared** (`rg-techhub-shared`): ACR, Key Vault, Hub VNet, VPN Gateway, KV Private Endpoint, ACME DNS Zone, PostgreSQL Private DNS Zone
2. **Staging/Production** (`rg-techhub-staging`, `rg-techhub-prod`): VNet, peering, Container Apps, PostgreSQL, PostgreSQL PE, KV DNS zone link, PostgreSQL DNS zone link

Shared must be deployed first — spoke deployments reference the hub VNet ID for peering.

## Implementation Reference

- Hub VNet + VPN Gateway: [infra/modules/hubNetwork.bicep](../infra/modules/hubNetwork.bicep)
- Spoke VNet: [infra/modules/network.bicep](../infra/modules/network.bicep)
- VNet Peering: [infra/modules/vnetPeering.bicep](../infra/modules/vnetPeering.bicep)
- Key Vault: [infra/modules/keyVault.bicep](../infra/modules/keyVault.bicep)
- Key Vault PE: [infra/modules/keyVaultPrivateEndpoint.bicep](../infra/modules/keyVaultPrivateEndpoint.bicep)
- PostgreSQL: [infra/modules/postgres.bicep](../infra/modules/postgres.bicep)
- PostgreSQL PE: [infra/modules/postgresPrivateEndpoint.bicep](../infra/modules/postgresPrivateEndpoint.bicep)
- PostgreSQL DNS Zone: [infra/modules/postgresDnsZone.bicep](../infra/modules/postgresDnsZone.bicep)
- DNS Zone Link: [infra/modules/privateDnsZoneLink.bicep](../infra/modules/privateDnsZoneLink.bicep)
- Shared orchestration: [infra/shared.bicep](../infra/shared.bicep)
- Environment orchestration: [infra/main.bicep](../infra/main.bicep)
