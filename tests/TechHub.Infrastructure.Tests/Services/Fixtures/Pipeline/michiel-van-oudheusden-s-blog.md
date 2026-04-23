June 2, 2025
 
 

 

 
 
 

 
 
 
 5 minute read
 
 
 
 

 
 
 

When we began migrating our application infrastructure to the cloud, we relied on AWS and traditional Windows servers running IIS. Although containers and Kubernetes dominate today’s conversations, there are organizations that still depend on familiar, “old-school” architectures. In one recent project, we found ourselves needing to deploy a web application across multiple IIS servers behind an AWS load balancer. Our challenge was simple in statement but complex in execution: how do you run the same GitHub Actions deployment workflow on every server in a scalable, maintainable way?

## Background

Our environment consisted of a load balancer distributing traffic to several Windows servers hosting IIS. These servers (EC2 instances) were spun up via CloudFormation, each automatically registering a GitHub self-hosted runner. In essence, every server became a target for GitHub Actions workflows.

To deploy across the entire group of servers with the same tag (for example, all IIS-Server machines), we needed the workflow to run on each of them, not just on a single available runner. While setting `runs-on: [self-hosted, ]` directs GitHub Actions to run the job on a runner matching that label, it does not instruct it to execute on all such servers simultaneously.

As [documented by GitHub](https://docs.github.com/en/actions/writing-workflows/workflow-syntax-for-github-actions), `runs-on` selects only one matching runner; to execute a job on multiple machines, you need to use a matrix strategy.

Hard-coding runner names would defeat our goal of dynamic scaling, since servers could be added or removed without updating the workflow.

We looked for an approach that would adapt dynamically as servers joined or left our environment. Azure DevOps offered “[deployment groups](https://learn.microsoft.com/en-us/azure/devops/pipelines/release/deployment-groups/deploying-azure-vms-deployment-groups?view=azure-devops)”, but GitHub Actions does not include a direct equivalent. Instead, we needed to leverage the GitHub API to query our organization’s runners at runtime, filter them by environment labels, and build a dynamic matrix of targets for our workflow. This blog post describes the story of how we achieved that goal.

## Gathering Runner Information

The first obstacle we encountered was obtaining an authentication token capable of listing all self-hosted runners in our organization. The default `GITHUB_TOKEN` provided to workflows only has repository scope, so it cannot retrieve organization-wide runner details. Our solution was to create a GitHub App with “Read access to organization self hosted runners”, then install it on our repository. By storing the App’s ID and private key as repository secrets (`RUNNER_TOKEN_APP_ID` and `RUNNER_TOKEN_APP_PRIVATE_KEY`), we could exchange these credentials for a short-lived token that the GitHub CLI (`gh`) could use to query the runners API.

Below is the YAML snippet for the `determine-runners` job. It runs on `ubuntu-latest`, invokes a third-party action to fetch an application token, then uses `gh api` and `jq` to filter runners with a specific environment label (e.g., `staging` or `production`) and a fixed label (e.g., `IIS-Server`):

```
jobs:
 determine-runners:
 name: Determine target runners
 runs-on: ubuntu-latest
 outputs:
 runners_json: $
 env:
 ORG_NAME: $
 TARGET_ENV_LABEL: $
 REQUIRED_LABEL_1: IIS-Server
 steps:
 - name: Get Organization API Token
 id: get_workflow_token
 uses: peter-murray/workflow-application-token-action@v4
 with:
 application_id: $
 application_private_key: $
 organization: $

 - name: Get matching runners (including offline)
 id: get_runners
 run: |
 # Retrieve all runners for the organization (up to 100 per page)
 runners_data=$(gh api "orgs/$/actions/runners?per_page=100" --jq '.runners[]')

 # Filter runners by both the fixed label and environment label
 matching_runners=$(echo "$runners_data" | jq -c \
 --arg label1 "$" \
 --arg label2 "$" \
 'select(.labels | map(.name) | (contains([$label1]) and contains([$label2])))'
 )

 # Build a JSON array of runner names
 runners_json=$(echo "$matching_runners" | jq -cs '[.[].name]')

 if [ -z "$runners_json" ] || [ "$runners_json" == "[]" ]; then
 echo "::error::No runners found with labels: '$' and '$'."
 exit 1
 else
 echo "runners_json=$runners_json" >> $GITHUB_OUTPUT
 fi
 env:
 GH_TOKEN: $

```

## Building the Deployment Matrix

Once we had a JSON list of runner names in the output variable `runners_json`, we could construct a dynamic matrix for our deployment job. The second job, `deployment-web-app`, depends on `determine-runners`. It uses `fromJson(...)` to transform the JSON string into an array for the matrix. Each array element corresponds to a runner name, resulting in one parallel job per server.

```
 deployment-web-app:
 name: Deploy to $ on $
 environment: $
 needs: [determine-runners]
 runs-on: $
 strategy:
 fail-fast: false
 matrix:
 runner: $
 steps:
 - name: Clean Workspace Folder
 run: Remove-Item -Recurse -Force $\*

 - name: Download artifact
 uses: actions/download-artifact@v4

 # Additional steps to deploy the IIS website go here

```

In our case, deploying to IIS involved copying build artifacts, stopping the IIS site, replacing the files, and restarting the service. By running these steps on each runner in parallel, we reduced total deployment time and avoided manual coordination.

## Lessons Learned

When we first set out, we worried that this dynamic approach would introduce more complexity than it solved. However, by automating the discovery of runners, we created a resilient pipeline that adapts as servers come online or go offline. Below are some key takeaways:

 
- 
 

**Use Labels Strategically**

By combining environment labels (e.g., `staging`, `production`) with a fixed label (e.g., `IIS-Server`), we narrowed down the runner list precisely. Labels become powerful selectors when applied consistently.

 
 
- 
 

**Manage GitHub App Credentials Carefully**

The process requires a GitHub App with appropriate permissions. You must secure its private key and ensure it remains installed on your repository. Losing this App or its credentials would break the token exchange and halt deployments.

 
 
- 
 

**CLI Tools Preinstalled**

We used a GitHub-hosted runner to run this workflow, which already has `gh` and `jq` installed by default.

 
 
- 
 

**Account for Pagination**

Our example handles only the first 100 runners. If your organization has more, implement pagination logic by iterating over pages until no runners remain.

 
 
- 
 

**Embrace Parallelism**

Running deployments in parallel saved us time and reduced the risk of inconsistent state. We could also monitor each runner’s status separately, making troubleshooting easier.

 

## Conclusion

GitHub Actions does not natively support a construct like deployment groups. By querying the GitHub API, filtering runners with `jq`, and building a dynamic matrix, we created a solution that scales automatically as servers change. Although it requires extra setup, a GitHub App, additional CLI tools, and careful label management, the result is a more flexible, resilient deployment pipeline.

While this post uses IIS server deployments as the example, the same technique can be applied to any deployment scenario. By defining tags or labels on your runners and building a dynamic matrix, you can select machines based on any condition, such as environment, role, or geographic region, and run workflows in parallel across them.