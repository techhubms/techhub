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
- **`AdminTokenValidationMiddleware`** — Web middleware for `/admin` routes. Access tokens live in an in-memory MSAL token cache, so after a server restart a signed-in user still has a valid cookie but no cached token. The middleware detects the empty cache and issues an OIDC challenge before Blazor renders, instead of letting the circuit fail
- **`AdminOnly` policy** — Authorization policy on the API (`src/TechHub.Api/Authorization/AdminAuthorization.cs`) that requires `RequireAuthenticatedUser()` + `RequireScope()`
- **Enterprise App** — "Assignment required" is enabled so only explicitly assigned users can sign in

### Logout

`POST /admin/logout` requires a valid antiforgery token. Missing or invalid tokens return `400` without signing out. Valid requests clear the local authentication cookie and sign out through Entra ID when OIDC is registered. Without OIDC, logout clears only the local cookie and redirects to `/`.

## Configuration

Both the Web and API projects read from the `AzureAd` configuration section. When `AzureAd:ClientId` is empty, authentication is disabled. Outside Production (local dev without Entra, tests, PR previews) admin endpoints then allow all requests; in Production the `AdminOnly` policy denies every request and a startup error is logged, so a missing setting fails closed.

### Web app (`src/TechHub.Web/appsettings.json`)

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

The API validates the `Admin.Access` scope (hardcoded in `Program.cs`) because the scope name is part of the app registration contract, not an environment-specific value. The API does not need a `ClientSecret` because it only validates incoming tokens — it does not acquire tokens itself.

## Local Development Setup

Use the management script to create all required Entra ID resources:

```powershell
./scripts/Manage-EntraId.ps1 -Environment localhost
```

This creates (on first run):

1. An app registration with localhost redirect URIs
2. An `Admin.Access` API scope
3. A client secret (90-day expiry by default)
4. A service principal with "assignment required" enabled
5. Assigns the current user to the enterprise app

The script outputs the registration details and points to `Setup-UserSecrets.ps1`, which fills all local user secrets (AzureAd settings, plus the AI and newsletter settings from Azure):

```powershell
./scripts/Setup-UserSecrets.ps1
```

To set the AzureAd values by hand instead, store them with .NET User Secrets:

```powershell
cd src/TechHub.Web
dotnet user-secrets set AzureAd:TenantId '<tenant-id>'
dotnet user-secrets set AzureAd:ClientId '<client-id>'
dotnet user-secrets set AzureAd:ClientSecret '<client-secret>'
dotnet user-secrets set AzureAd:Scopes 'api://<client-id>/Admin.Access'

cd ../TechHub.Api
dotnet user-secrets set AzureAd:TenantId '<tenant-id>'
dotnet user-secrets set AzureAd:ClientId '<client-id>'
```

On subsequent runs, the script detects the existing app registration, skips creation, and only rotates the secret. Old secrets remain valid for overlap — no downtime.

Parameters: `-DisplayName`, `-WebPort`, `-SecretExpiryDays`, `-SkipUserAssignment`, `-RemoveExpired`, `-GitHubSecretName` (defaults to `AZURE_AD_CLIENT_SECRET`), `-GitHubRepo` (defaults to the current repository), and `-DeploymentSpObjectId`.

`-DeploymentSpObjectId` takes the object ID of the GitHub Actions deployment service principal (`sp-techhubms`) and assigns it the roles the deployment needs: Key Vault Secrets Officer, Key Vault Contributor, and Resource Policy Contributor at subscription scope, plus User Access Administrator on `rg-techhub-prod` (so Bicep can create managed identity role assignments). It is safe to re-run.

Use `Assign-AdminUsers.ps1 -Environment <env>` to assign the admin users to the enterprise app.

## Secret Rotation

All environments use the same script. It appends a new secret without invalidating existing ones, so there is no downtime during rotation.

```powershell
# Localhost — creates or rotates the localhost app registration secret
./scripts/Manage-EntraId.ps1 -Environment localhost

# Production — rotates secret, updates GitHub, and cleans up expired secrets
./scripts/Manage-EntraId.ps1 -Environment production -RemoveExpired
```

For staging and production, the script pushes only the client secret (`AZURE_AD_CLIENT_SECRET` by default) to the GitHub Actions environment of the same name via `gh secret set`. Only the production client secret is used by the pipeline. The tenant and client IDs are configured in the Bicep parameter file, not written to GitHub secrets. There is no separate staging Azure environment. The `staging` GitHub environment is retained for the OIDC subject used by PR previews, which have admin authentication disabled.

## Infrastructure

The Bicep templates pass Azure AD configuration as App Service app settings:

| Env Variable | Source | Used By |
|---|---|---|
| `AzureAd__TenantId` | Plain app setting from `azureAdTenantId` in `infra/parameters/prod-applications.bicepparam` | Web + API |
| `AzureAd__ClientId` | Plain app setting from `azureAdClientId` in `infra/parameters/prod-applications.bicepparam` | Web + API |
| `AzureAd__ClientSecret` | Key Vault reference to `techhub-prod-aad-client-secret` | Web only |
| `AzureAd__Scopes` | Plain app setting derived in `web.bicep` as `api://<client-id>/Admin.Access` | Web only |

The tenant and client IDs are identifiers, not secrets, so they are committed in the parameter file. Only the client secret travels through the secret chain: GitHub production environment secret `AZURE_AD_CLIENT_SECRET` → env var in the deploy job → `Sync-KeyVaultSecrets.ps1` (called by `Deploy-Infrastructure.ps1`) → Key Vault secret → App Service Key Vault reference. `readEnvironmentVariable()` in the parameter files is only used for the image tags and infrastructure inputs such as the PostgreSQL password and admin IPs. PR preview sites leave the AzureAd values empty, which disables admin authentication there.

## Implementation Reference

- Auth setup — `src/TechHub.Web/Program.cs`, `src/TechHub.Api/Program.cs`
- Token handler — `src/TechHub.Web/Services/AdminTokenDelegatingHandler.cs`
- Entra ID management — `scripts/Manage-EntraId.ps1` (create + rotate for all environments)
- Bicep modules — `infra/modules/web.bicep`, `infra/modules/api.bicep`
- CI/CD secrets — `.github/workflows/ci.yml`, `.github/workflows/cd.yml`
