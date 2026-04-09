---
primary_section: ml
feed_name: Microsoft Tech Community
section_names:
- azure
- devops
- ml
title: 'Building an End-to-End MLOps Pipeline: From Training to Managed Endpoints on Azure'
author: Gapandey
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852
tags:
- Az ML
- Azure
- Azure Blob Storage
- Azure CLI
- Azure DevOps Pipelines
- Azure Machine Learning
- Azure ML Registry
- Blue Green Deployments
- Community
- Data Drift Monitoring
- DevOps
- Feature Engineering
- Joblib
- Least Privilege
- Managed Identity
- Managed Online Endpoints
- ML
- MLOps
- Model Versioning
- ONNX
- Pickle
- Pipeline Artifacts
- Python
- Schema Validation
- Scikit Learn
- StandardScaler
- User Managed Identity
- Workload Identity Federation
date: 2026-04-09 10:45:39 +00:00
---

Gapandey lays out a practical, end-to-end MLOps template on Azure: train a scikit-learn model from data in Azure Blob Storage, package it as a self-contained pickle bundle, register it in an Azure ML Registry with auto-versioning, and deploy it to an Azure ML Managed Online Endpoint via an Azure DevOps multi-stage pipeline.<!--excerpt_end-->

## Introduction

Machine learning models are only as valuable as the infrastructure that supports them. A model trained in a Jupyter notebook and saved to a shared folder creates a chain of problems: no versioning, no reproducibility, no clear ownership, and no automated path to production. When the data scientist who trained it goes on vacation, nobody knows how to retrain it or where the latest version lives.

A well-designed MLOps pipeline solves all of this. It makes training repeatable, artifacts versioned, and deployment automated — so that the path from code change to live endpoint is a single merge to main.

This post provides a **generic, end-to-end pattern** covering the full lifecycle:

1. **Train** a scikit-learn model against data in Azure Blob Storage
2. **Serialize** the model as a self-contained pickle bundle
3. **Register** it in an Azure ML Registry for cross-team discovery
4. **Deploy** it to an Azure ML Managed Online Endpoint for real-time scoring

## When to Use This Pattern

This pipeline template is a good fit when:

- Your training data lives in Azure Blob Storage (Parquet, CSV, or similar)
- You use scikit-learn (or any Python ML framework) for model training
- You need versioned model artifacts in a central registry
- You want an automated deployment path to a live scoring endpoint
- Downstream consumers (scoring pipelines, APIs, dashboards) need a reliable handoff mechanism
- You want to eliminate ad-hoc notebook-based training with no versioning or reproducibility

It is **not** the right fit if you need distributed training (use Azure ML pipelines instead), or if your model requires GPU inference (managed endpoints support GPU, but the config differs from what's shown here).

## Architecture Overview

Four-stage flow:

DevOps Gate → Train & Publish Artifact → Register in ML Registry → Deploy to Managed Endpoint

1. **DevOps Stage** — A required gate that logs the build number and validates the pipeline is running.
2. **Train Stage** — Installs Python dependencies, runs the training script against data in Azure Blob Storage, and publishes the pickle bundle as a pipeline artifact.
3. **Register Stage** — Downloads the artifact and registers it in an Azure ML Registry with automatic versioning.
4. **Deploy Stage** — Creates (or updates) a Managed Online Endpoint and deploys the newly registered model version to it for real-time scoring.

The first three stages run on every push to main. The Deploy stage can be gated with a manual approval if you want human review before going live.

## The Training Script

The training script is the core of this pipeline — everything else is orchestration around it. It's a standalone Python CLI that you should be able to run locally before it ever touches a pipeline.

General shape:

1. **Load data** from Azure Blob Storage (Parquet, CSV, etc.) using libraries like `adlfs` and `pyarrow`.
2. **Validate the schema** — check expected columns/types and minimum row counts; fail fast with clear errors.
3. **Engineer features** — derived columns, missing values, categorical encoding.
4. **Train the model** using scikit-learn.
5. **Apply preprocessing** (e.g., `StandardScaler`) and save the preprocessor alongside the model.
6. **Serialize a bundle** containing the model, preprocessor, feature column order, and training metadata into a single pickle file.

The script reads storage credentials from environment variables (keeping secrets out of the codebase). It accepts an `--output-path` argument and writes the serialized bundle there, which the pipeline publishes as an artifact.

### What Goes in the Bundle

The pickle file is a **self-contained scoring contract**:

| Key | Type | Purpose |
| --- | --- | --- |
| `model` | scikit-learn estimator | The trained model (e.g., `IsolationForest`, `RandomForestClassifier`) |
| `scaler` | `StandardScaler` (or similar) | The exact preprocessor fitted on training data |
| `feature_order` | `list[str]` | Column names in the exact order the model expects |
| `metadata.trained_at` | ISO timestamp | When the model was trained |
| `metadata.source_rows` | `int` | Raw row count |
| `metadata.clean_rows` | `int` | Row count after cleaning |
| `metadata.scikit_learn_version` | `str` | scikit-learn version used (pickle compatibility concern) |

This allows consumers to load the bundle and score new data without knowing training details.

## Choosing a Serialization Format

This template uses **pickle**, but choose based on your needs:

| Format | Best For | Trade-off |
| --- | --- | --- |
| pickle | Bundles with metadata (model + scaler + feature order + config) | Built-in; **not safe** to load from untrusted sources |
| joblib | Large NumPy array-heavy models | Faster for large arrays; extra dependency |
| ONNX | Cross-framework interoperability | Portable; not all model types supported |

**Security note:** Never load pickle files from untrusted sources — deserialization can execute arbitrary code. This is safest when artifacts are produced by your own pipeline and stored in an access-controlled registry.

## The Pipeline YAML

Replace `<your-...>` placeholders with your values.

```yaml
trigger:
  branches:
    include:
      - main
  paths:
    include:
      - <your-model-source-path>/*  # e.g., src/models/anomaly-detection/*

stages:
  - stage: DevOps
    displayName: Required DevOps Stage
    jobs:
      - job: Echo
        steps:
          - script: echo build initiated - $(Build.BuildNumber)

  - stage: Train
    dependsOn: DevOps
    displayName: Train Model & Publish Artifact
    jobs:
      - job: TrainModel
        steps:
          - checkout: self

          - task: UsePythonVersion@0
            inputs:
              versionSpec: '3.12'

          - script: |
              python -m pip install --upgrade pip
              pip install -r requirements.txt
            displayName: Install Python dependencies

          - script: |
              python <your-training-script>.py \
                --output-path "$(Build.ArtifactStagingDirectory)/model_bundle.pkl"
            displayName: Train model
            env:
              AZURE_STORAGE_ACCOUNT_NAME: $(AZURE_STORAGE_ACCOUNT_NAME)
              AZURE_STORAGE_ACCOUNT_KEY: $(AZURE_STORAGE_ACCOUNT_KEY)  # see Managed Identity note below

          - task: PublishPipelineArtifact@1
            inputs:
              artifactName: model-pkl
              targetPath: $(Build.ArtifactStagingDirectory)/model_bundle.pkl

  - stage: Register
    dependsOn: Train
    displayName: Register Model in ML Registry
    jobs:
      - job: RegisterModel
        steps:
          - task: DownloadPipelineArtifact@2
            inputs:
              artifactName: model-pkl
              targetPath: $(System.ArtifactsDirectory)/model-pkl

          - task: AzureCLI@2
            displayName: Register model in ML Registry
            inputs:
              azureSubscription: '<your-service-connection>'
              scriptType: ps
              scriptLocation: inlineScript
              inlineScript: |
                az extension add -n ml --yes
                az ml model create `
                  --name <your-model-name> `
                  --path "$(System.ArtifactsDirectory)/model-pkl/model_bundle.pkl" `
                  --type custom_model `
                  --registry-name <your-ml-registry> `
                  --resource-group <your-resource-group>
```

### Key Design Decisions

- **Credentials as environment variables**: store in an Azure DevOps variable group and inject via `env:`.
- **Prefer Managed Identity over keys**: recommended approach is a User Managed Identity (UMI) with `Storage Blob Data Reader`, using `DefaultAzureCredential` in the training script (when supported by the build agent).
- **Separate Train and Register stages**: publish training output as a pipeline artifact so failures in registration don’t force retraining.
- **Register in an Azure ML Registry (not workspace)**: `az ml model create` with `--registry-name` targets a shared registry.
- **Auto-versioning**: repeated `az ml model create` calls with the same `--name` increment the model version automatically.

## Permissions

Pipeline auth uses a User Managed Identity (UMI) linked to an Azure DevOps service connection via workload identity federation. Required roles:

| Role | Scope | Purpose |
| --- | --- | --- |
| Storage Blob Data Reader | Storage account/container | Read training data |
| AzureML Registry User | ML Registry | Register model artifacts |
| AzureML Data Scientist | ML Workspace | Create/update managed endpoints and deployments |

Least privilege is the goal: no subscription-level Owner/Contributor required.

If you can’t use workload identity federation and must use a service principal secret, store it as a secret variable and rotate regularly.

## Common Pitfalls

- **Column name mismatches**: handle case/spacing differences (e.g., `periodid` vs `Period ID`) and validate schema before training.
- **Windows agents use `cmd.exe`, not bash**: line continuations and bash-style commands may break; use PowerShell or single-line commands.
- **`checkout: self` vs named repository checkout**: named checkouts can pull default branch instead of the feature branch.
- **Start with the training script locally**: the pipeline is orchestration; debug the script first.
- **Pin dependencies**: use a `requirements.txt` with pinned versions to avoid silent behavior changes.

## Deploying to a Managed Online Endpoint

For real-time scoring, deploy the registered model to an Azure ML **Managed Online Endpoint**.

Key concepts:

- **Endpoint**: the HTTPS URL clients call; supports key auth or AAD tokens.
- **Deployment**: runs scoring code + model on provisioned compute.
- Multiple deployments (e.g., blue/green) can sit behind one endpoint for canary/A/B with traffic splitting.

### The Scoring Script

Azure ML calls `init()` once per container start, and `run()` for each request.

```python
# score.py — deployed alongside the model
import json
import pickle
import os
import numpy as np
import pandas as pd

def init():
    """Called once when the endpoint container starts."""
    global model_bundle
    model_path = os.path.join(os.getenv("AZUREML_MODEL_DIR"), "model_bundle.pkl")
    with open(model_path, "rb") as f:
        model_bundle = pickle.load(f)
    print(f"Model loaded. Trained at: {model_bundle['metadata']['trained_at']}")
    print(f"Expected features: {model_bundle['feature_order']}")

def run(raw_data):
    """Called on every scoring request."""
    try:
        data = json.loads(raw_data)
        df = pd.DataFrame(data["input_data"])

        # Enforce feature order from the bundle
        df = df[model_bundle["feature_order"]]

        # Apply the same scaler used during training
        scaled = model_bundle["scaler"].transform(df)

        # Predict
        predictions = model_bundle["model"].predict(scaled)

        return json.dumps({
            "predictions": predictions.tolist(),
            "model_version": model_bundle["metadata"].get("scikit_learn_version", "unknown"),
        })
    except KeyError as e:
        return json.dumps({"error": f"Missing expected column: {e}"})
    except Exception as e:
        return json.dumps({"error": str(e)})
```

Notes:

- `AZUREML_MODEL_DIR` is provided by Azure ML; it points to the downloaded model artifact.
- Feature order enforcement avoids silent scoring errors.
- Reusing the same scaler ensures scoring transformations match training.

### The Deploy Stage in the Pipeline

Add a Deploy stage after Register. Endpoint/deployment config is done with Azure ML CLI.

```yaml
- stage: Deploy
  dependsOn: Register
  displayName: Deploy to Managed Endpoint
  jobs:
    - job: DeployModel
      steps:
        - checkout: self  # to access score.py

        - task: AzureCLI@2
          displayName: Create or update endpoint
          inputs:
            azureSubscription: '<your-service-connection>'
            scriptType: ps
            scriptLocation: inlineScript
            inlineScript: |
              az extension add -n ml --yes

              # Create endpoint if it doesn't exist (idempotent)
              $exists = az ml online-endpoint show `
                --name <your-endpoint-name> `
                --resource-group <your-resource-group> `
                --workspace-name <your-workspace> 2>$null

              if (-not $exists) {
                az ml online-endpoint create `
                  --name <your-endpoint-name> `
                  --auth-mode key `
                  --resource-group <your-resource-group> `
                  --workspace-name <your-workspace>
              }

        - task: AzureCLI@2
          displayName: Deploy model to endpoint
          inputs:
            azureSubscription: '<your-service-connection>'
            scriptType: ps
            scriptLocation: inlineScript
            inlineScript: |
              az extension add -n ml --yes

              az ml online-deployment create `
                --name blue `
                --endpoint-name <your-endpoint-name> `
                --model azureml://registries/<your-ml-registry>/models/<your-model-name>/versions/<version-number> `
                --code-path ./scoring `
                --scoring-script score.py `
                --environment-image mcr.microsoft.com/azureml/openmpi4.1.0-ubuntu22.04:latest `
                --instance-type Standard_DS3_v2 `
                --instance-count 1 `
                --resource-group <your-resource-group> `
                --workspace-name <your-workspace> `
                --all-traffic

        - task: AzureCLI@2
          displayName: Smoke test the endpoint
          inputs:
            azureSubscription: '<your-service-connection>'
            scriptType: ps
            scriptLocation: inlineScript
            inlineScript: |
              az extension add -n ml --yes

              az ml online-endpoint invoke `
                --name <your-endpoint-name> `
                --resource-group <your-resource-group> `
                --workspace-name <your-workspace> `
                --request-file scoring/sample-request.json
```

**Version pinning is critical:** the scikit-learn version in the scoring environment must match training (pickle compatibility).

## Complete Pipeline — All Four Stages

```yaml
stages:
  - stage: DevOps   # Gate
  - stage: Train    # Train model → publish pickle artifact
    dependsOn: DevOps
  - stage: Register # Register pickle in Azure ML Registry
    dependsOn: Train
  - stage: Deploy   # Deploy to Managed Online Endpoint
    dependsOn: Register
  # Optional: add a manual approval gate here
  # condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
```

Each stage is independently retriable. If Deploy fails, you don't retrain or re-register — you just redeploy.

## Extending This Template

Ideas:

- **Model validation stage** between Register and Deploy (gate on metrics).
- **Batch scoring** using Azure ML Batch Endpoints.
- **Monitoring** with Azure ML model monitoring for drift and prediction distribution changes.
- **Multi-environment promotion** (dev → staging → prod).
- **A/B testing** with traffic splitting across deployments.

## Conclusion

Core pattern:

1. **Train** — run training script, serialize bundle
2. **Register** — push to Azure ML Registry with auto-versioning
3. **Deploy** — create/update Managed Online Endpoint
4. **Score** — clients call HTTPS API; endpoint handles scaling

Updated Apr 09, 2026

Version 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852)

