using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// Shared bUnit setup for admin page tests: persistent component state, no-op logging and the JS interop
/// call every <c>PageComponentBase</c> page makes after render. Derived classes register their own
/// <c>ITechHubApiClient</c> mock.
/// </summary>
public abstract class AdminPageTestBase : BunitContext
{
    protected AdminPageTestBase()
    {
        Services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        AddBunitPersistentComponentState();
        JSInterop.SetupVoid("markScriptsReady");
    }
}
