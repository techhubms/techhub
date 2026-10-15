using System.Buffers;

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

        if (original.Length == 0 && modified.Length == 0)
        {
            return [new DiffLine(DiffOp.Equal, string.Empty)];
        }

        var orig = original.Length == 0 ? Array.Empty<string>() : original.ReplaceLineEndings("\n").Split('\n');
        var mod = modified.Length == 0 ? Array.Empty<string>() : modified.ReplaceLineEndings("\n").Split('\n');
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
        var matches = new List<(int Original, int Modified)>();
        FindLcsMatches(orig, 0, m, mod, 0, n, matches);

        var result = new List<DiffLine>(m + n);
        int oi = 0, mi = 0;

        foreach (var (originalIndex, modifiedIndex) in matches)
        {
            while (mi < modifiedIndex)
            {
                result.Add(new DiffLine(DiffOp.Insert, mod[mi++]));
            }

            while (oi < originalIndex)
            {
                result.Add(new DiffLine(DiffOp.Delete, orig[oi++]));
            }

            result.Add(new DiffLine(DiffOp.Equal, orig[oi]));
            oi++;
            mi++;
        }

        while (mi < n)
        {
            result.Add(new DiffLine(DiffOp.Insert, mod[mi++]));
        }

        while (oi < m)
        {
            result.Add(new DiffLine(DiffOp.Delete, orig[oi++]));
        }

        return result;
    }

    private static void FindLcsMatches(
        string[] orig,
        int origStart,
        int origLength,
        string[] mod,
        int modStart,
        int modLength,
        List<(int Original, int Modified)> matches)
    {
        if (origLength == 0 || modLength == 0)
        {
            return;
        }

        if (origLength == 1)
        {
            for (int mi = modStart; mi < modStart + modLength; mi++)
            {
                if (orig[origStart] == mod[mi])
                {
                    matches.Add((origStart, mi));
                    break;
                }
            }

            return;
        }

        if (modLength == 1)
        {
            for (int oi = origStart; oi < origStart + origLength; oi++)
            {
                if (orig[oi] == mod[modStart])
                {
                    matches.Add((oi, modStart));
                    break;
                }
            }

            return;
        }

        if (origLength >= modLength)
        {
            int leftLength = origLength / 2;
            int modSplit = FindSplit(orig, origStart, leftLength, origLength - leftLength, mod, modStart, modLength);
            FindLcsMatches(orig, origStart, leftLength, mod, modStart, modSplit, matches);
            FindLcsMatches(orig, origStart + leftLength, origLength - leftLength, mod, modStart + modSplit, modLength - modSplit, matches);
        }
        else
        {
            int leftLength = modLength / 2;
            int origSplit = FindSplit(mod, modStart, leftLength, modLength - leftLength, orig, origStart, origLength);
            FindLcsMatches(orig, origStart, origSplit, mod, modStart, leftLength, matches);
            FindLcsMatches(orig, origStart + origSplit, origLength - origSplit, mod, modStart + leftLength, modLength - leftLength, matches);
        }
    }

    private static int FindSplit(
        string[] splitLines,
        int splitStart,
        int leftLength,
        int rightLength,
        string[] rangeLines,
        int rangeStart,
        int rangeLength)
    {
        var forward = ArrayPool<int>.Shared.Rent(rangeLength + 1);
        var backward = ArrayPool<int>.Shared.Rent(rangeLength + 1);

        try
        {
            Array.Clear(forward, 0, rangeLength + 1);
            for (int i = splitStart; i < splitStart + leftLength; i++)
            {
                int diagonal = 0;
                for (int j = 1; j <= rangeLength; j++)
                {
                    int previous = forward[j];
                    forward[j] = splitLines[i] == rangeLines[rangeStart + j - 1]
                        ? diagonal + 1
                        : Math.Max(forward[j], forward[j - 1]);
                    diagonal = previous;
                }
            }

            Array.Clear(backward, 0, rangeLength + 1);
            for (int i = splitStart + leftLength + rightLength - 1; i >= splitStart + leftLength; i--)
            {
                int diagonal = 0;
                for (int j = rangeLength - 1; j >= 0; j--)
                {
                    int previous = backward[j];
                    backward[j] = splitLines[i] == rangeLines[rangeStart + j]
                        ? diagonal + 1
                        : Math.Max(backward[j], backward[j + 1]);
                    diagonal = previous;
                }
            }

            int bestSplit = 0;
            int bestLength = -1;
            for (int j = 0; j <= rangeLength; j++)
            {
                int length = forward[j] + backward[j];
                if (length > bestLength)
                {
                    bestSplit = j;
                    bestLength = length;
                }
            }

            return bestSplit;
        }
        finally
        {
            ArrayPool<int>.Shared.Return(forward);
            ArrayPool<int>.Shared.Return(backward);
        }
    }
}
