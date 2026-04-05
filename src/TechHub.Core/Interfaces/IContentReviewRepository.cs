using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for content review queue — tracks fixer changes awaiting admin approval.
/// </summary>
public interface IContentReviewRepository
{
    /// <summary>Creates a new review record for a proposed content change.</summary>
    Task<long> CreateAsync(string slug, string collectionName, string changeType,
        string originalValue, string fixedValue, long? jobId = null, CancellationToken ct = default);

    /// <summary>Gets a specific review by ID.</summary>
    Task<ContentReview?> GetByIdAsync(long id, CancellationToken ct = default);

    /// <summary>Gets reviews filtered by status, newest first.</summary>
    Task<IReadOnlyList<ContentReview>> GetByStatusAsync(string? status = null, int limit = 100, CancellationToken ct = default);

    /// <summary>Gets summary counts by status.</summary>
    Task<ContentReviewSummary> GetSummaryAsync(CancellationToken ct = default);

    /// <summary>Approves a single review and applies the change to the content item.</summary>
    Task<bool> ApproveAsync(long id, CancellationToken ct = default);

    /// <summary>Rejects a single review (marks as rejected, does not modify content).</summary>
    Task<bool> RejectAsync(long id, CancellationToken ct = default);

    /// <summary>Approves all pending reviews and applies changes to content items.</summary>
    Task<int> ApproveAllAsync(CancellationToken ct = default);

    /// <summary>Rejects all pending reviews.</summary>
    Task<int> RejectAllAsync(CancellationToken ct = default);

    /// <summary>Updates the fixed value of a pending review (admin manual edit).</summary>
    Task<bool> UpdateFixedValueAsync(long id, string fixedValue, CancellationToken ct = default);

    /// <summary>Deletes reviews older than the specified cutoff that are not pending.</summary>
    Task<int> PurgeOldReviewsAsync(int keepDays = 30, CancellationToken ct = default);
}
