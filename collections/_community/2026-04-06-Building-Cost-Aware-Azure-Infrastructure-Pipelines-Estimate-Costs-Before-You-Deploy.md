---
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-cost-aware-azure-infrastructure-pipelines-estimate/ba-p/4508776
date: 2026-04-06 06:28:43 +00:00
title: 'Building Cost-Aware Azure Infrastructure Pipelines: Estimate Costs Before You Deploy'
tags:
- ARM Resource IDs
- Azure
- Azure App Service
- Azure Bicep
- Azure CLI
- Azure Retail Prices API
- Azure SQL Database
- Azure Virtual Machines
- Azure What If
- Azure/login
- Bicep What If
- Budget Thresholds
- CI/CD Gates
- Community
- Cost Management
- DevOps
- GitHub Actions
- IaC
- OData Filtering
- OIDC
- PR Comments
- Pull Request Automation
- Python
primary_section: azure
section_names:
- azure
- devops
author: whosocurious
---

whosocurious walks through building a cost-aware Azure IaC pipeline that estimates the monthly cost impact of Bicep changes in pull requests using Azure what-if, the Azure Retail Prices API, and GitHub Actions, then posts the results as a PR comment and optionally blocks merges over a threshold.<!--excerpt_end-->

# Building Cost-Aware Azure Infrastructure Pipelines: Estimate Costs Before You Deploy

Every developer who works with Infrastructure as Code (IaC) has seen this happen: a pull request tweaks a VM SKU, adds a resource, or scales a service, everything looks “correct”… and the next Azure bill shows a surprise.

This guide shows how to build a **cost-aware Azure infrastructure pipeline** that:

- Estimates the cost delta for IaC changes **before deployment**
- Posts the estimate as a **pull request comment**
- Optionally **blocks merges** when the delta exceeds a defined budget threshold

It uses:

- **Bicep** (IaC)
- **Azure what-if** deployment mode
- **Azure Retail Prices API**
- **GitHub Actions**
- A small **Python** script to calculate estimated monthly deltas

## The Problem: Cost Is a Blind Spot in IaC Reviews

Code reviews for Bicep or Terraform templates usually focus on correctness, security, and compliance. Cost often gets missed because:

- Developers don’t have easy access to pricing data during review
- Azure pricing varies by region, tier, reservation status, etc.
- There’s no built-in “cost diff” in most IaC tooling

The result: cost regressions slip through the way bugs do when there are no tests.

## Architecture Overview

The pipeline works as a sequence:

1. Run **Bicep what-if** to detect resource-level changes
2. Map those changes to SKU/service pricing via the **Azure Retail Prices API**
3. Summarize cost deltas and post them as a **PR comment**
4. Fail the workflow if the delta is above a threshold

## Step 1: Use Bicep What-If to Detect Changes

Azure’s what-if deployment mode previews what will be created/modified/deleted without deploying.

```bash
az deployment group what-if \
  --resource-group rg-myapp-prod \
  --template-file main.bicep \
  --parameters main.bicepparam \
  --result-format ResourceIdOnly \
  --out json > what-if-output.json
```

The JSON includes a `changes` array where each entry typically has:

- `resourceId` — full ARM resource ID
- `changeType` — `Create`, `Modify`, `Delete`, `NoChange`, `Deploy`
- `before` and `after` — resource properties for modifications

This output tells you **what** is changing so you can estimate **what it costs**.

## Step 2: Map Resources to Pricing with the Azure Retail Prices API

The **Azure Retail Prices API** is a free, unauthenticated REST API:

- Docs: https://learn.microsoft.com/en-us/rest/api/cost-management/retail-prices/azure-retail-prices
- Endpoint: `https://prices.azure.com/api/retail/prices`

Example Python function to estimate monthly VM cost (pay-as-you-go) by SKU and region:

```python
import requests

def get_vm_price(sku_name: str, region: str = "eastus") -> float | None:
    """Query the Azure Retail Prices API for a Linux VM's pay-as-you-go hourly rate."""
    api_url = "https://prices.azure.com/api/retail/prices"

    odata_filter = (
        f"armRegionName eq '{region}' "
        f"and armSkuName eq '{sku_name}' "
        f"and priceType eq 'Consumption' "
        f"and serviceName eq 'Virtual Machines' "
        f"and contains(meterName, 'Spot') eq false "
        f"and contains(productName, 'Windows') eq false"
    )

    response = requests.get(api_url, params={"$filter": odata_filter})
    response.raise_for_status()

    items = response.json().get("Items", [])
    if not items:
        return None

    hourly_rate = items[0]["retailPrice"]
    monthly_estimate = hourly_rate * 730  # avg hours per month
    return round(monthly_estimate, 2)

# Example usage
before_cost = get_vm_price("Standard_D4s_v5")  # e.g., $140.16/mo
after_cost = get_vm_price("Standard_D8s_v5")   # e.g., $280.32/mo

delta = after_cost - before_cost  # +$140.16/mo
```

You can extend this approach to other resource types (App Service Plans, Azure SQL databases, managed disks, etc.) by changing `serviceName` and other filter fields.

## Step 3: Build the GitHub Actions Workflow

This workflow:

- Triggers on PRs that touch `infra/**`
- Logs into Azure using **OIDC** (`azure/login@v2`)
- Runs **what-if** and saves JSON
- Runs a Python script to compute deltas
- Posts a sticky PR comment
- Fails the workflow if the delta exceeds `$500`

```yaml
name: Cost Estimate on PR

on:
  pull_request:
    paths:
      - "infra/**"

permissions:
  id-token: write # For Azure OIDC login
  contents: read
  pull-requests: write # To post comments

jobs:
  cost-estimate:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Azure Login (OIDC)
        uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

      - name: Run Bicep What-If
        run: |
          az deployment group what-if \
            --resource-group ${{ vars.RESOURCE_GROUP }} \
            --template-file infra/main.bicep \
            --parameters infra/main.bicepparam \
            --out json > what-if-output.json

      - name: Setup Python
        uses: actions/setup-python@v5
        with:
          python-version: "3.12"

      - name: Install dependencies
        run: pip install requests

      - name: Estimate cost delta
        id: cost
        run: |
          python infra/scripts/estimate_costs.py \
            --what-if-file what-if-output.json \
            --output-format github >> "$GITHUB_OUTPUT"

      - name: Comment on PR
        uses: marocchino/sticky-pull-request-comment@v2
        with:
          header: cost-estimate
          message: |
            ## Infrastructure Cost Estimate

            | Resource | Change | Before ($/mo) | After ($/mo) | Delta |
            |----------|--------|---------------|--------------|-------|
            ${{ steps.cost.outputs.table_rows }}

            **Estimated monthly impact: ${{ steps.cost.outputs.total_delta }}**

            _Prices are pay-as-you-go estimates from the Azure Retail Prices API. Actual costs may vary with reservations, savings plans, or hybrid benefit._

      - name: Gate on budget threshold
        if: ${{ steps.cost.outputs.delta_value > 500 }}
        run: |
          echo "::error::Monthly cost increase exceeds $500 threshold. Requires finance team approval."
          exit 1
```

## Step 4: The Cost Estimation Script

Core script (`infra/scripts/estimate_costs.py`) responsibilities:

- Parse the what-if output
- Map ARM resource types to Retail Prices API `serviceName`
- Extract SKUs (from `before/after`)
- Query prices and compute monthly deltas
- Emit GitHub Actions outputs for a markdown table and totals

```python
#!/usr/bin/env python3
"""Parse Bicep what-if output and estimate cost deltas using Azure Retail Prices API."""

import json
import argparse
import requests

PRICE_API = "https://prices.azure.com/api/retail/prices"

# Map ARM resource types to Retail API service names
RESOURCE_TYPE_MAP = {
    "Microsoft.Compute/virtualMachines": "Virtual Machines",
    "Microsoft.Compute/disks": "Storage",
    "Microsoft.Web/serverfarms": "Azure App Service",
    "Microsoft.Sql/servers/databases": "SQL Database",
}

def get_price(service_name: str, sku: str, region: str) -> float:
    """Query Azure Retail Prices API and return monthly cost estimate."""
    odata_filter = (
        f"armRegionName eq '{region}' "
        f"and armSkuName eq '{sku}' "
        f"and priceType eq 'Consumption' "
        f"and serviceName eq '{service_name}'"
    )

    resp = requests.get(PRICE_API, params={"$filter": odata_filter})
    resp.raise_for_status()

    items = resp.json().get("Items", [])
    if not items:
        return 0.0

    return items[0]["retailPrice"] * 730

def parse_what_if(filepath: str) -> list[dict]:
    """Extract resource changes from what-if JSON output."""
    with open(filepath) as f:
        data = json.load(f)

    results = []

    for change in data.get("changes", []):
        change_type = change.get("changeType", "")

        resource_type = change.get("resourceId", "").split("/providers/")[-1].split("/")[0:2]
        resource_type_str = "/".join(resource_type) if len(resource_type) == 2 else ""

        if resource_type_str not in RESOURCE_TYPE_MAP:
            continue

        before_sku = (change.get("before") or {}).get("sku", {}).get("name", "")
        after_sku = (change.get("after") or {}).get("sku", {}).get("name", "")
        region = (change.get("after") or change.get("before") or {}).get("location", "eastus")

        service = RESOURCE_TYPE_MAP[resource_type_str]

        before_price = get_price(service, before_sku, region) if before_sku else 0.0
        after_price = get_price(service, after_sku, region) if after_sku else 0.0

        results.append({
            "resource": change.get("resourceId", "").split("/")[-1],
            "change_type": change_type,
            "before": round(before_price, 2),
            "after": round(after_price, 2),
            "delta": round(after_price - before_price, 2),
        })

    return results

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--what-if-file", required=True)
    parser.add_argument("--output-format", default="text", choices=["text", "github"])
    args = parser.parse_args()

    changes = parse_what_if(args.what_if_file)
    total_delta = sum(c["delta"] for c in changes)

    if args.output_format == "github":
        rows = []
        for c in changes:
            sign = "+" if c["delta"] >= 0 else ""
            rows.append(
                f"| {c['resource']} | {c['change_type']} "
                f"| ${c['before']:.2f} | ${c['after']:.2f} "
                f"| {sign}${c['delta']:.2f} |"
            )

        print(f"table_rows={'chr(10)'.join(rows)}")

        sign = "+" if total_delta >= 0 else ""
        print(f"total_delta={sign}${total_delta:.2f}/mo")
        print(f"delta_value={total_delta}")

    else:
        for c in changes:
            print(
                f"{c['resource']}: {c['change_type']} "
                f"${c['before']:.2f} → ${c['after']:.2f} "
                f"(Δ ${c['delta']:+.2f})"
            )
        print(f"\nTotal monthly delta: ${total_delta:+.2f}")

if __name__ == "__main__":
    main()
```

## What the Developer Experience Looks Like

When the pipeline is active, PRs that touch infrastructure automatically get a cost summary comment, for example:

| Resource | Change | Before ($/mo) | After ($/mo) | Delta |
| --- | --- | --- | --- | --- |
| vm-api-prod | Modify | $140.16 | $280.32 | +$140.16 |
| disk-data-01 | Create | $0.00 | $73.22 | +$73.22 |
| plan-webapp | NoChange | $69.35 | $69.35 | +$0.00 |

**Estimated monthly impact: +$213.38/mo**

If the delta exceeds a configured limit (example: `$500/mo`), the workflow fails and requires explicit action—similar to a failing test.

## Extending This Further

Ideas for next steps:

1. Support **Azure Savings Plans and Reservations** (query `priceType eq 'Reservation'` and show both pay-as-you-go and committed pricing)
2. Track **cost trends over time** (store estimates in Azure Table Storage or a database and dashboard the cost trajectory)
3. Add **Slack/Teams notifications** when PRs exceed the threshold
4. Add **tag-based cost allocation** (parse resource tags from Bicep and attribute costs to teams/projects)
5. Run **multi-environment estimates** (dev/staging/prod parameter files)

## Key Takeaways

- **Azure what-if** provides a deployment preview that you can treat as the foundation for pre-deployment validation.
- The **Azure Retail Prices API** is free, unauthenticated, and queryable for granular pricing info.
- Cost gates in CI/CD treat budget overruns like test failures—merge blockers that require explicit approval.
- “Shift cost left”: catching cost issues during PR review is cheaper than finding them on the monthly bill.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-cost-aware-azure-infrastructure-pipelines-estimate/ba-p/4508776)

