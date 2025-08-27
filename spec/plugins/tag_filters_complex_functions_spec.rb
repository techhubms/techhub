require_relative 'spec_helper'
require_relative '../../_plugins/date_utils'
require_relative '../../_plugins/tag_filters'

class TagFilterComplexTester
  include Jekyll::TagFilters
  
  def initialize
    @site = nil
  end
end

RSpec.describe Jekyll::TagFilters do
  let(:filter_tester) { TagFilterComplexTester.new }
  
  describe '#generate_section_filter_buttons' do
    let(:preprocessed_data) do
      {
        'tag_counts' => {
          'ai' => 50,
          'github copilot' => 30,
          'news' => 25,
          'videos' => 15
        },
        'tag_mapping' => {
          'ai' => 'AI',
          'github copilot' => 'GitHub Copilot',
          'news' => 'News',
          'videos' => 'Videos'
        }
      }
    end
    
    let(:tag_display_limits) { [30, 100] }
    
    let(:sections) do
      {
        'ai' => {
          'category' => 'AI',
          'collections' => [
            { 'collection' => 'news' },
            { 'collection' => 'videos' }
          ]
        },
        'github-copilot' => {
          'category' => 'GitHub Copilot',
          'collections' => [
            { 'collection' => 'news' },
            { 'collection' => 'videos' }
          ]
        }
      }
    end
    
    before do
      allow(filter_tester).to receive(:generate_button_list).and_return([
        {
          'normalized' => 'ai',
          'display' => 'AI',
          'count' => 50,
          'css_class' => 'filter-width-2-digits',
          'is_top' => true
        },
        {
          'normalized' => 'news',
          'display' => 'News', 
          'count' => 25,
          'css_class' => 'filter-width-2-digits',
          'is_top' => false
        }
      ])
    end
    
    it 'generates section buttons from preprocessed data' do
      result = filter_tester.generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
      
      expect(result).to be_an(Array)
      expect(result.length).to eq(2)
    end
    
    it 'calls generate_button_list with correct parameters' do
      expect(filter_tester).to receive(:generate_button_list).with(
        hash_including('ai', 'news'),
        100, # max_tag_limit
        30   # top_tag_count
      )
      
      filter_tester.generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
    end
    
    it 'includes section categories that match preprocessed tags' do
      # Mock generate_button_list to verify the filter_data passed to it
      expect(filter_tester).to receive(:generate_button_list) do |filter_data, max_limit, top_count|
        expect(filter_data).to have_key('ai') # Should include AI section
        expect(filter_data['ai']['display']).to eq('AI')
        expect(filter_data['ai']['count']).to eq(50)
        []
      end
      
      filter_tester.generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
    end
    
    it 'includes collection types that are available in sections' do
      expect(filter_tester).to receive(:generate_button_list) do |filter_data, max_limit, top_count|
        expect(filter_data).to have_key('news') # Should include news collection
        expect(filter_data).to have_key('videos') # Should include videos collection
        []
      end
      
      filter_tester.generate_section_filter_buttons(preprocessed_data, tag_display_limits, sections)
    end
    
    it 'handles nil sections gracefully' do
      expect {
        filter_tester.generate_section_filter_buttons(preprocessed_data, tag_display_limits, nil)
      }.not_to raise_error
    end
    
    it 'handles empty preprocessed data' do
      empty_data = { 'tag_counts' => {}, 'tag_mapping' => {} }
      
      expect(filter_tester).to receive(:generate_button_list).with({}, 100, 30)
      
      filter_tester.generate_section_filter_buttons(empty_data, tag_display_limits, sections)
    end
  end

  describe '#generate_collection_filter_buttons' do
    let(:preprocessed_data) do
      {
        'tag_counts' => {
          'news' => 35,
          'videos' => 20,
          'posts' => 15,
          'community' => 10
        },
        'tag_mapping' => {
          'news' => 'News',
          'videos' => 'Videos',
          'posts' => 'Posts',
          'community' => 'Community'
        }
      }
    end
    
    let(:tag_display_limits) { [30, 100] }
    
    let(:section_data) do
      {
        'collections' => [
          { 'collection' => 'news' },
          { 'collection' => 'videos' },
          { 'collection' => 'posts' }
          # Note: community is not in this section's collections
        ]
      }
    end
    
    before do
      allow(filter_tester).to receive(:generate_button_list).and_return([
        {
          'normalized' => 'news',
          'display' => 'News',
          'count' => 35,
          'css_class' => 'filter-width-2-digits',
          'is_top' => true
        },
        {
          'normalized' => 'videos',
          'display' => 'Videos',
          'count' => 20,
          'css_class' => 'filter-width-2-digits',
          'is_top' => false
        }
      ])
    end
    
    it 'generates collection buttons for section from preprocessed data' do
      result = filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_data)
      
      expect(result).to be_an(Array)
      expect(result.length).to eq(2)
    end
    
    it 'only includes collections available in the section' do
      expect(filter_tester).to receive(:generate_button_list) do |filter_data, max_limit, top_count|
        expect(filter_data).to have_key('news')
        expect(filter_data).to have_key('videos')
        expect(filter_data).to have_key('posts')
        expect(filter_data).not_to have_key('community') # Not in section collections
        []
      end
      
      filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_data)
    end
    
    it 'uses correct counts from preprocessed data' do
      expect(filter_tester).to receive(:generate_button_list) do |filter_data, max_limit, top_count|
        expect(filter_data['news']['count']).to eq(35)
        expect(filter_data['videos']['count']).to eq(20)
        expect(filter_data['posts']['count']).to eq(15)
        []
      end
      
      filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_data)
    end
    
    it 'handles nil section_data gracefully' do
      expect {
        filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, nil)
      }.not_to raise_error
    end
    
    it 'handles section with no collections' do
      section_without_collections = {}
      
      expect(filter_tester).to receive(:generate_button_list).with({}, 100, 30)
      
      filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_without_collections)
    end
    
    it 'calls generate_button_list with correct parameters' do
      expect(filter_tester).to receive(:generate_button_list).with(
        hash_including('news', 'videos', 'posts'),
        100, # max_tag_limit
        30   # top_tag_count
      )
      
      filter_tester.generate_collection_filter_buttons(preprocessed_data, tag_display_limits, section_data)
    end
  end

  describe '#generate_all_filters' do
    let(:items) do
      [
        {
          'tags_normalized' => ['ai', 'machine-learning'],
          'tags' => ['AI', 'Machine Learning'],
          'date' => '2025-07-24',
          'categories' => ['AI'],
          'collection' => 'news'
        },
        {
          'tags_normalized' => ['github-copilot', 'ai'],
          'tags' => ['GitHub Copilot', 'AI'],
          'date' => '2025-07-20',
          'categories' => ['AI', 'GitHub Copilot'],
          'collection' => 'videos'
        }
      ]
    end
    
    let(:index_tag_mode) { 'tags' }
    
    let(:sections) do
      {
        'ai' => {
          'category' => 'AI',
          'collections' => [
            { 'collection' => 'news' },
            { 'collection' => 'videos' }
          ]
        }
      }
    end
    
    let(:section) { 'ai' }
    let(:category) { 'AI' }
    let(:collection) { 'news' }
    
    let(:tag_filter_config) do
      {
        'date_filters' => [
          { 'label' => 'Today', 'days' => 0 },
          { 'label' => 'Last 7 days', 'days' => 7 }
        ],
        'tag_display_limits' => {
          'collapsed_view_count' => 30,
          'expanded_view_max_count' => 100
        }
      }
    end
    
    before do
      # Mock the complex internal methods
      allow(filter_tester).to receive(:calculate_date_cutoffs).and_return({ 0 => 1721779200, 7 => 1721174400 })
      allow(filter_tester).to receive(:process_all_item_data_parallel).and_return({
        tag_data: {
          'tag_counts' => { 'ai' => 2, 'machine-learning' => 1 },
          'tag_mapping' => { 'ai' => 'AI', 'machine-learning' => 'Machine Learning' }
        },
        date_counts: { 0 => 1, 7 => 2 },
        filter_data: [],
        item_epochs: [1721779200, 1721433600]
      })
      allow(filter_tester).to receive(:determine_eligible_date_filters).and_return(['Today', 'Last 7 days'])
      allow(filter_tester).to receive(:generate_date_filter_buttons).and_return([
        { 'label' => 'Today', 'count' => 1 },
        { 'label' => 'Last 7 days', 'count' => 2 }
      ])
      allow(filter_tester).to receive(:generate_tag_filter_buttons).and_return([
        { 'normalized' => 'ai', 'display' => 'AI', 'count' => 2 }
      ])
    end
    
    it 'generates all filters successfully' do
      result = filter_tester.generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
      
      expect(result).to be_a(Hash)
      expect(result).to have_key('date_filter_buttons')
      expect(result).to have_key('mode_filter_buttons')
      expect(result).to have_key('preprocessed_data')
      expect(result).to have_key('tag_filter_data')
      expect(result).to have_key('tag_relationships')
      expect(result).to have_key('date_filter_config')
    end
    
    it 'raises error for empty items' do
      expect {
        filter_tester.generate_all_filters([], index_tag_mode, sections, section, category, collection, tag_filter_config)
      }.to raise_error(RuntimeError, "Items must be a non-empty array.")
    end
    
    it 'raises error for nil items' do
      expect {
        filter_tester.generate_all_filters(nil, index_tag_mode, sections, section, category, collection, tag_filter_config)
      }.to raise_error(RuntimeError, "Items must be a non-empty array.")
    end
    
    it 'handles sections mode' do
      allow(filter_tester).to receive(:generate_section_filter_buttons).and_return([
        { 'normalized' => 'ai', 'display' => 'AI', 'count' => 2 }
      ])
      
      result = filter_tester.generate_all_filters(items, 'sections', sections, section, category, collection, tag_filter_config)
      
      expect(result['mode_filter_buttons']).to be_an(Array)
    end
    
    it 'handles collections mode' do
      allow(filter_tester).to receive(:generate_collection_filter_buttons).and_return([
        { 'normalized' => 'news', 'display' => 'News', 'count' => 1 }
      ])
      
      result = filter_tester.generate_all_filters(items, 'collections', sections, section, category, collection, tag_filter_config)
      
      expect(result['mode_filter_buttons']).to be_an(Array)
    end
    
    it 'raises error for invalid index_tag_mode' do
      expect {
        filter_tester.generate_all_filters(items, 'invalid_mode', sections, section, category, collection, tag_filter_config)
      }.to raise_error(RuntimeError, /Invalid index_tag_mode: "invalid_mode"/)
    end
    
    it 'raises error when no eligible date filters found' do
      allow(filter_tester).to receive(:determine_eligible_date_filters).and_return([])
      
      expect {
        filter_tester.generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
      }.to raise_error(RuntimeError, /No eligible date filters found\. Please check your configuration/)
    end
    
    it 'calls extract_date_filter_config correctly' do
      expect(filter_tester).to receive(:extract_date_filter_config).with(tag_filter_config).and_return(['Today:0', 'Last 7 days:7'])
      
      filter_tester.generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
    end
    
    it 'calls extract_tag_display_limits correctly' do
      expect(filter_tester).to receive(:extract_tag_display_limits).with(tag_filter_config).and_return([30, 100])
      
      filter_tester.generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
    end
  end
end
