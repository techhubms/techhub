function ConvertTo-YamlFrontMatter {
    <#
    .SYNOPSIS
        Converts a hashtable to YAML frontmatter using YamlDotNet for 100% compatibility with .NET ContentFixer
    .DESCRIPTION
        This function uses the same YamlDotNet library as the ContentFixer to ensure
        identical YAML serialization between PowerShell content processing and C# content fixing.
    .PARAMETER FrontMatter
        Hashtable containing frontmatter key-value pairs
    .EXAMPLE
        $frontMatter = @{
            title = "My Article"
            tags = @("tag1", "tag2")
            date = "2026-01-16 10:00:00 +00:00"
        }
        ConvertTo-YamlFrontMatter -FrontMatter $frontMatter
    #>
    param(
        [Parameter(Mandatory = $true)]
        [hashtable]$FrontMatter
    )
    
    # Load YamlDotNet assembly if not already loaded (one-time operation)
    # Check if the type is already loaded to avoid redundant Add-Type calls
    if (-not ([System.Management.Automation.PSTypeName]'YamlDotNet.Serialization.SerializerBuilder').Type) {
        # DLL is copied to scripts/content-processing/lib/ directory for easy access
        $scriptRoot = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
        $yamlDotNetPath = Join-Path (Split-Path -Parent $scriptRoot) "lib/YamlDotNet.dll"
        
        if (-not (Test-Path $yamlDotNetPath)) {
            throw "YamlDotNet.dll not found at: $yamlDotNetPath. Please ensure the DLL is copied to scripts/content-processing/lib/"
        }
        
        Add-Type -Path $yamlDotNetPath
    }
    
    # Create serializer with underscored naming convention (matches ContentFixer)
    $serializerBuilder = New-Object YamlDotNet.Serialization.SerializerBuilder
    $serializer = $serializerBuilder.WithNamingConvention([YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention]::Instance).Build()
    
    # Convert PowerShell hashtable to .NET Dictionary for YamlDotNet
    $dictionary = New-Object 'System.Collections.Generic.Dictionary[string,object]'
    foreach ($key in $FrontMatter.Keys) {
        $value = $FrontMatter[$key]
        
        # Convert PowerShell arrays to .NET List<object> for proper YAML serialization
        if ($value -is [Array]) {
            $list = New-Object 'System.Collections.Generic.List[object]'
            foreach ($item in $value) {
                $list.Add($item)
            }
            $dictionary.Add($key, $list)
        }
        else {
            $dictionary.Add($key, $value)
        }
    }
    
    # Serialize to YAML
    $yaml = $serializer.Serialize($dictionary)
    
    # Return with YAML frontmatter delimiters
    return "---`n$yaml---`n"
}
