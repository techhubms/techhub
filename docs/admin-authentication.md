# Admin Authentication

Admin features (dashboard, content processing controls) are protected by Microsoft Entra ID (Azure AD) authentication. The system uses a single app registration shared between the Web frontend and the API backend.

## Architecture

The authentication flow uses two token types:

1. **ID token** — Authenticates the user to the Web app via OpenID Connect (OIDC)
2. **Access token** — Acquired by the Web app and forwarded as a Bearer token to the API

The Web app signs users in via OIDC, then uses `ITokenAcquisition` to obtain an access token scoped to the API. The API validates the token's audience (client ID) and scope claim.

### Token Flow

1. User navigates to `/admin` → Web app redirects to Entra ID login
2. Entra ID authenticates user → Returns ID token + authorization code to Web app
3. Web app exchanges authorization code for an access token (with `Admin.Access` scope)
4. Web app calls API with `Authorization: Bearer <access-token>` header
5. API validates the token audience and `scp` claim

### Key Components

- **`AdminTokenDelegatingHandler`** — HTTP delegating handler that acquires access tokens via `ITokenAcquisition` and attaches them to admin API requests
- **`AdminOnly` policy** — Authorization policy on the API that requires `RequireAuthenticatedUser()` + `RequireScope()`
- **Enterprise App** — "Assignment required" is enabled so only explicitly assigned users can sign in

## Configuration

Both the Web and API projects read from the `AzureAd` configuration section. When `AzureAd:ClientId` is empty, authentication is disabled and admin endpoints allow all requests (local dev without Entra).

### Web App (`src/TechHub.Web/appsettings.json`)

| Key | Description |
|-----|-------------|
| `AzureAd:Instance` | Login endpoint (`https://login.microsoftonline.com/`) |
| `AzureAd:TenantId` | Directory (tenant) ID |
| `AzureAd:ClientId` | Application (client) ID |
| `AzureAd:ClientSecret` | Client secret for OIDC confidential client flow |
| `AzureAd:CallbackPath` | OIDC redirect path (`/signin-oidc`) |
| `AzureAd:SignedOutCallbackPath` | Post-logout redirect path (`/signout-callback-oidc`) |
| `AzureAd:Scopes` | API scope to request (e.g., `api://<client-id>/Admin.Access`) |

### API (`src/TechHub.Api/appsettings.json`)

| Key | Description |
|-----|-------------|
| `AzureAd:Instance` | Login endpoint (`https://login.microsoftonline.com/`) |
| `AzureAd:TenantId` | Directory (tenant) ID |
| `AzureAd:ClientId` | Application (client) ID (same as Web) |
| `AzureAd:Scopes` | Expected scope in access token (e.g., `Admin.Access`) |

The API does not need a `ClientSecret` because it only validates incoming tokens — it does not acquire tokens itself.

## Local Development Setup

Use the setup script to create all required Entra ID resources:

```powershell
./scripts/Setup-EntraIdDev.ps1
```

This creates:

1. An app registration with localhost redirect URIs
2. An `Admin.Access` API scope
3. A client secret (90-day expiry by default)
4. A service principal with "assignment required" enabled
5. Assigns the current user to the enterprise app

The script outputs the configuration values. Store them using .NET User Secrets:

```powershell
cd src/TechHub.Web
dotnet user-secrets set AzureAd:TenantId '<tenant-id>'
dotnet user-secrets set AzureAd:ClientId '<client-id>'
dotnet user-secrets set AzureAd:ClientSecret '<client-secret>'
dotnet user-secrets set AzureAd:Scopes 'api://<client-id>/Admin.Access'

cd ../TechHub.Api
dotnet user-secrets set AzureAd:TenantId '<tenant-id>'
dotnet user-secrets set AzureAd:ClientId '<client-id>'
dotnet user-secrets set AzureAd:Scopes 'Admin.Access'
```

Parameters: `-DisplayName`, `-WebPort`, `-SecretExpiryDays`, `-SkipUserAssignment`.

## Secret Rotation

Client secrets should be rotated regularly. Use the rotation script:

```powershell
# Rotate and update GitHub Actions environment secret
./scripts/Rotate-EntraIdSecret.ps1 -ClientId '<client-id>' -Environment staging
./scripts/Rotate-EntraIdSecret.ps1 -ClientId '<client-id>' -Environment production -RemoveExpired

# Rotate without updating GitHub (manual)
./scripts/Rotate-EntraIdSecret.ps1 -ClientId '<client-id>' -SkipGitHub
```

The script creates a new secret (appended, not replacing), optionally updates GitHub Actions secrets via `gh secret set`, and can clean up expired secrets with `-RemoveExpired`.

## Infrastructure

The Bicep templates pass Azure AD configuration as environment variables to Container Apps:

| Env Variable | Source | Used By |
|---|---|---|
| `AzureAd__TenantId` | Secret ref in Container App | Web + API |
| `AzureAd__ClientId` | Secret ref in Container App | Web + API |
| `AzureAd__ClientSecret` | Secret ref in Container App | Web only |
| `AzureAd__Scopes` | Plain env var in Container App | Web + API |

These are fed from GitHub Actions secrets → `Deploy-Infrastructure.ps1` env vars → Bicep `readEnvironmentVariable()` → Container App secrets/env vars.

## Implementation Reference

- Auth setup — `src/TechHub.Web/Program.cs`, `src/TechHub.Api/Program.cs`
- Token handler — `src/TechHub.Web/Services/AdminTokenDelegatingHandler.cs`
- Local dev script — `scripts/Setup-EntraIdDev.ps1`
- Secret rotation — `scripts/Rotate-EntraIdSecret.ps1`
- Bicep modules — `infra/modules/web.bicep`, `infra/modules/api.bicep`
- CI/CD secrets — `.github/workflows/ci-cd.yml`
