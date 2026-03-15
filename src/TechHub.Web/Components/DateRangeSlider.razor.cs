using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TechHub.Web.Components;

/// <summary>
/// Event args for when the date range changes.
/// </summary>
public sealed class DateRangeChangedEventArgs : EventArgs
{
    public DateOnly FromDate { get; init; }
    public DateOnly ToDate { get; init; }
}

/// <summary>
/// Interactive date range slider for filtering content by publication date.
/// Provides dual-handle slider, preset buttons, and callback-driven state.
/// Uses exponential mapping so recent dates have finer granularity.
/// Defaults to last 90 days when no date range is specified.
/// Client-side JS clamping prevents handles from crossing; server-side Math.Clamp is a fallback.
/// Emits date range changes via callback; parent handles URL state.
/// </summary>
public partial class DateRangeSlider : ComponentBase, IAsyncDisposable
{
    // Exponential slider mapping constants.
    // The slider uses fixed integer positions (0..SliderSteps) mapped
    // to dates via a power curve. Recent dates (right side) get finer
    // granularity; older dates (left side) get coarser jumps.
    private const int SliderSteps = 1000;
    private const double Exponent = 2.0;

    [Inject]
    private ILogger<DateRangeSlider> Logger { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Start date of the selected range. If null, defaults to today minus DefaultLastDays.
    /// </summary>
    [Parameter]
    public DateOnly? FromDate { get; set; }

    /// <summary>
    /// End date of the selected range. If null, defaults to today.
    /// </summary>
    [Parameter]
    public DateOnly? ToDate { get; set; }

    /// <summary>
    /// Number of days to default to when no date range is specified (default: 90).
    /// Set to 0 for "All time" (no date filtering).
    /// </summary>
    [Parameter]
    public int DefaultLastDays { get; set; } = 90;

    /// <summary>
    /// Event callback fired when the date range changes (on slider release or preset click).
    /// </summary>
    [Parameter]
    public EventCallback<DateRangeChangedEventArgs> OnDateRangeChanged { get; set; }

    // Internal state: epoch values (day numbers) and slider positions (0..SliderSteps)
    private long _fromEpoch;
    private long _toEpoch;
    private long _minEpoch;
    private long _maxEpoch;
    private int _fromSliderPos;
    private int _toSliderPos;

    // Absolute boundaries for the slider
    private DateOnly _absoluteMinDate;
    private readonly DateOnly _absoluteMaxDate = DateOnly.FromDateTime(DateTime.Today);

    // Track initialization and parameter changes
    private bool _hasInitialized;
    private DateOnly? _previousFromDate;
    private DateOnly? _previousToDate;

    // JS interop for client-side clamping
    private ElementReference _sliderContainer;
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// The current "from" date based on slider position.
    /// </summary>
    public DateOnly CurrentFromDate => EpochToDate(_fromEpoch);

    /// <summary>
    /// The current "to" date based on slider position.
    /// </summary>
    public DateOnly CurrentToDate => EpochToDate(_toEpoch);

    protected override void OnInitialized()
    {
        if (_hasInitialized)
        {
            return;
        }

        _hasInitialized = true;

        // Set absolute min to ~3 years ago to cover historical content
        _absoluteMinDate = _absoluteMaxDate.AddYears(-3);

        _minEpoch = DateToEpoch(_absoluteMinDate);
        _maxEpoch = DateToEpoch(_absoluteMaxDate);

        // Initialize change-tracking fields from current parameter values
        // This prevents false change detection in OnParametersSet
        _previousFromDate = FromDate;
        _previousToDate = ToDate;

        InitializeFromParameters();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && RendererInfo.IsInteractive)
        {
            try
            {
                _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                    "import", "./js/date-range-slider.js");

                if (_jsModule is not null)
                {
                    await _jsModule.InvokeVoidAsync("initClamping", _sliderContainer);
                }
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected, safe to ignore
            }
        }
    }

    protected override void OnParametersSet()
    {
        // Only re-initialize if parameters actually changed
        if (FromDate != _previousFromDate || ToDate != _previousToDate)
        {
            _previousFromDate = FromDate;
            _previousToDate = ToDate;
            InitializeFromParameters();
        }
    }

    private void InitializeFromParameters()
    {
        var today = _absoluteMaxDate;

        var fromDate = FromDate ?? today.AddDays(-DefaultLastDays);
        var toDate = ToDate ?? today;

        // Clamp to valid range
        if (fromDate < _absoluteMinDate)
        {
            fromDate = _absoluteMinDate;
        }

        if (toDate > _absoluteMaxDate)
        {
            toDate = _absoluteMaxDate;
        }

        // Ensure from <= to
        if (fromDate > toDate)
        {
            fromDate = toDate.AddDays(-DefaultLastDays);
            if (fromDate < _absoluteMinDate)
            {
                fromDate = _absoluteMinDate;
            }
        }

        _fromEpoch = DateToEpoch(fromDate);
        _toEpoch = DateToEpoch(toDate);
        _fromSliderPos = EpochToSliderPos(_fromEpoch);
        _toSliderPos = EpochToSliderPos(_toEpoch);

        Logger.LogDebug("DateRangeSlider initialized: {From} to {To}", fromDate, toDate);
    }

    private void HandleFromInput(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newPos))
        {
            _fromSliderPos = Math.Clamp(newPos, 0, _toSliderPos);
            _fromEpoch = SliderPosToEpoch(_fromSliderPos);
        }
    }

    private async Task HandleFromChange(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newPos))
        {
            _fromSliderPos = Math.Clamp(newPos, 0, _toSliderPos);
            _fromEpoch = SliderPosToEpoch(_fromSliderPos);
            await NotifyDateRangeChanged();
        }
    }

    private void HandleToInput(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newPos))
        {
            _toSliderPos = Math.Clamp(newPos, _fromSliderPos, SliderSteps);
            _toEpoch = SliderPosToEpoch(_toSliderPos);
        }
    }

    private async Task HandleToChange(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newPos))
        {
            _toSliderPos = Math.Clamp(newPos, _fromSliderPos, SliderSteps);
            _toEpoch = SliderPosToEpoch(_toSliderPos);
            await NotifyDateRangeChanged();
        }
    }

    private async Task ApplyPreset(int days)
    {
        var today = _absoluteMaxDate;
        _toEpoch = DateToEpoch(today);

        if (days == 0)
        {
            // "All time" - set from to absolute min
            _fromEpoch = _minEpoch;
            _fromSliderPos = 0;
        }
        else
        {
            var fromDate = today.AddDays(-days);
            if (fromDate < _absoluteMinDate)
            {
                fromDate = _absoluteMinDate;
            }

            _fromEpoch = DateToEpoch(fromDate);
            _fromSliderPos = EpochToSliderPos(_fromEpoch);
        }

        _toSliderPos = EpochToSliderPos(_toEpoch);

        Logger.LogDebug("Applied preset: {Days} days, range {From} to {To}",
            days, CurrentFromDate, CurrentToDate);

        await NotifyDateRangeChanged();
    }

    private async Task NotifyDateRangeChanged()
    {
        // Defensive: ensure from <= to even if race conditions let values cross
        if (_fromEpoch > _toEpoch)
        {
            (_fromEpoch, _toEpoch) = (_toEpoch, _fromEpoch);
            (_fromSliderPos, _toSliderPos) = (_toSliderPos, _fromSliderPos);
        }

        var args = new DateRangeChangedEventArgs
        {
            FromDate = CurrentFromDate,
            ToDate = CurrentToDate
        };

        Logger.LogDebug("Date range changed: {From} to {To}", args.FromDate, args.ToDate);

        await OnDateRangeChanged.InvokeAsync(args);
    }

    private bool IsPresetActive(int days)
    {
        var today = _absoluteMaxDate;
        var currentTo = CurrentToDate;

        if (currentTo != today)
        {
            return false;
        }

        if (days == 0)
        {
            return CurrentFromDate == _absoluteMinDate;
        }

        var expectedFrom = today.AddDays(-days);
        return CurrentFromDate == expectedFrom;
    }

    // Helper: Format date for display (e.g., "Feb 10, 2026")
    private static string FormatDate(DateOnly date) =>
        date.ToString("MMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);

    // Helper: Calculate fill bar left position (percentage of slider range).
    // Uses slider positions directly so the fill aligns with thumb positions.
    private string GetFillLeft()
    {
        var left = Math.Min(_fromSliderPos, _toSliderPos);
        var pct = Math.Clamp((double)left / SliderSteps * 100, 0, 100);
        return pct.ToString("F1", CultureInfo.InvariantCulture);
    }

    // Helper: Calculate fill bar width (percentage of slider range).
    // Uses slider positions directly so the fill aligns with thumb positions.
    private string GetFillWidth()
    {
        var pct = Math.Clamp((double)Math.Abs(_toSliderPos - _fromSliderPos) / SliderSteps * 100, 0, 100);
        return pct.ToString("F1", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Maps a slider position (0..SliderSteps) to an epoch value using a power curve.
    /// Formula: epoch = minEpoch + range * (1 - (1-t)^Exponent) where t = pos / SliderSteps.
    /// At pos=0 → minDate (oldest), pos=SliderSteps → maxDate (today).
    /// Near today (right side), the curve flattens → small position changes = small date changes (fine control).
    /// Near minDate (left side), the curve is steep → small position changes = larger date jumps (coarse control).
    /// </summary>
    private long SliderPosToEpoch(int pos)
    {
        var t = (double)pos / SliderSteps;
        var totalRange = _maxEpoch - _minEpoch;
        var epoch = _minEpoch + (long)(totalRange * (1.0 - Math.Pow(1.0 - t, Exponent)));
        return Math.Clamp(epoch, _minEpoch, _maxEpoch);
    }

    /// <summary>
    /// Maps an epoch value to a slider position (0..SliderSteps), inverse of SliderPosToEpoch.
    /// </summary>
    private int EpochToSliderPos(long epoch)
    {
        var totalRange = _maxEpoch - _minEpoch;
        if (totalRange == 0)
        {
            return 0;
        }

        var normalized = Math.Clamp((double)(epoch - _minEpoch) / totalRange, 0.0, 1.0);
        var t = 1.0 - Math.Pow(1.0 - normalized, 1.0 / Exponent);
        return (int)Math.Round(t * SliderSteps);
    }

    // Convert DateOnly to epoch (days since Unix epoch)
    private static long DateToEpoch(DateOnly date) =>
        date.ToDateTime(TimeOnly.MinValue).Ticks / TimeSpan.TicksPerDay;

    // Convert epoch (days since Unix epoch) to DateOnly
    private static DateOnly EpochToDate(long epoch) =>
        DateOnly.FromDateTime(new DateTime(epoch * TimeSpan.TicksPerDay, DateTimeKind.Utc));

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit already disconnected, safe to ignore
            }
        }

        GC.SuppressFinalize(this);
    }
}
