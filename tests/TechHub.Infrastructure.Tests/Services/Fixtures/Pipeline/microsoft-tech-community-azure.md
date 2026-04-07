When organizations migrate to the cloud, applications are at the center of every decision. And behind every application is data, stored in file shares, accessed by users and services, and critical to keeping the application running. Moving that data seamlessly is not optional; it is a fundamental part of any successful migration. 

That is why we are excited to announce that [Azure Migrate](https://portal.azure.com/?feature.customportal=false#view/Microsoft_Azure_Migrate/AmhResourceMenuBlade/~/getStarted) now supports discovery and assessment of SMB and NFS file shares hosted on Windows and Linux servers, giving you a complete, data-driven picture of your file share estate and a clear path to Azure Files. 

File shares are the backbone of day-to-day operations for many enterprises, storing project files, user data, application content, and shared resources across teams. Yet when it comes to cloud migration planning, these workloads are often assessed manually through scripts or spreadsheets, leading to incomplete inventories and inaccurate cost estimates. 

With this new capability in Azure Migrate, you can now: 

- Discover SMB and NFS file shares across Windows and Linux servers, automatically and agentlessly

- Assess readiness of each file share for migration to Azure Files 

- Get right-sized recommendations mapped to the correct Azure Files tier, Standard or Premium 

- Visualize cost estimates before you commit to migration 

All of this from the same Azure Migrate portal you already use. No new tools, no extra agents. 

Previously, customers planning a migration of file shares to Azure Files had to rely on manual discovery methods, running scripts, compiling spreadsheets, or engaging consultants just to understand what they had. This created uncertainty, slowed decisions, and led to costly surprises post-migration. 

Now, with Azure Migrate’s file share discovery and assessment: 

Applications migrate as a whole, not in pieces. File share data is an integral part of your application. Azure Migrate helps you treat it that way, giving you visibility into the file shares your applications depend on so nothing gets left behind. 

No more manual inventory. The Azure Migrate appliance automatically discovers all SMB and NFS file shares across your Windows and Linux servers. 

No more sizing guesswork. Assessments analyze actual usage data and recommend the right Azure Files tier for each share, so you do not over-provision Premium tier where Standard suffices, or under-provision where performance matters. 

No more migration risk. Readiness checks surface compatibility issues, unsupported configurations, and permission gaps before you migrate, not after. 

Faster planning, faster execution. What used to take days of manual effort is now automated, giving your team a migration-ready inventory in hours. 

Getting started is straightforward with the existing Azure Migrate appliance: 

**Step 1: Set up or update your Azure Migrate appliance **

Deploy the Azure Migrate appliance in your on-premises environment if you have not already. If you have an existing appliance, update it to the latest version to enable file share discovery. 

**Step 2: Run discovery **

The appliance automatically discovers SMB and NFS file shares across your environment, along with metadata including storage consumed, file counts, and access patterns. Discovery typically completes within a few hours depending on the size of your environment. 

**Step 3: Create an Azure Files assessment **

Once discovery is complete, select the discovered file shares you want to include in scope and configure your assessment settings: region, pricing tier preference, and redundancy options. 

**Step 4: Review readiness and recommendations **

The assessment classifies each share as Ready, Ready with Conditions, or Not Ready for Azure Files, and provides: 

- Recommended Azure Files tier (Standard or Premium) per share 

- Monthly cost estimates 

- Readiness blockers to address before migration 

**Step 5: Plan and execute migration **

Use the assessment output to prioritize your migration waves and execute the data transfer using Azure Storage Mover or AzCopy. 

Your applications depend on their data. Make sure that data moves with them. 

Start leveraging file share discovery and assessment in Azure Migrate today and migrate to Azure Files with confidence. 

Learn more and get started with [Discovery and Assessment of fileshares to Azure Files](https://learn.microsoft.com/en-us/azure/migrate/create-file-share-assessment?view=migrate)