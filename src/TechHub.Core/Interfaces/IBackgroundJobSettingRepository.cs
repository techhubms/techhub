using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for background job enabled/disabled settings.
/// </summary>
public interface IBackgroundJobSettingRepository
{
    /// <summary>Gets all background job settings.</summary>
    Task<IReadOnlyList<BackgroundJobSetting>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Gets a single background job setting by name, or null if not found.</summary>
    Task<BackgroundJobSetting?> GetByNameAsync(string jobName, CancellationToken ct = default);

    /// <summary>Checks if a specific job is enabled. Returns false if the job is not found.</summary>
    Task<bool> IsEnabledAsync(string jobName, CancellationToken ct = default);

    /// <summary>Updates the enabled state for a job. Returns true if found and updated.</summary>
    Task<bool> SetEnabledAsync(string jobName, bool enabled, CancellationToken ct = default);
}
