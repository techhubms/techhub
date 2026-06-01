using System.Reflection;
using HandlebarsDotNet;
using Microsoft.Extensions.Hosting;

namespace TechHub.Infrastructure.Services.Newsletter;

/// <summary>
/// Singleton service that compiles Handlebars email templates once at startup.
/// Injected into <see cref="NewsletterService"/> so templates are not recompiled
/// per DI scope.
/// </summary>
public sealed class NewsletterTemplateProvider
{
    public HandlebarsTemplate<object, object> Shell { get; }
    public HandlebarsTemplate<object, object> SubscriberFooter { get; }
    public HandlebarsTemplate<object, object> AccountAction { get; }
    public HandlebarsTemplate<object, object> WeeklyContent { get; }
    public HandlebarsTemplate<object, object> DailyContent { get; }
    public HandlebarsTemplate<object, object> AdminContent { get; }

    public NewsletterTemplateProvider(IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {
            var resourcesPath = Path.GetFullPath(Path.Join(env.ContentRootPath, "..", "TechHub.Infrastructure", "Data", "Resources"));
            Shell = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-template.html")));
            SubscriberFooter = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-subscriber-footer.html")));
            AccountAction = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-account-action-content.html")));
            WeeklyContent = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-weekly-content.html")));
            DailyContent = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-daily-content.html")));
            AdminContent = Handlebars.Compile(File.ReadAllText(Path.Join(resourcesPath, "newsletter-admin-content.html")));
        }
        else
        {
            Shell = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-template.html"));
            SubscriberFooter = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-subscriber-footer.html"));
            AccountAction = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-account-action-content.html"));
            WeeklyContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-weekly-content.html"));
            DailyContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-daily-content.html"));
            AdminContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-admin-content.html"));
        }
    }

    private static string LoadEmbeddedHtml(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
