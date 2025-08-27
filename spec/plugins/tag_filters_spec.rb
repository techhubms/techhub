require_relative 'spec_helper'
require_relative '../../_plugins/date_utils'
require_relative '../../_plugins/tag_filters'

class TagFilterTester
  include Jekyll::TagFilters
  
  def initialize
    @site = nil  # Simple nil for testing
    @cached_split = {}
    @cached_normalize = {}
  end
end

RSpec.describe Jekyll::TagFilters do
  let(:filter_tester) { TagFilterTester.new }
  
  describe '#cached_string_split' do
    it 'splits string by whitespace by default' do
      result = filter_tester.cached_string_split('hello world test')
      expect(result).to eq(['hello', 'world', 'test'])
    end

    it 'splits string by custom pattern' do
      result = filter_tester.cached_string_split('hello,world,test', /,/)
      expect(result).to eq(['hello', 'world', 'test'])
    end

    it 'handles empty string' do
      result = filter_tester.cached_string_split('')
      expect(result).to eq([])
    end

    it 'caches results for performance' do
      string = 'hello world'
      result1 = filter_tester.cached_string_split(string)
      result2 = filter_tester.cached_string_split(string)
      expect(result1).to eq(result2)
      expect(result1.object_id).to eq(result2.object_id)
    end
  end

  describe '#cached_normalize' do
    it 'normalizes text to lowercase' do
      result = filter_tester.cached_normalize('GitHub Copilot')
      expect(result).to eq('github copilot')
    end

    it 'handles special characters and spaces' do
      result = filter_tester.cached_normalize('AI/ML & Data Science')
      expect(result).to eq('ai ml data science')  # Special chars become spaces, then squeezed
    end

    it 'handles empty string' do
      result = filter_tester.cached_normalize('')
      expect(result).to eq('')
    end

    it 'handles nil' do
      result = filter_tester.cached_normalize(nil)
      expect(result).to eq('')
    end

    it 'caches results for performance' do
      text = 'GitHub Copilot'
      result1 = filter_tester.cached_normalize(text)
      result2 = filter_tester.cached_normalize(text)
      expect(result1).to eq(result2)
      expect(result1.object_id).to eq(result2.object_id)
    end
  end

  describe '#build_word_index' do
    it 'builds word index from tag list' do
      tags = ['ai', 'machine learning', 'github copilot']
      result = filter_tester.build_word_index(tags)
      expect(result).to be_a(Hash)
    end

    it 'handles empty tag list' do
      result = filter_tester.build_word_index([])
      expect(result).to be_a(Hash)
      expect(result['tag_to_items']).to be_empty
      expect(result['word_to_tags']).to be_empty
    end


  end

  describe '#fast_subset_matching' do
    it 'performs fast subset matching' do
      word_index = { 'tag_to_items' => { 'ai' => ['item1', 'item2'] }, 'word_to_tags' => { 'ai' => ['ai', 'generative ai'] } }
      search_term = 'ai'
      result = filter_tester.fast_subset_matching(search_term, word_index)
      expect(result).to be_a(Array)
      expect(result).to eq(['item1', 'item2'])
    end

    it 'handles empty word index' do
      word_index = { 'tag_to_items' => {}, 'word_to_tags' => {} }
      result = filter_tester.fast_subset_matching('ai', word_index)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end


  end

  describe '#count_items_in_date_range' do
    let(:items) do
      [
        { 'date' => '2025-01-01' },
        { 'date' => '2024-12-25' },
        { 'date' => '2024-12-01' },
        { 'date' => '2024-01-01' }
      ]
    end

    it 'counts items within recent date range' do
      result = filter_tester.count_items_in_date_range(items, 30)
      expect(result).to be >= 0
      expect(result).to be_a(Integer)
    end

    it 'counts items within longer date range' do
      result = filter_tester.count_items_in_date_range(items, 365)
      expect(result).to be >= 0
      expect(result).to be_a(Integer)
    end

    it 'handles empty items array' do
      result = filter_tester.count_items_in_date_range([], 30)
      expect(result).to eq(0)
    end

    it 'handles nil items' do
      result = filter_tester.count_items_in_date_range(nil, 30)
      expect(result).to eq(0)
    end

    it 'handles invalid days parameter' do
      result = filter_tester.count_items_in_date_range(items, nil)
      expect(result).to eq(0)
    end
  end

  describe '#generate_tag_filter_data' do
    it 'generates JavaScript-ready data structure' do
      items = [
        { 'tags' => ['AI'], 'date' => '2025-01-01' }
      ]
      
      result = filter_tester.generate_tag_filter_data(items)
      
      expect(result).to be_a(Array)
    end

    it 'handles empty items array' do
      result = filter_tester.generate_tag_filter_data([])
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end

    it 'handles nil items' do
      result = filter_tester.generate_tag_filter_data(nil)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end
  end

  describe '#generate_tag_filter_buttons' do
    it 'generates tag buttons with display limits' do
      preprocessed_data = {
        'tag_counts' => { 'ai' => 5 },
        'original_tag_counts' => { 'ai' => 5 },
        'tag_mapping' => { 'ai' => 'AI' }
      }
      
      tag_display_limits = [5, 30]
      category = ['AI']
      collection = 'news'
      
      result = filter_tester.generate_tag_filter_buttons(preprocessed_data, tag_display_limits, category, collection)
      
      expect(result).to be_a(Array)
    end

    it 'handles empty preprocessed data' do
      preprocessed_data = {
        'tag_counts' => {},
        'original_tag_counts' => {},
        'tag_mapping' => {}
      }
      category = ['AI']
      collection = 'news'
      result = filter_tester.generate_tag_filter_buttons(preprocessed_data, [5, 30], category, collection)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end


  end
end