# CI/CD Pipeline

## Overview

Tech Hub uses GitHub Actions for continuous integration (CI) and continuous deployment (CD) to Azure Container Apps.

## Workflows

### Continuous Integration (CI)

**File**: [.github/workflows/ci.yml](../.github/workflows/ci.yml)

**Triggers**:

- Push to `main` branch
- Pull requests to `main` branch
- Manual dispatch

**Jobs**:

1. **Build** - Builds the entire .NET solution
2. **Unit Tests** - Runs all unit tests (excludes E2E)
3. **Integration Tests** - Runs API integration tests
4. **E2E Tests** - Runs end-to-end Playwright tests
5. **Lint** - Checks code formatting and markdown linting
6. **Security** - Scans for vulnerabilities in dependencies (Trivy + dependency scan)
7. **Code Coverage** - Generates coverage reports
8. **Quality Gate** - Validates all checks passed and provides clear summary

**Quality Gates**:

- All tests must pass
- No linting errors
- Build succeeds
- Security scan completes (warnings allowed)

**PR-Specific Features**:

When running on pull requests, the Quality Gate provides:

- Clear summary of what passed/failed
- Actionable guidance on how to fix failures  
- Ready-for-review confirmation when all checks pass
- Links to documentation for help

Jobs run in parallel for faster feedback (~5-10 minutes total).

### Deployment Pipeline

**File**: [.github/workflows/deploy.yml](../.github/workflows/deploy.yml)

**Triggers**:

- Automatic on push to `main` branch (staging only)
- Manual dispatch (with optional production deployment)

**Jobs**:

1. **Build & Push** - Builds Docker images once and pushes to Azure Container Registry
   - Tags images with commit SHA and `latest`
   - Pushes both API and Web images
   - Images are built once and reused for both staging and production

2. **Deploy to Staging** - Deploys to staging environment (automatic)
   - Updates Azure Container Apps with new images
   - Waits for deployment to stabilize (30 seconds)
   - Runs smoke tests (API health, Web homepage)
   - Validates health endpoints

3. **Deploy to Production** - Deploys to production environment (manual approval required)
   - **Only runs when manually triggered with `deploy-to-production: true`**
   - Validates staging health before proceeding
   - Creates pre-deployment backup of current production images
   - Updates Container Apps with same images used in staging
   - Runs comprehensive smoke tests (health, API endpoints, Web)
   - Monitors deployment for 5 minutes
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

### First-Time Deployment (Brand New Environment)

When deploying to a completely new Azure subscription for the first time:

1. **Deploy Shared Infrastructure** (one-time setup)
   - Go to GitHub Actions → "Deploy Infrastructure"
   - Select `deploy` mode
   - Select `shared` environment
   - Creates resource group `rg-techhub-shared`
   - Creates shared container registry `crtechhub`
   - This registry will be used by both staging and production

2. **Deploy Staging Infrastructure** (creates placeholder Container Apps)
   - Go to GitHub Actions → "Deploy Infrastructure"
   - Select `deploy` mode
   - Select `staging` environment
   - Container Apps will be created with placeholder images (`mcr.microsoft.com/dotnet/samples:aspnetapp`)
   - Azure OpenAI `oai-techhub-staging` will be created in Sweden Central with GPT-5.2 model

3. **Deploy Application** (builds and pushes real images)
   - Merge to `main` or manually trigger "Deploy to Azure" workflow
   - Builds Docker images and pushes to shared Azure Container Registry (`crtechhub`)
   - Updates Container Apps with real Tech Hub application images
   - Staging is now fully functional

4. **Deploy Production Infrastructure** (when ready)
   - Repeat infrastructure deployment for `production` environment
   - Reuses the same shared registry (`crtechhub`)
   - Creates separate Azure OpenAI `oai-techhub-prod` (independent from staging)
   - Then manually trigger production deployment via workflow

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

1. Merge PR to `main` branch
2. CI workflow runs automatically
3. Deployment workflow triggers automatically
4. Docker images built once (tagged with commit SHA and `latest`)
5. Images pushed to Azure Container Registry
6. Staging Container Apps updated
7. Smoke tests run
8. **Deployment complete** - Staging is live with new version

### Manual Production Deployment

1. **Validate staging** - Test staging environment thoroughly
2. Go to GitHub Actions → "Deploy to Azure"
3. Click "Run workflow"
4. Check "deploy-to-production" checkbox
5. Click "Run workflow"
6. **Approval required** - Workflow will pause at production job
   - Designated approvers receive notification
   - Review staging health, commit details, changes
   - Approve or reject deployment
7. After approval, deployment proceeds:
   - Validates staging health one more time
   - Backs up current production configuration
   - Deploys same images used in staging (no rebuild)
   - Runs comprehensive smoke tests
   - Monitors for 5 minutes
   - Auto-rollback if any health check fails
8. **Deployment complete** - Production is live with new version

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
