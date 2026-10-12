using TechHub.Core.Logging;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Endpoint filter that logs every request rejected with 400 Bad Request, including the reason.
/// Applied once on the root group in Program.cs so it wraps every endpoint group's own filters.
/// </summary>
public static class RejectedRequestLogging
{
    public static TBuilder WithRejectedRequestLogging<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
        => builder.AddEndpointFilter(LogBadRequestAsync);

    private static async ValueTask<object?> LogBadRequestAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);

        // Results<...> unions wrap the concrete result.
        var inner = result is INestedHttpResult nested ? nested.Result : result;

        if (inner is IStatusCodeHttpResult { StatusCode: StatusCodes.Status400BadRequest })
        {
            var request = context.HttpContext.Request;
            var reason = (inner as IValueHttpResult)?.Value as string;

            context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(RejectedRequestLogging).FullName!)
                .LogWarning("Rejected request {Method} {Path}{Query}: {Reason}",
                    request.Method.Sanitize(),
                    request.Path.Value.Sanitize(),
                    request.QueryString.Value.Sanitize(),
                    reason.Sanitize());
        }

        return result;
    }
}
