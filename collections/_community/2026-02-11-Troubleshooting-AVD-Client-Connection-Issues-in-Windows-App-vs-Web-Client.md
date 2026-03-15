---
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop/your-computer-was-unable-to-connect-to-the-remote-computer/m-p/4494411#M13999
title: Troubleshooting AVD Client Connection Issues in Windows App vs Web Client
author: cadminimum
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-11 18:03:11 +00:00
tags:
- AVD
- Azure
- Azure Virtual Desktop
- Community
- Conditional Access
- Entra ID
- Firewall
- Gateway
- Host Pool
- Private Endpoint
- RDP
- Remote Desktop
- Security
- SessionHost
- Troubleshooting
- Windows App
section_names:
- azure
- security
---
cadminimum documents a problem connecting to Azure Virtual Desktop sessions via the Windows App while the web client works, sharing troubleshooting steps involving RDP, Entra ID, and firewall traffic analysis.<!--excerpt_end-->

# Troubleshooting AVD Client Connection Issues in Windows App vs Web Client

**Author:** cadminimum

## Issue Summary

A new Azure Virtual Desktop (AVD) workspace was deployed with a SessionDesktop application and host pool. The web-based client allows successful connections and session launches, while the Windows App (Remote Desktop client) consistently fails regardless of being on-premises or off-premises, displaying the error "Your computer was unable to connect to the remote computer."

## Troubleshooting Steps Taken

- **Authentication:** Entra logs confirm that authentication to the Windows App succeeds. Conditional Access Policies also appear as satisfied and enforced, indicating no evident identity roadblocks.
- **Network Analysis:** The host pool is configured with a private endpoint. Firewall logs show traffic between the Windows client and the private endpoint on port 443, including access to several Microsoft FQDNs (e.g., `windows365.microsoft.com`, `rdweb-g-us-r0-wvd.microsoft.com`, `afdfp-rdgateway-r0.wvd.microsoft.com`). Notably, there is no traffic on RDP's traditional ports 3389/3390.
- **Client Versions:** Several versions of the Windows clients were tested:
  - Windows App version 2.0.918.0 (current at time of writing)
  - Classic Remote Desktop app (deprecated)
  Both experience the same failure.
- **Session App Configuration:** The "SessionDesktop" application exposes no parameters other than the display name.
- **RDP Properties & Advanced Config:** Multiple connection parameters (`gatewayusagemethod`, `gatewaybrokeringtype`, `wvd endpoint pool`, etc.) were tested within downloaded `.rdpw` files from the web client, but these had no effect on the outcome.
- **Other Attempts:** Changes to RDP Shortpath, network and credential delegation settings (Cred SSP), and various firewall rules were made.

## Observations & Hypotheses

- Since the web client connects without issue, the backend services and session hosts are reachable and authentication works, suggesting a client-specific, protocol-specific, or network routing isolation affecting the Windows App.
- Private endpoints and firewall configurations may be blocking protocols or FQDNs used exclusively by the Windows App clients, which can differ from web-based session brokering.
- The lack of activity on ports 3389/3390 likely means the sessions are (correctly) using reverse connect but may have other connectivity issues related to web sockets or service tags not covered by the firewall exceptions.
- Advanced RDP parameters in `.rdpw` files do not override all dynamic configuration imposed during session brokering, so manual edits may not influence session startup as expected.

## Next Steps for Resolution

- **Verify Required FQDNs:** Ensure all necessary Microsoft endpoints (per official documentation) for AVD—including those specifically needed for the Windows App—are accessible through your private endpoint/firewall.
- **Firewall Review:** Double-check that all required ports and protocols (particularly for reverse connect, websocket traffic, and hybrid join scenarios) are permitted outbound from the client and inbound to the hostpool via the private endpoint.
- **Client Logs:** Review diagnostics in the Windows App (enable advanced logging) to compare against successful web client connections.
- **RDP Shortpath:** Make sure this is either fully enabled or fully disabled on both clients and backend as partial configuration may block traffic.
- **Conditional Access:** Although authentication passes, check for any subtle policy differences specific to the Windows App client ID or related device conditions.
- **Microsoft Support Resources:** If all else fails, gather the logs and submit a support case to Azure—with details about the private endpoint, network traces, and confirmed rules above.

## Resources

- [Microsoft: Required URL list for Azure Virtual Desktop](https://learn.microsoft.com/en-us/azure/virtual-desktop/safe-url-list)
- [Troubleshooting AVD client connection issues](https://learn.microsoft.com/en-us/azure/virtual-desktop/troubleshoot-client)
- [RDP Shortpath configuration](https://learn.microsoft.com/en-us/azure/virtual-desktop/shortpath)

---

> **Any help would be appreciated.
>
> Thanks."

## Takeaway

This post is a strong troubleshooting reference for similar AVD connection problems, especially in hybrid environments with strict network segmentation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/your-computer-was-unable-to-connect-to-the-remote-computer/m-p/4494411#M13999)
