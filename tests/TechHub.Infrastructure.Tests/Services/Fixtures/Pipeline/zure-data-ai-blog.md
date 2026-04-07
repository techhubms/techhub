*This insight has been co-authored by our Data Scientist Erika Patrikainen and Data Consultant Heidi Hämäläinen.*

In our [previous blog](https://zure.com/blog/purview-data-governance-why-it-feels-hard-and-why-its-worth-it?hsLang=en), we set the ground for why data governance matters, how Purview can provide a more holistic view of your data estate, and explained the role of different types of metadata in the process.

In general, metadata management in Fabric has been fragmented, and continues to be that way in the Fabric and Power BI user interfaces. However, the latest Fabric features and AI tools do help with large-scale metadata management:

**OneLake Catalog Search API (Generally Available)**

Fabric now offers a GA-level search API for the OneLake Catalog, giving users deeper visibility into datasets, domains, and access patterns. This reduces governance blind spots and enables enterprise-level reporting and oversight. A single search request can locate matching items across your accessible estate based on catalog metadata and the user’s permissions.

**Workspace Tags (Generally Available)**

Stewards can tag workspaces with domain, owner, or classification metadata—improving catalog capabilities and aligning assets with business domains.

**Bulk Import & Export of Item Definitions (Preview)**

New APIs enable stewards to manage metadata definitions at scale. This reduces manual curation effort and supports updates to business terms, data assets, and glossary definitions in reports.

**Remote MCP Server for AI Agents**

Engineers can run AI agents *inside* Fabric with governed access to metadata—ensuring copilots follow security and governance rules automatically.

For metadata consumers, like report users, Purview Unified Catalog provides a data lineage view at a more granular level compared to Fabric. Data engineers, however, need a more efficient way to check the impact of their work. For example, answering a question like “Which reports need to be modified if these column values change?” might require a lot of clicks. Purview helps, but enabling Fabric Data Agents can take the user experience to the next level.

Compliance and Security teams also face the challenge of tracking where the data flows and how it is shared. We blogged earlier about [Microsoft Purview Data Security Posture Management (DSPM)](https://zure.com/blog/data-security-posture-management-for-ai?hsLang=en), a solution that focuses on data protection in a landscape with traditional and AI applications. The capabilities have now been extended with direct actions against risk behavior and enhanced monitoring of data usage in Fabric.

**Purview Data Loss Prevention (DLP) Policies for Fabric (Preview)**

The update extends DLP capabilities to structured data stored in OneLake, enabling organizations to automatically enforce restrictions and mitigate insider risk or accidental data leaks.

**Quick Policy for Data Theft, Purview Insider Risk Management (General Availability)**

A streamlined “quick policy” for data theft detection and prevention is now generally available—supporting rapid response to insider threats without manual rule creation. In addition, compliance and security teams can now access more granular risk reporting tied to Fabric usage.

**Purview DSPM for AI – Fabric Copilots and Data Agents (Preview)**

Fabric integrates with Purview’s Data Security Posture Management (DSPM) for AI, enabling leaders to monitor how AI agents and copilots use data—and to ensure usage complies with policy.

Modern data governance requires automation, but that is not the starting point. What we at Zure focus on with our customers are the prerequisites: strategies, policies, roles, and the value they drive. While AI agents and copilots become embedded in analytics workflows, the governance model must ensure that innovation does not come at the expense of security or compliance.