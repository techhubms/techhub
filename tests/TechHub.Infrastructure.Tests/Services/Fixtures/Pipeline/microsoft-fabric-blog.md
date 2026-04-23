Modern organizations are at a crossroads. Legacy SQL Server workloads need modernization, but migration complexity often stands in the way. What if you could bring your SQL databases into a unified analytics platform—without the hassle of manual schema conversion, complex ETL pipelines, or costly re-architecture?

That’s exactly what the new Migration Assistant for SQL database in Microsoft Fabric delivers.

Following [Announcing SQL database in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/announcing-sql-database-in-microsoft-fabric-public-preview), we’ve seen incredible momentum. Over 50,000+ SQL databases have been created in Fabric, and organizations are modernizing SQL workloads, reducing operational overhead, and bringing operational data closer to analytics and AI.

Now, with the **Migration assistant (Preview)**, we’re making it even easier to move SQL Server to Fabric with confidence.

## Why migrate to SQL database in Fabric?

SQL database in Fabric is built on three foundational pillars:

- **Simple**: Provision databases in seconds. No complex networking, storage, or configuration decisions. Just name your database and start building.

- **Autonomous and secure**: Serverless architecture that automatically scales compute and storage to meet demand. High availability, disaster recovery, Microsoft Entra authentication, customer-managed keys (CMK), and SQL auditing are all built-in.

- **Optimized for AI**: Native support for vector search (along with all the updates in this space across Microsoft SQL), Retrieval-Augmented Generation (RAG), and seamless integration with Azure OpenAI, Microsoft Foundry etc.

But the real gamechanger? Near real-time replication to OneLake. Your operational data becomes instantly available for analytics, Power BI reports, Spark notebooks, and AI workloads—without moving or duplicating data.

## Migration assistant for SQL database in Fabric

The Migration Assistant is a Fabric-native, wizard-driven experience designed to simplify moving SQL Server–based workloads into Fabric. Whether you’re migrating from on-premises SQL Server or Azure SQL Database, the assistant guides you through every step—from schema import to data copy—with clarity, control, and confidence.

*

Figure: Migration Assistant for SQL Server in Fabric landing page.*

## Key features

- DACPAC-based schema migration – Import your database schema using industry-standard DACPAC files.

- Compatibility assessment – Automatically identifies unsupported objects and compatibility issues before migration.

- Actionable guidance – Get clear, step-by-step recommendations to resolve issues.

- Built-in data copy workflows – Seamlessly move data using Fabric Copy Jobs powered by Data Factory.

- Preserves SQL skills – No need to learn new languages or tools. Use familiar T-SQL, SSMS, and Visual Studio Code.

## How it works: A step-by-step migration journey

**Step 1: Launch the Migration wizard**

From your Fabric workspace, select the “Migrate” button and select “Migrate to SQL database in Fabric”.

**Step 2: Upload DACPAC file**

Export your source database schema as a DACPAC file using SQL Server Management Studio (SSMS), the MSSQL extension for VS Code, or SqlPackage. Upload it to the wizard for validation.

**Step 3: Provision target database**

Name your new SQL database in Fabric. The wizard provisions it in seconds.

**Step 4: Deploy schema with compatibility checks**

The Migration assistant imports your schema and identifies any compatibility issues. For example, if your source database uses a feature that is not supported in SQL database in Fabric, the assistant flags these objects and provides guidance.

Copilot-powered fix suggestions help you resolve issues faster. Accept or reject recommendations interactively.

*

Figure: Steps to migrate using the Migration Assistant.*

**Step 5: Copy Data**

Once schema deployment is complete, launch a Fabric Copy Job to move your data. The assistant integrates with Fabric Data Gateway to securely connect to on-premises sources.

Run pre-deployment scripts (e.g., disable foreign keys) and post-deployment scripts (e.g., re-enable constraints) to ensure a smooth data load.

**Step 6: Validate and go live**

Review migration results in the Migration Assistant panel within the SQL Editor. Fix any remaining issues, validate your data, and you’re ready to go live.

## Coming soon

Ready to simplify your SQL migration journey? We will launch in a few weeks, and it will soon be accessible through the fabric portal. Join the conversation by sharing your feedback on the [Fabric C](https://blog.fabric.microsoft.com/blog/announcing-sql-database-in-fabric-is-now-generally-available-ga)[ommunity Forums](https://blog.fabric.microsoft.com/blog/announcing-sql-database-in-fabric-is-now-generally-available-ga).