## Introduction
One of the benefits of being part of the Microsoft MVP program is the access to private previews of services and features.

In August 2021, I was accepted to test and provide feedback on what was referred to as 'Azure Worker Apps', another Azure service Microsoft was developing to run containers. The first question that came to my mind upon learning about this service was:

"Why is Microsoft launching another service to run containers?"

That was indeed my feedback to the team: include in the documentation the reason why the service was created and when to use it instead of the existing options.

Fast forward, that service is now known as Azure Container Apps. Let's give a quick review of the use case for the other Azure Services before introducing Azure Container Apps.

## From simple to complex: comparing Azure's Containers Services

Azure Container Instances (ACI) is the most straightforward and quickest way to run a container in Azure, offering a solution for those who need to launch containers without the overhead of orchestration. It's ideal for simple applications, tasks, or jobs that require a single container to run on a short-term basis. However, ACI may not be the best fit for applications that require auto-scaling, persistent storage, or more complex orchestration, especially for web applications that could benefit from custom domain names, SSL certificates, and continuous deployment pipelines.

This is where Azure Web Apps for Containers comes into play. It builds upon the simplicity of running containers by adding the web application hosting capabilities of Azure Web Apps. This service is better suited for hosting web applications and APIs in containers, offering out-of-the-box features such as custom domains, SSL/TLS certifications, and scaling. It also integrates seamlessly with Azure DevOps and GitHub for continuous integration and delivery. While Azure Web Apps for Containers provides a more specialized environment for web hosting, it might not offer the granularity of control or scalability needed for more complex, microservices-based architectures or applications with high demands for customization and scalability.

Enter Azure Kubernetes Service (AKS), which addresses the complexities of running large-scale, microservices-based applications. AKS offers full Kubernetes orchestration, making it suitable for deploying, managing, and scaling complex applications across multiple containers. With AKS, you gain the benefits of Kubernetes without the burden of managing the Kubernetes infrastructure yourself. This service is ideal for scenarios where you need advanced orchestration, auto-scaling, and multi-container coordination. AKS fills the gaps when an application outgrows the capabilities of ACI and Azure Web Apps for Containers, providing a robust solution for enterprises and developers that require the full power of Kubernetes orchestration for their containerized applications.

## When should I use Azure Container Apps?

If you have some knowledge of Kubernetes, you might agree with me that there is a significant learning curve. One thing is to deploy a "hello world" application. Running real world applications following the best practices is a complete different thing. 

Now imagine teams that are just starting out without anyone experienced in administering a Kubernetes Cluster. For instance, setting up and managing Network Policies, Cluster Roles, Cluster Role Bindings, Persistent Volumes (PVs), and Persistent Volume Claims (PVCs) can be daunting tasks for those not familiar with Kubernetes operational details.

This is where Azure Container Apps enters, it is a fully managed serverless container service offered by Microsoft Azure that enables teams to deploy and run containerized applications without worrying about managing the underlying infrastructure (a.k.a Kubernetes Cluster).

## Azure Container Apps Components

Azure Container Apps is composed of several key components that work together to provide a seamless and flexible serverless container hosting environment. Let's dive into some of components that make up Azure Container Apps:

![Components of an Azure Container Apps](./images/azure-container-apps-containers.png)

### Environment

The first component of an Azure Container Apps is the Environment. It is a secure and isolated boundary where you define shared settings for networking, logging, and other services that your Container Apps can use. Think of it as the development or production environment for some application you want to run. 

### Container Apps

Next, the "Container App" component. You begin by choosing which container images to use, they are the Docker-compatible containers that hold your application code and its dependencies. You can use containers from public registries like Docker Hub or private registries such as Azure Container Registry. You also configure them with the necessary commands, ports, and environment variables, and the amount of CPU and memory to match the application's needs.

### Containers

These are the Docker-compatible containers that hold your application code and its dependencies. You can use containers from public registries like Docker Hub or private registries such as Azure Container Registry. 

### Replicas

A replica is an individual instance of a containerized application. You can run multiple instances simultaneously for better load handling and availability. When Dapr is integrated with Azure Container Apps, it envelops each replica with a separate container (a.k.a sidecar) running alongside your application container. This Dapr sidecar enhances your application by providing easy access to a suite of distributed system capabilities like state management, messaging, and service invocation without requiring changes to your application code.

This setup means as you scale your application by increasing the number of replicas in response to demand, each new replica is automatically paired with its own Dapr sidecar. This ensures all instances of your application can uniformly leverage the microservices components and patterns offered by Dapr, facilitating seamless scalability, resilience, and interoperability across your microservices architecture.

### Revisions

Revisions are immutable snapshots of your Container App configuration and code at a point in time. Each time you deploy or update a Container App, Azure Container Apps creates a new revision. This supports rollback scenarios and traffic splitting between different versions of your app for canary deployments or A/B testing.

### More Settings

Azure Container Apps supports both HTTP and HTTPS ingress. The ingress controls how external traffic reaches your Container Apps and is where you can configure custom domains, SSL/TLS certificates, authentication, and authorization.

Networking rules can be defined as well. They will help you manage how your application communicates internally and with the outside world, including securing connections and integrating with private networks for enhanced security.

You can also attach Azure Storage accounts as volumes in your container, allowing your application to access persistent storage. 

If the application uses secrets, you can inject them into your containers either from Azure Key Vault or defined directly in the Container App settings.

There are many other settings you can define for the Container App component. You can assign a managed identity for accessing other Azure services securely, configure the integration with Azure Monitor for logging, set up liveness and readiness probes to monitor the health of your containers, control deployment strategies and more.

## Creating your Azure Container Apps

The primary goal here is to illustrate the initial steps and make it accessible for beginners or anyone looking to explore the basics of container deployment with Azure Container Apps. I will cover more advanced scenarios and features in future articles.

In order to create a sample Azure Container App, let's first define a few variables.

```
RESOURCE_GROUP='rg-aca-demo'
LOCATION='eastus'
ENVIRONMENT_NAME='aca-env'
APP_NAME='aca-app'
CONTAINER_IMAGE='mcr.microsoft.com/k8se/quickstart:latest'
```

This following is the main command that instructs the Azure CLI to create a new Azure Container Apps Environment. 

```
az containerapp env create \
  --name $ENVIRONMENT_NAME \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION
```

Next, we will deploy our containerized application by creating a new Container App within the Container App Environment we just created. This command configures the app to be accessible from the outside world by enabling external ingress on port 80.

```
az containerapp create \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --environment $ENVIRONMENT_NAME \
  --image $CONTAINER_IMAGE \
  --target-port 80 \
  --ingress external \
  --query properties.configuration.ingress.fqdn
```

The --query option, included at the end of our command, is utilized to extract the Fully Qualified Domain Name (FQDN) of our application's ingress point. This results in providing us with a URL through which our application becomes accessible to the public.

![Azure Container App Ingress URL](./images/container-app-created.png)

To access our application, all that remains is to open this URL in a web browser. Doing so will bring us directly to the interface of our running Azure Container App:

![Running Azure Container Apps](./images/running-aca.png)

PS: Don't forget to delete everything after you finish with your tests!

## Conclusion

Azure Container Apps continues to evolve, offering more than just a streamlined deployment and management experience for containerized applications. 

It bridges the once-daunting gap between the complexities of Kubernetes orchestration and the rising demand for accessible, serverless container solutions while offering most of the functionality and power of the much more complex Kubernetes platform.
 
Since our initial discussion in Xpirit Magazine issue #12, Microsoft has shipped notable improvements including the integration of Managed Identities, which enhances security and simplifies accessing Azure resources. Furthermore, Microsoft has improved the developer experience by offering better tools for investigating running containers.

The introduction of GPU-accelerated containers, currently in preview, opens the door to exciting new possibilities, particularly for businesses looking to deploy advanced AI and machine learning models, such as Generative Pre-trained Transformers (GPTs), into their cloud infrastructure. These advancements not only represent a leap forward in technological capabilities but also align closely with Microsoft's mission statement "to empower every person and every organization on the planet to achieve more." By democratizing access to high-performance computing via GPU-accelerated containers, Microsoft Azure is enabling businesses of all sizes to have access to advanced computing resources that were previously out of reach. This empowerment allows organizations to innovate, solve complex problems, and deliver solutions and services that can make a meaningful impact on society and the economy.

With improvements in areas like advanced traffic shaping and more flexible hardware configuration options, Azure Container Apps can now support a broader range of application requirements, reflecting the feedback and evolving needs of its user base. This progression makes it an even more compelling choice for teams focused on developing and deploying impactful applications. By embracing Azure Container Apps, businesses are well-equipped to drive forward in the digital era, leveraging a platform that adapts and grows with the technological landscape.
