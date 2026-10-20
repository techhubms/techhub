# CI/CD Pipeline

## Overview

Tech Hub uses GitHub Actions for continuous integration (CI) and continuous deployment (CD) to a single Azure App Service Plan (Basic B1) hosting both the API and Web sites as separate Web Apps for Containers.

All deployment logic lives in reusable PowerShell scripts (`scripts/Deploy-Infrastructure.ps1` and `scripts/Deploy-Application.ps1`) that can be run both locally and from GitHub Actions workflows.

Production and PR-preview deployments authenticate with OIDC and use Azure Resource Manager
to provision sites or update container images. They do not use SCM/Kudu uploads or publishing
credentials. SCM access is denied and basic publishing authentication is disabled on both
apps. PR-preview failure diagnostics use site state and recent management activity instead
of SCM container log streaming. See [App Service publishing and diagnostics](network-architecture.md#app-service-publishing-and-diagnostics)
for the operational trade-offs.

## Workflows

The pipeline is split into two separate workflows to keep the GitHub Actions overview clean:
PR failures show up under "CI Pipeline" and production deployments under "CD Pipeline".

### CI Pipeline (Pull Requests)

**File**: [.github/workflows/ci.yml](../.github/workflows/ci.yml)

**Triggers**:

- Pull requests to `main` branch (opened, synchronize, reopened, closed) — runs quality-gate checks only
- Manual dispatch (`workflow_dispatch` with a required `pr_number` input) — builds, deploys, and E2E-tests a PR preview environment for that PR

Runs all quality checks on every PR push. PR preview build/deploy/E2E only run when explicitly triggered, and teardown runs automatically when a PR is closed (closing a PR runs only the teardown job, no quality checks) - see [PR Preview Environments](#pr-preview-environments) below.

### CD Pipeline (Production)

**File**: [.github/workflows/cd.yml](../.github/workflows/cd.yml)

**Triggers**:

- Push to `main` branch (CI + production deployment)
- Manual dispatch (CI + production deployment)

Runs all quality checks and, when they pass, builds images and deploys to production. There is no staging deployment job.

**CI Jobs** (run in parallel): in `ci.yml` they run only on `pull_request` events (not on manual dispatch and not on closed PRs). In `cd.yml` they run on every push to `main` and on manual dispatch.

1. **Build** - Builds the entire .NET solution
2. **Unit Tests** - Runs all unit tests
3. **Integration Tests** - Runs API integration tests
4. **PowerShell Tests** - Runs Pester tests for automation scripts
5. **JavaScript Tests** - Runs Vitest tests for client-side JavaScript
6. **Lint** - Checks code formatting and markdown linting
7. **Security** - Scans for vulnerabilities in dependencies (Trivy + dependency scan)
8. **CodeQL** - Static analysis for security vulnerabilities (code scanning)
9. **Code Coverage** - Generates coverage reports (informational, not part of the quality gate)
10. **Quality Gate** - Validates that the build, test, lint, security, and CodeQL jobs passed and provides a clear summary

**Quality Gates**:

- All tests must pass (unit, integration, PowerShell, JavaScript)
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
- **Deployment jobs use per-environment concurrency** (`deploy-production`) to prevent conflicting deploys to the same environment; a running deploy is never cancelled, so infrastructure/Bicep deployments always run to completion
- **Production E2E runs as the final steps of the `deploy-production` job**, so the same concurrency group covers deploy and E2E. A newer run's deploy starts only after the older run's deploy and E2E have finished; E2E never tests a site that a newer deploy is restarting, and nothing in the production pipeline is ever cancelled while running
- **GitHub keeps one pending job per concurrency group** — when several runs queue up, only the newest pending deploy survives and older pending ones are cancelled before they start (the newest commit contains them)
- **PR preview jobs use per-PR concurrency** (`pr-preview-{N}`) — new manual dispatches for an open PR cancel any in-progress preview deploy for that PR; each PR gets its own isolated database so there is no cross-PR interference
- CI jobs are stateless and safe to run in parallel across commits

### PR Preview Environments

PR preview is handled by jobs inside [.github/workflows/ci.yml](../.github/workflows/ci.yml), but
unlike the quality-gate jobs, the preview build/deploy/E2E jobs **only run on manual
`workflow_dispatch`** — they do not run automatically on `pull_request` events. This is a
deliberate choice for the App Service model: all open PR previews share one PR-preview App
Service Plan (`asp-techhub-pr`) (Regional VNet Integration is strictly 1 subnet : 1 Plan, so
per-PR Plans aren't practical), so preview deploys must be triggered explicitly rather than
firing automatically for every push to every open PR.

The shared Plan is **ephemeral, not persistent** — `scripts/Deploy-PrPreview.ps1` creates it on
the first PR preview deploy and deletes it once the last PR preview site is torn down (on PR
close or via nightly teardown), so it never bills while no PR previews are active. Its dedicated
subnet (`snet-app-service-pr`) is still created once by `infrastructure.bicep` and persists
regardless — see [docs/network-architecture.md](network-architecture.md).

**To deploy/refresh a PR preview**: go to GitHub Actions → "CI Pipeline" → "Run workflow", select
the PR's branch, and provide the PR number as the `pr_number` input. The workflow checks out
`refs/pull/{pr_number}/head` regardless of which ref is selected in the UI, builds and pushes
images, deploys the preview, and runs Playwright E2E tests against it.

When triggered, a **fully isolated preview environment** is deployed inside the **production
resource group** (`rg-techhub-prod`), using the shared PR-preview App Service Plan
(`asp-techhub-pr`). Each PR gets its own API + Web App Service sites
(`app-techhub-api-pr-{number}` and `app-techhub-web-pr-{number}`) and its own **PostgreSQL
Flexible Server** (`psql-techhub-pr-{number}`) created via Point-in-Time Restore (PITR) from the
production database.

**On manual dispatch for a PR** (`pr_number` input):

1. Docker images are built and pushed to **ghcr.io**, tagged `pr-{number}-{timestamp}`
2. A PR-specific PostgreSQL server is created via PITR from production (5–8 min), if it doesn't already exist
3. PR-specific App Service sites are created or updated on the shared PR-preview App Service Plan (`asp-techhub-pr` in `rg-techhub-prod`)
4. A comment is posted (or updated) on the PR with the preview URL
5. Playwright E2E tests run against the preview URL (`Category=Performance` excluded)

**On PR closed** (automatic, `pull_request` trigger, no quality gate or manual dispatch required):

1. The PR-specific App Service sites are deleted
2. The PR-specific PostgreSQL server is deleted
3. Docker images tagged for the PR are cleaned up from **ghcr.io**
4. The PR comment is updated to confirm the environment has been removed

**Key properties of PR preview environments**:

- Each PR gets an **isolated PostgreSQL database** cloned from production via PITR
- No shared database state — multiple PRs cannot interfere with each other
- Reuse production infrastructure (`rg-techhub-prod`) but on a separate, dedicated App Service Plan (`asp-techhub-pr`) so PR traffic never affects prod memory/CPU
- The Plan itself is ephemeral — created on the first active PR preview, deleted when the last one is torn down (no idle billing between previews)
- Accessible via the default `*.azurewebsites.net` hostname (no custom domain)
- Multiple PRs can run in parallel on the shared Plan, each with a unique URL and isolated database
- Concurrency per PR: new manual dispatches cancel in-progress deploys for the same PR
- Images tagged with `pr-{number}-{timestamp}` for easy identification
- Preview deploy/refresh is **manual-only** (`workflow_dispatch`) — it does not run automatically on every push to an open PR

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

Scans `rg-techhub-prod` for all active PR environments (App Service sites named
`app-techhub-api-pr-{N}` and PostgreSQL servers named `psql-techhub-pr-{N}`) and tears
them all down. This eliminates the cost of idle PostgreSQL servers and catches **orphaned
environments** that were not cleaned up when a PR was closed (e.g. due to a workflow failure).

After teardown, the PR comment on each affected pull request is updated with a nightly
teardown notice.

**Resuming after nightly teardown:** manually re-trigger the [CI Pipeline](../.github/workflows/ci.yml)
workflow (`workflow_dispatch`, providing the PR number) to rebuild images and redeploy the preview
environment. Pushing a new commit does **not** redeploy the preview automatically — only the
quality-gate checks re-run on push (preview deploy is manual-only, see above).

### Main Branch Deployment Jobs

Run only after the quality gate passes, and never on PRs.

1. **Build & Push** - Calls `Build-Images.ps1` to build Docker images and push to `ghcr.io`
   - Tags images with a timestamp (`yyyyMMddHHmmss` format)
   - Images are built fresh on `main` (PR preview images are separate `pr-{number}-{timestamp}` builds and are not promoted)
   - Prunes old production versions in `ghcr.io`, keeping the last 2 tagged versions per package (see [Container Image Retention](#container-image-retention))

2. **Deploy & E2E Tests (Production)** - Infrastructure + application deployment, followed by E2E tests in the same job
   - Uses GitHub environment protection (at least 1 required reviewer)
   - Automatically detects whether `infra/` or `scripts/` files have changed since the last successful deploy:
     - **Full deploy** (infrastructure changed): installs the `Az.PostgreSql` PowerShell module, runs `Deploy-Infrastructure.ps1` (Phase 1: Bicep infra), then `Deploy-Applications.ps1` (Phase 2: API + Web App Service Bicep)
     - **Fast update** (no infrastructure changes): runs `Deploy-Application.ps1` - updates the App Service sites' container image tags only (`az webapp config set`), no Bicep evaluation
   - `Deploy-Application.ps1` also sets the `DEPLOY_IMAGE_TAG` app setting and runs the [post-deploy gate](#post-deploy-gate)
   - Creates an Application Insights release annotation (see [Release Annotations](#release-annotations))
   - Waits 60 seconds for the deployment to stabilize, then runs E2E tests against production (excluding `Category=Performance` and `Category=DevEnvironment`)

#### Post-Deploy Gate

`scripts/Wait-ForLiveVersion.ps1` is the readiness gate after a deploy. It polls the Web `/version` endpoint (up to 300 seconds) until the expected image tag is reported, then runs health and homepage smoke tests. The Web site only serves once its startup has reached the API, so a matching `/version` confirms the Web -> API -> database chain. `Deploy-Application.ps1` (production) and `Deploy-PrPreview.ps1` (PR previews) both call it. `Deploy-Application.ps1` rolls back to the previous images when the gate fails.

#### Release Annotations

After each production deploy, `scripts/Add-AppInsightsAnnotation.ps1` creates an Application Insights release annotation (release name = image tag, plus commit, actor, and run URL), so deployments show up as markers on the telemetry charts.

#### Container Image Retention

- **Production images**: after each build, `cd.yml` deletes old production versions from `ghcr.io` and keeps the last 2 (PR images are excluded from this pruning)
- **PR images**: removed from `ghcr.io` when the PR is closed (see [PR Preview Environments](#pr-preview-environments))

### Dependency Automation and Git Hooks

- **Dependabot** ([.github/dependabot.yml](../.github/dependabot.yml)): weekly, grouped update PRs for GitHub Actions, NuGet, npm, Docker (root, API, and Web Dockerfiles), and dev container features
- **Dependency review** ([.github/workflows/dependency-review.yml](../.github/workflows/dependency-review.yml)): runs on every pull request and flags newly introduced vulnerable dependencies. Mark it as a required check to block merging
- **CodeQL configuration** ([.github/codeql/codeql-config.yml](../.github/codeql/codeql-config.yml)): excludes test fixtures and sample collections from code scanning
- **Husky pre-commit hook** (`.husky/pre-commit`): verifies `dotnet format` on staged C# files and runs markdownlint on staged Markdown files, so formatting issues are caught before CI

### Performance Tests

Database performance tests (`Category=Performance`) are **always excluded from CI** and can
only be run locally. They are excluded by `--filter-not-trait "Category=Performance"` in all
CI E2E runs.

**Why local-only**: The performance tests connect directly to a PostgreSQL database containing
a full production dataset (~4000+ content items). The production database is reachable from
the App Service sites only via a private endpoint on the VNet, plus IP-based firewall rules for admin
IPs, and is not accessible from GitHub Actions runners. There is no publicly reachable database
with real data available in CI.

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
- App Service Plan: `asp-techhub-prod` (Basic B1, shared by API + Web sites)
- API app: `app-techhub-api-prod`
- Web app: `app-techhub-web-prod`
- PostgreSQL: `psql-techhub-prod`
- Azure OpenAI: `oai-techhub-prod`
- Key Vault: `kv-techhub-prod`
- ACS (email): `acs-techhub-prod`
- **Manual approval required** via GitHub Environments

**PR preview environments** (created on-demand in `rg-techhub-prod`):

- App Service Plan: `asp-techhub-pr` (Basic B1, shared by all PR previews — separate from production, **ephemeral**: created on first PR preview deploy, deleted once the last one is torn down)
- Each PR gets its own App Service sites: `app-techhub-api-pr-{N}`, `app-techhub-web-pr-{N}`
- Each PR gets its own PostgreSQL: `psql-techhub-pr-{N}` (created via PITR from production)
- Shared managed identity: `id-techhub-pr` (created once by `infrastructure.bicep`)

**Container Images**: Pushed to `ghcr.io` (GitHub Container Registry). PR previews and production use separate builds: PR images are tagged `pr-{number}-{timestamp}` by `ci.yml`, and `cd.yml` rebuilds on `main` with a new timestamp tag.

**Rollback Strategy**:

- Manual rollback available via redeployment with a previous image tag
- No revision history like Container Apps — rollback always redeploys a specific tag via `Deploy-Application.ps1 -Tag <tag>`

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
| `techhub-pull-request` | `repo:...:pull_request` | `pr-preview-teardown` (also works for Dependabot PRs, which cannot read repository secrets but can use OIDC variables) |

The `pull_request` credential is used by the `pr-preview-teardown` job, which runs when a PR closes. Preview deploys (`workflow_dispatch`) use the `staging` environment credential instead.

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
| `GHCR_PAT` | GitHub PAT with `write:packages` scope. Synced to Key Vault as `techhub-github-registry-token` by `Sync-KeyVaultSecrets.ps1` so App Service sites can pull images from `ghcr.io`. Also used locally to push images with `Build-Images.ps1`. |

### Environment Secrets

Configure these per-environment in GitHub repository settings → Environments → (staging/production) → Environment secrets:

| Secret | Staging | Production | Notes |
|--------|---------|------------|-------|
| `POSTGRES_ADMIN_PASSWORD` | - | Required | Production DB admin password. `Deploy-Infrastructure.ps1` reads this from Key Vault automatically if not set; only needed explicitly in CI or on first deploy before Key Vault is populated. |
| `AZURE_AD_CLIENT_SECRET` | - | Required | Entra ID client secret for user authentication — set via `Manage-EntraId.ps1 -Environment production`. |
| `NEWSLETTER_UNSUBSCRIBE_SECRET` | - | Required | HMAC key used to sign newsletter unsubscribe and confirm links. Set once on initial deploy; `Sync-KeyVaultSecrets.ps1` preserves the existing Key Vault value on subsequent deploys if the env var is absent. |
| `WILDCARD_CERT_HUB_MS` | - | Required | PFX certificate for the `*.hub.ms` wildcard TLS domain. Set once on initial deploy or at renewal time; skipped on re-deploy if already in Key Vault. |
| `WILDCARD_CERT_XEBIA_MS` | - | Required | PFX certificate for the `*.xebia.ms` wildcard TLS domain. Set once on initial deploy or at renewal time; skipped on re-deploy if already in Key Vault. |

> **Values not needed as GitHub secrets:** The Entra ID tenant and client IDs for the admin dashboard are set in `infra/parameters/prod-applications.bicepparam` (they are identifiers, not secrets). Only the client secret is a GitHub secret. The AI service uses managed identity and RBAC, so no AI key exists. The ACS email endpoint (`NEWSLETTER_ACS_ENDPOINT`) is captured from infrastructure deployment outputs and written to Key Vault automatically. Only values that Azure cannot provide at deploy time live as GitHub secrets.

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
# Deploy the API + Web App Service sites with a specific image tag (fully standalone)
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

**Note**: The Bicep templates default `apiImageTag`/`webImageTag` to an empty string, which `Deploy-Applications.ps1` resolves to a placeholder tag (`00000000000000`) when no `-ImageTag` is supplied in `validate`/`whatif` mode — a real tag is required for `deploy`. This solves the chicken-and-egg problem of needing the App Service sites to exist before images are built, but needing images to exist before the sites can be created with a real tag.

### Automatic Production Deployment

1. Push to `main` branch (or merge PR)
2. CD workflow triggers automatically
3. All CI checks run (build, tests, lint, security)
4. Quality gate validates all checks passed
5. Docker images built (tagged with a timestamp)
6. Images pushed to `ghcr.io`
7. **Approval required** — Workflow pauses at the production deployment job
   - Designated approvers receive notification
   - Review commit details and changes
   - Approve or reject deployment
8. After approval, deployment proceeds:
   - Deploys production infrastructure (Phase 1 Bicep: VNet, KV, PostgreSQL, ACS, …) if changed
   - Deploys production applications (Phase 2 Bicep: API + Web App Service sites) if changed
   - Fast image-update path if only image tags changed
   - E2E tests run against production (excluding `Category=Performance`, `Category=DevEnvironment`)
9. **Deployment complete** — Production is live with new version

## Monitoring

### During Deployment

- GitHub Actions provides real-time logs
- The [post-deploy gate](#post-deploy-gate) validates the live version and critical endpoints
- Application Insights release annotations mark each deployment

### Post-Deployment

- Monitor Application Insights for errors
- Check App Service log stream / diagnostics in Azure Portal
- Review metrics in Aspire Dashboard (local development)

## Rollback Procedures

### Automatic Rollback

Production deployment automatically rolls back when `Deploy-Application.ps1` (the fast image-update path) fails its [post-deploy gate](#post-deploy-gate):

- The Web `/version` endpoint does not report the new image tag within 300 seconds
- The health check or homepage smoke test fails after the version is confirmed

There is no ongoing monitoring window after the gate passes.

### Manual Rollback

Redeploy a previous image tag (it must still exist in `ghcr.io`, see [Container Image Retention](#container-image-retention)):

```powershell
./scripts/Deploy-Application.ps1 -Tag <previous-tag>
```

### Emergency Rollback (Azure Portal)

1. Go to Azure Portal → App Services
2. Select the affected site (`app-techhub-api-prod` or `app-techhub-web-prod`)
3. Navigate to "Deployment Center" or "Container settings"
4. Set the container image tag back to a previous known-good tag and restart the site

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
- Review App Service log stream / diagnostics in Azure Portal

### Rollback Failures

- Use Azure Portal to manually set the container image tag on the affected App Service site and restart it
- Check if previous images still exist in `ghcr.io`
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
