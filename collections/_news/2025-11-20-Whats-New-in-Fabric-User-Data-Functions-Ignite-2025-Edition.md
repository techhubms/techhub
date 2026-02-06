---
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-user-data-functions-ignite-2025-edition/
title: 'What’s New in Fabric User Data Functions: Ignite 2025 Edition'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-20 11:00:00 +00:00
tags:
- API Integration
- Azure Key Vault
- Business Logic
- Centralized Logic
- Configuration Management
- Cosmos DB
- Data Engineering
- Event Processing
- Fabric Activator
- Managed Functions
- Microsoft Fabric
- NoSQL
- Python
- User Data Functions
- Variable Library
- Azure
- ML
- News
- .NET
- Machine Learning
section_names:
- azure
- dotnet
- ml
primary_section: ml
---
Microsoft Fabric Blog discusses the newest features and integrations available in Fabric User Data Functions, empowering developers to streamline data architectures and enhance business logic within Fabric using Python.<!--excerpt_end-->

# What’s New in Fabric User Data Functions: Ignite 2025 Edition

Fabric User Data Functions adds new capabilities designed to help architects and developers manage and extend data architectures within Microsoft Fabric. This update highlights four major features announced at Ignite 2025: Fabric Activator integration, Variable Library connectivity, Azure Key Vault support, and Cosmos DB access.

## Overview of Fabric User Data Functions

User Data Functions lets you write fully managed Python functions that execute within your Fabric environment. This centralizes business logic, supports integration with data sources, and enables reuse across Fabric items like Pipelines, Notebooks, and Power BI reports, reducing the burden of maintaining multiple one-off solutions.

**Key Benefits:**

- Centralized management for business logic
- Reusable Python code across Fabric architectures
- Integration with external services and APIs

## New Features and Integrations

### 1. Fabric Activator Integration (Preview)

- Trigger functions using Fabric Activator event rules
- Process events from Fabric and OneLake sources
- Pass properties from event data as function parameters
- Configure event-driven workflows without extra setup in User Data Functions

### 2. Variable Library Item Integration

- Connect functions to Variable Library items using the Functions portal
- Store and retrieve configuration settings, global constants, and environment variables
- Seamlessly switch environments (e.g., development vs. staging) without changing code

**Example Usage:**

```python
@udf.connection(alias="TestEnvironment", argName="varLib")
@udf.function()
def accessVariableLibrary(varLib: fn.FabricVariablesClient) -> str:
    containerName = varLib.getVariables().get("CosmosDbContainer")
    return containerName
```

### 3. Azure Key Vault Support

- Secure access to secrets (API keys, passwords, certificates) in Python functions
- Use Key Vault that your Fabric user account has access to (requires Reader permissions)

**Example Usage:**

```python
@udf.generic_connection(argName="keyVaultClient", audienceType="KeyVault")
@udf.function()
def retrieveNews(keyVaultClient: fn.FabricItem, requestBody: str) -> str:
    KEY_VAULT_URL = 'YOUR_KEY_VAULT_URL'
    KEY_VAULT_SECRET_NAME = 'YOUR_SECRET'
    credential = keyVaultClient.get_access_token()
    client = SecretClient(vault_url=KEY_VAULT_URL, credential=credential)
    api_key = client.get_secret(KEY_VAULT_SECRET_NAME).value
    return "Success"
```

### 4. Cosmos DB Support

- Access both Fabric and Azure Cosmos DB containers from Python functions
- Use NoSQL containers for flexible, schema-less storage of JSON documents
- Retrieve database endpoints and manage documents directly from Fabric

**Example Usage:**

```python
from fabric.functions.cosmosdb import get_cosmos_client
from azure.cosmos import exceptions

@udf.generic_connection(argName="cosmosDb", audienceType="CosmosDB")
@udf.function()
def insert_product(cosmosDb: fn.FabricItem) -> list[dict[str, Any]]:
    COSMOS_DB_URI = "{my-cosmos-artifact-uri}"
    DB_NAME = "{my-cosmos-artifact-name}"
    CONTAINER_NAME = "SampleData"
    cosmosClient = get_cosmos_client(cosmosDb, COSMOS_DB_URI)
    database = cosmosClient.get_database_client(DB_NAME)
    container = database.get_container_client(CONTAINER_NAME)
    productId = "8a82f850-a33b-4734-80ce-740ba16c39f1"
    iso_now = datetime.now(timezone.utc).isoformat() + "Z"
    product = { "id": productId, "docType": "product", "timestamp": iso_now }
    return container.create_item(body=product)
```

## Resources for Getting Started

- [Generic connections for Fabric items or Azure resources](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/python-programming-model#generic-connections-for-fabric-items-or-azure-resources)
- [Service details and limitations of Fabric User Data Functions](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-service-limits)
- [Create a Fabric User Data Functions item](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/create-user-data-functions-portal)

With these updates, Fabric User Data Functions offers improved integration and security options for building scalable data solutions. The new features make it easier to organize function logic, securely manage secrets, and seamlessly connect with critical data resources.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-user-data-functions-ignite-2025-edition/)
