function Write-ErrorDetails {
    <#
    .SYNOPSIS
        Writes detailed error information with consistent formatting
    .DESCRIPTION
        This function provides standardized error logging across all PowerShell scripts
        in the Tech Hub repository. It writes comprehensive error details including
        exception type, message, and stack traces.
    .PARAMETER ErrorRecord
        The error record object to process (typically $_ from a catch block)
    .PARAMETER Context
        Optional context string to describe what operation was being performed
    .EXAMPLE
        try {
            # Some operation
        }
        catch {
            Write-ErrorDetails -ErrorRecord $_ -Context "RSS feed processing"
            throw
        }
    #>
    param(
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,
        
        [Parameter(Mandatory = $false)]
        [string]$Context = "operation"
    )
    
    Write-Host "❌ Error occurred during $Context" -ForegroundColor Red
    Write-Host "Context: $Context"
    Write-Host "Exception Type: $($ErrorRecord.Exception.GetType().FullName)"
    Write-Host "Exception Message: $($ErrorRecord.Exception.Message)"
    Write-Host "Position: $($ErrorRecord.InvocationInfo.PositionMessage)"
    
    if ($ErrorRecord.Exception.StackTrace) {
        Write-Host "Stack Trace:"
        Write-Host $ErrorRecord.Exception.StackTrace
    }
    
    if ($ErrorRecord.ScriptStackTrace) {
        Write-Host "Script Stack Trace:"
        Write-Host $ErrorRecord.ScriptStackTrace
    }
    
    Write-Host "====================="
    Write-Host ""
    
    # Force output flush
    try {
        [Console]::Out.Flush()
        [Console]::Error.Flush()
    }
    catch {
        # Ignore flush errors in case console is not available
    }
}
