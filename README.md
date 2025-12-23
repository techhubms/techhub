# Tech Hub

Welcome to the Tech Hub ([tech.hub.ms](https://tech.hub.ms)) - an automated content aggregation and curation platform for Microsoft and technology sources.

## About This Project

The Tech Hub provides:

- **Automated Content Aggregation**: RSS feed processing with AI-powered categorization
- **Dynamic Sections**: Multiple technology hubs including AI, GitHub Copilot, ML, Azure, Coding, DevOps, and Security
- **Advanced Filtering**: Tag-based and date-based content filtering
- **Multiple Content Types**: News, blogs, videos, community discussions

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
   pwsh /workspaces/techhub/scripts/jekyll-start.ps1
   ```

4. **Access the Site**: Open browser to `http://localhost:4000`

#### Option 2: GitHub Codespaces

1. **Create Codespace**: Go to [GitHub repository](https://github.com/techhubms/techhub) → "Code" → "Codespaces" → "Create codespace on main"
2. **Start the Site**:

   ```powershell
   pwsh /workspaces/techhub/scripts/jekyll-start.ps1
   ```

3. **Access the Site**: Codespace will provide a URL, usually accessible through the "Ports" panel

## Documentation

### Development & AI Agents

For comprehensive development guides, architecture, and AI agent instructions, please refer to **[AGENTS.md](AGENTS.md)**.

This file serves as the primary entry point for understanding the codebase, development standards, and domain-specific guidelines.

### Architecture & Guides

Detailed documentation is available in the **[docs/](docs/)** directory. Key starting points include:

- **[Site Overview](docs/site-overview.md)**: Architecture and structure
- **[Content Management](docs/content-management.md)**: Creating content and RSS processing
- **[Documentation Guidelines](docs/documentation-guidelines.md)**: Structure and hierarchy

## Contributing

We welcome contributions! Please feel free to:

- Submit pull requests
- Report issues
- Suggest improvements
- Fork the repository for your own projects

## Commercial Use & Partnerships

This project is licensed under the MIT License, which allows commercial use. If you're using this project commercially, we'd love to hear from you! Consider:

- **Sponsoring the project** to support ongoing development
- **Partnering with us** for feature development or customization
- **Contributing back** improvements that benefit the community

For commercial partnerships or sponsorship opportunities, please contact [Reinier van Maanen](mailto:reinier.vanmaanen@xebia.com).

## Support

For questions or issues:

1. Check the relevant documentation in the [docs/](docs/) folder
2. Look at existing content files for examples
3. Report problems via GitHub Issues
4. Contact Reinier van Maanen or Rob Bos

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Reinier van Maanen
