---
external_url: https://dellenny.com/creating-custom-microsoft-teams-apps-and-tabs/
title: Creating Custom Microsoft Teams Apps and Tabs
author: Dellenny
feed_name: Dellenny's Blog
date: 2025-09-01 08:33:00 +00:00
tags:
- Angular
- App Manifest
- Authentication
- Azure Active Directory
- C#
- Custom Teams Apps
- Microsoft 365
- Microsoft Teams
- MSAL
- Node.js
- Power Apps
- Power Automate
- React
- Teams
- Teams Tab Development
- Teams Toolkit
- VS Code
- Web Development
- Coding
- Blogs
section_names:
- coding
primary_section: coding
---
Dellenny details how developers can build custom Microsoft Teams apps and tabs, covering low-code options and full-stack development using Teams Toolkit and Azure AD authentication.<!--excerpt_end-->

# Creating Custom Microsoft Teams Apps and Tabs

Microsoft Teams is more than a chat and meeting tool—it's a platform for building and embedding your organization’s unique business applications. This guide walks through the main approaches for extending Teams using custom apps and tabs.

## What Are Teams Apps and Tabs?

- **Teams Apps**: Packages that integrate additional functionality like bots, messaging extensions, or embedded dashboards directly into Teams.
- **Tabs**: Embeddable web experiences within Teams channels, chats, or personal scopes, useful for business dashboards and tailored workflows.

## Why Build Custom Teams Tabs?

- **Centralize access** to core business tools inside Teams.
- **Improve productivity** by reducing app switching.
- **Customize workflows** for specific organizational needs (e.g., approval flows, knowledge bases).
- **Provide consistency** in user experience within Teams.

## Ways to Build Custom Teams Apps

### 1. Low-Code/No-Code with Power Platform

- Use **Power Apps** for simple solutions.
- Integrate with Teams as tabs.
- Automate with **Power Automate** (e.g., approval processes).

### 2. Full Development with Teams Toolkit

- Install the **Teams Toolkit** in Visual Studio Code.
- Scaffold apps in Node.js or C#, using web frameworks like React or Angular for the UI.
- Deploy to Azure or other environments.

### 3. Embed Existing Web Apps

- Wrap a responsive web app in a Teams tab.
- Create an app manifest and point it to your app’s URL.
- Add Teams authentication using Azure AD for security.

## Key Steps to Create a Custom Tab

1. **Set up development environment**: Install Node.js, VS Code, and Teams Toolkit. Register the app with Azure Active Directory for authentication.
2. **Create new Teams app project**: Use the Toolkit to generate boilerplate code, selecting "Tab" as the capability.
3. **Build the UI**: Use React, Angular, or vanilla JavaScript; ensure responsiveness.
4. **Configure the manifest**: Set app name, description, icons, and valid domains; specify tab URLs and scopes.
5. **Test in Teams**: Use ngrok tunneling for local tests; sideload the app into Teams.
6. **Publish and distribute**: Package the .zip file with manifest and icons; upload to the organization’s Teams app store or submit to Microsoft AppSource.

## Best Practices

- Focus the UI on specific user problems.
- Match Teams' accessibility and design standards.
- Use Microsoft Identity (MSAL) for authentication.
- Test the app on desktop, web and mobile.

## Example Use Cases

- **HR Dashboard**: Onboarding and resources tab.
- **Project Tracker**: Kanban board synced with external systems.
- **Sales Dashboard**: Real-time CRM and deal tracking.

Building custom apps for Microsoft Teams helps streamline teamwork by embedding business solutions in the place users already collaborate.

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/creating-custom-microsoft-teams-apps-and-tabs/)
