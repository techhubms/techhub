using System.Reflection;
using HandlebarsDotNet;
using Microsoft.Extensions.Hosting;

namespace TechHub.Infrastructure.Services.Newsletter;

/// <summary>
/// Singleton service that compiles Handlebars email templates once at startup.
/// Injected into <see cref="NewsletterService"/> so templates are not recompiled
/// per DI scope.
/// </summary>
/// <remarks>
/// In development, templates are reloaded from disk (and recompiled) on each access to enable live editing.
/// </remarks>
public sealed class NewsletterTemplateProvider
{
    private readonly IHostEnvironment _env;
    private readonly string _resourcesPath = string.Empty;

    // Cache compiled templates for production performance
#pragma warning disable IDE0032
    private readonly HandlebarsTemplate<object, object>? _shell;
    private readonly HandlebarsTemplate<object, object>? _subscriberFooter;
    private readonly HandlebarsTemplate<object, object>? _accountAction;
    private readonly HandlebarsTemplate<object, object>? _weeklyContent;
    private readonly HandlebarsTemplate<object, object>? _dailyContent;
    private readonly HandlebarsTemplate<object, object>? _adminContent;
#pragma warning restore IDE0032

    public HandlebarsTemplate<object, object> Shell => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-template.html")))
        : _shell!;

    public HandlebarsTemplate<object, object> SubscriberFooter => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-subscriber-footer.html")))
        : _subscriberFooter!;

    public HandlebarsTemplate<object, object> AccountAction => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-account-action-content.html")))
        : _accountAction!;

    public HandlebarsTemplate<object, object> WeeklyContent => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-weekly-content.html")))
        : _weeklyContent!;

    public HandlebarsTemplate<object, object> DailyContent => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-daily-content.html")))
        : _dailyContent!;

    public HandlebarsTemplate<object, object> AdminContent => _env.IsDevelopment()
        ? Handlebars.Compile(File.ReadAllText(Path.Join(_resourcesPath, "newsletter-admin-content.html")))
        : _adminContent!;

    public NewsletterTemplateProvider(IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(env);
        _env = env;

        if (env.IsDevelopment())
        {
            _resourcesPath = Path.GetFullPath(Path.Join(env.ContentRootPath, "..", "TechHub.Infrastructure", "Data", "Resources"));
        }
        else
        {
            _shell = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-template.html"));
            _subscriberFooter = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-subscriber-footer.html"));
            _accountAction = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-account-action-content.html"));
            _weeklyContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-weekly-content.html"));
            _dailyContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-daily-content.html"));
            _adminContent = Handlebars.Compile(LoadEmbeddedHtml("TechHub.Infrastructure.Data.Resources.newsletter-admin-content.html"));
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
