---
layout: "post"
title: "Fabric Connection inside Notebook (Preview)"
description: "This news post from the Microsoft Fabric Blog introduces the cross-Fabric 'Get Data with Cloud Connection' feature, enabling secure and flexible connections to cloud data sources directly within notebooks. It details supported data sources, authentication methods, ways to create and manage connections, code generation, and permission requirements for data engineering workflows in Microsoft Fabric."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/32822/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-01-28 14:00:00 +00:00
permalink: "/2026-01-28-Fabric-Connection-inside-Notebook-Preview.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure Blob Storage", "Azure Key Vault", "Azure SQL Database", "Cloud Connection", "Cosmos DB", "Data Engineering", "Data Security", "Data Source Authentication", "Jupyter", "Microsoft Azure", "Microsoft Fabric", "ML", "News", "Notebook", "Python", "Service Principal", "Workspace Identity"]
tags_normalized: ["azure", "azure blob storage", "azure key vault", "azure sql database", "cloud connection", "cosmos db", "data engineering", "data security", "data source authentication", "jupyter", "microsoft azure", "microsoft fabric", "ml", "news", "notebook", "python", "service principal", "workspace identity"]
---

This Microsoft Fabric Blog post presents the new capability to create and manage secure cloud data connections inside notebooks, empowering data engineers with streamlined, credential-protected access to diverse sources. Authored by the Microsoft Fabric team.<!--excerpt_end-->

# Fabric Connection inside Notebook (Preview)

The Microsoft Fabric platform now includes 'Get Data with Cloud Connection'—a feature that allows users to securely and conveniently connect notebooks to a wide range of cloud data sources, including Azure Blob Storage, PostgreSQL, Azure Key Vault, Amazon S3, and more. This update focuses on enhancing data engineer productivity inside Fabric Notebooks, while maintaining secure credential handling and centralized connection management.

## Key Features

- **Cloud Source Support:** Connect to Azure Blob Storage, Azure SQL Database, Cosmos DB, PostgreSQL, and more.
- **Direct Notebook Integration:** Create new connections from within Notebook's 'New Connection' flow, or use the Fabric data source management page.
- **Secure Authentication:** Supports several authentication types, including Basic, Account Key, Token, Workspace Identity, and Service Principal.
- **Automatic Credential Management:** Credentials are handled securely and can be linked directly to notebooks, ensuring only authorized access.
- **Connection Management:** Connections can be toggled to allow 'code-first artifacts' (such as notebooks) to access them during creation—this cannot be changed later.
- **Code Generation:** Users can auto-generate Python code snippets to interact with connected resources, leveraging appropriate SDKs and authentication context.

## Creating a Fabric Connection in Notebooks

1. **Inside Notebook:**
    - Navigate to the **Connection** tab and click **Add connection**.
    - Select the target data source and provide relevant authentication credentials.
    - Choose the authentication type (Basic, Account Key, Token, Workspace Identity, SPN) as supported by your data source.

2. **Via Data Source Management Page:**
    - When creating a new connection, enable access for code-first artifacts by toggling the appropriate option (only available during creation).
    - After creation, link the connection to your notebook using the **Connect** action.

## Using Fabric Connections in Notebooks

- Once a connection is created and bound to a notebook, users can generate code snippets (Python by default) for accessing data.
- Example workflow (Azure Cosmos DB):

  ```python
  from azure.cosmos import CosmosClient
  import json
  import pandas as pd

  connection_id = 'your-connection-id'
  connection_credential = notebookutils.connections.getCredential(connection_id)
  credential_dict = json.loads(connection_credential['credential'])
  key = next(item['value'] for item in credential_dict['credentialData'] if item['name'] == 'key')
  endpoint = 'https://your-endpoint.documents.azure.com:443/'
  client = CosmosClient(endpoint, credential=key)
  databases = list(client.list_databases())
  database = databases[0]
  database_client = client.get_database_client(database['id'])
  containers = list(database_client.list_containers())
  container = containers[0]
  container_name = container['id']
  container_client = database_client.get_container_client(container_name)
  query = f"SELECT * FROM {container_name} p"
  items = list(container_client.query_items(query=query, enable_cross_partition_query=True))
  df = pd.DataFrame(items)
  display(df)
  ```

- If required packages are missing, a code cell with the correct `pip install` line will be auto-generated.

## Connection Permissions

- Running notebooks with connections will validate the user's permission to use the connection.
- Shared notebooks require that each user running code has the necessary permissions.
- For Workspace Identity or SPN-based authentication, ensure the correct permissions are assigned at the data source level.

## Additional Resources

- [Fabric connection inside Notebook (official docs)](https://learn.microsoft.com/fabric/data-engineering/fabric-connection-with-notebook)
- [Fabric Data Engineering Community (Ideas)](https://community.fabric.microsoft.com/t5/Data-Engineering-forums/ct-p/dataengineering)
- [Microsoft Fabric on Reddit](https://www.reddit.com/r/MicrosoftFabric/)

This feature streamlines secure data access workflows for engineers and data scientists leveraging Microsoft Fabric notebooks.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/32822/)
