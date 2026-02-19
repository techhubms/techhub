function Get-FilteredTags {
    <#
    .SYNOPSIS
        Filters and enhances tags
    .DESCRIPTION
        This function processes tags by:
        1. Removing deprecated section/collection names (Cloud, Machine Learning, etc.)
        2. Normalizing tag format (custom mappings + word mappings)
        3. Removing common filter words
        4. Removing duplicates and empty entries
        5. Returns array of processed tags
        
        NOTE: Current section/collection tags (AI, Azure, Blogs, etc.) are preserved
        if they appear in the input tags. This allows AI to assign them as content tags.
        Only deprecated tags are removed (Cloud, Machine Learning, Coding, etc.).
    .PARAMETER Tags
        Array of tags to process
    .PARAMETER Categories  
        Array of categories/sections
    .PARAMETER Collection
        Collection name
    #>
    param(
        [string[]]$Tags,
        [string[]]$Categories = @(),
        [string]$Collection = ""
    )

    # Validate parameters
    if ($null -eq $Tags) {
        throw "Tags parameter cannot be null"
    }
    
    if ($Tags.Count -eq 0) {
        throw "Tags parameter cannot be empty"
    }
    
    if ($null -eq $Categories) {
        throw "Categories parameter cannot be null"
    }
    
    if ($Categories.Count -eq 0) {
        throw "Categories parameter cannot be empty"
    }
    
    if ($null -eq $Collection -or [string]::IsNullOrWhiteSpace($Collection)) {
        throw "Collection parameter cannot be null or empty"
    }

    # Add the current categories and collection
    $allTags = @($Tags) + @($Categories) + @($Collection)

    # Filter out tags that only contain special characters (no alphanumeric characters)
    $allTags = $allTags | Where-Object { $_ -match '[a-zA-Z0-9]' }

    # Replace dashes and underscores with spaces in all tags
    $allTags = $allTags | ForEach-Object { $_ -replace '[-_]', ' ' }

    # Split tags on semicolons and commas, then flatten the array
    $expandedTags = @()
    foreach ($tag in $allTags) {
        if ($tag -match '[;,]') {
            # Split on both semicolons and commas, trim whitespace, and filter out empty entries
            $splitTags = $tag -split '[;,]' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' }
            $expandedTags += $splitTags
        }
        else {
            $expandedTags += $tag
        }
    }
    $allTags = $expandedTags

    $tagMappings = @{
        '.net'                   = '.NET'
        'dotnet'                 = '.NET'
        'dotnetcli'              = '.NET CLI'
        'a2a'                    = 'A2A'
        'ad'                     = 'AD'
        'ai'                     = 'AI'
        'artificialintelligence' = 'AI'
        'aiagent'                = 'AI Agent'
        'aiagents'               = 'AI Agents'
        'aks'                    = 'AKS'
        'azurekubernetesservice' = 'AKS'
        'azurek8s'               = 'AKS'
        'api'                    = 'API'
        'applicationinsights'    = 'Application Insights'
        'appinsights'            = 'Application Insights'
        'arm'                    = 'ARM'
        'armtemplates'           = 'ARM Templates'
        'as'                     = 'as'
        'aspnet'                 = 'ASP.NET'
        'aspnetcore'             = 'ASP.NET Core'
        'automapper'             = 'AutoMapper'
        'aws'                    = 'AWS'
        'azureai'                = 'Azure AI'
        'aifoundry'              = 'Azure AI Foundry'
        'azureappservice'        = 'Azure App Service'
        'cognitiveservices'      = 'Azure Cognitive Services'
        'azurecontainer'         = 'Azure Container'
        'azdo'                   = 'Azure DevOps'
        'azuredevops'            = 'Azure DevOps'
        'azurefunctions'         = 'Azure Functions'
        'azurekeyvault'          = 'Azure Key Vault'
        'azureml'                = 'Azure Machine Learning'
        'azuremonitor'           = 'Azure Monitor'
        'azureopenai'            = 'Azure OpenAI'
        'azuresql'               = 'Azure SQL'
        'azurestorage'           = 'Azure Storage'
        'bicep'                  = 'Bicep'
        'bingchat'               = 'Bing Chat'
        'blazor'                 = 'Blazor'
        'csharp'                 = 'C#'
        'cd'                     = 'CD'
        'ci'                     = 'CI'
        'cli'                    = 'CLI'
        'copilotchat'            = 'Copilot Chat'
        'copilotcli'             = 'Copilot CLI'
        'copilotvoice'           = 'Copilot Voice'
        'css'                    = 'CSS'
        'csv'                    = 'CSV'
        'dapper'                 = 'Dapper'
        'data&ai'                = 'Data & AI'
        'db'                     = 'DB'
        'developercommunity'     = 'Developer Community'
        'developertools'         = 'Developer Tools'
        'dns'                    = 'DNS'
        'docker'                 = 'Docker'
        'ef'                     = 'EF'
        'entityframework'        = 'EF'
        'efcore'                 = 'EF Core'
        'entityframeworkcore'    = 'EF Core'
        'etl'                    = 'ETL'
        'fsharp'                 = 'F#'
        'fluentassertions'       = 'FluentAssertions'
        'gcp'                    = 'GCP'
        'git'                    = 'Git'
        'github'                 = 'GitHub'
        'githubactions'          = 'GitHub Actions'
        'githubcopilot'          = 'GitHub Copilot'
        'grpc'                   = 'gRPC'
        'html'                   = 'HTML'
        'http'                   = 'HTTP'
        'https'                  = 'HTTPS'
        'iac'                    = 'IaC'
        'infrastructureascode'   = 'IaC'
        'ide'                    = 'IDE'
        'in'                     = 'in'
        'javascript'             = 'JavaScript'
        'js'                     = 'JavaScript'
        'json'                   = 'JSON'
        'jwt'                    = 'JWT'
        'k8s'                    = 'Kubernetes'
        'markdown'               = 'Markdown'
        'md'                     = 'Markdown'
        'maui'                   = 'MAUI'
        '.netmaui'               = 'MAUI'
        'modelcontextprotocol'   = 'MCP'
        'mcp'                    = 'MCP'
        'mcpserver'              = 'MCP Server'
        'mediatr'                = 'MediatR'
        'mscopilot'              = 'Microsoft Copilot'
        'machinelearning'        = 'ML'
        'ml'                     = 'ML'
        'msbuilds'               = 'MSBuild'
        'mstest'                 = 'MSTest'
        'mvc'                    = 'MVC'
        'nlog'                   = 'NLog'
        'nosql'                  = 'NoSQL'
        'npm'                    = 'npm'
        'nuget'                  = 'NuGet'
        'nunit'                  = 'NUnit'
        'oauth'                  = 'OAuth'
        'onboardtoazure'         = 'Onboard to Azure'
        'openai'                 = 'OpenAI'
        'orm'                    = 'ORM'
        'os'                     = 'OS'
        'pip'                    = 'pip'
        'polly'                  = 'Polly'
        'powerautomate'          = 'Power Automate'
        'powerapps'              = 'Power Apps'
        'powerbi'                = 'Power BI'
        'powerplatform'          = 'Power Platform'
        'pwa'                    = 'PWA'
        'qa'                     = 'QA'
        'rest'                   = 'REST'
        'saml'                   = 'SAML'
        'sdk'                    = 'SDK'
        'semantickernel'         = 'Semantic Kernel'
        'serilog'                = 'Serilog'
        'signalr'                = 'SignalR'
        'softwaredeveloper'      = 'Software Developer'
        'softwaredevelopment'    = 'Software Development'
        'spa'                    = 'SPA'
        'sql'                    = 'SQL'
        'sqlserver'              = 'SQL Server'
        'ssl'                    = 'SSL'
        'tcp'                    = 'TCP'
        'terraform'              = 'Terraform'
        'tls'                    = 'TLS'
        'toml'                   = 'TOML'
        'udp'                    = 'UDP'
        'ui'                     = 'UI'
        'ux'                     = 'UX'
        'vm'                     = 'VM'
        'vs'                     = 'VS'
        'visualstudio'           = 'VS'
        'vscode'                 = 'VS Code'
        'visualstudiocode'       = 'VS Code'
        'webapi'                 = 'Web API'
        'winui'                  = 'WinUI'
        'xamarin'                = 'Xamarin'
        'xml'                    = 'XML'
        'xunit'                  = 'xUnit'
        'yaml'                   = 'YAML'
        'yml'                    = 'YAML'
    }
    
    # Add dynamic mappings for categories and collections
    foreach ($category in $Categories) {
        $categoryKey = ($category.ToLowerInvariant() -replace ' ', '')
        $tagMappings[$categoryKey] = $category
    }

    $collectionKey = ($Collection.ToLowerInvariant() -replace ' ', '')
    $collectionValue = $Collection.Trim()
    if ($collectionValue.Length -le 3) {
        $collectionValue = $collectionValue.ToUpper()
    }
    else {
        $collectionValue = $collectionValue.Substring(0, 1).ToUpper() + $collectionValue.Substring(1).ToLower()
    }
    $tagMappings[$collectionKey] = $collectionValue

    if ($allTags.Count -eq 0) {
        return @()
    }

    $newTags = @()
    for ($i = 0; $i -lt $allTags.Count; $i++) {
        $tag = $allTags[$i]
        if ([string]::IsNullOrWhiteSpace($tag) -or $tag.Length -lt 1) {
            continue
        }

        # First, try full tag matching (highest priority)
        $tagKey = ($tag.ToLowerInvariant() -replace ' ', '')
        if ($tagMappings.ContainsKey($tagKey)) {
            $newTags += $tagMappings[$tagKey]
            continue
        }

        # If no full tag match, process individual words
        $words = $tag -split ' '
        # Remove words that look like a year in 20xx
        $words = @($words | Where-Object { $_ -notmatch '^20\d{2}$' })
        if (-not $words) {
            continue
        }

        for ($j = 0; $j -lt $words.Count; $j++) {
            $word = $words[$j]
            $wordKey = $word.ToLowerInvariant()
            if ($tagMappings.ContainsKey($wordKey)) {
                $words[$j] = $tagMappings[$wordKey]
            }
            elseif ($word.Length -gt 0 -and $word[0] -cmatch '[A-Z0-9]') {
                # leave as is
            }
            elseif ($word.Length -gt 1) {
                $words[$j] = $word.Substring(0, 1).ToUpper() + $word.Substring(1).ToLower()
            }
            elseif ($word.Length -eq 1) {
                $words[$j] = ([string]$word).ToUpper()
            }
        }
        if ($words -and $words.Count -gt 0) {
            $newTags += ($words -join ' ')
        }
    }
    $allTags = $newTags
    
    # Special rule: If "GitHub Copilot" tag exists, also add "AI" tag
    if ($allTags | Where-Object { $_ -eq 'GitHub Copilot' }) {
        $allTags += 'AI'
    }
    
    # Remove deprecated tags (old tag names that have been replaced)
    # These are legacy section/collection tags that are no longer valid
    $deprecatedTags = @(
        'Machine Learning',    # → ML
        'Artificial Intelligence', # → AI
        'Cloud',               # Deprecated section, no longer in use
        'Coding'               # → .NET
    )
    
    $allTags = $allTags | Where-Object {
        $tag = $_
        $deprecatedTags -notcontains $tag
    }
    
    # Remove case-insensitive duplicates, keeping the first (most uppercase) version
    $seen = @{}
    $filteredTags = @()
    foreach ($tag in $allTags) {
        $key = $tag.ToLowerInvariant()
        if (-not $seen.ContainsKey($key)) {
            $seen[$key] = $true
            $filteredTags += $tag
        }
    }

    # drop tags that are only numbers
    $filteredTags = $filteredTags | Where-Object {
        -not ($_ -match '^\d+$') -and $_.Length -gt 1
    }

    #drop tags that start with the letter L and are only followed by numbers
    $filteredTags = $filteredTags | Where-Object {
        -not ($_ -match '^L\d+$') -and $_.Length -gt 1
    }

    # drop tags that are hexadecimal color codes (3 or 6 character hex values), unless they match customTagMap or wordMap
    $filteredTags = $filteredTags | Where-Object {
        $tag = $_
        $tagKey = ($tag.ToLowerInvariant() -replace ' ', '')
        if ($tag.Length -eq 3 -or $tag.Length -eq 6) {
            if ($tag -match '^[0-9A-Fa-f]+$') {
                # Preserve if in unified tagMappings
                if ($tagMappings.ContainsKey($tagKey)) {
                    return $true
                }
                return $false  # Filter out hex color codes
            }
        }
        return $true  # Keep other tags
    }

    $filterWords = @(
        'Post',
        'Posts',
        'Update',
        'Updates',
        'Announcement',
        'Announcements',
        "What's New",
        'Article',
        'Articles',
        'Editorial',
        'Editorials',
        'Feature',
        'Features',
        'Release',
        'Releases',
        'Bulletin',
        'Bulletins',
        'Update Note',
        'Patch Note',
        'Patch',
        'Changelog',
        'Changelogs',
        'Press Release',
        'Press Releases',
        'Statement',
        'Statements',
        'Notification',
        'Notifications',
        'Alert',
        'Alerts',
        'Digest',
        'Digests',
        'Recap',
        'Recaps',
        'Review',
        'Reviews',
        'Insight',
        'Insights',
        'Report',
        'Reports',
        'Coverage',
        'Announcement Note',
        'Community Post',
        'Community Update',
        'Guest Blog',
        'Guest Post',
        'Guest Article',
        'Guest Editorial',
        'Version',
        'and', 'or', 'the', 'a', 'an', 'in', 'on', 'at', 'to', 'for', 'of', 'with', 'by', 'as',
        'is', 'are', 'was', 'were', 'be', 'been', 'being', 'have', 'has', 'had', 'do', 'does', 'did',
        'will', 'would', 'could', 'should', 'may', 'might', 'can', 'must', 'shall',
        'this', 'that', 'these', 'those', 'it', 'its', 'they', 'them', 'their', 'we', 'our', 'you', 'your',
        'he', 'him', 'his', 'she', 'her', 'hers', 'i', 'me', 'my', 'mine',
        'from', 'into', 'onto', 'upon', 'about', 'above', 'below', 'under', 'over', 'through',
        'between', 'among', 'during', 'before', 'after', 'while', 'since', 'until', 'unless',
        'if', 'when', 'where', 'why', 'how', 'what', 'which', 'who', 'whom', 'whose',
        'but', 'so', 'yet', 'nor', 'either', 'neither', 'both', 'all', 'any', 'some', 'each', 'every',
        'no', 'not', 'none', 'nothing', 'something', 'anything', 'everything',
        'here', 'there', 'everywhere', 'somewhere', 'anywhere', 'nowhere',
        'now', 'then', 'always', 'never', 'sometimes', 'often', 'usually', 'rarely', 'seldom',
        'very', 'too', 'quite', 'rather', 'pretty', 'much', 'many', 'more', 'most', 'less', 'least',
        'good', 'better', 'best', 'bad', 'worse', 'worst', 'big', 'bigger', 'biggest', 'small', 'smaller', 'smallest',
        'new', 'old', 'young', 'long', 'short', 'high', 'low', 'fast', 'slow', 'easy', 'hard'
    )
    
    $filterWords = $filterWords | ForEach-Object { $_.ToLowerInvariant() }
    
    # Filter out common filter words (articles, prepositions, etc.)
    # NOTE: We no longer need to preserve categories/collection since we don't add them anymore
    $filteredTags = $filteredTags | Where-Object {
        $tagLower = $_.ToLowerInvariant()
        return $tagLower -notin $filterWords
    }
    
    $finalTags = ($filteredTags | Sort-Object -Unique)
    
    return $finalTags
}
