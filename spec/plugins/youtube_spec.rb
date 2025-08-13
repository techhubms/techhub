require 'rspec'
require 'liquid'

# Load the module we want to test
require_relative '../../_plugins/youtube'

# Test helper to create mock tokens object for Liquid::Tag testing
def create_mock_tokens
  tokens = Object.new
  def tokens.line_number
    1
  end
  tokens
end

# Test helper to create YouTube tag instances
def create_youtube_tag(markup)
  YouTube.allocate.tap { |instance| instance.send(:initialize, 'youtube', markup, create_mock_tokens) }
end

RSpec.describe YouTube do
  describe 'initialization' do
    it 'extracts YouTube ID from markup' do
      youtube_tag = create_youtube_tag('dQw4w9WgXcQ')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'extracts YouTube ID with extra parameters' do
      youtube_tag = create_youtube_tag('dQw4w9WgXcQ 560 315')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'raises error when no YouTube ID provided' do
      expect {
        create_youtube_tag('')
      }.to raise_error('No YouTube ID provided in the "youtube" tag')
    end

    it 'raises error when markup is invalid' do
      expect {
        create_youtube_tag('   ')
      }.to raise_error('No YouTube ID provided in the "youtube" tag')
    end

    it 'handles YouTube ID with whitespace' do
      youtube_tag = create_youtube_tag('  dQw4w9WgXcQ  ')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end
  end

  describe '#render' do
    let(:context) { {} }
    let(:youtube_tag) { create_youtube_tag('dQw4w9WgXcQ') }

    it 'renders iframe with correct YouTube embed URL' do
      result = youtube_tag.render(context)
      
      expect(result).to include('<iframe')
      expect(result).to include('class="youtube"')
      expect(result).to include('src="https://www.youtube.com/embed/dQw4w9WgXcQ"')
      expect(result).to include('loading="lazy"')
      expect(result).to include('allowfullscreen')
    end

    it 'includes all required iframe attributes' do
      result = youtube_tag.render(context)
      
      # Check for security and accessibility attributes
      expect(result).to include('allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"')
      expect(result).to include('allowfullscreen')
      expect(result).to include('loading="lazy"')
    end

    it 'renders as a single line iframe tag' do
      result = youtube_tag.render(context)
      
      expect(result).to start_with('<iframe')
      expect(result).to end_with('></iframe>')
      expect(result.count("\n")).to eq(0)
    end

    it 'handles different YouTube IDs correctly' do
      different_id = 'abc123XYZ'
      youtube_tag = create_youtube_tag(different_id)
      result = youtube_tag.render(context)
      
      expect(result).to include("src=\"https://www.youtube.com/embed/#{different_id}\"")
    end
  end

  describe 'Liquid template registration' do
    it 'registers the tag with Liquid' do
      # Test that the tag is registered by creating a template and parsing it
      template_source = '{% youtube dQw4w9WgXcQ %}'
      
      expect {
        template = Liquid::Template.parse(template_source)
        result = template.render
        expect(result).to include('youtube.com/embed/dQw4w9WgXcQ')
      }.not_to raise_error
    end

    it 'works within a Liquid template context' do
      template_source = '{% youtube test123 %}'
      template = Liquid::Template.parse(template_source)
      result = template.render
      
      expect(result).to include('<iframe')
      expect(result).to include('youtube.com/embed/test123')
      expect(result).to include('class="youtube"')
    end
  end

  describe 'markup parsing edge cases' do
    it 'handles YouTube ID followed by numbers' do
      youtube_tag = create_youtube_tag('dQw4w9WgXcQ 560 315')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'handles YouTube ID with just one number' do
      youtube_tag = create_youtube_tag('dQw4w9WgXcQ 560')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'handles YouTube ID with extra whitespace and numbers' do
      youtube_tag = create_youtube_tag('  dQw4w9WgXcQ   560   315  ')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'handles common YouTube ID formats' do
      # Test various YouTube ID formats
      ids = [
        'dQw4w9WgXcQ',      # Standard 11-character ID
        'abc123XYZ-_',      # With allowed special characters
        '1234567890a',      # Alphanumeric
        'A1B2C3D4E5F'       # Mixed case
      ]

      ids.each do |id|
        youtube_tag = create_youtube_tag(id)
        expect(youtube_tag.instance_variable_get(:@id)).to eq(id)
        
        result = youtube_tag.render({})
        expect(result).to include("youtube.com/embed/#{id}")
      end
    end
  end

  describe 'error handling' do
    it 'handles special characters in markup gracefully' do
      expect {
        create_youtube_tag('!@#$%^&*()')
      }.to raise_error('No YouTube ID provided in the "youtube" tag')
    end

    it 'handles only whitespace in markup' do
      expect {
        create_youtube_tag('     ')
      }.to raise_error('No YouTube ID provided in the "youtube" tag')
    end

    it 'handles tabs and newlines in markup' do
      expect {
        create_youtube_tag("\t\n\r")
      }.to raise_error('No YouTube ID provided in the "youtube" tag')
    end
  end

  describe 'regex matching edge cases' do
    it 'handles YouTube ID with maximum length' do
      long_id = 'a' * 11  # YouTube IDs are typically 11 characters
      youtube_tag = create_youtube_tag(long_id)
      expect(youtube_tag.instance_variable_get(:@id)).to eq(long_id)
    end

    it 'extracts only the first token as YouTube ID' do
      youtube_tag = create_youtube_tag('dQw4w9WgXcQ some other text here')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('dQw4w9WgXcQ')
    end

    it 'handles single character YouTube ID' do
      youtube_tag = create_youtube_tag('a')
      expect(youtube_tag.instance_variable_get(:@id)).to eq('a')
    end
  end
end
