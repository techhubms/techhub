---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deployment-and-build-from-azure-linux-based-web-app/ba-p/4461950
title: Deployment and Build Strategies for Azure Linux Web Apps
author: theringe
feed_name: Microsoft Tech Community
date: 2025-10-16 12:39:45 +00:00
tags:
- Azure App Service
- Azure DevOps
- Azure Web Apps
- Build Pipeline
- CI/CD
- Deployment Automation
- Environment Variables
- Flask
- Linux Web Apps
- Oryx Build
- PaaS
- Python
- Startup Script
- VS Code
- YAML Pipelines
- Azure
- DevOps
- Community
- .NET
section_names:
- azure
- dotnet
- devops
primary_section: dotnet
---
theringe explains practical deployment and build strategies for Azure Linux Web Apps, showing how to use Oryx, runtime scripting, and CI/CD to streamline Python app delivery and troubleshooting.<!--excerpt_end-->

# Deployment and Build Strategies for Azure Linux Web Apps

This tutorial provides a practical guide to deploying Python (and other language) applications on Azure Linux Web Apps using four different deployment and build approaches. The strategies are applicable to Node.js, PHP, and more, helping you to understand the trade-offs and essential steps for successful cloud deployment.

## Table of Contents

1. Introduction
2. Deployment Sources
    - From Laptop
    - From CI/CD tools
3. Build Source
    - From Oryx Build
    - From Runtime
    - From Deployment Sources
4. Walkthroughs
    - Laptop + Oryx
    - Laptop + Runtime
    - Laptop
    - CI/CD concept
5. Conclusion

---

## 1. Introduction

Azure Linux Web Apps support multiple deployment and build methods, each with specific strengths and limitations. Correctly identifying your deployment method helps streamline troubleshooting and performance tuning. The focus is on the 'build' phase—ensuring all third-party dependencies are loaded so your application can run. For example, Python uses `pip install` for dependencies, Node.js uses `npm install`, PHP and Java rely on their own mechanisms.

## 2. Deployment Sources

### From Laptop

- **Best for**: Proof of concept, fast local development.
- **Advantages**:
  - Rapid iteration
  - Minimal setup
- **Limitations**:
  - Difficult for local environments to interact with cloud resources
  - OS and dependency mismatches between local and cloud

### From CI/CD Tools

- **Best for**: Version-controlled projects requiring automation.
- **Advantages**:
  - Focus on code, not deployment details
  - Automation on branch commits
- **Limitations**:
  - Build and runtime environments may diverge in subtle OS/package ways

## 3. Build Source

### Oryx Build

- **Scenario**: Offload builds from local or CI/CD to Azure platform
- **Advantages**:
  - Simple configuration
  - Multi-language support
- **Limitations**:
  - Build performance limited by App Service SKU
  - Potential mismatches between build and runtime environments for sensitive packages

### Runtime Build

- **Scenario**: Run build during app startup in its own environment
- **Advantages**:
  - High control over system-level operations
- **Limitations**:
  - Some settings, like system time sync, are inaccessible

### Deployment Source Build

- **Scenario**: Pre-package dependencies before deployment
- **Advantages**:
  - Can include proprietary packages
- **Limitations**:
  - Risks if dev/runtime OS or package support diverges

#### Comparison Table

| Type        | Method      | Scenario         | Advantage             | Limitation            |
|-------------|-------------|------------------|-----------------------|----------------------|
| Deployment  | From Laptop | POC / Dev        | Fast setup            | Poor cloud link      |
| Deployment  | From CI/CD  | Auto pipeline    | Focus on code         | OS mismatch          |
| Build       | From Oryx   | Platform build   | Simple, multi-lang    | Performance cap      |
| Build       | Runtime     | High control     | Flexible operations   | Limited access       |
| Build       | Deployment  | Pre-built deploy | Use private packages  | Environment mismatch |

## 4. Walkthroughs

### Laptop + Oryx

**Environment Variables:**

- `SCM_DO_BUILD_DURING_DEPLOYMENT=false`: Prevents deployment-time build in the deployment environment.
- `WEBSITE_RUN_FROM_PACKAGE=false`: Runs from file directory, not a prepackaged zip.
- `ENABLE_ORYX_BUILD=true`: Enables Azure platform build after deployment.

**Startup Command:**

```bash
bash /home/site/wwwroot/run.sh
```

**Sample Files:**

- `requirements.txt`:

    ```
    Flask==3.0.3

gunicorn==23.0.0
    ```

- `app.py`:

    ```python
    from flask import Flask
    app = Flask(__name__)
    @app.route("/")
    def home():
        return "Deploy from Laptop + Oryx"
    if __name__ == "__main__":
        import os
        app.run(host="0.0.0.0", port=8000)
    ```

- `run.sh`:

    ```bash
    #!/bin/bash
    gunicorn --bind=0.0.0.0:8000 app:app
    ```

- `.deployment`:

    ```
    [config]
    SCM_DO_BUILD_DURING_DEPLOYMENT=false
    ```

Deploy and verify your app after build and deployment finish.

### Laptop + Runtime

**Environment Variables:**

- `SCM_DO_BUILD_DURING_DEPLOYMENT=false`
- `WEBSITE_RUN_FROM_PACKAGE=false`
- `ENABLE_ORYX_BUILD=false`

**Startup Command:**

```bash
bash /home/site/wwwroot/run.sh
```

**Sample Files:**

- `requirements.txt`, `app.py`, `.deployment`—as above.
- `run.sh`:

    ```bash
    #!/bin/bash
    python -m venv venv
    source venv/bin/activate
    pip install -r requirements.txt
    gunicorn --bind=0.0.0.0:8000 app:app
    ```

Build and launch your app in the runtime environment.

### Laptop

**Environment Variables:**

- `SCM_DO_BUILD_DURING_DEPLOYMENT=false`
- `WEBSITE_RUN_FROM_PACKAGE=false`
- `ENABLE_ORYX_BUILD=false`

**Startup Command:**

```bash
bash /home/site/wwwroot/run.sh
```

**Sample Files:**

- `requirements.txt`, `app.py`, `.deployment`—as above.
- `run.sh`:

    ```bash
    #!/bin/bash
    source venv/bin/activate
    gunicorn --bind=0.0.0.0:8000 app:app
    ```

**Remember:** Complete the build locally before deploying.

```bash
python -m venv venv
source venv/bin/activate
pip install -r requirements.txt
```

### CI/CD Concept

When using Azure DevOps (ADO), pipelines mirror manual deployment but automate the steps using YAML workflow definitions. Each stage (build, deploy) runs commands in sequence on agent machines (for example, `ubuntu-latest` VMs), formalizing steps such as `pip install` or other language-specific build actions.

Typical `azure-pipelines.yml` fragments:

```yaml
stages:
  - stage: Build
    jobs:
      - job: Build
        steps:
          - script: pip install -r requirements.txt
  - stage: Deploy
    jobs:
      - job: Deploy
        steps:
          - script: az webapp deploy ...
```

The pipeline acts as an "automated laptop," ensuring consistency, automation, and auditability throughout the deployment process.

## 5. Conclusion

Choosing the appropriate deployment and build setup for your scenario impacts debugging, performance, and troubleshooting. Mastering these approaches is key for developers and DevOps engineers deploying to Azure Linux Web Apps.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deployment-and-build-from-azure-linux-based-web-app/ba-p/4461950)
