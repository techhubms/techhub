---
external_url: https://www.youtube.com/watch?v=csueqgXEroM
title: 'ASP.NET Community Standup: Preventing Login Redirects for APIs'
author: dotnet
feed_name: DotNet YouTube
date: 2025-08-25 22:49:37 +00:00
tags:
- .NET
- .NET 10
- API
- API Development
- API Security
- ASP.NET Core
- Authentication
- Authorization
- Daniel Roth
- Demo
- Developer Community
- Developer Tools
- Dotnetdeveloper
- HTTP 401
- Microsoft
- Software Developer
- Stephen Halter
- Web APIs
section_names:
- coding
- security
---
In this ASP.NET Community Standup, Daniel Roth and Stephen Halter discuss the improvements in ASP.NET Core for .NET 10 that eliminate login redirects for API endpoints. Presented by the dotnet team, this session is valuable for developers aiming to streamline API authentication workflows.<!--excerpt_end-->

{% youtube csueqgXEroM %}

# ASP.NET Community Standup: Preventing Login Redirects for APIs

Hosted by Daniel Roth and Stephen Halter, this session explores a significant security enhancement in ASP.NET Core within .NET 10. The update ensures that API endpoints now return the appropriate 401 Unauthorized response rather than redirecting users to a login page when authentication fails. This change brings ASP.NET Core in line with best practices for modern API development, improving compatibility for frontend and client applications that consume these APIs.

**Key Topics:**

- Default behavior for unauthenticated API requests now results in HTTP 401, not HTTP 302 redirects.
- Eliminates confusion and extra handling logic for clients that expect standards-compliant error codes.
- Streamlines the developer experience, making it easier to build robust and secure APIs with ASP.NET Core.
- Discussion includes practical use cases, examples, and community Q&A.

**Speakers:**

- Daniel Roth
- Stephen Halter

**Community Resources:**

- [API Best Practices](https://www.theurlist.com/aspnet-standup-20250902)

**Who should watch:**

- ASP.NET Core developers
- Backend engineers focused on secure API design
- Teams migrating to .NET 10

Stay up to date with the latest .NET and ASP.NET security improvements and join the ongoing community discussions!
