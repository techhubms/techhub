---
layout: "post"
title: "Static Egress Gateway in AKS: The Native Way to Control Multiple Outbound IPs"
description: "This post by pjlewis revisits the challenge of providing multiple outbound IP addresses in Azure Kubernetes Service (AKS), now using the native Static Egress Gateway feature. It provides hands-on guidance to implementation, contrasts with older approaches, and includes considerations for BYO networking and private egress scenarios."
author: "pjlewis"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-architecture-blog/static-egress-gateway-in-aks-the-native-way-to-control-multiple/ba-p/4484179"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-13 00:22:15 +00:00
permalink: "/2026-01-13-Static-Egress-Gateway-in-AKS-The-Native-Way-to-Control-Multiple-Outbound-IPs.html"
categories: ["Azure", "Coding", "DevOps", "Security"]
tags: ["AKS", "Az CLI", "Azure", "BYO Networking", "Coding", "Community", "DevOps", "Egress IP", "Gateway Node Pool", "Kubectl", "Kubernetes", "Kubernetes CRD", "LoadBalancer", "Network Architecture", "Network Security", "Private Egress", "Public IP Prefix", "Security", "Static Egress Gateway", "User Defined Routing", "YADA Demo App"]
tags_normalized: ["aks", "az cli", "azure", "byo networking", "coding", "community", "devops", "egress ip", "gateway node pool", "kubectl", "kubernetes", "kubernetes crd", "loadbalancer", "network architecture", "network security", "private egress", "public ip prefix", "security", "static egress gateway", "user defined routing", "yada demo app"]
---

pjlewis demonstrates how to manage multiple outbound egress IPs natively in Azure Kubernetes Service using the Static Egress Gateway feature, providing clear, actionable implementation steps and security-focused considerations.<!--excerpt_end-->

# Static Egress Gateway in AKS: The Native Way to Control Multiple Outbound IPs

## Introduction

As of mid-2024, Azure Kubernetes Service (AKS) natively supports Static Egress Gateway, simplifying the task of routing different application workloads along distinct outbound paths. In this guide, pjlewis compares earlier solutions—requiring multiple node pools and custom routing logic—to the more maintainable Static Egress Gateway feature, and provides step-by-step commands for implementation.

## The Problem Statement

AKS clusters commonly host applications that require different outbound egress IPs, making it necessary to route outbound traffic from specific workloads over distinct, predictable IPs for compliance, whitelisting, or security requirements.

## Static Egress Gateway vs. Custom Node Pools (2023 Approach)

The older approach to providing multiple egress IP addresses within AKS involved provisioning multiple node pools in separate subnets and manually managing outbound routing through user defined routes (UDRs). This model was complex, hard to automate, and scaled poorly.

The new Static Egress Gateway:

- Removes the need for multiple node pools and subnets strictly for egress.
- Handles the public IP prefix assignment and translation internally in AKS.
- Lets you allocate predictable public IPs to workloads with simple configuration.
- Minimizes operational overhead and complexity.

## Implementing Static Egress Gateway

### Prerequisites and Setup

1. **Set environment variables and create a resource group:**

   ```bash
   rg=egress-gw
   location=swedencentral
   vnet_name=vnet-egress-gw
   cluster=egress-gw
   vm_size=Standard_D4as_v6
   az group create -n $rg -l $location
   ```

2. **Create AKS cluster with Static Egress Gateway enabled:**

   ```bash
   az aks create \
     --name $cluster \
     --resource-group $rg \
     --location $location \
     --enable-static-egress-gateway
   az aks get-credentials -n $cluster -g $rg --overwrite
   ```

   - To enable on an existing cluster: `az aks update --name $cluster --resource-group $rg --enable-static-egress-gateway`

3. **Add gateway node pool:**

   ```bash
   az aks nodepool add \
     --cluster-name $cluster \
     --name gwpool1 \
     --resource-group $rg \
     --mode gateway \
     --node-count 2 \
     --gateway-prefix-size 30
   ```

   > **Note:** Use gateway node pools solely for egress, not general workloads.

### Apply StaticGatewayConfiguration CRD

Use a `StaticGatewayConfiguration` custom resource to assign the gateway node pool and IP ranges. Example manifest:

```yaml
apiVersion: egressgateway.kubernetes.azure.com/v1alpha1
kind: StaticGatewayConfiguration
metadata:
  name: egress-gw-1
  namespace: default
spec:
  gatewayNodepoolName: gwpool1
  excludeCidrs:
    - 10.0.0.0/8
    - 172.16.0.0/12
    - 169.254.169.254/32

# publicIpPrefixId: /subscriptions/<subscription-id>/resourceGroups/<resource-group>/providers/Microsoft.Network/publicIPPrefixes/<prefix-name>
```

Omit `publicIpPrefixId` to have AKS create the public IP prefix automatically, or set it to reuse an existing prefix.

Check your configuration:

```bash
kubectl describe StaticGatewayConfiguration egress-gw-1 -n default
```

Look for the provisioned egress IP prefix in the resource's status section.

### Deploying Workloads with Specific Egress

- Deploy your app's API component; for example, YADA demo application's API with a public-facing LoadBalancer service.
- Attach the correct `static-gateway-configuration` annotation to the deployment if you want it egressed via a specific gateway.

Example for default cluster egress:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-default
  labels:
    run: api-default
spec:
  replicas: 1
  selector:
    matchLabels:
      run: api-default
  template:
    metadata:
      labels:
        run: api-default
    spec:
      containers:
        - image: erjosito/yadaapi:1.0
          name: api-default
          ports:
            - containerPort: 8080
---
apiVersion: v1
kind: Service
metadata:
  name: yada-default
  annotations:
    service.beta.kubernetes.io/azure-load-balancer-internal: "false"
spec:
  type: LoadBalancer
  ports:
    - port: 8080
      targetPort: 8080
  selector:
    run: api-default
```

Example for Static Egress Gateway egress:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-egressgw
  labels:
    run: api-egressgw
spec:
  replicas: 1
  selector:
    matchLabels:
      run: api-egressgw
  template:
    metadata:
      labels:
        run: api-egressgw
      annotations:
        kubernetes.azure.com/static-gateway-configuration: egress-gw-1
    spec:
      containers:
        - image: erjosito/yadaapi:1.0
          name: api-egressgw
          ports:
            - containerPort: 8080
---
apiVersion: v1
kind: Service
metadata:
  name: yada-egressgw
  annotations:
    service.beta.kubernetes.io/azure-load-balancer-internal: "false"
spec:
  type: LoadBalancer
  ports:
    - port: 8080
      targetPort: 8080
  selector:
    run: api-egressgw
```

Deploy workloads:

```bash
kubectl apply -f yada-api-default.yaml
kubectl apply -f yada-api-egressgw.yaml
```

You can then compare inbound and outbound IP addresses to validate routing:

```bash
echo "default: svc IP=$(kubectl get svc yada-default -o jsonpath='{.status.loadBalancer.ingress[0].ip}'), egress IP=$(curl -s http://$(kubectl get svc yada-default -o jsonpath='{.status.loadBalancer.ingress[0].ip}'):8080/api/ip | jq -r '.my_public_ip')"
echo "egressgw: svc IP=$(kubectl get svc yada-egressgw -o jsonpath='{.status.loadBalancer.ingress[0].ip}'), egress IP=$(curl -s http://$(kubectl get svc yada-egressgw -o jsonpath='{.status.loadBalancer.ingress[0].ip}'):8080/api/ip | jq -r '.my_public_ip')"
```

## Private Egress (optional)

To use private egress IPs, specify `--vm-set-type VirtualMachines` when creating the gateway node pool, and configure `provisionPublicIps=false` in your StaticGatewayConfiguration. Note potential issues with the current private egress path (check Azure AKS documentation for updates).

```bash
az aks nodepool add \
  --cluster-name $cluster \
  --name privgwpool1 \
  --resource-group $rg \
  --mode gateway \
  --node-count 2 \
  --vm-set-type VirtualMachines \
  --gateway-prefix-size 30
```

## BYO Subnets & Custom Networking

When deploying AKS into custom (BYO) subnets:

- Ensure the gateway subnet accommodates the public IP prefix.
- Avoid UDRs that redirect pre-translation egress-gateway traffic.
- Avoid address range overlaps between pod/service CIDRs and gateway public IP prefix.

Key az CLI sequence:

```bash
az network vnet create --name $vnet_name --resource-group $rg --address-prefixes 10.240.0.0/16 --subnet-name aks-system --subnet-prefixes 10.240.0.0/22 --location $location --output none
az network vnet subnet create --vnet-name $vnet_name --resource-group $rg --address-prefix 10.240.4.0/22 --name aks-user --output none
az network vnet subnet create --vnet-name $vnet_name --resource-group $rg --address-prefix 10.240.8.0/24 --name aks-egress --output none
az aks create --resource-group $rg --name $cluster --location $location --enable-static-egress-gateway --vnet-subnet-id $(az network vnet subnet show -g $rg --vnet-name $vnet_name -n aks-system --query id -o tsv) --nodepool-name systempool1 --node-count 2 --node-vm-size $vm_size --network-plugin azure --network-dataplane=azure --output none
az aks nodepool add --cluster-name $cluster --name userpool1 --resource-group $rg --node-count 2 --vnet-subnet-id $(az network vnet subnet show -g $rg --vnet-name $vnet_name -n aks-user --query id -o tsv)
az aks nodepool add --cluster-name $cluster --name gwpool1 --resource-group $rg --mode gateway --node-count 2 --vnet-subnet-id $(az network vnet subnet show -g $rg --vnet-name $vnet_name -n aks-egress --query id -o tsv) --gateway-prefix-size 30
```

Assign required network contributor rights to the cluster identity as shown in the main content.

## Security and Automation Considerations

- Static Egress Gateway streamlines security-minded outbound access, enabling predictable egress IP usage for whitelisting and compliance.
- BYO network scenarios are well-supported but require careful planning regarding IP assignments and routing.
- This approach makes automation for multi-tenant or heavily-regulated workloads significantly easier.

## Conclusion

The Static Egress Gateway feature in AKS greatly simplifies the architecture needed to control multiple outbound IPs, reduces operational complexity, and meets security and compliance requirements cleanly. For implementations requiring advanced network segmentation or custom egress routing, BYO networking and private gateway options are supported with some additional caveats.

For continued updates on feature maturity or edge-case limitations, consult Azure documentation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/static-egress-gateway-in-aks-the-native-way-to-control-multiple/ba-p/4484179)
