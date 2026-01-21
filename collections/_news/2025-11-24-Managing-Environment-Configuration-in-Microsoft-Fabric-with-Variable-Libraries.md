---
external_url: https://blog.fabric.microsoft.com/en-US/blog/manage-environment-configuration-in-fabric-user-data-functions-with-variable-libraries/
title: Managing Environment Configuration in Microsoft Fabric with Variable Libraries
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-24 12:47:12 +00:00
tags:
- Azure Key Vault
- Azure OpenAI
- CI/CD
- Data Engineering
- Deployment Pipeline
- Dynamic Configuration
- Environment Configuration
- Lakehouse
- Microsoft Fabric
- Python
- Schema Management
- Secrets Management
- Secure Variables
- User Data Functions
- Variable Libraries
section_names:
- ai
- azure
- coding
- ml
---
Microsoft Fabric Blog offers practical insights for data engineers on managing environment configuration with variable libraries in Fabric User Data Functions. The tutorial includes code examples for secure integration with Azure OpenAI and Azure Key Vault.<!--excerpt_end-->

# Managing Environment Configuration in Microsoft Fabric with Variable Libraries

Data engineers often face the challenge of maintaining environment-specific configurations—such as Lakehouse names, file paths, and schema names—across development, testing, and production in Microsoft Fabric. Hardcoding such parameters is risky and impractical, especially for ongoing operations and CI/CD pipelines.

## Why Use Fabric Variable Libraries?

- **Consistency:** Centralizes configuration values, enabling reuse across multiple Fabric User Data Functions.
- **Security:** Sensitive details like API keys are referenced securely rather than stored in source code.
- **Flexibility:** Configuration updates do not require code redeployment.
- **Deployment Integration:** Variables can be injected into CI/CD workflows using Fabric deployment pipelines.

## Step-by-Step: Adding a Variable Library to User Data Functions

1. In **Develop mode**, open the Functions portal editor and select **Manage connections** in the ribbon.
2. Reference a Variable Library or other supported Fabric items (e.g., Fabric SQL Database).
3. Select the Variable Library item for your workspace.
4. Note the **Alias** field—you will use this alias in your code for referencing.

*Tip: Always use the latest `fabric-user-data-functions` library to ensure compatibility and security.*

## Accessing Variable Values in Code

When retrieving variables in your function, you reference the Variable Library by alias. This makes your configuration dynamic and your secrets secure.

### Example: Connecting Azure OpenAI with Fabric Variable Library and Key Vault

Here's a Python code snippet to demonstrate a Fabric User Data Function that obtains its configuration and credentials securely from both a Variable Library and Azure Key Vault:

```python
# Assume 'Manage connections' is used to add a Variable Library connection

def chat_request(prompt: str, keyVaultClient: fn.FabricItem, varLib: fn.FabricVariablesClient) -> str:
    '''
    Description: Sends a chat completion request to Azure OpenAI using configuration from Variable Library and Azure Key Vault.
    Pre-requisites:
    - Azure OpenAI endpoint
    - Azure Key Vault with OpenAI API key stored as a secret
    - Fabric identity allowed to access secrets
    - Variable Library with variables:
        - KEY_VAULT_URL
        - API_KEY_SECRET_NAME
        - ENDPOINT
        - MODEL
    - Dependencies: openai, azure-keyvault-secrets, fabric-user-data-functions
    '''
    # Retrieve variables
    variables = varLib.getVariables()
    key_vault_url = variables["KEY_VAULT_URL"]
    api_key_secret_name = variables["API_KEY_SECRET_NAME"]
    endpoint = variables["ENDPOINT"]
    model_name = variables["MODEL"]
    credential = keyVaultClient.get_access_token()
    secret_client = SecretClient(vault_url=key_vault_url, credential=credential)
    key = secret_client.get_secret(api_key_secret_name).value
    api_version = "2024-12-01-preview"
    client = AzureOpenAI(
        api_version=api_version,
        azure_endpoint=endpoint,
        api_key=key,
    )
    response = client.chat.completions.create(
        messages=[
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": prompt}
        ],
        max_completion_tokens=13107,
        temperature=1.0,
        top_p=1.0,
        frequency_penalty=0.0,
        presence_penalty=0.0,
        model=model_name
    )
    return response.choices[0].message.content
```

**Sample Variables to Configure:**

- `KEY_VAULT_URL`: Your Key Vault URL (e.g., <https://my-keyvault.vault.azure.net/>)
- `API_KEY_SECRET_NAME`: Secret name for the API key (e.g., "openai-api-key")
- `ENDPOINT`: Azure OpenAI endpoint (e.g., <https://your-resource.openai.azure.com/>)
- `MODEL`: Deployed OpenAI model name (e.g., "gpt-4")

### CI/CD Integration

Variable libraries make it easier to inject environment-specific settings in deployment pipelines:

- Update secrets or endpoints on demand
- Reference updated variables without code changes

## Conclusion

Using variable libraries within Fabric User Data Functions brings consistency, secure secret management, and easier CI/CD pipeline configuration. Data engineers can efficiently build robust, scalable, and secure solutions in Microsoft Fabric using these techniques. For more sample functions, visit [the GitHub repository](https://github.com/microsoft/fabric-user-data-functions-samples/tree/main/PYTHON).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/manage-environment-configuration-in-fabric-user-data-functions-with-variable-libraries/)
