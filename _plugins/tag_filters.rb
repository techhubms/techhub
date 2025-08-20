require_relative 'date_utils'
require 'set'

module Jekyll
  module TagFilters
    
    # Cache for date filter calculations to avoid repeated processing
    @@date_filter_cache = {}
    
    # String operation caches for performance
    @@string_split_cache = {}
    @@normalize_cache = {}
    
    # Tag word index cache for faster subset matching
    @@word_index_cache = {}
    
    # Pre-compiled regex patterns for common operations
    SPECIAL_CHARS_REGEX = /[^a-z0-9\s\-]/i.freeze
    WHITESPACE_REGEX = /\s+/.freeze
    
    # Cached string operations for performance
    def cached_string_split(string, pattern = WHITESPACE_REGEX)
      cache_key = "#{string}_#{pattern.source}"
      @@string_split_cache[cache_key] ||= string.split(pattern).reject(&:empty?)
    end
    
    def cached_normalize(tag)
      return '' if tag.nil? || tag.to_s.strip.empty?
      
      cache_key = tag.to_s
      @@normalize_cache[cache_key] ||= begin
        normalized = tag.to_s.downcase.strip
        normalized.gsub(SPECIAL_CHARS_REGEX, ' ').squeeze(' ').strip
      end
    end
    
    # Calculate date cutoffs for all date filters at once
    def calculate_date_cutoffs(date_filter_config)
      cutoffs = {}
      date_filter_config.each do |config|
        parts = config.split(':')
        days = parts[1].to_i
        
        if days == 0
          # Today only - get start of today in Brussels timezone
          cutoffs[days] = DateUtils.now_epoch() # Start of today
        else
          # Last N days - calculate cutoff
          now_epoch = DateUtils.now_epoch()
          days_in_seconds = (days - 1) * 86400
          cutoffs[days] = now_epoch - days_in_seconds
        end
      end
      cutoffs
    end
    
    # Build word index for faster subset matching operations
    def build_word_index(items)
      # Check cache first
      cache_key = items.object_id
      return @@word_index_cache[cache_key] if @@word_index_cache.key?(cache_key)
      
      # Create index of words -> tags that contain them
      word_to_tags = {}
      tag_to_items = {}
      
      items.each_with_index do |item, item_index|
        normalized_tags = item['tags_normalized'] || []
        
        normalized_tags.each_with_index do |normalized_tag, tag_index|
          next if normalized_tag.nil? || normalized_tag.empty?
          
          # Record this tag for this item
          tag_to_items[normalized_tag] ||= []
          tag_to_items[normalized_tag] << item_index
          
          # Index all words in this tag
          words = cached_string_split(normalized_tag)
          words.each do |word|
            next if word.length <= 1
            word_to_tags[word] ||= Set.new
            word_to_tags[word].add(normalized_tag)
          end
        end
      end
      
      # Convert Sets to arrays and deduplicate item lists
      word_to_tags.each do |word, tags|
        word_to_tags[word] = tags.to_a
      end
      
      tag_to_items.each do |tag, items|
        tag_to_items[tag] = items.uniq
      end
      
      word_index = {
        'word_to_tags' => word_to_tags,
        'tag_to_items' => tag_to_items
      }
      
      # Cache and return
      @@word_index_cache[cache_key] = word_index
      word_index
    end
    
    # Fast subset matching using word index
    def fast_subset_matching(search_tag, word_index)
      # Direct match first (most common case)
      if word_index['tag_to_items'].key?(search_tag)
        return word_index['tag_to_items'][search_tag]
      end
      
      # Find all tags that contain this word
      matching_items = Set.new
      
      # Split search tag into words
      search_words = cached_string_split(search_tag)
      
      # If it's a multi-word search, find exact matches only
      if search_words.size > 1
        # Only return direct matches for multi-word searches
        return []
      end
      
      # For single-word searches, find all tags containing this word
      search_word = search_words.first
      matching_tags = word_index['word_to_tags'][search_word] || []
      
      # For each matching tag, get all items
      matching_tags.each do |tag|
        items = word_index['tag_to_items'][tag] || []
        matching_items.merge(items)
      end
      
      matching_items.to_a
    end

    # Binary search optimization for date filtering - O(log n) instead of O(n)
    def optimize_date_filtering(item_epochs, date_cutoffs)
      return {} if item_epochs.empty?
      
      # Sort epochs once for binary search
      sorted_epochs = item_epochs.sort
      
      # Build date counts using binary search
      date_counts = {}
      date_cutoffs.each do |days, cutoff_epoch|
        if days == 0
          # Today only - need range search for start and end of day
          today_start = cutoff_epoch
          today_end = today_start + 86399 # End of day
          
          # Binary search for start index
          start_index = sorted_epochs.bsearch_index { |e| e >= today_start } || sorted_epochs.size
          end_index = sorted_epochs.bsearch_index { |e| e > today_end } || sorted_epochs.size
          
          date_counts[days] = end_index - start_index
        else
          # Last N days - binary search to find first index where epoch >= cutoff
          index = sorted_epochs.bsearch_index { |e| e >= cutoff_epoch } || sorted_epochs.size
          date_counts[days] = sorted_epochs.size - index
        end
      end
      
      date_counts
    end

    # Parallel processing for large item sets - 2-4x speedup on multi-core systems
    def process_all_item_data_parallel(items, category, collection, date_cutoffs, sections = nil)
      require 'parallel'
      
      # Determine optimal batch size based on processor count
      processor_count = Parallel.processor_count
      batch_size = [items.size / (processor_count * 2), 100].max
      
      # Process batches in parallel
      results = Parallel.map(items.each_slice(batch_size).to_a.each_with_index) do |batch, batch_index|
        batch_start_index = batch_index * batch_size
        process_batch(batch, category, collection, date_cutoffs, batch_start_index, sections)
      end
      
      # Merge results from parallel processing
      merge_batch_results(results)
    end
    
    # Helper to process a batch of items
    def process_batch(batch, category, collection, date_cutoffs, batch_start_index = 0, sections = nil)
      # Initialize data structures using Sets for performance
      tag_data = {
        'relationships' => {},
        'tag_counts' => {},
        'original_tag_counts' => {},
        'tag_mapping' => {},
        'redundant_tags' => []
      }
      
      # Build redundant tags set using Set for fast lookup
      redundant_tags = Set.new
      if category && !category.empty?
        if category.is_a?(Array)
          category.each { |cat| redundant_tags.add(cat.to_s.downcase) }
        else
          redundant_tags.add(category.to_s.downcase)
        end
      end
      if collection && !collection.empty?
        redundant_tags.add(collection.to_s.downcase)
      end
      
      # Initialize date counts
      date_counts = {}
      date_cutoffs.each { |days, _| date_counts[days] = 0 }
      
      # Filter data for JavaScript
      filter_data = []
      item_epochs = []
      
      # Process this batch of items
      batch.each_with_index do |item, batch_item_index|
        # Calculate global item index
        global_item_index = batch_start_index + batch_item_index
        
        # Extract date once
        item_date = DateUtils.extract_date(item)
        item_epoch = DateUtils.to_epoch(item_date)
        item_epochs << item_epoch
        
        # Process dates for all date filters at once
        date_cutoffs.each do |days, cutoff|
          if days == 0
            # Today only
            today_start = cutoff
            today_end = today_start + 86399
            if item_epoch >= today_start && item_epoch <= today_end
              date_counts[days] += 1
            end
          else
            # Last N days
            if item_epoch >= cutoff
              date_counts[days] += 1
            end
          end
        end
        
        # Process tags with optimized data structures
        original_tags = item['tags'] || []
        normalized_tags = item['tags_normalized'] || []
        
        # Keep track of which tags we've already processed for this item to avoid double-counting
        processed_tags_for_this_item = Set.new
        
        # Process each original tag pair
        original_tags.each_with_index do |original_tag, tag_index|
          next if tag_index >= normalized_tags.length
          normalized_tag = normalized_tags[tag_index]
          next if normalized_tag.nil? || normalized_tag.empty?
          
          # Cache string split results using optimized caching
          original_words = cached_string_split(original_tag)
          normalized_words = cached_string_split(normalized_tag)
          
          # Collect all tags and words that should be processed
          tags_to_process = []
          
          # Add the full tag
          tags_to_process << { normalized: normalized_tag, display: original_tag, is_original: true }
          
          # Add individual words
          normalized_words.each_with_index do |normalized_word, word_index|
            next if normalized_word.length <= 1 # Skip single letters
            next if normalized_word == normalized_tag # Skip if word is same as tag
            
            display_word = word_index < original_words.length ? original_words[word_index] : normalized_word
            tags_to_process << { normalized: normalized_word, display: display_word, is_original: false }
          end
          
          # Process all collected tags
          tags_to_process.each do |tag_data_item|
            normalized = tag_data_item[:normalized]
            display = tag_data_item[:display]
            is_original = tag_data_item[:is_original]
            
            # Build tag relationships using Set for deduplication
            tag_data['relationships'][normalized] ||= Set.new
            tag_data['relationships'][normalized].add(global_item_index)
            
            # Build tag mapping
            tag_data['tag_mapping'][normalized] ||= display
            
            # Count tags for subset matching (includes all words) - only count ONCE per item
            unless redundant_tags.include?(normalized) || processed_tags_for_this_item.include?(normalized)
              tag_data['tag_counts'][normalized] ||= 0
              tag_data['tag_counts'][normalized] += 1
              processed_tags_for_this_item.add(normalized)
            end
            
            # Count original tags only (for button generation) - only count ONCE per item
            if is_original && !redundant_tags.include?(normalized) && !processed_tags_for_this_item.include?("original_#{normalized}")
              tag_data['original_tag_counts'][normalized] ||= 0
              tag_data['original_tag_counts'][normalized] += 1
              processed_tags_for_this_item.add("original_#{normalized}")
            end
          end
        end
        
        # Build filter data for JavaScript - no description needed since we search DOM directly
        filter_data << {
          'index' => global_item_index,
          'epoch' => item_epoch,
          'tags' => normalized_tags,
          'tags_normalized' => normalized_tags
        }
      end
      
      # Convert Set relationships back to sorted arrays
      tag_data['relationships'].each do |tag, indices_set|
        tag_data['relationships'][tag] = indices_set.to_a.sort
      end
      
      # Convert redundant_tags Set to array for consistency
      tag_data['redundant_tags'] = redundant_tags.to_a
      
      # Fix display names for section and collection tags
      if sections
        tag_data['tag_mapping'].each do |normalized_tag, current_display|
          # Check if this normalized tag matches any section categories
          sections.each do |section_key, section_data|
            if section_data['category'] && normalized_tag == section_data['category'].downcase
              tag_data['tag_mapping'][normalized_tag] = section_data['category']
              break
            end
          end
          
          # Check if this normalized tag matches any collection types
          sections.each do |section_key, section_data|
            if section_data['collections']
              collection_config = section_data['collections'].find { |c| c['collection'] == normalized_tag }
              if collection_config && collection_config['title']
                tag_data['tag_mapping'][normalized_tag] = collection_config['title']
                break
              end
            end
          end
        end
      end
      
      # Return partial results for this batch
      {
        tag_data: tag_data,
        date_counts: date_counts,
        filter_data: filter_data,
        item_epochs: item_epochs
      }
    end
    
    # Helper to merge results from parallel processing
    def merge_batch_results(results)
      merged = {
        tag_data: {
          'relationships' => {},
          'tag_counts' => {},
          'original_tag_counts' => {},
          'tag_mapping' => {},
          'redundant_tags' => []
        },
        date_counts: {},
        filter_data: [],
        item_epochs: []
      }
      
      # Merge tag data
      results.each do |result|
        # Merge relationships
        result[:tag_data]['relationships'].each do |tag, indices|
          merged[:tag_data]['relationships'][tag] ||= []
          merged[:tag_data]['relationships'][tag].concat(indices)
        end
        
        # Merge tag counts
        result[:tag_data]['tag_counts'].each do |tag, count|
          merged[:tag_data]['tag_counts'][tag] ||= 0
          merged[:tag_data]['tag_counts'][tag] += count
        end
        
        # Merge original tag counts
        result[:tag_data]['original_tag_counts'].each do |tag, count|
          merged[:tag_data]['original_tag_counts'][tag] ||= 0
          merged[:tag_data]['original_tag_counts'][tag] += count
        end
        
        # Merge tag mapping (take any mapping, they should be consistent)
        merged[:tag_data]['tag_mapping'].merge!(result[:tag_data]['tag_mapping'])
        
        # Merge redundant tags
        merged[:tag_data]['redundant_tags'].concat(result[:tag_data]['redundant_tags'])
        
        # Merge date counts
        result[:date_counts].each do |days, count|
          merged[:date_counts][days] ||= 0
          merged[:date_counts][days] += count
        end
        
        # Append filter data and epochs
        merged[:filter_data].concat(result[:filter_data])
        merged[:item_epochs].concat(result[:item_epochs])
      end
      
      # Deduplicate arrays where needed
      merged[:tag_data]['relationships'].each do |tag, indices|
        merged[:tag_data]['relationships'][tag] = indices.uniq.sort
      end
      
      merged[:tag_data]['redundant_tags'] = merged[:tag_data]['redundant_tags'].uniq
      
      merged[:filter_data].sort_by! { |item| item['index'] }
      
      merged
    end
    
    # Determine eligible date filters based on content and 50% growth rule using pre-calculated counts
    def determine_eligible_date_filters(date_filter_config, max_counts_by_days)
      all_filters = date_filter_config.map do |config|
        parts = config.split(":")
        { label: parts[0].strip, days: parts[1].to_i }
      end

      eligible_filters = []
      prev_count = 0
      timezone_sensitive_days = all_filters.select { |f| f[:days] <= 2 }.map { |f| f[:days] }

      all_filters.each do |filter|
        count = max_counts_by_days[filter[:days]] || 0
        is_timezone_sensitive = timezone_sensitive_days.include?(filter[:days])

        if count > 0
          if is_timezone_sensitive
            eligible_filters << filter[:label]
            prev_count = count
          else
            if prev_count == 0
              eligible_filters << filter[:label]
              prev_count = count
            else
              base_threshold = (prev_count * 1.5).ceil
              min_threshold = prev_count + 1
              fifty_percent_threshold = [base_threshold, min_threshold].max
              if count >= fifty_percent_threshold
                eligible_filters << filter[:label]
                prev_count = count
              end
            end
          end
        end
      end

      total_items = max_counts_by_days.values.max || 0
      if eligible_filters.size > 1 && total_items > 0
        last_filter_label = eligible_filters.last
        last_filter = all_filters.find { |f| f[:label] == last_filter_label }
        last_filter_days = last_filter ? last_filter[:days] : 0
        last_filter_count = max_counts_by_days[last_filter_days] || 0

        threshold_percentage = (total_items * 0.85).ceil
        if last_filter_count >= threshold_percentage && !timezone_sensitive_days.include?(last_filter_days)
          eligible_filters.pop
        end
      end

      eligible_filters
    end
    
    # Compute exact counts per filter per timezone offset (-12 to +14 hours)
    # Returns: { days => { offset_hours => count, ... }, ... }
    def compute_counts_per_day_per_timezone(item_epochs, date_filter_config)
      require 'set'
      return {} if item_epochs.nil? || item_epochs.empty? || date_filter_config.nil?

      # Sort epochs once for efficient range queries
      sorted_epochs = item_epochs.sort

      # Build set of unique days values
      days_set = Set.new
      date_filter_config.each do |config|
        parts = config.split(':')
        days = parts[1].to_i
        days_set.add(days)
      end

      # Timezone offsets: -12 to +14 (inclusive)
      timezone_offsets = (-12..14).to_a

      # Get Brussels midnight epoch (reference for all calculations)
      now_epoch = DateUtils.now_epoch()

      # Result: { days => { offset_hours => count, ... }, ... }
      counts_by_days_and_tz = {}

      days_set.each do |days|
        counts_by_tz = {}
        timezone_offsets.each do |offset|
          offset_seconds = offset * 3600
          if days == 0
            # Today only: window is [midnight+offset, midnight+offset+86399]
            window_start = now_epoch + offset_seconds
            window_end = window_start + 86399
          else
            # Last N days: window is [midnight+offset-(N-1)*86400, midnight+offset+86399]
            days_in_seconds = (days - 1) * 86400
            window_start = now_epoch + offset_seconds - days_in_seconds
            window_end = now_epoch + offset_seconds + 86399
          end

          # Binary search for start and end indices
          start_idx = sorted_epochs.bsearch_index { |e| e >= window_start } || sorted_epochs.size
          end_idx = sorted_epochs.bsearch_index { |e| e > window_end } || sorted_epochs.size
          count = end_idx - start_idx
          counts_by_tz[offset] = count
        end
        counts_by_days_and_tz[days] = counts_by_tz
      end

      counts_by_days_and_tz
    end

    # Given { days => { offset_hours => count, ... }, ... }, return { days => max_count, ... }
    def get_max_counts_by_days(counts_per_day_per_timezone)
      return {} if counts_per_day_per_timezone.nil? || counts_per_day_per_timezone.empty?

      max_counts = {}
      counts_per_day_per_timezone.each do |days, counts_by_tz|
        if counts_by_tz && !counts_by_tz.empty?
          max_counts[days] = counts_by_tz.values.max || 0
        else
          max_counts[days] = 0
        end
      end
      max_counts
    end

    # Generate date filter buttons using optimized timezone calculations
    def generate_date_filter_buttons(date_filters, date_filter_config, max_counts_by_days)
      return [] if date_filters.nil? || date_filter_config.nil? || max_counts_by_days.nil?
      
      # If no max counts were calculated (empty items), return empty array
      return [] if max_counts_by_days.empty?

      config_lookup = {}
      date_filter_config.each do |config|
        parts = config.split(':')
        config_lookup[parts[0].strip] = parts[1].to_i
      end

      results = date_filters.map do |filter_label|
        days = config_lookup[filter_label] || 0
        max_count = max_counts_by_days[days] || 0

        width_class = if max_count >= 100
          'filter-width-3-digits'
        elsif max_count >= 10
          'filter-width-2-digits'
        else
          'filter-width-1-digit'
        end

        {
          'label' => filter_label,
          'width_class' => width_class,
          'data_tag' => filter_label.downcase,
          'count' => max_count
        }
      end

      results
    end
    
    # Main preprocessing function that does all heavy lifting once
    # Processes all subset matching logic and returns ready-to-use data
    # Usage: {{ items | preprocess_filter_data: category, collection }}
    def preprocess_filter_data(items, category, collection)
      return {} unless items
      
      # Initialize data structures
      tag_relationships = {}
      tag_counts = {} # Counts for individual words (for subset matching)
      original_tag_counts = {} # Counts for original tags only (for button generation)
      tag_mapping = {} # normalized -> original for display
      redundant_tags = Set.new
      
      # Build redundant tags set
      if category && !category.empty?
        if category.is_a?(Array)
          category.each { |cat| redundant_tags.add(cat.to_s.downcase) }
        else
          redundant_tags.add(category.to_s.downcase)
        end
      end
      if collection && !collection.empty?
        redundant_tags.add(collection.to_s.downcase)
      end
      
      # Single pass: expand all tags with subset matching and build all data structures
      items.each_with_index do |item, item_index|
        original_tags = item['tags'] || []
        normalized_tags = item['tags_normalized'] || []
        
        # Keep track of which tags we've already processed for this item to avoid double-counting
        processed_tags_for_this_item = Set.new
        
        # Process each original tag pair
        original_tags.each_with_index do |original_tag, tag_index|
          next if tag_index >= normalized_tags.length
          normalized_tag = normalized_tags[tag_index]
          next if normalized_tag.nil? || normalized_tag.empty?
          
          # Collect all tags and words that should be processed (with their display names)
          tags_to_process = []
          
          # Add the full tag
          tags_to_process << { normalized: normalized_tag, display: original_tag, is_original: true }
          
          # Add individual words from both tags using cached operations
          original_words = cached_string_split(original_tag)
          normalized_words = cached_string_split(normalized_tag)
          
          normalized_words.each_with_index do |normalized_word, word_index|
            next if normalized_word.length <= 1 # Skip single letters
            next if normalized_word == normalized_tag # Skip if word is same as tag (avoid duplication)
            
            # Use corresponding original word, or fall back
            display_word = if word_index < original_words.length
              original_words[word_index]
            else
              normalized_word
            end
            
            tags_to_process << { normalized: normalized_word, display: display_word, is_original: false }
          end
          
          # Process all collected tags
          tags_to_process.each do |tag_data|
            normalized = tag_data[:normalized]
            display = tag_data[:display]
            is_original = tag_data[:is_original]
            
            # Build tag relationships (for JavaScript filtering) - includes all words
            tag_relationships[normalized] ||= []
            tag_relationships[normalized] << item_index
            
            # Build tag mapping (normalized -> display) - includes all words
            tag_mapping[normalized] ||= display
            
            # Count tags for subset matching (includes all words) - only count ONCE per item
            unless redundant_tags.include?(normalized) || processed_tags_for_this_item.include?(normalized)
              tag_counts[normalized] ||= 0
              tag_counts[normalized] += 1
              processed_tags_for_this_item.add(normalized)
            end
            
            # Count original tags only (for button generation) - only count ONCE per item
            if is_original && !redundant_tags.include?(normalized) && !processed_tags_for_this_item.include?("original_#{normalized}")
              original_tag_counts[normalized] ||= 0
              original_tag_counts[normalized] += 1
              processed_tags_for_this_item.add("original_#{normalized}")
            end
          end
        end
      end
      
      # Clean up relationships (remove duplicates and sort)
      tag_relationships.each do |tag, indices|
        tag_relationships[tag] = indices.uniq.sort
      end
      
      # Return all processed data - everything other functions need
      {
        'relationships' => tag_relationships,
        'tag_counts' => tag_counts, # All words for subset matching
        'original_tag_counts' => original_tag_counts, # Only original tags for buttons
        'tag_mapping' => tag_mapping,
        'redundant_tags' => redundant_tags.to_a
      }
    end
    
    # Helper function to extract date filter configuration
    def extract_date_filter_config(tag_filter_config)
      date_filter_config = []
      if tag_filter_config && tag_filter_config['date_filters']
        tag_filter_config['date_filters'].each do |filter|
          date_filter_config << "#{filter['label']}:#{filter['days']}"
        end
      end
      date_filter_config
    end
    
    # Helper function to extract tag display limits
    def extract_tag_display_limits(tag_filter_config)
      unless tag_filter_config && tag_filter_config['tag_display_limits']
        raise "Tag filter configuration must include 'tag_display_limits' with 'collapsed_view_count' and 'expanded_view_max_count'."
      end
      
      [
        tag_filter_config['tag_display_limits']['collapsed_view_count'] || 30,
        tag_filter_config['tag_display_limits']['expanded_view_max_count'] || 100
      ]
    end
    
    # Complete filter generation - does everything in one efficient call
    # Usage: {{ items | generate_all_filters: index_tag_mode, sections, section, category, collection, tag_filter_config }}
    def generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
      if !items || items.empty?
        raise "Items must be a non-empty array."
      end
      
      # Sort items by date (newest first) to match DOM rendering order in posts.html
      # This ensures JavaScript data has the same order as the DOM elements
      sorted_items = items.sort_by { |item| 
        begin
          date = DateUtils.extract_date(item)
          DateUtils.to_epoch(date)
        rescue
          0 # Fallback for items without valid dates
        end
      }.reverse

      # Extract configuration once using helper functions
      date_filter_config = extract_date_filter_config(tag_filter_config)
      tag_display_limits = extract_tag_display_limits(tag_filter_config)

      # Step 2: Pre-calculate date cutoffs
      date_cutoffs = calculate_date_cutoffs(date_filter_config)

      # Step 3: Single pass processing of all item data (with parallel processing for large datasets)
      processed_data = process_all_item_data_parallel(sorted_items, category, collection, date_cutoffs, sections)      # Extract processed data
      preprocessed_data = processed_data[:tag_data]
      tag_filter_data = processed_data[:filter_data]
      item_epochs = processed_data[:item_epochs]
      
      counts_per_day_per_timezone = compute_counts_per_day_per_timezone(item_epochs, date_filter_config)

      max_counts_by_days = get_max_counts_by_days(counts_per_day_per_timezone)

      # Generate date filter buttons using correct counts
      date_filters = determine_eligible_date_filters(date_filter_config, max_counts_by_days)
      if !date_filters || date_filters.size == 0
        raise "No eligible date filters found. Please check your configuration and make sure there is content in section '#{section}'."
      end
      date_filter_buttons = generate_date_filter_buttons(date_filters, date_filter_config, max_counts_by_days)
      
      # Generate mode-specific filter buttons
      mode_filter_buttons = case index_tag_mode
      when 'sections'
        generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
      when 'collections'
        generate_collection_filter_buttons(preprocessed_data, tag_display_limits, sections[section])
      when 'tags'
        generate_tag_filter_buttons(preprocessed_data, tag_display_limits, category, collection)
      else
        raise "Invalid index_tag_mode: #{index_tag_mode.inspect}. Must be 'sections', 'collections', or 'tags'."
      end

      # Step 5 & 6: Use preprocessed data directly (no redundant preprocessing)
      tag_relationships = preprocessed_data['relationships'] || {}
      
      {
        'date_filter_buttons' => date_filter_buttons,
        'mode_filter_buttons' => mode_filter_buttons,
        'preprocessed_data' => preprocessed_data,
        'tag_filter_data' => tag_filter_data,
        'tag_relationships' => tag_relationships,
        'date_filter_config' => date_filter_config
      }
    end
    
    # Usage: {{ items | generate_tag_filter_data }}
    def generate_tag_filter_data(items)
      return [] unless items
      
      items.map.with_index do |item, index|
        # Get normalized tags for filtering
        normalized_tags = item['tags_normalized'] || []
        
        # Extract date using DateUtils
        item_date = DateUtils.extract_date(item)
        
        {
          'index' => index,
          'epoch' => DateUtils.to_epoch(item_date),
          'tags' => normalized_tags, # Use normalized tags for filtering
          'tags_normalized' => normalized_tags,
        }
      end
    end


    # Calculate count of items within a date range
    # Usage: {{ items | count_items_in_date_range: 7 }} (last 7 days)
    # Usage: {{ items | count_items_in_date_range: 0 }} (today only)
    def count_items_in_date_range(items, days)
      return 0 if items.nil? || !items.respond_to?(:count)
      
      now_epoch = DateUtils.now_epoch()
      target_days = days.to_i
      
      if target_days == 0
        # Today only
        items.count do |item|
          item_epoch = DateUtils.to_epoch(DateUtils.extract_date(item))
          item_epoch == now_epoch
        end
      else
        # "Last N days" means today + (N-1) days back
        # So "last 2 days" = today + 1 day back = 2 days total
        days_in_seconds = (target_days - 1) * 86400
        cutoff_epoch = now_epoch - days_in_seconds
        
        items.count do |item|
          item_epoch = DateUtils.to_epoch(DateUtils.extract_date(item))
          item_epoch >= cutoff_epoch
        end
      end
    end
    
    def generate_tag_filter_buttons(preprocessed_data, tag_display_limits, category, collection)
      max_tag_limit = tag_display_limits[1]
      top_tag_count = tag_display_limits[0]
      
      tag_counts = preprocessed_data['tag_counts'] || {} # Use full counts (includes subset matching)
      original_tag_counts = preprocessed_data['original_tag_counts'] || {}
      tag_mapping = preprocessed_data['tag_mapping'] || {}
      
      # Build set of tags to exclude (current section and collection only)
      tags_to_exclude = Set.new
      
      # Add current section category if provided
      if category && !category.empty?
        if category.is_a?(Array)
          category.each { |cat| tags_to_exclude.add(cat.to_s.downcase) }
        else
          tags_to_exclude.add(category.to_s.downcase)
        end
      end
      
      # Add current collection if provided
      if collection && !collection.empty?
        tags_to_exclude.add(collection.to_s.downcase)
      end
      
      # Build filter data: show all tags, excluding only current section and collection
      filter_data = {}
      original_tag_counts.each do |normalized_tag, _original_count|
        # Skip if this tag is the current section or collection
        next if tags_to_exclude.include?(normalized_tag)
        
        # Use the full count (includes subset matching) for accurate button display
        full_count = tag_counts[normalized_tag] || 0
        # Use the display name from tag mapping (which should be fixed during preprocessing)
        original_tag = tag_mapping[normalized_tag] || normalized_tag
        
        filter_data[normalized_tag] = {
          'normalized' => normalized_tag,
          'display' => original_tag,
          'count' => full_count # This includes subset matching, so "AI" shows count of all "AI", "Generative AI", etc.
        }
      end
      
      generate_button_list(filter_data, max_tag_limit, top_tag_count)
    end
    
    def generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
      max_tag_limit = tag_display_limits[1]
      top_tag_count = tag_display_limits[0]
      
      # Handle nil sections gracefully
      return [] if sections.nil? || sections.empty?
      
      # Use preprocessed data - only show sections and collections
      tag_counts = preprocessed_data['tag_counts'] || {}
      tag_mapping = preprocessed_data['tag_mapping'] || {}
      
      # Build set of all section categories and collection types to include
      tags_to_include = Set.new
      
      # Add all section categories
      sections.each do |section_key, section_data|
        if section_data['category']
          tags_to_include.add(section_data['category'].downcase)
        end
      end
      
      # Add all collection types from all sections
      sections.each do |section_key, section_data|
        if section_data['collections']
          section_data['collections'].each do |collection_config|
            if collection_config['collection']
              tags_to_include.add(collection_config['collection'])
            end
          end
        end
      end
      
      # Generate buttons only for sections and collections
      filter_data = {}
      tag_counts.each do |normalized_tag, count|
        # Only include tags that are sections or collections
        next unless tags_to_include.include?(normalized_tag)
        
        # Use the display name from tag mapping (which should be fixed during preprocessing)
        display_name = tag_mapping[normalized_tag] || normalized_tag
        
        filter_data[normalized_tag] = {
          'normalized' => normalized_tag,
          'display' => display_name,
          'count' => count
        }
      end
      
      generate_button_list(filter_data, max_tag_limit, top_tag_count)
    end
    
    def generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_data)
      max_tag_limit = tag_display_limits[1]
      top_tag_count = tag_display_limits[0]
      
      # Use preprocessed data - show all collections within the current section
      tag_counts = preprocessed_data['tag_counts'] || {}
      tag_mapping = preprocessed_data['tag_mapping'] || {}
      
      # Generate buttons for all collections within the current section
      filter_data = {}
      
      # Get all collections available for the current section
      available_collections = Set.new
      if section_data && section_data['collections']
        section_data['collections'].each do |collection_config|
          if collection_config['collection']
            available_collections.add(collection_config['collection'])
          end
        end
      end
      
      # Process preprocessed tag data - show all collections from current section
      tag_counts.each do |normalized_tag, count|
        # Check if this normalized tag matches any available collection types from section
        if available_collections.include?(normalized_tag)
          # Use the display name from tag mapping (which should be fixed during preprocessing)
          display_name = tag_mapping[normalized_tag] || normalized_tag
          
          filter_data[normalized_tag] = {
            'normalized' => normalized_tag,
            'display' => display_name,
            'count' => count
          }
        end
      end
      
      generate_button_list(filter_data, max_tag_limit, top_tag_count)
    end
    
    def generate_button_list(filter_data, max_tag_limit, top_tag_count)
      return [] if filter_data.empty?
      
      # Convert to array and sort by count (descending), then by display name
      all_filters = filter_data.values.sort_by { |filter| [-filter['count'], filter['display']] }
      
      # Limit filters
      limited_filters = all_filters.size > max_tag_limit ? all_filters[0...max_tag_limit] : all_filters
      
      # Sort by display name for final output
      sorted_filters = limited_filters.sort_by { |filter| filter['display'] }
      
      # Get top filters for prioritization (by count)
      top_filters = limited_filters[0...top_tag_count]
      top_filter_set = Set.new(top_filters.map { |filter| filter['normalized'] })
      
      sorted_filters.map do |filter|
        width_class = if filter['count'] >= 100
          'filter-width-3-digits'
        elsif filter['count'] >= 10
          'filter-width-2-digits'
        else
          'filter-width-1-digit'
        end
        
        {
          'normalized' => filter['normalized'],
          'display' => filter['display'],
          'count' => filter['count'],
          'width_class' => width_class,
          'is_top_tag' => top_filter_set.include?(filter['normalized'])
        }
      end
    end
  end
end

Liquid::Template.register_filter(Jekyll::TagFilters)
