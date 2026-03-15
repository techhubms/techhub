---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-on-behalf-of-flow-for-entra-based-mcp-servers/ba-p/4486760
title: Implementing Microsoft Entra On-Behalf-Of (OBO) Flow in Python MCP Servers with FastMCP
author: Pamela_Fox
feed_name: Microsoft Tech Community
date: 2026-01-19 08:00:00 +00:00
tags:
- Access Tokens
- Admin Consent
- Authentication
- Authorization
- Azure Active Directory
- AzureProvider
- Cosmos DB
- Dynamic Client Registration
- FastMCP
- Identity Management
- MCP
- Microsoft Entra ID
- Microsoft Graph API
- MSAL Python SDK
- OAuth2
- On Behalf Of Flow
- Python
- Service Principal
- Azure
- Security
- Community
- .NET
section_names:
- azure
- dotnet
- security
primary_section: dotnet
---
Pamela Fox provides an in-depth walkthrough on enabling delegated access in Python MCP servers using Microsoft Entra with the on-behalf-of flow. This technical article guides developers through secure integration steps and code examples.<!--excerpt_end-->

# Implementing Microsoft Entra On-Behalf-Of (OBO) Flow in Python MCP Servers with FastMCP

**Author: Pamela Fox**

In this article, Pamela Fox demonstrates step-by-step how to add delegated authentication and resource access in Python MCP (Model Context Protocol) servers by leveraging Microsoft Entra and the on-behalf-of (OBO) flow, alongside code samples using FastMCP and the Microsoft Graph API.

## Overview

Modern MCP servers often require robust authentication and delegated authorization. Microsoft Entra (formerly Azure AD) provides OAuth2-based identity management, and with the OBO flow, servers can act on behalf of users to access additional APIs (like Microsoft Graph). This guide details how to set up and use these patterns with Python technologies.

## Authentication and Authorization: MCP and Microsoft Entra

- MCP authorization is based on OAuth2, treating MCP clients as OAuth2 clients and MCP servers as OAuth2 resource servers.
- MCP adds essentials like Protected Resource Metadata and Authorization Server Metadata.
- For dynamic scenarios, MCP supports Dynamic Client Registration (DCR) and Client ID Metadata Documents (CIMD), but Microsoft Entra natively only supports the required metadata, not DCR/CIMD.
- To enable DCR with Entra, use the FastMCP SDK, which implements DCR via an OAuth proxy (FastMCP acts as an authorization server, proxying access to Entra and storing OAuth client info, e.g., in Cosmos DB).

## Registering Your Server with Microsoft Entra

1. **App Registration**: Register your MCP server as an application in Entra (Azure AD) using the [Microsoft Graph SDK for Python](https://github.com/microsoftgraph/msgraph-sdk-python).
2. **Configuration Steps:**
    - Set display name, sign-in audience (single tenant), required redirect URIs.
    - Specify scopes for exposed API—these will show as OAuth2 permission scopes.
    - Example code for registration (simplified):

    ```python
    request_app = Application(
        display_name="FastMCP Server App",
        sign_in_audience="AzureADMyOrg",
        web=WebApplication(
            redirect_uris=[
                "http://localhost:8000/auth/callback",
                "https://vscode.dev/redirect",
                "http://127.0.0.1:33418",
                "https://deployedurl.com/auth/callback"
            ],
        ),
        api=ApiApplication(
            oauth2_permission_scopes=[...],
            requested_access_token_version=2,
        )
    )
    app = await graph_client.applications.post(request_app)
    await graph_client.applications.by_application_id(app.id).patch(
        Application(identifier_uris=[f"api://{app.app_id}"]))
    ```

3. **Create Service Principal:**

    ```python
    request_principal = ServicePrincipal(app_id=app.app_id, display_name=app.display_name)
    await graph_client.service_principals.post(request_principal)
    ```

4. **Register a Secret** (for now—consider managed identities in the future):

    ```python
    password_credential = await graph_client.applications.by_application_id(app.id).add_password.post(
        AddPasswordPostRequestBody(
            password_credential=PasswordCredential(display_name="FastMCPSecret")
        )
    )
    ```

## Admin Consent for OBO Flow

- To enable on-behalf-of flows (OBO), grant admin consent for the appropriate Microsoft Graph API or other resource scopes.
- Example code:

    ```python
    server_principal = await graph_client.service_principals_with_app_id(app.app_id).get()
    grant = GrantDefinition(
        principal_id=server_principal.id,
        resource_app_id="00000003-0000-0000-c000-000000000000",  # Graph API
        scopes=["User.Read", "email", "offline_access", "openid", "profile"],
        target_label="server application"
    )
    resource_principal = await graph_client.service_principals_with_app_id(grant.resource_app_id).get()
    desired_scope = grant.scope_string()
    await graph_client.oauth2_permission_grants.post(...)
    ```

## Integrating FastMCP with Entra

- Set up FastMCP's [AzureProvider](https://gofastmcp.com/integrations/azure) for authentication, using the app/client details:

    ```python
    auth = AzureProvider(
        client_id=os.environ["ENTRA_PROXY_AZURE_CLIENT_ID"],
        client_secret=os.environ["ENTRA_PROXY_AZURE_CLIENT_SECRET"],
        tenant_id=os.environ["AZURE_TENANT_ID"],
        base_url=entra_base_url,
        required_scopes=["mcp-access"],
        client_storage=oauth_client_store, # e.g., Cosmos DB
    )
    ```

- Implement middleware to extract the Entra object ID ("oid") from user tokens for downstream logic and auditing:

    ```python
    class UserAuthMiddleware(Middleware):
        def _get_user_id(self):
            token = get_access_token()
            if not (token and hasattr(token, "claims")):
                return None
            return token.claims.get("oid")
        ...
    ```

- Integrate middleware with your FastMCP server and tools.

## OBO Flow for Graph API Access

- Use the [MSAL Python SDK](https://learn.microsoft.com/entra/msal/python/) to configure a `ConfidentialClientApplication`:

    ```python
    confidential_client = ConfidentialClientApplication(
        client_id=os.environ["ENTRA_PROXY_AZURE_CLIENT_ID"],
        client_credential=os.environ["ENTRA_PROXY_AZURE_CLIENT_SECRET"],
        authority=f"https://login.microsoftonline.com/{os.environ['AZURE_TENANT_ID']}",
        token_cache=TokenCache(),
    )
    ```

- To exchange the MCP access token for a Graph API token (OBO):

    ```python
    access_token = get_access_token()
    graph_resource_access_token = confidential_client.acquire_token_on_behalf_of(
        user_assertion=access_token.token,
        scopes=["https://graph.microsoft.com/.default"]
    )
    graph_token = graph_resource_access_token["access_token"]
    ```

- Use this Graph API token for any permitted Microsoft Graph operations, such as group membership checks, querying user data, or accessing OneDrive.

    ```python
    async with httpx.AsyncClient() as client:
        url = (f"https://graph.microsoft.com/v1.0/me/transitiveMemberOf/microsoft.graph.group?$filter=id eq '{group_id}'&$count=true")
        response = await client.get(
            url,
            headers={
                "Authorization": f"Bearer {graph_token}",
                "ConsistencyLevel": "eventual",
            }
        )
        data = response.json()
    ```

- The same pattern enables delegated access to other Microsoft cloud resources.

## Open Source Reference and Code

Developers can find complete working code in the [python-mcp-demos repository](https://github.com/Azure-Samples/python-mcp-demos), including example scripts for app registration, consent, and FastMCP server configuration:

- [`auth_init.py`](https://github.com/Azure-Samples/python-mcp-demos/blob/main/infra/auth_init.py): Registers the Entra app, service principal, secrets, and admin consent
- [`auth_update.py`](https://github.com/Azure-Samples/python-mcp-demos/blob/main/infra/auth_update.py): Updates redirect URIs
- [`auth_entra_mcp.py`](https://github.com/Azure-Samples/python-mcp-demos/blob/main/servers/auth_entra_mcp.py): Main server setup and OBO tool usage

## Conclusion

By combining Microsoft Entra for identity with FastMCP and the OBO flow, you can implement fine-grained, delegated resource access in Python MCP servers, enabling secure integrations with resources like the Microsoft Graph API. This pattern supports scalable, secure, and standards-based solutions as required for modern cloud-native applications.

**Acknowledgements:** Thanks to Matt Gotteiner for guidance and review of the OBO implementation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-on-behalf-of-flow-for-entra-based-mcp-servers/ba-p/4486760)
