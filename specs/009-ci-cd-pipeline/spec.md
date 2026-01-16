# Feature Specification: CI/CD Pipeline with GitHub Actions

**Priority**: 🔴 CRITICAL - Phase 2-3 (Implement early for automated testing)  
**Created**: 2026-01-02  
**Status**: Placeholder  
**Implementation Order**: Should be implemented Phase 2-3 alongside testing specs, not #26

## Overview

Define GitHub Actions workflows for continuous integration (build, test, lint) and continuous deployment (deploy to Azure Container Apps) with automated quality gates and rollback capabilities.

## Why This Is Critical Early

- **Automated testing** - Run unit/component/integration/E2E tests on every commit
- **Quality gates** - Block merges if tests fail or quality drops
- **Fast feedback** - Know immediately if changes break tests
- **Automated deployment** - One-command deploy to staging/production
- **Consistency** - Same build process for all developers

## Scope

**CI Workflow** (on every push/PR):

1. **Build**:
   - Restore .NET dependencies
   - Build all projects (TechHub.Api, TechHub.Web, TechHub.Core, TechHub.Infrastructure)
   - Build fails = pipeline fails

2. **Test**:
   - Run unit tests (002-unit-testing)
   - Run integration tests (003-integration-testing)
   - Run component tests (023-component-testing)
   - Run E2E tests (024-e2e-testing)
   - Generate code coverage report
   - Test failures = pipeline fails

3. **Lint & Format**:
   - Run dotnet format check
   - Run ESLint on JavaScript files
   - Markdown lint on spec files
   - Linting errors = pipeline fails

4. **Security Scan**:
   - Run dependency vulnerability scan
   - Check for known CVEs in NuGet packages
   - Critical vulnerabilities = pipeline fails

5. **Quality Gates**:
   - Code coverage > 80%
   - No critical vulnerabilities
   - All tests pass
   - Linting passes

**CD Workflow** (on merge to main):

1. **Build Docker Images**:
   - Build TechHub.Api container
   - Build TechHub.Web container
   - Tag with commit SHA and version

2. **Push to Azure Container Registry**:
   - Authenticate with service principal
   - Push tagged images to ACR

3. **Deploy to Staging**:
   - Deploy containers to staging environment
   - Run smoke tests against staging
   - Health check endpoints

4. **Manual Approval Gate**:
   - Require manual approval for production deploy
   - Review staging environment first

5. **Deploy to Production**:
   - Blue/green deployment for zero downtime
   - Deploy to production Container Apps
   - Run smoke tests against production
   - Monitor for errors
   - Auto-rollback on health check failures

**Environments**:

- Development: Auto-deploy on every commit to dev branch
- Staging: Auto-deploy on merge to main
- Production: Manual approval required after staging validation

## Implementation Note

⚠️ **This spec is numbered 026 but should be implemented in Phase 2-3**, as soon as testing specs are in place. CI/CD enables automated testing on every commit, critical for quality.

## Deployment Strategy

### Staging Environment

**Pre-Deployment Checklist**:

- [ ] All tests passing (unit, integration, component, E2E)
- [ ] Staging environment deployed and tested
- [ ] Performance benchmarks met (Lighthouse > 95)
- [ ] User acceptance testing completed
- [ ] Monitoring and alerts configured

### Deployment Window

**Preferred Schedule**: Saturday 2:00 AM - 4:00 AM CET (low traffic period)

**Content Freeze**: 2 hours before deployment (for major releases)

- Announce planned maintenance window
- Deploy to staging for final validation
- Monitor for critical issues

**Maximum Downtime**: 1 hour (acceptable for content site)

### Deployment Steps

1. **Pre-Deployment**: Run automated tests, build Docker images, push to registry
2. **Deployment**: Deploy containers to Azure Container Apps, verify health checks
3. **Validation**: Test critical paths (home, sections, content rendering, API endpoints)
4. **DNS Update**: Update DNS to point to new deployment
5. **Monitoring**: Monitor error rates, performance, analytics tracking
6. **Completion**: Announce deployment complete, continue extended monitoring

### Rollback Plan

**Automatic Rollback Triggers**:

- Container Apps health checks fail
- Error rate > 10% for 5+ minutes
- Site unreachable after DNS update

**Rollback Process**:

1. Revert to previous container image version
2. Verify site accessible and functioning
3. Troubleshoot issues offline
4. Schedule retry deployment

**Deployment Retention**: Keep last 5 successful deployments for quick rollback

### Post-Deployment Monitoring

**First 24 Hours**:

- Active monitoring of error rates and performance
- Quick response to critical issues

**First 7 Days**:

- Daily review of Application Insights metrics
- Monitor Core Web Vitals and user experience
- Address high-priority bugs

**First 30 Days**:

- Weekly performance reviews
- User feedback analysis
- Continuous improvement planning

---

## Status

📝 **Placeholder** - Needs complete GitHub Actions workflow YAML files, Azure authentication setup, environment configurations, and rollback automation.

## References

- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Deploy to Azure Container Apps with GitHub Actions](https://learn.microsoft.com/azure/container-apps/github-actions)
- [.NET CI/CD Best Practices](https://learn.microsoft.com/dotnet/devops/dotnet-test-github-action)
