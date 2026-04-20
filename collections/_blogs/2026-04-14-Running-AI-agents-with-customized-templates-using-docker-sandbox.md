---
date: 2026-04-14 10:00:00 +00:00
tags:
- .NET
- .NET Install.sh
- .NET SDK
- Agent User
- AI
- Apt Get
- Blogs
- BuildKit
- Claude Code
- Containerd
- Datadog Dd Trace .NET
- Debian
- DevOps
- Docker
- Docker Compose
- Docker Desktop
- Docker Hub
- Docker Sandboxes
- Dockerfile
- Getting Started
- GitHub CLI
- Installation
- Microvm
- Multi Stage Build
- NO PROXY
- OCI Images
- Reverse Engineering
- Sbx
- Sudoers
- Ubuntu
author: Andrew Lock
title: Running AI agents with customized templates using docker sandbox
section_names:
- devops
- dotnet
feed_name: Andrew Lock's Blog
primary_section: dotnet
external_url: https://andrewlock.net/running-ai-agents-with-customized-templates-in-docker-sandbox/
---

Andrew Lock explains how to build and publish custom Docker Sandbox templates so AI-agent sandboxes start with the tooling you need, including an example that installs the .NET SDK and a more advanced approach that swaps the base image while reapplying the sandbox layering.<!--excerpt_end-->

# Running AI agents with customized templates using docker sandbox

This post follows on directly from a previous post where Andrew Lock describes running AI agents safely using Docker’s sandbox tool, `sbx`. Here he focuses on **creating custom templates** so sandboxes start with extra tools already installed.

## Running agents safely in a Docker sandbox

AI agents can generate an “infuriating number of tool calls” that interrupt work.

![The Claude Code permissions call, asking "Do you want to create test.txt?"](/content/images/2026/claude_code_tool.png)

Using Claude’s **“bypass permissions”** mode (aka YOLO/dangerous mode) can be risky. Docker Sandboxes provide isolation:

- Sandboxes run in **microVMs** isolated from the host.
- Only the allowed **working directory** is accessible.
- Network traffic goes through a **proxy** that can block traffic or inject credentials without exposing them to the agent.

A practical problem: some projects require a lot of tooling (example: `datadog/dd-trace-dotnet`), and reinstalling those tools in every new sandbox is tedious. The solution is to build **custom templates**.

## Creating a custom Claude Code template

Docker’s docs describe custom environments for supported agents:

- `claude-code` — includes a variety of dev tools
- `claude-code-docker` — same as above, plus Docker Engine

> There's also a `claude-code-minimal` template which is similar to `claude-code`, but includes fewer tools, so you don't have npm, python, or golang, for example.

### Requirements

- You need **Docker Desktop** because you’re building an **OCI image** (effectively a Docker image).
- Even though sandboxes run in microVMs (not containers), the template mechanism is OCI-image-based.

### Example: add tooling (installing .NET as an example)

The template images are Ubuntu-based, so package installs use `apt-get`. They also include two users: `root` and `agent`.

- Use `root` for system-level packages.
- Use `agent` for tools that install into the home directory.

```dockerfile
FROM docker/sandbox-templates:claude-code-docker

# Switch to root to run package manager installs (.NET dependencies)
USER root
RUN apt-get update \
 && apt-get install -y --no-install-recommends \
    ca-certificates \
    libc6 \
    libgcc-s1 \
    libgssapi-krb5-2 \
    libicu76 \
    libssl3t64 \
    libstdc++6 \
    tzdata \
    zlib1g

# Most tools should be installed at user-level, using the agent user
USER agent
RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0 --no-path
ENV DOTNET_ROOT=/home/agent/.dotnet \
    PATH=$PATH:/home/agent/.dotnet:/home/agent/.dotnet/tools
```

### Build and publish

You can build with `docker build`, but you **must push** the image to an OCI registry (Docker Hub works). You can’t rely on a local-only build because the sandbox doesn’t share your local Docker image store.

```bash
docker build -t my-org/my-template:v1 --push .
```

Then run with `sbx` using the fully qualified registry reference:

```bash
sbx run -t docker.io/my-org/my-template:v1 claude
```

Notes:

- The template is pulled and cached.
- You must include the registry prefix (e.g., `docker.io/...`), which differs from typical local Docker usage.

> Andrew mentions he published some .NET-focused templates on Docker Hub and links both the tags page and the Dockerfile definitions.

## What if you need to change the base image?

The supported approach is to start from `docker/sandbox-templates` and layer extra tools. At the time of writing, those images are based on Ubuntu 25.10.

If you need an older base image (for example, building/testing the Datadog .NET SDK on older distros to support older `glibc`), swapping base images isn’t officially supported.

Andrew describes reverse-engineering the `docker/sandbox-templates` layering by inspecting image layers on Docker Hub, then recreating those steps on top of an arbitrary base image.

### Reverse-engineered approach (high-level)

The recreated Dockerfile:

- Configures environment variables (including `NO_PROXY`/`no_proxy`)
- Installs core utilities and keyrings
- Creates/configures the `agent` user and sudoers entries
- Sets up `CLAUDE_ENV_FILE` as a persistent environment/session file
- Installs development tools (optionally including npm, golang, python, make, etc.)
- Installs Claude Code

#### Example Dockerfile (condensed from the post)

```dockerfile
FROM dd-trace-dotnet/debian-tester AS base

# Grab stuff from the original sandbox
ENV NPM_CONFIG_PREFIX=/usr/local/share/npm-global
ENV PATH=/home/agent/.local/bin:/usr/local/share/npm-global/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
ENV NO_PROXY=localhost,127.0.0.1,::1,172.17.0.0/16
ENV no_proxy=localhost,127.0.0.1,::1,172.17.0.0/16

WORKDIR /home/agent/workspace
RUN apt-get update \
 && apt-get install -yy --no-install-recommends \
    ca-certificates \
    curl \
    gnupg \
 && install -m 0755 -d /etc/apt/keyrings \
 && curl -fsSL https://download.docker.com/linux/debian/gpg | \
    gpg --dearmor -o /etc/apt/keyrings/docker.gpg \
 && chmod a+r /etc/apt/keyrings/docker.gpg \
 && echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian \
  $(. /etc/os-release && echo \"${UBUNTU_CODENAME:-$VERSION_CODENAME}\") stable" | \
  tee /etc/apt/sources.list.d/docker.list > /dev/null

# Remove base image user
# Create non-root user
# Configure sudoers
# Create sandbox config
# Set up npm global package folder under /usr/local/share
RUN userdel ubuntu || true \
 && useradd --create-home --uid 1000 --shell /bin/bash agent \
 && groupadd -f docker \
 && usermod -aG sudo agent \
 && usermod -aG docker agent \
 && mkdir /etc/sudoers.d \
 && chmod 0755 /etc/sudoers.d \
 && echo "agent ALL=(ALL) NOPASSWD:ALL" > /etc/sudoers.d/agent \
 && echo "Defaults:%sudo env_keep += \"http_proxy https_proxy no_proxy HTTP_PROXY HTTPS_PROXY NO_PROXY SSL_CERT_FILE NODE_EXTRA_CA_CERTS REQUESTS_CA_BUNDLE JAVA_TOOL_OPTIONS\"" > /etc/sudoers.d/proxyconfig \
 && mkdir -p /home/agent/.docker/sandbox/locks \
 && chown -R agent:agent /home/agent \
 && mkdir -p /usr/local/share/npm-global \
 && chown -R agent:agent /usr/local/share/npm-global

RUN touch /etc/sandbox-persistent.sh && chmod 644 /etc/sandbox-persistent.sh && chown agent:agent /etc/sandbox-persistent.sh
ENV BASH_ENV=/etc/sandbox-persistent.sh

# Source the sandbox persistent environment file
# Export BASH_ENV so non-interactive child shells also source the persistent env
RUN echo 'if [ -f /etc/sandbox-persistent.sh ]; then . /etc/sandbox-persistent.sh; fi; export BASH_ENV=/etc/sandbox-persistent.sh' \
 | tee /etc/profile.d/sandbox-persistent.sh /tmp/sandbox-bashrc-prepend /home/agent/.bashrc > /dev/null \
 && chmod 644 /etc/profile.d/sandbox-persistent.sh \
 && cat /tmp/sandbox-bashrc-prepend /etc/bash.bashrc > /tmp/new-bashrc \
 && mv /tmp/new-bashrc /etc/bash.bashrc \
 && chmod 644 /etc/bash.bashrc \
 && rm /tmp/sandbox-bashrc-prepend && chmod 644 /home/agent/.bashrc \
 && chown agent:agent /home/agent/.bashrc

USER root

# Setup Github keys
RUN curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg \
 | tee /etc/apt/keyrings/githubcli-archive-keyring.gpg > /dev/null \
 && chmod a+r /etc/apt/keyrings/githubcli-archive-keyring.gpg \
 && echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" \
 | tee /etc/apt/sources.list.d/github-cli.list > /dev/null

# Install all the tools available in the claude-code-docker image
RUN apt-get update \
 && apt-get install -yy --no-install-recommends \
    dnsutils \
    docker-buildx-plugin \
    docker-ce-cli \
    docker-compose-plugin \
    git \
    jq \
    less \
    lsof \
    make \
    procps \
    psmisc \
    ripgrep \
    rsync \
    socat \
    sudo \
    unzip \
    gh \
    bc \
    default-jdk-headless \
    golang \
    man-db \
    nodejs \
    npm \
    python3 \
    python3-pip \
    containerd.io docker-ce \
 && apt-get clean \
 && rm -rf /var/lib/apt/lists/*

LABEL com.docker.sandboxes.start-docker=true

USER agent

FROM base AS claude

# Install Claude Code
RUN curl -fsSL https://claude.ai/install.sh | bash

ENV CLAUDE_ENV_FILE=/etc/sandbox-persistent.sh
CMD ["claude", "--dangerously-skip-permissions"]
```

### Minimal variant

If you don’t want the extra tools (npm/python/golang), you can instead mirror the `claude-code-minimal` tool set. The tool install step becomes:

```dockerfile
RUN apt-get update \
 && apt-get install -yy --no-install-recommends \
    bubblewrap \
    dnsutils \
    docker-buildx-plugin \
    docker-ce-cli \
    docker-compose-plugin \
    git \
    gh \
    jq \
    less \
    lsof \
    make \
    procps \
    psmisc \
    ripgrep \
    rsync \
    socat \
    sudo \
    unzip \
 && apt-get clean \
 && rm -rf /var/lib/apt/lists/*
```

Build and push as before:

```bash
docker build --tag dd-trace-dotnet/sandbox --push .
```

## Updating the version of Claude Code only

Andrew uses a multi-stage build so updating Claude Code doesn’t require rebuilding the whole image:

```dockerfile
FROM base AS claude

# Install Claude Code
RUN curl -fsSL https://claude.ai/install.sh | bash

ENV CLAUDE_ENV_FILE=/etc/sandbox-persistent.sh
CMD ["claude", "--dangerously-skip-permissions"]
```

Then rebuild only that stage using `--no-cache-filter`:

```bash
docker build --tag dd-trace-dotnet/sandbox --no-cache-filter claude .
```

## Summary

- You can create **custom Docker Sandbox templates** by starting from `docker/sandbox-templates` and layering additional tools.
- Templates must be pushed to an **OCI registry** and referenced with a full registry prefix when used via `sbx`.
- If you need a different base image (e.g., older distros for compatibility testing), you can approximate the sandbox template setup by inspecting image layers and recreating the key configuration steps, though this isn’t officially supported.
- Using a multi-stage build can make it faster to update just the Claude Code layer.


[Read the entire article](https://andrewlock.net/running-ai-agents-with-customized-templates-in-docker-sandbox/)

