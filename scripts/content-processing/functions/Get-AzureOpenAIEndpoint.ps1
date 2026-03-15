function Get-AzureOpenAIEndpoint {
    <#
    .SYNOPSIS
    Gets the Azure OpenAI endpoint URL for the specified environment.

    .DESCRIPTION
    Returns the fully qualified Azure OpenAI endpoint URL for the specified environment.
    The endpoint follows the Azure Cognitive Services format used by AI Foundry.

    .PARAMETER Environment
    The environment to get the endpoint for. Valid values are 'staging' or 'prod'.
    Defaults to 'staging'.

    .OUTPUTS
    String containing the complete Azure OpenAI endpoint URL

    .EXAMPLE
    $endpoint = Get-AzureOpenAIEndpoint -Environment 'prod'
    # Returns: https://oai-techhub-prod.cognitiveservices.azure.com/openai/deployments/gpt-5.2/chat/completions

    .EXAMPLE
    $endpoint = Get-AzureOpenAIEndpoint
    # Returns: https://oai-techhub-staging.cognitiveservices.azure.com/openai/deployments/gpt-5.2/chat/completions
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false)]
        [ValidateSet('staging', 'prod')]
        [string]$Environment = 'staging'
    )
    
    $modelName = Get-AzureOpenAIModelName
    $resourceName = "oai-techhub-$Environment"
    return "https://$resourceName.cognitiveservices.azure.com/openai/deployments/$modelName/chat/completions"
}
