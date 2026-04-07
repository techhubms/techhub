---
primary_section: ai
author: Pamela_Fox
tags:
- Access Token V2.0
- Admin Consent
- AI
- App Registration
- Azure AD
- Azure Container Apps
- AzureJWTVerifier
- Bearer Token
- Community
- Delegated Permissions
- FastMCP
- Federated Identity Credential
- JWT Validation
- Least Privilege
- Managed Identity
- MCP
- Microsoft Entra ID
- Microsoft Graph
- Microsoft Graph SDK
- MSAL
- OAuth 2.1
- OBO
- Oid Claim
- On Behalf Of Flow
- Pre Authorized Applications
- PRM
- Protected Resource Metadata
- Python
- Security
- Service Principal
- Token Claims
- User Impersonation
- VS Code
title: Building MCP servers with Entra ID and pre-authorized clients
date: 2026-04-06 07:00:00 +00:00
section_names:
- ai
- security
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-mcp-servers-with-entra-id-and-pre-authorized-clients/ba-p/4508453
feed_name: Microsoft Tech Community
---

Pamela_Fox walks through implementing Model Context Protocol (MCP) server authentication with Microsoft Entra ID using the pre-registered (pre-authorized client) path, including Entra app registration setup, token validation in FastMCP, and an optional on-behalf-of flow to call Microsoft Graph securely.<!--excerpt_end-->

# Building MCP servers with Entra ID and pre-authorized clients

The [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) gives AI agents a standard way to call external tools. Authentication gets trickier when those tools need to know *who the user is*. This guide shows how to build an MCP server with the [Python FastMCP package](https://gofastmcp.com/) that authenticates users with [Microsoft Entra ID](https://www.microsoft.com/en-us/security/business/identity-access/microsoft-entra-id) when connecting from a **pre-authorized client** such as [VS Code](https://code.visualstudio.com/).

If you need to support *any* MCP client, the author points to a previous post: [Using on-behalf-of flow for Entra-based MCP servers](https://techcommunity.microsoft.com/blog/azuredevcommunityblog/using-on-behalf-of-flow-for-entra-based-mcp-servers/4486760). With Entra as the authorization server, supporting arbitrary clients currently requires an OAuth proxy, which the Entra team discourages due to increased security risk.

## MCP auth

MCP includes an authorization protocol based on OAuth 2.1: the MCP client sends a request with a **Bearer token** from an authorization server, and the MCP server validates that token.

![Diagram showing an MCP client sending a request with a bearer token in the Authorization header to an MCP server](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEjJ-esVZPwODrltqvBNAkaebIVA0CjM4IjU-u9fz8el37vMXLvcYXeDAgenRAqhHB7n3J0pouIf9zcLlcCIKN4ALiZWCTX-admPePnGyUJVYvygAcw_RLAPFb3dDCgQhGVbClm2aSALWVcsV6t1KVn3HVsD4wEhyphenhyphenEuLubyS0PPoNWztQpA4evGqkGkh0Q/s1600/Screenshot%202026-04-02%20at%203.59.47%E2%80%AFPM.png)

In OAuth 2.1 terms:

- **MCP client** = OAuth client
- **MCP server** = resource server
- **Signed-in user** = resource owner
- **Authorization server** issues an access token
- Here, **Microsoft Entra ID** is the authorization server

![Diagram mapping MCP roles to OAuth roles: MCP client as OAuth client, MCP server as resource server, signed-in user as resource owner, and Entra as authorization server](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEhMEzqo-lzKSZyCnuJSlHaPSPKLHKQZxGolECmon5Ahkpr8YGWuimOIRPWfezrUZn4sFejCeTXGJq6gLefA80KMCMPTHR8S3MpBtcB0ZLMLwJ3joB3mX3HOoM-3SIx4LJa8E3mRFt5zdEKSFlbeA-KPdFtSaDZB7Ei6dJ2m3LHLDo4anMZ6A420PuLrNA/s1600/Screenshot%202026-04-02%20at%204.30.39%E2%80%AFPM.png)

### How OAuth clients are recognized in MCP auth

MCP auth describes three ways an authorization server can relate to a client:

- **Pre-registration**: the auth server already knows the client ID
- **CIMD (Client Identity Metadata Document)**: client sends a CIMD URL with JSON metadata
- **DCR (Dynamic Client Registration)**: server registers new clients dynamically (considered “legacy” vs CIMD)

VS Code supports MCP auth including CIMD and DCR. **Entra does not support CIMD or DCR**, which leaves **pre-registration** as the official approach. A proxy can be used to bridge CIMD/DCR, but it increases security risk.

![Comparison diagram showing which MCP client and authorization server combinations support pre-registration, CIMD, or DCR](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEjH_BfQlvZfEqfGaXhbfmTt4lgcWJz59DUjgFlTcr6OY3olwqFW8e_fm-7nznIgUaKW7P4p4leUtxyUI0Xo-CpzuK-DrXYYN-_sahKFAKh1eEvld7utf16w3m86B90SbJu0VojHJAVG-0f_h_v2LW_IqcB37UcTzRtSOY4iZEEidhYyI_pG-Da-k7C98A/s1600/Screenshot%202026-04-02%20at%201.24.46%E2%80%AFPM.png)

### Pre-registered flow overview

When using pre-registration:

- User asks to use an auth-restricted MCP server
- MCP client sends request with no token
- MCP server replies `401` with a pointer to its PRM (Protected Resource Metadata)
- MCP client reads PRM to discover the authorization server and options
- MCP client redirects to the authorization server with its client ID
- User signs in
- Auth server returns an authorization code
- Client exchanges code for an access token
- Client retries original request with Bearer token
- MCP server validates token and succeeds

![Sequence diagram of the pre-registered OAuth flow between the user, VS Code MCP client, MCP server, and Microsoft Entra authorization server](https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEgVcZzoMOLfAjmNZwlJU9ccmSrkEybHnH-3DQ6LXScu_SfFcEil_3f7Zb8ejDV3B2nf8wii_I049m_mF0UHFX0Wkt7WuzTs2Nvp8aixOomLQMmO8lgz4592AlHWttotoGtLpDHiUrmqHSEvUkG4Jp9pHxhSXBfHaShyQ1uCAJhL0km6UgKr-kxJQE697g/s1600/Screenshot%202026-04-02%20at%201.48.04%E2%80%AFPM.png)

## Registering the MCP server with Entra

Before the server can authorize users, register it in Entra using an **app registration**. The author notes this can be done via Azure Portal, Azure CLI, Microsoft Graph SDK, or Bicep. The example uses the **Python Microsoft Graph SDK** to do it programmatically.

### Create the Entra app registration (protected resource + pre-authorized client)

The key part is configuring the MCP server as an API/protected resource with a delegated permission scope (here, `user_impersonation`) and adding **VS Code** as a **pre-authorized application**.

```python
scope_id = str(uuid.uuid4())

Application(
  display_name="Entra App for MCP server",
  sign_in_audience="AzureADMyOrg",
  api=ApiApplication(
    requested_access_token_version=2,
    oauth2_permission_scopes=[
      PermissionScope(
        admin_consent_description="Allows access to the MCP server as the signed-in user.",
        admin_consent_display_name="Access MCP Server",
        id=scope_id,
        is_enabled=True,
        type="User",
        user_consent_description="Allow access to the MCP server on your behalf.",
        user_consent_display_name="Access MCP Server",
        value="user_impersonation",
      )
    ],
    pre_authorized_applications=[
      PreAuthorizedApplication(
        app_id=VSCODE_CLIENT_ID,
        delegated_permission_ids=[scope_id],
      )
    ],
  ),
)
```

What the configuration is doing:

- `requested_access_token_version=2`: Entra can issue v1.0 or v2.0 tokens; FastMCP’s validator expects **v2.0**.
- `oauth2_permission_scopes`: defines a delegated permission scope (`user_impersonation`) so MCP clients can request tokens for your server.
- `pre_authorized_applications`: tells Entra that specific client apps (here, VS Code) are pre-approved to request tokens for your API without an extra consent prompt.

With that in place, VS Code requests the scope:

- `api://{app_id}/user_impersonation`

…and the FastMCP server validates that the incoming token contains that scope.

### Create a service principal

Create a [Service Principal](https://learn.microsoft.com/entra/identity-platform/app-objects-and-service-principals?tabs=browser) for the app registration:

```python
request_principal = ServicePrincipal(app_id=app.app_id, display_name=app.display_name)
await graph_client.service_principals.post(request_principal)
```

### Securing credentials for Entra app registrations

Options discussed:

- **Client secret**: easiest, but requires secure storage and rotation.
- **Certificate**: stronger, but needs lifecycle management.
- **Managed identity as Federated Identity Credential (MI-as-FIC)**: avoids secrets/certs and is generally best on Azure, but not available for local dev.

The author uses **two app registrations**:

- Local dev: client secret
- Production: managed identity with FIC, intended for **Azure Container Apps**

Create a password credential for local dev:

```python
password_credential = await graph_client.applications.by_application_id(app.id).add_password.post(
  AddPasswordPostRequestBody(
    password_credential=PasswordCredential(display_name="FastMCPSecret")
  )
)
```

Create the federated identity credential for the production app registration:

```python
fic = FederatedIdentityCredential(
  name="miAsFic",
  issuer=f"https://login.microsoftonline.com/{tenant_id}/v2.0",
  subject=managed_identity_principal_id,
  audiences=["api://AzureADTokenExchange"],
)

await graph_client.applications.by_application_id(prod_app_id).federated_identity_credentials.post(fic)
```

Then:

- Local `.env` points to the secret-secured local app registration
- Azure Container App environment variables point to the FIC-secured production app registration

### Granting admin consent (for OBO to downstream APIs)

This is needed only if the MCP server uses **on-behalf-of (OBO)** to exchange the incoming access token for a token to a downstream API like **Microsoft Graph**.

Because the VS Code sign-in flow does not include a separate user consent step for downstream Graph scopes, the post grants **admin consent** up front.

Admin consent example for Microsoft Graph scopes:

```python
server_principal = await graph_client.service_principals_with_app_id(app.app_id).get()

graph_principal = await graph_client.service_principals_with_app_id(
  "00000003-0000-0000-c000-000000000000"  # Microsoft Graph
).get()

await graph_client.oauth2_permission_grants.post(
  OAuth2PermissionGrant(
    client_id=server_principal.id,
    consent_type="AllPrincipals",
    resource_id=graph_principal.id,
    scope="User.Read email offline_access openid profile",
  )
)
```

## Using FastMCP servers with Entra

Configure FastMCP auth using a remote auth provider and an Entra JWT verifier:

```python
from fastmcp.server.auth import RemoteAuthProvider
from fastmcp.server.auth.providers.azure import AzureJWTVerifier

verifier = AzureJWTVerifier(
  client_id=ENTRA_CLIENT_ID,
  tenant_id=AZURE_TENANT_ID,
  required_scopes=["user_impersonation"],
)

auth = RemoteAuthProvider(
  token_verifier=verifier,
  authorization_servers=[f"https://login.microsoftonline.com/{AZURE_TENANT_ID}/v2.0"],
  base_url=base_url,
)
```

A key point: you do **not** pass a client secret here, even for local dev, because FastMCP validates tokens using Entra’s public keys.

### Middleware to capture the signed-in user ID

The post adds middleware to pull claims from the current access token and store the user’s Entra object ID (`oid`) in FastMCP state:

```python
class UserAuthMiddleware(Middleware):
  def _get_user_id(self):
    token = get_access_token()
    if not (token and hasattr(token, "claims")):
      return None
    return token.claims.get("oid")

  async def on_call_tool(self, context: MiddlewareContext, call_next):
    user_id = self._get_user_id()
    if context.fastmcp_context is not None:
      await context.fastmcp_context.set_state("user_id", user_id)
    return await call_next(context)

  async def on_read_resource(self, context: MiddlewareContext, call_next):
    user_id = self._get_user_id()
    if context.fastmcp_context is not None:
      await context.fastmcp_context.set_state("user_id", user_id)
    return await call_next(context)
```

Initialize the server with auth + middleware:

```python
mcp = FastMCP("Expenses Tracker", auth=auth, middleware=[UserAuthMiddleware()])
```

Now every request requires authentication; missing/invalid tokens yield a `401`, which triggers VS Code to start the MCP authorization flow.

![Screenshot of the VS Code prompt asking the user to sign in before using the authenticated MCP server](https://github.com/pamelafox/azure-cosmosdb-identity-aware-mcp-server/raw/main/readme_copilot_auth.png)

### Using the user ID inside a tool

Example tool usage: pull `user_id` from state and use it for user-scoped data access (example shows writing an expense item to Cosmos DB):

```python
@mcp.tool
async def add_user_expense(
  date: Annotated[date, "Date of the expense in YYYY-MM-DD format"],
  amount: Annotated[float, "Positive numeric amount of the expense"],
  description: Annotated[str, "Human-readable description of the expense"],
  ctx: Context,
):
  """Add a new expense to Cosmos DB."""
  user_id = await ctx.get_state("user_id")
  if not user_id:
    return "Error: Authentication required (no user_id present)"

  expense_item = {
    "id": str(uuid.uuid4()),
    "user_id": user_id,
    "date": date.isoformat(),
    "amount": amount,
    "description": description,
  }

  await cosmos_container.create_item(body=expense_item)
```

## Using OBO flow in FastMCP server

With admin consent granted, the server can use an OBO flow to call Microsoft Graph on behalf of the user.

### MSAL setup (local dev using client secret)

```python
from msal import ConfidentialClientApplication

confidential_client = ConfidentialClientApplication(
  client_id=entra_client_id,
  client_credential=os.environ["ENTRA_DEV_CLIENT_SECRET"],
  authority=f"https://login.microsoftonline.com/{os.environ['AZURE_TENANT_ID']}",
  token_cache=TokenCache(),
)
```

### MSAL setup (production using managed identity assertion / FIC)

```python
from msal import ManagedIdentityClient, TokenCache, UserAssignedManagedIdentity

mi_client = ManagedIdentityClient(
  UserAssignedManagedIdentity(client_id=os.environ["AZURE_CLIENT_ID"]),
  http_client=requests.Session(),
  token_cache=TokenCache(),
)

def _get_mi_assertion():
  result = mi_client.acquire_token_for_client(resource="api://AzureADTokenExchange")
  if "access_token" not in result:
    raise RuntimeError(
      f"Failed to get MI assertion: {result.get('error_description', 'unknown error')}"
    )
  return result["access_token"]

confidential_client = ConfidentialClientApplication(
  client_id=entra_client_id,
  client_credential={"client_assertion": _get_mi_assertion},
  authority=f"https://login.microsoftonline.com/{os.environ['AZURE_TENANT_ID']}",
  token_cache=TokenCache(),
)
```

### Exchange the MCP access token for a Graph token (OBO)

```python
graph_resource_access_token = confidential_client.acquire_token_on_behalf_of(
  user_assertion=access_token.token,
  scopes=["https://graph.microsoft.com/.default"],
)

graph_token = graph_resource_access_token["access_token"]
```

### Example: check group membership via Microsoft Graph

```python
client = httpx.AsyncClient()

url = (
  "https://graph.microsoft.com/v1.0/me/transitiveMemberOf/microsoft.graph.group"
  f"?$filter=id eq '{group_id}'&$count=true"
)

response = await client.get(
  url,
  headers={
    "Authorization": f"Bearer {graph_token}",
    "ConsistencyLevel": "eventual",
  },
)

data = response.json()
membership_count = data.get("@odata.count", 0)
is_admin = membership_count > 0
```

### Restrict tool visibility/usage (FastMCP 3.0 auth constraints)

FastMCP 3.0 can restrict tool visibility based on an authorization check. The post wraps the Graph check and uses it as a tool constraint:

```python
async def require_admin_group(ctx: AuthContext) -> bool:
  graph_token = exchange_for_graph_token(ctx.token.token)
  return await check_user_in_group(graph_token, admin_group_id)

@mcp.tool(auth=require_admin_group)
async def get_expense_stats(ctx: Context):
  """Get expense statistics. Only accessible to admins."""
  ...
```

FastMCP runs the check:

- when listing tools (to determine visibility)
- again when invoking the tool (a just-in-time check)

The author notes OBO can also be used inside tools for other Graph-backed actions (upload, email, etc.).

## Full example repository

Source code: [azure-cosmosdb-identity-aware-mcp-server](https://github.com/pamelafox/azure-cosmosdb-identity-aware-mcp-server)

Key files:

- [`auth_init.py`](https://github.com/pamelafox/azure-cosmosdb-identity-aware-mcp-server/blob/main/infra/auth_init.py): creates Entra app registrations (prod + local), defines `user_impersonation`, pre-authorizes VS Code, creates service principal, grants Graph admin consent.
- [`auth_postprovision.py`](https://github.com/pamelafox/azure-cosmosdb-identity-aware-mcp-server/blob/main/infra/auth_postprovision.py): adds the FIC after deployment so the container app’s managed identity can act as the production Entra app.
- [`main.py`](https://github.com/pamelafox/azure-cosmosdb-identity-aware-mcp-server/blob/main/servers/main.py): implements FastMCP server with `RemoteAuthProvider` + `AzureJWTVerifier`, and Microsoft Graph calls via OBO.


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-mcp-servers-with-entra-id-and-pre-authorized-clients/ba-p/4508453)

