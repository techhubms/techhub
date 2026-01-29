---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enforcing-angular-unit-test-coverage-in-azure-devops-pipelines-a/ba-p/4446485
title: 'Enforcing Angular Unit Test Coverage in Azure DevOps Pipelines: A Step-by-Step Guide'
author: Sakshi_Gupta22
feed_name: Microsoft Tech Community
date: 2025-08-21 15:04:06 +00:00
tags:
- .NET Core
- Angular
- Azure DevOps
- ChromeHeadless
- CI/CD Pipeline
- Cobertura
- Code Coverage
- Continuous Integration
- JUnit
- Karma
- npm
- Test Automation
- Test Reporting
- Unit Testing
- YAML Pipeline
- Azure
- Coding
- DevOps
- Community
section_names:
- azure
- coding
- devops
primary_section: coding
---
Sakshi_Gupta22 provides a step-by-step guide to configuring Azure DevOps pipelines for Angular projects, ensuring 70%+ unit test coverage with automated reporting and quality gates for sustainable delivery.<!--excerpt_end-->

# Enforcing Angular Unit Test Coverage in Azure DevOps Pipelines: A Step-by-Step Guide

Unit tests are often neglected as feature development is prioritized. This guide delivers a detailed workflow for integrating mandatory Angular test coverage into Azure DevOps pipelines, making high quality measurable and enforceable for every build.

## Pipeline Overview

The pipeline includes these essential stages:

- **Install Dependencies**: Ensures Angular CLI and required npm packages are available.
- **Run Angular Unit Tests**: Executes `ng test` in CI mode with coverage enabled.
- **Generate Reports**: Produces test results in JUnit and Cobertura formats for further processing.
- **Publish to Azure DevOps**: Pushes test and coverage reports to the Azure DevOps UI.
- **Verify Coverage**: Fails the pipeline automatically if coverage falls below 70%.

This approach continually validates Angular code and enforces code quality in a scalable manner.

---

## Step-by-Step Integration Guide

### 1. Install Angular Dependencies

```yaml
- script: |
    npm cache clean --force
    npm set "//<your-org>.jfrog.io/:_authToken=$(npm-auth-token)"
    npm config set @your-org:registry https://<your-org>.jfrog.io/artifactory/api/npm/<your-repo>/
    npm install -g npm
    npm install -g @angular/cli
    npm install -g @angular-devkit/build-angular --force
    rm -rf node_modules package-lock.json
    npm install --force
  displayName: Install npm packages
  workingDirectory: '$(System.DefaultWorkingDirectory)/unittestintegartion'
```

**Why it matters**: Ensures a clean environment and all testing dependencies are ready.

### 2. Run Angular Unit Tests with Coverage

```yaml
- script: |
    CHROME_PATH=$(which google-chrome || which chromium-browser)
    if [ -z "$CHROME_PATH" ]; then
      echo "Error: Chrome or Chromium is not installed on your system."
      exit 1
    fi
    export CHROME_BIN="$CHROME_PATH"
    echo "Running Angular tests"
    npm run test:ci -- --code-coverage
  workingDirectory: '$(System.DefaultWorkingDirectory)/unittestintegration/src/ui'
  displayName: Run Angular tests for remote
  continueOnError: true
```

**Why it matters**: Runs tests in CI using ChromeHeadless and collects coverage data.

### 3. Create Report Directories

```yaml
- script: |
    mkdir -p reports/junit
    mkdir -p reports/coverage
  displayName: Create reports directories
  workingDirectory: '$(System.DefaultWorkingDirectory)/unittestintegration/src/ui'
```

**Why it matters**: Ensures storage locations for reports exist.

### 4. Publish Test Results

```yaml
- task: PublishTestResults@1
  displayName: 'Publish Test Results - Generate Test Report remote'
  inputs:
    testResultsFiles: '$(System.DefaultWorkingDirectory)/unittestintegration/src/ui/reports/junit/unit-test-results.xml'
```

**Why it matters**: Makes test results visible in the Azure DevOps UI for team awareness.

### 5. Use .NET Core SDK

```yaml
- task: UseDotNet@2
  displayName: 'Use .NET Core sdk 9.0.x'
  inputs:
    version: 9.0.x
```

**Why it matters**: Required for full publishing capability of code coverage artifacts.

### 6. Publish Code Coverage

```yaml
- task: PublishCodeCoverageResults@2
  inputs:
    codeCoverageTool: 'Cobertura'
    summaryFileLocation: '$(System.DefaultWorkingDirectory)/unittestintegration/src/ui/coverage/code-coverage.xml'
    reportDirectory: '$(System.DefaultWorkingDirectory)/unittestintegration/src/ui/coverage'
  displayName: 'Publish UI Code Coverage Results-remote'
```

**Why it matters**: Enables detailed code coverage insights directly in the DevOps UI.

### 7. Enforce 70% Coverage Threshold

```yaml
- script: |
    angular_coverage=$(grep -oP 'line-rate="(\d+\.\d+)"' $(System.DefaultWorkingDirectory)/unittestintegration/src/ui/coverage/code-coverage.xml | head -1 | grep -oP '\d+\.\d+')
    angular_coverage=$(echo "$angular_coverage * 100" | bc)
    if (( $(echo "$angular_coverage < 70" | bc -l) )); then
      echo "Angular test coverage is below 70%: $angular_coverage%"
      exit 1
    fi
    echo "Angular test coverage is above 70%: $angular_coverage%"
  displayName: Verify Angular test coverage-remote
```

**Why it matters**: Fails the build automatically if test coverage drops below 70%, acting as a team-wide quality gate.

### 8. Configure Karma for CI

Include Cobertura and JUnit reporters in `karma.conf.js`:

```js
coverageReporter: {
  dir: require('path').join(__dirname, 'coverage'),
  reporters: [
    { type: 'html' },
    { type: 'lcov' },
    { type: 'cobertura', file: 'code-coverage.xml' },
    { type: 'text-summary' }
  ]
},
junitReporter: {
  outputDir: 'reports/junit',
  outputFile: 'unit-test-results.xml',
  useBrowserName: false
}
```

**Why it matters**: Enables appropriate reporting for integration with Azure DevOps.

### 9. Update package.json

Add a CI-specific test script and supporting dependencies:

```json
"scripts": {
  "test:ci": "ng test --watch=false --browsers=ChromeHeadless --code-coverage"
},
"devDependencies": {
  "karma-coverage": "^2.0.3",
  "karma-junit-reporter": "^2.0.1",
  "chrome-launcher": "^0.14.0"
}
```

**Why it matters**: Ensures required packages and CI scripts are available for automated testing.

---

## Takeaway

Unit tests aren’t just for show — they give teams confidence in every code change. Enforcing Angular unit tests in your Azure DevOps CI/CD flow (using JUnit, Cobertura, and a minimum 70% threshold) creates a reliable quality gate that scales with your delivery teams.

> *Authored by Sakshi_Gupta22.*

---

*Last updated: Aug 21, 2025*

*Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enforcing-angular-unit-test-coverage-in-azure-devops-pipelines-a/ba-p/4446485)
