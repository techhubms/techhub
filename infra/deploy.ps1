#!/usr/bin/env pwsh

<#
.SYNOPSIS
Deploy Azure SQL Database infrastructure using Bicep templates

.DESCRIPTION
This script deploys the Azure SQL Database infrastructure defined in main.bicep.
It supports both development and production environments with appropriate parameter files.

.PARAMETER ResourceGroupName
The name of the Azure resource group where resources will be deployed

.PARAMETER Environment
The environment to deploy (dev, test, prod)

.PARAMETER Location
Azure region for the resource group (if it needs to be created)

.PARAMETER ParametersFile
Path to the parameters file (defaults to parameters.template.json for dev)

.PARAMETER WhatIf
Show what would be deployed without actually deploying

.PARAMETER Force
Skip confirmation prompts

.EXAMPLE
./deploy.ps1 -ResourceGroupName "rg-tech-hub-ms-dev" -Environment "dev"

.EXAMPLE
./deploy.ps1 -ResourceGroupName "rg-tech-hub-ms-prod" -Environment "prod" -ParametersFile "./parameters.prod.json"

.EXAMPLE
./deploy.ps1 -ResourceGroupName "rg-tech-hub-ms-dev" -Environment "dev" -WhatIf
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ResourceGroupName,
    
    [Parameter(Mandatory = $true)]
    [ValidateSet("dev", "test", "prod")]
    [string]$Environment,
    
    [Parameter(Mandatory = $false)]
    [string]$Location = "westeurope",
    
    [Parameter(Mandatory = $false)]
    [string]$ParametersFile = "",
    
    [Parameter(Mandatory = $false)]
    [switch]$WhatIf,
    
    [Parameter(Mandatory = $false)]
    [switch]$Force
)

# Set error action preference
$ErrorActionPreference = "Stop"

# Get script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Set default parameters file based on environment
if ([string]::IsNullOrEmpty($ParametersFile)) {
    switch ($Environment) {
        "dev" { $ParametersFile = Join-Path $ScriptDir "parameters.template.json" }
        "test" { $ParametersFile = Join-Path $ScriptDir "parameters.template.json" }
        "prod" { $ParametersFile = Join-Path $ScriptDir "parameters.prod.json" }
    }
}

# Paths
$BicepFile = Join-Path $ScriptDir "main.bicep"
$DeploymentName = "sql-db-deployment-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

Write-Host "🚀 Azure SQL Database Deployment Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Resource Group: $ResourceGroupName" -ForegroundColor Green
Write-Host "Environment: $Environment" -ForegroundColor Green
Write-Host "Location: $Location" -ForegroundColor Green
Write-Host "Parameters File: $ParametersFile" -ForegroundColor Green
Write-Host "Bicep File: $BicepFile" -ForegroundColor Green
Write-Host "Deployment Name: $DeploymentName" -ForegroundColor Green
Write-Host ""

# Validate files exist
if (-not (Test-Path $BicepFile)) {
    Write-Error "Bicep file not found: $BicepFile"
    exit 1
}

if (-not (Test-Path $ParametersFile)) {
    Write-Error "Parameters file not found: $ParametersFile"
    Write-Host "💡 Tip: Copy parameters.template.json and customize it for your environment" -ForegroundColor Yellow
    exit 1
}

# Check if Azure CLI is installed and user is logged in
try {
    $null = az account show 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Please log in to Azure CLI first:" -ForegroundColor Red
        Write-Host "   az login" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "❌ Azure CLI is not installed or not in PATH" -ForegroundColor Red
    Write-Host "   Please install Azure CLI: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli" -ForegroundColor Yellow
    exit 1
}

# Get current subscription
$CurrentSubscription = az account show --query "{name:name, id:id}" --output json | ConvertFrom-Json
Write-Host "📋 Current Subscription: $($CurrentSubscription.name) ($($CurrentSubscription.id))" -ForegroundColor Blue

# Check if parameters file contains placeholder values
$ParametersContent = Get-Content $ParametersFile -Raw | ConvertFrom-Json
$PlaceholderValues = @(
    "YOUR_SUBSCRIPTION_ID_HERE",
    "CHANGE_THIS_PASSWORD_123!",
    "REPLACE_WITH_SECURE_PASSWORD"
)

$HasPlaceholders = $false
foreach ($param in $ParametersContent.parameters.PSObject.Properties) {
    if ($param.Value.value -in $PlaceholderValues) {
        Write-Host "⚠️  Parameter '$($param.Name)' contains placeholder value: $($param.Value.value)" -ForegroundColor Yellow
        $HasPlaceholders = $true
    }
}

if ($HasPlaceholders) {
    Write-Host "❌ Please update the parameters file with actual values before deploying" -ForegroundColor Red
    exit 1
}

# Validate password complexity for production
if ($Environment -eq "prod") {
    $SqlPassword = $ParametersContent.parameters.sqlAdminPassword.value
    if ($SqlPassword.Length -lt 12) {
        Write-Host "❌ Production environment requires SQL password to be at least 12 characters" -ForegroundColor Red
        exit 1
    }
}

# Confirmation for production deployments
if ($Environment -eq "prod" -and -not $Force -and -not $WhatIf) {
    Write-Host "⚠️  You are about to deploy to PRODUCTION environment!" -ForegroundColor Yellow
    $Confirm = Read-Host "Are you sure you want to continue? (yes/no)"
    if ($Confirm -ne "yes") {
        Write-Host "❌ Deployment cancelled" -ForegroundColor Red
        exit 0
    }
}

# Check if resource group exists
Write-Host "🔍 Checking if resource group exists..." -ForegroundColor Blue
$ResourceGroupExists = az group exists --name $ResourceGroupName --output tsv

if ($ResourceGroupExists -eq "false") {
    Write-Host "📦 Creating resource group: $ResourceGroupName" -ForegroundColor Green
    
    if (-not $WhatIf) {
        az group create --name $ResourceGroupName --location $Location
        if ($LASTEXITCODE -ne 0) {
            Write-Host "❌ Failed to create resource group" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "   [WHAT-IF] Would create resource group: $ResourceGroupName in $Location" -ForegroundColor Yellow
    }
} else {
    Write-Host "✅ Resource group already exists: $ResourceGroupName" -ForegroundColor Green
}

# Deploy Bicep template
Write-Host "🚀 Starting deployment..." -ForegroundColor Blue

$DeployCommand = @(
    "az", "deployment", "group", "create",
    "--resource-group", $ResourceGroupName,
    "--template-file", $BicepFile,
    "--parameters", "@$ParametersFile",
    "--name", $DeploymentName
)

if ($WhatIf) {
    $DeployCommand += "--what-if"
    Write-Host "   [WHAT-IF] Running deployment validation..." -ForegroundColor Yellow
} else {
    Write-Host "   Deploying infrastructure..." -ForegroundColor Green
}

# Execute deployment
try {
    & $DeployCommand[0] @($DeployCommand[1..($DeployCommand.Length-1)])
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Deployment failed" -ForegroundColor Red
        exit 1
    }
    
    if (-not $WhatIf) {
        Write-Host "✅ Deployment completed successfully!" -ForegroundColor Green
        
        # Get deployment outputs
        Write-Host ""
        Write-Host "📋 Deployment Outputs:" -ForegroundColor Cyan
        $Outputs = az deployment group show --resource-group $ResourceGroupName --name $DeploymentName --query "properties.outputs" --output json | ConvertFrom-Json
        
        if ($Outputs) {
            foreach ($output in $Outputs.PSObject.Properties) {
                Write-Host "   $($output.Name): $($output.Value.value)" -ForegroundColor White
            }
            
            # Show sample table creation script
            if ($Outputs.sampleTableScript) {
                Write-Host ""
                Write-Host "💡 Sample table creation script:" -ForegroundColor Cyan
                Write-Host "   You can run this script in SQL Server Management Studio or Azure Data Studio"
                Write-Host "   to create a sample table structure for your application."
            }
        }
        
        # Create the news articles table if it doesn't exist
        Write-Host ""
        Write-Host "🗄️ Creating news articles table..." -ForegroundColor Blue
        
        try {
            $CreateTableScript = @"
        # Create the articles table if it doesn't exist
        Write-Host ""
        Write-Host "🗄️ Creating articles table..." -ForegroundColor Blue
        
        try {
            $CreateTableScript = @"
-- Create articles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='articles' AND xtype='U')
BEGIN
    CREATE TABLE articles (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        title NVARCHAR(500) NOT NULL,
        description NVARCHAR(MAX),
        author NVARCHAR(200),
        canonical_url NVARCHAR(1000),
        feed_name NVARCHAR(200) NULL,
        feed_url NVARCHAR(1000) NULL,
        date DATETIME2 NOT NULL,
        permalink NVARCHAR(1000),
        categories NVARCHAR(MAX), -- JSON array as string
        tags NVARCHAR(MAX), -- JSON array as string
        tags_normalized NVARCHAR(MAX), -- JSON array as string
        collection NVARCHAR(100) NOT NULL, -- news, videos, community, posts, etc.
        content NVARCHAR(MAX), -- Full article content
        created_at DATETIME2 DEFAULT GETUTCDATE(),
        updated_at DATETIME2 DEFAULT GETUTCDATE()
    );
    
    -- Create indexes for performance
    CREATE INDEX IX_articles_date ON articles (date DESC);
    CREATE INDEX IX_articles_collection ON articles (collection);
    CREATE INDEX IX_articles_created_at ON articles (created_at DESC);
    CREATE INDEX IX_articles_date_collection ON articles (collection, date DESC);
    
    PRINT 'Table articles created successfully with indexes';
END
ELSE
BEGIN
    PRINT 'Table articles already exists';
END

-- Create a view for easier querying with JSON parsing
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_articles_parsed')
BEGIN
    EXEC('
    CREATE VIEW vw_articles_parsed AS
    SELECT 
        id, title, description, author, canonical_url, feed_name, feed_url,
        date, permalink, collection, content, created_at, updated_at,
        JSON_VALUE(categories, ''$[0]'') as primary_category,
        JSON_VALUE(tags, ''$[0]'') as primary_tag,
        (SELECT COUNT(*) FROM OPENJSON(categories)) as category_count,
        (SELECT COUNT(*) FROM OPENJSON(tags)) as tag_count,
        CASE collection
            WHEN ''videos'' THEN ''internal''
            WHEN ''magazines'' THEN ''internal''
            ELSE ''external''
        END as viewing_mode
    FROM articles;
    ');
    PRINT 'View vw_articles_parsed created successfully';
END

-- Create stored procedure for inserting articles
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_InsertArticle')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_InsertArticle
        @title NVARCHAR(500),
        @description NVARCHAR(MAX) = NULL,
        @author NVARCHAR(200) = NULL,
        @canonical_url NVARCHAR(1000) = NULL,
        @feed_name NVARCHAR(200) = NULL,
        @feed_url NVARCHAR(1000) = NULL,
        @date DATETIME2,
        @permalink NVARCHAR(1000) = NULL,
        @categories NVARCHAR(MAX) = NULL,
        @tags NVARCHAR(MAX) = NULL,
        @tags_normalized NVARCHAR(MAX) = NULL,
        @collection NVARCHAR(100),
        @content NVARCHAR(MAX) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        IF EXISTS (SELECT 1 FROM articles WHERE canonical_url = @canonical_url AND collection = @collection AND @canonical_url IS NOT NULL)
        BEGIN
            UPDATE articles 
            SET title = @title, description = @description, author = @author,
                feed_name = @feed_name, feed_url = @feed_url, date = @date,
                permalink = @permalink, categories = @categories, tags = @tags,
                tags_normalized = @tags_normalized, content = @content, updated_at = GETUTCDATE()
            WHERE canonical_url = @canonical_url AND collection = @collection;
            
            SELECT id as inserted_id FROM articles WHERE canonical_url = @canonical_url AND collection = @collection;
        END
        ELSE
        BEGIN
            INSERT INTO articles (title, description, author, canonical_url, feed_name, feed_url, 
                                date, permalink, categories, tags, tags_normalized, collection, content)
            VALUES (@title, @description, @author, @canonical_url, @feed_name, @feed_url,
                   @date, @permalink, @categories, @tags, @tags_normalized, @collection, @content);
            
            SELECT SCOPE_IDENTITY() as inserted_id;
        END
    END
    ');
    PRINT 'Stored procedure sp_InsertArticle created successfully';
END
"@

            # Get connection details from deployment outputs
            if ($Outputs.serverName -and $Outputs.databaseName) {
                $ServerName = $Outputs.serverName.value
                $DatabaseName = $Outputs.databaseName.value
                
                Write-Host "   Server: $ServerName" -ForegroundColor White
                Write-Host "   Database: $DatabaseName" -ForegroundColor White
                
                # Save the script to a temporary file
                $ScriptPath = Join-Path $env:TEMP "create_articles_table.sql"
                $CreateTableScript | Out-File -FilePath $ScriptPath -Encoding UTF8
                
                Write-Host "   SQL script saved to: $ScriptPath" -ForegroundColor Yellow
                Write-Host "   💡 To create the table, run:" -ForegroundColor Cyan
                Write-Host "      sqlcmd -S $ServerName -d $DatabaseName -i `"$ScriptPath`"" -ForegroundColor Yellow
                Write-Host "   Or connect using Azure Data Studio/SSMS and execute the script manually" -ForegroundColor Yellow
                
            } else {
                Write-Host "   ⚠️  Could not retrieve database connection details from deployment" -ForegroundColor Yellow
                Write-Host "   Please create the table manually using the script below:" -ForegroundColor Yellow
                Write-Host $CreateTableScript -ForegroundColor Gray
            }
            
        } catch {
            Write-Host "   ⚠️  Error preparing table creation script: $_" -ForegroundColor Yellow
            Write-Host "   You can create the table manually after connecting to the database" -ForegroundColor Yellow
        }

        Write-Host ""
        Write-Host "🎉 Next steps:" -ForegroundColor Cyan
        Write-Host "   1. Connect to your SQL Database using the connection details above"
        Write-Host "   2. Run the table creation script to set up the articles table"
        Write-Host "   3. Configure your application connection string"
        Write-Host "   4. Set up proper access policies and firewall rules as needed"
        Write-Host "   5. Consider enabling full-text search for better content search capabilities"
        Write-Host "   6. Use the sp_InsertArticle stored procedure to add content from all collections"-- Create a view for easier querying with JSON parsing
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_news_articles_parsed')
BEGIN
    EXEC('
    CREATE VIEW vw_news_articles_parsed AS
    SELECT 
        id,
        layout,
        title,
        description,
        author,
        excerpt_separator,
        canonical_url,
        viewing_mode,
        feed_name,
        feed_url,
        date,
        permalink,
        collection,
        content,
        created_at,
        updated_at,
        -- Parse JSON arrays for easier querying
        JSON_VALUE(categories, ''$[0]'') as primary_category,
        JSON_VALUE(tags, ''$[0]'') as primary_tag,
        -- Count of tags and categories
        (SELECT COUNT(*) FROM OPENJSON(categories)) as category_count,
        (SELECT COUNT(*) FROM OPENJSON(tags)) as tag_count
    FROM news_articles;
    ');
    PRINT 'View vw_news_articles_parsed created successfully';
END

-- Create stored procedure for inserting articles
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_InsertNewsArticle')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_InsertNewsArticle
        @layout NVARCHAR(50) = NULL,
        @title NVARCHAR(500),
        @description NVARCHAR(MAX) = NULL,
        @author NVARCHAR(200) = NULL,
        @excerpt_separator NVARCHAR(50) = NULL,
        @canonical_url NVARCHAR(1000) = NULL,
        @viewing_mode NVARCHAR(50) = NULL,
        @feed_name NVARCHAR(200) = NULL,
        @feed_url NVARCHAR(1000) = NULL,
        @date DATETIME2,
        @permalink NVARCHAR(1000) = NULL,
        @categories NVARCHAR(MAX) = NULL,
        @tags NVARCHAR(MAX) = NULL,
        @tags_normalized NVARCHAR(MAX) = NULL,
        @collection NVARCHAR(100) = ''news'',
        @content NVARCHAR(MAX) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        INSERT INTO news_articles (
            layout, title, description, author, excerpt_separator,
            canonical_url, viewing_mode, feed_name, feed_url, date,
            permalink, categories, tags, tags_normalized, collection, content
        )
        VALUES (
            @layout, @title, @description, @author, @excerpt_separator,
            @canonical_url, @viewing_mode, @feed_name, @feed_url, @date,
            @permalink, @categories, @tags, @tags_normalized, @collection, @content
        );
        
        SELECT SCOPE_IDENTITY() as inserted_id;
    END
    ');
    PRINT 'Stored procedure sp_InsertNewsArticle created successfully';
END
"@

            # Get connection details from deployment outputs
            if ($Outputs.serverName -and $Outputs.databaseName) {
                $ServerName = $Outputs.serverName.value
                $DatabaseName = $Outputs.databaseName.value
                
                Write-Host "   Server: $ServerName" -ForegroundColor White
                Write-Host "   Database: $DatabaseName" -ForegroundColor White
                
                # Save the script to a temporary file
                $ScriptPath = Join-Path $env:TEMP "create_news_table.sql"
                $CreateTableScript | Out-File -FilePath $ScriptPath -Encoding UTF8
                
                Write-Host "   SQL script saved to: $ScriptPath" -ForegroundColor Yellow
                Write-Host "   💡 To create the table, run:" -ForegroundColor Cyan
                Write-Host "      sqlcmd -S $ServerName -d $DatabaseName -i `"$ScriptPath`"" -ForegroundColor Yellow
                Write-Host "   Or connect using Azure Data Studio/SSMS and execute the script manually" -ForegroundColor Yellow
                
            } else {
                Write-Host "   ⚠️  Could not retrieve database connection details from deployment" -ForegroundColor Yellow
                Write-Host "   Please create the table manually using the script below:" -ForegroundColor Yellow
                Write-Host $CreateTableScript -ForegroundColor Gray
            }
            
        } catch {
            Write-Host "   ⚠️  Error preparing table creation script: $_" -ForegroundColor Yellow
            Write-Host "   You can create the table manually after connecting to the database" -ForegroundColor Yellow
        }

        Write-Host ""
        Write-Host "🎉 Next steps:" -ForegroundColor Cyan
        Write-Host "   1. Connect to your SQL Database using the connection details above"
        Write-Host "   2. Run the table creation script to set up the news_articles table"
        Write-Host "   3. Configure your application connection string"
        Write-Host "   4. Set up proper access policies and firewall rules as needed"
        Write-Host "   5. Consider enabling full-text search for better content search capabilities"
        
    } else {
        Write-Host "✅ What-if analysis completed" -ForegroundColor Green
    }
    
} catch {
    Write-Host "❌ Error during deployment: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🏁 Script completed successfully!" -ForegroundColor Green
