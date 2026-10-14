# Technology Stack

## Runtime & Core Frameworks

- .NET 10 (SDK pinned in `global.json`)
- C# (`LangVersion=latest`, so C# 14 with the .NET 10 SDK) with file-scoped namespaces and nullable reference types
- ASP.NET Core Minimal APIs with dependency injection
- Blazor InteractiveServer with prerendering
- .NET Aspire for orchestration and telemetry

## Data & Content

- **PostgreSQL**: Primary data store (see [database.md](database.md))
- **Npgsql** and **Dapper**: Database access
- **Markdig**: Markdown rendering
- **HtmlSanitizer**: Sanitizes rendered markdown HTML (see [input-validation-and-sanitization.md](input-validation-and-sanitization.md))
- **YamlDotNet**: YAML parsing
- **Handlebars.Net**: Newsletter email templates
- **SmartReader**: Article extraction from web pages
- **Polly** and `Microsoft.Extensions.Http.Resilience`: HTTP client resilience

## API & Integration

- **Swashbuckle** and `Microsoft.AspNetCore.OpenApi`: OpenAPI document and Swagger UI
- **ModelContextProtocol.AspNetCore**: MCP server at `/mcp`
- **Microsoft.Identity.Web**: Entra ID authentication for the admin area
- **Azure.Communication.Email**: Newsletter email delivery
- **Azure AI Foundry** (OpenAI-compatible chat completions): AI categorization of content

## Observability

- **OpenTelemetry** with the Azure Monitor exporter (Application Insights) and the OTLP exporter (Aspire Dashboard). See [telemetry.md](telemetry.md)

## Frontend

- Semantic HTML5 & Modern CSS3
- Vanilla JavaScript (ES2024+)
- No unified JS framework (pure web standards)
- Highlight.js and Mermaid, loaded from CDNs on demand

## Infrastructure

- Azure App Service
- Bicep IaC
- GitHub Actions CI/CD

## Testing

- **xUnit v3**: Unit and integration test framework (assembly fixtures, `ValueTask`-based lifecycle)
- **Moq**: Mocking framework
- **FluentAssertions**: Assertion library
- **Microsoft.AspNetCore.Mvc.Testing**: API integration testing
- **Testcontainers**: Real PostgreSQL for integration and repository tests
- **bUnit**: Blazor component testing
- **Playwright .NET**: End-to-end testing
- **Vitest** with **jsdom**: JavaScript unit testing
- **Pester v5**: PowerShell script testing

## Scripting

- PowerShell 7+ (cross-platform)
- Az PowerShell modules where scripts need them (for example `Az.PostgreSql` in the CD pipeline)
- Azure CLI for most infrastructure and deployment scripts
- Content processing and roundup generation run as C# background services, not PowerShell
