using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Api.Endpoints;

public static class NewsletterEndpoints
{
    public static IEndpointRouteBuilder MapNewsletterEndpoints(this IEndpointRouteBuilder app)
    {
        var publicGroup = app.MapGroup("/api/newsletter")
            .WithTags("Newsletter")
            .RequireRateLimiting("api-public");

        publicGroup.MapGet("/sections", GetNewsletterSectionsAsync)
            .WithName("GetNewsletterSections")
            .WithSummary("Get newsletter sections")
            .WithDescription("Returns all newsletter-eligible sections except the synthetic 'all' section.")
            .Produces<IReadOnlyList<Section>>(StatusCodes.Status200OK);

        publicGroup.MapPost("/subscribe", SubscribeAsync)
            .WithName("NewsletterSubscribe")
            .WithSummary("Subscribe to newsletters")
            .WithDescription("Creates or updates a newsletter subscription for the requested weekly and daily sections.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        publicGroup.MapPost("/unsubscribe", UnsubscribeAsync)
            .WithName("NewsletterUnsubscribe")
            .WithSummary("Unsubscribe from newsletters")
            .WithDescription("Unsubscribes an email address when a valid unsubscribe token is supplied.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        var adminGroup = app.MapGroup("/api/admin/newsletter")
            .WithTags("Admin")
            .RequireAuthorization("AdminOnly")
            .RequireRateLimiting("api-admin");

        adminGroup.MapGet("/subscribers", GetSubscribersAsync)
            .WithName("GetNewsletterSubscribers")
            .WithSummary("Get newsletter subscribers")
            .WithDescription("Returns newsletter subscribers for the admin dashboard with optional paging and filters.")
            .Produces<IReadOnlyList<NewsletterSubscriber>>(StatusCodes.Status200OK);

        adminGroup.MapPut("/subscribers/{id:long}", UpdateSubscriberAsync)
            .WithName("UpdateNewsletterSubscriber")
            .WithSummary("Update a newsletter subscriber")
            .WithDescription("Updates display name and section preferences for an existing newsletter subscriber.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        adminGroup.MapDelete("/subscribers/{id:long}", DeleteSubscriberAsync)
            .WithName("DeleteNewsletterSubscriber")
            .WithSummary("Delete a newsletter subscriber")
            .WithDescription("Deletes a newsletter subscriber from the admin dashboard.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        adminGroup.MapGet("/send-log", GetSendLogAsync)
            .WithName("GetNewsletterSendLog")
            .WithSummary("Get newsletter send log")
            .WithDescription("Returns the most recent newsletter send log entries for admin troubleshooting.")
            .Produces<IReadOnlyList<NewsletterSendLogEntry>>(StatusCodes.Status200OK);

        adminGroup.MapPost("/trigger", TriggerNewsletterAsync)
            .WithName("TriggerNewsletter")
            .WithSummary("Trigger newsletter sending")
            .WithDescription("Triggers an immediate newsletter processing run.")
            .Produces(StatusCodes.Status202Accepted);

        adminGroup.MapPost("/test-send", TestSendNewsletterAsync)
            .WithName("TestSendNewsletter")
            .WithSummary("Trigger a newsletter test send")
            .WithDescription("Triggers a test roundup email to a specific recipient.")
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> GetNewsletterSectionsAsync(IContentRepository contentRepository, CancellationToken ct)
    {
        var sections = await contentRepository.GetAllSectionsAsync(ct);
        var result = sections
            .Where(s => !string.Equals(s.Name, "all", StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Results.Ok(result);
    }

    private static async Task<IResult> SubscribeAsync(
        NewsletterSubscribeRequest request,
        IContentRepository contentRepository,
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            return Results.BadRequest("A valid email address is required.");
        }

        var weekly = request.WeeklySections ?? [];
        var daily = request.DailySections ?? [];
        if (weekly.Count == 0 && daily.Count == 0)
        {
            return Results.BadRequest("Select at least one weekly or daily section.");
        }

        var validSections = (await contentRepository.GetAllSectionsAsync(ct))
            .Where(s => !string.Equals(s.Name, "all", StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Name.Trim().ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var normalizedWeekly = NormalizeRequestedSections(weekly, validSections);
        var normalizedDaily = NormalizeRequestedSections(daily, validSections);
        if (normalizedWeekly.Count == 0 && normalizedDaily.Count == 0)
        {
            return Results.BadRequest("Select at least one valid weekly or daily section.");
        }

        var id = await subscriberRepository.UpsertSubscriberAsync(
            request.Email,
            request.DisplayName,
            normalizedWeekly,
            normalizedDaily,
            ct);

        return Results.Ok(new { id, message = "Subscribed" });
    }

    private static async Task<IResult> UnsubscribeAsync(
        NewsletterUnsubscribeRequest request,
        IOptions<NewsletterOptions> options,
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token))
        {
            return Results.BadRequest("Email and token are required.");
        }

        var secret = options.Value.UnsubscribeSecret;
        if (string.IsNullOrWhiteSpace(secret))
        {
            return Results.BadRequest("Unsubscribe is not configured.");
        }

        if (!NewsletterService.IsValidUnsubscribeToken(request.Email, request.Token, secret))
        {
            return Results.BadRequest("Invalid unsubscribe token.");
        }

        var updated = await subscriberRepository.UnsubscribeAsync(request.Email, ct);
        return updated
            ? Results.Ok(new { message = "Unsubscribed" })
            : Results.NotFound(new { message = "Subscription not found" });
    }

    private static async Task<IResult> GetSubscribersAsync(
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 200,
        [FromQuery] string? search = null,
        [FromQuery] bool? confirmed = null)
    {
        var subscribers = await subscriberRepository.GetSubscribersAsync(page, pageSize, search, confirmed, ct);
        return Results.Ok(subscribers);
    }

    private static async Task<IResult> UpdateSubscriberAsync(
        long id,
        NewsletterSubscriberUpdateRequest request,
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct)
    {
        var updated = await subscriberRepository.UpdateSubscriberAsync(
            id,
            request.DisplayName,
            request.WeeklySections ?? [],
            request.DailySections ?? [],
            ct);

        return updated ? Results.Ok() : Results.NotFound();
    }

    private static async Task<IResult> DeleteSubscriberAsync(
        long id,
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct)
    {
        var deleted = await subscriberRepository.DeleteSubscriberAsync(id, ct);
        return deleted ? Results.Ok() : Results.NotFound();
    }

    private static async Task<IResult> GetSendLogAsync(
        INewsletterSubscriberRepository subscriberRepository,
        CancellationToken ct,
        [FromQuery] int count = 100)
    {
        var log = await subscriberRepository.GetSendLogAsync(count, ct);
        return Results.Ok(log);
    }

    private static IResult TriggerNewsletterAsync(NewsletterBackgroundService backgroundService)
    {
        backgroundService.TriggerImmediateRun();
        return Results.Accepted("/api/admin/newsletter/send-log", new { message = "Newsletter send triggered" });
    }

    private static IResult TestSendNewsletterAsync(
        [FromQuery] string email,
        NewsletterBackgroundService backgroundService,
        [FromQuery] string? roundupSlug = null)
    {
        if (!IsValidEmail(email))
        {
            return Results.BadRequest("A valid email address is required.");
        }

        backgroundService.TriggerTestSend(email.Trim(), roundupSlug);
        return Results.Accepted("/api/admin/newsletter/send-log", new { message = "Newsletter test send triggered" });
    }

    private static bool IsValidEmail(string input)
    {
        try
        {
            var parsed = new MailAddress(input.Trim());
            return string.Equals(parsed.Address, input.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static List<string> NormalizeRequestedSections(
        IReadOnlyList<string> requestedSections,
        HashSet<string> validSections) =>
        requestedSections
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToLowerInvariant())
            .Where(validSections.Contains)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
}
