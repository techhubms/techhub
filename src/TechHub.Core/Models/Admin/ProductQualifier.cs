namespace TechHub.Core.Models.Admin;

/// <summary>Kinds of qualifiers: the subscription plans of a product and the surfaces (IDEs, clients) it is available on.</summary>
public static class QualifierKinds
{
    /// <summary>A subscription plan or tier: Free, Pro, Business, Enterprise.</summary>
    public const string Plan = "plan";

    /// <summary>A surface: Visual Studio Code, JetBrains IDEs, GitHub.com.</summary>
    public const string Surface = "surface";

    /// <summary>The valid kinds.</summary>
    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.Ordinal) { Plan, Surface };
}

/// <summary>A plan or surface by id, kind and name.</summary>
public sealed class QualifierReference
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>One of <see cref="QualifierKinds.All"/>.</summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>The canonical qualifier name.</summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>Another name for a <see cref="ProductQualifier"/> (VS Code for Visual Studio Code).</summary>
public sealed class QualifierAlias
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The alternative name.</summary>
    public string Alias { get; set; } = string.Empty;
}

/// <summary>A plan or surface of the vocabulary with its aliases and how often it is used.</summary>
public sealed class ProductQualifier
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>One of <see cref="QualifierKinds.All"/>.</summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>The canonical qualifier name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of timeline events that carry this qualifier.</summary>
    public int EventCount { get; set; }

    /// <summary>Number of articles whose product name implies this qualifier.</summary>
    public int ItemCount { get; set; }

    /// <summary>Other names for this qualifier.</summary>
    public IReadOnlyList<QualifierAlias> Aliases { get; set; } = [];
}

/// <summary>A plan or surface name the AI returned for an event that is not in the vocabulary yet.</summary>
public sealed class UnmatchedQualifierName
{
    /// <summary>One of <see cref="QualifierKinds.All"/>.</summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>The name as the AI returned it.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of timeline events with this name.</summary>
    public int EventCount { get; set; }
}
