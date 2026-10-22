using System.Text.Json.Nodes;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// The strict JSON schema (<c>response_format</c>) of the classification step.
/// Strict mode requires every property to be listed in <c>required</c>; fields that do not apply to excluded
/// items are nullable or empty arrays instead.
/// </summary>
internal static class ClassificationSchema
{
    /// <summary>
    /// Builds the <c>response_format</c> object. A new instance is returned for every call because
    /// <see cref="JsonNode"/> instances can only have a single parent.
    /// </summary>
    /// <param name="sectionNames">The section slugs the AI may return, taken from the configured sections.</param>
    public static JsonObject BuildResponseFormat(IReadOnlyList<string> sectionNames)
    {
        ArgumentNullException.ThrowIfNull(sectionNames);

        var sections = StringEnum(sectionNames);
        var nullableSections = StringEnum(sectionNames, includeNull: true);
        var significance = StringEnum(RoundupMetadata.ValidSignificance.Order(StringComparer.Ordinal), includeNull: true);
        var contentKinds = StringEnum(RoundupMetadata.ValidContentKinds.Order(StringComparer.Ordinal), includeNull: true);
        var lifecycles = StringEnum(RoundupMetadata.ValidLifecycles.Order(StringComparer.Ordinal), includeNull: true);
        var roles = StringEnum([RoundupMetadata.RoleLead, RoundupMetadata.RoleBrief]);
        var timelineEvents = TimelineEventsSchema();

        var json = $$"""
            {
              "type": "json_schema",
              "json_schema": {
                "name": "content_classification",
                "strict": true,
                "schema": {
                  "type": "object",
                  "additionalProperties": false,
                  "required": ["included", "explanation", "title", "author", "sections", "primary_section", "tags", "excerpt", "section_roles", "key_facts", "significance", "content_kind", "lifecycle", "products", "timeline_events"],
                  "properties": {
                    "included": { "type": "boolean" },
                    "explanation": { "type": "string" },
                    "title": { "type": ["string", "null"] },
                    "author": { "type": ["string", "null"] },
                    "sections": { "type": "array", "items": { "type": "string", "enum": {{sections}} } },
                    "primary_section": { "type": ["string", "null"], "enum": {{nullableSections}} },
                    "tags": { "type": "array", "items": { "type": "string" } },
                    "excerpt": { "type": ["string", "null"] },
                    "section_roles": {
                      "type": "array",
                      "items": {
                        "type": "object",
                        "additionalProperties": false,
                        "required": ["section", "role"],
                        "properties": {
                          "section": { "type": "string", "enum": {{sections}} },
                          "role": { "type": "string", "enum": {{roles}} }
                        }
                      }
                    },
                    "key_facts": { "type": "array", "items": { "type": "string" } },
                    "significance": { "type": ["string", "null"], "enum": {{significance}} },
                    "content_kind": { "type": ["string", "null"], "enum": {{contentKinds}} },
                    "lifecycle": { "type": ["string", "null"], "enum": {{lifecycles}} },
                    "products": { "type": "array", "items": { "type": "string" } },
                    "timeline_events": {{timelineEvents}}
                  }
                }
              }
            }
            """;

        return JsonNode.Parse(json)!.AsObject();
    }

    /// <summary>
    /// Builds the <c>response_format</c> of the timeline-only extraction used by the backfill job.
    /// </summary>
    public static JsonObject BuildTimelineResponseFormat()
    {
        var json = $$"""
            {
              "type": "json_schema",
              "json_schema": {
                "name": "timeline_extraction",
                "strict": true,
                "schema": {
                  "type": "object",
                  "additionalProperties": false,
                  "required": ["timeline_events"],
                  "properties": {
                    "timeline_events": {{TimelineEventsSchema()}}
                  }
                }
              }
            }
            """;

        return JsonNode.Parse(json)!.AsObject();
    }

    private static string TimelineEventsSchema()
    {
        var eventKinds = StringEnum(TimelineEvent.ValidKinds.Order(StringComparer.Ordinal));
        var precisions = StringEnum(TimelineEvent.ValidPrecisions.Order(StringComparer.Ordinal), includeNull: true);
        var dateSources = StringEnum([TimelineEvent.DateSourceExplicit, TimelineEvent.DateSourcePublication]);

        return $$"""
            {
              "type": "array",
              "items": {
                "type": "object",
                "additionalProperties": false,
                "required": ["kind", "product", "feature", "description", "date_source", "date", "date_precision", "quote"],
                "properties": {
                  "kind": { "type": "string", "enum": {{eventKinds}} },
                  "product": { "type": "string" },
                  "feature": { "type": ["string", "null"] },
                  "description": { "type": "string" },
                  "date_source": { "type": "string", "enum": {{dateSources}} },
                  "date": { "type": ["string", "null"] },
                  "date_precision": { "type": ["string", "null"], "enum": {{precisions}} },
                  "quote": { "type": ["string", "null"] }
                }
              }
            }
            """;
    }

    private static string StringEnum(IEnumerable<string> values, bool includeNull = false)
    {
        var array = new JsonArray();
        foreach (var value in values)
        {
            array.Add(value);
        }

        if (includeNull)
        {
            array.Add(null);
        }

        return array.ToJsonString();
    }
}
