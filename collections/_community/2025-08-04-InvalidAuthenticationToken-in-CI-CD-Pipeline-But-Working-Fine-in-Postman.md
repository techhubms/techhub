---
external_url: https://www.reddit.com/r/AZURE/comments/1mhac2s/invalidauthenticationtoken_in_cicd_pipeline_but/
title: InvalidAuthenticationToken in CI-CD Pipeline But Working Fine in Postman
author: sayytoabhishekkumar
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-04 11:01:26 +00:00
tags:
- Access Token
- Authentication
- Azure REST API
- CI/CD
- Get AzAccessToken
- Invoke RestMethod
- Pipeline
- Postman
- PowerShell
section_names:
- azure
- devops
- security
---
Authored by sayytoabhishekkumar, this post explores an 'InvalidAuthenticationToken' issue encountered during Azure REST API calls from a CI-CD pipeline—even though the equivalent request functions as expected in Postman.<!--excerpt_end-->

## Problem Overview

The author describes an authentication issue when calling the Azure Management API as part of a CI/CD pipeline. Despite using a token and URI that work flawlessly in Postman, invoking the same request through PowerShell in the pipeline results in an `InvalidAuthenticationToken` error.

## PowerShell Script Used

```powershell
$baseUrl = "https://management.azure.com"
$token = (Get-AzAccessToken -ResourceUrl $baseUrl).Token
$RId = (Get-AzResource -ResourceGroupName $resourceGroupName -Name $queryPackName).ResourceId
$restAPi = "$baseUrl$RId/savedSearches?api-version=2025-12-01"
$response = Invoke-RestMethod -Uri $restAPi -Method Get -Headers @{Authorization = "Bearer $token"}
```

## Issue Details

- **In Pipeline**: Running the above code in a CI-CD pipeline results in an `InvalidAuthenticationToken` error from Azure.
- **Manual (Postman)**: Extracting the `$restAPi` URL and `$token` value and using them directly in Postman works, returning the expected results.

## Analysis & Context

- The code assembles an Azure Management REST API endpoint and retrieves an access token with `Get-AzAccessToken`.
- The token is inserted into the authorization header for a `GET` REST call.
- The disparity suggests either a difference in token scope, environment variables, identity context, or how the CI pipeline is configured versus the interactive environment used for Postman.

## Solutions and Troubleshooting Steps (Implied)

- Check if the identity used in the pipeline matches the one used for generating the token manually.
- Ensure that the Service Principal (or Managed Identity) running in the CI/CD environment has sufficient permissions on the Azure resources.
- Compare token contents (claims, audiences) between those obtained in the pipeline and in your local environment/postman for discrepancies.
- Double-check that `Get-AzAccessToken` in the pipeline is not returning a cached, expired, or otherwise invalid token.
- Validate that the base URL and resource IDs are formed correctly and consistently in both environments.

## References

- [Original Reddit post](https://www.reddit.com/r/AZURE/comments/1mhac2s/invalidauthenticationtoken_in_cicd_pipeline_but/)

## Summary

The main challenge lies in inconsistent authentication behaviors between automated CI/CD pipeline runs and manual attempts through Postman, likely stemming from environmental, identity, or token configuration differences.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhac2s/invalidauthenticationtoken_in_cicd_pipeline_but/)
