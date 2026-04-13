---
section_names:
- azure
title: Connecting an ExpressRoute circuit to Megaport Virtual Edge
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/connecting-an-expressroute-circuit-to-megaport-virtual-edge/ba-p/4510770
primary_section: azure
author: Marc de Droog
tags:
- 802.1Q
- ARP Troubleshooting
- ASN
- Azure
- Azure ExpressRoute
- Azure Portal
- BGP
- Cisco 8000v
- Cisco C8000
- Community
- ExpressRoute Circuit
- ExpressRoute Private Peering
- ICMP Ping
- Megaport
- Megaport Cloud Router
- Megaport Virtual Edge
- Network Virtual Appliance
- NVA
- SSH
- VLAN
- Vnic
date: 2026-04-13 09:31:17 +00:00
---

Marc de Droog walks through connecting an Azure ExpressRoute circuit to Megaport Virtual Edge (MVE) running a Cisco 8000v NVA, including circuit creation, Megaport VXC setup, Azure Private Peering configuration, and the required Cisco IOS interface/BGP commands.<!--excerpt_end-->

# Connecting an ExpressRoute circuit to Megaport Virtual Edge

Megaport is an ExpressRoute partner in many locations: https://learn.microsoft.com/en-us/azure/expressroute/expressroute-locations?tabs=america%2Cj-m%2Cus-government-cloud%2Ca-C#global-commercial-azure

The [Megaport Cloud Router (MCR)](https://docs.megaport.com/mcr/) allows ExpressRoute customers to connect leased lines to their on-premise locations, and to connect other cloud providers. MCR is easy to set up and operate, and it automatically configures ExpressRoute Private Peering on both the Megaport and Azure sides. However, it does not have a command line interface and does not permit advanced configuration.

For advanced scenarios, [Megaport Virtual Edge (MVE)](https://docs.megaport.com/mve/) provides a platform to run fully configurable Network Virtual Appliances (NVAs) from a variety of vendors.

This post describes how to connect ExpressRoute to MVE running a Cisco 8000v NVA.

## Create the ExpressRoute circuit

1. In the Azure portal, create an ExpressRoute circuit with **Standard Resiliency** in a peering location where Megaport is available.
2. When the circuit deployment is completed, copy the **Service key**.

## Create MVE and ExpressRoute connections

1. Log in to the [Megaport management portal](https://portal.megaport.com/), go to **Services**, and click **Create MVE**.
2. Select **Cisco C8000** as the Vendor / Product.

On the next screen:

- Select the **Location** where the MVE is to be deployed (use the ExpressRoute peering location).
- Select the **MVE size**.

On the following screen:

- Select **Autonomous** under Appliance Mode.
- Paste a **2048-bit RSA SSH public key** (example guidance: https://learn.microsoft.com/en-us/viva/glint/setup/sftp-ssh-key-gen).
- Under **Virtual Interfaces (vNICs)**, add vNICs as needed.
  - One ExpressRoute circuit requires **2 vNICs** (one for each path).
  - **vNIC0** will be used to connect a **Megaport Internet VXC** for SSH access to the device.

On the next screen:

- Under **Finalize Details**, give the MVE a name.
- Verify the Summary, then click **Add MVE**.

### Provision an Internet VXC for SSH access

After adding the MVE, click **Create Megaport Internet** in the pop-up to provision an internet VXC:

- Select the location with the **lowest latency** to the MVE (top of the list).

On the next screen:

- Leave the name as proposed or change as needed.
- Set **Rate Limit** to **20 Mbps** (lowest possible; SSH access only).
- Leave **A-vNIC** set to **vNIC-0**.
- Leave **Preferred A-End VLAN** as **Untagged**.

Verify the configuration and click **Add VXC**.

On the main **Services** page, the MVE and Internet VXC will show **"Order pending"**.

### Create VXCs to the ExpressRoute circuit

Click **+Connection** in the MVE box to connect a VXC to the ExpressRoute Circuit:

- Under **Choose Destination Type**, select **Cloud**.
- Select **Microsoft Azure** as the Provider.
- Paste the circuit **Service Key** and select **Port** for the **Primary** path.
- Click **Next**.

On the next screen:

- Give the connection a name.
- Leave the **Rate Limit** as proposed (set to the circuit bandwidth).
- At **A-end vNIC**, select **vNIC-1** (do not leave this at vNIC-0).
- At **Preferred A-End VLAN**, turn off **Untag** and enter a VLAN number.
  - This VLAN will be used later for the sub-interface configuration on the MVE.

Scroll down to **Azure peering VLAN**:

- Leave **Configure Azure Peering VLAN** turned on.
- Enter the same VLAN ID that will be used for the **Private Peering** on the Azure end.
- Click **Next**.

Verify the configuration summary and click **Add VXC**.

Repeat the process for the **Secondary** path:

- Terminate on **vNIC-2**.
- Use a **different** VLAN ID for **Preferred A-End VLAN**.
- Use the VLAN ID that will be used in the Private Peering for **Azure peering VLAN**.

When the second ExpressRoute VXC is configured:

1. Click **Review Order** in the right-hand bar of the Services screen.
2. After validation completes, click **Order Now**.

This provisions the MVE and the VXC (typically a few minutes). In the Azure portal, the ExpressRoute circuit **Provider Status** will change to **Provisioned**.

## Configure Private Peering

1. In the Azure portal, open the ExpressRoute circuit.
2. With Provider Status now **Provisioned**, enable Private Peering:
   - Click **Peerings** under Settings.
   - Click **Azure private**.
3. Enter the **Peer ASN** and the **Primary** and **Secondary** subnets.
4. Under **VLAN ID**, enter the **same number** configured under **Azure peering VLAN** in the Primary and Secondary VXC configurations in the Megaport portal.

## Configure Cisco IOS

### SSH to the MVE

Establish an SSH session to the MVE using the public IP address from the internet VXC and the private key that matches the public key used when deploying the MVE:

```bash
ssh -i <private-key-file> mveadmin@<public ip>
```

### Configure interfaces and subinterfaces

```text
interface GigabitEthernet2
 no ip address
 no shutdown
 negotiation auto
!
interface GigabitEthernet2.100
 encapsulation dot1Q 100
 ip address 192.168.0.1 255.255.255.252
!
interface GigabitEthernet3
 no ip address
 no shutdown
 negotiation auto
!
interface GigabitEthernet3.101
 encapsulation dot1Q 101
 ip address 192.168.0.5 255.255.255.252
```

Notes:

- Use the **Preferred A-end VLAN** values set in the primary and secondary VXCs to configure `encapsulation dot1Q` on the subinterfaces.
- Use the **lower** address of the /30 subnets configured on the Azure **Private Peering**.

### Validate connectivity and ARP

The higher IP addresses of the Private Peering should respond to ping:

```text
ping 192.168.0.2
Type escape sequence to abort.
Sending 5, 100-byte ICMP Echos to 192.168.0.2, timeout is 2 seconds:
!!!!!
Success rate is 100 percent (5/5), round-trip min/avg/max = 1/1/1 ms
```

If ping does not work, there likely is an ARP resolution issue:

- Run `show arp` and `debug arp`.
- Check the Azure-side ARP table guidance: https://learn.microsoft.com/en-us/troubleshoot/azure/expressroute/expressroute-troubleshooting-arp-resource-manager

### Configure BGP

```text
router bgp 64000
 bgp log-neighbor-changes
 neighbor 192.168.0.2 remote-as 12076
 neighbor 192.168.0.2 soft-reconfiguration inbound
 neighbor 192.168.0.6 remote-as 12076
 neighbor 192.168.0.6 soft-reconfiguration inbound
```

Verify both neighbors show `BGP state = Established`:

```text
sh ip bgp neighbor 192.168.0.2
BGP neighbor is 192.168.0.2, remote AS 12076, external link
BGP version 4, remote router ID 192.168.0.2
BGP state = Established, up for 1d21h
...
```

This completes the basic configuration of ExpressRoute to MVE.

## Version info

- Updated Apr 13, 2026
- Version 3.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking-blog/connecting-an-expressroute-circuit-to-megaport-virtual-edge/ba-p/4510770)

