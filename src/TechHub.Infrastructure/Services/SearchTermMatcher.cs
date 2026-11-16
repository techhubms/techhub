namespace TechHub.Infrastructure.Services;

/// <summary>A word of the search vocabulary with the number of articles that use it.</summary>
public sealed record SearchVocabularyWord(string Word, int DocumentCount);

/// <summary>
/// Finds the word that a mistyped search term most likely meant, by edit distance (insertions, deletions, substitutions and
/// swaps of two neighbouring letters count as one edit).
/// </summary>
public static class SearchTermMatcher
{
    /// <summary>Terms shorter than this are never corrected: a short word is too close to too many others.</summary>
    public const int MinTermLength = 4;

    /// <summary>The number of edits a term of this length may be from the word it was meant to be.</summary>
    public static int MaxDistance(int termLength) => termLength switch
    {
        < MinTermLength => 0,
        < 8 => 1,
        _ => 2
    };

    /// <summary>
    /// Returns the word of the vocabulary closest to <paramref name="term"/>, or <c>null</c> when none is within reach. Of words
    /// at the same distance the most frequent wins, which is the first one when the vocabulary is ordered by frequency.
    /// </summary>
    public static string? FindClosest(string term, IReadOnlyList<SearchVocabularyWord> vocabulary)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(vocabulary);

        var max = MaxDistance(term.Length);
        if (max == 0)
        {
            return null;
        }

        string? best = null;
        var bestDistance = max + 1;
        foreach (var candidate in vocabulary)
        {
            if (Math.Abs(candidate.Word.Length - term.Length) > max)
            {
                continue;
            }

            var distance = Distance(term, candidate.Word, Math.Min(max, bestDistance - 1));
            if (distance < bestDistance)
            {
                best = candidate.Word;
                bestDistance = distance;
            }
        }

        return best;
    }

    /// <summary>
    /// Returns the words of the vocabulary within reach of <paramref name="term"/>, closest first and, at the same distance, in
    /// vocabulary order (most frequent first), at most <paramref name="count"/> of them.
    /// </summary>
    public static IReadOnlyList<string> FindClosest(string term, IReadOnlyList<SearchVocabularyWord> vocabulary, int count)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(vocabulary);

        var max = MaxDistance(term.Length);
        if (max == 0 || count <= 0)
        {
            return [];
        }

        var candidates = new List<(string Word, int Distance)>();
        foreach (var candidate in vocabulary)
        {
            if (Math.Abs(candidate.Word.Length - term.Length) > max)
            {
                continue;
            }

            var distance = Distance(term, candidate.Word, max);
            if (distance <= max)
            {
                candidates.Add((candidate.Word, distance));
            }
        }

        // OrderBy is stable, so the vocabulary order (frequency) breaks ties
        return candidates.OrderBy(c => c.Distance).Take(count).Select(c => c.Word).ToList();
    }

    /// <summary>
    /// The optimal string alignment distance of two words. Gives up once it is certain to exceed <paramref name="maxDistance"/>
    /// and then returns <c>maxDistance + 1</c>.
    /// </summary>
    public static int Distance(string a, string b, int maxDistance)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (Math.Abs(a.Length - b.Length) > maxDistance)
        {
            return maxDistance + 1;
        }

        var previousPrevious = new int[b.Length + 1];
        var previous = new int[b.Length + 1];
        var current = new int[b.Length + 1];
        for (var j = 0; j <= b.Length; j++)
        {
            previous[j] = j;
        }

        for (var i = 1; i <= a.Length; i++)
        {
            current[0] = i;
            var rowMinimum = current[0];
            for (var j = 1; j <= b.Length; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                var value = Math.Min(Math.Min(previous[j] + 1, current[j - 1] + 1), previous[j - 1] + cost);
                if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                {
                    value = Math.Min(value, previousPrevious[j - 2] + 1);
                }

                current[j] = value;
                rowMinimum = Math.Min(rowMinimum, value);
            }

            if (rowMinimum > maxDistance)
            {
                return maxDistance + 1;
            }

            (previousPrevious, previous, current) = (previous, current, previousPrevious);
        }

        return Math.Min(previous[b.Length], maxDistance + 1);
    }
}
