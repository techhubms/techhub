# frozen_string_literal: true

module Jekyll
  # Hooks to generate sitemap.xml and robots.txt after site is written
  Jekyll::Hooks.register :site, :post_write do |site|
    generator = SitemapWriter.new(site)
    generator.generate_sitemap
    generator.generate_robots_txt
  end

  # Writer class that creates sitemap.xml and robots.txt files
  class SitemapWriter
    SITEMAP_FILE = 'sitemap.xml'
    ROBOTS_FILE = 'robots.txt'
    EXCLUDED_EXTENSIONS = %w[.xml .json .txt .pdf].freeze
    EXCLUDED_PATHS = %w[/404.html].freeze

    def initialize(site)
      @site = site
      @site_url = get_site_url
    end

    def generate_sitemap
      sitemap_content = build_sitemap_xml
      write_file(SITEMAP_FILE, sitemap_content)
    end

    def generate_robots_txt
      robots_content = build_robots_txt
      write_file(ROBOTS_FILE, robots_content)
    end

    private

    def get_site_url
      url = @site.config['url'] || 'http://localhost:4000'
      url = url.chomp('/')
      baseurl = @site.config['baseurl'] || ''
      "#{url}#{baseurl}"
    end

    def build_sitemap_xml
      xml = []
      xml << '<?xml version="1.0" encoding="UTF-8"?>'
      xml << '<urlset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" '
      xml << 'xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/0.9 '
      xml << 'http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd" '
      xml << 'xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">'

      collect_urls.each do |url|
        xml << '<url>'
        xml << "<loc>#{xml_escape(url)}</loc>"
        xml << '</url>'
      end

      xml << '</urlset>'
      xml.join("\n")
    end

    def build_robots_txt
      sitemap_url = "#{@site_url}/#{SITEMAP_FILE}"
      "Sitemap: #{sitemap_url}\n"
    end

    def collect_urls
      urls = []

      # Add all pages
      @site.pages.each do |page|
        next if should_exclude?(page)

        url = page_url(page)
        urls << url if url
      end

      # Note: Collections (individual posts, videos, news items, etc.) are excluded
      # Only the main site structure (section indexes, collection pages) is included

      urls.sort.uniq
    end

    def should_exclude?(item)
      url = item.respond_to?(:url) ? item.url : nil
      return true unless url

      # Exclude specific paths
      return true if EXCLUDED_PATHS.include?(url)

      # Exclude files with certain extensions
      return true if EXCLUDED_EXTENSIONS.any? { |ext| url.end_with?(ext) }

      # Exclude if item has sitemap: false in frontmatter
      return true if item.data['sitemap'] == false

      false
    end

    def page_url(page)
      return nil unless page.url

      "#{@site_url}#{page.url}"
    end

    def xml_escape(text)
      text.to_s
          .gsub('&', '&amp;')
          .gsub('<', '&lt;')
          .gsub('>', '&gt;')
          .gsub('"', '&quot;')
          .gsub("'", '&apos;')
    end

    def write_file(filename, content)
      file_path = File.join(@site.dest, filename)
      
      # Ensure the destination directory exists
      FileUtils.mkdir_p(@site.dest) unless Dir.exist?(@site.dest)
      
      File.write(file_path, content)
    end
  end
end
