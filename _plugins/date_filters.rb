module Jekyll
  module DateFilters
    # Convert a date to Unix epoch timestamp
    # Usage: {{ item.date | to_epoch }}
    def to_epoch(input)
      DateUtils.to_epoch(input)
    end
    
    # Convert a date to YYYY-MM-DD format then to epoch
    # Usage: {{ item.date | date_to_epoch }}
    def date_to_epoch(input)
      DateUtils.date_to_epoch(input)
    end
    
    # Get current time as epoch timestamp (ignores input)
    # Usage: {{ '' | now_epoch }}
    def now_epoch(input)
      DateUtils.now_epoch()
    end

    # Normalize date format for parsing - handles timezone issues
    # Usage: {{ raw_date_string | normalize_date_format | to_epoch }}
    def normalize_date_format(date_str)
      DateUtils.normalize_date_format(date_str)
    end
    
    # Normalize time to midnight for consistent epoch calculations
    # Usage: {{ item.date | normalize_to_midnight | to_epoch }}
    def normalize_to_midnight(input)
      DateUtils.normalize_to_midnight(input)
    end
    
    # Filter array to only items with valid dates
    # Usage: {{ site.documents | with_dates }}
    def with_dates(items)
      return [] if items.nil? || !items.respond_to?(:select)
      
      items.select { |item| DateUtils.has_valid_date?(item) }
    end
    
    # Sort items by date (newest first by default)
    # Usage: {{ site.blogs | sort_by_date }} or {{ site.blogs | sort_by_date: 'asc' }}
    def sort_by_date(items, direction = 'desc')
      return [] if items.nil? || !items.respond_to?(:sort)
      
      items.sort { |a, b| DateUtils.compare_by_date(a, b, direction) }
    end
    
    # Apply "20 + same-day" limiting rule to an array of items
    # Groups items by collection first, then applies the limiting rule per collection.
    # This ensures that if one collection has many items published quickly,
    # other collections still get representation in the final result.
    # Also excludes items older than 30 days.
    # Usage: {{ site.blogs | limit_with_same_day }}
    # Usage: {{ site.blogs | limit_with_same_day: 5 }}
    def limit_with_same_day(items, limit = 20)
      return [] if items.nil? || !items.respond_to?(:size) || items.size == 0
      
      # Filter out items without dates first
      dated_items = with_dates(items)
      
      # Filter out items older than 7 days
      cutoff_date = DateUtils.now_epoch() - (7 * 24 * 60 * 60) # 7 days ago in seconds
      recent_items = dated_items.select do |item|
        item_epoch = DateUtils.date_to_epoch(DateUtils.extract_date(item))
        item_epoch >= cutoff_date
      end
      dated_items = recent_items
      return [] if dated_items.empty?
      
      # Group items by collection
      collection_groups = {}
      dated_items.each do |item|
        collection_name = get_item_collection(item)
        collection_groups[collection_name] ||= []
        collection_groups[collection_name] << item
      end
      
      # Apply "20 + same-day" rule to each collection separately
      limited_collections = {}
      collection_groups.each do |collection_name, collection_items|
        limited_collections[collection_name] = apply_same_day_limit_to_collection(collection_items, limit)
      end
      
      # Merge all limited collections and sort by date (newest first)
      all_limited_items = limited_collections.values.flatten
      sort_by_date(all_limited_items, 'desc')
    end

    private

    # Extract collection name from an item
    def get_item_collection(item)
      if item.respond_to?(:collection)
        if item.collection.respond_to?(:label)
          # Jekyll document object with collection object
          item.collection.label
        elsif item.collection.is_a?(String)
          # Jekyll document object with collection string
          item.collection
        else
          # Jekyll document object with unknown collection type
          'unknown'
        end
      elsif item.is_a?(Hash)
        # Hash representation (for tests)
        item['collection'] || item[:collection] || 'unknown'
      else
        # Fallback
        'unknown'
      end
    end

    # Apply the original "20 + same-day" logic to a single collection
    def apply_same_day_limit_to_collection(collection_items, limit = 20)
      # Sort by date (newest first)
      sorted_items = sort_by_date(collection_items, 'desc')
      
      # If we have limit or fewer items, return all of them
      return sorted_items if sorted_items.size <= limit
      
      # Get the first limit items
      first_items = sorted_items[0, limit]
      last_item = first_items.last
      
      # Get the date of the last item as YYYY-MM-DD string
      last_item_day = DateUtils.to_date_string(DateUtils.extract_date(last_item))
      
      # Start with the first limit items
      result = first_items.dup
      
      # Add any additional items from the same day as the last item
      sorted_items[limit..-1].each do |item|
        item_day = DateUtils.to_date_string(DateUtils.extract_date(item))
        
        if item_day == last_item_day
          result << item
        else
          # Different day - stop processing (assumes items are sorted by date)
          break
        end
      end
      
      result
    end
  end
end

Liquid::Template.register_filter(Jekyll::DateFilters)
