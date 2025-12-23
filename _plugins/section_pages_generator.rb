
require 'fileutils'
require 'yaml'
require 'json'
require_relative 'jekyll_file_writer'


module Jekyll
  class SectionPagesGenerator < Generator
    safe true
    priority :highest

    def generate(site)
      sections_file = File.join(site.source, '_data', 'sections.json')
      return unless File.exist?(sections_file)
      
      begin
        sections = JSON.parse(File.read(sections_file))
      rescue JSON::ParserError => e
        Jekyll.logger.error "SectionPagesGenerator:", "Failed to parse sections.json: #{e.message}"
        return
      end

      sections.each do |section_key, section_data|
        next unless section_data['collections']
        next unless section_data['collections'].respond_to?(:each)

        # Generate main section index page
        create_section_index_page(site, section_data)

        # Generate collection pages for this section
        section_data['collections'].each do |page_data|
          next unless page_data['collection'] # Only generate for pages that have a collection field
          create_collection_page(site, section_data, page_data)
        end
      end
    end

    private

    def create_section_index_page(site, section_data)
      # Create a new Jekyll page using PageWithoutAFile
      page = Jekyll::PageWithoutAFile.new(site, site.source, section_data['section'], 'index.md')
      
      # Determine RSS feed URL based on section
      rss_feed_url = if section_data['section'] == 'all'
        '/rss/feed.xml'
      elsif section_data['section'] == 'github-copilot'
        '/rss/github-copilot.xml'
      else
        "/rss/#{section_data['section']}.xml"
      end
      
      # Set page data (front matter)
      page.data['layout'] = 'home'
      page.data['section'] = section_data['section']
      page.data['category'] = section_data['category']
      page.data['title'] = section_data['title']
      page.data['description'] = section_data['description']
      page.data['rss_feed'] = rss_feed_url
      
      # Set page content
      page.content = generate_section_index_content(section_data)
      
      # Add the page to the site's pages collection
      site.pages << page
      
      Jekyll.logger.info "SectionPagesGenerator:", "Created section index page for #{section_data['section']}"
    end

    def create_collection_page(site, section_data, page_data)
      # Create a new Jekyll page using PageWithoutAFile
      filename = "#{page_data['collection']}.html"
      page = Jekyll::PageWithoutAFile.new(site, site.source, section_data['section'], filename)
      
      # Set page data (front matter)
      page.data['layout'] = 'page'
      page.data['section'] = section_data['section']
      page.data['category'] = section_data['category']
      page.data['collection'] = page_data['collection']
      page.data['title'] = page_data['title']
      page.data['description'] = page_data['description']
      page.data['index_tag_mode'] = 'tags'
      
      # Set page content
      page.content = generate_collection_page_content(section_data, page_data)
      
      # Add the page to the site's pages collection
      site.pages << page
      
      Jekyll.logger.info "SectionPagesGenerator:", "Created collection page: #{section_data['section']}/#{filename}"
    end

    def generate_section_index_content(section_data)
      # Generate collections array from section_data in Ruby
      collections = section_data['collections']&.map { |collection| collection['collection'] }&.compact || []
      collections_string = collections.join(',')
      
      <<~CONTENT
        {%- comment -%} Dynamic section index - generated from sections config {%- endcomment -%}
        
        {%- assign section_data = site.data.sections[page.section] -%}
        {%- assign collections = "#{collections_string}" | split: "," -%}

        {%- assign items = site.documents | where_exp: "item", "collections contains item.collection" -%}
        {%- if page.category != "All" -%}
          {%- assign items = items | where_exp: "item", "item.categories contains page.category" -%}
        {%- endif -%}
        {%- assign items = items | where_exp: "item", "item.date <= site.time" -%}
        {%- assign items = items | limit_with_same_day: 50 -%}

        {{ section_data.description }}

        <div class="navigation-collections-grid">
        {%- for collection_info in section_data.collections -%}
          <a href="{{ collection_info.url | relative_url }}" class="navigation-collection-square">
            <div class="navigation-collection-content">
              <span class="navigation-collection-title">{{ collection_info.title | escape }}</span>
              <span class="navigation-collection-desc">{{ collection_info.description | escape }}</span>
            </div>
          </a>
        {%- endfor -%}
        </div>

        <div class="index-latest">
          <h2>Latest of everything</h2>
          {%- if page.category == "All" -%}
            {% include filters.html items=items index_tag_mode="sections" section=page.section %}
          {%- else -%}
            {% include filters.html items=items index_tag_mode="collections" section=page.section %}
          {%- endif -%}
          {% include posts.html section=page.section items=items %}
        </div>
      CONTENT
    end

    def generate_collection_page_content(section_data, page_data)
      # Generate values directly in Ruby instead of Liquid assignments
      <<~CONTENT
        {%- comment -%} Dynamic collection page - generated from sections config {%- endcomment -%}

        {%- assign items = site[page.collection] -%}
        {%- if page.category != "All" -%}
          {%- assign items = items | where_exp: "item", "item.categories contains page.category" -%}
        {%- endif -%}
        {%- if page.collection != "events" -%}
          {%- assign items = items | where_exp: "item", "item.date <= site.time" -%}
        {%- endif -%}

        {% include filters.html items=items index_tag_mode="tags" section=page.section %}
        {% include posts.html section=page.section items=items %}
      CONTENT
    end
  end
end
