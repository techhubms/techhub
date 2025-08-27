class YouTube < Liquid::Tag
  Syntax = /^\s*([a-zA-Z0-9_-]+)(?:\s+.*)?$/

  def initialize(tagName, markup, tokens)
    super

    if markup.nil? || markup.strip.empty?
      raise "No YouTube ID provided in the \"youtube\" tag"
    end

    if markup.strip =~ Syntax then
      @id = $1
    else
      raise "No YouTube ID provided in the \"youtube\" tag"
    end
  end

  def render(context)
    "<iframe class=\"youtube\" src=\"https://www.youtube.com/embed/#{@id}\" loading=\"lazy\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>"
  end

  Liquid::Template.register_tag "youtube", self
end