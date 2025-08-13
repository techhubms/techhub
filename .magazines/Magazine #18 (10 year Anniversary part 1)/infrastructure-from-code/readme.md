# Infrastructure from Code: simplifying cloud deployments
Over the years, I’ve worked with several infrastructure as code (IaC) tools, and one challenge stands out: the effort required to bridge the gap between application development and infrastructure management. Infrastructure from Code addresses this issue by abstracting infrastructure directly from application logic, making development workflows more seamless and agile. In this article, we will discuss various tools that lead up to Infrastructure from Code. We will dive into what Infrastructure from Code entails and the issues it tries to solve.

## What is Infrastructure from Code?
Infrastructure from Code allows you to generate and manage cloud infrastructure directly from application code. Unlike traditional IaC, which involves manually defining infrastructure with tools like Terraform or Bicep, Infrastructure from Code streamlines this process by inferring the required infrastructure from how the application is written. This shift enables developers to focus more on their code, with the confidence that their infrastructure will align seamlessly.

This is important because it provisions infrastructure faster, reducing delays during the development cycle. It ensures application and infrastructure changes are always in sync, which improves consistency across environments. By reducing the need for developers to understand detailed infrastructure concepts, it simplifies workflows and enables teams to focus more on application development. But is Infrastructure from Code the right approach for you? 

Before we dive into an Infrastructure from Code tool and see how that works, we first take a little tour through history to see how we got here and what changes in our industry are responsible for that. We start with looking at the traditional IaC tools and their challenges. Next, we will talk a little about how DevOps and Platform Engineering changed the way we look at these tools and their usage. We will see how tools that build abstractions on top of traditional IaC tools are becoming increasingly common. Finally, we will look at a tool that take this abstraction one step further by automatically provisioning infrastructure based on the application code.

![image](images/history.png)

## A look back: traditional IaC
Traditional IaC has been very useful for managing infrastructure in a reproducible and predictable way. Tools like Terraform and Bicep make it easier to automate complex cloud environments by using declarative configuration languages.

These tools have several advantages:
- **Consistency:** Infrastructure can be defined in a consistent manner, reducing the risk of human error.
- **Scalability:** Infrastructure can be easily scaled up or down based on demand.
- **Collaboration:** Teams can work together on infrastructure code, making it easier to share knowledge and best practices.
- **Documentation:** Infrastructure code serves as documentation, making it easier to understand the architecture.
- **Testing:** Infrastructure code can be tested and validated before deployment, reducing the risk of errors in production.
- **Automation:** Eliminate manual errors and streamline provisioning.
- **Version Control:** Infrastructure definitions can be stored and tracked in repositories.
- **Reusability:** Modular setups allow for better management across regions, environments and teams.

Despite its advantages, traditional IaC still has some pain points:
- **Learning curve:** Tools like Terraform or Bicep require an in-depth understanding of their syntax. For example, newcomers to Terraform often struggle using more complex constructs like for_each or functions like flatten. These can be difficult for those unfamiliar with advanced HCL features. Handling state files in Terraform, whether stored locally or in remote backends like Azure storage, also adds complexity, especially when dealing with collaboration across multiple environments. Additionally, setting up and testing complex Terraform modules for scalability and reuse requires thorough understanding. Another problem is that learning just Terraform or Bicep is not enough. An engineer also needs to know about various other tools and concepts, such as Azure CLI, Azure DevOps, GitHub Actions, and CI/CD pipelines. This can be overwhelming for newcomers.
- **Synchronization overhead:** Keeping application and infrastructure changes aligned can slow down development. If an engineer alters application logic, updating the corresponding infrastructure code is often a separate and time-consuming process. Often, a separate team is needed to manage the infrastructure code, which can lead to wait time and communication overhead.
- **Deep understanding of infrastructure:** Engineers need to understand cloud concepts and services to define infrastructure correctly. This can be a barrier for those who are more focused on application development. For example, setting up a simple website on an Azure App Service requires knowledge of App Service Plans, networking, identity, and other Azure services.
- **Multi cloud support:** While tools like Terraform and Pulumi support multiple cloud providers, managing multi-cloud setups can be complex. Engineers need to understand the nuances of each cloud provider, which can be challenging. Also, I still talk to people who think that when they use Terraform, they can easily switch between cloud providers. This is not the case. While the syntax might be the same, the resources are not and most work needs to be re-done.
- **Complexity in advanced scenarios:** Advanced scenarios, such as cross-region failovers or integrating third-party tools, often require custom scripting or manual intervention. This can lead to increased complexity and potential errors.
- **Tooling and ecosystem:** The ecosystem around IaC tools can be fragmented. For example, while Terraform has a rich set of providers, not all cloud services are covered. This can lead to situations where engineers need to write custom providers or scripts to manage specific resources.

## DevOps and Platform Engineering
The rise of DevOps and platform engineering has further complicated the landscape. As organizations adopt DevOps practices, the need for collaboration between development and operations teams has become more and more challenging, especially in large organisations. This shift has led to the emergence of new roles, such as platform engineers, who focus on building and maintaining the infrastructure that supports application development.
Platform engineers are responsible for creating self-service platforms that enable developers to deploy and manage their applications without needing deep infrastructure knowledge. This shift has led to the development of tools and frameworks that abstract away the complexities of traditional IaC, allowing developers to focus on writing code rather than managing infrastructure. 

## Alternatives to traditional IaC
One benefit of tools like Bicep or Terraform is that they do give you full power over the infrastructure that needs to be provisioned. However, this power comes at a cost: the need to understand the underlying infrastructure and the tools themselves. Before we dive into Infrastructure from Code, let’s see how three other tools leverage above tools to make our lives easier. Since each of these tools gives us a higher level of abstracting, we can nicely plot them on a ladder. The higher we go, the less developers need to know about the underlying infrastructure and the tools used to provision it.

![image](images/tools-ladder.png)

### Azure Developer CLI (azd)
First, there is the Azure Developer CLI (azd). Azure azd is a tool that accelerates provisioning and deploying app resources on Azure. Engineers start by creating a project directory and running commands like azd init to initialize the application template. This scaffolds both the code and infrastructure. This might include a C# web application and it's needed infrastructure like Azure App Services, Function Apps, or Cosmos DB, storage, all based on the selected template. The underlying infrastructure is defined using either Bicep or Terraform. The default is Bicep, but you can switch to Terraform, if you prefer, which is still in beta.

Using azd up, developers can deploy the infrastructure and application in a single step, which simplifies the lifecycle of resource creation, application deployment, and environment setup. For example, if an engineer is deploying a web application, azd up handles tasks like creating the App Service Plan, provisioning a resource group, and applying appropriate configurations—all driven by the application’s needs.

This approach solves problems such as the time-intensive coordination between development and DevOps teams, misaligned application requirements, and redundant scripting for repetitive deployments. Furthermore, Azure azd enables developers to maintain consistency across environments by defining everything as part of the project template. It also helps developers focus on application logic, abstracting the underlying infrastructure details which they might not have enough knowledge on.

However, Azure azd has limitations. It is primarily tailored to Azure-specific services, which means engineers cannot use it in multi-cloud strategies. Additionally, while it simplifies standard use cases, customizing infrastructure for advanced scenarios—like cross-region failovers or integrating third-party tools—often requires supplementing azd with traditional IaC tools like Terraform or ARM templates. I therefore see Azure azd more as a tool that simplifies the standard use cases, to quickly get started but not as a replacement for traditional IaC tools.

### Pulumi
Pulumi is another tool that (mainly) uses Terraform under the hood to perform its provisioning. Pulumi allows developers to define infrastructure using familiar programming languages like C#, Python, TypeScript, or Go. This enables developers to use the same language for both application and infrastructure code, reducing context switching and enabling a more seamless development experience.

Pulumi solves a few challenges engineers normally run into with traditional IaC, like debugging the written code. It also enables developers to reuse existing programming constructs, such as loops and conditionals, which are familiar and expressive. They can also leverage the same concepts to share code between teams and projects, which can be beneficial for scaling infrastructure setups or setting compliant standards.

Despite its advantages, Pulumi does have limitations. It requires developers to have programming skills beyond standard IaC tool syntax, which might be a barrier. I therefor see Pulumi only useful for application developers that already have a deep understanding of a programming language. For those that don't, it might be better to stick with traditional IaC tools. One benefit of Azure azd is that it abstracts the underlying infrastructure details, which makes it easier for developers to get started. Pulumi, on the other hand, requires a deeper understanding of cloud infrastructure concepts like as was needed when using Terraform or Bicep.

``` c#
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

return await Pulumi.Deployment.RunAsync(() =>
{
    while (!Debugger.IsAttached)
    {
        Thread.Sleep(100);
    }

    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("rg-pulumi-debug");

    // Create an Azure resource (Storage Account)
    var storageAccount = new StorageAccount("stgpulumidebug", new StorageAccountArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Sku = new SkuArgs
        {
            Name = SkuName.Standard_LRS
        },
        Kind = Kind.StorageV2
    });
});
```

### Radius
![image](images/radius.png)
Radius was originally created by Microsoft's incubation team but is now open source and a CNCF project. It is a cloud-native application platform designed to help not only developers but also the the infrastructure operators that support them. The goal is to make cloud-native application development more efficient and accessible, regardless of the cloud platform you land on. Radius provides this self-service platform that allows developers to focus on their core responsibilities. For some, that means coding microservices; for others, it means creating application infrastructure. 

Cloud-native applications become more and more complex. The synergy between developers and IT operators therefor becomes more and more crucial. Radius facilitates collaboration between these two key teams. Here's how it works:

- **Defining Your Application:** Developers define their applications in Radius, including all the services, dependencies, and relationships between them, where they normally would do that in infrastructure or Kubernetes resources. This approach streamlines the development process, making it easier to manage complex applications.
- **Crafting Environments:** In parallel, operators define environments within Radius, encapsulating infrastructure templates, policies, and organizational requirements specific to their chosen platform, whether it's Azure, AWS, or self-hosted. This ensures that your applications can run smoothly on any cloud platform.

The code snippet below shows how to define a simple application in Radius. The code is written in Bicep, which is the underlying language used by Radius. The code defines a simple web application that uses a Redis store. The application is defined as a container, and the Redis store is defined as a datastore. Notice how we didn’t specify detailed information about the underlying infrastructure. This is all handled by Radius.

``` bicep
extension radius

@description('The Radius Application ID. Injected automatically by the rad CLI.')
param application string

@description('The environment ID of your Radius Application. Set automatically by the rad CLI.')
param environment string

resource demo 'Applications.Core/containers@2023-10-01-preview' = {
  name: 'demo'
  properties: {
    application: application
    container: {
      image: 'ghcr.io/radius-project/samples/demo:latest'
      ports: {
        web: {
          containerPort: 3000
        }
      }
    }
    connections: {
      redis: {
        source: db.id
      }
    }
  }
}

resource db 'Applications.Datastores/redisCaches@2023-10-01-preview' = {
  name: 'db'
  properties: {
    application: application
    environment: environment
  }
}
```

Running the command 'rad up' will deploy the application to your local cluster—for example, using K3D. The translation between the types defined in above example and what infrastructure is actually run is done by what we call 'Recipes' in Radius. So, when you run 'rad up', Radius will look at the application and environment definitions, and then use the recipes to deploy the application to the local cluster. When deploying to e.g. Azure, Radius will use the Azure recipes to deploy the application to Azure. The image below shows how Radius uses the recipes to deploy the application to Azure.

![image](images/radius-app.png)

## Infrastructure from Code
Using any of the tools above, you still need to define the infrastructure. Its either done directly through e.g. Terraform or by using a tool that abstracts the infrastructure like Azure azd. Infrastructure from Code takes this abstraction one step further by automatically provisioning infrastructure based on the application code. This approach is particularly powerful for serverless and event-driven applications, where infrastructure components, like APIs, message queues, or databases, are tightly coupled to code.

There are various tools that implement this concept in one way or another. Let's compare some of these tools at a high level before we dig into a few to see them in action. When picking a tool, there are multiple factors we can look at. For example, what cloud platforms can they target, what programming languages do they support and what IaC language do they use under the hood. Here's a table of the tools and the supported cloud platforms.

| Tool | Azure | AWS | GCP |
|------|-------|-----|-----|
| Nitric | X | X |  X |            
| Klotho |  | X |  |         
| Ampt |  | X |  |    |
| Encore |  | X | X |

Supported programming languages:
| Tool | .Net | Java | Python | Go | TypeScript | Node.js |
|------|-------|-----|-----|------------|----|----|
| Nitric | X* | X | X  | X | X | |
| Klotho |  | | X | X          |
| Ampt |  |  |  |           | x | X
| Encore |  |  |  | X          | X|X|

Underlying IaC language:
| Tool  | Terraform | Pulumi | Other |
|------|-----|-----|------------|
| Nitric | X | X | X | 
| Klotho |  |  | X | 
| Ampt |  |  |   X 
| Encore |  |  | X | 

After reviewing this list, I decided to dig into Nitric. This tool allows me to target Azure and write my code in a language that I know (sort of ;-)), TypeScript. There is (very) experimental support for C#, so I decided not to take that route.

### **Nitric**
Nitric offers a framework for building backends. It's a declarative framework with common components like APIs, queues, and databases. Nitric abstracts the underlying infrastructure, allowing developers to focus on building applications but also allowing developers to interact with these resources. It becomes easy to swap underlying services as they are pluggable.

A simple API endpoint in Nitric looks like this:
```typescript
import { api } from '@nitric/sdk'

const main = api('main')

main.get('/hello/:name', async ({ req, res }) => {
  const { name } = req.params
  ctx.res.body = `Hello ${name}`
})
```

The above code is called a service in Nitric. It's the entry point for the application. You can define one or multiple services. By importing the SDK and creating an instance of the api type, you're instructing Nitric to expose an API. When we later run Nitric and deploy to Azure, this will create an Azure Container App to run the application on and will create an API Management instance to expose and test the API.

In a config file, like the one below, you tell Nictric where to look for the services and how to run them.
```yaml
name: example
services:
  - match: services/*.ts
    start: npm run dev:services $SERVICE_PATH
```

Then we have the concept of resources. These are the underlying infrastructure components that the services uses. In the example below, I'm using a storage resource.

```typescript
import { bucket } from '@nitric/sdk'

const uploads = bucket('uploads').allow('read')

const files = await uploads.files()

files.forEach((file) => {
  console.log(file.name)
})
```

The above bucket will be deployed in Azure as a Storage Account. The bucket is then used to store files in blobs and the code above lists all the files in the bucket. See how easy it is to define your requirements without writing a single line of infrastructure code?

But how does Nitric know how to deploy the resources? This is done in a nitric.<environment>.yaml file. In this file, you define the provider to be used. In the example below, I'm using the Pulumi provider to target Azure. 

```yaml
provider: nitric/azure@1.1.1
region: northeurope
```

With that in place, it's time to run our app. You can do that locally by running 'nitric start' from a terminal. All services will then be emulated locally using containers. It also provides a Dashboard UI to interact with the resources.

![image](images/nictric-dashboard.png)

When you now run 'nitric up', your application will be deployed to the cloud. In my case, I see various resources like an API Management instance, Azure Storage Account, and a Function App being created. The API Management instance is used to expose the API endpoints. The Azure Storage Account is used to store files in blobs. The Function App is used to run the application code. All without us defining a single line of infrastructure.

### When and why to use Infrastructure from Code
Now we've seen an example of how we can levarage the power of IfC using Nitric, let's talk about where I see it fit. Infrastructure from Code is best suited for situations where developer velocity and simplicity matter more than fine-grained infrastructure control. It shines particularly in the following contexts:

#### Startups and MVPs
Startups often need to ship quickly without dedicating time to infrastructure design. IfC tools allow them to:
- Focus purely on writing product code.
- Automatically deploy infrastructure without needing much support from a platform team.
- Iterate fast and pivot without worrying about cloud plumbing.

#### Event-driven applications
Many IfC tools are designed with cloud-native, event-driven patterns in mind (e.g., HTTP routes, queues, functions). They're ideal for:
- APIs triggered by HTTP or events.
- Background workers, cron jobs.
- Serverless use cases that glue services together.

#### Internal tools and prototypes
For internal apps that solve operational problems or automate workflows:
- Simplicity and speed often outweigh the need for enterprise-grade infra design.
- IfC lets devs roll out tools without engaging infra/platform teams.

#### Developer-driven teams
In teams where developers own the full lifecycle (build → deploy → monitor), IfC reduces the need to learn Terraform, Bicep, or YAML.
- It supports the “you build it, you run it” model.
- Lower barrier to cloud deployment = more empowered devs.

## Considerations and future outlook
A tool like Nitric looks really promising and makes creating a new application really easy. The tool, and other IfC tools as well, don't really feel mature yet. I got quite a few errors along the way. Most of them were quickly fixed in a new version but still. The support for various cloud resources is also quite small. While I really do see this new approach working in the future, it also introduces some additional considerations:

- **Abstraction overhead:** Too much abstraction might reduce control.
- **Cloud or vendor lock-in:** Tools can sometimes be tightly bound to specific providers or vendor SDKs, in the case of Nitric.
- **Complex scenarios:** Advanced customizations might still rely on traditional IaC techniques.

Infrastructure from Code offers a transformative approach to cloud development. By focusing on application-driven workflows and abstracting the details of infrastructure setup, developers can move faster and reduce operational complexity. Tools like Nitric can really speed up your development processes but are not for everyone. Any of the other mentioned tools can already be a significat step up compared to using traditinal IaC.

Below you will find a table that lists the tools mentioned in this post and summarizes their properties like abstraction level, language used, and cloud support. This should help you to pick the right tool for your needs.


| Tool      | Infra Defined In       | Abstraction Level | Language Used     | Cloud Support  | Best For                             |
|-----------|------------------------|-------------------|-------------------|----------------|---------------------------------------|
| Terraform | Declarative (HCL)      | Low               | HCL               | Multi-cloud     | Infra teams comfortable with code     |
| Bicep     | Declarative (Bicep)    | Low               | Bicep             | Azure           | Azure-focused infra teams             |
| Pulumi    | Imperative (Code)      | Medium            | TypeScript, Python, Go, C# | Multi-cloud | Developers preferring real languages  |
| azd       | Templates + IaC        | Medium            | CLI (Bicep/Terraform under the hood) | Azure | Quick app bootstrapping on Azure     |
| Radius    | Recipes + IaC          | High              | Config + Bicep    | Azure, AWS, Google, Kubernetes | Platform teams enabling developers    |
| Nitric    | In Application Code    | Highest           | TypeScript, Python, Go | Multi-cloud | Serverless and event-driven apps      |
