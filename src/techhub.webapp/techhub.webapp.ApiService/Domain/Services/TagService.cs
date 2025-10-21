namespace techhub.webapp.ApiService.Domain.Services;

using techhub.webapp.ApiService.Domain.Interfaces;
using techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Implementation of tag processing logic.
/// Handles tag normalization, subset matching, and relationship building.
/// </summary>
public class TagService : ITagService
{
    public string NormalizeTag(string tag)
    {
        return Tag.NormalizeTag(tag);
    }

    public IEnumerable<Tag> GetRelatedTags(string tag, IEnumerable<Tag> allTags)
    {
        var normalizedTag = Tag.NormalizeTag(tag);
        
        return allTags.Where(t =>
        {
            var normalizedCandidateTag = Tag.NormalizeTag(t.Display);
            return Tag.IsSubsetOf(normalizedTag, normalizedCandidateTag) ||
                   Tag.IsSubsetOf(normalizedCandidateTag, normalizedTag);
        });
    }

    public Dictionary<string, List<int>> BuildTagRelationships(IEnumerable<ContentItem> items)
    {
        var relationships = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        var itemsList = items.ToList();

        // Build relationships for all tags found in content
        for (int i = 0; i < itemsList.Count; i++)
        {
            var item = itemsList[i];
            foreach (var itemTag in item.TagsNormalized)
            {
                var normalizedTag = Tag.NormalizeTag(itemTag);
                
                if (!relationships.ContainsKey(normalizedTag))
                    relationships[normalizedTag] = new List<int>();

                if (!relationships[normalizedTag].Contains(i))
                    relationships[normalizedTag].Add(i);
            }
        }

        return relationships;
    }

    public Dictionary<string, int> GetTagCounts(IEnumerable<ContentItem> items)
    {
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            foreach (var tag in item.Tags)
            {
                if (counts.ContainsKey(tag))
                    counts[tag]++;
                else
                    counts[tag] = 1;
            }
        }

        return counts;
    }

    public async Task<IEnumerable<Tag>> GetMostUsedTagsAsync(IEnumerable<ContentItem> items, int count)
    {
        var tagCounts = GetTagCounts(items);

        var tags = tagCounts
            .OrderByDescending(x => x.Value)
            .Take(count)
            .Select(x => new Tag
            {
                Display = x.Key,
                Count = x.Value,
                FirstSeen = items
                    .Where(item => item.Tags.Contains(x.Key, StringComparer.OrdinalIgnoreCase))
                    .Min(item => item.PublishedDate)
            });

        return await Task.FromResult(tags);
    }

    public bool ValidateTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return false;

        // Tag should not be too long
        if (tag.Length > 100)
            return false;

        // Tag should contain at least one letter or number
        return tag.Any(char.IsLetterOrDigit);
    }

    public bool IsSubsetOf(string subsetTag, string fullTag)
    {
        return Tag.IsSubsetOf(subsetTag, fullTag);
    }
}
