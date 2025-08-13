module Jekyll
  module StringFilters
    # Check if a string matches a regex pattern
    # Returns true if the string matches the pattern, false otherwise
    def regex_match(string, pattern)
      return false unless string && pattern
      
      begin
        regex = Regexp.new(pattern)
        !regex.match(string.to_s).nil?
      rescue RegexpError => e
        Jekyll.logger.warn "Invalid regex pattern '#{pattern}': #{e.message}"
        false
      end
    end
    
    # Check if a string contains only letters and hyphens
    # This is a specialized version for section name validation
    def is_letters_and_hyphen_only(string)
      return false unless string.is_a?(String) && !string.empty?
      
      # Pattern: only lowercase letters, uppercase letters, and hyphens
      pattern = /^[a-zA-Z-]+$/
      !pattern.match(string).nil?
    end
  end
end

Liquid::Template.register_filter(Jekyll::StringFilters)
