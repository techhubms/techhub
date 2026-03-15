---
external_url: https://www.devclass.com/development/2025/06/30/microsoft-to-finally-expunge-the-azure-ad-graph-api/101141
title: 'Microsoft to Finally Retire Azure AD Graph API: Migration Paths and Implications'
author: DevClass.com
primary_section: azure
feed_name: DevClass
date: 2025-06-30 16:45:40 +00:00
tags:
- .NET
- ADAL
- Application Modernization
- Authentication
- Authorization
- Azure
- Azure AD Graph API
- Blogs
- Cloud Security
- Entra Admin Center
- Identity Management
- Microsoft Entra ID
- Microsoft Graph API
- Migration
- MSAL
- PowerShell
- SDK Deprecation
- Security
section_names:
- azure
- security
---
DevClass.com reports on Microsoft's September 2025 deadline for retiring the Azure AD Graph API, discussing technical impacts, migration steps, and security considerations for developers and IT administrators.<!--excerpt_end-->

# Microsoft to Finally Retire Azure AD Graph API: Migration Paths and Implications

Microsoft has announced that the Azure AD Graph API will be removed beginning in early September 2025, as confirmed in an official post. This marks the completion of a multi-year transition process away from the legacy API toward Microsoft Graph API and the Microsoft Authentication Library (MSAL), both of which provide broader features and support for newer identity scenarios—including both Entra ID and personal Microsoft accounts.

## Background

Azure AD, now branded as Microsoft Entra ID, is a core directory behind Microsoft 365 and other cloud-based solutions. Many custom and partner applications have historically used the Azure AD Graph API and the Azure AD Authentication Library (ADAL) for user authentication and authorization.

Since 2020, Microsoft has indicated that the Azure AD Graph API and ADAL would be "feature-frozen" and replaced by Microsoft Graph API and MSAL. Support has been repeatedly extended due to customer feedback and migration complexity. Microsoft now confirms that after September 2025, affected applications will cease to function. There will also be one or two temporary outage tests before the final retirement.

## Developer Impact

- Applications—both first- and third-party—that rely on Azure AD Graph API or ADAL (including PowerShell modules, Microsoft.Azure.KeyVault, and Microsoft.Azure.Management.Automation)—will need to migrate to supported libraries.
- Older SDKs for platforms such as .NET, JavaScript, Java, Python, Android, and iOS are impacted, as they may depend on deprecated services.
- Organizations must identify which applications use the soon-to-be-retired APIs. Microsoft recommends using the Entra admin center's recommendations and the sign-ins workbook to track API/library usage.

## Migration Steps

1. **Audit Application Dependencies**: Use Entra admin center tools to list applications still tied to ADAL or Azure AD Graph APIs.
2. **Update Libraries and SDKs**: Migrate authentication code to MSAL and API requests to Microsoft Graph API.
3. **Leverage Migration Guides and Samples**: Microsoft offers sample code, e.g., for ASP.NET Core web apps using MSAL, to ease modernization.
4. **Plan for Outage Tests**: Microsoft will run temporary outages before final shutdown; organizations should use these to validate all dependencies are updated.
5. **Be Aware of Premium Feature Limitations**: Some advanced features require Entra ID premium licenses.

## Security and Identity Management Implications

Migrating to the new APIs not only avoids service interruption but also improves security posture, given that Microsoft Graph and MSAL are regularly updated with enhanced security measures and broader support. Centralized management with Entra ID provides robust options for access control and compliance.

## References and Resources

- [Official MS announcement on Azure AD Graph API retirement](https://techcommunity.microsoft.com/blog/microsoft-entra-blog/important-update-azure-ad-graph-retirement/4364990)
- [Entra Recommendations on API usage](https://learn.microsoft.com/en-us/entra/identity/monitoring-health/recommendation-migrate-to-microsoft-graph-api)
- [Sign-ins workbook for application/library usage](https://learn.microsoft.com/en-gb/entra/identity-platform/howto-get-list-of-all-auth-library-apps)
- [Sample code for MSAL migration](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2)

## Conclusion

By September 2025, all custom applications must be updated to use Microsoft Graph API and MSAL or risk complete service interruption. Starting audits and code updates now, and utilizing Microsoft's recommended assessment tools, is essential for a smooth transition.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/development/2025/06/30/microsoft-to-finally-expunge-the-azure-ad-graph-api/101141)
