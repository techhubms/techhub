---
layout: "post"
title: "Rebuilding Search Architecture for High Availability in GitHub Enterprise Server"
description: "David Tippett details the engineering overhaul of GitHub Enterprise Server's search architecture to improve high availability (HA), resilience, and maintenance. The update leverages Elasticsearch’s Cross Cluster Replication (CCR), reducing operational complexity for administrators and enhancing durability and fault tolerance for enterprise users."
author: "David Tippett"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/architecture-optimization/how-we-rebuilt-the-search-architecture-for-high-availability-in-github-enterprise-server/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2026-03-03 18:45:09 +00:00
permalink: "/2026-03-03-Rebuilding-Search-Architecture-for-High-Availability-in-GitHub-Enterprise-Server.html"
categories: ["DevOps"]
tags: ["Architecture & Optimization", "CCR", "Cluster Management", "Cross Cluster Replication", "DevOps", "Distributed Systems", "Elasticsearch", "Engineering", "Engineering Workflow", "Failover", "GitHub Enterprise Server", "High Availability", "Index Replication", "Infrastructure Optimization", "News", "Search Architecture", "Search Engineering", "Server Resilience", "System Administration"]
tags_normalized: ["architecture and optimization", "ccr", "cluster management", "cross cluster replication", "devops", "distributed systems", "elasticsearch", "engineering", "engineering workflow", "failover", "github enterprise server", "high availability", "index replication", "infrastructure optimization", "news", "search architecture", "search engineering", "server resilience", "system administration"]
---

David Tippett shares the engineering journey behind enhancing GitHub Enterprise Server's search reliability and high availability, focusing on Elasticsearch’s Cross Cluster Replication and resilient workflows.<!--excerpt_end-->

# Rebuilding Search Architecture for High Availability in GitHub Enterprise Server

**Author:** David Tippett

## Overview

Search is foundational to GitHub Enterprise Server (GHES): powering not just search bars and filtering, but also core components like the releases and projects pages, and even issue and pull request counts. As such, enhancing search durability and high availability (HA) were primary engineering goals this past year.

## Challenges with Previous Architecture

- **Old Approach:** Search utilized Elasticsearch clusters spanning both primary and replica nodes for HA deployments. Primary nodes handled all writes/traffic, with replicas as read-only followers, using a leader/follower pattern.
- **Issue:** Elasticsearch clusters could reassign primary shards to replica nodes. If a replica was down for maintenance, this could lock the server in an unhealthy state, requiring the replica to rejoin before recovery—a fragile design for uptime and durability.
- Previous solutions included health checks and attempted search mirroring, but these added complexity and weren't robust for long-term maintenance.

## The Architectural Shift: Cross Cluster Replication (CCR)

- **CCR Utilization:** Now leveraging Elasticsearch's Cross Cluster Replication (CCR), each GHES instance operates as a single-node cluster instead of multi-node clusters.
- **Benefit:** CCR allows index data to be replicated between independent Elasticsearch instances (primary and replica nodes). This ensures that critical data is always available and reduces the operational burden during upgrades or failovers.
- **Technical Consideration:** Data is copied after being durably persisted in Lucene segments, preventing loss due to node failovers.

## Under the Hood: Workflows & Automation

- **Bootstrapping:** Administrators need a bootstrap step to attach followers to existing indexes and enable auto-follow for future indexes.
- **Sample Workflow (Pseudocode):**

  ```python
  function bootstrap_ccr(primary, replica):
      # Fetch the current indexes
      primary_indexes = list_indexes(primary)
      replica_indexes = list_indexes(replica)
      managed = filter(primary_indexes, is_managed_ghe_index)
      for index in managed:
          if index not in replica_indexes:
              ensure_follower_index(replica, leader=primary, index=index)
          else:
              ensure_following(replica, leader=primary, index=index)
      ensure_auto_follow_policy(replica, leader=primary, patterns=[managed_index_patterns], exclude=[system_index_patterns])
  ```

- **Lifecycle Management:** Custom workflows for failover, index deletion, and upgrades are required, since Elasticsearch only manages document replication; index state and lifecycle still need manual/automated logic.

## How to Get Started

- **Contact GitHub Support:** To move to the new HA mode, contact [support@github.com](mailto:support@github.com) to set up the necessary license for GitHub Enterprise Server.
- **Configuration:** Set `ghe-config app.elasticsearch.ccr true` and run a `config-apply` or upgrade to version 3.19.1 or later.
- **Migration:** Upon restart, GHES will consolidate data onto primary nodes, break out the old clustering, and establish CCR-based replication. The process duration depends on the instance’s size.
- **Rollout:** The HA mode is currently optional, but will become the default over the next two years, providing ample time for feedback and transition.

## Visual Architecture

- *Refer to the embedded diagrams in the original post for breakdowns of the architectural evolution and replication flows.*

## Summary

The move to CCR in GHES dramatically simplifies administrator workload and increases resilience, making search faster, safer, and more robust for enterprises.

For more details on enabling the new HA mode or technical guidance, reach out to GitHub support.

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/architecture-optimization/how-we-rebuilt-the-search-architecture-for-high-availability-in-github-enterprise-server/)
