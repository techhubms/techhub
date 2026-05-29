# CI/CD Pipeline

## Overview

Tech Hub uses GitHub Actions for continuous integration (CI) and continuous deployment (CD) to Azure Container Apps.

All deployment logic lives in reusable PowerShell scripts (`scripts/Deploy-Infrastructure.ps1` and `scripts/Deploy-Application.ps1`) that can be run both locally and from GitHub Actions workflows.

## Workflows

The pipeline is split into two separate workflows to keep the GitHub Actions overview clean:
PR failures show up under "CI Pipeline" and production deployments under "CD Pipeline".

### CI Pipeline (Pull Requests)

**File**: [.github/workflows/ci.yml](../.github/workflows/ci.yml)

**Triggers**:

- Pull requests to `main` branch (opened, synchronize, reopened, closed)

Runs all quality checks and, when they pass, deploys and tears down PR preview environments.

### CD Pipeline (Production)

**File**: [.github/workflows/cd.yml](../.github/workflows/cd.yml)

**Triggers**:

- Push to `main` branch (CI + production deployment)
- Manual dispatch (CI + production deployment)

Runs all quality checks and, when they pass, deploys to staging and production.

**CI Jobs** (run on every trigger except closed PRs, in parallel):

1. **Build** - Builds the entire .NET solution
2. **Unit Tests** - Runs all unit tests
3. **Integration Tests** - Runs API integration tests
4. **PowerShell Tests** - Runs Pester tests for automation scripts
5. **Lint** - Checks code formatting and markdown linting
6. **Security** - Scans for vulnerabilities in dependencies (Trivy + dependency scan)
7. **CodeQL** - Static analysis for security vulnerabilities (code scanning)
8. **Code Coverage** - Generates coverage reports
9. **Quality Gate** - Validates all checks passed and provides a clear summary

**Quality Gates**:

- All tests must pass (unit, integration, PowerShell)
- No linting errors
- Build succeeds with zero warnings (`TreatWarningsAsErrors` enabled)
- Security scan passes (Trivy fails the build on CRITICAL or HIGH vulnerabilities)
- CodeQL analysis completes successfully (code scanning runs on every push/PR)

**Supply Chain Security**:

- All GitHub Actions are SHA-pinned to specific commit hashes (not mutable tags) to prevent supply chain attacks
- Action versions are annotated with comments (e.g., `# v4.3.1`) for readability

Jobs run in parallel for faster feedback (~5-10 minutes total).

**Concurrency Strategy**:

- **No workflow-level concurrency group** — each push starts its own CI run immediately, so new commits are never blocked by older runs waiting for environment approval
- **Deployment jobs use per-environment concurrency** (`deploy-production`) to prevent conflicting deploys to the same environment
- **PR preview jobs use per-PR concurrency** (`pr-preview-{N}`) — new pushes to an open PR cancel any in-progress preview deploy for that PR; each PR gets its own isolated database so there is no cross-PR interference
- CI jobs are stateless and safe to run in parallel across commits

### PR Preview Environments

PR preview is handled by jobs inside [.github/workflows/ci.yml](../.github/workflows/ci.yml).
This means **the quality gate must pass before a preview is deployed** —
if unit tests, integration tests, lint, or security checks fail, no container is built and no
preview environment is created.

When a pull request is opened or updated and the quality gate passes, a **fully isolated preview
environment** is automatically deployed inside the **production resource group** (`rg-techhub-prod`),
using the production Container Apps Environment (`cae-techhub-prod`).
Each PR gets its own Container Apps (`ca-techhub-api-pr-{number}` and
`ca-techhub-web-pr-{number}`) and its own **PostgreSQL Flexible Server** (`psql-techhub-pr-{number}`)
created via Point-in-Time Restore (PITR) from the production database.

**On PR opened / new commit pushed** (after quality gate passes):

1. Docker images are built and pushed to **ghcr.io**, tagged `pr-{number}-{timestamp}`
2. A PR-specific PostgreSQL server is created via PITR from production (5–8 min)
3. PR-specific Container Apps are created or updated in the production environment (`cae-techhub-prod` in `rg-techhub-prod`)
4. A comment is posted (or updated) on the PR with the direct Container Apps URL
5. Playwright E2E tests run against the preview URL (`Category=Performance` excluded)

**On PR closed**:

1. The PR-specific Container Apps are deleted (no quality gate required)
2. The PR-specific PostgreSQL server is deleted
3. Docker images tagged for the PR are cleaned up from **ghcr.io**
4. The PR comment is updated to confirm the environment has been removed

**Key properties of PR preview environments**:

- Each PR gets an **isolated PostgreSQL database** cloned from production via PITR
- No shared database state — multiple PRs cannot interfere with each other
- Reuse production infrastructure (`rg-techhub-prod`, Container Apps Environment `cae-techhub-prod`)
- Accessible via the default Azure Container Apps URL (no custom domain)
- Multiple PRs run in parallel, each with a unique URL and isolated database
- Concurrency per PR: new pushes cancel in-progress deploys for the same PR
- Images tagged with `pr-{number}-{timestamp}` for easy identification

**Script**: `scripts/Deploy-PrPreview.ps1` — can also be run locally:

```powershell
# Deploy PR #42 preview environment (Azure login required)
./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action deploy -Tag "pr-42-dev"

# Remove the PR #42 preview environment
./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action teardown
```

### Nightly PR Environment Teardown

**File**: [.github/workflows/pr-env-nightly-teardown.yml](../.github/workflows/pr-env-nightly-teardown.yml)

**Triggers**:

- Scheduled daily at 21:00 UTC (22:00 CET / 23:00 CEST depending on DST)
- Manual dispatch with optional `dry_run` flag

Scans `rg-techhub-prod` for all active PR environments (Container Apps named
`ca-techhub-api-pr-{N}` and PostgreSQL servers named `psql-techhub-pr-{N}`) and tears
them all down. This eliminates the cost of idle PostgreSQL servers and catches **orphaned
environments** that were not cleaned up when a PR was closed (e.g. due to a workflow failure).

After teardown, the PR comment on each affected pull request is updated with a nightly
teardown notice.

**Resuming after nightly teardown:**

| You want to... | How |
|---|---|
| Continue testing the same code | Push an empty commit (or re-push the branch tip) — CI builds new images and deploys automatically |
| Push new changes and redeploy | Push a commit — CI builds new images and deploys automatically |

### Main Branch Deployment Jobs

Run only after the quality gate passes, and never on PRs.

1. **Build & Push** - Calls `Build-Images.ps1` to build Docker images and push to `ghcr.io`
   - Tags images with a timestamp (`yyyyMMddHHmmss` format)
   - Images are built once and reused for production

2. **Deploy to Production** - Infrastructure + application deployment
   - Uses GitHub environment protection (at least 1 required reviewer)
   - Automatically detects whether `infra/` or `scripts/` files have changed since the last successful deploy:
     - **Full deploy** (infrastructure changed): runs `Deploy-Infrastructure.ps1` (Phase 1: Bicep infra), then `Deploy-Applications.ps1` (Phase 2: Container Apps Bicep)
     - **Fast update** (no infrastructure changes): runs `Deploy-Application.ps1` — updates Container App image tags only, no Bicep evaluation
   - After deploy: E2E tests run against production (excluding `Category=Performance` and `Category=DevEnvironment`)

### Performance Tests

Database performance tests (`Category=Performance`) are **always excluded from CI** and can
only be run locally. They are excluded by `--filter-not-trait "Category=Performance"` in all
CI E2E runs.

**Why local-only**: The performance tests connect directly to a PostgreSQL database containing
a full production dataset (~4000+ content items). The production database uses IP-based
firewall rules (admin IPs and the NAT Gateway public IP) and is not accessible from GitHub
Actions runners. There is no publicly reachable database with real data available in CI.

**To run locally**:

```powershell
# 1. Ensure local PostgreSQL has real data (restore from production)
./scripts/Restore-Database.ps1 -Target local

# 2. Run performance tests only
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj `
  -- --filter-trait "Category=Performance"
```

**Environments**:

**Production**:

- Resource Group: `rg-techhub-prod`
- Container Apps Environment: `cae-techhub-prod` (shared by PR preview environments too)
- API App: `ca-techhub-api-prod`
- Web App: `ca-techhub-web-prod`
- PostgreSQL: `psql-techhub-prod`
- Azure OpenAI: `oai-techhub-prod`
- Key Vault: `kv-techhub-prod`
- ACS (email): `acs-techhub-prod`
- **Manual approval required** via GitHub Environments

**PR preview environments** (created on-demand in `rg-techhub-prod`):

- Container Apps Environment: `cae-techhub-prod` (shared with production)
- Each PR gets its own Container Apps: `ca-techhub-api-pr-{N}`, `ca-techhub-web-pr-{N}`
- Each PR gets its own PostgreSQL: `psql-techhub-pr-{N}` (created via PITR from production)
- Shared managed identity: `id-techhub-pr` (created once by `infrastructure.bicep`)

**Container Images**: Pushed to `ghcr.io` (GitHub Container Registry). The same image built and tested in a PR preview is deployed directly to production — no rebuild.

**Rollback Strategy**:

- Manual rollback available via redeployment with a previous image tag
- Container Apps revision history provides additional rollback options

## Docker Images

### API Image

**File**: [src/TechHub.Api/Dockerfile](../src/TechHub.Api/Dockerfile)

**Features**:

- Multi-stage build for optimization
- .NET 10 runtime
- Runs on port 8080
- Non-root user for security
- Includes curl for health checks

### Web Image

**File**: [src/TechHub.Web/Dockerfile](../src/TechHub.Web/Dockerfile)

**Features**:

- Multi-stage build for optimization
- .NET 10 runtime
- Runs on port 8080
- Non-root user for security
- Includes curl for health checks

## Azure Authentication

The pipeline authenticates to Azure using [OIDC (Workload Identity Federation)](https://docs.github.com/en/actions/security-for-github-actions/security-hardening-your-deployments/configuring-openid-connect-in-azure). GitHub Actions obtains a short-lived token from Azure at job start - no long-lived credentials are stored in GitHub at all.

### Service Principal

- **Name**: `sp-techhubms`
- **Application (client) ID**: stored as the `AZURE_CLIENT_ID` repository variable

### Federated Credentials

Four federated credentials on `sp-techhubms` cover every job in the pipeline:

| Credential name | OIDC subject | Jobs covered |
|---|---|---|
| `techhub-environment-staging` | `repo:...:environment:staging` | `pr-preview-deploy` |
| `techhub-environment-production` | `repo:...:environment:production` | `deploy-production` |
| `techhub-ref-main` | `repo:...:ref:refs/heads/main` | `teardown-all-pr-envs` (nightly scheduled job) |
| `techhub-pull-request` | `repo:...:pull_request` | `pr-preview-teardown` (includes Dependabot PRs) |

The `pull_request` credential is what allows Dependabot PRs to get full preview environments - GitHub blocks repository secrets from Dependabot, but OIDC variables are accessible.

### Setup

Run `scripts/Setup-OidcAuthentication.ps1` to configure the federated credentials and set the GitHub variables in one step. Requires Azure CLI and GitHub CLI authenticated with appropriate permissions.

## GitHub Configuration

### Repository Variables

Configure these in GitHub repository settings → Secrets and variables → Actions → Variables:

| Variable | Value |
|---|---|
| `AZURE_CLIENT_ID` | Application (client) ID of `sp-techhubms` |
| `AZURE_TENANT_ID` | Azure Active Directory tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |
| `ADMIN_IP_ADDRESSES` | Comma-separated IPs allowed through PostgreSQL and Key Vault firewalls |

`Setup-OidcAuthentication.ps1` sets the Azure identity variables automatically from the current Azure CLI session.

### Repository Secrets

One repository-level secret is required (accessible to all environments and jobs):

| Secret | Notes |
|--------|-------|
| `GHCR_PAT` | GitHub PAT with `write:packages` scope. Synced to Key Vault as `techhub-github-registry-token` by `Sync-KeyVaultSecrets.ps1` so Container Apps can pull images from `ghcr.io`. Also used locally to push images with `Build-Images.ps1`. |

### Environment Secrets

Configure these per-environment in GitHub repository settings → Environments → (staging/production) → Environment secrets:

| Secret | Staging | Production | Notes |
|--------|---------|------------|-------|
| `POSTGRES_ADMIN_PASSWORD` | - | Required | Production DB admin password. `Deploy-Infrastructure.ps1` reads this from Key Vault automatically if not set; only needed explicitly in CI or on first deploy before Key Vault is populated. |
| `AZURE_AD_CLIENT_SECRET` | - | Required | Entra ID client secret for user authentication — set via `Manage-EntraId.ps1 -Environment prod`. |
| `NEWSLETTER_UNSUBSCRIBE_SECRET` | - | Required | HMAC key used to sign newsletter unsubscribe and confirm links. Set once on initial deploy; `Sync-KeyVaultSecrets.ps1` preserves the existing Key Vault value on subsequent deploys if the env var is absent. |
| `WILDCARD_CERT_HUB_MS` | - | Required | PFX certificate for the `*.hub.ms` wildcard TLS domain. Set once on initial deploy or at renewal time; skipped on re-deploy if already in Key Vault. |
| `WILDCARD_CERT_XEBIA_MS` | - | Required | PFX certificate for the `*.xebia.ms` wildcard TLS domain. Set once on initial deploy or at renewal time; skipped on re-deploy if already in Key Vault. |

> **Values resolved automatically — not needed as GitHub secrets:** Tenant ID and Client ID are resolved from the active Azure CLI session and app registration name. The AI key is read directly from the Azure Cognitive Services account. The ACS email endpoint (`NEWSLETTER_ACS_ENDPOINT`) is captured from infrastructure deployment outputs and written to Key Vault automatically — no manual input needed. Only values that Azure cannot provide at deploy time live as GitHub secrets.

## GitHub Environments

Create these environments in GitHub:

### Staging Environment

- **Name**: `staging`
- **Protection rules**: None (auto-deploy)
- **Purpose**: Provides the OIDC federated credential subject (`environment:staging`) for `pr-preview-deploy`. PR environments are deployed into `rg-techhub-prod` — there are no separate staging Azure resources.

### Production Environment

- **Name**: `production`
- **Protection rules**:
  - Required reviewers (at least 1)
  - Wait timer: 5 minutes (optional)

## Deployment Process

### Local Deployment (PowerShell)

Both deployment scripts can be run locally, making it easy to test without triggering GitHub Actions workflows.

**Infrastructure deployment**:

```powershell
# Preview what would change
./scripts/Deploy-Infrastructure.ps1 -Mode whatif

# Deploy production infrastructure (reads ADMIN_IP_ADDRESSES from GitHub variable,
# POSTGRES_ADMIN_PASSWORD from Key Vault automatically).
# On first deploy, also set the secrets that don't yet exist in Key Vault:
#   $env:AZURE_AD_CLIENT_SECRET = '...'
#   $env:GHCR_PAT = '...'
#   $env:NEWSLETTER_UNSUBSCRIBE_SECRET = '...'
#   $env:WILDCARD_CERT_HUB_MS = '...'
#   $env:WILDCARD_CERT_XEBIA_MS = '...'
# On re-deploys these are skipped automatically if already present in Key Vault.
./scripts/Deploy-Infrastructure.ps1 -Mode deploy
```

**Application deployment**:

```powershell
# Deploy Container Apps with a specific image tag (fully standalone)
./scripts/Deploy-Applications.ps1 -Mode deploy -ImageTag "20260501120000"

# Fast image-only update (no Bicep evaluation)
./scripts/Deploy-Application.ps1 -Tag "20260501120000"
```

### First-Time Deployment (Brand New Environment)

When deploying to a completely new Azure subscription for the first time:

1. **Deploy Infrastructure** (initial setup)
   - Go to GitHub Actions → "CD Pipeline"
   - Click "Run workflow"
   - This builds images, deploys production infrastructure (`rg-techhub-prod`), and deploys the applications
   - All resources live in a single resource group: `rg-techhub-prod`

2. **Deploy Production** (when ready)
   - The production deployment job awaits approval
   - All CI checks run first, then images are built, then after approval, production infrastructure and application deploy

**Note**: The Bicep templates use `imageTag = 'initial'` which deploys a Microsoft-provided placeholder ASP.NET app. The deployment workflow immediately replaces this with your actual application. This solves the chicken-and-egg problem of needing Container Apps to exist before images are built, but needing images to exist before Container Apps can be created.

### Automatic Production Deployment

1. Push to `main` branch (or merge PR)
2. CD workflow triggers automatically
3. All CI checks run (build, tests, lint, security)
4. Quality gate validates all checks passed
5. Docker images built once (tagged with a timestamp)
6. Images pushed to `ghcr.io`
7. **Approval required** — Workflow pauses at the production deployment job
   - Designated approvers receive notification
   - Review commit details and changes
   - Approve or reject deployment
8. After approval, deployment proceeds:
   - Deploys production infrastructure (Phase 1 Bicep: VNet, KV, PostgreSQL, ACS, …) if changed
   - Deploys production applications (Phase 2 Bicep: Container Apps) if changed
   - Fast image-update path if only image tags changed
   - E2E tests run against production (excluding `Category=Performance`, `Category=DevEnvironment`)
9. **Deployment complete** — Production is live with new version

## Monitoring

### During Deployment

- GitHub Actions provides real-time logs
- Smoke tests validate critical endpoints
- Health checks ensure services are responding

### Post-Deployment

- Monitor Application Insights for errors
- Check Container Apps logs in Azure Portal
- Review metrics in Aspire Dashboard (local development)

## Rollback Procedures

### Automatic Rollback

Production deployment automatically rolls back if:

- Smoke tests fail
- Health checks fail during 5-minute monitoring window

### Manual Rollback

1. Find the previous successful deployment's commit SHA
2. Go to "Deploy to Production" workflow
3. Run workflow with previous commit SHA as image tag
4. Approve deployment

### Emergency Rollback (Azure Portal)

1. Go to Azure Portal → Container Apps
2. Select the affected app
3. Navigate to "Revisions"
4. Activate previous revision

## Troubleshooting

### Build Failures

- Check .NET SDK version compatibility
- Verify all dependencies restore correctly
- Review build logs in GitHub Actions

### Test Failures

- Check test logs for specific failures
- Ensure database migrations are current
- Verify test data is correct

### Deployment Failures

- Verify Azure credentials are valid
- Check Container Registry authentication
- Ensure correct resource group and app names
- Review Azure Container Apps logs

### Rollback Failures

- Use Azure Portal to manually activate previous revision
- Check if previous images still exist in ACR
- Verify Azure permissions

## Best Practices

### Before Merging to Main

- ✅ All CI checks must pass
- ✅ Code reviewed and approved
- ✅ Tests added/updated for changes
- ✅ Documentation updated

### Before Production Deployment

- ✅ PR E2E tests passed in preview environment
- ✅ No known critical issues
- ✅ Deployment scheduled during low-traffic window
- ✅ Monitoring ready
- ✅ Rollback plan understood

### After Production Deployment

- 📊 Monitor Application Insights for 24 hours
- 🔍 Check error rates and performance
- 📈 Review analytics for traffic patterns
- 📝 Document any issues encountered

## Related Documentation

- [Network Architecture](network-architecture.md) - Infrastructure details
- [Testing Strategy](testing-strategy.md) - Testing approach
- [Repository Structure](repository-structure.md) - Project organization
