class YouTube < Liquid::Tag
  Syntax = /^\s*([a-zA-Z0-9_-]+)(?:\s+.*)?$/

  def initialize(tagName, markup, tokens)
    super
    @markup = markup.strip

    if @markup.nil? || @markup.empty?
      raise "No YouTube ID provided in the \"youtube\" tag"
    end
  end

  def render(context)
    id = @markup
    
    # Check if the markup looks like a Liquid variable (e.g., page.youtube_id)
    if @markup.include?('.')
      # Split on dots to navigate the context (e.g., "page.youtube_id" -> ["page", "youtube_id"])
      parts = @markup.split('.')
      value = context
      
      parts.each do |part|
        value = value[part]
        break if value.nil?
      end
      
      id = value.to_s.strip if value
    end
    
    # Validate the ID matches YouTube ID format
    if id.nil? || id.empty? || id !~ Syntax
      raise "No valid YouTube ID provided in the \"youtube\" tag. Markup: '#{@markup}', Resolved: '#{id}'"
    end
    
    "<iframe class=\"youtube\" src=\"https://www.youtube.com/embed/#{id}\" loading=\"lazy\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>"
  end

  Liquid::Template.register_tag "youtube", self
end