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
  
  # Helper method to create mock items for testing
  def create_mock_item(date_string, tags = [])
    {
      'date' => date_string,
      'tags' => tags,
      'epoch' => DateUtils.date_to_epoch(date_string)
    }
  end

  # Helper method to prepare max_counts_by_days parameter for the production method
  def prepare_max_counts_by_days(items, date_filter_config)
    return {} if items.nil? || date_filter_config.nil?
    
    # Convert items to item_epochs
    item_epochs = items.map { |item| DateUtils.to_epoch(item['date']) }
    
    # Calculate max_counts_by_days from the items and date_filter_config
    counts_per_day_per_timezone = filter_tester.compute_counts_per_day_per_timezone(item_epochs, date_filter_config)
    filter_tester.get_max_counts_by_days(counts_per_day_per_timezone)
  end

  # Mock items with various dates for testing
  let(:test_items) do
    [
      # Today's posts (July 24, 2025 - current date)
      create_mock_item('2025-07-24 00:00:00 +00:00', ['AI', 'Today']),
      create_mock_item('2025-07-24 12:00:00 +00:00', ['Tech', 'Today']),
      
      # Yesterday's posts (July 23, 2025)
      create_mock_item('2025-07-23 00:00:00 +00:00', ['AI', 'Yesterday']),
      create_mock_item('2025-07-23 18:00:00 +00:00', ['Tech', 'Yesterday']),
      
      # 3 days ago (July 21, 2025)
      create_mock_item('2025-07-21 00:00:00 +00:00', ['AI', 'ThreeDaysAgo']),
      
      # 7 days ago (July 17, 2025)
      create_mock_item('2025-07-17 00:00:00 +00:00', ['AI', 'WeekAgo']),
      
      # 10 days ago (July 14, 2025)
      create_mock_item('2025-07-14 00:00:00 +00:00', ['AI', 'OldPost']),
      
      # 30 days ago (June 24, 2025)
      create_mock_item('2025-06-24 00:00:00 +00:00', ['AI', 'MonthAgo'])
    ]
  end

  describe '#count_items_in_date_range' do
    it 'counts items within recent date range' do
      result = filter_tester.count_items_in_date_range(test_items, 30)
      expect(result).to be >= 0
      expect(result).to be_a(Integer)
    end

    it 'counts items within longer date range' do
      result = filter_tester.count_items_in_date_range(test_items, 365)
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
      result = filter_tester.count_items_in_date_range(test_items, nil)
      expect(result).to eq(0)
    end
  end

  describe '#generate_date_filter_buttons' do
    let(:date_filters) do
      ['Today', 'Last 7 days', 'Last 30 days']
    end

    let(:date_filter_config) do
      ['Today:0', 'Last 7 days:7', 'Last 30 days:30']
    end

    it 'returns array of button data with valid inputs' do
      max_counts_by_days = prepare_max_counts_by_days(test_items, date_filter_config)
      result = filter_tester.generate_date_filter_buttons(date_filters, date_filter_config, max_counts_by_days)
      expect(result).to be_a(Array)
    end

    it 'handles empty items array' do
      max_counts_by_days = prepare_max_counts_by_days([], date_filter_config)
      result = filter_tester.generate_date_filter_buttons(date_filters, date_filter_config, max_counts_by_days)
      expect(result).to be_a(Array)
    end

    it 'handles nil items' do
      max_counts_by_days = prepare_max_counts_by_days(nil, date_filter_config)
      result = filter_tester.generate_date_filter_buttons(date_filters, date_filter_config, max_counts_by_days)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end

    it 'handles empty date_filters array' do
      max_counts_by_days = prepare_max_counts_by_days(test_items, date_filter_config)
      result = filter_tester.generate_date_filter_buttons([], date_filter_config, max_counts_by_days)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end

    it 'handles nil date_filters' do
      max_counts_by_days = prepare_max_counts_by_days(test_items, date_filter_config)
      result = filter_tester.generate_date_filter_buttons(nil, date_filter_config, max_counts_by_days)
      expect(result).to be_a(Array)
      expect(result).to be_empty
    end

    it 'handles nil date_filter_config' do
      max_counts_by_days = prepare_max_counts_by_days(test_items, nil)
      result = filter_tester.generate_date_filter_buttons(date_filters, nil, max_counts_by_days)
      expect(result).to be_a(Array)
    end
  end
end
