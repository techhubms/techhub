---
layout: "post"
title: "Troubleshooting Kubernetes Network Issues with Retina and eBPF"
description: "This article, authored by kamilp, explores how the open-source Retina platform streamlines network troubleshooting for Kubernetes environments at scale. It demonstrates using Retina's CLI for distributed packet captures, persistent storage, and advanced network debugging via eBPF-powered tools (pwru, bpftool, Inspektor Gadget). The post details practical CLI examples and advanced use of Retina Shell for deep inspection on AKS and other cloud-native clusters."
author: "kamilp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/troubleshooting-network-issues-with-retina/ba-p/4446071"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-22 16:38:59 +00:00
permalink: "/2025-08-22-Troubleshooting-Kubernetes-Network-Issues-with-Retina-and-eBPF.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "Azure", "Bpftool", "Cloud Native", "Community", "DevOps", "Distributed Systems", "Ebpf", "Inspektor Gadget", "Kubectl", "Kubernetes", "Network Debugging", "Network Troubleshooting", "Observability", "Packet Capture", "Persistent Volume", "Pwru", "Retina", "Security"]
tags_normalized: ["aks", "azure", "bpftool", "cloud native", "community", "devops", "distributed systems", "ebpf", "inspektor gadget", "kubectl", "kubernetes", "network debugging", "network troubleshooting", "observability", "packet capture", "persistent volume", "pwru", "retina", "security"]
---

kamilp provides a deep dive into network observability for Kubernetes clusters using Retina. The article explains distributed packet capture, persistent storage, and advanced eBPF-based network analysis using Retina and its accompanying shell tools.<!--excerpt_end-->

# Troubleshooting Kubernetes Network Issues with Retina and eBPF

**Author:** kamilp

Kubernetes clusters at enterprise scale pose unique networking challenges, especially for microservices spanning hundreds or thousands of pods across numerous nodes. Traditional troubleshooting with tools like `tcpdump` is impractical due to the distributed, ephemeral nature of containers.

## What is Retina?

Retina is a cloud-agnostic, open-source network observability platform for Kubernetes, leveraging eBPF to gather deep networking insights without the overhead of manual tool installation. It is developed with multi-cloud compatibility and automation in mind.

- [Blog post: Retina, eBPF, and Multi-Cloud Observability](https://techcommunity.microsoft.com/blog/linuxandopensourceblog/ebpf-powered-observability-beyond-azure-a-multi-cloud-perspective-with-retina/4403361)
- [Retina Documentation](https://retina.sh/)

## Distributed Packet Captures

Retina streamlines the process of capturing traffic across Kubernetes clusters:

- **On-Demand Capture:** Set node, pod, and traffic filters to remotely initiate packet captures cluster-wide via CLI or CRD.
- **Centralized Storage:** Captures are output to host filesystems, Kubernetes persistent volumes (PVC), or cloud blob storage (including Azure blob storage), making post-capture analysis easier.
- **Automation:** Each capture is encapsulated in a Kubernetes job, running for a configurable duration and saving both `.pcap` and rich metadata including iptables, socket, and kernel state.
- **Naming Conventions:** Captures and resulting tarballs are uniquely named for traceability.

For an exhaustive list of all captured metadata, visit [Tarball Contents](https://retina.sh/docs/Captures/cli#file-and-directory-structure-inside-the-tarball).

### How to Use

- **Install Retina CLI:**

  ```bash
  kubectl krew install retina
  ```

- **Create a Packet Capture:**

  ```bash
  kubectl retina capture create
  ```

- **Monitor and Download Captures:**

  ```bash
  kubectl retina capture list
  kubectl retina capture download --name <capture-name>
  tar -xvf <name.tar.gz>
  ```

#### Common Scenarios

- Capture traffic for one deployment:

  ```bash
  kubectl retina capture create \
    --name coredns \
    --pod-selectors "k8s-app=kube-dns" \
    --namespace-selectors="kubernetes.io/metadata.name=kube-system"
  ```

- Filter for DNS traffic with tcpdump:

  ```bash
  kubectl retina capture create \
    --name coredns \
    --tcpdump-filter "tcp and port 53"
  ```

- Capture across all Linux nodes, uploading to cloud blob:

  ```bash
  kubectl retina capture create \
    --name all-linux-node-443 \
    --node-selectors "kubernetes.io/os=linux" \
    --tcpdump-filter "tcp and port 443" \
    --blob-upload <SAS URL>
  ```

## Advanced Debug Tools: Retina Shell

The [Retina Shell](https://retina.sh/docs/Troubleshooting/shell) (experimental as of v1.0.0-rc2) provides an interactive shell environment in pods or nodes with tools like `curl`, `ping`, `nslookup`, `iptables`, and expert instrumentation for network debugging.

### Integrated Tools

- **pwru:** [pwru](https://github.com/cilium/pwru) is an eBPF-based packet tracing tool for the Linux kernel.
  - *Example:* Trace HTTP traffic between microservices:

    ```bash
    pwru "tcp and (src port 8080 or dst port 8080)"
    ```

- **bpftool:** [bpftool](https://github.com/libbpf/bpftool) inspects and manipulates BPF program and map objects.
  - *Example:* Show loaded programs:

    ```bash
    bpftool prog show
    ```

- **Inspektor Gadget:** [Inspektor Gadget](https://github.com/inspektor-gadget/inspektor-gadget) offers observability gadgets for tracing Kubernetes events (DNS, OOM kills, etc.)
  - *Example:* Trace DNS queries:

    ```bash
    ig run trace_dns:latest -n <namespace> -p <pod>
    ```

**Retina Shell only supports Linux-based targets as of v1.0.0-rc2.**

## Conclusion

Retina provides a comprehensive network observability solution for Kubernetes, reducing manual effort and enabling distributed, persistent packet captures as well as advanced live troubleshooting with eBPF-based tools. Whether on Azure Kubernetes Service (AKS) or multi-cloud deployments, it's a vital addition for debugging complex networking scenarios.

- [GitHub: microsoft/retina](https://github.com/microsoft/retina)
- [Contributing Guide](https://retina.sh/docs/Contributing/development/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/troubleshooting-network-issues-with-retina/ba-p/4446071)
