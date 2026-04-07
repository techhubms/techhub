namespace TechHub.Web.Helpers;

public enum DiffOp { Equal, Insert, Delete }

public record DiffLine(DiffOp Op, string Text);

/// <summary>
/// Produces line-level and token-level diffs for display in the admin reviews page.
/// Uses a simple LCS (Longest Common Subsequence) algorithm — no external dependencies.
/// </summary>
public static class DiffHelper
{

    /// <summary>
    /// Produces a unified line-level diff between two multi-line strings.
    /// </summary>
    public static IReadOnlyList<DiffLine> DiffLines(string original, string modified)
    {
        ArgumentNullException.ThrowIfNull(original);
        ArgumentNullException.ThrowIfNull(modified);

        var orig = original.ReplaceLineEndings("\n").Split('\n');
        var mod = modified.ReplaceLineEndings("\n").Split('\n');
        return Lcs(orig, mod);
    }

    /// <summary>
    /// Diffs two tags_csv values (e.g. ",AI,GitHub Copilot,.NET,").
    /// Returns which tags were removed, added, or kept unchanged.
    /// </summary>
    public static (IReadOnlyList<string> Removed, IReadOnlyList<string> Added, IReadOnlyList<string> Kept)
        DiffTagsCsv(string originalCsv, string fixedCsv)
    {
        ArgumentNullException.ThrowIfNull(originalCsv);
        ArgumentNullException.ThrowIfNull(fixedCsv);

        var orig = ParseCsv(originalCsv);
        var fixed_ = ParseCsv(fixedCsv);

        var origSet = orig.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var fixedSet = fixed_.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var removed = orig.Where(t => !fixedSet.Contains(t)).ToList();
        var added = fixed_.Where(t => !origSet.Contains(t)).ToList();
        var kept = orig.Where(t => fixedSet.Contains(t)).ToList();

        return (removed, added, kept);
    }

    private static List<string> ParseCsv(string csv) =>
        csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
           .ToList();

    private static List<DiffLine> Lcs(string[] orig, string[] mod)
    {
        int m = orig.Length, n = mod.Length;

        // Build LCS table bottom-up using a jagged array to satisfy CA1814
        var c = new int[m + 1][];
        for (int i = 0; i <= m; i++)
        {
            c[i] = new int[n + 1];
        }

        for (int i = m - 1; i >= 0; i--)
        {
            for (int j = n - 1; j >= 0; j--)
            {
                c[i][j] = orig[i] == mod[j]
                    ? c[i + 1][j + 1] + 1
                    : Math.Max(c[i + 1][j], c[i][j + 1]);
            }
        }

        // Backtrack to produce diff
        var result = new List<DiffLine>(m + n);
        int oi = 0, mi = 0;

        while (oi < m || mi < n)
        {
            if (oi < m && mi < n && orig[oi] == mod[mi])
            {
                result.Add(new DiffLine(DiffOp.Equal, orig[oi]));
                oi++;
                mi++;
            }
            else if (mi < n && (oi >= m || c[oi + 1][mi] <= c[oi][mi + 1]))
            {
                result.Add(new DiffLine(DiffOp.Insert, mod[mi]));
                mi++;
            }
            else
            {
                result.Add(new DiffLine(DiffOp.Delete, orig[oi]));
                oi++;
            }
        }

        return result;
    }
}
