/**
 * Client-side filtering functionality for Tech Hub
 * Handles tag and date filtering with URL state management
 */

(function () {
    'use strict';

    /**
     * Filter state manager
     */
    class FilterState {
        constructor() {
            this.selectedTags = [];
            this.dateFrom = null;
            this.dateTo = null;
            this.debounceTimer = null;
            this.debounceDelay = 300; // ms
        }

        /**
         * Initialize from URL parameters
         */
        initializeFromUrl() {
            const urlParams = new URLSearchParams(window.location.search);

            // Parse tags parameter
            const tagsParam = urlParams.get('tags');
            if (tagsParam) {
                this.selectedTags = tagsParam.split(',').map(t => t.trim()).filter(t => t);
            }

            // Parse date range parameters
            const fromParam = urlParams.get('from');
            const toParam = urlParams.get('to');

            if (fromParam) {
                this.dateFrom = this.parseDate(fromParam);
            }

            if (toParam) {
                this.dateTo = this.parseDate(toParam);
            }

            return this;
        }

        /**
         * Parse date string to Date object
         */
        parseDate(dateString) {
            try {
                const date = new Date(dateString);
                return isNaN(date.getTime()) ? null : date;
            } catch {
                return null;
            }
        }

        /**
         * Format date for URL (YYYY-MM-DD)
         */
        formatDate(date) {
            if (!date) return null;
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        }

        /**
         * Toggle tag selection
         */
        toggleTag(tag) {
            const index = this.selectedTags.indexOf(tag);
            if (index > -1) {
                this.selectedTags.splice(index, 1);
            } else {
                this.selectedTags.push(tag);
            }
        }

        /**
         * Add tag to selection
         */
        addTag(tag) {
            if (!this.selectedTags.includes(tag)) {
                this.selectedTags.push(tag);
            }
        }

        /**
         * Remove tag from selection
         */
        removeTag(tag) {
            const index = this.selectedTags.indexOf(tag);
            if (index > -1) {
                this.selectedTags.splice(index, 1);
            }
        }

        /**
         * Clear all tags
         */
        clearTags() {
            this.selectedTags = [];
        }

        /**
         * Set date range
         */
        setDateRange(from, to) {
            this.dateFrom = from;
            this.dateTo = to;
        }

        /**
         * Clear date range
         */
        clearDateRange() {
            this.dateFrom = null;
            this.dateTo = null;
        }

        /**
         * Clear all filters
         */
        clearAll() {
            this.selectedTags = [];
            this.dateFrom = null;
            this.dateTo = null;
        }

        /**
         * Check if any filters are active
         */
        hasActiveFilters() {
            return this.selectedTags.length > 0 || this.dateFrom !== null || this.dateTo !== null;
        }

        /**
         * Update URL with current filter state (debounced)
         */
        updateUrl(immediate = false) {
            if (this.debounceTimer) {
                clearTimeout(this.debounceTimer);
            }

            const updateFn = () => {
                const url = new URL(window.location.href);
                const params = url.searchParams;

                // Update tags parameter
                if (this.selectedTags.length > 0) {
                    params.set('tags', this.selectedTags.join(','));
                } else {
                    params.delete('tags');
                }

                // Update date range parameters
                if (this.dateFrom) {
                    params.set('from', this.formatDate(this.dateFrom));
                } else {
                    params.delete('from');
                }

                if (this.dateTo) {
                    params.set('to', this.formatDate(this.dateTo));
                } else {
                    params.delete('to');
                }

                // Update browser history without reload
                const newUrl = params.toString() ? `${url.pathname}?${params.toString()}` : url.pathname;
                window.history.pushState({}, '', newUrl);
            };

            if (immediate) {
                updateFn();
            } else {
                this.debounceTimer = setTimeout(updateFn, this.debounceDelay);
            }
        }

        /**
         * Get filter request object for API
         */
        toFilterRequest(sectionName = null, collectionName = null) {
            return {
                selectedTags: this.selectedTags,
                dateFrom: this.dateFrom,
                dateTo: this.dateTo,
                sectionName: sectionName,
                collectionName: collectionName
            };
        }
    }

    /**
     * Tag matching utility (subset matching with word boundaries)
     */
    class TagMatcher {
        /**
         * Check if searchTag matches itemTag using subset matching
         * Examples:
         *   - "AI" matches "AI", "Generative AI", "Azure AI"
         *   - "AI" does NOT match "AIR"
         *   - "Visual Studio" matches "Visual Studio", "Visual Studio Code"
         */
        static matches(searchTag, itemTag) {
            if (!searchTag || !itemTag) return false;

            const normalizedSearch = this.normalize(searchTag);
            const normalizedItem = this.normalize(itemTag);

            // Exact match
            if (normalizedSearch === normalizedItem) return true;

            // Word boundary match (search tag must be complete word in item tag)
            const pattern = new RegExp(`\\b${this.escapeRegExp(normalizedSearch)}\\b`, 'i');
            return pattern.test(normalizedItem);
        }

        /**
         * Normalize tag for matching
         */
        static normalize(tag) {
            return tag.trim().toLowerCase();
        }

        /**
         * Escape special regex characters
         */
        static escapeRegExp(string) {
            return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
        }

        /**
         * Check if any selected tags match item tags
         */
        static matchesAny(selectedTags, itemTags) {
            if (!selectedTags || selectedTags.length === 0) return true;
            if (!itemTags || itemTags.length === 0) return false;

            return selectedTags.some(selectedTag =>
                itemTags.some(itemTag => this.matches(selectedTag, itemTag))
            );
        }
    }

    /**
     * Date range utility
     */
    class DateRangeHelper {
        /**
         * Get date range preset
         */
        static getPreset(presetName) {
            const now = new Date();
            now.setHours(23, 59, 59, 999); // End of today

            const from = new Date(now);

            switch (presetName) {
                case 'last7days':
                    from.setDate(from.getDate() - 7);
                    break;
                case 'last30days':
                    from.setDate(from.getDate() - 30);
                    break;
                case 'last90days':
                    from.setDate(from.getDate() - 90);
                    break;
                case 'alltime':
                    return { from: null, to: null };
                default:
                    from.setDate(from.getDate() - 90); // Default to 90 days
            }

            from.setHours(0, 0, 0, 0); // Start of day

            return { from, to: now };
        }

        /**
         * Check if date is within range
         */
        static isInRange(date, from, to) {
            if (!date) return false;

            const d = new Date(date);

            if (from && d < from) return false;
            if (to && d > to) return false;

            return true;
        }

        /**
         * Format date for display
         */
        static formatDisplay(date) {
            if (!date) return '';

            const options = { year: 'numeric', month: 'short', day: 'numeric' };
            return date.toLocaleDateString('en-US', options);
        }
    }

    /**
     * Export utilities to global scope
     */
    window.TechHub = window.TechHub || {};
    window.TechHub.Filtering = {
        FilterState,
        TagMatcher,
        DateRangeHelper
    };

    /**
     * Initialize on page load if needed
     */
    document.addEventListener('DOMContentLoaded', () => {
        console.log('Tech Hub filtering utilities loaded');
    });

})();
