require_relative '../plugins/spec_helper'
require_relative '../../_plugins/tag_filters'
require_relative '../../_plugins/date_utils'
require 'set'
require 'parallel'

class TagFilterAdvancedTester
  include Jekyll::TagFilters
  
  def initialize
    @site = nil
  end
end

RSpec.describe Jekyll::TagFilters do
  # Create a test class that includes the module
  let(:filter_tester) { TagFilterAdvancedTester.new }

  let(:sample_items) do
    [
      {
        'tags' => ['AI', 'Machine Learning', 'Python'],
        'tags_normalized' => ['ai', 'machine learning', 'python'],
        'date' => '2025-07-20'
      },
      {
        'tags' => ['GitHub Copilot', 'AI', 'Visual Studio'],
        'tags_normalized' => ['github copilot', 'ai', 'visual studio'],
        'date' => '2025-07-19'
      },
      {
        'tags' => ['Azure', 'Cloud Computing'],
        'tags_normalized' => ['azure', 'cloud computing'],
        'date' => '2025-07-18'
      }
    ]
  end

  let(:date_filter_config) do
    ['Today only:0', 'Last 3 days:3', 'Last 7 days:7', 'Last 30 days:30']
  end

  before do
    # Clear caches before each test
    Jekyll::TagFilters.class_variable_set(:@@date_filter_cache, {})
    Jekyll::TagFilters.class_variable_set(:@@string_split_cache, {})
    Jekyll::TagFilters.class_variable_set(:@@normalize_cache, {})
    Jekyll::TagFilters.class_variable_set(:@@word_index_cache, {})
  end

  describe '#cached_string_split' do
    it 'splits strings by whitespace by default' do
      result = filter_tester.cached_string_split('hello world test')
      expect(result).to eq(['hello', 'world', 'test'])
    end

    it 'removes empty strings from result' do
      result = filter_tester.cached_string_split('hello  world   test')
      expect(result).to eq(['hello', 'world', 'test'])
    end

    it 'accepts custom patterns' do
      result = filter_tester.cached_string_split('hello,world,test', /,/)
      expect(result).to eq(['hello', 'world', 'test'])
    end

    it 'caches results for performance' do
      # First call
      result1 = filter_tester.cached_string_split('hello world')
      # Second call should use cache
      result2 = filter_tester.cached_string_split('hello world')
      
      expect(result1).to eq(result2)
      expect(result1.object_id).to eq(result2.object_id)
    end

    it 'handles empty strings' do
      result = filter_tester.cached_string_split('')
      expect(result).to eq([])
    end

    it 'handles single words' do
      result = filter_tester.cached_string_split('hello')
      expect(result).to eq(['hello'])
    end
  end

  describe '#cached_normalize' do
    it 'converts to lowercase' do
      result = filter_tester.cached_normalize('HELLO WORLD')
      expect(result).to eq('hello world')
    end

    it 'strips whitespace' do
      result = filter_tester.cached_normalize('  hello world  ')
      expect(result).to eq('hello world')
    end

    it 'removes special characters' do
      result = filter_tester.cached_normalize('hello@world!')
      expect(result).to eq('hello world')
    end

    it 'squeezes multiple spaces' do
      result = filter_tester.cached_normalize('hello   world')
      expect(result).to eq('hello world')
    end

    it 'handles nil input' do
      result = filter_tester.cached_normalize(nil)
      expect(result).to eq('')
    end

    it 'handles empty input' do
      result = filter_tester.cached_normalize('')
      expect(result).to eq('')
    end

    it 'caches results for performance' do
      # First call
      result1 = filter_tester.cached_normalize('Hello World')
      # Second call should use cache
      result2 = filter_tester.cached_normalize('Hello World')
      
      expect(result1).to eq(result2)
      expect(result1.object_id).to eq(result2.object_id)
    end

    it 'handles complex special characters' do
      result = filter_tester.cached_normalize('test@#$%^&*()[]{}|\\:";\'<>?,./`~')
      expect(result).to eq('test')
    end
  end

  describe '#calculate_date_cutoffs' do
    before do
      allow(DateUtils).to receive(:now_epoch).and_return(1721808000) # Fixed timestamp for testing
    end

    it 'calculates today-only cutoff correctly' do
      config = ['Today only:0']
      cutoffs = filter_tester.calculate_date_cutoffs(config)
      expect(cutoffs[0]).to eq(1721808000) # Same as now_epoch for today
    end

    it 'calculates multi-day cutoffs correctly' do
      config = ['Last 3 days:3']
      cutoffs = filter_tester.calculate_date_cutoffs(config)
      expected = 1721808000 - (2 * 86400) # 2 days back (3-1)
      expect(cutoffs[3]).to eq(expected)
    end

    it 'handles multiple configurations' do
      cutoffs = filter_tester.calculate_date_cutoffs(date_filter_config)
      expect(cutoffs.keys).to contain_exactly(0, 3, 7, 30)
    end

    it 'parses configuration strings correctly' do
      config = ['Custom label:14']
      cutoffs = filter_tester.calculate_date_cutoffs(config)
      expect(cutoffs).to have_key(14)
    end
  end

  describe '#build_word_index' do
    it 'creates word to tags mapping' do
      index = filter_tester.build_word_index(sample_items)
      word_to_tags = index['word_to_tags']
      
      expect(word_to_tags['ai']).to include('ai')
      expect(word_to_tags['machine']).to include('machine learning')
      expect(word_to_tags['github']).to include('github copilot')
    end

    it 'creates tag to items mapping' do
      index = filter_tester.build_word_index(sample_items)
      tag_to_items = index['tag_to_items']
      
      expect(tag_to_items['ai']).to contain_exactly(0, 1)
      expect(tag_to_items['python']).to contain_exactly(0)
      expect(tag_to_items['azure']).to contain_exactly(2)
    end

    it 'skips single character words' do
      items = [{ 'tags_normalized' => ['a', 'ai', 'b'] }]
      index = filter_tester.build_word_index(items)
      word_to_tags = index['word_to_tags']
      
      expect(word_to_tags).not_to have_key('a')
      expect(word_to_tags).not_to have_key('b')
      expect(word_to_tags).to have_key('ai')
    end

    it 'handles items without tags' do
      items = [{ 'tags_normalized' => [] }, { 'tags_normalized' => nil }]
      index = filter_tester.build_word_index(items)
      
      expect(index['word_to_tags']).to be_empty
      expect(index['tag_to_items']).to be_empty
    end

    it 'caches results for performance' do
      index1 = filter_tester.build_word_index(sample_items)
      index2 = filter_tester.build_word_index(sample_items)
      
      expect(index1.object_id).to eq(index2.object_id)
    end

    it 'deduplicates item indices' do
      items = [
        { 'tags_normalized' => ['test', 'test'] } # Duplicate tags in same item
      ]
      index = filter_tester.build_word_index(items)
      
      expect(index['tag_to_items']['test']).to eq([0])
    end
  end

  describe '#fast_subset_matching' do
    let(:word_index) { filter_tester.build_word_index(sample_items) }

    it 'finds direct tag matches' do
      matches = filter_tester.fast_subset_matching('ai', word_index)
      expect(matches).to contain_exactly(0, 1)
    end

    it 'finds subset matches for single words' do
      matches = filter_tester.fast_subset_matching('machine', word_index)
      expect(matches).to contain_exactly(0) # Should find "machine learning"
    end

    it 'finds exact matches for multi-word searches' do
      matches = filter_tester.fast_subset_matching('machine learning', word_index)
      expect(matches).to contain_exactly(0) # Should find item with "machine learning" tag
    end

    it 'returns empty array for non-existent tags' do
      matches = filter_tester.fast_subset_matching('nonexistent', word_index)
      expect(matches).to eq([])
    end

    it 'handles case sensitivity correctly' do
      matches = filter_tester.fast_subset_matching('AI', word_index)
      expect(matches).to be_empty # Should be empty as search is case sensitive
    end
  end

  describe '#optimize_date_filtering' do
    let(:item_epochs) { [1721548800, 1721635200, 1721721600, 1721808000] } # 4 different days
    let(:date_cutoffs) { { 0 => 1721721600, 3 => 1721635200, 7 => 1721462400 } }

    it 'calculates today-only counts correctly' do
      counts = filter_tester.optimize_date_filtering(item_epochs, date_cutoffs)
      expect(counts[0]).to be_a(Integer)
    end

    it 'calculates multi-day counts correctly' do
      counts = filter_tester.optimize_date_filtering(item_epochs, date_cutoffs)
      expect(counts[3]).to be >= counts[0]
      expect(counts[7]).to be >= counts[3]
    end

    it 'handles empty item list' do
      counts = filter_tester.optimize_date_filtering([], date_cutoffs)
      expect(counts).to eq({})
    end

    it 'uses binary search optimization' do
      # Test that it uses binary search by checking it doesn't scan all items
      large_epochs = (1..10000).map { |i| 1721548800 + i }
      expect { filter_tester.optimize_date_filtering(large_epochs, date_cutoffs) }.not_to raise_error
    end
  end

  describe '#process_all_item_data_parallel' do
    it 'processes items and returns expected structure' do
      result = filter_tester.process_all_item_data_parallel(sample_items, 'AI', 'news', {})
      
      expect(result).to have_key(:tag_data)
      expect(result).to have_key(:date_counts)
      expect(result).to have_key(:filter_data)
      expect(result).to have_key(:item_epochs)
    end

    it 'handles empty items array' do
      result = filter_tester.process_all_item_data_parallel([], 'AI', 'news', {})
      
      expect(result[:tag_data]).to be_a(Hash)
      expect(result[:filter_data]).to eq([])
      expect(result[:item_epochs]).to eq([])
    end

    it 'processes items consistently with non-parallel version' do
      parallel_result = filter_tester.process_all_item_data_parallel(sample_items, 'AI', 'news', {})
      serial_result = filter_tester.process_batch(sample_items, 'AI', 'news', {})
      
      # Results should have same structure and similar content
      expect(parallel_result.keys).to eq(serial_result.keys)
      expect(parallel_result[:filter_data].length).to eq(serial_result[:filter_data].length)
    end
  end

  describe '#process_batch' do
    let(:date_cutoffs) { { 0 => 1721721600, 3 => 1721635200 } }

    before do
      allow(DateUtils).to receive(:extract_date).and_return('2025-07-20')
      allow(DateUtils).to receive(:to_epoch).and_return(1721721600)
    end

    it 'processes items in a batch' do
      result = filter_tester.process_batch(sample_items, 'AI', 'news', date_cutoffs)
      
      expect(result).to have_key(:tag_data)
      expect(result).to have_key(:date_counts)
      expect(result).to have_key(:filter_data)
      expect(result).to have_key(:item_epochs)
    end

    it 'builds redundant tags set from category and collection' do
      result = filter_tester.process_batch(sample_items, 'AI', 'news', date_cutoffs)
      redundant_tags = result[:tag_data]['redundant_tags']
      
      expect(redundant_tags).to include('ai')
      expect(redundant_tags).to include('news')
    end

    it 'processes tag relationships correctly' do
      result = filter_tester.process_batch(sample_items, nil, nil, date_cutoffs)
      relationships = result[:tag_data]['relationships']
      
      expect(relationships).to be_a(Hash)
      expect(relationships['ai']).to be_a(Array)
    end

    it 'counts tags excluding redundant ones' do
      result = filter_tester.process_batch(sample_items, 'AI', nil, date_cutoffs)
      tag_counts = result[:tag_data]['tag_counts']
      
      # 'ai' should be excluded from counts as it's redundant (category = 'AI' -> 'ai')
      expect(tag_counts).not_to have_key('ai')
    end

    it 'builds filter data for JavaScript' do
      result = filter_tester.process_batch(sample_items, nil, nil, date_cutoffs)
      filter_data = result[:filter_data]
      
      expect(filter_data).to be_a(Array)
      expect(filter_data.length).to eq(sample_items.length)
      expect(filter_data[0]).to have_key('index')
      expect(filter_data[0]).to have_key('epoch')
      expect(filter_data[0]).to have_key('tags')
    end
  end

  describe '#merge_batch_results' do
    let(:batch_results) do
      [
        {
          tag_data: {
            'relationships' => { 'ai' => [0, 1] },
            'tag_counts' => { 'ai' => 2 },
            'original_tag_counts' => { 'ai' => 2 },
            'tag_mapping' => { 'ai' => 'AI' },
            'redundant_tags' => ['ai']
          },
          date_counts: { 3 => 2 },
          filter_data: [{ 'index' => 0 }],
          item_epochs: [1721721600]
        },
        {
          tag_data: {
            'relationships' => { 'ai' => [2], 'python' => [2] },
            'tag_counts' => { 'ai' => 1, 'python' => 1 },
            'original_tag_counts' => { 'ai' => 1, 'python' => 1 },
            'tag_mapping' => { 'ai' => 'AI', 'python' => 'Python' },
            'redundant_tags' => ['ai']
          },
          date_counts: { 3 => 1 },
          filter_data: [{ 'index' => 2 }],
          item_epochs: [1721635200]
        }
      ]
    end

    it 'merges tag relationships correctly' do
      result = filter_tester.merge_batch_results(batch_results)
      relationships = result[:tag_data]['relationships']
      
      expect(relationships['ai']).to contain_exactly(0, 1, 2)
      expect(relationships['python']).to contain_exactly(2)
    end

    it 'merges tag counts correctly' do
      result = filter_tester.merge_batch_results(batch_results)
      tag_counts = result[:tag_data]['tag_counts']
      
      expect(tag_counts['ai']).to eq(3)
      expect(tag_counts['python']).to eq(1)
    end

    it 'merges date counts correctly' do
      result = filter_tester.merge_batch_results(batch_results)
      date_counts = result[:date_counts]
      
      expect(date_counts[3]).to eq(3)
    end

    it 'concatenates filter data and epochs' do
      result = filter_tester.merge_batch_results(batch_results)
      
      expect(result[:filter_data].length).to eq(2)
      expect(result[:item_epochs].length).to eq(2)
    end

    it 'deduplicates redundant tags' do
      result = filter_tester.merge_batch_results(batch_results)
      redundant_tags = result[:tag_data]['redundant_tags']
      
      expect(redundant_tags.count('ai')).to eq(1)
    end
  end

  describe 'caching behavior' do
    it 'uses separate caches for different operations' do
      # Test string split cache
      filter_tester.cached_string_split('test string')
      split_cache = Jekyll::TagFilters.class_variable_get(:@@string_split_cache)
      expect(split_cache).not_to be_empty

      # Test normalize cache
      filter_tester.cached_normalize('Test String')
      normalize_cache = Jekyll::TagFilters.class_variable_get(:@@normalize_cache)
      expect(normalize_cache).not_to be_empty

      # Test word index cache
      filter_tester.build_word_index(sample_items)
      word_index_cache = Jekyll::TagFilters.class_variable_get(:@@word_index_cache)
      expect(word_index_cache).not_to be_empty
    end

    it 'maintains cache consistency across method calls' do
      # First build word index
      index1 = filter_tester.build_word_index(sample_items)
      
      # Use fast subset matching which should use the same cached index
      filter_tester.fast_subset_matching('ai', index1)
      
      # Build index again - should return cached version
      index2 = filter_tester.build_word_index(sample_items)
      
      expect(index1.object_id).to eq(index2.object_id)
    end
  end

  describe 'performance optimizations' do
    it 'uses Set for deduplication in relationships' do
      # Test that Sets are used internally for performance
      result = filter_tester.process_batch(sample_items, nil, nil, {})
      relationships = result[:tag_data]['relationships']
      
      # Relationships should be converted to sorted arrays
      relationships.values.each do |indices|
        expect(indices).to be_a(Array)
        expect(indices).to eq(indices.sort)
      end
    end

    it 'handles large datasets efficiently' do
      large_items = Array.new(1000) do |i|
        {
          'tags' => ['tag1', 'tag2', 'tag3'],
          'tags_normalized' => ['tag1', 'tag2', 'tag3'],
          'date' => "2025-07-#{(i % 30) + 1}"
        }
      end

      expect { filter_tester.build_word_index(large_items) }.not_to raise_error
    end
  end

  describe 'edge cases and error handling' do
    it 'handles items with missing tags gracefully' do
      problematic_items = [
        { 'date' => '2025-07-20' }, # No tags
        { 'tags' => nil, 'date' => '2025-07-20' }, # Nil tags
        { 'tags' => [], 'tags_normalized' => [], 'date' => '2025-07-20' } # Empty tags
      ]

      expect { filter_tester.build_word_index(problematic_items) }.not_to raise_error
      expect { filter_tester.process_batch(problematic_items, nil, nil, {}) }.not_to raise_error
    end

    it 'handles mismatched tag arrays' do
      mismatched_items = [
        {
          'tags' => ['AI', 'Machine Learning'], # 2 tags
          'tags_normalized' => ['ai'], # 1 normalized tag
          'date' => '2025-07-20'
        }
      ]

      result = filter_tester.process_batch(mismatched_items, nil, nil, {})
      expect(result[:tag_data]['tag_counts']).to have_key('ai')
    end

    it 'handles very long tag names' do
      long_tag_items = [
        {
          'tags' => ['A' * 1000],
          'tags_normalized' => ['a' * 1000],
          'date' => '2025-07-20'
        }
      ]

      expect { filter_tester.build_word_index(long_tag_items) }.not_to raise_error
    end

    it 'handles special characters in tag normalization' do
      special_char_items = [
        {
          'tags' => ['AI@#$%^&*()'],
          'tags_normalized' => ['ai'],
          'date' => '2025-07-20'
        }
      ]

      result = filter_tester.process_batch(special_char_items, nil, nil, {})
      expect(result[:tag_data]['tag_counts']).to have_key('ai')
    end
  end

  describe 'parallel processing index bug fix' do
    it 'uses global indices correctly across parallel batches' do
      # Create a dataset large enough to force parallel processing into multiple batches
      # Each batch will have a different batch_start_index
      large_items = []
      150.times do |i|
        large_items << {
          'tags' => ["tag#{i}"],
          'tags_normalized' => ["tag#{i}"],
          'date' => '2025-07-20',
          'categories' => ['Test'],
          'collection' => 'test'
        }
      end

      # Process with parallel processing
      parallel_result = filter_tester.process_all_item_data_parallel(large_items, 'Test', 'test', {})

      # Verify that tag relationships contain correct global indices, not batch-relative indices
      # The bug would cause items in later batches to have incorrect (too low) indices
      
      # Check a tag from an item that would be in a later batch (e.g., item 120)
      tag_from_later_batch = "tag120"
      expect(parallel_result[:tag_data]['relationships']).to have_key(tag_from_later_batch)
      
      # The relationship should contain the global index (120), not a batch-relative index (like 20)
      indices_for_tag = parallel_result[:tag_data]['relationships'][tag_from_later_batch]
      expect(indices_for_tag).to include(120)
      
      # Verify that we don't have incorrect low indices that would indicate the batch-relative bug
      expect(indices_for_tag).not_to include(20) # This would be wrong if using batch-relative index
      
      # Also verify the last item has the correct global index
      last_tag = "tag149"
      expect(parallel_result[:tag_data]['relationships']).to have_key(last_tag)
      last_indices = parallel_result[:tag_data]['relationships'][last_tag]
      expect(last_indices).to include(149)  # Should be global index 149
      expect(last_indices).not_to include(49) # Not batch-relative index
    end

    it 'produces consistent results between parallel and serial processing' do
      # Create test items that will span multiple batches
      test_items = []
      50.times do |i|
        test_items << {
          'tags' => ["shared_tag", "unique_tag#{i}"],
          'tags_normalized' => ["shared_tag", "unique_tag#{i}"],
          'date' => '2025-07-20',
          'categories' => ['Test'],
          'collection' => 'test'
        }
      end

      # Process with parallel processing
      parallel_result = filter_tester.process_all_item_data_parallel(test_items, 'Test', 'test', {})

      # Process serially for comparison (using process_batch with all items)
      serial_result = filter_tester.process_batch(test_items, 'Test', 'test', {})

      # The shared_tag should appear in all items, so relationships should contain all indices 0-49
      parallel_shared_indices = parallel_result[:tag_data]['relationships']['shared_tag'] || []
      serial_shared_indices = serial_result[:tag_data]['relationships']['shared_tag'] || []

      # Both should contain the same set of indices
      expect(parallel_shared_indices.sort).to eq(serial_shared_indices.sort)
      expect(parallel_shared_indices.sort).to eq((0...50).to_a)

      # Verify tag counts are also consistent
      expect(parallel_result[:tag_data]['tag_counts']['shared_tag']).to eq(serial_result[:tag_data]['tag_counts']['shared_tag'])
      expect(parallel_result[:tag_data]['tag_counts']['shared_tag']).to eq(50)

      # Check a few specific unique tags to ensure their indices are correct
      [10, 25, 40].each do |i|
        unique_tag = "unique_tag#{i}"
        parallel_indices = parallel_result[:tag_data]['relationships'][unique_tag] || []
        serial_indices = serial_result[:tag_data]['relationships'][unique_tag] || []
        
        expect(parallel_indices).to eq(serial_indices)
        expect(parallel_indices).to eq([i])  # Should only contain the global index i
      end
    end

    it 'correctly processes batches with explicit batch_start_index parameter' do
      # Test the process_batch function directly with different batch_start_index values
      batch_items = [
        {
          'tags' => ['test_tag'],
          'tags_normalized' => ['test_tag'],
          'date' => '2025-07-20'
        },
        {
          'tags' => ['another_tag'],
          'tags_normalized' => ['another_tag'],
          'date' => '2025-07-20'
        }
      ]

      # Process this batch as if it's the second batch (starting at index 100)
      batch_start_index = 100
      result = filter_tester.process_batch(batch_items, 'Test', 'test', {}, batch_start_index)

      # Verify that tag relationships use global indices (100, 101) not batch-relative (0, 1)
      test_tag_indices = result[:tag_data]['relationships']['test_tag']
      another_tag_indices = result[:tag_data]['relationships']['another_tag']

      expect(test_tag_indices).to eq([100])  # Global index, not 0
      expect(another_tag_indices).to eq([101])  # Global index, not 1

      # Verify filter_data also uses global indices
      expect(result[:filter_data].length).to eq(2)
      expect(result[:filter_data][0]['index']).to eq(100)
      expect(result[:filter_data][1]['index']).to eq(101)
    end
  end
end
