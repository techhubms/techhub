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
/// the titles, excerpts and tags, which is where people look for names. It needs no database extension.
/// </summary>
public sealed partial class SearchQueryCorrector : ISearchQueryCorrector
{
    private const int MaxTermsChecked = 10;
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
        var replacements = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var term in unknown)
        {
            if (SearchTermMatcher.FindClosest(term, vocabulary) is { } closest)
            {
                replacements[term] = closest;
            }
        }

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
        var found = await _connection.QueryAsync<string>(
            new CommandDefinition(Sql, new { Terms = toCheck.ToArray() }, cancellationToken: ct));
        foreach (var term in found)
        {
            known.Add(term);
            _cache.Set(KnownTermKey(term), true, _knownTermTtl);
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
