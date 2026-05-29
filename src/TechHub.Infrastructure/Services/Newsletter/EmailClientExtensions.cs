using Azure.Communication.Email;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;

namespace TechHub.Infrastructure.Services.Newsletter;

public static class EmailClientExtensions
{
    public static IServiceCollection AddAcsEmailClient(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<NewsletterOptions>>().Value;
            if (string.IsNullOrWhiteSpace(options.Endpoint))
            {
                return new EmailClient(new Uri("https://invalid.communication.azure.com/"), new DefaultAzureCredential());
            }

            return new EmailClient(new Uri(options.Endpoint), new DefaultAzureCredential());
        });

        return services;
    }
}
