class YouTube < Liquid::Tag
  Syntax = /^[a-zA-Z0-9_-]+$/

  def initialize(tagName, markup, tokens)
    super
    @markup = markup.strip

    if @markup.nil? || @markup.empty?
      raise "No YouTube ID provided in the \"youtube\" tag"
    end
  end

  def render(context)
    # Parse the markup to extract YouTube ID and optional width/height
    tokens = @markup.split(/\s+/)
    id_token = tokens[0]
    
    # Check if the first token looks like a Liquid variable (e.g., page.youtube_id)
    if id_token.include?('.')
      # Use Liquid's context lookup for nested variables
      resolved = if context.respond_to?(:lookup)
        context.lookup(id_token)
      else
        context[id_token]
      end
      id = resolved.to_s.strip if resolved
    else
      id = id_token
    end
    
    # Validate the ID matches YouTube ID format (alphanumeric, hyphens, underscores)
    if id.nil? || id.empty? || id !~ Syntax
      raise "No valid YouTube ID provided in the \"youtube\" tag. Markup: '#{@markup}', Resolved ID: '#{id}'"
    end
    
    # Optional: Extract width and height from subsequent numeric tokens
    # width = tokens[1] if tokens.length > 1 && tokens[1] =~ /^\d+$/
    # height = tokens[2] if tokens.length > 2 && tokens[2] =~ /^\d+$/
    # Note: Currently not used in iframe output, but available for future enhancement
    
    "<iframe class=\"youtube\" src=\"https://www.youtube.com/embed/#{id}\" loading=\"lazy\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>"
  end

  Liquid::Template.register_tag "youtube", self
end