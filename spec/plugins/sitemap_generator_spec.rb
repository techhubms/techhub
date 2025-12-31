# frozen_string_literal: true

require_relative '../plugins/spec_helper'
require_relative '../../_plugins/sitemap_generator'

RSpec.describe Jekyll::SitemapWriter do
  let(:site) do
    instance_double(
      Jekyll::Site,
      config: site_config,
      dest: dest_dir,
      pages: pages,
      collections: collections
    )
  end

  let(:site_config) do
    {
      'url' => 'https://example.com',
      'baseurl' => ''
    }
  end

  let(:dest_dir) { '/tmp/jekyll_test' }
  let(:pages) { [] }
  let(:collections) { {} }
  let(:writer) { described_class.new(site) }

  before do
    FileUtils.mkdir_p(dest_dir)
  end

  after do
    FileUtils.rm_rf(dest_dir)
  end

  describe '#generate_sitemap and #generate_robots_txt' do
    context 'with basic pages' do
      let(:page1) do
        instance_double(
          Jekyll::Page,
          url: '/',
          data: {}
        )
      end

      let(:page2) do
        instance_double(
          Jekyll::Page,
          url: '/about.html',
          data: {}
        )
      end

      let(:pages) { [page1, page2] }

      it 'generates sitemap.xml' do
        writer.generate_sitemap

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        expect(File.exist?(sitemap_path)).to be true

        content = File.read(sitemap_path)
        expect(content).to include('<?xml version="1.0" encoding="UTF-8"?>')
        expect(content).to include('<urlset')
        expect(content).to include('<loc>https://example.com/</loc>')
        expect(content).to include('<loc>https://example.com/about.html</loc>')
        expect(content).to include('</urlset>')
      end

      it 'generates robots.txt' do
        writer.generate_robots_txt

        robots_path = File.join(dest_dir, 'robots.txt')
        expect(File.exist?(robots_path)).to be true

        content = File.read(robots_path)
        expect(content).to eq("Sitemap: https://example.com/sitemap.xml\n")
      end
    end

    context 'with baseurl configured' do
      let(:site_config) do
        {
          'url' => 'https://example.com',
          'baseurl' => '/blog'
        }
      end

      let(:pages) { [instance_double(Jekyll::Page, url: '/', data: {})] }

      it 'includes baseurl in URLs' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)
        expect(content).to include('<loc>https://example.com/blog/</loc>')

        robots_path = File.join(dest_dir, 'robots.txt')
        robots_content = File.read(robots_path)
        expect(robots_content).to eq("Sitemap: https://example.com/blog/sitemap.xml\n")
      end
    end

    context 'with pages excluded via frontmatter' do
      let(:included_page) do
        instance_double(
          Jekyll::Page,
          url: '/index.html',
          data: {}
        )
      end

      let(:excluded_page) do
        instance_double(
          Jekyll::Page,
          url: '/private.html',
          data: { 'sitemap' => false }
        )
      end

      let(:pages) { [included_page, excluded_page] }

      it 'excludes pages with sitemap: false' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        expect(content).to include('/index.html')
        expect(content).not_to include('/private.html')
      end
    end

    context 'with excluded file types' do
      let(:html_page) do
        instance_double(
          Jekyll::Page,
          url: '/page.html',
          data: {}
        )
      end

      let(:xml_page) do
        instance_double(
          Jekyll::Page,
          url: '/feed.xml',
          data: {}
        )
      end

      let(:json_page) do
        instance_double(
          Jekyll::Page,
          url: '/api.json',
          data: {}
        )
      end

      let(:pages) { [html_page, xml_page, json_page] }

      it 'excludes XML and JSON files' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        expect(content).to include('/page.html')
        expect(content).not_to include('/feed.xml')
        expect(content).not_to include('/api.json')
      end
    end

    context 'with 404 page' do
      let(:normal_page) do
        instance_double(
          Jekyll::Page,
          url: '/index.html',
          data: {}
        )
      end

      let(:error_page) do
        instance_double(
          Jekyll::Page,
          url: '/404.html',
          data: {}
        )
      end

      let(:pages) { [normal_page, error_page] }

      it 'excludes 404 page' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        expect(content).to include('/index.html')
        expect(content).not_to include('/404.html')
      end
    end

    context 'with collection documents' do
      let(:doc1) do
        instance_double(
          Jekyll::Document,
          url: '/posts/first-post.html',
          data: {}
        )
      end

      let(:doc2) do
        instance_double(
          Jekyll::Document,
          url: '/posts/second-post.html',
          data: {}
        )
      end

      let(:collection) do
        instance_double(
          Jekyll::Collection,
          docs: [doc1, doc2]
        )
      end

      let(:collections) { { 'posts' => collection } }
      let(:pages) { [] }

      it 'excludes collection documents from sitemap' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        expect(content).not_to include('/posts/first-post.html')
        expect(content).not_to include('/posts/second-post.html')
      end
    end

    context 'with special characters in URLs' do
      let(:page_with_special_chars) do
        instance_double(
          Jekyll::Page,
          url: '/test&page.html',
          data: {}
        )
      end

      let(:pages) { [page_with_special_chars] }

      it 'escapes special XML characters' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        expect(content).to include('test&amp;page.html')
        expect(content).not_to include('test&page.html')
      end
    end

    context 'with duplicate URLs' do
      let(:page1) do
        instance_double(
          Jekyll::Page,
          url: '/same.html',
          data: {}
        )
      end

      let(:page2) do
        instance_double(
          Jekyll::Page,
          url: '/same.html',
          data: {}
        )
      end

      let(:pages) { [page1, page2] }

      it 'removes duplicate URLs' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)

        # Count occurrences of the URL
        url_count = content.scan(%r{<loc>https://example\.com/same\.html</loc>}).length
        expect(url_count).to eq(1)
      end
    end

    context 'when URL is missing in site config' do
      let(:site_config) { {} }
      let(:pages) { [instance_double(Jekyll::Page, url: '/', data: {})] }

      it 'uses default localhost URL' do
        writer.generate_sitemap; writer.generate_robots_txt

        sitemap_path = File.join(dest_dir, 'sitemap.xml')
        content = File.read(sitemap_path)
        expect(content).to include('http://localhost:4000/')

        robots_path = File.join(dest_dir, 'robots.txt')
        robots_content = File.read(robots_path)
        expect(robots_content).to include('http://localhost:4000/sitemap.xml')
      end
    end
  end
end
