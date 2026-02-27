# CI/CD Pipeline

## Overview

Tech Hub uses GitHub Actions for continuous integration (CI) and continuous deployment (CD) to Azure Container Apps.

All deployment logic lives in reusable PowerShell scripts (`scripts/Deploy-Infrastructure.ps1` and `scripts/Deploy-Application.ps1`) that can be run both locally and from GitHub Actions workflows.

## Workflows

### Unified CI/CD Pipeline

**File**: [.github/workflows/ci-cd.yml](../.github/workflows/ci-cd.yml)

**Triggers**:

- Push to `dotnet-migration` branch (CI + deploy to staging)
- Pull requests to `main` branch (CI only, no deployment)
- Manual dispatch (CI + deploy to staging + optional production)

This is a single unified pipeline that runs all quality checks first, and only deploys after they pass. On pull requests, deployment jobs are skipped entirely.

**CI Jobs** (run on every trigger):

1. **Build** - Builds the entire .NET solution
2. **Unit Tests** - Runs all unit tests (excludes E2E)
3. **Integration Tests** - Runs API integration tests
4. **E2E Tests** - Runs end-to-end Playwright tests
5. **PowerShell Tests** - Runs Pester tests for automation scripts
6. **Lint** - Checks code formatting and markdown linting
7. **Security** - Scans for vulnerabilities in dependencies (Trivy + dependency scan)
8. **Code Coverage** - Generates coverage reports
9. **Quality Gate** - Validates all checks passed and provides clear summary

**Quality Gates**:

- All tests must pass (unit, integration, E2E, PowerShell)
- No linting errors
- Build succeeds
- Security scan completes (warnings allowed)

**PR-Specific Features**:

When running on pull requests, the Quality Gate provides:

- Clear summary of what passed/failed
- Actionable guidance on how to fix failures
- Ready-for-review confirmation when all checks pass
- Links to documentation for help
- **No deployment** — PR runs stop after the quality gate

Jobs run in parallel for faster feedback (~5-10 minutes total).

**Concurrency Strategy**:

- **No workflow-level concurrency group** — each push starts its own CI run immediately, so new commits are never blocked by older runs waiting for environment approval
- **Deployment jobs use per-environment concurrency** (`deploy-staging`, `deploy-production`) to prevent conflicting deploys to the same environment
- CI jobs are stateless and safe to run in parallel across commits

**Deployment Jobs** (run only after quality gate passes, never on PRs):

1. **Detect Changes** - Uses `dorny/paths-filter` to check if `infra/**` files changed
   - Requires full git history (`fetch-depth: 0`) for reliable comparison across multi-commit pushes
   - Manual dispatch also supports `force-infra-deploy` to override detection

2. **Deploy Shared Infrastructure** - Deploys shared resources (ACR) via `Deploy-Infrastructure.ps1`
   - Only runs when infrastructure files changed (or force flag set)

3. **Build & Push** - Calls `Deploy-Application.ps1 -SkipDeploy` to build Docker images and push to ACR
   - Requires **Detect Changes** to have succeeded (ensures quality gate passed)
   - Waits for shared infrastructure (ACR) to be ready first
   - Tags images with commit SHA and `latest`
   - Images are built once and reused for both staging and production

4. **Deploy Staging Infrastructure** - Deploys staging resources via `Deploy-Infrastructure.ps1`
   - Only runs when infrastructure files changed (or force flag set)

5. **Deploy to Staging** - Calls `Deploy-Application.ps1 -SkipBuild -SkipPush` to deploy (automatic)
   - Waits for both image build and staging infrastructure to complete
   - Updates Azure Container Apps with new images
   - Runs smoke tests (health, homepage)

6. **Deploy Production Infrastructure** - Deploys production resources via `Deploy-Infrastructure.ps1`
   - Only runs when infrastructure files changed AND production is approved
   - Uses GitHub environment protection for approval

7. **Deploy to Production** - Calls `Deploy-Application.ps1 -SkipBuild -SkipPush` for production
   - Deploys same images used in staging (no rebuild)
   - Runs smoke tests (health, homepage)
   - **Auto-rollback** on health check failures

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

- Automatic rollback if smoke tests fail
- Restores previous container images
- Manual rollback available via redeployment

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

Configure these secrets in GitHub repository settings:

- `AZURE_CREDENTIALS` - Azure service principal credentials (JSON)
- `AZURE_CONTAINER_REGISTRY` - Name of shared Azure Container Registry: `crtechhub`
- `AZURE_SUBSCRIPTION_ID` - Azure subscription ID

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
  - Environment secrets: Same as repository secrets

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
   - Check **"Force infrastructure deployment"** (since there's no previous commit to detect changes from)
   - This deploys shared infrastructure (ACR), staging infrastructure, builds images, and deploys to staging — all in one run
   - Shared resources: `rg-techhub-shared` with container registry `crtechhub`
   - Staging resources: `rg-techhub-staging` with Container Apps, PostgreSQL, OpenAI, etc.

2. **Deploy Production** (when ready)
   - Go to GitHub Actions → "CI/CD Pipeline"
   - Check both **"Deploy to production"** and **"Force infrastructure deployment"**
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

1. Push to `dotnet-migration` branch (or merge PR)
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
4. Check "Deploy to production" checkbox (and optionally "Force infrastructure deployment")
5. Click "Run workflow"
6. All CI checks run first, then staging deploys
7. **Approval required** - Workflow will pause at the production deployment job
   - Designated approvers receive notification
   - Review staging health, commit details, changes
   - Approve or reject deployment
8. After approval, deployment proceeds:
   - Validates staging health one more time
   - Backs up current production configuration
   - Deploys same images used in staging (no rebuild)
   - Runs comprehensive smoke tests
   - Monitors for 5 minutes
   - Auto-rollback if any health check fails
9. **Deployment complete** - Production is live with new version

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
