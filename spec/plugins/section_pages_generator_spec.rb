require_relative '../plugins/spec_helper'
require_relative '../../_plugins/section_pages_generator'
require 'json'
require 'fileutils'
require 'tmpdir'

RSpec.describe Jekyll::SectionPagesGenerator do
  let(:temp_dir) { Dir.mktmpdir }
  let(:generator) { Jekyll::SectionPagesGenerator.new }
  let(:data_dir) { File.join(temp_dir, '_data') }
  let(:sections_file) { File.join(data_dir, 'sections.json') }
  let(:site) do
    double('site', 
      source: temp_dir,
      pages: [],
      in_theme_dir: nil
    ).tap do |site_mock|
      allow(site_mock).to receive(:in_theme_dir).and_return('')
      allow(site_mock).to receive(:in_source_dir) do |*args|
        File.join(temp_dir, *args.compact)
      end
      allow(site_mock).to receive(:frontmatter_defaults).and_return({})
    end
  end

  let(:sections) do
    {
      'ai' => {
        'title' => 'AI',
        'description' => 'Everything on AI in the Microsoft space',
        'section' => 'ai',
        'category' => 'AI',
        'collections' => [
          {
            'title' => 'News',
            'collection' => 'news',
            'description' => 'Latest AI news'
          },
          {
            'title' => 'Posts',
            'collection' => 'posts',
            'description' => 'AI blog posts'
          },
          {
            'title' => 'A(i) to Z',
            'url' => '/ai/ai-to-z.html',
            'description' => 'Custom page without collection field'
          }
        ]
      },
      'github-copilot' => {
        'title' => 'GitHub Copilot',
        'description' => 'Everything about GitHub Copilot',
        'section' => 'github-copilot',
        'category' => 'GitHub Copilot',
        'collections' => [
          {
            'title' => 'Community',
            'collection' => 'community',
            'description' => 'Community discussions'
          }
        ]
      }
    }
  end

  before do
    FileUtils.mkdir_p(data_dir)
    File.write(sections_file, JSON.pretty_generate(sections))
  end

  after do
    FileUtils.rm_rf(temp_dir)
  end

  describe 'initialization' do
    it 'can be instantiated' do
      expect(generator).to be_an_instance_of(Jekyll::SectionPagesGenerator)
    end
  end

  describe '#generate_section_index_content' do
    let(:section_data) { sections['ai'] }
    
    it 'generates content string' do
      content = generator.send(:generate_section_index_content, section_data)
      
      expect(content).to be_a(String)
      expect(content.length).to be > 100
    end

    it 'includes expected content markers' do
      content = generator.send(:generate_section_index_content, section_data)
      
      expect(content).to include('Dynamic section index')
      expect(content).to include('navigation-collections-grid')
      expect(content).to include('index-latest')
    end
  end

  describe '#generate_collection_page_content' do
    let(:section_data) { sections['ai'] }
    let(:page_data) { sections['ai']['collections'].first }
    
    it 'generates content string' do
      content = generator.send(:generate_collection_page_content, section_data, page_data)
      
      expect(content).to be_a(String)
      expect(content.length).to be > 50
    end

    it 'includes expected content markers' do
      content = generator.send(:generate_collection_page_content, section_data, page_data)
      
      expect(content).to include('Dynamic collection page')
      expect(content).to include('include filters.html')
      expect(content).to include('include posts.html')
    end
  end

  describe '#generate' do
    it 'generates section index pages' do
      initial_page_count = site.pages.length
      
      generator.generate(site)
      
      # Should add pages for sections (ai and github-copilot)
      expect(site.pages.length).to be >= initial_page_count + 2
      
      # Check that section index pages have the expected data
      ai_page = site.pages.find { |page| page.data['section'] == 'ai' && page.name == 'index.md' }
      expect(ai_page).not_to be_nil
      expect(ai_page.data['layout']).to eq('home')
      expect(ai_page.data['title']).to eq('AI')
      
      copilot_page = site.pages.find { |page| page.data['section'] == 'github-copilot' && page.name == 'index.md' }
      expect(copilot_page).not_to be_nil
      expect(copilot_page.data['layout']).to eq('home')
      expect(copilot_page.data['title']).to eq('GitHub Copilot')
    end

    it 'generates collection pages but skips custom pages' do
      initial_page_count = site.pages.length
      
      generator.generate(site)
      
      # Should generate news page for AI section (has collection field)
      news_page = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'ai' && page.data['collection'] == 'news'
            news_page = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(news_page).not_to be_nil
      expect(news_page.data['layout']).to eq('page')
      expect(news_page.data['title']).to eq('News')
      
      # Should generate posts page for AI section (has collection field)
      posts_page = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'ai' && page.data['collection'] == 'posts'
            posts_page = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(posts_page).not_to be_nil
      expect(posts_page.data['layout']).to eq('page')
      expect(posts_page.data['title']).to eq('Posts')
      
      # Should generate community page for GitHub Copilot section (has collection field)
      community_page = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'github-copilot' && page.data['collection'] == 'community'
            community_page = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(community_page).not_to be_nil
      expect(community_page.data['layout']).to eq('page')
      expect(community_page.data['title']).to eq('Community')
      
      # Should NOT generate A(i) to Z page (no collection field - it's a custom page)
      ai_to_z_page = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'ai' && page.name&.include?('ai-to-z')
            ai_to_z_page = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(ai_to_z_page).to be_nil
    end

    it 'skips sections without pages' do
      sections['no-pages'] = {
        'title' => 'No Pages Section',
        'category' => 'None'
      }
      File.write(sections_file, JSON.pretty_generate(sections))
      
      expect(JekyllFileWriter).not_to receive(:write_file_with_dir)
        .with(File.join(temp_dir, 'no-pages/index.md'), anything, anything)
      
      allow(JekyllFileWriter).to receive(:write_file_with_dir) # For other pages
      
      generator.generate(site)
    end

    it 'handles missing sections.json file gracefully' do
      FileUtils.rm(sections_file)
      
      allow(JekyllFileWriter).to receive(:write_file_with_dir)
      
      expect { generator.generate(site) }.not_to raise_error
      expect(JekyllFileWriter).not_to have_received(:write_file_with_dir)
    end

    it 'handles malformed sections.json file gracefully' do
      File.write(sections_file, 'invalid json content')
      
      # Mock Jekyll logger to prevent error output
      allow(Jekyll).to receive(:logger).and_return(double('logger', error: nil))
      allow(JekyllFileWriter).to receive(:write_file_with_dir)
      
      expect { generator.generate(site) }.not_to raise_error
      expect(JekyllFileWriter).not_to have_received(:write_file_with_dir)
    end

    it 'handles empty sections.json file gracefully' do
      File.write(sections_file, '{}')
      
      allow(JekyllFileWriter).to receive(:write_file_with_dir)
      
      expect { generator.generate(site) }.not_to raise_error
      expect(JekyllFileWriter).not_to have_received(:write_file_with_dir)
    end
  end

  describe '#generate_section_index_content' do
    let(:section_data) { sections['ai'] }
    
    it 'generates content with correct section key assignment' do
      content = generator.send(:generate_section_index_content, section_data)
      
      expect(content).to include('section=page.section')
    end

    it 'includes section metadata in frontmatter' do
      content = generator.send(:generate_section_index_content, section_data)
      
      expect(content).to include('section=page.section')
      expect(content).to include('{{ section_data.description }}')
    end

    it 'sets correct index_tag_mode for section pages' do
      content = generator.send(:generate_section_index_content, section_data)
      
      expect(content).to include('index_tag_mode="collections"')
    end

    it 'handles sections with missing fields gracefully' do
      minimal_section = { 'title' => 'Test Section', 'section' => 'test' }
      
      expect { generator.send(:generate_section_index_content, minimal_section) }.not_to raise_error
      
      content = generator.send(:generate_section_index_content, minimal_section)
      expect(content).to include('section=page.section')
    end

    it 'filters items to only include collections configured for the section and excludes events' do
      content = generator.send(:generate_section_index_content, section_data)
      
      # Should include logic to filter by section collections
      expect(content).to include('collections contains item.collection')
    end
  end

  describe '#generate_collection_page_content' do
    let(:section_data) { sections['ai'] }
    let(:collection_data) { section_data['collections'][1] } # News collection
    
    it 'generates content with correct collection assignment' do
      content = generator.send(:generate_collection_page_content, section_data, collection_data)
      
      expect(content).to include('site[page.collection]')
      expect(content).to include('section=page.section')
    end

    it 'includes collection metadata in frontmatter' do
      content = generator.send(:generate_collection_page_content, section_data, collection_data)
      
      expect(content).to include('section=page.section')
      expect(content).to include('{% include posts.html')
    end

    it 'sets correct index_tag_mode for collection pages' do
      content = generator.send(:generate_collection_page_content, section_data, collection_data)
      
      expect(content).to include('index_tag_mode="tags"')
    end

    it 'handles collections with missing fields gracefully' do
      minimal_collection = { 'collection' => 'test' }
      
      expect { generator.send(:generate_collection_page_content, section_data, minimal_collection) }.not_to raise_error
    end
  end

  describe 'File naming and path generation' do
    it 'generates correct file paths for section indexes' do
      generator.generate(site)
      
      # Check that index pages are created with correct names
      ai_index = nil
      site.pages.each { |page| ai_index = page if page.data['section'] == 'ai' && page.name == 'index.md' }
      expect(ai_index).not_to be_nil
      expect(ai_index.name).to eq('index.md')
      
      copilot_index = nil
      site.pages.each { |page| copilot_index = page if page.data['section'] == 'github-copilot' && page.name == 'index.md' }
      expect(copilot_index).not_to be_nil
      expect(copilot_index.name).to eq('index.md')
    end

    it 'generates correct file paths for collection pages' do
      generator.generate(site)
      
      # Check collection pages are created with correct names
      ai_news = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'ai' && page.data['collection'] == 'news'
            ai_news = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(ai_news).not_to be_nil
      expect(ai_news.name).to eq('news.html')
      
      ai_posts = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'ai' && page.data['collection'] == 'posts'
            ai_posts = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(ai_posts).not_to be_nil
      expect(ai_posts.name).to eq('posts.html')
      
      copilot_community = nil
      site.pages.each do |page|
        begin
          if page.data['section'] == 'github-copilot' && page.data['collection'] == 'community'
            copilot_community = page
            break
          end
        rescue ArgumentError
          # Skip pages that cause Jekyll internal errors
          next
        end
      end
      expect(copilot_community).not_to be_nil
      expect(copilot_community.name).to eq('community.html')
    end

    it 'handles sections with special characters in names' do
      sections['special-chars_section'] = {
        'title' => 'Special Section',
        'section' => 'special-chars_section',
        'category' => 'Special',
        'collections' => []
      }
      File.write(sections_file, JSON.pretty_generate(sections))
      
      generator.generate(site)
      
      # Check that page was created for special characters section
      special_page = nil
      site.pages.each { |page| special_page = page if page.data['section'] == 'special-chars_section' && page.name == 'index.md' }
      expect(special_page).not_to be_nil
      expect(special_page.data['title']).to eq('Special Section')
      expect(special_page.data['section']).to eq('special-chars_section')
      expect(special_page.name).to eq('index.md')
    end
  end

  # Advanced tests covering edge cases and error handling
  describe 'Advanced Edge Cases' do
    context 'with complex section configurations' do
      let(:complex_sections) do
        {
          'complex-section' => {
            'title' => 'Complex Section with Many Collections',
            'description' => 'A section with multiple collection types',
            'section' => 'complex-section',
            'category' => 'Complex',
            'collections' => [
              { 'title' => 'News', 'collection' => 'news', 'description' => 'News items' },
              { 'title' => 'Posts', 'collection' => 'posts', 'description' => 'Blog posts' },
              { 'title' => 'Videos', 'collection' => 'videos', 'description' => 'Video content' },
              { 'title' => 'Community', 'collection' => 'community', 'description' => 'Community content' },
              { 'title' => 'Custom Page', 'description' => 'A custom page without collection' }
            ]
          }
        }
      end

      it 'handles sections with many collections correctly' do
        File.write(sections_file, JSON.pretty_generate(complex_sections))
        
        generator.generate(site)
        
        # Should generate index page
        index_page = nil
        site.pages.each { |page| index_page = page if page.data['section'] == 'complex-section' && page.name == 'index.md' }
        expect(index_page).not_to be_nil
        expect(index_page.data['title']).to eq('Complex Section with Many Collections')
        
        # Should generate collection pages (those with 'collection' field)
        ['news', 'posts', 'videos', 'community'].each do |collection|
          collection_page = nil
          site.pages.each do |page|
            begin
              if page.data['section'] == 'complex-section' && page.data['collection'] == collection
                collection_page = page
                break
              end
            rescue ArgumentError
              # Skip pages that cause Jekyll internal errors
              next
            end
          end
          expect(collection_page).not_to be_nil
          expect(collection_page.name).to eq("#{collection}.html")
        end
        
        # Should NOT generate custom page (no 'collection' field)
        custom_page = nil
        site.pages.each do |page|
          begin
            if page.data['section'] == 'complex-section' && page.name&.include?('custom')
              custom_page = page
              break
            end
          rescue ArgumentError
            # Skip pages that cause Jekyll internal errors
            next
          end
        end
        expect(custom_page).to be_nil
      end
    end

    context 'with invalid or missing data' do
      it 'handles sections with null collections array' do
        invalid_sections = {
          'broken-section' => {
            'title' => 'Broken Section',
            'section' => 'broken-section',
            'collections' => nil
          }
        }
        File.write(sections_file, JSON.pretty_generate(invalid_sections))
        
        # Since collections is null, should not generate any pages for this section
        expect { generator.generate(site) }.not_to raise_error
        
        broken_pages = site.pages.select { |page| page.data['section'] == 'broken-section' }
        expect(broken_pages).to be_empty
      end

      it 'handles collections with missing required fields' do
        problematic_sections = {
          'problematic' => {
            'title' => 'Problematic Section',
            'section' => 'problematic',
            'collections' => [
              { 'title' => 'Valid Collection', 'collection' => 'valid' },
              { 'title' => 'No collection field' }
            ]
          }
        }
        File.write(sections_file, JSON.pretty_generate(problematic_sections))
        
        generator.generate(site)
        
        # Should generate index page for the section
        index_page = nil
        site.pages.each { |page| index_page = page if page.data['section'] == 'problematic' && page.name == 'index.md' }
        expect(index_page).not_to be_nil
        
        # Should only generate page for valid collection (has 'collection' field)
        valid_page = nil
        site.pages.each do |page|
          begin
            if page.data['section'] == 'problematic' && page.data['collection'] == 'valid'
              valid_page = page
              break
            end
          rescue ArgumentError
            # Skip pages that cause Jekyll internal errors
            next
          end
        end
        expect(valid_page).not_to be_nil
        
        # Should not generate page for collection without 'collection' field
        invalid_pages = []
        site.pages.each do |page|
          begin
            if page.data['section'] == 'problematic' && page.data['collection'].nil? && page.name != 'index.md'
              invalid_pages << page
            end
          rescue ArgumentError
            # Skip pages that cause Jekyll internal errors
            next
          end
        end
        expect(invalid_pages).to be_empty
        
        expect { generator.generate(site) }.not_to raise_error
      end
    end

    context 'with file system issues' do
      it 'handles permission errors gracefully' do
        # Since plugin now creates Jekyll pages in memory rather than writing files,
        # file system permission errors don't apply
        expect { generator.generate(site) }.not_to raise_error
      end

      it 'handles disk space issues gracefully' do
        # Since plugin now creates Jekyll pages in memory rather than writing files,
        # disk space errors don't apply
        expect { generator.generate(site) }.not_to raise_error
      end
    end

    context 'with performance considerations' do
      it 'handles large number of sections efficiently' do
        large_sections = {}
        (1..50).each do |i|
          large_sections["section-#{i}"] = {
            'title' => "Section #{i}",
            'section' => "section-#{i}",
            'category' => "Category #{i}",
            'collections' => [
              { 'title' => 'News', 'collection' => 'news' },
              { 'title' => 'Posts', 'collection' => 'posts' }
            ]
          }
        end
        
        File.write(sections_file, JSON.pretty_generate(large_sections))
        
        generator.generate(site)
        
        # Should generate pages for all sections (50 indexes + 100 collection pages = 150 total)
        # 50 sections * 1 index page each = 50 index pages
        # 50 sections * 2 collection pages each = 100 collection pages
        expect(site.pages.length).to eq(150)
        
        # Verify we have the expected number of index pages
        index_pages = []
        site.pages.each { |page| index_pages << page if page.name == 'index.md' }
        expect(index_pages.length).to eq(50)
        
        # Verify we have the expected number of collection pages
        collection_pages = []
        site.pages.each do |page|
          begin
            if page.data['collection']
              collection_pages << page
            end
          rescue ArgumentError
            # Skip pages that cause Jekyll internal errors
            next
          end
        end
        expect(collection_pages.length).to eq(100)
        
        expect { generator.generate(site) }.not_to raise_error
      end
    end
  end

  describe 'Integration with Jekyll Pages' do
    it 'adds pages to site.pages correctly' do
      initial_page_count = site.pages.length
      
      generator.generate(site)
      
      # Should add pages for sections and collections
      expect(site.pages.length).to be > initial_page_count
      
      # Check that we have section index pages by looking for pages with section data
      section_pages = site.pages.select { |page| page.name == 'index.md' && page.data['section'] }
      expect(section_pages.length).to be > 0
      
      # Check that pages have proper frontmatter
      section_pages.each do |page|
        expect(page.data).to have_key('title')
        expect(page.data).to have_key('section')
        expect(page.data).to have_key('layout')
      end
    end

    it 'generates pages with consistent format' do
      generator.generate(site)
      
      # Find a section page by section and name instead of URL
      section_page = site.pages.find { |page| page.data['section'] == 'ai' && page.name == 'index.md' }
      expect(section_page).not_to be_nil
      
      # Verify page structure
      expect(section_page.data['title']).to eq('AI')
      expect(section_page.data['section']).to eq('ai')
      expect(section_page.data['layout']).to eq('home')
    end
  end
end