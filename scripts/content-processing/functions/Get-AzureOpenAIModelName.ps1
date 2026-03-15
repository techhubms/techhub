function Get-AzureOpenAIModelName {
    <#
    .SYNOPSIS
    Gets the Azure OpenAI model name used for content processing.

    .DESCRIPTION
    Returns the model name configured for AI-powered content generation and processing.
    This is used consistently across all content processing scripts.

    .OUTPUTS
    String representing the model name (e.g., "gpt-5.2")

    .EXAMPLE
    $model = Get-AzureOpenAIModelName
    #>

    return "gpt-5.2"
}
