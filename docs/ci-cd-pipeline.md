# CI/CD Pipeline

## Overview

Tech Hub uses GitHub Actions for continuous integration (CI) and continuous deployment (CD) to Azure Container Apps.

All deployment logic lives in reusable PowerShell scripts (`scripts/Deploy-Infrastructure.ps1` and `scripts/Deploy-Application.ps1`) that can be run both locally and from GitHub Actions workflows.

## Workflows

### Unified CI/CD Pipeline

**File**: [.github/workflows/ci-cd.yml](../.github/workflows/ci-cd.yml)

**Triggers**:

- Push to `main` branch (CI + staging + production deployment)
- Pull requests to `main` branch (CI + PR preview deployment)
- Manual dispatch (CI + staging + production deployment)

This is a single unified pipeline. All CI quality checks run first; preview and production
deployments are gated on them.

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
- **Deployment jobs use per-environment concurrency** (`deploy-staging`, `deploy-production`) to prevent conflicting deploys to the same environment
- **PR preview jobs use per-PR concurrency** (`pr-preview-{N}`) — new pushes to an open PR cancel any in-progress preview deploy for that PR
- CI jobs are stateless and safe to run in parallel across commits

### PR Preview Environments

PR preview is handled by jobs inside [.github/workflows/ci-cd.yml](../.github/workflows/ci-cd.yml),
not a separate workflow. This means **the quality gate must pass before a preview is deployed** —
if unit tests, integration tests, lint, or security checks fail, no container is built and no
preview environment is created.

When a pull request is opened or updated and the quality gate passes, a dedicated preview
environment is automatically deployed to the **staging Azure Container Apps Environment**.
Each PR gets its own Container Apps (`ca-techhub-api-pr-{number}` and
`ca-techhub-web-pr-{number}`) without touching the permanent staging application.

**On PR opened / new commit pushed** (after quality gate passes):

1. Docker images are built and pushed to ACR, tagged `pr-{number}-{timestamp}`
2. PR-specific Container Apps are created or updated in the staging environment
3. A comment is posted (or updated) on the PR with the direct Container Apps URL
4. Playwright E2E tests run against the preview URL (`Category=Performance` excluded)

**On PR closed**:

1. The PR-specific Container Apps are deleted (no quality gate required)
2. The PR comment is updated to confirm the environment has been removed

**Key properties of PR preview environments**:

- Reuse staging infrastructure (Container Apps Environment, VNet, PostgreSQL, monitoring)
- Accessible via the default Azure Container Apps URL (no custom domain)
- Multiple PRs run in parallel, each with a unique URL
- Concurrency per PR: new pushes cancel in-progress deploys for the same PR
- Images tagged with `pr-{number}-{timestamp}` for easy identification

**Script**: `scripts/Deploy-PrPreview.ps1` — can also be run locally:

```powershell
# Deploy PR #42 preview environment (Azure login required)
$env:POSTGRES_ADMIN_PASSWORD = "<staging-password>"
./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action deploy -Tag "pr-42-dev"

# Remove the PR #42 preview environment
./scripts/Deploy-PrPreview.ps1 -PrNumber 42 -Action teardown
```

### Main Branch Deployment Jobs

Run only after the quality gate passes, and never on PRs.

Every deployment runs the full Bicep template. ARM is idempotent and only redeploys resources whose desired state actually changed, so there is no need for change detection logic.

1. **Deploy Shared Infrastructure** - Deploys shared resources (ACR) via `Deploy-Infrastructure.ps1`

2. **Build & Push** - Calls `Deploy-Application.ps1 -SkipDeploy` to build Docker images and push to ACR
   - Waits for shared infrastructure (ACR) to be ready first
   - Tags images with commit SHA and `latest`
   - Images are built once and reused for both staging and production

3. **Deploy to Staging** - Full infrastructure + application deployment
   - Runs `Deploy-Infrastructure.ps1` with image tag — Bicep manages everything declaratively (infra config + container image)
   - ARM is idempotent: unchanged resources are not touched
   - Runs smoke tests (health check + homepage)

4. **E2E Tests (Staging)** - Playwright browser tests against staging
   - Runs after staging deployment succeeds
   - Uses `E2E_BASE_URL=https://staging-tech.hub.ms`
   - Excludes performance tests — see [Performance Tests](#performance-tests)
   - Must pass before production deployment proceeds

5. **Deploy to Production** - Full infrastructure + application deployment
   - Uses GitHub environment protection for approval
   - Same flow as staging: single `Deploy-Infrastructure.ps1` call with image tag
   - Deploys same images used in staging (no rebuild)
   - Runs smoke tests (health check + homepage)

### Performance Tests

Database performance tests (`Category=Performance`) are **always excluded from CI** and can
only be run locally. They are excluded by `--filter-not-trait "Category=Performance"` in all
CI E2E runs.

**Why local-only**: The performance tests connect directly to a PostgreSQL database containing
a full production dataset (~4000+ content items). The production and staging databases are
behind Azure private endpoints and are not accessible from GitHub Actions runners. There is
no publicly reachable database with real data available in CI.

**To run locally**:

```powershell
# 1. Ensure local PostgreSQL has real data (restore from production)
./scripts/Restore-Database.ps1 -Target local

# 2. Run performance tests only
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj `
  -- --filter-trait "Category=Performance"
```

**Environments**:

**Shared Resources** (one-time setup):

- Resource Group: `rg-techhub-shared`
- Container Registry: `crtechhub` (used by both staging and production)
- Same tested images are promoted from staging → production

**Staging**:

- Resource Group: `rg-techhub-staging`
- API App: `ca-techhub-api-staging`
- Web App: `ca-techhub-web-staging`
- Azure OpenAI: `oai-techhub-staging` (independent testing)
- Auto-deployed on every merge to `main`
- No approval required

**Production**:

- Resource Group: `rg-techhub-prod`
- API App: `ca-techhub-api-prod`
- Web App: `ca-techhub-web-prod`
- Azure OpenAI: `oai-techhub-prod` (production workloads)
- **Manual approval required** via GitHub Environments
- Same Docker images as staging (no rebuild)

**Rollback Strategy**:

- Manual rollback available via redeployment with a previous image tag
- Container Apps revision history provides additional rollback options

## Docker Images

### API Image

**File**: [src/TechHub.Api/Dockerfile](../src/TechHub.Api/Dockerfile)

**Features**:

- Multi-stage build for optimization
- .NET 9.0 runtime
- Runs on port 8080
- Non-root user for security
- Includes curl for health checks

### Web Image

**File**: [src/TechHub.Web/Dockerfile](../src/TechHub.Web/Dockerfile)

**Features**:

- Multi-stage build for optimization
- .NET 9.0 runtime
- Runs on port 8080
- Non-root user for security
- Includes curl for health checks

## GitHub Secrets Required

### Repository Secrets

Configure these in GitHub repository settings → Secrets and variables → Actions → Repository secrets:

- `AZURE_CREDENTIALS` - Azure service principal credentials (JSON)

### Environment Secrets

Configure these per-environment in GitHub repository settings → Environments → (staging/production) → Environment secrets:

| Secret | Staging | Production | Notes |
|--------|---------|------------|-------|
| `POSTGRES_ADMIN_PASSWORD` | Staging DB password | Production DB password | Set manually, stored in 1Password |
| `AZURE_AD_CLIENT_SECRET` | Entra ID client secret | Entra ID client secret | Set via `Manage-EntraId.ps1 -Environment <env>` |

> **Tenant ID, Client ID, and AI key are no longer GitHub secrets.** `Deploy-Infrastructure.ps1` resolves the tenant ID from the active Azure CLI session, the client ID by looking up the app registration by name (`TechHub Staging` / `TechHub Production`), and the AI key directly from the Azure Cognitive Services account. Only values that cannot be read from Azure need to live as GitHub secrets.

## GitHub Environments

Create these environments in GitHub:

### Staging Environment

- **Name**: `staging`
- **Protection rules**: None (auto-deploy)

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
# Preview staging changes (safe — no modifications)
./scripts/Deploy-Infrastructure.ps1 -Environment staging -Mode whatif

# Deploy shared resources (ACR)
./scripts/Deploy-Infrastructure.ps1 -Environment shared -Mode deploy

# Deploy staging infrastructure
$env:POSTGRES_ADMIN_PASSWORD = "<password>"
./scripts/Deploy-Infrastructure.ps1 -Environment staging -Mode deploy

# Deploy production infrastructure
./scripts/Deploy-Infrastructure.ps1 -Environment production -Mode deploy
```

**Application deployment**:

```powershell
# Build, push, and deploy to staging (images tagged 'dev' by default)
./scripts/Deploy-Application.ps1 -Environment staging

# Build and push only (no container app update)
./scripts/Deploy-Application.ps1 -Environment staging -SkipDeploy

# Deploy a specific tag
./scripts/Deploy-Application.ps1 -Environment staging -Tag "v1.0.0"

# Deploy previously pushed images (skip build and push)
./scripts/Deploy-Application.ps1 -Environment staging -Tag "dev" -SkipBuild -SkipPush
```

When run locally, images default to the `dev` tag to distinguish them from CI-produced images.

### First-Time Deployment (Brand New Environment)

When deploying to a completely new Azure subscription for the first time:

1. **Deploy Infrastructure + Application** (initial setup)
   - Go to GitHub Actions → "CI/CD Pipeline"
   - Click "Run workflow"
   - This deploys shared infrastructure (ACR), staging infrastructure, builds images, and deploys to staging — all in one run
   - Shared resources: `rg-techhub-shared` with container registry `crtechhub`
   - Staging resources: `rg-techhub-staging` with Container Apps, PostgreSQL, OpenAI, etc.

2. **Deploy Production** (when ready)
   - After staging succeeds, the production deployment awaits approval
   - All CI checks run first, then staging deploys, then after approval, production infrastructure and application deploy
   - Creates separate Azure OpenAI `oai-techhub-prod` (independent from staging)

**Note**: The Bicep templates use `imageTag = 'initial'` which deploys a Microsoft-provided placeholder ASP.NET app. The deployment workflow immediately replaces this with your actual application. This solves the chicken-and-egg problem of needing Container Apps to exist before images are built, but needing images to exist before Container Apps can be created.

**Why Separate OpenAI Instances?**

- **Test safely**: Try new model versions (e.g., GPT-5.3) in staging before production
- **Independent quotas**: Staging testing won't affect production rate limits
- **Cost tracking**: See staging vs production AI costs separately
- **Configuration testing**: Test new content filters, deployments, etc. without risk

**Why a Shared Registry?** Both staging and production use the same container registry. This ensures:

- **Immutability**: The exact same tested image from staging is deployed to production (no rebuild risk)
- **Cost-effective**: No duplicate storage of images
- **Simpler management**: Single source of truth for all container images

### Automatic Staging Deployment

1. Push to `main` branch (or merge PR)
2. Unified CI/CD workflow triggers automatically
3. All CI checks run (build, tests, lint, security)
4. Quality gate validates all checks passed
5. Docker images built once (tagged with commit SHA and `latest`)
6. Images pushed to Azure Container Registry
7. Staging Container Apps updated
8. Smoke tests run
9. **Deployment complete** - Staging is live with new version

### Manual Production Deployment

1. **Validate staging** - Test staging environment thoroughly
2. Go to GitHub Actions → "CI/CD Pipeline"
3. Click "Run workflow"
4. All CI checks run first, then staging deploys
5. **Approval required** - Workflow will pause at the production deployment job
   - Designated approvers receive notification
   - Review staging health, commit details, changes
   - Approve or reject deployment
6. After approval, deployment proceeds:
   - Validates staging health one more time
   - Backs up current production configuration
   - Deploys same images used in staging (no rebuild)
   - Runs comprehensive smoke tests
   - Monitors for 5 minutes
   - Auto-rollback if any health check fails
7. **Deployment complete** - Production is live with new version

**Key Points**:

- **Same images** are deployed to both staging and production (no rebuild between environments)
- Production deployment **requires human approval** via GitHub Environments
- Failed deployments trigger **automatic rollback** to previous version
- All deployments are tagged with commit SHA for traceability

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

- ✅ Staging validated and working
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

- [Azure Infrastructure](../specs/008-azure-infrastructure/spec.md) - Infrastructure details
- [Testing Strategy](testing-strategy.md) - Testing approach
- [Repository Structure](repository-structure.md) - Project organization
