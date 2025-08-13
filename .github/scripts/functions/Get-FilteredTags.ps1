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
        'Magazines', 
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
        } else {
            $expandedTags += $tag
        }
    }
    $allTags = $expandedTags

    # Custom replacements for specific tags
    $customTagMap = @{
        'aiagent'                = 'AI Agent'
        'aiagents'               = 'AI Agents'
        'onboardtoazure'         = 'Onboard to Azure'
        'artificialintelligence' = 'Artificial Intelligence'
        'githubcopilot'          = 'GitHub Copilot'
        'vs'                     = 'Visual Studio'
        'vscode'                 = 'Visual Studio Code'
        'vs code'                = 'Visual Studio Code'
        'infrastructureascode'   = 'IaC'
        'infrastructure as code' = 'IaC'
        'dotnet'                 = '.NET'
        'csharp'                 = 'C#'
        'fsharp'                 = 'F#'
        'aspnet'                 = 'ASP.NET'
        'aspnetcore'             = 'ASP.NET Core'
        'webapi'                 = 'Web API'
        'ef'                     = 'Entity Framework'
        'efcore'                 = 'Entity Framework Core'
        'entityframework'        = 'Entity Framework'
        'semantickernel'         = 'Semantic Kernel'
        'azureopenai'            = 'Azure OpenAI'
        'azureai'                = 'Azure AI'
        'azureml'                = 'Azure Machine Learning'
        'aifoundry'              = 'Azure AI Foundry'
        'cognitiveservices'      = 'Azure Cognitive Services'
        'azurefunctions'         = 'Azure Functions'
        'azuredevops'            = 'Azure DevOps'
        'azdo'                   = 'Azure DevOps'
        'githubactions'          = 'GitHub Actions'
        'powerplatform'          = 'Power Platform'
        'powerapps'              = 'Power Apps'
        'powerbi'                = 'Power BI'
        'powerautomate'          = 'Power Automate'
        'bingchat'               = 'Bing Chat'
        'mscopilot'              = 'Microsoft Copilot'
        'copilotchat'            = 'Copilot Chat'
        'copilotvoice'           = 'Copilot Voice'
        'copilotcli'             = 'Copilot CLI'
        'bicep'                  = 'Bicep'
        'terraform'              = 'Terraform'
        'armtemplates'           = 'ARM Templates'
        'azureappservice'        = 'Azure App Service'
        'azuresql'               = 'Azure SQL'
        'azurestorage'           = 'Azure Storage'
        'azurekeyvault'          = 'Azure Key Vault'
        'azurecontainer'         = 'Azure Container'
        'azurek8s'               = 'Azure Kubernetes Service'
        'aks'                    = 'Azure Kubernetes Service'
        'signalr'                = 'SignalR'
        'blazor'                 = 'Blazor'
        'maui'                   = '.NET MAUI'
        'winui'                  = 'WinUI'
        'xamarin'                = 'Xamarin'
        'nuget'                  = 'NuGet'
        'dotnetcli'              = '.NET CLI'
        'msbuild'                = 'MSBuild'
        'polly'                  = 'Polly'
        'fluentassertions'       = 'FluentAssertions'
        'mediatr'                = 'MediatR'
        'automapper'             = 'AutoMapper'
        'serilog'                = 'Serilog'
        'nlog'                   = 'NLog'
        'dapper'                 = 'Dapper'
        'xunit'                  = 'xUnit'
        'nunit'                  = 'NUnit'
        'mstest'                 = 'MSTest'
        'applicationinsights'    = 'Application Insights'
        'appinsights'            = 'Application Insights'
        'azuremonitor'           = 'Azure Monitor'
        'data&ai'                = 'Data & AI'
    }

    # Define a word map for special casing, similar to customTagMap
    $wordMap = @{
        'a2a'        = 'A2A'
        'dotnet'     = '.NET'
        '.net'       = '.NET'
        'in'         = 'in'
        'as'         = 'as'
        'ai'         = 'AI'
        'github'     = 'GitHub'
        'openai'     = 'OpenAI'
        'iac'        = 'IaC'
        'api'        = 'API'
        'ui'         = 'UI'
        'ux'         = 'UX'
        'css'        = 'CSS'
        'html'       = 'HTML'
        'js'         = 'JavaScript'
        'javascript' = 'JavaScript'
        'sql'        = 'SQL'
        'json'       = 'JSON'
        'xml'        = 'XML'
        'http'       = 'HTTP'
        'https'      = 'HTTPS'
        'ml'         = 'ML'
        'ci'         = 'CI'
        'cd'         = 'CD'
        'qa'         = 'QA'
        'ide'        = 'IDE'
        'cli'        = 'CLI'
        'sdk'        = 'SDK'
        'orm'        = 'ORM'
        'mvc'        = 'MVC'
        'spa'        = 'SPA'
        'pwa'        = 'PWA'
        'rest'       = 'REST'
        'grpc'       = 'gRPC'
        'tcp'        = 'TCP'
        'udp'        = 'UDP'
        'dns'        = 'DNS'
        'ssl'        = 'SSL'
        'tls'        = 'TLS'
        'jwt'        = 'JWT'
        'oauth'      = 'OAuth'
        'saml'       = 'SAML'
        'ad'         = 'AD'
        'arm'        = 'ARM'
        'vm'         = 'VM'
        'os'         = 'OS'
        'db'         = 'DB'
        'nosql'      = 'NoSQL'
        'etl'        = 'ETL'
        'csv'        = 'CSV'
        'yaml'       = 'YAML'
        'yml'        = 'YAML'
        'toml'       = 'TOML'
        'markdown'   = 'Markdown'
        'md'         = 'Markdown'
        'git'        = 'Git'
        'npm'        = 'npm'
        'nuget'      = 'NuGet'
        'pip'        = 'pip'
        'docker'     = 'Docker'
        'k8s'        = 'Kubernetes'
        'aws'        = 'AWS'
        'gcp'        = 'GCP'
        'vs'         = 'VS'
        'vscode'     = 'VS Code'
    }
    
    # Add dynamic mappings for categories and collections
    foreach ($category in $Categories) {
        $categoryKey = ($category.ToLowerInvariant() -replace ' ', '')
        $customTagMap[$categoryKey] = $category
    }

    $collectionKey = ($Collection.ToLowerInvariant() -replace ' ', '')
    $collectionValue = $Collection.Trim()
    if ($collectionValue.Length -le 3) {
        $collectionValue = $collectionValue.ToUpper()
    }
    else {
        $collectionValue = $collectionValue.Substring(0, 1).ToUpper() + $collectionValue.Substring(1).ToLower()
    }
    $customTagMap[$collectionKey] = $collectionValue

    if ($allTags.Count -eq 0) {
        return @()
    }

    $newTags = @()
    for ($i = 0; $i -lt $allTags.Count; $i++) {
        $tag = $allTags[$i]
        if ([string]::IsNullOrWhiteSpace($tag) -or $tag.Length -lt 1) {
            continue
        }

        # Apply custom tag mappings after word processing
        $tagKey = ($tag.ToLowerInvariant() -replace ' ', '')
        if ($customTagMap.ContainsKey($tagKey)) {
            $newTags += $customTagMap[$tagKey]
            continue
        }

        $words = $tag -split ' '
        # Remove words that look like a year in 20xx
        $words = @($words | Where-Object { $_ -notmatch '^20\d{2}$' })
        if (-not $words) {
            continue
        }

        for ($j = 0; $j -lt $words.Count; $j++) {
            $word = $words[$j]
            $wordKey = $word.ToLowerInvariant()
            if ($wordMap.ContainsKey($wordKey)) {
                $words[$j] = $wordMap[$wordKey]
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
                # Preserve if in customTagMap or wordMap
                if ($customTagMap.ContainsKey($tagKey) -or $wordMap.ContainsKey($tagKey)) {
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
            '.' = 'Dot'
            '&' = 'And'
            '@' = 'At'
            '*' = 'Star'
            '!' = 'Bang'
            '|' = 'Pipe'
            '\' = 'Backslash'
            '/' = 'Slash'
        }
        
        foreach ($char in $replacements.Keys) {
            $escapedChar = [regex]::Escape($char)
            $replacement = $replacements[$char]
            
            # Replace character between two other characters (add spaces around)
            $tag = $tag -replace "(\w)$escapedChar(\w)", "`$1 $replacement `$2"
            
            # Replace character with space before it (space + char)
            $tag = $tag -replace " $escapedChar(\w)", " $replacement `$1"
            
            # Replace character with space after it (char + space)
            $tag = $tag -replace "(\w)$escapedChar ", "`$1 $replacement "
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
                original = $finalTags[$i]
                normalized = $normalizedTags[$i]
            }
        }
    }
    
    $orderedTags = $validPairs | ForEach-Object { $_.original }
    $orderedNormalized = $validPairs | ForEach-Object { $_.normalized }
    
    return @{
        tags = $orderedTags
        tags_normalized = $orderedNormalized
    }
}
