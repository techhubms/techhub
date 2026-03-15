---
external_url: https://devclass.com/2026/01/29/kubernetes-leadership-warns-of-ingress-nginx-risks-but-has-also-hastened-its-deprecation/
title: Kubernetes Leadership Urges Migration from Ingress NGINX Due to Security Risks and Deprecation
author: Tim Anderson
primary_section: devops
feed_name: DevClass
date: 2026-01-29 17:01:00 +00:00
tags:
- Blogs
- Cilium
- Cloud Native
- Deprecation
- Development
- DevOps
- Envoy Gateway
- F5 NGINX Ingress
- Gateway API
- Ingres NGINX
- Ingress NGINX
- Kubecon
- Kubernetes
- Migration
- Open Source
- Remote Code Execution
- Security
- Security Risks
- Traefik
- Vulnerability Management
section_names:
- devops
- security
---
Tim Anderson outlines the Kubernetes committees' warnings and technical reasons for the urgent migration from Ingress NGINX, detailing project deprecation, security issues, and community response.<!--excerpt_end-->

# Kubernetes Leadership Urges Migration from Ingress NGINX Due to Security Risks and Deprecation

**Author: Tim Anderson**

The Kubernetes Steering and Security Response Committees have issued a strong warning regarding the continued use of Ingress NGINX, which is set to receive no further security patches after March 2026. The committees emphasized the severity of the situation, urging users to begin migrating to alternatives immediately. Despite some recent offers to help maintain the project, these were deemed too late, and the official decision was to accelerate deprecation.

## Key Points

- **End of Support**: Ingress NGINX will not receive security patches after March 2026.
- **Widespread Usage**: Around 50% of cloud-native environments rely on this ingress controller.
- **Migration Mandate**: Official recommendations stress immediate migration planning, as existing deployments may continue to work but will grow increasingly insecure.
- **No Simple Replacement**: Alternatives like Gateway API, Envoy Gateway, Traefik, Cilium, and F5 NGINX Ingress are not drop-in replacements—migrations can be complex due to feature differences, especially around annotations.

## Background and Deprecation Timeline

Issues with a shortage of maintainers and contributors have long plagued the project, coming to a head at KubeCon Atlanta in November 2025. There, maintainer announcements confirmed the cessation of development on Ingress NGINX and its intended successor, InGate, due to lack of interest and unsustainable pressure on the small team.

Migrating away from Ingress NGINX is not straightforward. While the Kubernetes Gateway API is promoted as a successor, the reliance on specific NGINX annotations creates compatibility issues. Even close alternatives, such as F5 NGINX Ingress, use different annotations and require manual migration, with guides provided by project maintainers.

## Security Concerns

Several critical security issues have impacted Ingress NGINX, including the notorious "Ingres Nightmare" vulnerability enabling remote code execution via arbitrary NGINX configuration injection. The project’s openness to features like user-supplied Lua scripts was cited as a risk leading to these vulnerabilities. With maintainers burned out and unable to continue, the risk surface was deemed unacceptable without adequate caretaker resources.

## The Community and Official Guidance

Recent offers to aid project maintenance were ultimately declined by project leadership, who noted that rebuilding trust takes time. Users are urged to contribute to alternative projects like Gateway API or consider commercial support paths, such as those offered by SuSe for legacy NGINX users.

## Migration Options

- **Gateway API**: Officially recommended, with broader and more future-proof capabilities, though not a direct substitute for all workflows.
- **Other Controllers**: Envoy Gateway, Traefik, Cilium, F5 NGINX Ingress (with caveats).
- **Commercial Support**: SuSe offers transitional support for legacy deployments.

This deprecation highlights the challenges of open-source sustainability for critical cloud-native infrastructure and signals a significant shift in recommended Kubernetes ingress controller practices.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/29/kubernetes-leadership-warns-of-ingress-nginx-risks-but-has-also-hastened-its-deprecation/)
