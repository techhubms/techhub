---
external_url: https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-storage-connector-for-pytorch/
title: Integrating Azure Blob Storage into PyTorch Workflows with azstoragetorch
author: Rohit Ganguly
feed_name: Microsoft DevBlog
date: 2025-06-26 15:00:12 +00:00
tags:
- Azstoragetorch
- Azure Blob Storage
- Azure SDK
- Azure Storage
- DataLoader
- Datasets
- DefaultAzureCredential
- Distributed Training
- Model Checkpointing
- Model Deployment
- Python
- Pytorch
- Storage
section_names:
- azure
- ml
---
Authored by Rohit Ganguly, this article details the release and capabilities of azstoragetorch, enabling direct Azure Blob Storage integration in PyTorch-based machine learning workflows.<!--excerpt_end-->

## Introducing the Azure Storage Connector for PyTorch

**Author:** Rohit Ganguly

### Overview

Microsoft has announced the release of the Azure Storage Connector for PyTorch (`azstoragetorch`), a library designed to simplify the integration of Azure Blob Storage with PyTorch workflows. This library aims to provide a frictionless experience for machine learning practitioners handling large datasets, model checkpoints, and distributed training via direct access to Azure Storage within their PyTorch code.

---

### What is PyTorch?

[PyTorch](https://pytorch.org/) is a leading open-source machine learning framework favored for its flexibility, ease of use, and robust production and research capabilities. A core aspect of machine learning with PyTorch involves managing significant data volumes—across local, cloud, or distributed environments—necessitating efficient data access for training and model management.

---

### Motivation: Bridging PyTorch and Azure Storage

The `azstoragetorch` package closes the gap between PyTorch and Azure Storage:

- **Direct API Integration:** Seamlessly integrates key PyTorch APIs such as model checkpointing and dataset loading with Azure Blob Storage.
- **Efficiency:** Supports both reading and writing files (e.g., datasets, model weights) directly to blobs in Azure without manual data copying or additional storage abstractions.

---

### Getting Started with azstoragetorch

#### Installation

Install from PyPI using pip:

```bash
pip install azstoragetorch
```

This will also bring in dependencies such as `torch` and `azure-storage-blob`.

#### Authentication

The connector leverages Azure’s [`DefaultAzureCredential`](https://learn.microsoft.com/python/api/azure-identity/azure.identity.defaultazurecredential?view=azure-python) for authentication, providing automatic, secure credential discovery (including support for Microsoft Entra ID tokens) suitable for most deployment environments.

---

### Saving and Loading Model Checkpoints

The [`azstoragetorch.io.BlobIO`](https://azure.github.io/azure-storage-for-pytorch/api.html#azstoragetorch.io.BlobIO) class is a file-like API that fits seamlessly into PyTorch’s save/load idioms. Example:

```python
import torch
import torchvision.models  # pip install torchvision
from azstoragetorch.io import BlobIO

CONTAINER_URL = "https://<my-storage-account>.blob.core.windows.net/<my-container>"
model = torchvision.models.resnet18(weights="DEFAULT")

# Save model to Azure Blob

with BlobIO(f"{CONTAINER_URL}/model_weights.pth", "wb") as f:
    torch.save(model.state_dict(), f)

# Load model from Azure Blob

with BlobIO(f"{CONTAINER_URL}/model_weights.pth", "rb") as f:
    model.load_state_dict(torch.load(f))
```

---

### Using PyTorch Datasets with Azure Blob Storage

PyTorch workflows often use `Dataset` and `DataLoader` objects. azstoragetorch supports both map-style and iterable-style datasets:

- **`BlobDataset`**: Provides random (indexed) access (map-style)
- **`IterableBlobDataset`**: Optimized for streaming large datasets (iterable-style)

Initialization can be performed using container URLs (auto-lists blobs) or explicit lists of blob URLs. Data transformation logic (e.g., converting Azure storage blobs directly to tensors) can be supplied via a `transform` callable.

#### Example: Loading Images from Azure Storage

```python
from torch.utils.data import DataLoader
import torchvision.transforms
from PIL import Image
from azstoragetorch.datasets import BlobDataset

def blob_to_category_and_tensor(blob):
    with blob.reader() as f:
        img = Image.open(f).convert("RGB")
        img_transform = torchvision.transforms.Compose([
            torchvision.transforms.Resize(256),
            torchvision.transforms.CenterCrop(224),
            torchvision.transforms.ToTensor(),
            torchvision.transforms.Normalize(
                mean=[0.485, 0.456, 0.406],
                std=[0.229, 0.224, 0.225]
            ),
        ])
        img_tensor = img_transform(img)
        category = blob.blob_name.split("/")[-2]
        return category, img_tensor

CONTAINER_URL = "https://<my-storage-account>.blob.core.windows.net/<my-container>"
dataset = BlobDataset.from_container_url(
    CONTAINER_URL, prefix="datasets/caltech101", transform=blob_to_category_and_tensor
)
loader = DataLoader(dataset, batch_size=32)
for categories, img_tensors in loader:
    print(categories, img_tensors.size())
```

---

### Key Features

- **Zero Configuration**: Automatic credential discovery with `DefaultAzureCredential`.
- **Familiar Patterns**: Mimics native PyTorch and Python file I/O interfaces.
- **End-to-End Integration**: Supports model save/load and direct dataset access via DataLoader.
- **Flexible Access Patterns**: Use whole containers or specific blob lists for large/unstructured datasets.
- **Debugging-Friendly**: Standard Python exceptions and clear error reporting.

---

### Example Use Cases

- **Distributed Training**: Share checkpoints across compute nodes without shared file systems.
- **Model Sharing**: Move trained models between teams or deployment environments efficiently.
- **Dataset Management**: Access Azure-stored datasets directly, eliminating local storage dependencies.
- **Rapid Experimentation**: Switch between models and datasets easily with minimal data movement overhead.

---

### Availability and Feedback

azstoragetorch is available in Public Preview and open-source. Microsoft encourages the community to participate via GitHub—contributing feedback, feature requests, and proposals for future integration.

---

### Resources

- [azstoragetorch Documentation](https://aka.ms/azstoragetorch)
- [Samples and Quickstarts](https://aka.ms/azstoragetorch/samples)
- [Data-Intensive AI Training & Inferencing with Azure Blob Storage](https://build.microsoft.com/sessions/BRK192)
- [Azure Blob Storage Python SDK Quickstart](https://learn.microsoft.com/azure/storage/blobs/storage-quickstart-blobs-python)
- [GitHub Repository & Issues](https://github.com/Azure/azure-storage-for-pytorch/issues)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-storage-connector-for-pytorch/)
