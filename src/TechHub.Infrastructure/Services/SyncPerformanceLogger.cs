using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Helper for detailed performance logging during sync operations.
/// </summary>
internal sealed class SyncPerformanceLogger
{
    private readonly ILogger _logger;
    private readonly Stopwatch _operationStopwatch = new();
    private readonly Dictionary<string, long> _metrics = [];

    public SyncPerformanceLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void StartOperation(string operationName)
    {
        _operationStopwatch.Restart();
        _logger.LogInformation("▶️ Starting: {Operation}", operationName);
    }

    public void EndOperation(string operationName, object? additionalInfo = null)
    {
        var elapsed = _operationStopwatch.ElapsedMilliseconds;
        _metrics[operationName] = elapsed;

        if (additionalInfo != null)
        {
            _logger.LogInformation("⏱️ {Operation}: {ElapsedMs}ms - {Info}", operationName, elapsed, additionalInfo);
        }
        else
        {
            _logger.LogInformation("⏱️ {Operation}: {ElapsedMs}ms", operationName, elapsed);
        }
    }

    public void LogSlowOperation(string operation, long thresholdMs, long actualMs, string? context = null)
    {
        if (actualMs > thresholdMs)
        {
            if (context != null)
            {
                _logger.LogWarning("⚠️ Slow {Operation}: {ActualMs}ms (threshold: {ThresholdMs}ms) - {Context}",
                    operation, actualMs, thresholdMs, context);
            }
            else
            {
                _logger.LogWarning("⚠️ Slow {Operation}: {ActualMs}ms (threshold: {ThresholdMs}ms)",
                    operation, actualMs, thresholdMs);
            }
        }
    }

    public void LogBatchProgress(int batchNum, int batchSize, long totalMs, long processingMs, long commitMs)
    {
        var msPerItem = totalMs / (double)batchSize;
        _logger.LogInformation("⏱️ Batch {BatchNum}: {BatchSize} items in {TotalMs}ms (processing: {ProcessingMs}ms, commit: {CommitMs}ms, {MsPerItem:F2}ms/item)",
            batchNum, batchSize, totalMs, processingMs, commitMs, msPerItem);
    }

    public void LogSummary()
    {
        _logger.LogInformation("📊 Performance Summary:");
        foreach (var (operation, duration) in _metrics.OrderByDescending(x => x.Value))
        {
            _logger.LogInformation("  {Operation}: {Duration}ms", operation, duration);
        }
    }
}