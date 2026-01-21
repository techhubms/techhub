---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-for-python-on-app-service-for-linux-pyproject-toml-uv/ba-p/4468903
title: 'What’s New for Python on Azure App Service for Linux: pyproject.toml, uv, and Modern Deployments'
author: TulikaC
feed_name: Microsoft Tech Community
date: 2025-11-12 15:16:13 +00:00
tags:
- App Service For Linux
- Azure App Service
- CI/CD
- Cloud Development
- Continuous Deployment
- Deployment Pipeline
- Flask
- GitHub Actions
- Oryx
- PEP 621
- Poetry
- Pyproject.toml
- Python
- Setup.py
- Setuptools
- Uv
- Virtual Environments
section_names:
- azure
- coding
- devops
---
TulikaC explains recent upgrades for building and deploying Python apps on Azure App Service for Linux, covering modern tools like pyproject.toml, uv, Poetry, and offering improved DevOps workflows.<!--excerpt_end-->

# What’s New for Python on App Service for Linux: pyproject.toml, uv, and More

Modern Python development and deployment on Azure App Service for Linux has become easier and more robust, thanks to a series of upgrades described by TulikaC. This post covers:

- **Support for pyproject.toml with uv and Poetry**: Enabling reproducible, modern Python builds.
- **Continued setup.py support**: Backward compatibility for classic workflows.
- **Refreshed .bashrc experience**: Improving SSH sessions in Azure App Service Linux containers.
- **Ready-to-use GitHub Actions sample workflows**: Simplifying automation for various Python project types.

## Key Upgrades

### 1. pyproject.toml with uv and Poetry

- **uv** is a new, fast Python project and package manager written in Rust, delivering rapid dependency resolution and robust workflows ([Astral uv docs](https://docs.astral.sh/uv/)).
- If your repository contains both `pyproject.toml` and `uv.lock`, Azure App Service for Linux now triggers automated uv builds for repeatable environments without extra configuration.
- **pyproject.toml** (per [PEP 621](https://peps.python.org/pep-0621/)) standardizes Python project metadata and dependencies for tools like uv and Poetry.
- Quickstart steps:
  1. `pip install uv`
  2. `uv init` (creates pyproject.toml and a sample main.py)
  3. Add dependencies with `uv add <package>`, e.g., `uv add flask`.
  4. Lock dependencies and run code using `uv run app.py`.
- A minimal pyproject.toml example for a Flask app:

  ```toml
  [project]
  name = "uv-pyproject"
  version = "0.1.0"
  description = "Add your description here"
  readme = "README.md"
  requires-python = ">=3.14"
  dependencies = ["flask>=3.1.2",]
  ```

- App Service’s default startup is `app.py`. If your script is `main.py`, either rename or specify a startup command (e.g., `uv run uvicorn main:app --host 0.0.0.0 --port 8000`).
- During deployment, you’ll see logs confirming uv environment creation and dependency syncing.
- Projects using **Poetry** (with pyproject.toml and poetry.lock) are supported equally ([Poetry reference](https://python-poetry.org/docs/pyproject/)).
- For a working uv sample, check the [lowlight-enhancer-uv project](https://github.com/Azure-Samples/appservice-ai-samples/tree/main/lowlight-enhancer-uv).

### 2. setup.py Support

- The **setup.py** format (classic Setuptools packaging) remains fully supported for projects that haven’t switched to pyproject.toml.
- Minimal example:

  ```python
  # setup.py
  from setuptools import setup, find_packages

  setup(
      name="flask-app",
      version="0.1.0",
      packages=find_packages(exclude=("tests",)),
      python_requires=">=3.14",
      install_requires=["Flask>=3.1.2",],
      include_package_data=True,
  )
  ```

- During deployment, App Service builds virtual environments and installs dependencies as specified in setup.py.
- For Setuptools configuration options, consult the [Setuptools user guide](https://setuptools.pypa.io/en/latest/userguide/quickstart.html) and [keywords reference](https://setuptools.pypa.io/en/latest/references/keywords.html).

### 3. Enhanced SSH/Bash Shell Experience

- Logging into the App Service Linux container via SSH now presents:
  - Clear runtime version information
  - Links to general and Python-specific documentation
  - Instance Name/Id for diagnostics
  - Clean shell startup with error handling improvements (no more confusing auto-activate errors)
- More shell usability options and quality-of-life updates are planned.

### 4. GitHub Actions Sample Workflows

Ready-to-use sample workflows for deploying Python apps are available, supporting all major build/deploy scenarios:

- [Deployment with pyproject.toml + uv](https://github.com/Azure/actions-workflow-samples/blob/master/AppService/Python-GHA-Samples/Python-PyProject-Uv-Sample.yml)
- [Deployment with Poetry](https://github.com/Azure/actions-workflow-samples/blob/master/AppService/Python-GHA-Samples/Python-Poetry-Sample.yml)
- [Deployment with setup.py](https://github.com/Azure/actions-workflow-samples/blob/master/AppService/Python-GHA-Samples/Python-SetupPy-Sample.yml)
- [Local build and deploy workflow](https://github.com/Azure/actions-workflow-samples/blob/master/AppService/Python-GHA-Samples/Python-Local-Built-Deploy-Sample.yml)

**To use:**

1. Copy the relevant YAML workflow into your repo’s `.github/workflows/` directory.
2. Set authentication (OIDC with azure/login is recommended).
3. Fill in required workflow inputs (app name, resource group, any required image/sidecar parameters).
4. Commit and run the workflow via push or on-demand.

For more on automated deployments, see [Microsoft Learn’s deployment with GitHub Actions docs](https://learn.microsoft.com/azure/app-service/deploy-github-actions).

## What’s Next

Azure App Service for Linux is continuing to deliver new upgrades for Python, aiming for faster builds, better support for AI/ML workloads, and enhanced diagnostics.

## Summary

TulikaC’s update helps developers:

- Use modern Python build tools with Azure App Service for Linux
- Automate deployments using robust CI/CD pipelines with GitHub Actions
- Troubleshoot and manage container shell experience with greater ease

## Further Reading

- [Azure App Service Documentation](https://learn.microsoft.com/azure/app-service/)
- [pyproject.toml PEP 621](https://peps.python.org/pep-0621/)
- [Astral uv Documentation](https://docs.astral.sh/uv/)
- [Poetry Documentation](https://python-poetry.org/docs/pyproject/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-for-python-on-app-service-for-linux-pyproject-toml-uv/ba-p/4468903)
