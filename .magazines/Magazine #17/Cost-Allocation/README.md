# Ensuring Effective Cost Allocation in Azure

## Introduction to Cost Allocation 

I have been in organizations where we received a monthly email with a massive Excel spreadsheet, asking who owns which cost and how we could reduce it. As you can imagine, this process repeated itself because no one knew or claimed ownership. This is why it is crucial that the costs reported by the cloud provider and visible on the cloud bill can be linked to the right owners.

Cost allocation is the set of practices used to divide up a consolidated cloud invoice among those responsible for its various components. In the context of FinOps, this involves dividing the cloud service provider invoices among the different IT groups within an organization.

Why is this important? Cost allocation is essential for showback and chargeback. By bringing the costs back to individual projects or teams within an organization, we achieve financial transparency and accountability. This, in turn, helps optimize costs, as it becomes clear who owns what resources and who is responsible for managing and reducing expenses.

Cost allocation is achieved through a combination of functional activities, primarily focusing on using a consistent hierarchy of accounts, projects, subscriptions, resource groups, and other logical groupings of resources. This also includes applying resource-level metadata—tags or labels—within the cloud service provider or through a third-party FinOps platform.

## Cost Allocation in Azure

Azure resources are always part of a resource group. Resource groups belong to a subscription, which in turn roll up into management groups. This tree-like structure provides a flexible framework for setting up various cost allocation schemes.

![](images/image-17.png)

https://learn.microsoft.com/en-us/azure/cost-management-billing/costs/cost-allocation-introduction

### Management Groups

Management groups can be used to map subscriptions to different business units or environments. This top-level organization helps in categorizing costs at a broad level.

### Subscriptions

Subscriptions are a crucial partition in Azure. They can be used for access control and governance, but also serve as a fundamental unit for cost allocation. Each subscription can be assigned to a specific department, project, or environment, allowing for clear visibility into spending.

### Resource Groups and Tags

Within a subscription, resource groups can contain multiple resources. Resource groups support tagging, which involves assigning key/value pairs to resources. Tags can be used to specify details like project name, business unit, or cost center.

## Tag Inheritance

By utilizing the [tag inheritance feature](https://learn.microsoft.com/en-us/azure/cost-management-billing/costs/enable-tag-inheritance), tags applied to a resource group can be inherited by the resources within the group. This ensures consistent tagging across all resources, including the ones which do not expose tags, simplifying cost allocation and reporting.

![](images/automatically-apply-tags-new-usage-data-2.png)

Tag inheritance helps in maintaining uniformity and reducing the administrative overhead of manually tagging each resource.

## Enforcing Tags with Azure Policies

Azure Policies can be used to enforce the use of tags. Policies can ensure that resources are tagged correctly upon creation and can even block the creation of resources that do not comply with the tagging strategy. Azure Policies provide a mechanism to automate governance and ensure compliance with organizational standards, helping maintain a structured and manageable cloud environment.

By leveraging the hierarchical structure of management groups, subscriptions, and resource groups, along with effective tagging and enforcement policies, you can achieve precise cost allocation in Azure. This structure allows for detailed tracking and accountability, making it easier to manage and optimize cloud spending.

## Shared Cost Allocation

In most organizations, systems are not operated in isolation but rely on shared components such as API Management, hub/spoke networks, virtual machines, and more. It would be unfair if the team delivering these shared services bore all the costs, while the teams consuming them paid nothing and perceived them as free services. Therefore, it’s crucial to implement showback and potentially chargeback mechanisms for these shared costs.

### Showback and Chargeback

**Showback** involves tracking and reporting the usage and associated costs of shared services to the consuming teams. **Chargeback** goes a step further, where the consuming teams are billed for their usage. Both practices promote financial transparency and accountability, ensuring that all teams are aware of and responsible for their consumption of shared services.

### Configuring Cost Allocation Rules in Azure

If you are an Enterprise Agreement or Microsoft Customer Agreement user, Azure allows you to configure cost allocation rules. Here’s how you can set it up:

#### Select Sources and Targets

- Identify the source subscriptions, resource groups, or tags representing the shared services whose costs need to be allocated.

- Identify the target subscriptions, resource groups, or tags representing the consuming teams or projects that should share these costs.

#### Determine Allocation Method

- **Even Distribution**: Spread the costs evenly across all targets.

- **Percentage-Based Distribution**: Allocate costs based on a predefined percentage for each target.

- **Usage-Based Distribution**: Allocate costs based on actual usage metrics such as compute, storage, or network consumption. This method ensures a fairer distribution of costs based on how much each team or project actually uses the shared services.

![](images/cost-distribution.png)

### Important Considerations

While Azure allows you to configure cost allocation rules for showback purposes, it does not change the actual billing. If you want to implement chargeback, where consuming teams are billed for their usage, you will need to handle this outside of Azure using your organization's internal billing and accounting systems.

By implementing shared cost allocation, you ensure that all teams are aware of and accountable for their usage of shared services. This promotes a culture of cost-consciousness and helps optimize the overall cloud spending in your organization.

## Validating and Analyzing Cost Allocation Setup

Now that you have created a cost allocation setup, whether through management groups, subscriptions, or tagging on resource groups, it’s essential to validate and analyze the setup to ensure accuracy and effectiveness.

### Validating Tagging with Azure CLI

To validate that your resources are correctly tagged, you can use the Azure CLI. The following command will list all the resource groups along with the "owner" or "creator" tags. If these tags are not set, it will indicate 'missing':

```bash
az group list --query "[].{name:name, Owner:tags.owner || tags.Owner || tags.creator || tags.Creator || 'missing'}" -o table
```

This command will output a table showing each resource group with the specified tag names. Adjust the tag names as needed to match your organization’s tagging conventions.

### Using Azure Cost Analysis

Once your resources are tagged, the next step is to use Azure Cost Analysis to view and filter your costs:

#### Navigate to Azure Cost Management + Billing

- Go to the Azure Portal and select "Cost Management + Billing."

- Open the "Cost Analysis" section.

- Use the filtering options to filter costs by tags. This allows you to see the cost distribution based on the tags applied to your resources.

Note that [tags do not work retroactively](https://learn.microsoft.com/en-gb/azure/cost-management-billing/costs/understand-cost-mgt-data#how-tags-are-used-in-cost-and-usage-data). If a tag is applied to a resource today, it will not affect the cost data for previous dates. Ensure that tags are applied to the resources themselves, not just the resource groups. Use tag inheritance or Azure Policies to enforce consistent tagging across all resources.

### Limitations and Considerations

While Azure’s native tools provide powerful features for cost allocation, they do have some limitations:

- Non-Retroactive Tags: Tags applied today will not impact historical cost data.

- Complex Filtering: Azure’s native tools may not support complex filtering and multi-dimensional cost allocation needs.

These limitations might warrant the use of third-party tools, which offer more advanced capabilities, such as:

- Backdating Resources: Ability to apply tags and allocate costs for historical data.

- Multi-Dimensional Analysis: Advanced filtering options to allocate costs based on multiple dimensions and criteria.

By validating your tagging setup and using Azure Cost Analysis, you can ensure accurate and effective cost allocation. This process helps maintain financial transparency and accountability, allowing for better cloud cost management and optimization.
