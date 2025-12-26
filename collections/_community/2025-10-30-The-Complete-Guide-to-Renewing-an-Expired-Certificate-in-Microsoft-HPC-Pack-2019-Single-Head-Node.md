---
layout: "post"
title: "The Complete Guide to Renewing an Expired Certificate in Microsoft HPC Pack 2019 (Single Head Node)"
description: "This in-depth guide, authored by vinilv, provides step-by-step instructions for administrators to renew expired certificates in Microsoft HPC Pack 2019 clusters with a single head node. It covers checking expiry, creating a new self-signed certificate, updating cluster nodes and databases, and restoring cluster functionality without reinstallation."
author: "vinilv"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-30 06:27:43 +00:00
permalink: "/community/2025-10-30-The-Complete-Guide-to-Renewing-an-Expired-Certificate-in-Microsoft-HPC-Pack-2019-Single-Head-Node.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Certificate Renewal", "Cluster Security", "Community", "Compute Node", "Data Protection", "Head Node", "HPC Cluster Manager", "Microsoft HPC Pack", "PowerShell", "Private Key", "Root Certificate", "Security", "Self Signed Certificate", "SQL Server Management Studio", "SSLThumbprint", "Windows Server"]
tags_normalized: ["azure", "certificate renewal", "cluster security", "community", "compute node", "data protection", "head node", "hpc cluster manager", "microsoft hpc pack", "powershell", "private key", "root certificate", "security", "self signed certificate", "sql server management studio", "sslthumbprint", "windows server"]
---

vinilv details how to renew expired certificates in Microsoft HPC Pack 2019 clusters, guiding administrators through PowerShell commands, certificate creation, installation, and restoring secure node communication.<!--excerpt_end-->

# The Complete Guide to Renewing an Expired Certificate in Microsoft HPC Pack 2019 (Single Head Node)

Managing certificates is essential for the security and proper functioning of Microsoft HPC Pack 2019 clusters. In this comprehensive guide, vinilv explains how to identify, replace, and update an expired certificate in a single-head-node cluster, ensuring restored communication and business continuity.

## Step 1: Check the Current Certificate Expiry

- Use PowerShell to list and examine the current HPC certificate:

  ```powershell
  Get-ChildItem -Path Cert:\LocalMachine\root | Where-Object { ---
layout: "post"
title: "The Complete Guide to Renewing an Expired Certificate in Microsoft HPC Pack 2019 (Single Head Node)"
description: "This in-depth guide, authored by vinilv, provides step-by-step instructions for administrators to renew expired certificates in Microsoft HPC Pack 2019 clusters with a single head node. It covers checking expiry, creating a new self-signed certificate, updating cluster nodes and databases, and restoring cluster functionality without reinstallation."
author: "vinilv"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-30 06:27:43 +00:00
permalink: "2025-10-30-The-Complete-Guide-to-Renewing-an-Expired-Certificate-in-Microsoft-HPC-Pack-2019-Single-Head-Node.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Certificate Renewal", "Cluster Security", "Community", "Compute Node", "Data Protection", "Head Node", "HPC Cluster Manager", "Microsoft HPC Pack", "PowerShell", "Private Key", "Root Certificate", "Security", "Self Signed Certificate", "SQL Server Management Studio", "SSLThumbprint", "Windows Server"]
tags_normalized: [["azure", "certificate renewal", "cluster security", "community", "compute node", "data protection", "head node", "hpc cluster manager", "microsoft hpc pack", "powershell", "private key", "root certificate", "security", "self signed certificate", "sql server management studio", "sslthumbprint", "windows server"]]
---

vinilv details how to renew expired certificates in Microsoft HPC Pack 2019 clusters, guiding administrators through PowerShell commands, certificate creation, installation, and restoring secure node communication.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444)
.Subject -like "HPC" }
  $thumbprint = "<Thumbprint value from the previous command>".ToUpper()
  $cert = Get-ChildItem -Path Cert:\LocalMachine\My | Where-Object { ---
layout: "post"
title: "The Complete Guide to Renewing an Expired Certificate in Microsoft HPC Pack 2019 (Single Head Node)"
description: "This in-depth guide, authored by vinilv, provides step-by-step instructions for administrators to renew expired certificates in Microsoft HPC Pack 2019 clusters with a single head node. It covers checking expiry, creating a new self-signed certificate, updating cluster nodes and databases, and restoring cluster functionality without reinstallation."
author: "vinilv"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-30 06:27:43 +00:00
permalink: "2025-10-30-The-Complete-Guide-to-Renewing-an-Expired-Certificate-in-Microsoft-HPC-Pack-2019-Single-Head-Node.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Certificate Renewal", "Cluster Security", "Community", "Compute Node", "Data Protection", "Head Node", "HPC Cluster Manager", "Microsoft HPC Pack", "PowerShell", "Private Key", "Root Certificate", "Security", "Self Signed Certificate", "SQL Server Management Studio", "SSLThumbprint", "Windows Server"]
tags_normalized: [["azure", "certificate renewal", "cluster security", "community", "compute node", "data protection", "head node", "hpc cluster manager", "microsoft hpc pack", "powershell", "private key", "root certificate", "security", "self signed certificate", "sql server management studio", "sslthumbprint", "windows server"]]
---

vinilv details how to renew expired certificates in Microsoft HPC Pack 2019 clusters, guiding administrators through PowerShell commands, certificate creation, installation, and restoring secure node communication.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444)
.Thumbprint -eq $thumbprint }
  $cert | Select-Object Subject, NotBefore, NotAfter, Thumbprint, Date
  ```

- Confirm the system date:

  ```powershell
  Date
  ```

## Step 2: Prepare a New Self-Signed Certificate

- Requirements:
  - Private key capable of key exchange
  - Key usage: Digital Signature, Key Encipherment, Key Agreement, Certificate Signing
  - Enhanced key usage: Client Authentication, Server Authentication
  - Same subject name as the old certificate
- To find the subject name:

  ```powershell
  $thumbprint = (Get-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\HPC -Name SSLThumbprint).SSLThumbPrint
  $subjectName = (Get-Item Cert:\LocalMachine\My\$thumbprint).Subject
  $subjectName
  ```

## Step 3: Create and Export the Certificate

- Generate a new certificate (valid for 1 year), then export .pfx and .cer files:

  ```powershell
  $subjectName = "HPC Pack Node Communication"

  $pfxcert = New-SelfSignedCertificate -Subject $subjectName -KeySpec KeyExchange -KeyLength 2048 -HashAlgorithm SHA256 -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1,1.3.6.1.5.5.7.3.2") -Provider "Microsoft Enhanced RSA and AES Cryptographic Provider" -CertStoreLocation Cert:\CurrentUser\My -KeyExportPolicy Exportable -NotAfter (Get-Date).AddYears(1) -NotBefore (Get-Date).AddDays(-1)

  $certThumbprint = $pfxcert.Thumbprint

  $null = New-Item $env:Temp\$certThumbprint -ItemType Directory

  $pfxPassword = Get-Credential -UserName 'Protection password' -Message 'Enter protection password below'

  Export-PfxCertificate -Cert Cert:\CurrentUser\My\$certThumbprint -FilePath "$env:Temp\$certThumbprint\PrivateCert.pfx" -Password $pfxPassword.Password
  Export-Certificate -Cert Cert:\CurrentUser\My\$certThumbprint -FilePath "$env:Temp\$certThumbprint\PublicCert.cer" -Type CERT -Force
  start "$env:Temp\$certThumbprint"
  ```

## Step 4: Copy Certificate to Install Share

- On the head node, copy the generated certificates to:
  - `C:\Program Files\Microsoft HPC Pack 2019\Data\InstallShare\Certificates`

## Step 5: Rotate Certificates on Compute Nodes First

- Download [`Update-HpcNodeCertificate.ps1`](https://raw.githubusercontent.com/Azure/hpcpack-template/master/Scripts/Update-HpcNodeCertificate.ps1) to your HPC install share.
- On each compute node (as Administrator):

  ```powershell
  PowerShell.exe -ExecutionPolicy ByPass -Command "\\<headnode>\REMINST\Update-HpcNodeCertificate.ps1 -PfxFilePath \\<headnode>\REMINST\Certificates\HpcCnCommunication.pfx -Password <password>"
  ```

- After rotation, compute nodes may show as Offline until the head node is updated.

## Step 6: Update Certificate on the Head Node

- On the head node (as Administrator):

  ```powershell
  $certPassword = ConvertTo-SecureString -String "YourPassword" -AsPlainText -Force

  Import-PfxCertificate -FilePath "C:\Program Files\Microsoft HPC Pack 2019\Data\InstallShare\Certificates\PrivateCert.pfx" -CertStoreLocation "Cert:\LocalMachine\My" -Password $certPassword
  PowerShell.exe -ExecutionPolicy ByPass -Command "Import-certificate -FilePath \\master\REMINST\Certificates\PublicCert.cer -CertStoreLocation cert:\LocalMachine\Root"
  Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\HPC" -Name SSLThumbprint -Value <Thumbprint>
  Set-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\HPC" -Name SSLThumbprint -Value <Thumbprint>
  ```

## Step 7: Update Thumbprint in SQL Database

- Install [SQL Server Management Studio (SSMS)](https://aka.ms/ssms/21/release/vs_SSMS.exe) and connect to the HPC database.
- Navigate to `HPCHAStorage → Tables → dbo.DataTable` and update the SSLThumbprint:

  ```sql
  Update dbo.DataTable set dvalue='<NewThumbprint>' where dpath = 'HKEY_LOCAL_MACHINE\Software\Microsoft\HPC' and dkey = 'SSLThumbprint'
  ```

## Step 8: Reboot the Head Node

- Restart the head node to apply all changes.
- After reboot, open HPC Cluster Manager to confirm restored functionality.

## Summary

By following these instructions, administrators can safely renew expired HPC Pack 2019 certificates, ensuring secure communication and minimizing downtime for high-performance computing workloads.

---

*Authored by vinilv. For feedback and questions, comment below or follow Azure High Performance Computing Blog for future updates.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/the-complete-guide-to-renewing-an-expired-certificate-in/ba-p/4465444)
