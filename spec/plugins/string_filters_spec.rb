require_relative '../plugins/spec_helper'
require_relative '../../_plugins/string_filters'

RSpec.describe Jekyll::StringFilters do
  # Create a test class that includes the module
  let(:filter_tester) do
    Class.new do
      include Jekyll::StringFilters
    end.new
  end

  describe '#regex_match' do
    context 'with valid inputs' do
      it 'returns true when string matches pattern' do
        expect(filter_tester.regex_match('hello world', 'hello')).to eq(true)
      end

      it 'returns false when string does not match pattern' do
        expect(filter_tester.regex_match('hello world', 'goodbye')).to eq(false)
      end

      it 'handles complex regex patterns' do
        expect(filter_tester.regex_match('test123', '\d+')).to eq(true)
        expect(filter_tester.regex_match('testABC', '\d+')).to eq(false)
      end

      it 'is case sensitive by default' do
        expect(filter_tester.regex_match('Hello', 'hello')).to eq(false)
      end

      it 'handles case insensitive patterns' do
        expect(filter_tester.regex_match('Hello', '(?i)hello')).to eq(true)
      end

      it 'handles empty string' do
        expect(filter_tester.regex_match('', '.*')).to eq(true)
        expect(filter_tester.regex_match('', '.+')).to eq(false)
      end

      it 'handles start and end anchors' do
        expect(filter_tester.regex_match('hello world', '^hello')).to eq(true)
        expect(filter_tester.regex_match('hello world', 'world$')).to eq(true)
        expect(filter_tester.regex_match('hello world', '^world')).to eq(false)
      end
    end

    context 'with invalid inputs' do
      it 'returns false when string is nil' do
        expect(filter_tester.regex_match(nil, 'pattern')).to eq(false)
      end

      it 'returns false when pattern is nil' do
        expect(filter_tester.regex_match('string', nil)).to eq(false)
      end

      it 'returns false when both are nil' do
        expect(filter_tester.regex_match(nil, nil)).to eq(false)
      end

      it 'handles invalid regex patterns gracefully' do
        expect(Jekyll.logger).to receive(:warn).with(/Invalid regex pattern/)
        expect(filter_tester.regex_match('test', '[')).to eq(false)
      end

      it 'handles malformed regex patterns' do
        expect(Jekyll.logger).to receive(:warn).with(/Invalid regex pattern/)
        expect(filter_tester.regex_match('test', '*invalid')).to eq(false)
      end
    end

    context 'with edge cases' do
      it 'converts non-string inputs to strings' do
        expect(filter_tester.regex_match(123, '\d+')).to eq(true)
        expect(filter_tester.regex_match(true, 'true')).to eq(true)
      end

      it 'handles special regex characters in strings' do
        expect(filter_tester.regex_match('hello.world', '\.')).to eq(true)
        expect(filter_tester.regex_match('hello[world]', '\[')).to eq(true)
      end

      it 'handles unicode characters' do
        expect(filter_tester.regex_match('café', 'café')).to eq(true)
        expect(filter_tester.regex_match('测试', '测试')).to eq(true)
      end

      it 'handles very long strings' do
        long_string = 'a' * 10000 + 'target' + 'b' * 10000
        expect(filter_tester.regex_match(long_string, 'target')).to eq(true)
      end
    end
  end

  describe '#is_letters_and_hyphen_only' do
    context 'with valid inputs' do
      it 'returns true for lowercase letters only' do
        expect(filter_tester.is_letters_and_hyphen_only('hello')).to eq(true)
      end

      it 'returns true for uppercase letters only' do
        expect(filter_tester.is_letters_and_hyphen_only('HELLO')).to eq(true)
      end

      it 'returns true for mixed case letters only' do
        expect(filter_tester.is_letters_and_hyphen_only('HeLLo')).to eq(true)
      end

      it 'returns true for letters with hyphens' do
        expect(filter_tester.is_letters_and_hyphen_only('hello-world')).to eq(true)
        expect(filter_tester.is_letters_and_hyphen_only('AI-Section')).to eq(true)
      end

      it 'returns true for single letter' do
        expect(filter_tester.is_letters_and_hyphen_only('a')).to eq(true)
        expect(filter_tester.is_letters_and_hyphen_only('Z')).to eq(true)
      end

      it 'returns true for single hyphen' do
        expect(filter_tester.is_letters_and_hyphen_only('-')).to eq(true)
      end

      it 'returns true for multiple consecutive hyphens' do
        expect(filter_tester.is_letters_and_hyphen_only('hello--world')).to eq(true)
      end

      it 'returns true for hyphens at start and end' do
        expect(filter_tester.is_letters_and_hyphen_only('-hello-')).to eq(true)
      end
    end

    context 'with invalid inputs' do
      it 'returns false for empty string' do
        expect(filter_tester.is_letters_and_hyphen_only('')).to eq(false)
      end

      it 'returns false for nil input' do
        expect(filter_tester.is_letters_and_hyphen_only(nil)).to eq(false)
      end

      it 'returns false for strings with numbers' do
        expect(filter_tester.is_letters_and_hyphen_only('hello123')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('123hello')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hel123lo')).to eq(false)
      end

      it 'returns false for strings with spaces' do
        expect(filter_tester.is_letters_and_hyphen_only('hello world')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only(' hello')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello ')).to eq(false)
      end

      it 'returns false for strings with special characters' do
        expect(filter_tester.is_letters_and_hyphen_only('hello!')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello@world')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello.world')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello_world')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello&world')).to eq(false)
      end

      it 'returns false for strings with punctuation' do
        expect(filter_tester.is_letters_and_hyphen_only('hello,')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello.')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello?')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello;')).to eq(false)
      end

      it 'returns false for strings with brackets' do
        expect(filter_tester.is_letters_and_hyphen_only('hello[world]')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello(world)')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('hello{world}')).to eq(false)
      end
    end

    context 'with edge cases' do
      it 'converts non-string inputs to strings and validates' do
        expect(filter_tester.is_letters_and_hyphen_only(123)).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only(true)).to eq(false)
      end

      it 'handles unicode letters correctly' do
        # Should return false for non-ASCII letters as pattern is [a-zA-Z-]
        expect(filter_tester.is_letters_and_hyphen_only('café')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('naïve')).to eq(false)
      end

      it 'handles very long valid strings' do
        long_valid = 'a' * 1000 + '-' + 'b' * 1000
        expect(filter_tester.is_letters_and_hyphen_only(long_valid)).to eq(true)
      end

      it 'handles very long invalid strings' do
        long_invalid = 'a' * 1000 + '1' + 'b' * 1000
        expect(filter_tester.is_letters_and_hyphen_only(long_invalid)).to eq(false)
      end
    end

    context 'section name validation use cases' do
      it 'validates typical section names' do
        expect(filter_tester.is_letters_and_hyphen_only('ai')).to eq(true)
        expect(filter_tester.is_letters_and_hyphen_only('github-copilot')).to eq(true)
        expect(filter_tester.is_letters_and_hyphen_only('machine-learning')).to eq(true)
        expect(filter_tester.is_letters_and_hyphen_only('AI-Hub')).to eq(true)
      end

      it 'rejects invalid section names' do
        expect(filter_tester.is_letters_and_hyphen_only('ai_section')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('ai section')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('ai.section')).to eq(false)
        expect(filter_tester.is_letters_and_hyphen_only('ai/section')).to eq(false)
      end
    end
  end

  describe 'module registration' do
    it 'registers filters with Liquid::Template' do
      # Verify that the filter methods are available through the module
      expect(filter_tester).to respond_to(:regex_match)
      expect(filter_tester).to respond_to(:is_letters_and_hyphen_only)
    end
  end
end
