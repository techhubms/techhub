# Managed DevOps Pools

Have you ever had to deploy, configure, and maintain your own DevOps agents, be it for Azure DevOps or GitHub? If so, then you probably found out it is such a hassle to keep everything up-to-date and up-and-running.

Managed DevOps Pools have recently been announced as Public Preview. In this article, we go over the most important features and capabilities of the new service and provide examples on how to implement this using Infrastructure as Code with Terraform.

## Managed DevOps Pools, what are they?

Managed DevOps Pools are Microsoft-hosted agents that can be configured to use private networking. This allows the agents to use private DNS zones, private endpoints, your own Azure Firewall (or an appliance) and with the added benefit of having Microsoft maintain these resources.

The Managed DevOps Pools also allow you to specify which SKU, Disks and OS Images you want to use. So whether you need specific compute power, a lot of disk space, or to use a specific OS Image (even from the Azure Marketplace), Managed DevOps Pools enable you to customize the agent pool to your needs. 

There are two options to use when it comes to private networking. It can be configured to use an isolated network, which is supplied and managed by Microsoft, or it can use an existing virtual network within the Azure tenant. The added benefit of the latter is that you are able to configure routing, DNS server configuration, and Network Security Groups, managing allowed and denied traffic more specifically for your needs.

There is another option that is quite powerful: you can use the Managed DevOps Pools for multiple Azure DevOps organizations and/or multiple projects within these organizations. Some companies use multiple DevOps organizations, i.e. one (1) for production and one (1) for sandbox environments. These can then all use the same Managed DevOps Pool.

### So what's the managed part then?

Although you have to deploy some of the Azure resources yourself, the compute instances behind the scenes are managed by Microsoft. This will actually save you a ton of time and headaches (in my case). The downside of this behind the scenes management is you will not be able to see parts of the solution and therefore troubleshooting might become harder.

### Summary

The benefits of Managed DevOps Pools are that while Microsoft will manage them, they are directly usable within your private networking. It has access to lots of images to be used for the agents, be it the Microsoft images, marketplace images, or even custom created images.

## Three key takeways for using Managed DevOps pools

By leveraging Managed DevOps Pools, you will have more time to spend on tasks that provide business value, and you will get the same amount of ease and security as you would with self-hosted agents.

1. Focus on business value - This service enables me to focus on delivering my business value, instead of maintaining and managing my self-hosted agents. I can automatically use the latest Microsoft hosted agent versions, without having to checkout the repository and build my custom images based on the Microsoft image repository.

2. Simplified administrative tasks - Not worrying about the compute instances of the Virtual Machine Scale Set, since those are maintained and managed by Microsoft as a Platform-as-a-Service (PaaS) offering. Also, the Azure DevOps agent pool configuration is done by the Managed DevOps Pool creation, so there is no need to configure Azure DevOps or wait for the Azure DevOps administrator to help out.

3. Managed Agents but with private networking - The Managed DevOps Pools can directly use an available virtual network in the Azure tenant, allowing for better access to other services without the need to open up any services to the public internet, as you would have to using Microsoft-hosted agents.

## Implementing DevOps Pools using Infrastructure as Code and Terraform

Ok, so let's start with identifying every service that we need and how it all works together. 

Our objective is to set up a Managed DevOps Pool that is able to use private networking, linked to our networking hub, and use the same (Ubuntu) image as the Microsoft-hosted agents.

We prefer to use Infrastructure as Code (IaC) to minimize human failure and to create solutions that can be built, changed and managed in a consistent and repeatable way. My preferred IaC language is Terraform, so we will be using Terraform to deploy the resources.

This means we will have to perform the following tasks:

1. `Basic terraform setup` - We need to initialize Terraform and the basic repository to be able to deploy the Azure resources
2. `Request or update Quotas` - Managed DevOps Pool quotas are set to 0 by default, so we will need to request a quota increase in order to use the Managed DevOps Pools
3. `Create a resource group` - All Azure resources must be deployed into a resource group, so we will create a resource group for the Managed DevOps Pool resources
4. `Create a DevCenter and create a project` - DevCenter is a collection of projects with similar settings. It can be used to supply catalogs with Infrastructure as Code templates, which are available for all projects in the DevCenter, as well as creating development environments for development teams to use
5. `Create a Virtual network, peering to the central hub and create a subnet` - To enable private networking, we will need to create a virtual network with a subnet
6. `Create the Managed DevOps Pool` - The Managed DevOps Pool will create the compute instances that can run the Azure DevOps jobs, and we will supply the correct resource configuration based on the steps before

### Basic terraform setup

When using Terraform, we will need to supply the provider information and configuration. We will be using both the `AzureRM` and the `AzAPI` providers. Details on using the `AzAPI` provider are described in the sections where it applies.

```tf
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.0.1"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "1.15.0"
    }
  }
}

provider "azurerm" {
  subscription_id = "{insert your subscription ID here}"
  features {}

  resource_provider_registrations = "Extended"
  resource_providers_to_register  = ["Microsoft.DevCenter", "Microsoft.DevopsInfrastructure"]
}

provider "azapi" {}
```

Notice the [new annotation for resource providers](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs#resource_provider_registrations), which is introduced in the latest `AzureRM` version.

Also notice the providers, which are required to be set on the subscription so the subscription is activated to use the resources within the subscription:

- The `Microsoft.DevCenter` provider allows the creation and usage of the DevCenter resource and the projects in the DevCenter. 
- The `Microsoft.DevOpsInfrastructure` provider enables the subscription to create and deploy the Managed DevOps Pools.

### Managed DevOps Pools Quotas

One important thing to understand before deploying, is that we will need to request a quota increase. These quotas are specifically for Managed DevOps Pools, so your normal Virtual Machine SKU quotas are not valid for the Managed DevOps Pools SKUs. 

You can request the quotas as you would normally do with other quotas, via the Azure Portal on the subscription page:

![Usage + quotas](./images/usage-and-quota.png)

Make sure you select the Provider `Managed DevOps Pools` to see the quotas:

![Managed DevOps Pools - quotas](./images/provider-quotas.png)

If you do not know how to do this or if the button `New Quota Request` is greyed out, please reach out to your platform engineers or CSP to help you out.

### Variables and locals

I am using a `variables.tf` and `locals.tf` file to determine certain values to be used in the deployment, which I will briefly explain in this section.

The variables.tf describes all the variables that need to be supplied to run the Terraform deployment. The full file is available in the GitHub repository (available in the links section), but below two variables are shown:

```tf
variable "scaffold_company_short_name" {
  description = "Abbreviation of the company name to make all Azure resources unique within the Azure Tenant."
  type        = string

  validation {
    condition     = length(var.scaffold_company_short_name) <= 6
    error_message = "The company short name must be 6 characters or less."
  }
}

variable "devops_organization_url" {
  description = "The URL of the Azure DevOps organization to add the Managed DevOps Pool to."
  type        = string
}
```

These variables are used to provide input to re-use the deployment for multiple projects, customers or purposes. 

The naming convention is provided in the `locals.tf` file.

```tf
locals {
  rgName               = "rg-${var.scaffold_company_short_name}-devpool-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
  vnetName             = "vnet-${var.scaffold_company_short_name}-devpool-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
  devCenterName        = "devc-${var.scaffold_company_short_name}-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
  devCenterProjectName = "devpr-${var.scaffold_company_short_name}-devpool-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
  snetName             = "snet-${var.scaffold_company_short_name}-devpool-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
  poolName             = "pool-${var.scaffold_company_short_name}-devpool-${var.scaffold_environment}-${var.scaffold_location_short_name}-001"
}
```

Note that the `devCenterName` value does not contain the `-devpool` section, this is done to stay within the naming length restriction of 26 characters. We have validations on the variables to ensure this naming convention cannot surpass the length restriction.

The locals provide the naming convention that I like to use for this solution. Feel free to change them to your needs or preference accordingly. The naming convention is based on the [Cloud Adoption Framework naming convention](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming). Feel free to overwrite these when other naming conventions should apply.

### Create the resource group

Like every Azure resource deployment, we start with creating a `Resource Group` to place all the Azure resources in.

```tf
resource "azurerm_resource_group" "rg" {
  name     = local.rgName
  location = var.scaffold_location

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
```

### Create Dev Center resource and a Dev Center Project

The basis of the Managed DevOps Pools is the `Dev Center` resource, along with a `DevCenter Project`. 

As described briefly earlier, the DevCenter is a collection of projects with similar settings. It can be used to supply catalogs with Infrastructure as Code templates, which are available for all projects in the DevCenter, as well as creating development environments for development teams to use.
A DevCenter Project is contained part that can be made available to specific teams and resources, i.e. Dev Boxes, Deployment Environments or Managed DevOps Pools.

```tf
resource "azurerm_dev_center" "devcenter" {
  name                = local.devCenterName
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}
```

The DevCenter name cannot be longer than 26 characters, since we created the variables with validations, we should not reach this number. However, when you update the sample code and update the naming convention or variables, be aware you might reach this naming length restriction.

```tf
resource "azurerm_dev_center_project" "devcenter_project" {
  name                = local.devCenterProjectName
  dev_center_id       = azurerm_dev_center.devcenter.id
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}
```

### Create the virtual network and subnet

Now that we have the Dev Center set up, we can move forward to create the `virtual network` and `subnet` to be used by the Managed DevOps Pool to allow for private networking. 

```tf 
resource "azurerm_virtual_network" "vnet" {
  name                = local.vnetName
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  address_space       = [var.vnet_devpool_ip_range]
  dns_servers         = var.vnet_dns_servers
}

# Optionally peer the virtual network to the Virtual Hub
resource "azurerm_virtual_hub_connection" "agents" {
  depends_on = [azurerm_virtual_network.vnet]
  count      = var.virtual_hub_id != null ? 1 : 0

  name                      = "conn-${local.vnetName}"
  internet_security_enabled = true
  virtual_hub_id            = var.virtual_hub_id
  remote_virtual_network_id = azurerm_virtual_network.vnet.id
}
```

Since I am always using a Cloud Platform, I want to link my virtual network to the centralized hub. You can choose to not use this resource, by not providing a value (or providing the value `null`) to the variable `virtual_hub_id`, if you do not want to use any connection with a Virtual Hub. If you want to know more about what a hub is, please have a look at the [Microsoft Learn pages about hub-and-spokes as part of the Cloud Adoption Framework](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/hub-spoke-network-topology).

```tf
resource "azapi_resource" "snet" {
  depends_on = [azurerm_virtual_network.vnet]

  name      = local.snetName
  type      = "Microsoft.Network/virtualNetworks/subnets@2023-11-01"
  parent_id = azurerm_virtual_network.vnet.id

  body = jsonencode({
    properties = {
      addressPrefix = var.vnet_devpool_ip_range
      delegations = [
        {
          name = "Microsoft.DevOpsInfrastructure/pools"
          properties = {
            serviceName = "Microsoft.DevOpsInfrastructure/pools"
          }
        }
      ]
    }
  })
}
```

Next we use the earlier refered to `AzAPI` resource in order to create a delegated subnet for the Managed DevOps pool. We need this AzAPI resource because the `AzureRM` provider does not provide a known terraform configuration for the `Microsoft.DevOpsInfrastructure/pools` delegation service name. Using the `AzAPI` resource we can pass our delegation information conveniently.

### Creating the Managed DevOps Pools

Now we have the fundamental resources we are going to create the Managed DevOps Pool, again using `AzAPI` provider.

```tf
resource "azapi_resource" "pool" {
  name      = local.poolName
  type      = "microsoft.devopsinfrastructure/pools@2024-04-04-preview"
  location  = azurerm_resource_group.rg.location
  parent_id = azurerm_resource_group.rg.id

  body = jsonencode({
    properties = {
      organizationProfile = {
        organizations = [
          {
            projects    = var.devops_projects
            url         = var.devops_organization_url
            parallelism = var.agent_maximumConcurrency
          }
        ]
        kind = "AzureDevOps" # Currently only AzureDevOps is supported
        permissionProfile = {
          kind = "CreatorOnly" # Can also be set to "Inherit" or "SpecificAccounts"
          # If you want to use specific accounts, you can add them here using the users and groups properties
          # users = [
          #   "Patrick.deKruijf@xebia.com"
          # ]
          # groups = []
        }
      }

      devCenterProjectResourceId = azurerm_dev_center_project.devcenter_project.id

      maximumConcurrency = var.agent_maximumConcurrency

      agentProfile = {
        kind = "Stateless" # I would recommend setting this to "Stateless", since this ensures a fresh agent is used for each job.
        #   kind             = "Stateful"
        #   maxAgentLifetime = "7.00:00:00" # Property is required when set to "Stateful"

        # If you do not want to turn off scaling, remove the complete resourcePredictionsProfile block
        # There is also a "Manual" option, which allows you to set the minimum and maximum number of agents based on a schedule.
        resourcePredictionsProfile = {
          predictionPreference = "MostCostEffective" # There are 5 options, ranging from "MostCostEffective" to "MostPerformance"
          kind                 = "Automatic"         # Can also be set to Manual or 
        }

      }

      fabricProfile = {
        sku = {
          name = "Standard_D2ads_v5"
        }

        images = [
          {
            aliases            = ["ubuntu-22.04"]
            buffer             = "*"
            wellKnownImageName = "ubuntu-22.04/latest"
          },
          # You can add more images if needed, also referencing resource IDs for images
          # {
          #   resourceId = "/Subscriptions/5ab24a52-44e0-4bdf-a879-cc38371a4403/Providers/Microsoft.Compute/Locations/westeurope/Publishers/canonical/ArtifactTypes/VMImage/Offers/0001-com-ubuntu-server-focal/Skus/20_04-lts-gen2/versions/latest",
          #   buffer     = "*"
          # }
        ]

        osProfile = {
          # Not much to configure here just yet, but Microsoft is working on adding Key Vault support too
          secretsManagementSettings = {
            observedCertificates = [],
            keyExportable        = false
          },
          logonType = "Service" # Can also be set to "Interactive"
        },

        # If you want to use an isolated network, remove the complete networkProfile block
        networkProfile = {
          subnetId = azapi_resource.snet.id
        }

        storageProfile = {
          osDiskStorageAccountType = "Premium", # Standard, StandardSSD, Premium
          dataDisks = [
            # Create additional data disks if needed
            # {
            #   diskSizeGiB        = 100
            #   caching            = "ReadWrite"
            #   storageAccountType = "StandardSSD_LRS"
            #   driveLetter        = "Z"
            # }
          ]
        },

        kind = "Vmss" # Currently only "Vmss" is supported
      }
    }
  })
}
```

For each of the properties a section is created below to explain what each setting expects, what the options are and when you should use a certain option. 

#### Available properties

##### organizationProfile

```powershell
organizationProfile = {
  organizations = [
    {
      projects    = var.devops_projects # This field accepts an Array[] of projects that should be able to use the Managed DevOps Pool
      url         = var.devops_organization_url # This should be the URL of a Azure DevOps organization (i.e. https://dev.azure.com/{organizationName})
      parallelism = var.agent_maximumConcurrency # This setting sets the maximum amount of concurrent agents that can be used in parallel
    }
  ]
  kind = "AzureDevOps" # Currently only AzureDevOps is supported, hopefully we can see GitHub here soon too
  permissionProfile = {
    kind = "CreatorOnly" # Can also be set to "Inherit" or "SpecificAccounts"
    users = [ # If you want to use specific accounts, you can add them here using the users and groups properties
      "Patrick.deKruijf@xebia.com"
    ]
    groups = [] # If you want to use specific accounts, you can add them here using the users and groups properties
  }
}
```
##### devCenterProjectResourceId

```powershell
devCenterProjectResourceId = azurerm_dev_center_project.devcenter_project.id
```

##### maximumConcurrency

```powershell
maximumConcurrency = var.agent_maximumConcurrency
```

##### agentProfile

```powershell
agentProfile = {
  kind = "Stateless" # You can use either "Stateful" or "Stateless". I would recommend setting this to "Stateless", since this ensures a fresh agent is used for each job.
  #   maxAgentLifetime = "7.00:00:00" # Property is required when the kind property is set to "Stateful"

  # If you do not want to turn off scaling, remove the complete resourcePredictionsProfile block
  # There is also a "Manual" option, which allows you to set the minimum and maximum number of agents based on a schedule.
  resourcePredictionsProfile = {
    predictionPreference = "MostCostEffective" # There are 5 options, ranging from "MostCostEffective" to "MostPerformance"
    kind                 = "Automatic"         # Can also be set to Manual 
  }
}
```

#####

```powershell
fabricProfile = {
  sku = {
    name = "Standard_D2ads_v5" # Make sure this SKU is allowed based on the Managed DevOps Pool quotas
  }

  images = [
    {
      aliases            = ["ubuntu-22.04"]
      buffer             = "*"
      wellKnownImageName = "ubuntu-22.04/latest"
    },
    # You can add more images if needed, also referencing resource IDs for images
    # {
    #   resourceId = "/Subscriptions/5ab24a52-44e0-4bdf-a879-cc38371a4403/Providers/Microsoft.Compute/Locations/westeurope/Publishers/canonical/ArtifactTypes/VMImage/Offers/0001-com-ubuntu-server-focal/Skus/20_04-lts-gen2/versions/latest",
    #   buffer     = "*"
    # }
  ]

  osProfile = {
    # Not much to configure here just yet, but Microsoft is working on adding Key Vault support too
    secretsManagementSettings = {
      observedCertificates = [],
      keyExportable        = false
    },
    logonType = "Service" # Can also be set to "Interactive"
  },

  # If you want to use an isolated network, remove the complete networkProfile block
  networkProfile = {
    subnetId = azapi_resource.snet.id # This is the resource ID of the virtual network you want to have the Managed DevOps Pool connect to
  }

  storageProfile = {
    osDiskStorageAccountType = "Premium", # Standard, StandardSSD, Premium
    dataDisks = [
      # Create additional data disks if needed
      {
        diskSizeGiB        = 100
        caching            = "ReadWrite"
        storageAccountType = "StandardSSD_LRS"
        driveLetter        = "Z"
      }
    ]
  },

  kind = "Vmss" # Currently only "Vmss" is supported
}
```

### Required traffic rules (firewall or network security groups)

In order to all the resources actually work, you will need to allow traffic to specific domains. So these domains needs to be allowed from a network perspective to make the Managed DevOps Pool functional.

Endpoints that the Managed DevOps Pool service depends on:
- `*.prod.manageddevops.microsoft.com` - Managed DevOps Pools endpoint
- `rmprodbuilds.azureedge.net` - Worker binaries
- `vstsagentpackage.azureedge.net` - Azure DevOps agent CDN location
- `*.queue.core.windows.net` - Worker queue for communicating with Managed DevOps Pools service
- `server.pipe.aria.microsoft.com` - Common client side telemetry solution (and used by the Agent Pool Validation extension among others)
- `azure.archive.ubuntu.com` - Provisioning Linux machines - this is ***HTTP***, not HTTPS
- `www.microsoft.com` - Provisioning Linux machines
- `packages.microsoft.com` - Provisioning Linux machines
- `ppa.launchpad.net` - Provisioning Ubuntu machines
- `dl.fedoraproject.org` - Provisioning certain Linux distros

Needed by Azure DevOps agent:
- `dev.azure.com`
- `*.services.visualstudio.com`
- `*.vsblob.visualstudio.com`
- `*.vssps.visualstudio.com`
- `*.visualstudio.com `- These entries are the minimum domains required. If you have any issues, see [Azure DevOps allowlist](https://learn.microsoft.com/en-us/azure/devops/organizations/security/allow-list-ip-url) for the full list of domains required.

For more information regarding the outbound traffic, please see [this Microsoft learn page](https://learn.microsoft.com/en-us/azure/devops/managed-devops-pools/configure-networking?view=azure-devops&tabs=azure-portal#restricting-outbound-connectivity).

### Start the deployment

To start the deployment, we will need to provide the deployment with the values for the variables required. See an example file below:

```bash
scaffold_location               = "westeurope"
scaffold_environment            = "production"
scaffold_environment_short_name = "prod"
scaffold_location_short_name    = "weu"
scaffold_company_short_name     = "{insert-your-company-short-name}"

virtual_hub_id           = "/subscriptions/{insert-your-hub-subscription-id}/resourceGroups/{insert-your-hub-resource-group-name}/providers/Microsoft.Network/virtualHubs/{insert-your-virtual-hub-name}"
vnet_devpool_ip_range    = "{insert-your-ip-range}"
vnet_dns_servers         = ["{insert-your-dns-server-ip}"]
agent_maximumConcurrency = 2 # This is the maximum number of agents that can run concurrently, keep in mind the SKU quota you have on your subscription
devops_organization_url  = "https://dev.azure.com/{insert-your-organization-name}"
devops_projects          = ["{inset-your-project-name}", "{insert another project name}"]
```

Since we're using Terraform, we can simply run the following commands:
```bash
terraform init # This will initialize the backend settings and install the required providers
terraform validate # This will check if all 
terraform plan --var-file=test.tfvars # This will execute a 'dry-run' to see what would be created, modified or removed, using the tfvars-file referenced
terraform apply --var-file=test.tfvars # This will firstly do another dry-run, asking a 'yes' to continue with the actual deployment, using the tfvars-file referenced
```

Note that this will create a local Terraform state file, which is fine for my demo purposes. When you are using this is a production environment, please update the state backend accordingly.

For the explaination of the deployment and required steps, we use the commands directly into a terminal. We prefer and recommend to use Azure Pipelines or GitHub Actions to deploy Infrastructure as Code. 

## Cost of using DevOps Pools

Managed DevOps Pools pricing is determined by the cost of the Azure services your pool uses, like compute, storage, and data egress, combined with the standard Azure DevOps Services pricing for self-hosted agents.

- `Azure Services` - The Managed DevOps Pool uses Azure resources to supply the functionality. These resources will be billed to your Azure subscription. During public preview there are no extra costs for the Managed DevOps Pools resource itself
- `Azure DevOps Services` - The cost for the DevOps Services are tied to the costs of self-hosted agents, this means that you will have to pay for parallel jobs. The first parallel job is free, then it is set to $15 per additional parallel job.

Please note that parallel jobs are shared between all pipelines and pool in your organization.

## Conclusion

In my opinion using Managed DevOps Pools is definately worth it. You get the ease of mind, because it is a PaaS offering. It directly integrates into your private network within your Azure tenant, allowing for better and safer connections. And with the code repository, you will get a quick starter template to be able to use it. A Managed DevOps Pool is only available for the teams that have access to it and you can created multiple pools with different settings, i.e. a CPU-intensive pool and a Memory-intensive pool for different teams. 

Microsoft is also considering adding support for container-based agents to improve on-demand spin up times. I am excited to see the improvements in the future!

## Additional information

### Origin story

In case you want to read more about the origin story, please read the [story by Suraj Gupta and Elize Tarasila](https://devblogs.microsoft.com/engineering-at-microsoft/managed-devops-pools-the-origin-story)

### Links

- [Code Repository](https://github.com/patrick-de-kruijf/managed-devops-pools)
- [Microsoft Learn](https://learn.microsoft.com/en-us/azure/devops/managed-devops-pools/?view=azure-devops)
- [Origin Story](https://devblogs.microsoft.com/engineering-at-microsoft/managed-devops-pools-the-origin-story/)
- [Public preview announcement](https://devblogs.microsoft.com/devops/managed-devops-pools/)