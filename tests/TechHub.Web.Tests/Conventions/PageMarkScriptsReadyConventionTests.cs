using FluentAssertions;

namespace TechHub.Web.Tests.Conventions;

/// <summary>
/// Validates that every Razor page inherits PageComponentBase.
/// The base class enforces the markScriptsLoading/markScriptsReady lifecycle,
/// which is required for scroll position restoration on back/forward navigation.
/// See src/TechHub.Web/AGENTS.md for the rule.
/// </summary>
public class PageMarkScriptsReadyConventionTests
{
    private static readonly string _pagesDirectory = Path.GetFullPath(
        Path.Join(
            new DirectoryInfo(AppContext.BaseDirectory).Parent!.Parent!.Parent!.Parent!.Parent!.FullName,
            "src", "TechHub.Web", "Components", "Pages"));

    /// <summary>
    /// Pages that are exempt from the markScriptsReady rule because they only
    /// render a child component that handles it (e.g., AdminLogin renders AdminLoginContent).
    /// </summary>
    private static readonly HashSet<string> _exemptPages = new(StringComparer.OrdinalIgnoreCase)
    {
        "AdminLogin.razor",
    };

    [Fact]
    public void AllPages_ShouldInheritPageComponentBase()
    {
        // Arrange
        Directory.Exists(_pagesDirectory).Should().BeTrue(
            $"Pages directory should exist at {_pagesDirectory}");

        var razorFiles = Directory.GetFiles(_pagesDirectory, "*.razor", SearchOption.AllDirectories)
            .Where(f => !_exemptPages.Contains(Path.GetFileName(f)))
            .ToList();

        razorFiles.Should().NotBeEmpty("there should be Razor page files to validate");

        // Act & Assert
        var violations = new List<string>();

        foreach (var file in razorFiles)
        {
            var content = File.ReadAllText(file);
            var relativePath = Path.GetRelativePath(_pagesDirectory, file);

            if (!content.Contains("@inherits PageComponentBase"))
            {
                violations.Add(relativePath);
            }
        }

        violations.Should().BeEmpty(
            "every page must inherit PageComponentBase, which enforces the " +
            "markScriptsLoading/markScriptsReady lifecycle for scroll position restoration. " +
            "Missing pages: {0}. See src/TechHub.Web/AGENTS.md for details.",
            string.Join(", ", violations));
    }
}
