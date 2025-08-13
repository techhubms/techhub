# Azure Infrastructure for Tech Hub

This directory contains Bicep templates and deployment scripts for setting up the complete Azure infrastructure for the Tech Hub, including Azure SQL Database and Azure Static Web Apps for Jekyll hosting.

## Files

- **main.bicep** - Main Bicep template defining the Azure infrastructure
- **parameters.template.json** - Template parameters file for development/testing
- **parameters.prod.json** - Production parameters file with enhanced security settings
- **deploy.ps1** - PowerShell deployment script with validation and error handling

## Infrastructure Components

The Bicep template creates the following Azure resources:

### Core Database Resources

- Azure SQL Server - Logical server hosting the database
- Azure SQL Database - Configured for optimal performance with 1K-100K rows
- Backup Configuration - Automated backups with configurable retention

### Static Website Hosting

- Azure Static Web Apps - Hosts the Jekyll static website with GitHub integration
- Custom Domain Support - Optional custom domain configuration
- SSL Certificates - Automatically managed SSL certificates
- Global CDN - Built-in content delivery network

### Security Components

- Azure Key Vault - Secure storage for connection strings and secrets
- Firewall Rules - Configured to allow Azure services
- Advanced Threat Protection - Optional security monitoring (production)
- Audit Logging - Optional compliance and security auditing

### Performance Optimization

- Appropriate SKU Selection - Standard S2/S3 tiers optimized for the expected workload
- Index Strategy - Sample table includes performance indexes
- Connection Pooling - Optimized connection string configuration
- Static Web Apps - Free tier with global CDN for fast content delivery

## Quick Start

### Prerequisites

1. Azure CLI installed and configured

   ```bash
   az login
   az account set --subscription "your-subscription-id"
   ```

2. PowerShell (PowerShell Core 7+ recommended)
3. GitHub repository with required secrets configured

### Required GitHub Secrets

Configure these secrets in your GitHub repository for automated deployment:

- `AZURE_CREDENTIALS`: Service principal credentials (JSON format)
- `AZURE_SUBSCRIPTION_ID`: Your Azure subscription ID  
- `SQL_ADMIN_PASSWORD`: Strong password for SQL Server admin
- `AZURE_STATIC_WEB_APPS_API_TOKEN`: API token from Static Web Apps (auto-generated)

### Deployment Options

#### Option 1: GitHub Actions (Recommended)

1. **Deploy Infrastructure**:
   - Go to Actions tab in GitHub
   - Run "Deploy Infrastructure" workflow
   - Provide required parameters (resource group, location, etc.)

2. **Deploy Jekyll Website**:
   - Push to main branch or create a pull request
   - "Deploy Jekyll to Azure Static Web Apps" workflow runs automatically

#### Option 2: Manual Deployment

1. Customize Parameters

   ```bash
   # Copy template and customize for your environment
   cp parameters.template.json parameters.dev.json
   # Edit parameters.dev.json with your values
   ```

2. Deploy to Development

   ```powershell
   ./deploy.ps1 -ResourceGroupName "rg-myapp-dev" -Environment "dev"
   ```

3. Deploy to Production

   ```powershell
   ./deploy.ps1 -ResourceGroupName "rg-myapp-prod" -Environment "prod" -ParametersFile "./parameters.prod.json"
   ```

### What-If Analysis

Preview changes before deployment:

```powershell
./deploy.ps1 -ResourceGroupName "rg-myapp-dev" -Environment "dev" -WhatIf
```

## Configuration Options

### Static Web Apps Configuration

To enable Static Web Apps deployment:

```json
"enableStaticWebApp": true,
"repositoryUrl": "https://github.com/your-org/your-repo",
"repositoryBranch": "main"
```

Jekyll build configuration (automatically configured):
- **App Location**: `/` (root directory)
- **Output Location**: `_site` (Jekyll build output)  
- **Build Command**: `bundle exec jekyll build --destination _site`

### Database Sizing Options

For 1,000 - 10,000 rows:

```json
"sqlDatabaseTier": "Basic",
"sqlDatabaseSku": "Basic"
```

For 10,000 - 100,000 rows (recommended):

```json
"sqlDatabaseTier": "Standard", 
"sqlDatabaseSku": "S2"
```

For 100,000+ rows with high performance needs:

```json
"sqlDatabaseTier": "Standard",
"sqlDatabaseSku": "S3"
```

### Security Levels

Development Environment:

- Basic firewall rules
- Standard backup retention (7 days)
- Advanced Threat Protection disabled

Production Environment:

- Advanced Threat Protection enabled
- Extended backup retention (35 days)
- Audit logging enabled
- Enhanced monitoring

## Sample Database Schema

The template includes a sample table structure optimized for typical application data:

```sql
CREATE TABLE [dbo].[ApplicationData] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Category] NVARCHAR(100) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Active',
    [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(255) NOT NULL,
    [ModifiedBy] NVARCHAR(255) NOT NULL,
    [Tags] NVARCHAR(500) NULL,
    [AdditionalData] NVARCHAR(MAX) NULL
);
```

Included Features:

- Auto-incrementing primary key
- Audit fields (Created/Modified dates and users)
- Flexible text fields for various data types
- Performance indexes on key fields
- Automatic timestamp updates via triggers

## Security Best Practices

### Connection String Management

- Connection strings are automatically stored in Azure Key Vault
- Use managed identities when possible
- Never commit passwords to source control

### Access Control

- Configure proper firewall rules for your application
- Use Azure Active Directory authentication when possible
- Follow principle of least privilege for database permissions

### Monitoring

- Enable Advanced Threat Protection in production
- Configure audit logging for compliance requirements
- Set up alerts for unusual database activity

## Cost Optimization

### Development Environment

- Use Basic or Standard S0/S1 tiers
- Consider auto-pause for serverless options
- Shorter backup retention periods

### Production Environment

- Monitor DTU/vCore usage and scale as needed
- Use Standard S2/S3 for balanced performance and cost
- Consider Premium tiers only if specific features are required

## Maintenance

### Regular Tasks

- Monitor database performance metrics
- Review and rotate admin passwords quarterly
- Update backup retention policies as needed
- Review firewall rules and access patterns

### Scaling Considerations

- Monitor database size growth
- Plan for index maintenance as data grows
- Consider partitioning strategies for tables exceeding 1M rows

## Troubleshooting

### Common Issues

Authentication Error:

```bash
az login --use-device-code
az account set --subscription "your-subscription-id"
```

Parameter Validation Errors:

- Ensure all placeholder values are replaced in parameters file
- Check password complexity requirements
- Validate subscription ID format

Connection Issues:

- Verify firewall rules include your client IP
- Check if Azure services are allowed through firewall
- Confirm connection string format and credentials

### Getting Help

- Check Azure Resource logs in the portal
- Use `az deployment group show` to view deployment details
- Review SQL Database metrics and diagnostic logs

## Additional Resources

- [Azure SQL Database Documentation](https://docs.microsoft.com/en-us/azure/sql-database/)
- [Bicep Language Reference](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [SQL Database Performance Best Practices](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-performance-guidance)
- [Azure Key Vault Best Practices](https://docs.microsoft.com/en-us/azure/key-vault/general/best-practices)
