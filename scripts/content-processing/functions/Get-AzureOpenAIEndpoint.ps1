function Get-AzureOpenAIEndpoint {
    <#
    .SYNOPSIS
    Gets the Azure OpenAI endpoint URL for the specified environment.

    .DESCRIPTION
    Returns the fully qualified Azure AI Services endpoint URL for the specified environment.
    The endpoint follows the modern AI Services format and includes the model name in the path.

    .PARAMETER Environment
    The environment to get the endpoint for. Valid values are 'staging' or 'prod'.
    Defaults to 'staging'.

    .OUTPUTS
    String containing the complete Azure OpenAI endpoint URL

    .EXAMPLE
    $endpoint = Get-AzureOpenAIEndpoint -Environment 'prod'
    # Returns: https://oai-techhub-prod.services.ai.azure.com/models/gpt-5.2/chat/completions

    .EXAMPLE
    $endpoint = Get-AzureOpenAIEndpoint
    # Returns: https://oai-techhub-staging.services.ai.azure.com/models/gpt-5.2/chat/completions
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false)]
        [ValidateSet('staging', 'prod')]
        [string]$Environment = 'staging'
    )
    
    $modelName = Get-AzureOpenAIModelName
    $resourceName = "oai-techhub-$Environment"
    return "https://$resourceName.services.ai.azure.com/models/$modelName/chat/completions"
}
