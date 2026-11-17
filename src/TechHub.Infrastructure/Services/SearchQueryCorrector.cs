using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// PostgreSQL implementation of <see cref="ISearchQueryCorrector"/>. A word is unknown when the full-text index has no word that
/// starts with it (the same test as the search itself, which matches word beginnings and expands compound words). The replacement comes from the words of
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
    private readonly ISqlDialect _dialect;

    public SearchQueryCorrector(IDbConnection connection, IMemoryCache cache, ISqlDialect dialect)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(dialect);

        _connection = connection;
        _cache = cache;
        _dialect = dialect;
    }

    [GeneratedRegex("[a-zA-Z0-9]+")]
    private static partial Regex WordPattern();

    /// <inheritdoc/>
    public async Task<string?> CorrectAsync(string query, SearchScope? scope = null, CancellationToken ct = default)
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

        var filter = ScopeFilter.From(scope);
        var known = await FindKnownTermsAsync(terms, filter, ct);
        var unknown = terms.Where(t => !known.Contains(t)).ToList();
        if (unknown.Count == 0)
        {
            return null;
        }

        var vocabulary = await GetVocabularyAsync(ct);

        // The vocabulary also holds words that the search cannot match (tags are not indexed, excerpts are indexed stemmed and
        // the vocabulary spans all articles, not only those of the scope), so a candidate is only used when the same test as for
        // the typed terms finds it within the scope of the search.
        var candidates = unknown.ToDictionary(t => t, t => SearchTermMatcher.FindClosest(t, vocabulary, MaxCandidatesPerTerm), StringComparer.Ordinal);
        var searchable = await FindKnownTermsAsync(candidates.Values.SelectMany(c => c).Distinct(StringComparer.Ordinal).ToList(), filter, ct);
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
    private async Task<HashSet<string>> FindKnownTermsAsync(IReadOnlyList<string> terms, ScopeFilter filter, CancellationToken ct)
    {
        var known = new HashSet<string>(StringComparer.Ordinal);
        var toCheck = new List<string>();
        foreach (var term in terms)
        {
            if (_cache.TryGetValue(KnownTermKey(filter, term), out _))
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

        // The same query transformation and the same filters as the search itself (prefix match plus the expansion of compound
        // words such as "openai" to "open", within the sections, collections and dates of the request), so a term the search
        // finds results for is never called unknown and a term it finds nothing for never known
        var sql = $"""
            SELECT t.term
            FROM unnest(@Terms, @Queries) AS t(term, query)
            WHERE EXISTS (SELECT 1 FROM content_items c WHERE c.search_vector @@ to_tsquery('simple', t.query){filter.Conditions})
            """;
        var checkTerms = toCheck.ToArray();
        var queries = checkTerms.Select(_dialect.TransformFullTextQuery).ToArray();
        var parameters = filter.CreateParameters();
        parameters.Add("Terms", checkTerms);
        parameters.Add("Queries", queries);
        // A result is only cached when no invalidation happened during the lookup, or it could bring back a word whose articles were just removed
        var generation = _cache.GetGeneration();
        var found = await _connection.QueryAsync<string>(new CommandDefinition(sql, parameters, cancellationToken: ct));
        foreach (var term in found)
        {
            known.Add(term);
            _cache.SetIfCurrent(generation, KnownTermKey(filter, term), true, _knownTermTtl);
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

    private static string KnownTermKey(ScopeFilter filter, string term) => $"search:known:{filter.CacheKey}:{term}";

    /// <summary>
    /// The filters of a <see cref="SearchScope"/> as the search applies them (see <c>ContentRepository</c>): the sections by
    /// bitmask, without the cross-section articles, the collections by name and the dates in unix seconds. The dates are
    /// rounded down to five minutes, because a window like "the last 90 days" moves with every request and would otherwise
    /// never share a cache entry.
    /// </summary>
    private sealed record ScopeFilter(int SectionMask, string[]? Collections, long? FromEpoch, long? ToEpoch)
    {
        private const long BucketSeconds = 300;

        /// <summary>Extra <c>AND</c> conditions on <c>content_items c</c>; they contain no user input, only parameter names.</summary>
        public string Conditions
        {
            get
            {
                var sql = new StringBuilder();
                if (SectionMask > 0)
                {
                    sql.Append(" AND (c.sections_bitmask & @SectionMask) > 0 AND c.primary_section_name != 'all'");
                }

                if (Collections is not null)
                {
                    sql.Append(" AND c.collection_name = ANY(@Collections)");
                }

                if (FromEpoch.HasValue)
                {
                    sql.Append(" AND c.date_epoch >= @FromEpoch");
                }

                if (ToEpoch.HasValue)
                {
                    sql.Append(" AND c.date_epoch <= @ToEpoch");
                }

                return sql.ToString();
            }
        }

        public string CacheKey =>
            $"s{SectionMask}|c{(Collections is null ? string.Empty : string.Join(',', Collections))}|f{FromEpoch}|t{ToEpoch}";

        public DynamicParameters CreateParameters()
        {
            var parameters = new DynamicParameters();
            if (SectionMask > 0)
            {
                parameters.Add("SectionMask", SectionMask);
            }

            if (Collections is not null)
            {
                parameters.Add("Collections", Collections);
            }

            if (FromEpoch.HasValue)
            {
                parameters.Add("FromEpoch", FromEpoch.Value);
            }

            if (ToEpoch.HasValue)
            {
                parameters.Add("ToEpoch", ToEpoch.Value);
            }

            return parameters;
        }

        public static ScopeFilter From(SearchScope? scope)
        {
            if (scope is null)
            {
                return new ScopeFilter(0, null, null, null);
            }

            // "all" is no filter, as for the search itself
            var sectionMask = scope.Sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase))
                ? 0
                : SectionBitmasks.For(scope.Sections);
            var collections = scope.Collections.Count == 0 || scope.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase))
                ? null
                : scope.Collections.Select(c => c.ToLowerInvariant().Trim()).Order(StringComparer.Ordinal).ToArray();

            return new ScopeFilter(sectionMask, collections, Bucket(scope.DateFrom), Bucket(scope.DateTo));
        }

        private static long? Bucket(DateTimeOffset? value) =>
            value.HasValue ? value.Value.ToUnixTimeSeconds() / BucketSeconds * BucketSeconds : null;
    }
}
