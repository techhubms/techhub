---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/common-misconceptions-when-running-locally-vs-deploying-to-azure/ba-p/4473938
title: 'Troubleshooting Azure Linux Web App Deployment: Compatibility, Environment Variables, and Memory'
author: theringe
feed_name: Microsoft Tech Community
date: 2025-11-30 09:14:23 +00:00
tags:
- App Service SKU
- Azure App Service
- Azure Portal
- Build Troubleshooting
- Compatibility
- Container Startup
- Deployment Center
- Environment Variables
- Exit Code 137
- Flask
- Gunicorn
- Linux Web App
- Memory Scaling
- Numpy
- Oryx Build
- pip
- Python
- PyTorch
- Requirements.txt
- Scikit Learn
- Sklearn
- Startup Script
- Streamlit
section_names:
- ai
- azure
- coding
- devops
---
theringe provides a practical troubleshooting guide for deploying Python web apps with AI-related packages on Azure Linux Web Apps, focusing on compatibility, environment variables, and memory challenges.<!--excerpt_end-->

# Troubleshooting Azure Linux Web App Deployment: Compatibility, Environment Variables, and Memory

## Introduction

A frequent challenge for developers is when an application works flawlessly in the local environment but encounters failures after being deployed to Azure. This article introduces typical categories of such problems and walks through troubleshooting steps, focusing on Python applications using AI-related packages (like scikit-learn and numpy) on Azure Linux-based Web Apps.

## Common Categories of Deployment Issues

### 1. Environment Variable Handling and Package Compatibility

**Scenario:** Deploying a Flask + sklearn app to Azure Linux Web App fails due to package deprecation.

- Locally, you might unknowingly rely on old packages (e.g., `sklearn` instead of `scikit-learn`). Azure’s build process enforces stricter compatibility checks.
- Compatibility errors can appear if environment variables (like `SKLEARN_ALLOW_DEPRECATED_SKLEARN_PACKAGE_INSTALL`) are only available during runtime, not build time.
- Solution: Shift build steps to runtime using a startup script (e.g., `run.sh`) that sets appropriate environment variables before running `pip install`.
- Key variables and config:
  - `SCM_DO_BUILD_DURING_DEPLOYMENT=false`
  - `ENABLE_ORYX_BUILD` toggled in Azure Portal
  - Custom startup command: `bash run.sh`

### Sample Code

```python
from flask import Flask
app = Flask(__name__)
@app.route("/")
def index():
    return "hello deploy environment variable"
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=8000)
```

**requirements.txt Example:**

```
Flask==3.1.0
gunicorn==23.0.0
sklearn
```

### 2. Build-Time Errors Caused by AI-Related Packages

- When installing Python AI libraries like `numpy`, you may hit compatibility issues with Python versions on Azure (e.g., `numpy==1.21.0` lacks pre-built wheels for Python 3.10, leading to slow source compilation and container startup timeouts).
- Troubleshooting involves reviewing real-time deployment logs:
  - `docker.log` for startup issues
  - `default_docker.log` for deep container execution logs
- Solutions for compatibility/build issues:
  1. Increase allowed container startup time (`WEBSITES_CONTAINER_START_TIME_LIMIT`)
  2. Downgrade Python version
  3. Upgrade problematic package to compatible version (e.g., `numpy==1.25.0` for Python 3.10)

### 3. Memory Limitations during Build

- Large AI packages (Streamlit, PyTorch) may fail to install due to insufficient memory.
- Error code `137` in logs signals the container ran out of memory.
- Solving requires scaling up the App Service SKU to provide more memory during build.

## Best Practices for Diagnosing and Resolving Issues

- Always check deployment logs at `https://<YOUR_APP_NAME>.scm.azurewebsites.net/newui` for actionable errors.
- Prefer up-to-date package versions that are compatible with your runtime’s Python version.
- Use environment variables judiciously, and know the difference between those set at build time versus runtime.
- Consider startup scripting to control package installations and environment setup.
- Monitor and adjust memory allocations as needed.

## Conclusion

Most deployment-related issues for Azure Linux Web Apps revolve around package compatibility, environment variable placement, and platform resource limitations. By following troubleshooting steps illustrated in these examples—checking logs, tweaking environment settings, adjusting build strategies, and scaling resources—developers can resolve common failures and ensure smoother deployments for Python-based, AI-enabled apps on Azure.

---

**Related Resources:**

- [Deployment and Build from Azure Linux based Web App | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/appsonazureblog/deployment-and-build-from-azure-linux-based-web-app/4461950)
- [numpy · PyPI](https://pypi.org/project/numpy/1.21.0/#files)
- [numpy · PyPI (1.25.0)](https://pypi.org/project/numpy/1.25.0/#files)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/common-misconceptions-when-running-locally-vs-deploying-to-azure/ba-p/4473938)
