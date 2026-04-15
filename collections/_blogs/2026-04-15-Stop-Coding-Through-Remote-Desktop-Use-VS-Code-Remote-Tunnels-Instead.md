---
primary_section: devops
section_names:
- devops
title: 'Stop Coding Through Remote Desktop: Use VS Code Remote Tunnels Instead'
tags:
- '  No Sleep'
- Blogs
- Code Tunnel
- Code Tunnel Service Install
- Command Palette
- Customer VM
- Developer Experience
- DevOps
- Firewall Restrictions
- GitHub Authentication
- Microsoft Dev Tunnels
- Outbound Connections
- Productivity
- Programming
- Remote   Tunnels Extension
- Remote Desktop
- Remote Development
- Remote Explorer
- Secure Tunneling
- Service Mode
- Tooling
- VS Code
- VS Code Remote Tunnels
- VS Code Server
- Vscode.dev
author: Emanuele Bartolesi
external_url: https://dev.to/playfulprogramming/stop-coding-through-remote-desktop-use-vs-code-remote-tunnels-instead-37oc
feed_name: Emanuele Bartolesi's Blog
date: 2026-04-15 07:10:40 +00:00
---

Emanuele Bartolesi explains why Remote Desktop is a poor fit for day-to-day development on customer VMs, and shows how VS Code Remote Tunnels restores a normal local-editor workflow while keeping code and execution on the remote machine.<!--excerpt_end-->

# Stop Coding Through Remote Desktop: Use VS Code Remote Tunnels Instead

There’s a pattern that shows up often in customer environments:

- You get access to a VM for development.
- The code stays on that VM.
- Dependencies are already installed.
- The VM has the right internal network access.

On paper, it sounds fine—until you’re expected to do all development via Remote Desktop.

## The real-world scenario

Remote Desktop is tolerable for admin work, but it’s a poor experience for actual coding:

- Editor feels disconnected from your normal workflow
- Shortcuts feel wrong
- Terminal usage is awkward
- Moving between tools is slower than it should be

In this specific scenario, there was another constraint: the author couldn’t sign into VS Code on the customer VM with their own account, so they couldn’t use a normal setup or synced configuration.

VS Code Remote Tunnels is positioned as the better solution here: keep work and execution inside the customer VM, but get the editor experience back on your local machine.

## What VS Code Remote Tunnels actually does

VS Code Remote Tunnels lets you connect to a remote machine from VS Code **without setting up SSH access**.

The model:

- Start a secure tunnel from the remote machine.
- Connect to that machine from a VS Code client elsewhere.

Microsoft describes it as connecting to a remote machine (desktop or VM) through a secure tunnel using **Microsoft dev tunnels**.

What changes with this approach:

- Code stays on the remote VM
- Terminals run on the remote VM
- Debugging happens against the remote VM
- VS Code installs server-side components on the remote machine
- Your local VS Code becomes the client for that remote environment

### Why “no SSH required” matters

In customer environments, SSH is often blocked, restricted, or not part of the approved setup. Remote Tunnels provides a different path while still keeping work inside the target machine.

### How you can connect

Two main options are described:

- Local desktop VS Code using the **Remote - Tunnels** extension
- **VS Code for the Web** via browser (useful for locked-down devices, but desktop is usually better)

### Key limitation (important)

- The VS Code Server behind this model is designed for a **single user**, not a shared multi-user development service.
- Microsoft states that **hosting the VS Code Server as a service is not allowed**.

So this is a personal remote development workflow, not a team-hosted IDE platform.

## Why it fits this customer use case

This setup fits when:

- The customer wants work to stay inside their environment (code, dependencies, network access).
- Moving work locally would create friction (approvals, additional risk, extra setup).
- Remote Desktop is the main alternative—and it’s a bad trade-off for developer productivity.

## How to set it up

### 1) Download VS Code Server

You can download from the official VS Code download page:

- https://code.visualstudio.com/Download

Select your OS and CPU family. (The article notes you can install VS Code with UI, but that doesn’t make sense if your goal is server-only.)

![VS Code download page showing OS and CPU options](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fw3offlfi335g79fsr34n.png)

### 2) Start the tunnel

Run on the customer VM:

```shell
code tunnel
```

What happens:

- VS Code downloads and starts VS Code Server on the machine.
- It creates the tunnel for remote access.
- First run prompts you to accept server license terms (unless you pass a CLI flag to accept them up front).

![CLI output showing tunnel setup](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fikffgyfveu46m9vighps.png)

### 3) Authenticate and connect

After starting the tunnel, the CLI provides a `vscode.dev` URL tied to the remote machine.

When you open it the first time from a client, you authenticate with GitHub. The article describes this as how VS Code checks both sides are tied to the same account and that you’re allowed to access the remote machine.

![Browser preview of remote tunnel connection](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F9ktcf3ckbdzikmorj01k.png)

### Two practical ways to connect

1) **Browser (fastest to prove it works)**

- Open the generated `vscode.dev` link.

2) **Local desktop VS Code (usually the best experience)**

- Install the Remote - Tunnels extension:
  - https://marketplace.visualstudio.com/items?itemName=ms-vscode.remote-server
- Open the Command Palette and run:
  - **Remote Tunnels: Connect to Tunnel**
- Or use **Remote Explorer** to see available machines.

![Remote - Tunnels extension shown in VS Code](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fy34nyhzwxunb9fou0ar4.png)

### The basic flow

1. On the customer VM, run `code tunnel`
2. Authenticate when prompted
3. Open the generated `vscode.dev` URL, or connect from local VS Code
4. Work on the remote codebase from your own editor

### Operational details to know

- The tunnel is reachable only while VS Code is running on the remote machine, unless you configure it to keep running.
- Microsoft documents service mode using:

```shell
code tunnel service install
```

- There’s also a `--no-sleep` option to help prevent the remote machine from going to sleep.

The article also notes this approach typically doesn’t require inbound listeners on the VM:

- Hosting/connecting uses **authenticated outbound connections** to the Azure-hosted tunneling service.
- No firewall changes are generally necessary.

## Trade-offs and limits

Remote Tunnels is useful, but it’s not “magic.” The article highlights constraints:

1) **Trust / policy model**

- You authenticate with GitHub or Microsoft.
- The tunnel is brokered through Microsoft’s dev tunnel service.
- Some customer environments may reject this on policy, even if it works technically.

2) **Extension support (especially in the browser)**

- Desktop VS Code is usually the best experience.
- Browser support varies; extensions may run locally or remotely depending on extension type and client.

3) **Not a shared remote IDE platform**

- VS Code Server is intended for a single user.
- Hosting it as a service is not allowed.

4) **Persistence / reliability**

- If the remote machine sleeps/shuts down, or the tunnel service isn’t kept running, the session is gone until it’s back.

## Appendix: GitHub Copilot quota visibility in VS Code (author plug)

The article ends with a small promotional section for a VS Code extension called **Copilot Insights** that shows GitHub Copilot plan and quota status inside VS Code.

Marketplace link:

- https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights

![Screenshot promoting Copilot Insights extension for Copilot quota visibility](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)


[Read the entire article](https://dev.to/playfulprogramming/stop-coding-through-remote-desktop-use-vs-code-remote-tunnels-instead-37oc)

