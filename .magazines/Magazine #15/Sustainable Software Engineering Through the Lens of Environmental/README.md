# Sustainable Software Engineering Through the Lens of Environmental
Did you know that sustainable software engineering is a topic we frequently discuss and engage with? However, our conversations predominantly revolve around the economic dimension, such as optimizing costs in cloud computing, or the technical dimension, particularly when addressing code maintainability. But were you aware that sustainable software engineering encompasses five distinct dimensions?

![Dimensions Sustainable Software Engineering.](./images/sustainablesoftwareengineeringdimensions.png "Dimensions Sustainable Software Engineering.")
Source: https://se.ewi.tudelft.nl/research-lines/sustainable-se/

The remaining three dimensions are individual, social, and environmental. In this article, we will shift our focus to the latter dimension and explore how we can nurture a greener environment through software engineering, paving the way for 'GreenOps' (yes, another 'Ops' term!). The best part? It's not as daunting as it may seem!

## Sustainable Software Engineering Environmental Dimension?
When addressing sustainable software engineering within the environmental context, we are essentially examining the software's impact on the environment. This impact can be substantial. For instance, were you aware that operational software contributes to a significant 2-3% of global CO2 emissions (https://techmonitor.ai/focus/tech-industry-carbon-emissions-progress)? To put it into perspective, this level of emissions is on par with that of the aviation industry (https://www.iea.org/energy-system/transport/aviation). Consequently, when we delve into the realm of sustainable software engineering with environmental concerns in focus, our primary objective revolves around reducing CO2 emissions. We can achieve this, for example, by optimizing hardware utilization to minimize e-waste or by enhancing the energy efficiency of our software. These are the key areas we will explore in this article. The good news is that we don't have to start entirely from scratch.

## The Green Software Foundation and Microsoft's Well Architected Framework
The Green Software Foundation (GSF) is a non-profit organization dedicated to mitigating the environmental impact of software (https://greensoftware.foundation/). They achieve this mission by furnishing a framework for green software engineering.

![The Green Software Foundation.](./images/thegreensoftwarefoundation.png "The Green Software Foundation.")

This framework rests upon six core principles:

- Carbon Efficiency: Strive to emit the least possible amount of carbon.
- Energy Efficiency: Endeavor to use the minimum amount of energy necessary.
- Carbon Awareness: Adjust operations based on electricity cleanliness; do more when it's cleaner and less when it's dirtier.
- Hardware Efficiency: Minimize the embodied carbon in hardware usage.
- Measurement: Understand that what you cannot measure, you cannot improve.
- Climate Commitment: Gain a deep understanding of the precise mechanisms behind carbon reduction.

Additionally, GSF has meticulously documented cloud-agnostic patterns aligned with each principle. These principles and patterns have been integrated into practices that can be readily applied within Microsoft's Azure cloud infrastructure, following the Well Architected Framework. This comprehensive framework outlines best practices for the development of cloud-native applications and includes an entire section dedicated to sustainable workloads. Moving forward in this article, we will delve into these principles, patterns, and practices on Azure. Without further delay, let's commence with an exploration of the first principle, an accompanying pattern, and a tangible implementation on the Azure platform.

## Carbon Efficiency
Carbon efficiency pertains to the efficiency of a process, product, or organization in minimizing carbon emissions while still achieving its objectives. This constitutes a fundamental principle within environmental sustainability initiatives and endeavors to combat climate change. The central concept revolves around the minimization of the carbon footprint linked to any unit of work to the greatest extent possible.

### Pattern: Data Lifecycle Management
One effective pattern to strive for in order to achieve carbon efficiency is data lifecycle management. Data lifecycle management encompasses the comprehensive process of overseeing data from its inception to its deletion. This entails careful consideration of how data is stored, processed, and analyzed. The primary objective of data lifecycle management is to ensure the efficient and effective utilization of data while concurrently minimizing its environmental footprint, particularly within the context of this article. This emphasis on efficient data management stems from the realization that both the processing and storage of data consume energy, consequently contributing to carbon emissions.

#### Practice: Azure Storage Lifecycle Management
Azure Storage lifecycle management provides a rule-based policy that enables you to manage blob data by transitioning it to the appropriate access tiers or expiring data when it reaches the end of its lifecycle. Using this lifecycle management policy, you can:

- Swiftly transition blobs from cool or cold or archive storage tiers to hot storage when they are accessed, optimizing for performance.
- Move current versions of a blob, previous versions, or blob snapshots to a cooler storage tier if these objects have not been accessed or modified for a specified period, thereby optimizing for cost.
- Automatically delete current versions of a blob, previous versions, or blob snapshots when they reach the end of their respective lifecycles.
- Establish the aformentioned rules to be executed daily at the storage account level.
- Apply rules selectively to containers or a subset of blobs using criteria such as name prefixes or blob index tags.

Lifecycle management policies can be automated. Here's an example of how to achieve this in Terraform. 

```terraform
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "random_id" "id" {
  byte_length = 8
}

resource "azurerm_resource_group" "sustainability-example" {
  name     = "rg-${random_id.id.hex}"
  location = "West Europe"
}

resource "azurerm_storage_account" "sustainability-example" {
  name                = "sa${random_id.id.hex}"
  resource_group_name = azurerm_resource_group.sustainability-example.name

  location                 = azurerm_resource_group.sustainability-example.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "BlobStorage"
}

resource "azurerm_storage_container" "sustainability-example" {
  name                  = "examplecontainer"
  storage_account_name  = azurerm_storage_account.sustainability-example.name
  container_access_type = "private"
}

resource "azurerm_storage_management_policy" "sustainability-example" {
  storage_account_id = azurerm_storage_account.sustainability-example.id

  rule {
    name    = "MoveToCoolStorage"
    enabled = true
    filters {
      prefix_match = [azurerm_storage_account.sustainability-example.name]
      blob_types   = ["blockBlob"]
    }
    actions {
      base_blob {
        tier_to_cool_after_days_since_modification_greater_than = 2
      }
      snapshot {
        delete_after_days_since_creation_greater_than = 2
      }
    }
  }
}
```

This example provides all the essential components to kickstart your project. It initiates the process by generating a Resource Group suffixed with a random number, utilizing the Terraform resource `random_id`. Subsequently, it establishes a Storage Account with the account kind set to `BlobStorage`. Within this Storage Account, a container is created.

The pivotal segment of this setup lies at the end, where we leverage the Terraform resource `azurerm_storage_management_policy` to define a lifecycle management policy for the previously created Storage Account. This policy incorporates a rule that triggers an action to transfer the blob to the 'cool' access tier if it remains unmodified for a duration of 2 days, aptly named `MoveToCoolStorage`. Additionally, it implements an action to remove snapshots after a 2-day period.

This comprehensive configuration will set the foundation for efficient data management within your Azure environment.

## Energy Efficiency
The principle of energy efficiency is founded on the concept that we can curtail our energy consumption by employing the minimal amount of energy required to attain equivalent or superior outcomes. Consequently, this approach aids in diminishing our carbon footprint, as energy consumption serves as a reliable proxy for carbon emissions.

### Pattern: Energy Proportional Computing
Energy proportional computing is a pattern that aims to reduce energy consumption in computing systems by ensuring that the energy consumption of the system is proportional to its workload. 

#### Practice: Azure Container Apps Jobs
Achieving energy proportional computing is possible by leveraging Azure Container Apps Jobs, which allow you to execute containerized tasks for a defined duration before terminating. These jobs can be employed for various tasks, including data processing, machine learning, or any scenario requiring on-demand processing. Each job execution typically handles a single unit of work.

![Azure Container Apps Jobs.](./images/AzureContainerAppsJobs.png "Azure Container Apps Jobs.")

Job executions can commence manually, follow a predefined schedule, or trigger in response to specific events. These jobs encompass various tasks, such as on-demand batch processes and scheduled activities. Essentially, when the system's workload is light, it should consume minimal energy, and when the workload intensifies, its energy consumption should proportionally increase. This scalability in energy consumption, aligned with workload fluctuations, exemplifies an energy-efficient system.

## Carbon Awareness
The principle of carbon awareness revolves around the concept that we can diminish our carbon emissions and contribute to combating climate change by aligning our energy consumption with the availability of clean energy sources. In essence, this entails utilizing more energy during periods when renewable sources, such as wind and solar, are producing the highest electricity output, while conserving energy during times when fossil fuels are the primary energy generation source.

### Pattern: Process when the carbon intensity is low
Carbon intensity is a measure of the amount of carbon dioxide (CO2) emitted per unit of energy produced. This pattern involves scheduling computing workloads to run during times when the carbon intensity of the energy grid is low. The carbon intensity of the energy grid varies depending on factors such as the time of day, weather conditions, and energy demand.

#### Practice: Use Carbon Aware SDK CLI in your pipeline to deploy to regions with low carbon intensity
The Carbon Aware SDK CLI is a valuable tool designed to assist developers in deploying their applications to regions characterized by low carbon intensity. Its functionality hinges on the analysis of carbon intensity across various regions, enabling the deployment of applications to the region exhibiting the least carbon intensity. This strategic approach substantially contributes to minimizing the overall carbon footprint associated with the application.

However, it's essential to acknowledge that this solution may not be suitable for every workload. Organizational policies may exist that prohibit deployments in certain regions, thereby limiting its applicability. As an illustrative scenario, consider a situation where you're deploying Template Specs and conducting automated testing within your Azure pipeline. In such cases, the Carbon Aware SDK can be employed to ensure that test Template Specs are deployed in regions characterized by low carbon intensity, aligning with sustainability objectives.

## Hardware Efficiency
The principle of hardware efficiency is centered around the reduction of embodied carbon during both the production and utilization of hardware. Consumption related to cloud-based services, encompassing servers, network cables, and other components, contributes significantly to embodied carbon. Sustainable software engineering in cloud environments can play a pivotal role in assisting businesses in curbing their carbon footprint by optimizing hardware usage to minimize embodied carbon to the greatest extent possible.

### Pattern: Use Spot Instances When Possible
Spot instances represent a feature available from cloud computing providers like Amazon Web Services (AWS) and Google Cloud Platform (GCP), enabling users to leverage surplus computing capacity. Typically, these instances are referred to as "spot" instances due to their availability and pricing that can vary based on demand. The outcome is substantial cost savings, particularly for workloads that don't have stringent time constraints and can withstand interruptions. This approach also enhances the utilization of hardware resources, leading to a reduction in embodied carbon, which aligns well with the themes explored in this article.

#### Practice: Spot Containers on Azure
Azure Container Instances (ACI) Spot Containers offer the capability to execute interruptible workloads in a containerized format, leveraging unused Azure capacity. They combine the simplicity of ACI with the cost-effectiveness associated with Spot VMs, which is particularly noteworthy as they come at a lower cost compared to regular ACI containers. Azure Container Instances Spot Containers offer support for both Linux and Windows containers, ensuring flexibility across various operating system environments. It's important to note that, unlike Spot VMs, you can't select eviction types or policies for these containers. In the event of an eviction, the container groups hosting the workloads are automatically restarted without any manual intervention. However, it's crucial to acknowledge that spot containers may not be suitable for all types of workloads, as they can be interrupted at any time. Therefore, it's advisable to design your applications to gracefully handle interruptions. As an example, consider a scenario where:

![Azure Container Instances Spot Containers.](./images/AzureContainerInstancesSpotContainers.png "Azure Container Instances Spot Containers.")

You have a Queue Storage containing messages, and a Function App triggered by the Queue Storage. The Function App performs various processing tasks, such as storing a blob in a Blob Storage, before completing its execution by acknowledging the message in the queue. In the event the spot container is evicted before the Function App can finish processing, the message will not be acknowledged and will remain in the queue. This illustrates how interruptible workloads can be structured.

## Measurement
This principle underscores the significance of collecting and analyzing data related to energy consumption, infrastructure utilization, and performance metrics. By doing so, businesses can pinpoint areas where they can optimize both their software and infrastructure to reduce their environmental impact.

### Pattern: Carbon Emissions Tracking
By actively tracking carbon emissions, software engineers can identify specific areas where emissions can be curtailed and then take steps to mitigate their environmental footprint. The key components of this process include establishing a baseline, measuring emissions, analyzing the resulting data, and generating reports that inform actionable measures.

#### Practice: Emissions Impact Dashboard on Azure 
The Emissions Impact Dashboard, a tool provided by Microsoft Azure, facilitates the monitoring and analysis of carbon emissions linked to the use of Azure services. This dashboard offers a comprehensive array of metrics and visualizations that aid in comprehending the carbon footprint associated with service usage and identifying opportunities for emission reductions. A section of the dashboard is showcased below:

![Emissions Impact Dashboard on Azure.](./images/EmissionsImpactDashboardPowerBI.png "Emissions Impact Dashboard on Azure.")

Within this section of the dashboard, users can view carbon emissions data across their usage, thereby obtaining insights into the carbon intensity score per subscription. This initial data can serve as a starting point for identifying areas where optimization can be pursued.

## Climate Commitment
In the context of sustainable development, climate commitment necessitates that businesses proactively engage in efforts to diminish their carbon footprint. This commitment may encompass the establishment of carbon reduction targets, the adoption of energy-efficient technologies and practices, and investments in renewable energy sources. Pertaining to cloud computing, climate commitment holds significant relevance for sustainable software engineering. Through a comprehensive understanding of the precise mechanisms behind carbon reduction, businesses can effectively determine the most advantageous strategies for minimizing their carbon footprint in relation to their utilization of cloud services.

### Pattern: Define Policies
Policies constitute a set of rules governing the behavior of a system. They are instrumental in enforcing security, compliance, and other requirements. In the realm of sustainable software engineering, policies can be harnessed to outline the desired behavior of a system with regard to carbon emissions.

#### Practice: Azure Policy
One practical approach involves establishing an Azure Policy initiative focused on "Sustainability". This initiative can encompass various policies, as illustrated in the example below using Terraform:

```terraform
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_policy_definition" "energy_efficient_vm_sizes" {
  name         = "energy-efficient-vm-sizes"
  policy_type  = "Custom"
  mode         = "Indexed"
  display_name = "Energy Efficient VM Sizes"

  metadata = jsonencode({
    version  = "1.0.0"
    category = "Sustainability"
  })

  policy_rule = <<POLICY_RULE
{
  "if": {
    "allOf": [
      {
        "field": "type",
        "equals": "Microsoft.Compute/virtualMachines"
      },
      {
        "not": {
          "field": "Microsoft.Compute/virtualMachines/sku.name",
          "in": ["Standard_B1ls", "Standard_B1s", "Standard_B1ms", "Standard_B2s", "Standard_B2ms"]
        }
      }
    ]
  },
  "then": {
    "effect": "deny"
  }
}
POLICY_RULE
}

resource "azurerm_policy_definition" "renewable_energy_regions" {
  name         = "renewable-energy-regions"
  policy_type  = "Custom"
  mode         = "Indexed"
  display_name = "Renewable Energy Regions"

  metadata = jsonencode({
    version  = "1.0.0"
    category = "Sustainability"
  })

  policy_rule = <<POLICY_RULE
{
  "if": {
    "allOf": [
      {
        "field": "location",
        "notIn": ["westeurope", "uksouth", "northeurope", "eastus", "westus2", "canadacentral"]
      },
      {
        "field": "type",
        "equals": "Microsoft.Compute/virtualMachines"
      }
    ]
  },
  "then": {
    "effect": "deny"
  }
}
POLICY_RULE
}

resource "azurerm_policy_set_definition" "sustainable_initiative" {
  name         = "sustainable-initiative"
  policy_type  = "Custom"
  display_name = "Sustainable Initiative"

  metadata = jsonencode({
    version  = "1.0.0"
    category = "Sustainability"
  })

  policy_definition_reference {
    policy_definition_id = azurerm_policy_definition.energy_efficient_vm_sizes.id
    reference_id         = "energy-efficient-vm-sizes"
  }

  policy_definition_reference {
    policy_definition_id = azurerm_policy_definition.renewable_energy_regions.id
    reference_id         = "renewable-energy-regions"
  }
}
```

This initiative creates a policy to ensure that VMs are only deployed in regions with a higher proportion of renewable energy sources, such as North Europe. You can find more information about this approach by visiting the Electricity Maps website and examining carbon intensity data. The second Azure Policy restricts the creation of VMs to the B-series, known for their burstable performance and energy-efficient CPU scaling.

## In Conclusion
The Corporate Sustainability Reporting Directive (CSRD) is a new EU guideline that has been adopted by the European Union. In the context of sustainable software engineering, the CSRD mandates that companies report on their endeavors to diminish their environmental impact and promote sustainability. This includes initiatives related to the utilization of renewable energy, energy-efficient hardware and software, and practices aligned with the circular economy. Such reporting measures aim to enhance transparency and accountability among companies while encouraging the adoption of more sustainable practices. For publicly listed companies, compliance with these regulations is mandatory starting in 2024, with other companies expected to follow suit later. My colleagues and I are enthusiastic about assisting companies in embracing sustainable software engineering. Stay tuned for more knowledge sessions and blogs on this subject, and explore opportunities to address readily achievable sustainability goals tomorrow!
