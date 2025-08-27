require_relative '../plugins/spec_helper'
require_relative '../../_plugins/date_filters'
require_relative '../../_plugins/date_utils'

RSpec.describe Jekyll::DateFilters do
  # Create a test class that includes the module
  let(:filter_tester) do
    Class.new do
      include Jekyll::DateFilters
    end.new
  end

  let(:sample_date) { '2025-07-20 12:00:00' }
  let(:sample_time) { Time.parse(sample_date) }
  let(:sample_items) do
    [
      double('item1', date: '2025-07-20'),
      double('item2', date: '2025-07-19'),
      double('item3', date: '2025-07-18'),
      double('item4', date: nil),
      'invalid_item'
    ]
  end

  before do
    # Mock DateUtils methods for consistent testing
    allow(DateUtils).to receive(:to_epoch).and_return(1721491200)
    allow(DateUtils).to receive(:date_to_epoch).and_return(1721491200)
    allow(DateUtils).to receive(:now_epoch).and_return(1721491200)
    allow(DateUtils).to receive(:normalize_date_format).and_return(sample_date)
    allow(DateUtils).to receive(:normalize_to_midnight).and_return(sample_time)
    allow(DateUtils).to receive(:has_valid_date?).and_return(true)
    allow(DateUtils).to receive(:compare_by_date).and_return(-1)
    allow(DateUtils).to receive(:extract_date).and_return(sample_date)
    allow(DateUtils).to receive(:to_date_string).and_return('2025-07-20')
  end

  describe '#to_epoch' do
    it 'delegates to DateUtils.to_epoch' do
      expect(DateUtils).to receive(:to_epoch).with(sample_date)
      filter_tester.to_epoch(sample_date)
    end

    it 'returns epoch timestamp' do
      result = filter_tester.to_epoch(sample_date)
      expect(result).to eq(1721491200)
    end

    it 'handles nil input' do
      allow(DateUtils).to receive(:to_epoch).with(nil).and_return(0)
      result = filter_tester.to_epoch(nil)
      expect(result).to eq(0)
    end

    it 'handles empty string input' do
      allow(DateUtils).to receive(:to_epoch).with('').and_return(0)
      result = filter_tester.to_epoch('')
      expect(result).to eq(0)
    end
  end

  describe '#date_to_epoch' do
    it 'delegates to DateUtils.date_to_epoch' do
      expect(DateUtils).to receive(:date_to_epoch).with(sample_date)
      filter_tester.date_to_epoch(sample_date)
    end

    it 'handles multiple date formats' do
      dates = [
        '2025-07-20',
        '2025-07-20 12:00:00',
        '2025-07-20T12:00:00',
        '2025-07-20 12:00:00 +0200'
      ]
      
      dates.each do |date|
        expect(DateUtils).to receive(:date_to_epoch).with(date)
        filter_tester.date_to_epoch(date)
      end
    end

    it 'normalizes different times on same day to same epoch' do
      allow(DateUtils).to receive(:date_to_epoch).and_return(1721491200)
      
      result1 = filter_tester.date_to_epoch('2025-07-20 08:00:00')
      result2 = filter_tester.date_to_epoch('2025-07-20 20:00:00')
      
      expect(result1).to eq(result2)
    end
  end

  describe '#now_epoch' do
    it 'delegates to DateUtils.now_epoch' do
      expect(DateUtils).to receive(:now_epoch).with(no_args)
      filter_tester.now_epoch(nil) # now_epoch in date_filters expects an input parameter
    end

    it 'delegates to DateUtils.now_epoch and ignores input' do
      expect(DateUtils).to receive(:now_epoch)
      filter_tester.now_epoch('any input')
    end

    it 'returns current Brussels midnight epoch' do
      result = filter_tester.now_epoch('')
      expect(result).to eq(1721491200)
    end

    it 'ignores input parameter completely' do
      result1 = filter_tester.now_epoch(nil)
      result2 = filter_tester.now_epoch('anything')
      result3 = filter_tester.now_epoch(123456)
      
      expect(result1).to eq(result2)
      expect(result2).to eq(result3)
    end

    it 'should be consistent for same day calls' do
      result1 = filter_tester.now_epoch('')
      result2 = filter_tester.now_epoch('')
      expect(result1).to eq(result2)
    end
  end

  describe '#normalize_date_format' do
    it 'delegates to DateUtils.normalize_date_format' do
      expect(DateUtils).to receive(:normalize_date_format).with('2025-01-01')
      filter_tester.normalize_date_format('2025-01-01')
    end

    it 'delegates to DateUtils.normalize_date_format' do
      expect(DateUtils).to receive(:normalize_date_format).with(sample_date)
      filter_tester.normalize_date_format(sample_date)
    end

    it 'handles timezone format fixes' do
      allow(DateUtils).to receive(:normalize_date_format).with('2025-07-20 +0000').and_return('2025-07-20 +00:00')
      result = filter_tester.normalize_date_format('2025-07-20 +0000')
      expect(result).to eq('2025-07-20 +00:00')
    end

    it 'handles timezone abbreviations' do
      allow(DateUtils).to receive(:normalize_date_format).with('2025-07-20 CEST').and_return('2025-07-20 +02:00')
      result = filter_tester.normalize_date_format('2025-07-20 CEST')
      expect(result).to eq('2025-07-20 +02:00')
    end
  end

  describe '#normalize_to_midnight' do
    it 'delegates to DateUtils.normalize_to_midnight' do
      expect(DateUtils).to receive(:normalize_to_midnight).with('2025-01-01')
      filter_tester.normalize_to_midnight('2025-01-01')
    end

    it 'delegates to DateUtils.normalize_to_midnight' do
      expect(DateUtils).to receive(:normalize_to_midnight).with(sample_date)
      filter_tester.normalize_to_midnight(sample_date)
    end

    it 'returns normalized time at midnight' do
      result = filter_tester.normalize_to_midnight(sample_date)
      expect(result).to eq(sample_time)
    end
  end

  describe '#with_dates' do
    let(:items) do
      [
        { 'date' => '2025-01-01' },
        { 'title' => 'No date' },
        { 'date' => '2025-01-02' }
      ]
    end

    it 'filters items with valid dates' do
      # Override the global mock to be more specific
      allow(DateUtils).to receive(:has_valid_date?) do |item|
        item.is_a?(Hash) && item.key?('date') && !item['date'].nil?
      end
      
      result = filter_tester.with_dates(items)
      expect(result.length).to eq(2)
      expect(result.map { |item| item['date'] }).to eq(['2025-01-01', '2025-01-02'])
    end

    it 'handles nil input' do
      expect(filter_tester.with_dates(nil)).to eq([])
    end

    it 'handles non-array input' do
      expect(filter_tester.with_dates('not an array')).to eq([])
    end
  end

  describe '#sort_by_date' do
    let(:items) do
      [
        { 'date' => '2025-01-01' },
        { 'date' => '2025-01-03' },
        { 'date' => '2025-01-02' }
      ]
    end

    it 'sorts by date descending by default' do
      # Override the global mock to do actual date comparison
      allow(DateUtils).to receive(:compare_by_date) do |item_a, item_b, direction|
        date_a = Date.parse(item_a['date'])
        date_b = Date.parse(item_b['date'])
        
        if direction.to_s.downcase == 'asc'
          date_a <=> date_b
        else
          date_b <=> date_a  # Default: newest first
        end
      end
      
      result = filter_tester.sort_by_date(items)
      dates = result.map { |item| item['date'] }
      expect(dates).to eq(['2025-01-03', '2025-01-02', '2025-01-01'])
    end

    it 'sorts by date ascending when specified' do
      # Override the global mock to do actual date comparison
      allow(DateUtils).to receive(:compare_by_date) do |item_a, item_b, direction|
        date_a = Date.parse(item_a['date'])
        date_b = Date.parse(item_b['date'])
        
        if direction.to_s.downcase == 'asc'
          date_a <=> date_b
        else
          date_b <=> date_a  # Default: newest first
        end
      end
      
      result = filter_tester.sort_by_date(items, 'asc')
      dates = result.map { |item| item['date'] }
      expect(dates).to eq(['2025-01-01', '2025-01-02', '2025-01-03'])
    end

    it 'handles nil input' do
      expect(filter_tester.sort_by_date(nil)).to eq([])
    end
  end

  describe '#limit_with_same_day' do
    let(:items) do
      [
        { 'date' => '2025-01-20' },
        { 'date' => '2025-01-19' },
        { 'date' => '2025-01-18' }
      ] + Array.new(25) { |i| { 'date' => '2025-01-10' } }
    end

    it 'limits to 20 items plus same-day items' do
      result = filter_tester.limit_with_same_day(items)
      expect(result.length).to be > 20 # Should include extra same-day items
    end

    it 'returns all items if 20 or fewer' do
      short_list = items.first(10)
      result = filter_tester.limit_with_same_day(short_list)
      expect(result.length).to eq(10)
    end

    it 'handles nil input' do
      expect(filter_tester.limit_with_same_day(nil)).to eq([])
    end

    it 'handles empty array' do
      expect(filter_tester.limit_with_same_day([])).to eq([])
    end

    it 'excludes items older than 7 days' do
      allow(DateUtils).to receive(:now_epoch).and_return(Time.parse('2025-01-15 00:00:00').to_i)
      
      recent_items = [
        { 'date' => '2025-01-15', 'collection' => 'news' }, # Today
        { 'date' => '2025-01-14', 'collection' => 'news' }, # 1 day ago
        { 'date' => '2025-01-09', 'collection' => 'news' }, # 6 days ago
        { 'date' => '2025-01-08', 'collection' => 'news' }, # 7 days ago (boundary)
      ]
      
      old_items = [
        { 'date' => '2025-01-07', 'collection' => 'news' }, # 8 days ago (should be excluded)
        { 'date' => '2025-01-01', 'collection' => 'news' }, # 14 days ago (should be excluded)
        { 'date' => '2024-12-01', 'collection' => 'news' }, # Very old (should be excluded)
      ]
      
      all_items = recent_items + old_items
      
      # Mock DateUtils methods that are used in the filtering logic
      allow(DateUtils).to receive(:has_valid_date?).and_return(true)
      allow(DateUtils).to receive(:extract_date) { |item| item['date'] }
      allow(DateUtils).to receive(:date_to_epoch) do |date|
        Time.parse("#{date} 00:00:00").to_i
      end
      allow(DateUtils).to receive(:compare_by_date) do |a, b, direction|
        a_date = Time.parse("#{a['date']} 00:00:00")
        b_date = Time.parse("#{b['date']} 00:00:00")
        direction == 'desc' ? b_date <=> a_date : a_date <=> b_date
      end
      
      result = filter_tester.limit_with_same_day(all_items)
      
      # Should only include items from the last 7 days
      expect(result.length).to eq(4) # All recent items
      result_dates = result.map { |item| item['date'] }.sort
      expect(result_dates).to eq(['2025-01-08', '2025-01-09', '2025-01-14', '2025-01-15'])
    end

    context 'with multiple collections' do
      let(:community_items) do
        # Use current date minus 1 day to ensure they're recent
        current_date = Date.today.strftime('%Y-%m-%d')
        Array.new(25) { |i| { 'date' => current_date, 'collection' => 'community' } }
      end
      
      let(:news_items) do
        # Use current date minus 2 days to ensure they're recent
        recent_date = (Date.today - 1).strftime('%Y-%m-%d')
        Array.new(15) { |i| { 'date' => recent_date, 'collection' => 'news' } }
      end
      
      let(:video_items) do
        # Use current date minus 3 days to ensure they're recent
        recent_date = (Date.today - 2).strftime('%Y-%m-%d')
        Array.new(5) { |i| { 'date' => recent_date, 'collection' => 'videos' } }
      end

      it 'applies 20+ same-day rule per collection, not globally' do
        # Override global mocks for this test to allow proper date handling
        allow(DateUtils).to receive(:extract_date) { |item| item['date'] }
        allow(DateUtils).to receive(:to_date_string) { |date_str| date_str }
        allow(DateUtils).to receive(:compare_by_date) do |item_a, item_b, direction|
          date_a = Date.parse(item_a['date'])
          date_b = Date.parse(item_b['date'])
          
          if direction.to_s.downcase == 'asc'
            date_a <=> date_b
          else
            date_b <=> date_a  # Default: newest first
          end
        end
        
        # Mock 7-day filtering to return current epoch time and properly handle recency
        current_epoch = Time.now.to_i
        
        allow(DateUtils).to receive(:now_epoch).and_return(current_epoch)
        allow(DateUtils).to receive(:date_to_epoch) do |date_str|
          # All our test dates are recent, so they should pass the 7-day filter
          Time.parse("#{date_str} 00:00:00").to_i
        end
        
        mixed_items = community_items + news_items + video_items
        result = filter_tester.limit_with_same_day(mixed_items)
        
        # Should get all 25 community items (since they're all same day)
        # Should get all 15 news items (less than 20)
        # Should get all 5 video items (less than 20)
        expect(result.length).to eq(45) # 25 + 15 + 5
        
        # Verify we have items from each collection
        result_collections = result.map { |item| item['collection'] }.uniq.sort
        expect(result_collections).to eq(['community', 'news', 'videos'])
      end

      it 'limits each collection to 20+ same-day rule independently' do
        # Override global mocks for this test to allow proper date handling
        allow(DateUtils).to receive(:extract_date) { |item| item['date'] }
        allow(DateUtils).to receive(:to_date_string) { |date_str| date_str }
        allow(DateUtils).to receive(:compare_by_date) do |item_a, item_b, direction|
          date_a = Date.parse(item_a['date'])
          date_b = Date.parse(item_b['date'])
          
          if direction.to_s.downcase == 'asc'
            date_a <=> date_b
          else
            date_b <=> date_a  # Default: newest first
          end
        end
        
        # Mock 7-day filtering to allow these test dates
        current_time = Time.parse("2025-12-31 00:00:00")
        allow(DateUtils).to receive(:now_epoch).and_return(current_time.to_i)
        allow(DateUtils).to receive(:date_to_epoch) do |date_str|
          Time.parse("#{date_str} 00:00:00").to_i
        end
        
        # Use recent dates for the test (within 7 days of our mocked current time)
        day3 = '2025-12-30'  # 1 day ago
        day2 = '2025-12-29'  # 2 days ago
        day1 = '2025-12-28'  # 3 days ago
        day4 = '2025-12-31'  # current day
        
        # Create scenario where community has items spanning multiple days
        # After sorting (newest first), we want exactly 20 items to end on a specific day
        community_day3 = Array.new(5) { |i| { 'date' => day3, 'collection' => 'community' } }  # 5 newest
        community_day2 = Array.new(10) { |i| { 'date' => day2, 'collection' => 'community' } } # 10 middle  
        community_day1 = Array.new(15) { |i| { 'date' => day1, 'collection' => 'community' } } # 15 oldest
        
        # News has 10 items, all same day
        news_items = Array.new(10) { |i| { 'date' => day4, 'collection' => 'news' } }
        
        mixed_items = community_day1 + community_day2 + community_day3 + news_items
        result = filter_tester.limit_with_same_day(mixed_items)
        
        # After sorting by date (newest first), community items are:
        # Items 1-5: from day3 (2025-12-30)
        # Items 6-15: from day2 (2025-12-29)
        # Items 16-20: first 5 from day1 (2025-12-28) - 20th item is from day1
        # Since 20th item is from day1, all remaining items from day1 should be included
        # So we get: 5 + 10 + 15 = 30 community items total
        # News should include all 10 items (less than 20)
        community_results = result.select { |item| item['collection'] == 'community' }
        news_results = result.select { |item| item['collection'] == 'news' }
        
        expect(community_results.length).to eq(30) # All included due to same-day rule
        expect(news_results.length).to eq(10) # All included (less than 20)
      end

      it 'properly limits when 20th item creates boundary' do
        # Override global mocks for this test to allow proper date handling
        allow(DateUtils).to receive(:extract_date) { |item| item['date'] }
        allow(DateUtils).to receive(:to_date_string) { |date_str| date_str }
        allow(DateUtils).to receive(:compare_by_date) do |item_a, item_b, direction|
          date_a = Date.parse(item_a['date'])
          date_b = Date.parse(item_b['date'])
          
          if direction.to_s.downcase == 'asc'
            date_a <=> date_b
          else
            date_b <=> date_a  # Default: newest first
          end
        end
        
        # Mock 7-day filtering to allow these test dates
        current_time = Time.parse("2025-12-31 00:00:00")
        allow(DateUtils).to receive(:now_epoch).and_return(current_time.to_i)
        allow(DateUtils).to receive(:date_to_epoch) do |date_str|
          Time.parse("#{date_str} 00:00:00").to_i
        end
        
        # Use recent dates for the test (within 7 days of our mocked current time)
        newest_day = '2025-12-30'     # 1 day ago
        boundary_day = '2025-12-29'   # 2 days ago
        older_day = '2025-12-28'      # 3 days ago
        
        # Create a scenario where the 20th item is clearly on a boundary day
        # and we can verify limitation works
        community_newer = Array.new(19) { |i| { 'date' => newest_day, 'collection' => 'community' } } # 19 newest
        community_boundary = Array.new(3) { |i| { 'date' => boundary_day, 'collection' => 'community' } } # 3 on boundary day  
        community_older = Array.new(10) { |i| { 'date' => older_day, 'collection' => 'community' } } # 10 older (should be excluded)
        
        mixed_items = community_newer + community_boundary + community_older
        result = filter_tester.limit_with_same_day(mixed_items)
        
        # After sorting: 19 from newest_day, then 3 from boundary_day, then 10 from older_day
        # Items 1-19: from newest_day
        # Item 20: first from boundary_day (20th item is from boundary_day)
        # Since 20th item is from boundary_day, include all from boundary_day (3 total)
        # Do NOT include any from older_day (different day from 20th item)
        # Total: 19 + 3 = 22 community items
        community_results = result.select { |item| item['collection'] == 'community' }
        
        expect(community_results.length).to eq(22) # 19 + 3, excludes the 10 older items
        
        # Verify that no items from older_day are included
        older_items = community_results.select { |item| item['date'] == older_day }
        expect(older_items.length).to eq(0)
      end
    end
  end

  # Additional advanced tests for edge cases and error handling
  describe 'Advanced Edge Cases' do
    describe '#to_epoch with invalid dates' do
      it 'handles malformed date strings' do
        invalid_dates = ['invalid-date', '2025-13-01', '2025-01-32', 'not-a-date']
        
        invalid_dates.each do |invalid_date|
          allow(DateUtils).to receive(:to_epoch).with(invalid_date).and_return(0)
          result = filter_tester.to_epoch(invalid_date)
          expect(result).to eq(0)
        end
      end

      it 'handles edge case years' do
        edge_dates = ['1970-01-01', '2038-01-19', '2099-12-31']
        
        edge_dates.each_with_index do |date, index|
          epoch_value = [0, 2147483647, 4102444800][index]
          allow(DateUtils).to receive(:to_epoch).with(date).and_return(epoch_value)
          result = filter_tester.to_epoch(date)
          expect(result).to eq(epoch_value)
        end
      end
    end

    describe '#limit_with_same_day advanced scenarios' do
      it 'handles items with same date at the 20th position' do
        # Create exactly 20 items with different dates, then multiple items with the same date
        different_dates = (1..19).map { |i| { 'date' => "2025-01-#{i.to_s.rjust(2, '0')}" } }
        same_day_items = Array.new(5) { { 'date' => '2025-01-20' } }
        items = different_dates + same_day_items
        
        result = filter_tester.limit_with_same_day(items)
        expect(result.length).to eq(24) # 19 + 5 same-day items
      end

      it 'handles items where 20+ items are all on the same day' do
        all_same_day = Array.new(30) { { 'date' => '2025-01-01' } }
        
        result = filter_tester.limit_with_same_day(all_same_day)
        expect(result.length).to eq(30) # Should include all same-day items
      end

      it 'handles mixed valid and invalid date items' do
        mixed_items = [
          { 'date' => '2025-01-01' },
          { 'title' => 'No date field' },
          { 'date' => nil },
          { 'date' => '' },
          { 'date' => '2025-01-02' }
        ]
        
        # Assuming the filter handles invalid dates gracefully
        result = filter_tester.limit_with_same_day(mixed_items)
        expect(result.length).to eq(5) # Should return all since < 20
      end
    end

    describe '#sort_by_date with edge cases' do
      it 'handles items with missing or nil dates' do
        items_with_nils = [
          { 'date' => '2025-01-01' },
          { 'date' => nil },
          { 'date' => '2025-01-02' },
          { 'title' => 'No date' }
        ]
        
        # The sort should handle nils gracefully
        expect { filter_tester.sort_by_date(items_with_nils) }.not_to raise_error
      end

      it 'handles duplicate dates' do
        duplicate_dates = [
          { 'date' => '2025-01-01' },
          { 'date' => '2025-01-01' },
          { 'date' => '2025-01-02' },
          { 'date' => '2025-01-01' }
        ]
        
        result = filter_tester.sort_by_date(duplicate_dates)
        expect(result.length).to eq(4)
      end
    end

    describe '#with_dates edge cases' do
      it 'handles deeply nested date structures' do
        # Override the global mock to check for direct 'date' field only
        allow(DateUtils).to receive(:has_valid_date?) do |item|
          item.is_a?(Hash) && item.key?('date') && !item['date'].nil? && !item['date'].to_s.empty?
        end
        
        nested_items = [
          { 'meta' => { 'date' => '2025-01-01' } },
          { 'date' => '2025-01-02' },
          { 'frontmatter' => { 'published' => '2025-01-03' } }
        ]
        
        # Should only find items with direct 'date' field
        result = filter_tester.with_dates(nested_items)
        expect(result.length).to eq(1)
        expect(result.first['date']).to eq('2025-01-02')
      end

      it 'handles various falsy date values' do
        # Override the global mock to check for truthy date values
        allow(DateUtils).to receive(:has_valid_date?) do |item|
          item.is_a?(Hash) && item.key?('date') && 
          !item['date'].nil? && 
          item['date'] != false &&
          item['date'] != 0 &&
          !item['date'].to_s.empty?
        end
        
        falsy_dates = [
          { 'date' => nil },
          { 'date' => '' },
          { 'date' => false },
          { 'date' => 0 },
          { 'date' => '2025-01-01' } # Only valid one
        ]
        
        result = filter_tester.with_dates(falsy_dates)
        expect(result.length).to eq(1)
        expect(result.first['date']).to eq('2025-01-01')
      end
    end

    describe 'Performance considerations' do
      it 'handles large datasets efficiently' do
        large_dataset = Array.new(1000) { |i| { 'date' => "2025-01-01" } }
        
        expect { filter_tester.limit_with_same_day(large_dataset) }.not_to raise_error
        expect { filter_tester.sort_by_date(large_dataset) }.not_to raise_error
        expect { filter_tester.with_dates(large_dataset) }.not_to raise_error
      end
    end
  end
end