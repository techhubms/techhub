# GitHub Token Configuration for Infrastructure Deployment

## Overview

The Azure infrastructure deployment includes a Static Web Apps resource that requires access to the GitHub repository. This requires a GitHub token with appropriate permissions.

## Required GitHub Secrets

### STATIC_WEB_APP_TOKEN (Recommended)

Create a Personal Access Token (PAT) with the following permissions:

- **repo** (Full control of private repositories)
  - This includes: repo:status, repo_deployment, public_repo, repo:invite, security_events

### How to Create the PAT

1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Set expiration (recommended: 90 days or less for security)
4. Select scopes:
   - ✅ **repo** (Full control of private repositories)
5. Generate the token and copy it immediately
6. Add it to GitHub repository secrets as `STATIC_WEB_APP_TOKEN`

### Fallback Configuration

If `STATIC_WEB_APP_TOKEN` is not available, the deployment will fall back to using `GITHUB_TOKEN` (the default GitHub Actions token). However, this may have limited permissions and might not work for all Static Web Apps operations.

## Repository Secrets Setup

In your GitHub repository settings → Secrets and variables → Actions, ensure you have:

1. **AZURE_CREDENTIALS** - Azure service principal credentials
2. **AZURE_SUBSCRIPTION_ID** - Azure subscription ID
3. **STATIC_WEB_APP_TOKEN** - GitHub PAT with repo permissions (recommended)

## Parameter Files Updated

The following parameter files have been updated to include the GitHub repository configuration:

- `infra/parameters.template.json` - Template for new environments
- `infra/parameters.prod.json` - Production environment parameters

New parameters added:

- `repositoryUrl`: GitHub repository URL (https://github.com/techhubms/techhub)
- `repositoryBranch`: Deployment branch (main)
- `githubToken`: GitHub token (passed from workflow environment)

## Security Notes

- The GitHub token is marked as `@secure()` in the Bicep template
- The token is passed via environment variables in the GitHub workflow
- Never commit actual token values to the repository
- Rotate tokens regularly for security
- Use the minimum required permissions (repo scope is sufficient)

## Testing

You can test the infrastructure deployment using:

```powershell
# Validate template

./test-infrastructure-deployment.ps1 -Mode validate

# See what would be deployed

./test-infrastructure-deployment.ps1 -Mode whatif
```

Note: Local testing will use a placeholder token value, which is sufficient for validation and whatif operations.
