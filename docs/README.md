# Tech Hub Documentation

Welcome to the Tech Hub documentation. This site ([tech.hub.ms](https://tech.hub.ms)) automatically aggregates and curates content from various Microsoft and technology sources.

## For AI Agents

**CRITICAL**: If you're an AI agent working on this repository, read [`.github/copilot-instructions.md`](../.github/copilot-instructions.md) first for essential development instructions.
**CRITICAL**: Paths in the documentation are sometimes relative, like `.github/` or `../.github` or absolute like `/workspaces/techhub/.github`. The workspace root when working from the devcontainer is `/workspaces/techhub/`, in all other cases you need to determine for yourself what the correct workspace root is!

## About This Site

The Tech Hub provides:

- **Automated Content Aggregation**: RSS feed processing with AI-powered categorization
- **Dynamic Sections**: Multiple technology hubs including AI, GitHub Copilot, Data, Azure, Coding, DevOps, and Security
- **Advanced Filtering**: Tag-based and date-based content filtering
- **Multiple Content Types**: News, blogs, videos, community discussions

## Support

For questions or issues:

1. Check the relevant documentation above
2. Look at existing content files for examples
3. Report problems via GitHub Issues
4. Contact Reinier van Maanen or Rob Bos

## Quick Start

Get the Tech Hub running in just a few minutes using containers.

### Prerequisites

#### Dev Container (Recommended)

- Docker: For running the dev container
- VS Code: With the Dev Containers extension
- Git: For version control

#### GitHub Codespaces

- GitHub Account: With access to the repository

### Setup Options

#### Option 1: Dev Container (Recommended)

1. **Open in VS Code**:

   ```bash
   code /path/to/tech
   ```

2. **Reopen in Container**:

   - VS Code will prompt to reopen in dev container
   - Or use Command Palette: "Dev Containers: Reopen in Container"

3. **Start the Site**:

   ```powershell
   pwsh /workspaces/techhub/jekyll-start.ps1
   ```

4. **Access the Site**: Open browser to `http://localhost:4000`

#### Option 2: GitHub Codespaces

1. **Create Codespace**: Go to [GitHub repository](https://github.com/techhubms/techhub) → "Code" → "Codespaces" → "Create codespace on main"
2. **Start the Site**:

   ```powershell
   pwsh /workspaces/techhub/jekyll-start.ps1
   ```

3. **Access the Site**: Codespace will provide a URL, usually accessible through the "Ports" panel

### Read the documentation to find out more

- If you just want to add content, read [content-management.md](content-management.md)
- For complete documentation organization and structure, see [documentation-guidelines.md](documentation-guidelines.md)
