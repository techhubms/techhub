using Microsoft.AspNetCore.Components;

namespace TechHub.Web.Services;

/// <summary>
/// Service for handling errors and exceptions in the application.
/// Provides methods to navigate to error pages with exception details.
/// </summary>
internal class ErrorService
{
    private Exception? _lastException;

    /// <summary>
    /// Stores an exception and navigates to the error page.
    /// </summary>
    /// <param name="exception">The exception that occurred</param>
    /// <param name="navigation">NavigationManager to perform navigation</param>
    /// <param name="logger">Logger to log the exception</param>
    public void HandleError(Exception exception, NavigationManager navigation, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(navigation);
        ArgumentNullException.ThrowIfNull(logger);

        // Store the exception for the error page to retrieve
        _lastException = exception;

        // Log the exception with full details
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        // Navigate to error page
        navigation.NavigateTo("/error", forceLoad: true);
    }

    /// <summary>
    /// Gets the last exception that was handled.
    /// This is used by the Error page to display exception details.
    /// </summary>
    /// <returns>The last exception, or null if no exception was stored</returns>
    public Exception? GetLastException()
    {
        var exception = _lastException;
        _lastException = null; // Clear after retrieving
        return exception;
    }
}
