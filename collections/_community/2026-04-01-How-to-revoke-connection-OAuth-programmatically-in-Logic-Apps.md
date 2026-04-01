---
title: How to revoke connection OAuth programmatically in Logic Apps
date: 2026-04-01 11:02:44 +00:00
author: Omar_Abu_Arisheh
feed_name: Microsoft Tech Community
primary_section: azure
section_names:
- azure
- security
tags:
- API Connections
- App Registration
- ARM REST API
- AuthorizationFailed
- Azure
- Azure Logic Apps
- Azure RBAC
- Azure Resource Manager
- Bearer Token
- Client Credentials Flow
- Community
- Contributor Role
- Curl
- Custom RBAC Role
- Invoke An HTTP Request
- Least Privilege
- Logic Apps Consumption
- Microsoft Entra ID
- Microsoft.Web/connections
- OAuth 2.0
- Postman
- Revoke Connection Keys
- Security
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/how-to-revoke-connection-oauth-programmatically-in-logic-apps/ba-p/4506825
---

Omar_Abu_Arisheh shows how to revoke an Azure Logic Apps API Connection OAuth token programmatically by calling the Azure Management (ARM) revokeConnectionKeys API, including options using a Logic App HTTP action, Postman, or cURL, and the RBAC permissions you need.<!--excerpt_end-->

## Overview

In some cases, you might need to revoke an API Connection OAuth setup in Azure Logic Apps without using the Azure portal **Revoke** button. You can do this by calling the Azure Management (ARM) API action that revokes the connection keys.

The ARM endpoint used throughout is:

https://management.azure.com/subscriptions/[SUBSCRIPTION_ID]/resourceGroups/[RESOURCE_GROUP]/providers/Microsoft.Web/connections/[NAME_OF_CONNECTION]/revokeConnectionKeys?api-version=2018-07-01-preview

## Option 1: Use Logic Apps “Invoke an HTTP request”

1. Identify the **API Connection name** you want to revoke.
2. Create a **Logic App (Consumption)**.
3. Add a trigger of your choice.
4. Add the **Invoke an HTTP request** action.
5. Create/select a connection in the **same tenant** that has access to the target connection.
6. Configure the action to call the revoke endpoint:

   - Method: `POST`
   - URL:

     https://management.azure.com/subscriptions/[SUBSCRIPTION_ID]/resourceGroups/[RESOURCE_GROUP]/providers/Microsoft.Web/connections/[NAME_OF_CONNECTION]/revokeConnectionKeys?api-version=2018-07-01-preview

7. Run a test; it should succeed.

## Option 2: Use an App Registration (works for Logic Apps, Postman, etc.)

### 1) Prepare the App Registration

Create or use an **App Registration** and ensure it has the required API permissions for Azure management (`https://management.azure.com/`).

### 2) Get a Bearer token (client credentials flow)

Request a token from:

https://login.microsoftonline.com/[TENANT_ID]/oauth2/v2.0/token

Use this body (x-www-form-urlencoded):

Client_Id=[CLIENT_ID_OF_THE_APP_REG]&Client_Secret=[CLIENT_SECRET_FROM_APP_REG]&grant_type=client_credentials&scope=https://management.azure.com/.default

Header:

- `Content-Type: application/x-www-form-urlencoded`

### 3) (Logic Apps) Parse the token response

If you’re doing this in Logic Apps:

1. Add a **Parse JSON** action.
2. Use the body from the token HTTP call as input.
3. Use this schema:

```json
{
  "properties": {
    "access_token": { "type": "string" },
    "expires_in": { "type": "integer" },
    "ext_expires_in": { "type": "integer" },
    "token_type": { "type": "string" }
  },
  "type": "object"
}
```

### 4) Call the revoke API with the token

Call the revoke endpoint again, this time including an `Authorization` header:

- Header key: `Authorization`
- Header value: `Bearer <ACCESS_TOKEN>`

Endpoint:

https://management.azure.com/subscriptions/[SUBSCRIPTION_ID]/resourceGroups/[RESOURCE_GROUP]/providers/Microsoft.Web/connections/[NAME_OF_CONNECTION]/revokeConnectionKeys?api-version=2018-07-01-preview

## Option 3: Use cURL

### 1) Request the token

```bash
curl -X POST \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=[CLIENT_ID_OF_APP_REG]" \
  -d "scope=https://management.azure.com/.default" \
  -d "client_secret=[CLIENT_SECRET_FROM_APP_REG]" \
  -d "grant_type=client_credentials" \
  "https://login.microsoftonline.com/[TENANT_ID]/oauth2/v2.0/token"
```

Use the `access_token` field from the response as your Bearer token.

### 2) Call the revoke API

```bash
curl -X POST "https://management.azure.com/subscriptions/[SUBSCRIPTION_ID]/resourceGroups/[RESOURCE_GROUP]/providers/Microsoft.Web/connections/[NAME_OF_CONNECTION]/revokeConnectionKeys?api-version=2018-07-01-preview" \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"key":"value"}'
```

## Authorization/RBAC requirements and common error

If you get an error like:

```json
{
  "error": {
    "code": "AuthorizationFailed",
    "message": "The client '[App_reg_client_id]' with object id '[App_reg_object_id]' does not have authorization to perform action 'Microsoft.Web/connections/revokeConnectionKeys/action' over scope '/subscriptions/[subscription_id]/resourceGroups/[resource_group_id]/providers/Microsoft.Web/connections/[connection_name]' ..."
  }
}
```

You need to grant the app registration permissions on the scope that contains the API Connection:

- Simple fix: assign **Contributor** on the **resource group** that contains the API Connection.

### Least-privilege alternative: custom RBAC role

Create a **custom RBAC role** that includes these actions, then assign it to the **app registration’s object ID** at the appropriate scope:

```json
{
  "actions": [
    "Microsoft.Web/connections/read",
    "Microsoft.Web/connections/write",
    "Microsoft.Web/connections/delete",
    "Microsoft.Web/connections/revokeConnectionKeys/action"
  ]
}
```


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/how-to-revoke-connection-oauth-programmatically-in-logic-apps/ba-p/4506825)

