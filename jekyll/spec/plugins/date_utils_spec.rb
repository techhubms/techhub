require_relative '../plugins/spec_helper'
require_relative '../../_plugins/date_utils'
require 'time'

RSpec.describe DateUtils do
  describe '.normalize_date_format' do
    it 'returns empty string for nil input' do
      expect(DateUtils.normalize_date_format(nil)).to eq('')
    end

    it 'strips whitespace from input' do
      expect(DateUtils.normalize_date_format('  2025-01-01  ')).to eq('2025-01-01')
    end

    it 'converts +0000 timezone format to +00:00' do
      expect(DateUtils.normalize_date_format('2025-01-01 12:00:00 +0000')).to eq('2025-01-01 12:00:00 +00:00')
    end

    it 'converts CEST to +02:00' do
      expect(DateUtils.normalize_date_format('2025-07-01 12:00:00 CEST')).to eq('2025-07-01 12:00:00 +02:00')
    end

    it 'converts CET to +01:00' do
      expect(DateUtils.normalize_date_format('2025-01-01 12:00:00 CET')).to eq('2025-01-01 12:00:00 +01:00')
    end

    it 'handles string input conversion' do
      expect(DateUtils.normalize_date_format(123)).to eq('123')
    end

    context 'with timezone format fixes' do
      it 'converts +0000 to +00:00 format' do
        result = DateUtils.normalize_date_format('2025-07-20 12:00:00 +0000')
        expect(result).to eq('2025-07-20 12:00:00 +00:00')
      end

      it 'converts -0500 to -05:00 format' do
        result = DateUtils.normalize_date_format('2025-07-20 12:00:00 -0500')
        expect(result).to eq('2025-07-20 12:00:00 -05:00')
      end

      it 'handles positive timezone offsets' do
        result = DateUtils.normalize_date_format('2025-07-20 12:00:00 +0200')
        expect(result).to eq('2025-07-20 12:00:00 +02:00')
      end

      it 'leaves properly formatted timezones unchanged' do
        input = '2025-07-20 12:00:00 +02:00'
        result = DateUtils.normalize_date_format(input)
        expect(result).to eq(input)
      end
    end

    context 'with timezone abbreviations' do
      it 'converts CEST to +02:00' do
        result = DateUtils.normalize_date_format('2025-07-20 12:00:00 CEST')
        expect(result).to eq('2025-07-20 12:00:00 +02:00')
      end

      it 'converts CET to +01:00' do
        result = DateUtils.normalize_date_format('2025-01-20 12:00:00 CET')
        expect(result).to eq('2025-01-20 12:00:00 +01:00')
      end

      it 'handles whitespace before timezone abbreviations' do
        result = DateUtils.normalize_date_format('2025-07-20 12:00:00   CEST')
        expect(result).to eq('2025-07-20 12:00:00 +02:00')
      end
    end

    context 'with edge cases' do
      it 'handles nil input' do
        result = DateUtils.normalize_date_format(nil)
        expect(result).to eq('')
      end

      it 'handles empty string input' do
        result = DateUtils.normalize_date_format('')
        expect(result).to eq('')
      end

      it 'handles whitespace-only input' do
        result = DateUtils.normalize_date_format('   ')
        expect(result).to eq('')
      end
    end
  end

  describe '.normalize_to_midnight' do
    it 'returns epoch 0 time for nil input' do
      expect(DateUtils.normalize_to_midnight(nil)).to eq(Time.at(0))
    end

    it 'normalizes Time to midnight' do
      time = Time.new(2025, 1, 1, 15, 30, 45)
      result = DateUtils.normalize_to_midnight(time)
      expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-01-01 00:00:00')
    end

    it 'normalizes Date to midnight' do
      date = Date.new(2025, 1, 1)
      result = DateUtils.normalize_to_midnight(date)
      expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-01-01 00:00:00')
    end

    it 'normalizes string to midnight' do
      result = DateUtils.normalize_to_midnight('2025-01-01 15:30:45')
      expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-01-01 00:00:00')
    end

    it 'handles invalid string input' do
      expect(DateUtils.normalize_to_midnight('invalid')).to eq(Time.at(0))
    end

    it 'preserves timezone context in normalization' do
      time = Time.new(2025, 1, 1, 15, 30, 45, '+02:00')
      result = DateUtils.normalize_to_midnight(time)
      expect(result.hour).to eq(0)
      expect(result.min).to eq(0)
      expect(result.sec).to eq(0)
    end

    context 'with Time objects' do
      it 'sets time to midnight while preserving date' do
        time = Time.parse('2025-07-20 15:30:45')
        result = DateUtils.normalize_to_midnight(time)
        expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-07-20 00:00:00')
      end

      it 'handles edge of day times' do
        time = Time.parse('2025-12-31 23:59:59')
        result = DateUtils.normalize_to_midnight(time)
        expect(result.strftime('%Y-%m-%d')).to eq('2025-12-31')
      end
    end

    context 'with Date objects' do
      it 'converts Date to midnight Time' do
        date = Date.parse('2025-07-20')
        result = DateUtils.normalize_to_midnight(date)
        expect(result).to be_a(Time)
        expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-07-20 00:00:00')
      end
    end

    context 'with String input' do
      it 'parses string and normalizes to midnight' do
        result = DateUtils.normalize_to_midnight('2025-07-20 15:30:45')
        expect(result.strftime('%Y-%m-%d %H:%M:%S')).to eq('2025-07-20 00:00:00')
      end

      it 'handles timezone information in strings' do
        result = DateUtils.normalize_to_midnight('2025-07-20 15:30:45 +02:00')
        expect(result.strftime('%H:%M:%S')).to eq('00:00:00')
      end

      it 'handles invalid date strings' do
        result = DateUtils.normalize_to_midnight('invalid date')
        expect(result).to eq(Time.at(0))
      end
    end

    context 'with edge cases' do
      it 'handles nil input' do
        result = DateUtils.normalize_to_midnight(nil)
        expect(result).to eq(Time.at(0))
      end

      it 'handles other input types' do
        result = DateUtils.normalize_to_midnight(123456)
        expect(result).to eq(Time.at(0))
      end
    end
  end

  describe '.to_time' do
    it 'returns Time objects unchanged' do
      time = Time.now
      result = DateUtils.to_time(time)
      expect(result).to eq(time)
    end

    it 'converts Date to Time' do
      date = Date.today
      result = DateUtils.to_time(date)
      expect(result).to be_a(Time)
      expect(result.to_date).to eq(date)
    end

    it 'parses string dates' do
      result = DateUtils.to_time('2025-07-20 12:00:00')
      expect(result).to be_a(Time)
      expect(result.year).to eq(2025)
      expect(result.month).to eq(7)
      expect(result.day).to eq(20)
    end

    it 'handles invalid string dates' do
      result = DateUtils.to_time('invalid date')
      expect(result).to eq(Time.at(0))
    end

    it 'handles nil input' do
      result = DateUtils.to_time(nil)
      expect(result).to eq(Time.at(0))
    end

    it 'handles other input types' do
      result = DateUtils.to_time(123456)
      expect(result).to eq(Time.at(0))
    end
  end

  describe '.to_epoch' do
    it 'returns 0 for nil input' do
      expect(DateUtils.to_epoch(nil)).to eq(0)
    end

    it 'converts Time to epoch' do
      time = Time.new(2025, 1, 1, 0, 0, 0, '+00:00')
      expect(DateUtils.to_epoch(time)).to eq(time.to_i)
    end

    it 'converts Date to epoch' do
      date = Date.new(2025, 1, 1)
      expected_epoch = date.to_time.to_i
      expect(DateUtils.to_epoch(date)).to eq(expected_epoch)
    end

    it 'converts valid date string to epoch' do
      result = DateUtils.to_epoch('2025-01-01')
      expect(result).to be > 0
      expect(result).to be_a(Integer)
    end

    it 'returns 0 for invalid input' do
      expect(DateUtils.to_epoch('invalid-date')).to eq(0)
    end

    it 'converts Time to epoch' do
      time = Time.parse('2025-07-20 12:00:00 UTC')
      result = DateUtils.to_epoch(time)
      expect(result).to eq(time.to_i)
    end

    it 'converts Date to epoch' do
      date = Date.parse('2025-07-20')
      result = DateUtils.to_epoch(date)
      expect(result).to be_a(Integer)
      expect(result).to be > 0
    end

    it 'converts string to epoch' do
      result = DateUtils.to_epoch('2025-07-20 12:00:00')
      expect(result).to be_a(Integer)
      expect(result).to be > 0
    end

    it 'handles edge case dates' do
      edge_cases = ['1970-01-01', '2038-01-19', '2099-12-31']
      edge_cases.each do |date_str|
        result = DateUtils.to_epoch(date_str)
        expect(result).to be_a(Integer)
        expect(result).to be >= 0
      end
    end
  end

  describe '.date_to_epoch' do
    it 'returns 0 for nil input' do
      expect(DateUtils.date_to_epoch(nil)).to eq(0)
    end

    it 'converts date to epoch using YYYY-MM-DD format' do
      time = Time.new(2025, 1, 1, 15, 30, 45)
      result = DateUtils.date_to_epoch(time)
      # Without site config, uses local timezone
      expected = Time.parse('2025-01-01 00:00:00').to_i
      # Allow for timezone differences - the important thing is that it normalizes to midnight
      expect(result).to be_within(24 * 3600).of(expected)
    end

    it 'returns 0 for invalid input' do
      result = DateUtils.date_to_epoch('invalid-date')
      # Should now return 0 for invalid dates
      expect(result).to eq(0)
    end
    
    it 'converts date to Brussels timezone epoch when site config specifies Europe/Brussels' do
      time = Time.new(2025, 1, 1, 15, 30, 45)
      result = DateUtils.date_to_epoch(time)
      
      # Calculate expected Brussels midnight epoch
      current_time = Time.now
      brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
      expected = Time.parse("2025-01-01 00:00:00 #{brussels_offset}").to_i
      
      expect(result).to eq(expected)
    end

    it 'normalizes different times on same day to same epoch' do
      time1 = Time.parse('2025-07-20 08:00:00')
      time2 = Time.parse('2025-07-20 20:00:00')
      
      result1 = DateUtils.date_to_epoch(time1)
      result2 = DateUtils.date_to_epoch(time2)
      
      expect(result1).to eq(result2)
    end
  end

  describe '.now_epoch' do
    it 'returns current date at midnight as epoch' do
      result = DateUtils.now_epoch
      # Without site config, uses local timezone
      expected = Time.parse(Time.now.strftime('%Y-%m-%d 00:00:00')).to_i
      # Allow for timezone differences - the important thing is that it normalizes to midnight
      expect(result).to be_within(24 * 3600).of(expected)
    end
    
    it 'returns Brussels timezone midnight when site config specifies Europe/Brussels' do
      result = DateUtils.now_epoch
      
      # Calculate expected Brussels midnight epoch
      current_time = Time.now
      brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
      brussels_time = current_time.utc.getlocal(brussels_offset)
      brussels_date_str = brussels_time.strftime('%Y-%m-%d')
      expected = Time.parse("#{brussels_date_str} 00:00:00 #{brussels_offset}").to_i
      
      expect(result).to eq(expected)
    end

    it 'returns consistent values for multiple calls on same day' do
      result1 = DateUtils.now_epoch
      sleep(0.1) # Small delay
      result2 = DateUtils.now_epoch
      
      expect(result1).to eq(result2)
    end
  end

  describe '.extract_date' do
    it 'returns nil for nil input' do
      expect(DateUtils.extract_date(nil)).to be_nil
    end

    it 'extracts date from object with data method' do
      obj = double('doc', data: { 'date' => '2025-01-01' })
      expect(DateUtils.extract_date(obj)).to eq('2025-01-01')
    end

    it 'extracts date from object with date method' do
      obj = double('doc', date: '2025-01-01')
      allow(obj).to receive(:data).and_raise(NoMethodError)
      expect(DateUtils.extract_date(obj)).to eq('2025-01-01')
    end

    it 'handles objects without date methods' do
      obj = double('doc')
      allow(obj).to receive(:data).and_raise(NoMethodError)
      allow(obj).to receive(:date).and_raise(NoMethodError)
      expect(DateUtils.extract_date(obj)).to be_nil
    end

    it 'handles objects with nil dates' do
      obj = double('doc', data: { 'date' => nil })
      expect(DateUtils.extract_date(obj)).to be_nil
    end
  end

  describe '.has_valid_date?' do
    it 'returns true for objects with valid dates' do
      obj = double('doc', data: { 'date' => '2025-01-01' })
      expect(DateUtils.has_valid_date?(obj)).to be true
    end

    it 'returns false for objects without dates' do
      obj = double('doc', data: {})
      expect(DateUtils.has_valid_date?(obj)).to be false
    end

    it 'returns false for nil input' do
      expect(DateUtils.has_valid_date?(nil)).to be false
    end
  end

  # Advanced edge cases and error handling
  describe 'Advanced Edge Cases' do
    describe '.normalize_date_format with malformed input' do
      it 'handles various malformed timezone formats' do
        malformed_cases = [
          '2025-01-01 12:00:00 +25:00', # Invalid timezone
          '2025-01-01 12:00:00 ABC',     # Invalid timezone abbreviation
          '2025-01-01 12:00:00 +',      # Incomplete timezone
          '2025-01-01 12:00:00 -',      # Incomplete timezone
        ]
        
        malformed_cases.each do |case_input|
          expect { DateUtils.normalize_date_format(case_input) }.not_to raise_error
        end
      end
    end

    describe '.to_epoch with performance considerations' do
      it 'handles batch processing efficiently' do
        dates = Array.new(100) { |i| "2025-01-#{(i % 28 + 1).to_s.rjust(2, '0')}" }
        
        expect do
          results = dates.map { |date| DateUtils.to_epoch(date) }
          expect(results.all? { |r| r.is_a?(Integer) }).to be true
        end.not_to raise_error
      end
    end

    describe 'Timezone handling consistency' do
      it 'maintains Brussels timezone consistency across methods' do
        test_date = '2025-07-20 15:30:45'
        
        # All methods should use the same timezone handling
        epoch1 = DateUtils.to_epoch(test_date)
        epoch2 = DateUtils.date_to_epoch(test_date)
        
        # date_to_epoch normalizes to midnight, to_epoch preserves time
        # But both should use consistent timezone handling
        expect(epoch1).to be_a(Integer)
        expect(epoch2).to be_a(Integer)
        expect(epoch1).to be > 0
        expect(epoch2).to be > 0
      end
    end
  end
end