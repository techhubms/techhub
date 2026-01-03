# Tech Hub

Welcome to the Tech Hub ([tech.hub.ms](https://tech.hub.ms)) - an automated content aggregation and curation platform for Microsoft and technology sources.

## About This Project

The Tech Hub provides:

- **Automated Content Aggregation**: RSS feed processing with AI-powered categorization
- **Dynamic Sections**: Configuration-driven hubs defined in `_data/sections.json` (e.g., AI, GitHub Copilot, Azure, .NET)
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
   code /path/to/repo
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

**Custom Agents**: Use the `@fullstack` agent for all Jekyll, Liquid templating, Ruby plugins, JavaScript, PowerShell, and testing work. See [.github/agents/fullstack.md](.github/agents/fullstack.md) for details.

### Functional Documentation

Framework-agnostic functional documentation is available in the **[docs/](docs/)** directory:

- **[Filtering System](docs/filtering-system.md)**: Tag and date filtering logic
- **[Content Management](docs/content-management.md)**: Creating content and RSS processing
- **[Documentation Guidelines](docs/AGENTS.md)**: Documentation structure and maintenance

### Content Writing

For content creation and writing guidelines:

- **[Collections Guide](collections/AGENTS.md)**: Content management overview
- **[Markdown Guidelines](collections/markdown-guidelines.md)**: Formatting and structure standards
- **[Writing Style Guide](collections/writing-style-guidelines.md)**: Tone and style standards

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

1. Check the [AGENTS.md](AGENTS.md) file for development guidance
2. Review the [docs/](docs/) folder for functional documentation
3. Look at existing content files for examples
4. Report problems via GitHub Issues
5. Contact Reinier van Maanen or Rob Bos

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Reinier van Maanen
