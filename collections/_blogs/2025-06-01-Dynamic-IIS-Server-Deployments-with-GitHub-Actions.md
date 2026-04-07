---
feed_name: Michiel van Oudheusden's Blog
section_names:
- devops
external_url: https://mindbyte.nl/2025/06/02/dynamic-iis-server-deployments-github-actions.html
date: 2025-06-01 22:00:00 +00:00
tags:
- AWS
- AWS EC2
- AWS Load Balancer
- Blogs
- CI/CD
- CloudFormation
- DevOps
- Dynamic Matrix
- Gh API
- GitHub Actions
- GitHub API
- GitHub App
- GitHub CLI
- GitHub TOKEN
- Iis
- Jq
- Matrix Strategy
- Parallel Deployments
- Runner Labels
- Self Hosted Runners
- Windows
- Windows Server
- Workflow Outputs
- Workflow Security
author: Michiel van Oudheusden
title: Dynamic IIS Server Deployments with GitHub Actions
primary_section: devops
---

Michiel van Oudheusden explains how to deploy an IIS web app to multiple Windows servers by dynamically discovering labeled self-hosted GitHub runners and generating a GitHub Actions matrix so each server gets its own parallel deployment job.<!--excerpt_end-->

# Dynamic IIS Server Deployments with GitHub Actions

When migrating application infrastructure to the cloud, it’s common to end up with a mix of newer patterns (containers/Kubernetes) and “old-school” setups like IIS on Windows servers. In this scenario, a web application needed to be deployed across multiple IIS servers behind an AWS load balancer, and the goal was to run the same GitHub Actions deployment workflow on *every* server in a scalable way.

## Background

- Traffic is distributed by a load balancer to several Windows servers running IIS.
- The servers are AWS EC2 instances provisioned via CloudFormation.
- Each server automatically registers a GitHub **self-hosted runner**.

### The problem with `runs-on`

Using:

- `runs-on: [self-hosted, <label>]`

…only selects **one** matching runner. It does **not** run the job on *all* runners that match the label.

GitHub documents that `runs-on` selects a single runner; to execute across multiple machines you need a **matrix strategy**.

### Why not hard-code runner names?

Hard-coding runner names breaks scaling, because instances can be added/removed without updating the workflow.

### Why not “deployment groups”?

Azure DevOps has “deployment groups”, but GitHub Actions doesn’t offer a direct equivalent. The workaround is to:

1. Query the GitHub API for self-hosted runners
2. Filter runners by labels
3. Build a matrix dynamically at runtime

## Gathering runner information

### Token and permissions challenge

The default workflow `GITHUB_TOKEN` is repository-scoped, so it can’t list organization-wide runners.

Solution:

- Create a **GitHub App** with **Read access to organization self hosted runners**
- Install it on the repository
- Store credentials as repository secrets:
  - `RUNNER_TOKEN_APP_ID`
  - `RUNNER_TOKEN_APP_PRIVATE_KEY`

Those credentials are exchanged for a short-lived token that `gh` can use to query the runners API.

### `determine-runners` job

This job:

- Runs on `ubuntu-latest`
- Uses `peter-murray/workflow-application-token-action@v4` to obtain an org-scoped token
- Uses `gh api` + `jq` to:
  - list runners
  - filter by an environment label (e.g. `staging` / `production`)
  - filter by a fixed label (e.g. `IIS-Server`)
  - output a JSON array of runner names

```yaml
jobs:
  determine-runners:
    name: Determine target runners
    runs-on: ubuntu-latest
    outputs:
      runners_json: ${{ steps.get_runners.outputs.runners_json }}
    env:
      ORG_NAME: ${{ vars.ORG_NAME }}
      TARGET_ENV_LABEL: ${{ vars.TARGET_ENV_LABEL }}
      REQUIRED_LABEL_1: IIS-Server
    steps:
      - name: Get Organization API Token
        id: get_workflow_token
        uses: peter-murray/workflow-application-token-action@v4
        with:
          application_id: ${{ secrets.RUNNER_TOKEN_APP_ID }}
          application_private_key: ${{ secrets.RUNNER_TOKEN_APP_PRIVATE_KEY }}
          organization: ${{ env.ORG_NAME }}

      - name: Get matching runners (including offline)
        id: get_runners
        run: |
          # Retrieve all runners for the organization (up to 100 per page)
          runners_data=$(gh api "orgs/${ORG_NAME}/actions/runners?per_page=100" --jq '.runners[]')

          # Filter runners by both the fixed label and environment label
          matching_runners=$(echo "$runners_data" | jq -c \
            --arg label1 "${REQUIRED_LABEL_1}" \
            --arg label2 "${TARGET_ENV_LABEL}" \
            'select(.labels | map(.name) | (contains([$label1]) and contains([$label2])))')

          # Build a JSON array of runner names
          runners_json=$(echo "$matching_runners" | jq -cs '[.[].name]')

          if [ -z "$runners_json" ] || [ "$runners_json" == "[]" ]; then
            echo "::error::No runners found with labels: '${REQUIRED_LABEL_1}' and '${TARGET_ENV_LABEL}'."
            exit 1
          else
            echo "runners_json=$runners_json" >> $GITHUB_OUTPUT
          fi
        env:
          GH_TOKEN: ${{ steps.get_workflow_token.outputs.token }}
```

Source reference: GitHub workflow syntax docs: https://docs.github.com/en/actions/writing-workflows/workflow-syntax-for-github-actions

## Building the deployment matrix

Once `runners_json` is available, the deployment job uses `fromJson(...)` to convert the JSON string into a matrix array.

Each runner name becomes one parallel matrix entry—effectively “run this deployment job on every matching server.”

```yaml
deployment-web-app:
  name: Deploy to ${{ matrix.runner }} on ${{ env.TARGET_ENV_LABEL }}
  environment: ${{ env.TARGET_ENV_LABEL }}
  needs: [determine-runners]
  runs-on: ${{ matrix.runner }}
  strategy:
    fail-fast: false
    matrix:
      runner: ${{ fromJson(needs.determine-runners.outputs.runners_json) }}
  steps:
    - name: Clean Workspace Folder
      run: Remove-Item -Recurse -Force ${{ github.workspace }}\*

    - name: Download artifact
      uses: actions/download-artifact@v4

    # Additional steps to deploy the IIS website go here
```

In this case, the IIS deployment steps included:

- copying build artifacts
- stopping the IIS site
- replacing files
- restarting the service

Running these steps in parallel reduced total deployment time and avoided manual coordination.

## Lessons learned

- **Use labels strategically**: combine environment labels (`staging`, `production`) with a fixed role label (`IIS-Server`) to target the right machines.
- **Manage GitHub App credentials carefully**: losing the App installation or private key breaks the token exchange and halts deployments.
- **CLI tools are preinstalled** on GitHub-hosted runners: `gh` and `jq` were already available.
- **Account for pagination**: the sample only fetches the first 100 runners; larger orgs should implement paging.
- **Embrace parallelism**: parallel matrix runs speed up deployments and make it easier to see which server failed.

## Conclusion

GitHub Actions doesn’t provide deployment groups out of the box. By querying the GitHub API for self-hosted runners, filtering them by labels, and generating a dynamic matrix, you can deploy in parallel to all matching machines—even as servers come and go.

Although this post uses IIS as the example, the same dynamic-matrix approach can work for any scenario where you want to target machines by environment, role, or other labeling conventions.

[Read the entire article](https://mindbyte.nl/2025/06/02/dynamic-iis-server-deployments-github-actions.html)

