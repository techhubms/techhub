function Get-FilteredTags {
    <#
    .SYNOPSIS
        Filters and enhances tags based on categories and collection type
    .DESCRIPTION
        This function processes tags by:
        1. Normalizing tag format (custom mappings + word mappings)
        2. Removing common filter words
        3. Removing duplicates and empty entries
        4. Preserving categories and collection values
        5. Returns hashtable with both 'tags' and 'tags_normalized' arrays
    .PARAMETER Tags
        Array of tags to process
    .PARAMETER Categories  
        Array of categories to preserve (won't be filtered out)
    .PARAMETER Collection
        Collection name to preserve (won't be filtered out)
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

    # Filter out all known categories and collections from original tags first
    $allKnownCategories = @(
        'AI', 
        'GitHub Copilot', 
        'Azure', 
        'Coding', 
        'DevOps', 
        'Security', 
        'ML'
    )
    
    $allKnownCollections = @(
        'News', 
        'Posts', 
        'Videos', 
        'Community', 
        'Events', 
        'Roundups'
    )
    
    # Remove all categories and collections from the original tags
    $cleanedTags = $Tags | Where-Object {
        $tag = $_
        # Filter out if it's a known category or collection
        $isKnownCategory = $allKnownCategories -contains $tag
        $isKnownCollection = $allKnownCollections -contains $tag
        return -not ($isKnownCategory -or $isKnownCollection)
    }
    
    # Now add the current categories and collection
    $allTags = @($cleanedTags) + @($Categories) + @($Collection)

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
        '.net'                     = '.NET'
        'dotnet'                   = '.NET'
        'dotnetcli'                = '.NET CLI'
        'a2a'                      = 'A2A'
        'ad'                       = 'AD'
        'ai'                       = 'AI'
        'artificialintelligence'   = 'AI'
        'aiagent'                  = 'AI Agent'
        'aiagents'                 = 'AI Agents'
        'aks'                      = 'AKS'
        'azurekubernetesservice'   = 'AKS'
        'azurek8s'                 = 'AKS'
        'api'                      = 'API'
        'applicationinsights'      = 'Application Insights'
        'appinsights'              = 'Application Insights'
        'arm'                      = 'ARM'
        'armtemplates'             = 'ARM Templates'
        'as'                       = 'as'
        'aspnet'                   = 'ASP.NET'
        'aspnetcore'               = 'ASP.NET Core'
        'automapper'               = 'AutoMapper'
        'aws'                      = 'AWS'
        'azureai'                  = 'Azure AI'
        'aifoundry'                = 'Azure AI Foundry'
        'azureappservice'          = 'Azure App Service'
        'cognitiveservices'        = 'Azure Cognitive Services'
        'azurecontainer'           = 'Azure Container'
        'azdo'                     = 'Azure DevOps'
        'azuredevops'              = 'Azure DevOps'
        'azurefunctions'           = 'Azure Functions'
        'azurekeyvault'            = 'Azure Key Vault'
        'azureml'                  = 'Azure Machine Learning'
        'azuremonitor'             = 'Azure Monitor'
        'azureopenai'              = 'Azure OpenAI'
        'azuresql'                 = 'Azure SQL'
        'azurestorage'             = 'Azure Storage'
        'bicep'                    = 'Bicep'
        'bingchat'                 = 'Bing Chat'
        'blazor'                   = 'Blazor'
        'csharp'                   = 'C#'
        'cd'                       = 'CD'
        'ci'                       = 'CI'
        'cli'                      = 'CLI'
        'copilotchat'              = 'Copilot Chat'
        'copilotcli'               = 'Copilot CLI'
        'copilotvoice'             = 'Copilot Voice'
        'css'                      = 'CSS'
        'csv'                      = 'CSV'
        'dapper'                   = 'Dapper'
        'data&ai'                  = 'Data & AI'
        'db'                       = 'DB'
        'developercommunity'       = 'Developer Community'
        'developertools'           = 'Developer Tools'
        'dns'                      = 'DNS'
        'docker'                   = 'Docker'
        'ef'                       = 'EF'
        'entityframework'          = 'EF'
        'efcore'                   = 'EF Core'
        'entityframeworkcore'      = 'EF Core'
        'etl'                      = 'ETL'
        'fsharp'                   = 'F#'
        'fluentassertions'         = 'FluentAssertions'
        'gcp'                      = 'GCP'
        'git'                      = 'Git'
        'github'                   = 'GitHub'
        'githubactions'            = 'GitHub Actions'
        'githubcopilot'            = 'GitHub Copilot'
        'grpc'                     = 'gRPC'
        'html'                     = 'HTML'
        'http'                     = 'HTTP'
        'https'                    = 'HTTPS'
        'iac'                      = 'IaC'
        'infrastructureascode'     = 'IaC'
        'ide'                      = 'IDE'
        'in'                       = 'in'
        'javascript'               = 'JavaScript'
        'js'                       = 'JavaScript'
        'json'                     = 'JSON'
        'jwt'                      = 'JWT'
        'k8s'                      = 'Kubernetes'
        'markdown'                 = 'Markdown'
        'md'                       = 'Markdown'
        'maui'                     = 'MAUI'
        '.netmaui'                 = 'MAUI'
        'modelcontextprotocol'     = 'MCP'
        'mediatr'                  = 'MediatR'
        'mscopilot'                = 'Microsoft Copilot'
        'ml'                       = 'ML'
        'msbuilds'                 = 'MSBuild'
        'mstest'                   = 'MSTest'
        'mvc'                      = 'MVC'
        'nlog'                     = 'NLog'
        'nosql'                    = 'NoSQL'
        'npm'                      = 'npm'
        'nuget'                    = 'NuGet'
        'nunit'                    = 'NUnit'
        'oauth'                    = 'OAuth'
        'onboardtoazure'           = 'Onboard to Azure'
        'openai'                   = 'OpenAI'
        'orm'                      = 'ORM'
        'os'                       = 'OS'
        'pip'                      = 'pip'
        'polly'                    = 'Polly'
        'powerautomate'            = 'Power Automate'
        'powerapps'                = 'Power Apps'
        'powerbi'                  = 'Power BI'
        'powerplatform'            = 'Power Platform'
        'pwa'                      = 'PWA'
        'qa'                       = 'QA'
        'rest'                     = 'REST'
        'saml'                     = 'SAML'
        'sdk'                      = 'SDK'
        'semantickernel'           = 'Semantic Kernel'
        'serilog'                  = 'Serilog'
        'signalr'                  = 'SignalR'
        'softwaredeveloper'        = 'Software Developer'
        'softwaredevelopment'      = 'Software Development'
        'spa'                      = 'SPA'
        'sql'                      = 'SQL'
        'sqlserver'                = 'SQL Server'
        'ssl'                      = 'SSL'
        'tcp'                      = 'TCP'
        'terraform'                = 'Terraform'
        'tls'                      = 'TLS'
        'toml'                     = 'TOML'
        'udp'                      = 'UDP'
        'ui'                       = 'UI'
        'ux'                       = 'UX'
        'vm'                       = 'VM'
        'vs'                       = 'VS'
        'visualstudio'             = 'VS'
        'vscode'                   = 'VS Code'
        'visualstudiocode'         = 'VS Code'
        'webapi'                   = 'Web API'
        'winui'                    = 'WinUI'
        'xamarin'                  = 'Xamarin'
        'xml'                      = 'XML'
        'xunit'                    = 'xUnit'
        'yaml'                     = 'YAML'
        'yml'                      = 'YAML'
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

    # if there is a tag as part of the value contains the word 'AI' or 'GitHub Copilot', add 'AI' and 'GitHub Copilot' respectively
    $aiFound = $false
    $githubCopilotFound = $false
    foreach ($tag in $allTags) {
        if ($tag -match '(?i)\bai\b') {
            $aiFound = $true
        }
        if ($tag -match '(?i)\bgithub copilot\b') {
            $githubCopilotFound = $true
        }
    }
    if ($aiFound -and ($allTags -notcontains 'AI')) {
        $allTags += 'AI'
    }
    if ($githubCopilotFound -and ($allTags -notcontains 'GitHub Copilot')) {
        $allTags += 'GitHub Copilot'
        $allTags += 'AI'
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
        'Blog',
        'Blogs',
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
    
    # Remove categories and collections from filter words so they never get filtered out accidentally
    $categoriesToKeep = @()
    if ($Categories -and $Categories.Count -gt 0) {
        $categoriesToKeep = $Categories | Where-Object { $_ -and $_.Trim() -ne '' } | ForEach-Object { $_.ToLowerInvariant() }
    }
    
    $collectionToKeep = $null
    if ($Collection -and $Collection.Trim() -ne '') {
        $collectionToKeep = $Collection.ToLowerInvariant()
    }

    $filteredTags = $filteredTags | Where-Object {
        $tagLower = $_.ToLowerInvariant()
        
        # Keep if it's a category we want to preserve
        if ($categoriesToKeep -contains $tagLower) {
            return $true
        }
        
        # Keep if it's the collection we want to preserve
        if ($collectionToKeep -and $tagLower -eq $collectionToKeep) {
            return $true
        }
        
        # Otherwise, filter out if it's in the filter words list
        return $tagLower -notin $filterWords
    }
    
    $finalTags = ($filteredTags | Sort-Object -Unique)
    
    # Generate normalized versions using the same logic as Ruby and JavaScript
    # CRITICAL: Keep same order as finalTags - no additional sorting!
    $normalizedTags = $finalTags | ForEach-Object {
        $tag = $_
        
        # Preprocess special characters - exact same as Ruby/JavaScript
        $replacements = @{
            '#' = 'Sharp'
            '+' = 'Plus'
            '-' = 'Min'
            '&' = 'And'
            '@' = 'At'
            '*' = 'Star'
            '!' = 'Bang'
            '|' = 'Pipe'
            '\' = 'Backslash'
            '/' = 'Slash'
            '.' = 'Dot'
        }
        
        foreach ($char in $replacements.Keys) {
            $escapedChar = [regex]::Escape($char)
            $replacement = $replacements[$char]
            
            # Replace character directly without adding extra spaces
            $tag = $tag -replace $escapedChar, $replacement
        }
        
        # Apply normalization (remove non-alphanumeric/space/hyphen, downcase, clean whitespace)
        $tag = $tag -replace '[^a-zA-Z0-9\s\-]', ''
        $tag = $tag.ToLowerInvariant().Trim() -replace '\s+', ' '
        
        return $tag
    } | Where-Object { $_ -and $_.Trim() -ne '' }
    
    # Ensure arrays are same length - filter out empty normalized tags from both arrays
    $validPairs = @()
    for ($i = 0; $i -lt $finalTags.Count; $i++) {
        if ($i -lt $normalizedTags.Count -and $normalizedTags[$i] -and $normalizedTags[$i].Trim() -ne '') {
            $validPairs += @{
                original   = $finalTags[$i]
                normalized = $normalizedTags[$i]
            }
        }
    }
    
    $orderedTags = $validPairs | ForEach-Object { $_.original }
    $orderedNormalized = $validPairs | ForEach-Object { $_.normalized }
    
    return @{
        tags            = $orderedTags
        tags_normalized = $orderedNormalized
    }
}
