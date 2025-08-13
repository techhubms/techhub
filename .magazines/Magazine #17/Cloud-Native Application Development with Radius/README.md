# The future of Cloud-native software development with Radius
(By Loek Duys)

## The rise of platform engineering
Over the years, the process of software development has changed a lot. The way applications are built, deployed, and managed today is completely different from ten years ago. Initially, our industry relied on monolithic architectures, where the entire application was a single, simple, cohesive unit. This approach made the development process straightforward initially, but as applications grew in complexity, maintaining and scaling them became increasingly challenging. Every change required a complete redeployment, leading to long development cycles and heightened risks of introducing errors. On top of that, a single bug in the software could take down an entire system.

### Ever increasing complexity

To overcome these limitations, we transitioned to Service-Oriented Architecture (SOA). SOA decomposed applications into smaller, independent services that communicated over a network. This modular approach improved maintainability and scalability of applications, as each service could be developed, deployed, and scaled independently. However, SOA introduced its own set of complexities, such as the need for robust inter-service communication and service management. On top of that, services depending on multiple layers of other downstream services resulted in cascading errors, so a single issue could still result in large scale unavailability. 

### DevOps
The introduction of DevOps marked a cultural and operational shift in software development. DevOps emphasized the collaboration between development and operations teams, breaking down silos and fostering a culture of continuous integration and continuous delivery (CI/CD) and an Agile way of working. This approach enabled faster, more reliable and efficient software delivery by automating infrastructure management and the deployment processes. The focus on collaboration and automation greatly improved the efficiency of software development. Adopting a DevOps culture did increase the overall cognitive load for team members, as they need to learn about CI/CD and automation.

### Microservices
Building on the principles of SOA, Microservices architecture further decomposed applications into self-contained autonomous business capabilities. Each Microservice focused on a specific business function and could be independently developed, deployed, and scaled. This granularity improved agility and scalability but also introduced challenges in managing service dependencies, communication, and data consistency, further increasing the cognitive load for team members.

### Containers and orchestration
The use of containers greatly simplified application deployment, by packaging an application together with its direct dependencies like libraries, frameworks and content files. Running containers reliably at scale lead to the introduction of container orchestrators like Kubernetes. Kubernetes is able to run containerized workloads on a cluster of virtual machines, and provides many additional features. DevOps teams did need to learn how to containerize their apps and how to deploy them to an orchestrator, again increasing complexity.

### Cloud
Around the same time, the Cloud became more and more popular as an environment to run software. We started building Cloud-native software. Using IaaS and PaaS services instead of custom built self-hosted tools greatly accelerated teams. The downside was that they first needed to understand which Cloud service to use when. Not an easy task considering that Azure has more than 200 services and products at the time of writing.

### Light at the end of the tunnel

Today, we have arrived at platform engineering, a field that takes the best practices and tools from previous methodologies to optimize the development, deployment, and management of applications. This is a significant step in lowering cognitive load on product teams. Platform engineering provides a standardized environment that integrates tools and processes to enhance collaboration and efficiency. It abstracts many of the complexities of underlying infrastructure, enabling developers to focus on delivering features and value rather than managing operational details. (If you want to read more about Platform Engineering, have a look at the article 'Was shift left the right move?' by Sander and Chris, also in this magazine!)

This is where Radius comes into the picture. Radius is designed to simplify the development and deployment of Cloud-native applications. The core feature of Radius, is the application graph, which represents the relationships and dependencies within applications. It can be visualized in the Radius Portal which is installed with the product, or with the Radius CLI. This enables collaboration between people with a development role and people with an operations role.

# What is Radius and how does it help developers?

Radius was originally developed by Microsoft's incubation team. Nowadays, it is an open-source project and part of the Cloud-Native Computing Foundation (CNCF) which is an organization that supports open-source Cloud-native projects. Radius is designed to address the challenges of modern Cloud-native software development. As Cloud-native architecture becomes the standard for many IT companies, managing the complexity of applications across multiple environments can be complicated. Radius provides a platform that simplifies the entire lifecycle of Cloud-native applications. It bridges the gap between developers and operators, enabling collaboration.

## Applications
One of the key features of Radius is the Application graph. Graphs visually represent the relationships and dependencies between different components of an application, like compute, data storage, messaging and networking. This visual approach simplifies the understanding and management of complex applications. By mapping out how various services interact, developers can quickly understand how they work. The application graph also makes it easy to see how to operate the application, which dependencies should be implemented as PaaS services, and which as containers.

In Radius applications are defined using the Bicep language. Up until now, Bicep was a domain-specific language for Azure resource deployments. It has now been extended to include Radius resources, Kubernetes resources and Dapr resources. (It even supports EntraID, but that is outside of the scope of this article.) In the fragment below, you can see a simple Radius application. It defines an Environment which specifies a lifecycle stage for the application. It also contains an Application definition, and as part of the application and environment it runs one containerized web server. (a sample application created by the Radius team.)

```bicep
//import Radius resource types
extension radius

//define radius environment
resource env 'Applications.Core/environments@2023-10-01-preview' = {
  name: 'test'
  properties: {
    compute: {
      kind: 'kubernetes'
      namespace: 'test'
    }
  }
}

//define radius application
resource app 'Applications.Core/applications@2023-10-01-preview' = {
  name: 'demo01'
  properties: {
    environment: env.id
  }
}

//define container that runs the application
resource container01 'Applications.Core/containers@2023-10-01-preview' = {
  name: 'container01'
  properties: {
    application: app.id
    environment: env.id
    container: {
      image: 'ghcr.io/radius-project/samples/demo:latest'
      imagePullPolicy: 'IfNotPresent'
      ports: {
        web: {
          containerPort: 3000
        }
      }
    }
  }
}
```
## Environments

In Radius, environments are used to manage the lifecycle of cloud-native applications. Environments in Radius represent distinct stages such as development, testing, staging, and production, each tailored to a specific phase of the application lifecycle.

## Resource groups

Radius Resource Groups are logical containers that help manage and organize the resources required for deploying and running applications. These groups contain various resources such as Environments, containers, databases, and networking components, all necessary to run a specific workload. Radius utilizes Resource Groups to streamline the deployment process, making it easier to manage resources collectively rather than individually. In the future, Radius Resource Groups will also act as a security boundary by applying Role Based Access control policies at this level.

The image below visually represents the relationship between Application components, Environment and Resource group. (The first demo app does not have a database yet, but we will add it later.)
<p align="center">
  <img src="images/AppEnvRg.png" alt="Diagram" style="max-width: 400px;">
</p>


## Running the demo 
If you want to run this and other examples yourself, the quickest way to get up and running is by creating a GitHub Codespace here: https://github.com/codespaces/new/radius-project/samples. This article uses v0.37.
Run this command to fix an issue in the Codespace:
```cmd
@loekd ➜ /workspaces/samples (v0.37) $ sh ./.devcontainer/on-create.sh 
```
This should install the proper Radius CLI version.

The bicep file of the app above, can be found here: https://github.com/loekd/radius-demos/blob/main/01-Bicep/app_v1.bicep
Copy the files to your Codespace before deploying them. 
For example, by using `curl`:
```cmd
@loekd ➜ /workspaces/samples (v0.37) $ curl -O https://raw.githubusercontent.com/loekd/radius-demos/main/01-Bicep/app_v1.bicep
```


Also, make sure to enable the extensions preview for Bicep in 'bicepconfig.json'. Example:
```json
{
    "experimentalFeaturesEnabled": {
      "extensibility": true,
      "extensionRegistry": true,
      "dynamicTypeLoading": true
    },
    "extensions": {
      "radius": "br:biceptypes.azurecr.io/radius:0.37"
    }
}
```
> Check the image version for the Radius extension.

Prepare Radius for the application. Choose `k3d-k3s-default` as the target Kubernetes cluster (K3d running locally).
Create an environment named 'test' and a namespace named 'test'. Don't configure any cloud features, and don't set up an application (as this is already done using the Bicep code above).

```cmd
@loekd ➜ /workspaces/samples (v0.37) $ rad init --full
                                                     
Initializing Radius. This may take a minute or two...
                                                     
✅ Install Radius v0.37.0                            
   - Kubernetes cluster: k3d-k3s-default             
   - Kubernetes namespace: radius-system             
✅ Create new environment test                       
   - Kubernetes namespace: test                      
✅ Update local configuration                        
                                                     
Initialization complete! Have a RAD time 😎
```
You can then run your Radius application using the `rad run` command:
```cmd
@loekd ➜ /workspaces/samples (v0.37) $ rad run ./app_v1.bicep --application demo01 --group test
Building ./app_v1.bicep...

Deploying template './app_v1.bicep' for application 'demo01' and environment 'test' from workspace 'default'...

Deployment In Progress... 

Completed            demo01          Applications.Core/applications
Completed            test            Applications.Core/environments
...                  container01     Applications.Core/containers

Deployment Complete

Resources:
    demo01          Applications.Core/applications
    container01     Applications.Core/containers
    test            Applications.Core/environments

Starting log stream...

+ container01-b5b9bf6bc-657p4 › container01
container01-b5b9bf6bc-657p4 container01 [port-forward] connected from localhost:3000 -> ::3000
container01-b5b9bf6bc-657p4 container01 No APPLICATIONINSIGHTS_CONNECTION_STRING found, skipping Azure Monitor setup
container01-b5b9bf6bc-657p4 container01 Using in-memory store: no connection string found
container01-b5b9bf6bc-657p4 container01 Server is running at http://localhost:3000
dashboard-5d64c96ff-9jwf7 dashboard [port-forward] connected from localhost:7007 -> ::7007
```
The CLI is collecting output generated by the container, and displaying them in your terminal. Also, the CLI has created the necessary port forwards, so you can access the application running on the K3d cluster using `localhost`. Press `Control` and click on the URL `http://localhost:3000` in the output to see the web page running. It should show the text 'Welcome to the Radius demo'.

> In your Codespace, hit `Control + C` to terminate the log and port forwarding 

Next, remove the application by running the following script:

```cmd
rad app delete demo01 --group test -y
```

Repeat this process for the next demos.

## Recipes
Radius also uses Recipes. Recipes enable a separation of concerns between IT operators and developers by automating infrastructure deployments for application dependencies. Developers can select **which types** of resources they need in their applications, such as Mongo Databases, Redis Caches, or Dapr State Stores, while IT operators define **how** these resources should be deployed and configured within their environment, whether it be as containers, Azure resources, or AWS resources. 
Before developers can use a Recipe, they must be versioned and published to an OCI-compliant registry like Azure Container Registry.

When a developer deploys an application and its resources, Recipes automatically deploy the necessary backing infrastructure and bind it to the developer’s resources. Recipes supports multiple Infrastructure as Code (IaC) languages, including Bicep and Terraform, and can be referenced in an Environment. By using Recipes, a single application definition can be deployed to a non-production Environment with a containerized state store, and to production with a Cloud-based store like Cosmos DB without changes, but simply by referencing different Recipes.

### Connections
Recipes must follow some rules so Radius can connect dependencies created through Recipes to Application containers. For example, if you deploy a database using a Recipe, the container that uses that database will need to know how to connect to it. Radius solves this by requiring a Recipe to return details about the deployed resource, like connection strings, and/or credentials. These details are known as Connections.

In the script below, you can see a Dapr Application that uses a Recipe to define its Dapr State Store. The Recipe is referenced from the Environment definition. Also, it uses the concept of Bicep modules to reference resources from other files that will define the frontend and backend elements for the Application. 

>If you want to deploy this example, use the file `app.bicep`.

The image below visually represents the relationship between Frontend, Backend, Dapr State store, and Redis Database. In this situation, the connection is configured for the Backend, so it knows which Dapr State store component to use.
<p align="center">
  <img src="images/AppDapr.png" alt="Diagram" style="max-width: 200px;">
</p>

**app.bicep:**
```bicep
extension radius

//define explicit radius environment
resource env 'Applications.Core/environments@2023-10-01-preview' = {
  name: 'test'
  properties: {
    compute: {
    }
    //register recipe using Bicep
    recipes: {      
      'Applications.Dapr/stateStores': {
        default: {
          templateKind: 'bicep'
          templatePath: 'acrradius.azurecr.io/recipes/statestore:0.1.0'
        }
      }    
    }
  }
}

resource app 'Applications.Core/applications@2023-10-01-preview' = {
  name: 'demo02'
  properties: {
    environment: env.id
  }
}

module frontend 'frontend.bicep'= {
  name: 'frontend'
  params: {
    environment: env.id
    application: app.id
  }
  dependsOn: [
    backend
  ]
}

module backend 'backend.bicep'= {
  name: 'backend'
  params: {
    environment: env.id
    application: app.id
  }
}

```
In the fragment below, you can see the contents of the `backend.bicep` file. Notice that the Application has a Connection that references the State Store. The application does not need to know the concrete implementation of the Store, just how to connect.
The State Store `stateStore02` is deployed by declaring the resource, and referencing the Recipe name `default`. Radius will use the Environment definition to find the template and ensure it is created.

**backend.bicep:**
```bicep
import radius as radius

@description('Specifies the environment for resources.')
param environment string

@description('The ID of your Radius Application.')
param application string

// The backend container that is connected to the Dapr state store
resource backend02 'Applications.Core/containers@2023-10-01-preview' = {
  name: 'backend02'
  properties: {
    application: application
    container: {
      image: 'ghcr.io/radius-project/samples/dapr-backend:latest'
      ports: {
        orders: {
          containerPort: 3000
        }
      }
    }
    //connection provides component name, not connection string
    connections: {
      orders: {
        source: stateStore02.id
      }
    }
    extensions: [
      {
        kind: 'daprSidecar'
        appId: 'backend'
        appPort: 3000
      }
    ]
  }
}

// The Dapr state store that is connected to the backend container
resource stateStore02 'Applications.Dapr/stateStores@2023-10-01-preview' = {
  name: 'statestore02'
  properties: {
    // Provision Redis Dapr state store automatically via Radius Recipe
    environment: environment
    application: application
    resourceProvisioning: 'recipe'
    recipe: {     
      name: 'default'
    }
  }
}

```

The example has two Recipes, one that defines a containerized State Store, and one that defines an Azure CosmosDb State Store.
Have a look at the simplified version of a Recipe for dev/test below. It uses Kubernetes resources to deploy Redis and a Dapr component to describe it. It returns the Dapr Component so Radius can use it to create Connections.

### Recipe 1
(Full file: https://github.com/loekd/radius-demos/blob/main/02-Dapr/recipes/stateStoreRecipe.bicep)

```bicep
extension kubernetes with {
  //Kubernetes extension
} as kubernetes

param context object

resource redis 'apps/Deployment@v1' = {
  //Kubernetes deployment that deploys Redis containers
}

resource svc 'core/Service@v1' = {
  //Kubernetes service that connects to Redis Pods
}

resource daprComponent 'dapr.io/Component@v1alpha1' = {
  //Dapr component that describes the State Store
}

//Mandatory output for Radius to connect State Store to App
output result object = {
  resources: [ .. ]
  values: {
    type: daprType
    version: daprVersion
    metadata: daprComponent.spec.metadata
  }
}
```

In the script below, you can see a simplified version of a Recipe that creates an Azure Cosmos Db State Store. 
By referencing this template from the Environment, the application will be deployed with a production-ready State Store.

### Recipe 2
(Full file: https://github.com/loekd/radius-demos/blob/main/02-Dapr/recipes/cosmos_statestore_recipe.bicep)

```bicep
param context object
param location string = 'northeurope'
param accountName string = context.resource.name
param databaseName string = context.resource.name
param appId string

// Cosmos DB Account
resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2021-06-15' = {
  //Azure resource spec
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
  //Azure resource spec
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  //Azure resource spec
}

resource daprComponent 'dapr.io/Component@v1alpha1' = {
  //Dapr component that describes the State Store
}

output result object = {
  resources: [..]
  values: {
    type: daprType
    version: daprVersion
    metadata: daprComponent.spec.metadata
    server: cosmosDbAccount.properties.documentEndpoint
    database: databaseName    
    collection: containerName
    port: 443
  }
}
```

## Bringing it together
You now know that you can have different Environments, each defining the same resource types, with different concrete implementations. On non-production environments you can use containerized datastores, like Redis from Recipe 1. On production environments, you should use PaaS services like Azure Cosmos Db from Recipe 2 above.

This is what the Environment definitions would look like:

### Non-production Environment
```bicep
resource env 'Applications.Core/environments@2023-10-01-preview' = {
  name: 'test'
  properties: {
    recipes: {      
      //containerized Dapr State store recipe
      'Applications.Dapr/stateStores': {
        default: {
          templateKind: 'bicep'
          templatePath: 'acrradius.azurecr.io/recipes/localstatestore:0.1.2'
        }        
      }
```

### Production Environment
```bicep
resource env 'Applications.Core/environments@2023-10-01-preview' = {
  name: 'prod'
  properties: {
    recipes: {      
      //CosmosDb Dapr State store recipe
      'Applications.Dapr/stateStores': {
        default: {
          templateKind: 'bicep'
          templatePath: 'acrradius.azurecr.io/recipes/cosmosstatestore:0.1.0'
        }        
      }
```
By deploying the **same** Application definition in two **different** environments, it will use different state stores. Notice how the resource type of `stateStore02` matches the Recipe's resource type, and that the `stateStore02.properties.recipe.name` ('default') also matches the Recipe's name in the Environment. You can use multiple Recipes for the same resource type, by using different names.

```bicep
resource stateStore02 'Applications.Dapr/stateStores@2023-10-01-preview' = {
  name: 'statestore02'
  properties: {
    // Provision Redis Dapr state store automatically via Radius Recipe
    environment: environment
    application: application
    resourceProvisioning: 'recipe'
    recipe: {     
      name: 'default'
    }
  }
}
```

# Networking
The last thing we need to arrange is networking. Up until now, we have been using tunneling to access the application through `localhost`.  For this, we can use a Gateway.

## Gateway
Usually, a Cloud-native app will have a web based interface. Often, it will have an API. Using a Radius Gateway, you can expose both at the same domain by creating traffic routing rules. It has basic support for path rewriting. You can also use a Gateway for TLS offloading. Below, you can see an example of a Gateway that routes traffic for a host named 'example.com'.

It examines the incoming web request path. If the path contains `/api`, it will send that request to the container named `green`. If the request path contains `/blue`, the request will be sent to a container named `blue`. Also, the matched word `blue` will be stripped from the path sent downstream because of the `replacePrefix` part. This demonstrates how simple request rewriting works in Radius.
All other requests will be sent to the `nginx` container, which runs a web frontend.

<p align="center">
  <img src="images/AppGw.png" alt="Diagram" style="max-width: 400px;">
</p>

```bicep
resource gateway 'Applications.Core/gateways@2023-10-01-preview' = {
  name: 'gateway01'
  properties: {
    application: app.id
    environment: env.id
    internal: true
    hostname: {
      fullyQualifiedHostname: 'test.loekd.com'
    }
    routes: [
      {
        path: '/api' 
        destination: 'http://green:8080'
      }      
      {
        path: '/blue'
        destination: 'http://blue:8082'
        replacePrefix: '/'
      }
      {
        path: '/'
        destination: 'http://nginx:80'
      }
    ]
  }
}
```
> If you want to deploy a working version of a Gateway, check out the file here: https://github.com/loekd/radius-demos/blob/main/03-Gateway/app_gw.bicep
>- call blue API: `curl -HHost:test.loekd.com http://localhost/blue/api/color`
>- call green API: `curl -HHost:test.loekd.com http://localhost/api/color`
>- call the main site: `curl -HHost:test.loekd.com http://localhost`

# Conclusion

Radius transforms the development and deployment process by abstracting the complexities of modern Cloud environments. By using Applications, Connections, and Recipes, it allows developers to focus on building features rather than managing infrastructure. By defining Recipes, people with an operations role can ensure that the infrastructure is compliant. The Application graph facilitates people in both roles to understand and discuss Applications and their dependencies.

At the time of writing, Radius is still in its early stages. However, especially within larger organizations, and once generally available, Radius could be a valuable tool for software development teams. Make sure to get some hands on experience with it today, provide feedback to the team, or contribute code yourself. 
>This could also mean that the samples in this document are outdated and no longer work. Breaking changes are not uncommon while this platform is being built. The concepts and ideas however, will likely remain the same.

Useful links:
- Radius documentation: https://docs.radapp.io/
- Radius GitHub: https://github.com/radius-project
- Radius sample applications from this article: https://github.com/loekd/radius-demos
- Cloud native computing foundation: https://www.cncf.io/ 


