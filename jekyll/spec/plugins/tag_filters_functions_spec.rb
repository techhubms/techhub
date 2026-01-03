require_relative 'spec_helper'
require_relative '../../_plugins/date_utils'
require_relative '../../_plugins/tag_filters'

class TagFilterMethodTester
  include Jekyll::TagFilters
  
  def initialize
    @site = nil
  end
end

RSpec.describe Jekyll::TagFilters do
  let(:filter_tester) { TagFilterMethodTester.new }
  
  describe '#extract_date_filter_config' do
    it 'extracts date filter configuration from tag filter config' do
      tag_filter_config = {
        'date_filters' => [
          { 'label' => 'Today', 'days' => 0 },
          { 'label' => 'Last 7 days', 'days' => 7 },
          { 'label' => 'Last 30 days', 'days' => 30 }
        ]
      }
      
      result = filter_tester.extract_date_filter_config(tag_filter_config)
      
      expect(result).to eq(['Today:0', 'Last 7 days:7', 'Last 30 days:30'])
    end
    
    it 'returns empty array when no date_filters in config' do
      tag_filter_config = { 'other_config' => 'value' }
      
      result = filter_tester.extract_date_filter_config(tag_filter_config)
      
      expect(result).to eq([])
    end
    
    it 'returns empty array when config is nil' do
      result = filter_tester.extract_date_filter_config(nil)
      
      expect(result).to eq([])
    end
    
    it 'handles empty date_filters array' do
      tag_filter_config = { 'date_filters' => [] }
      
      result = filter_tester.extract_date_filter_config(tag_filter_config)
      
      expect(result).to eq([])
    end
    
    it 'does not strip whitespace from labels' do
      tag_filter_config = {
        'date_filters' => [
          { 'label' => '  Today  ', 'days' => 0 },
          { 'label' => '  Last 7 days  ', 'days' => 7 }
        ]
      }
      
      result = filter_tester.extract_date_filter_config(tag_filter_config)
      
      expect(result).to eq(['  Today  :0', '  Last 7 days  :7'])
    end
  end

  describe '#extract_tag_display_limits' do
    it 'extracts tag display limits from configuration' do
      tag_filter_config = {
        'tag_display_limits' => {
          'collapsed_view_count' => 25,
          'expanded_view_max_count' => 75
        }
      }
      
      result = filter_tester.extract_tag_display_limits(tag_filter_config)
      
      expect(result).to eq([25, 75])
    end
    
    it 'returns default values when limits are not specified' do
      tag_filter_config = {
        'tag_display_limits' => {}
      }
      
      result = filter_tester.extract_tag_display_limits(tag_filter_config)
      
      expect(result).to eq([30, 100])
    end
    
    it 'uses partial configuration with defaults' do
      tag_filter_config = {
        'tag_display_limits' => {
          'collapsed_view_count' => 15
        }
      }
      
      result = filter_tester.extract_tag_display_limits(tag_filter_config)
      
      expect(result).to eq([15, 100])
    end
    
    it 'raises error when tag_display_limits is missing' do
      tag_filter_config = { 'other_config' => 'value' }
      
      expect {
        filter_tester.extract_tag_display_limits(tag_filter_config)
      }.to raise_error(RuntimeError, /Tag filter configuration must include 'tag_display_limits'/)
    end
    
    it 'raises error when config is nil' do
      expect {
        filter_tester.extract_tag_display_limits(nil)
      }.to raise_error(RuntimeError, /Tag filter configuration must include 'tag_display_limits'/)
    end
  end

  describe '#generate_button_list' do
    let(:filter_data) do
      {
        'ai' => {
          'normalized' => 'ai',
          'display' => 'AI',
          'count' => 150
        },
        'github-copilot' => {
          'normalized' => 'github-copilot',
          'display' => 'GitHub Copilot',
          'count' => 75
        },
        'visual-studio' => {
          'normalized' => 'visual-studio',
          'display' => 'Visual Studio',
          'count' => 25
        }
      }
    end
    
    it 'generates button list sorted by display name' do
      result = filter_tester.generate_button_list(filter_data, 100, 30)
      
      expect(result.length).to eq(3)
      expect(result[0]['display']).to eq('AI')
      expect(result[1]['display']).to eq('GitHub Copilot')
      expect(result[2]['display']).to eq('Visual Studio')
    end
    
    it 'limits results when max_tag_limit is exceeded' do
      result = filter_tester.generate_button_list(filter_data, 2, 30)
      
      expect(result.length).to eq(2)
      # Should keep highest count items (AI and GitHub Copilot)
      displays = result.map { |r| r['display'] }
      expect(displays).to include('AI', 'GitHub Copilot')
    end
    
    it 'returns empty array for empty filter data' do
      result = filter_tester.generate_button_list({}, 100, 30)
      
      expect(result).to eq([])
    end
    
    it 'includes correct CSS width classes and top tag markers based on count' do
      result = filter_tester.generate_button_list(filter_data, 100, 30)
      
      ai_button = result.find { |r| r['display'] == 'AI' }
      github_button = result.find { |r| r['display'] == 'GitHub Copilot' }
      vs_button = result.find { |r| r['display'] == 'Visual Studio' }
      
      expect(ai_button['width_class']).to eq('filter-width-3-digits')
      expect(github_button['width_class']).to eq('filter-width-2-digits')
      expect(vs_button['width_class']).to eq('filter-width-2-digits')
    end
    
    it 'marks top filters correctly' do
      result = filter_tester.generate_button_list(filter_data, 100, 2)
      
      ai_button = result.find { |r| r['display'] == 'AI' }
      github_button = result.find { |r| r['display'] == 'GitHub Copilot' }
      vs_button = result.find { |r| r['display'] == 'Visual Studio' }
      
      expect(ai_button['is_top_tag']).to be true
      expect(github_button['is_top_tag']).to be true
      expect(vs_button['is_top_tag']).to be false
    end
  end



  describe '#preprocess_filter_data' do
    let(:items) do
      [
        {
          'tags_normalized' => ['ai', 'machine-learning'],
          'tags' => ['AI', 'Machine Learning']
        },
        {
          'tags_normalized' => ['github-copilot', 'ai'],
          'tags' => ['GitHub Copilot', 'AI']
        }
      ]
    end
    
    it 'processes filter data correctly' do
      result = filter_tester.preprocess_filter_data(items, 'AI', 'news')
      
      expect(result).to be_a(Hash)
      expect(result).to have_key('tag_counts')
      expect(result).to have_key('original_tag_counts')
      expect(result).to have_key('tag_mapping')
      expect(result).to have_key('redundant_tags')
    end
    
    it 'builds tag counts correctly' do
      result = filter_tester.preprocess_filter_data(items, 'AI', 'news')
      
      tag_counts = result['tag_counts']
      # Note: AI should not be counted because it's in the redundant tags (category)
      expect(tag_counts['machine-learning']).to eq(1)
      expect(tag_counts['github-copilot']).to eq(1)
      # AI appears in both items but should be excluded as redundant
    end
    
    it 'excludes redundant tags' do
      result = filter_tester.preprocess_filter_data(items, 'AI', 'news')
      
      redundant_tags = result['redundant_tags']
      expect(redundant_tags).to include('ai') # category
      expect(redundant_tags).to include('news') # collection
    end
    
    it 'returns empty hash for nil items' do
      result = filter_tester.preprocess_filter_data(nil, 'AI', 'news')
      
      expect(result).to eq({})
    end
    
    it 'handles array category' do
      result = filter_tester.preprocess_filter_data(items, ['AI', 'GitHub Copilot'], 'news')
      
      redundant_tags = result['redundant_tags']
      expect(redundant_tags).to include('ai')
      expect(redundant_tags).to include('github copilot')
    end
    
    it 'creates tag mapping for display names' do
      result = filter_tester.preprocess_filter_data(items, 'AI', 'news')
      
      tag_mapping = result['tag_mapping']
      expect(tag_mapping).to be_a(Hash)
      # Should map normalized tags to their display forms
    end
  end
end
