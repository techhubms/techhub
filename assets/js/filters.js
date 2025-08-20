// =============================================================================
// GLOBAL STATE AND CONFIGURATION
// =============================================================================

// === WINDOW SCOPE VARIABLES (accessible globally) ===
window.activeFilters = new Set();
window.collapsedTagsVisible = false;
window.isUpdating = false; // Centralized mutex for all filter operations (accessible globally)
window.dateFilterMappings = {}; // Pre-calculated date filter mappings (dateFilter -> Set of post indices) - set by initializeTagFilter
window.dateFilterConfig = {}; // Date filter configuration from Jekyll - set by initializeTagFilter
window.tagRelationships = {}; // Tag relationships for optimized filtering (tag -> item indices mapping) - set by initializeTagFilter
window.cachedCountSpans = new Map(); // Cache for filter button count span elements (tag -> DOM element mapping)
window.dateFilters = []; // Array of date filter labels
window.currentFilterData = []; // Array of post data from Jekyll
window.textSearchQuery = ''; // Current text search query

// === LOCAL VARIABLES (file scope only) ===
let cachedPosts = null;
let cachedDateButtons = null;
let cachedNonDateButtons = null;
let cachedHiddenTagButtons = null;
let cachedTextSearchInput = null;
let cachedTextSearchClearBtn = null;

// =============================================================================
// INITIALIZATION FUNCTIONS
// =============================================================================

// Data initialization - runs immediately with parameters
function initializeTagFilter(filterData, relationships, dateConfig) {
    // Validate required parameters
    if (!Array.isArray(filterData)) {
        throw new Error('initializeTagFilter: filterData must be a non-empty array');
    }

    if (relationships !== null && typeof relationships !== 'object') {
        throw new Error('initializeTagFilter: relationships must be an object or null');
    }

    if (!Array.isArray(dateConfig)) {
        throw new Error('initializeTagFilter: dateConfig must be an array');
    }

    // Store tag relationships for optimized filtering
    window.tagRelationships = relationships || {};

    // Store current data
    window.currentFilterData = filterData;

    // Set up date filter configuration from Jekyll config
    window.dateFilterConfig = {};
    window.dateFilters = [];

    dateConfig.forEach(config => {
        if (typeof config === 'string' && config.includes(':')) {
            const [label, days] = config.split(':');
            window.dateFilterConfig[label] = parseInt(days, 10);
            window.dateFilters.push(label.toLowerCase());
        }
    });

    // Pre-calculate date filter mappings for ultra-fast filtering
    preCalculateDateFilters(window.currentFilterData);

    // Convert filter data to include normalized tag sets
    window.currentFilterData.forEach(item => {
        if (item.tags_normalized && Array.isArray(item.tags_normalized)) {
            item.tagSet = new Set(item.tags_normalized);
        }
    });

    // Parse URL filters and apply them
    parseURLFilters();

    // Initialize DOM when ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeDOM);
    } else {
        initializeDOM();
    }
}

function initializeDOM() {
    // Initialize DOM cache
    initDOMCache();

    // Set up centralized event delegation
    setupEventDelegation();

    //Make sure the tags are shown expanded if needed
    updateCollapsedTagsVisibility();

    // Always update display to initialize button counts, regardless of active filters
    updateDisplay(true);
}

// Initialize DOM cache
function initDOMCache() {
    if (!cachedPosts) {
        const postElements = document.querySelectorAll('.navigation-post-square');
        cachedPosts = Array.from(postElements).map((el, index) => ({
            el: el,
            index: index  // Add index for tag relationship lookups
        }));
    }

    if (!cachedDateButtons) {
        cachedDateButtons = document.querySelectorAll('.date-filter-btn');

        // Cache count spans for date buttons
        cachedDateButtons.forEach(btn => {
            const tag = btn.getAttribute('data-tag');
            const countSpan = btn.querySelector('.filter-count');
            if (countSpan && tag) {
                window.cachedCountSpans.set(tag, countSpan);
            }
        });
    }

    if (!cachedNonDateButtons) {
        cachedNonDateButtons = document.querySelectorAll('.tag-filter-btn[data-filter-type="tag"]');

        // Cache count spans for non-date buttons
        cachedNonDateButtons.forEach(btn => {
            const tag = btn.getAttribute('data-tag');
            const countSpan = btn.querySelector('.filter-count');
            if (countSpan && tag) {
                window.cachedCountSpans.set(tag, countSpan);
            }
        });
    }

    if (!cachedHiddenTagButtons) {
        cachedHiddenTagButtons = document.querySelectorAll('.hidden-tag-btn');
    }

    if (!cachedTextSearchInput) {
        cachedTextSearchInput = document.getElementById('text-search-input');
    }

    if (!cachedTextSearchClearBtn) {
        cachedTextSearchClearBtn = document.getElementById('text-search-clear');
    }
}

// Pre-calculate date filter mappings for all posts
function preCalculateDateFilters(filterData) {
    window.dateFilterMappings = {};

    // Calculate current epoch dynamically in browser using visitor's local timezone
    const now = new Date();

    // Calculate today start in visitor's local timezone for better user experience
    // Create a date in the user's local timezone and get midnight (start of day)
    const today = new Date(now);
    today.setHours(0, 0, 0, 0);
    const todayStart = Math.floor(today.getTime() / 1000);
    const todayEnd = todayStart + 86399; // End of today (23:59:59)

    // Pre-calculate for each date filter
    Object.entries(window.dateFilterConfig).forEach(([dateFilter, daysBack]) => {
        const postIndices = new Set();

        filterData.forEach((item, i) => {
            const postEpoch = item?.epoch || 0;
            let isWithinFilter = false;

            if (daysBack === 0) {
                // Today only
                isWithinFilter = postEpoch >= todayStart && postEpoch <= todayEnd;
            } else {
                // "Last N days" means today + (N-1) days back
                // So "last 7 days" = today + 6 days back = 7 days total
                const daysInSeconds = (daysBack - 1) * 86400;
                const cutoffEpoch = todayStart - daysInSeconds;
                isWithinFilter = postEpoch >= cutoffEpoch;
            }

            if (isWithinFilter) postIndices.add(i);
        });

        // Use lowercase key to match data-tag attributes
        const lowercaseKey = dateFilter.toLowerCase();
        window.dateFilterMappings[lowercaseKey] = postIndices;
    });
}

// Parse URL parameters and restore filters
function parseURLFilters() {
    const urlParams = new URLSearchParams(window.location.search);
    const filters = urlParams.get('filters');

    if (filters) {
        const decodedFilters = decodeURIComponent(filters);
        decodedFilters.split(',').forEach(filter => {
            if (filter) {
                window.activeFilters.add(filter);
            }
        });
    }

    const searchQuery = urlParams.get('search');
    if (searchQuery && cachedTextSearchInput) {
        window.textSearchQuery = decodeURIComponent(searchQuery);
        cachedTextSearchInput.value = window.textSearchQuery;
    }

    const showCollapsed = urlParams.get('showCollapsed');
    if (showCollapsed === 'true') {
        window.collapsedTagsVisible = true;
    }
}

// =============================================================================
// CENTRALIZED EVENT DELEGATION
// =============================================================================

function setupEventDelegation() {
    // Collapsed event handler for all filter interactions
    document.addEventListener('click', handleFilterClick);
    
    // Text search event handlers
    if (cachedTextSearchInput) {
        cachedTextSearchInput.addEventListener('input', handleTextSearchInput);
        cachedTextSearchInput.addEventListener('keydown', handleTextSearchKeydown);
    }
    
    if (cachedTextSearchClearBtn) {
        cachedTextSearchClearBtn.addEventListener('click', handleTextSearchClear);
    }
}

function handleFilterClick(event) {
    // Centralized mutex check - prevents all overlapping filter operations
    if (window.isUpdating) {
        event.preventDefault();
        return;
    }

    const target = event.target;
    let actionToExecute = null;

    // Check if this is a click we should handle (but don't execute yet)
    if (target.id === 'tags-clear-btn') {
        actionToExecute = () => executeClearFilters();
    } else if (target.id === 'toggle-collapsed-tags') {
        actionToExecute = () => executeToggleCollapsedTags();
    } else if (target.matches('.tag-filter-btn')) {
        const tag = target.getAttribute('data-tag');
        const filterType = target.getAttribute('data-filter-type');
        if (tag && filterType) {
            actionToExecute = () => executeFilterToggle(tag, filterType);
        }
    } else {
        // Check if we clicked on a child element of a button
        const parentButton = target.closest('.tag-filter-btn');
        if (parentButton) {
            // Handle parent button actions
            if (parentButton.id === 'tags-clear-btn') {
                actionToExecute = () => executeClearFilters();
            } else if (parentButton.id === 'toggle-collapsed-tags') {
                actionToExecute = () => executeToggleCollapsedTags();
            } else if (parentButton.matches('.tag-filter-btn')) {
                const tag = parentButton.getAttribute('data-tag');
                const filterType = parentButton.getAttribute('data-filter-type');
                if (tag && filterType) {
                    actionToExecute = () => executeFilterToggle(tag, filterType);
                }
            }
        }
    }

    // Only set mutex and execute if we're handling this click
    if (actionToExecute) {
        event.preventDefault();
        window.isUpdating = true;

        try {
            actionToExecute();
        } finally {
            // Always release mutex, even if an error occurs
            window.isUpdating = false;
        }
    }
}

// =============================================================================
// TEXT SEARCH EVENT HANDLERS
// =============================================================================

function handleTextSearchInput(event) {
    if (window.isUpdating) return;
    
    const query = event.target.value.trim();
    window.textSearchQuery = query;
    
    // Update clear button visibility
    updateTextSearchClearButton();
    
    // Debounce the search to avoid excessive filtering
    clearTimeout(window.textSearchTimeout);
    window.textSearchTimeout = setTimeout(() => {
        if (!window.isUpdating) {
            window.isUpdating = true;
            try {
                updateDisplay();
                updateURL();
            } finally {
                window.isUpdating = false;
            }
        }
    }, 250);
}

function handleTextSearchKeydown(event) {
    if (event.key === 'Escape') {
        clearTextSearch();
    }
}

function handleTextSearchClear(event) {
    if (window.isUpdating) {
        event.preventDefault();
        return;
    }
    
    clearTextSearch();
}

function clearTextSearch() {
    if (cachedTextSearchInput) {
        cachedTextSearchInput.value = '';
        window.textSearchQuery = '';
        updateTextSearchClearButton();
        
        window.isUpdating = true;
        try {
            updateDisplay();
            updateURL();
        } finally {
            window.isUpdating = false;
        }
    }
}

function updateTextSearchClearButton() {
    if (cachedTextSearchClearBtn) {
        if (window.textSearchQuery && window.textSearchQuery.trim()) {
            cachedTextSearchClearBtn.classList.remove('hidden');
        } else {
            cachedTextSearchClearBtn.classList.add('hidden');
        }
    }
}

// Text search filtering function - searches directly on DOM content
function passesTextSearch(postData, postIndex) {
    if (!window.textSearchQuery) return true;
    
    const query = window.textSearchQuery.toLowerCase();
    
    // Search in DOM content for this post
    if (postIndex < cachedPosts.length) {
        const postElement = cachedPosts[postIndex].el;  // Access the 'el' property
        
        // Search in title
        const titleElement = postElement.querySelector('.navigation-post-title');
        if (titleElement && titleElement.textContent.toLowerCase().includes(query)) {
            return true;
        }
        
        // Search in description/excerpt
        const descElement = postElement.querySelector('.navigation-post-desc');
        if (descElement && descElement.textContent.toLowerCase().includes(query)) {
            return true;
        }
        
        // Search in author/feed name and date
        const metaElements = postElement.querySelectorAll('.navigation-post-meta-value');
        for (const metaElement of metaElements) {
            if (metaElement.textContent.toLowerCase().includes(query)) {
                return true;
            }
        }
        
        // Search in tags from data attribute (more efficient than server data)
        const tagsAttribute = postElement.getAttribute('data-tags');
        if (tagsAttribute && tagsAttribute.toLowerCase().includes(query)) {
            return true;
        }
    }
    
    return false;
}

// =============================================================================
// CORE FILTERING LOGIC
// =============================================================================

// Shared display update logic
function updateDisplay(initialPageLoad = false) {
    const activeDateFilter = Array.from(window.activeFilters).find(filter => window.dateFilters.includes(filter));
    const modeFilters = Array.from(window.activeFilters).filter(filter => !window.dateFilters.includes(filter));

    // Optimization: On initial page load with no active filters, only update date button counts
    if (initialPageLoad && window.activeFilters.size === 0) {
        // Only update date button counts - skip all other processing for performance
        updateDateButtonCountsAndState();
        return;
    }

    const visibilityUpdates = [];
    const visiblePosts = [];

    for (let i = 0; i < cachedPosts.length; i++) {
        const post = cachedPosts[i];
        const postData = window.currentFilterData[i];

        let isVisible;

        // If no filters are active and no text search, show all posts
        if (window.activeFilters.size === 0 && !window.textSearchQuery) {
            isVisible = true;
        } else {
            const passesModeFilter = modeFilters.length === 0 || checkFilterForCurrentMode(i, modeFilters);
            const passesDateFilter = !activeDateFilter || isWithinDateFilter(i, activeDateFilter);
            const passesTextFilter = passesTextSearch(postData);
            isVisible = passesModeFilter && passesDateFilter && passesTextFilter;
        }

        visibilityUpdates.push({ element: post.el, visible: isVisible });

        if (isVisible) {
            // Store visible posts for optimized button counting
            visiblePosts.push({ post, postData });
        }
    }

    // Apply visibility updates
    visibilityUpdates.forEach(({ element, visible }) => {
        element.style.display = visible ? '' : 'none';
    });

    updateNonDateButtonsCountsAndState(visiblePosts);

    // Efficiently update date button counts using pre-calculated mappings
    updateDateButtonCountsAndState();
    
    // Update text search clear button visibility
    updateTextSearchClearButton();
}

// Efficiently update date button counts using pre-calculated date filter mappings
function updateDateButtonCountsAndState() {
    const modeFilters = Array.from(window.activeFilters).filter(filter => !window.dateFilters.includes(filter));

    // Get date filters in config order (not just keys)
    const dateFilterOrder = Object.entries(window.dateFilterConfig)
        .sort((a, b) => a[1] - b[1]) // sort by daysBack ascending
        .map(([label]) => label.toLowerCase());

    // Step 1: Determine base set of buttons to show (raw counts, 50% rule, hide 0s)
    let prevCount = 0;
    let firstShown = false;
    const baseSet = [];

    dateFilterOrder.forEach(dateFilter => {
        const datePostIndices = window.dateFilterMappings[dateFilter];

        const dateButton = Array.from(cachedDateButtons).find(btn => btn.getAttribute('data-tag') === dateFilter);
        if (!dateButton) return;

        const countSpan = window.cachedCountSpans.get(dateFilter);
        if (!countSpan) return;

        const rawCount = datePostIndices.size;

        // Hide if count is 0
        if (rawCount === 0) {
            dateButton.style.display = 'none';
            dateButton.disabled = true;
            dateButton.classList.add('disabled');
            return;
        }

        // 50% increase rule: Only show if count >= max(prevCount * 1.5, prevCount + 1), except always show the first eligible filter
        let showButton = false;
        if (!firstShown) {
            showButton = true;
            firstShown = true;
        } else {
            const baseThreshold = Math.ceil(prevCount * 1.5);
            const minThreshold = prevCount + 1;
            const fiftyPercentThreshold = Math.max(baseThreshold, minThreshold);

            if (rawCount >= fiftyPercentThreshold) {
                showButton = true;
            }
        }

        if (showButton) {
            baseSet.push({ dateFilter, dateButton, countSpan, datePostIndices });
            prevCount = rawCount;
        } else {
            dateButton.style.display = 'none';
            dateButton.disabled = true;
            dateButton.classList.add('disabled');
        }
    });

    // Step 2: If filters, recalculate counts for base set only
    const filteredCounts = {};
    if (modeFilters.length > 0) {
        baseSet.forEach(({ dateFilter, datePostIndices }) => {
            const filteredCount = [...datePostIndices].filter(index => {
                return checkFilterForCurrentMode(index, modeFilters);
            }).length;
            filteredCounts[dateFilter] = filteredCount;
        });
    }

    // Step 3: For all base set buttons, update count, state, and visibility
    baseSet.forEach(({ dateFilter, dateButton, countSpan, datePostIndices }) => {
        const count = modeFilters.length > 0 ? filteredCounts[dateFilter] : datePostIndices.size;
        // Update count display
        countSpan.textContent = `(${count})`;

        // Update button state
        const isActive = window.activeFilters.has(dateFilter);
        const shouldBeEnabled = count > 0 || isActive;

        dateButton.classList.toggle('active', isActive);
        dateButton.disabled = !shouldBeEnabled;
        dateButton.classList.toggle('disabled', !shouldBeEnabled);
    });
}

// Unified button state updates using mode configuration (non-date buttons only)
function updateNonDateButtonsCountsAndState(visiblePosts) {
    // Use cached non-date filter buttons only

    // Use pre-calculated visiblePosts instead of recalculating
    cachedNonDateButtons.forEach(btn => {
        const tag = btn.getAttribute('data-tag');
        if (!tag) return;

        let count = 0;

        if (window.tagRelationships && window.tagRelationships[tag]) {
            // Use pre-calculated mappings from server - works for all filter modes
            const relatedPostIndices = window.tagRelationships[tag];
            count = relatedPostIndices.filter(index => {
                // Only count if this post is currently visible (handles any combination of date + mode filters)
                return visiblePosts.some(({ post }) => post.index === index);
            }).length;
        } else {
            count = 0;
        }

        // Update count display if count span exists
        const countSpan = window.cachedCountSpans.get(tag);
        if (countSpan) {
            countSpan.textContent = `(${count})`;
        }

        // Update button state immediately in the same loop
        const isActive = window.activeFilters.has(tag);
        const shouldBeEnabled = count > 0;

        // Only update DOM if state actually changed - performance optimization
        const currentlyActive = btn.classList.contains('active');
        const currentlyEnabled = !btn.disabled;

        if (currentlyActive !== isActive) {
            btn.classList.toggle('active', isActive);
        }

        if (currentlyEnabled !== shouldBeEnabled) {
            btn.disabled = !shouldBeEnabled;
            btn.classList.toggle('disabled', !shouldBeEnabled);
        }
    });
}

// Execute functions - called by centralized event handler with mutex protection
function executeFilterToggle(tag, filterType) {
    if (filterType === 'date') {
        const isCurrentlyActive = window.activeFilters.has(tag);

        if (isCurrentlyActive) {
            // Simple case: just deactivate the current filter and we're done
            window.activeFilters.delete(tag);
        } else {
            // Different case: clear all date filters and activate the new one
            window.dateFilters.forEach(filter => {
                if (window.activeFilters.has(filter)) {
                    window.activeFilters.delete(filter);
                }
            });
            window.activeFilters.add(tag);
        }
    } else {
        // For other filters: simple toggle
        if (window.activeFilters.has(tag)) {
            window.activeFilters.delete(tag);
        } else {
            window.activeFilters.add(tag);
        }
    }

    updateDisplay();
    updateURL();
}

function executeClearFilters() {
    window.activeFilters.clear();

    if (window.collapsedTagsVisible) {
        window.collapsedTagsVisible = false;
        updateCollapsedTagsVisibility();
    }

    updateDisplay();
    updateURL();
}

function executeToggleCollapsedTags() {
    window.collapsedTagsVisible = !window.collapsedTagsVisible;

    updateCollapsedTagsVisibility();
    updateURL();
}

// More/Less tags functionality
function updateCollapsedTagsVisibility() {
    cachedHiddenTagButtons.forEach(btn => btn.classList.toggle('show', window.collapsedTagsVisible));

    const toggleBtn = document.getElementById('toggle-collapsed-tags');
    if (toggleBtn) {
        toggleBtn.textContent = window.collapsedTagsVisible ? 'Less' : 'More';
        toggleBtn.classList.toggle('active', window.collapsedTagsVisible);
    }
}

// URL management
function updateURL() {
    const url = new URL(window.location);

    // Handle filters parameter
    if (window.activeFilters.size > 0) {
        const filterArray = Array.from(window.activeFilters);
        url.searchParams.set('filters', filterArray.join(','));
    } else {
        url.searchParams.delete('filters');
    }

    // Handle text search parameter
    if (window.textSearchQuery && window.textSearchQuery.trim()) {
        url.searchParams.set('search', window.textSearchQuery.trim());
    } else {
        url.searchParams.delete('search');
    }

    // Handle showCollapsed parameter
    if (window.collapsedTagsVisible) {
        url.searchParams.set('showCollapsed', 'true');
    } else {
        url.searchParams.delete('showCollapsed');
    }

    window.history.replaceState({}, '', url);
}

// Simplified filter checking - everything is tag-based now
function checkFilterForCurrentMode(postIndex, filters) {
    // Use optimized tag relationships if available
    if (window.tagRelationships && Object.keys(window.tagRelationships).length > 0) {
        // For all tag filtering, ALL selected filters must match (AND logic)
        return filters.every(filter => {
            // Check if this post's index is in the tag relationship mapping for this filter
            return window.tagRelationships[filter] &&
                window.tagRelationships[filter].includes(postIndex);
        });
    }

    // No fallback - this indicates a problem with data generation
    console.error('Tag relationships not available for filtering. This indicates a problem with server-side data generation.');
    return false;
}

// Date filtering function using pre-calculated mappings
function isWithinDateFilter(postIndex, dateFilter) {
    // Use pre-calculated mappings for ultra-fast lookups
    return window.dateFilterMappings[dateFilter].has(postIndex);
}

// Make initializeTagFilter globally accessible
window.initializeTagFilter = initializeTagFilter;

// Export for testing
if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    // Test-only function to reset caches
    function resetCaches() {
        cachedPosts = null;
        cachedDateButtons = null;
        cachedNonDateButtons = null;
        cachedHiddenTagButtons = null;
    }

    module.exports = {
        updateDateButtonCountsAndState,
        initDOMCache,
        updateDisplay,
        resetCaches
    };
}
