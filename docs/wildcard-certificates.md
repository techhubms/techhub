# Wildcard Certificates

Tech Hub uses **Let's Encrypt wildcard certificates** for `*.hub.ms` and `*.xebia.ms`, stored in Azure Key Vault (`kv-techhub-prod`). Renewal is automated via `certbot-dns-azure` using ACME DNS challenge delegation.

## Architecture

Wildcard certs require **DNS-01 validation** — Let's Encrypt must verify domain ownership by checking a TXT record at `_acme-challenge.<domain>`. Rather than giving certbot access to the entire DNS zone at GoDaddy, challenge records are **delegated to a small Azure DNS zone** (`acme.hub.ms`).

```text
Let's Encrypt asks:  _acme-challenge.hub.ms TXT ?
                           │
                           ▼
GoDaddy DNS:         _acme-challenge.hub.ms CNAME → hub-ms.acme.hub.ms
                           │
                           ▼
Azure DNS zone:      hub-ms.acme.hub.ms TXT → <certbot-generated-token>
```

This means:

- **GoDaddy stays under shared control** — other teams manage their own `xebia.ms` subdomains freely
- **Only two static CNAME records + NS delegation** are needed at GoDaddy (see [prerequisites](#prerequisites))
- **certbot-dns-azure** creates/deletes TXT records in Azure DNS automatically during renewal

## Prerequisites

### One-time GoDaddy setup

Add these CNAME records in GoDaddy DNS for each domain:

| Domain | GoDaddy Record | Type | Value |
|--------|---------------|------|-------|
| `hub.ms` | `_acme-challenge` | CNAME | `hub-ms.acme.hub.ms` |
| `xebia.ms` | `_acme-challenge` | CNAME | `xebia-ms.acme.hub.ms` |

These are permanent — they never need updating.

### One-time GoDaddy NS delegation

After deploying the infrastructure (which creates the `acme.hub.ms` zone), add NS records for `acme` in the `hub.ms` zone at GoDaddy pointing to the Azure DNS nameservers:

```bash
# Get the nameservers after deploying infra
az network dns zone show --resource-group rg-techhub-prod --name acme.hub.ms --query nameServers -o tsv
```

Add each returned nameserver as an NS record for `acme` in GoDaddy's `hub.ms` zone.

### Tool installation

```bash
pip install certbot certbot-dns-azure
```

## Azure Infrastructure

The ACME DNS zone is deployed as part of shared infrastructure:

- **Bicep module**: `infra/modules/acmeDnsZone.bicep`
- **Deployed by**: `infra/infrastructure.bicep` → `acmeDnsZone` module
- **Resource**: `Microsoft.Network/dnsZones` — `acme.hub.ms` in `rg-techhub-prod`

Deploy with:

```bash
./scripts/Deploy-Infrastructure.ps1 -Environment shared -Mode deploy
```

### App Service integration

Wildcard certificates from Key Vault are imported into the production App Service Plan and bound to custom domains on each site (API + Web):

- **Certificate loading**: `infra/modules/wildcardCert.bicep` imports the PFX from Key Vault into `Microsoft.Web/certificates`, scoped to the App Service Plan
- **Key Vault access**: `infra/modules/kvSecretsUserRole.bicep` grants the managed identity Key Vault Secrets User role so it can read certificate secrets
- **Domain binding**: `infra/modules/web.bicep` uses `sslState: 'SniEnabled'` with the wildcard cert thumbprint for all custom domains. Every domain must have a matching wildcard certificate configured — there is no automatic fallback. If a certificate is missing for a domain, deployment will fail, ensuring misconfigurations are caught immediately
- **Configuration**: Set `wildcardCertNames` in environment parameter files (e.g., `{ "hub.ms": "wildcard-hub-ms" }`)

> **Note**: `wildcardCert.bicep` is intentionally **not** part of the regular `infrastructure.bicep`/`applications.bicep` deploy cycle — `Microsoft.Web/certificates` resources only re-read the Key Vault secret at deploy time (no auto-refresh on KV secret rotation), so it must be redeployed standalone whenever a new PFX is written to Key Vault. `Renew-WildcardCertificates.ps1` does this automatically after each renewal.

## Certificate Renewal

### Automated renewal

```bash
./scripts/Renew-WildcardCertificates.ps1 -Email admin@xebia.com
```

The script:

1. Validates prerequisites (Azure CLI, certbot, DNS zone)
2. Generates certbot-dns-azure configuration
3. Runs certbot for each domain (`*.hub.ms` + `hub.ms`, `*.xebia.ms` + `xebia.ms`)
4. Converts PEM certificates to PFX format
5. Temporarily opens the Key Vault firewall for the machine's outbound IP
6. Writes PFX as a secret into Key Vault (`kv-techhub-prod`)
7. Redeploys `infra/modules/wildcardCert.bicep` per domain to import the renewed PFX into the App Service Plan
8. Restores the Key Vault firewall and cleans up temporary files

> **Note on App Service cert update**: `Microsoft.Web/certificates` reads the Key Vault secret only at deploy time — it does not auto-refresh when the KV secret is rotated. That's why the renewal script redeploys `wildcardCert.bicep` per domain after writing the new PFX to Key Vault, rather than relying on the certificate resource to pick up the change on its own.

### Dry run (test without issuing certificates)

```bash
./scripts/Renew-WildcardCertificates.ps1 -Email admin@xebia.com -DryRun
```

### Force renewal

```bash
./scripts/Renew-WildcardCertificates.ps1 -Email admin@xebia.com -Force
```

### Network access

Key Vault firewall blocks public IPs by default (`publicNetworkAccess: Disabled`, `defaultAction: Deny`). The renewal script automatically detects the machine's outbound IP, adds a temporary firewall rule, polls until it propagates, then removes it in a `finally` block. No VPN access or manual firewall changes are needed.

## Certificate Details

| Certificate | Key Vault Name | SANs | Validity |
|-------------|---------------|------|----------|
| `*.hub.ms` | `wildcard-hub-ms` | `*.hub.ms`, `hub.ms` | 90 days |
| `*.xebia.ms` | `wildcard-xebia-ms` | `*.xebia.ms`, `xebia.ms` | 90 days |

## Renewal Schedule

Let's Encrypt certificates expire after **90 days**. Renewal should run every **60 days** to allow a 30-day buffer. This can be scheduled via GitHub Actions or Azure Automation.
