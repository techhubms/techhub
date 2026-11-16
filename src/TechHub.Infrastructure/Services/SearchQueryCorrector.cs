using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// PostgreSQL implementation of <see cref="ISearchQueryCorrector"/>. A word is unknown when the full-text index has no word that
/// starts with it (the same test as the search itself, which matches word beginnings). The replacement comes from the words of
/// the titles, excerpts and tags, which is where people look for names, as long as the same prefix test finds it. It needs no database extension.
/// </summary>
public sealed partial class SearchQueryCorrector : ISearchQueryCorrector
{
    private const int MaxTermsChecked = 10;
    private const int MaxCandidatesPerTerm = 8;
    private const string VocabularyKey = "search:vocabulary";

    private static readonly TimeSpan _vocabularyTtl = TimeSpan.FromHours(1);
    private static readonly TimeSpan _knownTermTtl = TimeSpan.FromMinutes(30);

    private readonly IDbConnection _connection;
    private readonly IMemoryCache _cache;

    public SearchQueryCorrector(IDbConnection connection, IMemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(cache);

        _connection = connection;
        _cache = cache;
    }

    [GeneratedRegex("[a-zA-Z0-9]+")]
    private static partial Regex WordPattern();

    /// <inheritdoc/>
    public async Task<string?> CorrectAsync(string query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }

        var terms = WordPattern().Matches(query)
            .Select(m => m.Value.ToLowerInvariant())
            .Where(t => t.Length >= SearchTermMatcher.MinTermLength && t.Any(char.IsLetter))
            .Distinct(StringComparer.Ordinal)
            .Take(MaxTermsChecked)
            .ToList();
        if (terms.Count == 0)
        {
            return null;
        }

        var known = await FindKnownTermsAsync(terms, ct);
        var unknown = terms.Where(t => !known.Contains(t)).ToList();
        if (unknown.Count == 0)
        {
            return null;
        }

        var vocabulary = await GetVocabularyAsync(ct);

        // The vocabulary also holds words that the search cannot match (tags are not indexed and excerpts are indexed stemmed),
        // so a candidate is only used when the same prefix test as for the typed terms finds it.
        var candidates = unknown.ToDictionary(t => t, t => SearchTermMatcher.FindClosest(t, vocabulary, MaxCandidatesPerTerm), StringComparer.Ordinal);
        var searchable = await FindKnownTermsAsync(candidates.Values.SelectMany(c => c).Distinct(StringComparer.Ordinal).ToList(), ct);
        var replacements = candidates
            .Select(c => (Term: c.Key, Closest: c.Value.FirstOrDefault(searchable.Contains)))
            .Where(r => r.Closest is not null)
            .ToDictionary(r => r.Term, r => r.Closest!, StringComparer.Ordinal);

        if (replacements.Count == 0)
        {
            return null;
        }

        return WordPattern().Replace(query, m =>
            replacements.TryGetValue(m.Value.ToLowerInvariant(), out var replacement) ? replacement : m.Value);
    }

    // Only terms that were found are cached: the cache can then not grow with every nonsense word a visitor types
    private async Task<HashSet<string>> FindKnownTermsAsync(IReadOnlyList<string> terms, CancellationToken ct)
    {
        var known = new HashSet<string>(StringComparer.Ordinal);
        var toCheck = new List<string>();
        foreach (var term in terms)
        {
            if (_cache.TryGetValue(KnownTermKey(term), out _))
            {
                known.Add(term);
            }
            else
            {
                toCheck.Add(term);
            }
        }

        if (toCheck.Count == 0)
        {
            return known;
        }

        // Terms contain letters and digits only, so they cannot change the tsquery
        const string Sql = """
            SELECT t.term
            FROM unnest(@Terms) AS t(term)
            WHERE EXISTS (SELECT 1 FROM content_items c WHERE c.search_vector @@ to_tsquery('simple', t.term || ':*'))
            """;
        // A result is only cached when no invalidation happened during the lookup, or it could bring back a word whose articles were just removed
        var generation = _cache.GetGeneration();
        var found = await _connection.QueryAsync<string>(
            new CommandDefinition(Sql, new { Terms = toCheck.ToArray() }, cancellationToken: ct));
        foreach (var term in found)
        {
            known.Add(term);
            _cache.SetIfCurrent(generation, KnownTermKey(term), true, _knownTermTtl);
        }

        return known;
    }

    // Most frequent first, so that of two equally close words the more frequent one is chosen
    private async Task<IReadOnlyList<SearchVocabularyWord>> GetVocabularyAsync(CancellationToken ct)
    {
        return await _cache.GetOrCreateSingleFlightAsync(VocabularyKey, async entry =>
        {
            entry.SetAbsoluteExpiration(_vocabularyTtl);

            const string Sql = """
                SELECT word AS Word, ndoc AS DocumentCount
                FROM ts_stat($$SELECT to_tsvector('simple', coalesce(title, '') || ' ' || coalesce(excerpt, '') || ' ' || coalesce(tags_csv, '')) FROM content_items$$)
                WHERE word ~ '^[a-z0-9]{4,30}$' AND ndoc >= 2
                ORDER BY ndoc DESC, word
                """;
            var words = await _connection.QueryAsync<SearchVocabularyWord>(new CommandDefinition(Sql, cancellationToken: ct));
            return (IReadOnlyList<SearchVocabularyWord>)words.ToList();
        }, ct);
    }

    private static string KnownTermKey(string term) => $"search:known:{term}";
}
