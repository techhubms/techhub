# Shared date utility class for consistent date handling across Jekyll plugins
class DateUtils
  # Normalize date format for parsing - handles timezone issues
  def self.normalize_date_format(date_str)
    return '' if date_str.nil?
    
    # Fix common timezone format issues
    normalized = date_str.to_s.strip
    
    # Convert +0000 to +00:00 format (4-digit to 6-character with colon)
    normalized = normalized.gsub(/([+-])(\d{2})(\d{2})$/, '\1\2:\3')
    
    # Handle CEST and other timezone abbreviations by converting to UTC
    if normalized.include?('CEST')
      # CEST is UTC+2, so subtract 2 hours and make it UTC
      normalized = normalized.gsub(/\s+CEST$/, ' +02:00')
    elsif normalized.include?('CET')
      # CET is UTC+1, so subtract 1 hour and make it UTC  
      normalized = normalized.gsub(/\s+CET$/, ' +01:00')
    end
    
    normalized
  end

  # Normalize time to midnight for consistent epoch calculations
  def self.normalize_to_midnight(input)
    return Time.at(0) if input.nil?
    
    case input
    when Time
      time = input
    when Date
      time = input.to_time
    when String
      begin
        time = Time.parse(normalize_date_format(input))
      rescue ArgumentError
        return Time.at(0)
      end
    else
      return Time.at(0)
    end
    
    # Convert to YYYY-MM-DD format and parse back to get midnight
    date_string = time.strftime('%Y-%m-%d')
    Time.parse("#{date_string} 00:00:00")
  end

  # Convert input to Time object with proper normalization
  def self.to_time(input)
    return Time.at(0) if input.nil?
    
    case input
    when Time
      input
    when Date
      input.to_time
    when String
      begin
        Time.parse(normalize_date_format(input))
      rescue ArgumentError
        Time.at(0)
      end
    else
      Time.at(0)
    end
  end

  # Convert a date to Unix epoch timestamp
  def self.to_epoch(input)
    return 0 if input.nil?
    to_time(input).to_i
  end

  # Convert a date to YYYY-MM-DD format then to epoch (for consistency with Jekyll date handling)
  def self.date_to_epoch(input)
    return 0 if input.nil?
    
    begin
      # First convert to YYYY-MM-DD format, then to epoch
      time = to_time(input)
      return 0 if time == Time.at(0)  # Handle invalid dates early
      
      formatted_date = time.strftime('%Y-%m-%d')
      
      # Always use Europe/Brussels timezone calculation
      current_time = Time.now
      brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
      brussels_midnight = Time.parse("#{formatted_date} 00:00:00 #{brussels_offset}")
      brussels_midnight.to_i
    rescue StandardError
      0
    end
  end

  # Get current time as epoch timestamp (midnight of today in Brussels timezone)
  def self.now_epoch
    require 'time'
    
    # Always use Europe/Brussels timezone (UTC+1 standard, UTC+2 daylight saving)
    current_time = Time.now
    
    # Convert to Brussels timezone - in July this would be UTC+2
    utc_now = current_time.utc
    brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
    
    # Get Brussels time by applying offset
    brussels_time = utc_now.getlocal(brussels_offset)
    
    # Get Brussels date and convert to midnight epoch
    brussels_date_str = brussels_time.strftime('%Y-%m-%d')
    brussels_midnight = Time.parse("#{brussels_date_str} 00:00:00 #{brussels_offset}")
    
    brussels_midnight.to_i
  end

  # Extract date from various object types (documents, hashes, etc.)
  def self.extract_date(item)
    return nil if item.nil?
    
    # Try different ways to get the date
    date_field = nil
    
    begin
      if item.respond_to?(:data) && item.data.is_a?(Hash)
        date_field = item.data['date']
      end
    rescue NoMethodError
      # data method exists but raises error, continue to next approach
    end
    
    if date_field.nil?
      begin
        if item.respond_to?(:date)
          date_field = item.date
        end
      rescue NoMethodError
        # date method exists but raises error, continue to next approach
      end
    end
    
    if date_field.nil? && item.respond_to?(:[])
      date_field = item['date']
    end
    
    date_field
  end

  # Compare two items by date
  def self.compare_by_date(item_a, item_b, direction = 'desc')
    date_a = extract_date(item_a)
    date_b = extract_date(item_b)
    
    time_a = to_time(date_a)
    time_b = to_time(date_b)
    
    if direction.to_s.downcase == 'asc'
      time_a <=> time_b
    else
      time_b <=> time_a  # Default: newest first
    end
  end

  # Get date as YYYY-MM-DD string
  def self.to_date_string(input)
    return '' if input.nil?
    to_time(input).strftime('%Y-%m-%d')
  end

  # Check if an item has a valid date
  def self.has_valid_date?(item)
    date_field = extract_date(item)
    !date_field.nil? && to_time(date_field) != Time.at(0)
  end
end
