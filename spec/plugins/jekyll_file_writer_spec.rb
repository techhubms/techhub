require 'rspec'
require 'jekyll'
require 'fileutils'
require 'tmpdir'

# Load the module we want to test
require_relative '../../_plugins/jekyll_file_writer'

RSpec.describe JekyllFileWriter do
  let(:temp_dir) { Dir.mktmpdir }
  let(:test_file_path) { File.join(temp_dir, 'subdir', 'test_file.txt') }
  let(:test_content) { 'Test content for file' }

  after do
    FileUtils.remove_entry(temp_dir) if Dir.exist?(temp_dir)
  end

  describe '.write_file_with_dir' do
    it 'creates directory and writes file when directory does not exist' do
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      
      expect(File.exist?(test_file_path)).to be true
      expect(File.read(test_file_path)).to eq(test_content)
      expect(Dir.exist?(File.dirname(test_file_path))).to be true
    end

    it 'writes file when directory already exists' do
      FileUtils.mkdir_p(File.dirname(test_file_path))
      
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      
      expect(File.exist?(test_file_path)).to be true
      expect(File.read(test_file_path)).to eq(test_content)
    end

    it 'does not overwrite file with same content' do
      # First write
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      original_mtime = File.mtime(test_file_path)
      
      # Wait a bit to ensure mtime would change if file was rewritten
      sleep(0.1)
      
      # Second write with same content
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      
      # File should not have been rewritten (mtime unchanged)
      expect(File.mtime(test_file_path)).to eq(original_mtime)
    end

    it 'overwrites file with different content' do
      # First write
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      
      # Second write with different content
      new_content = 'New test content'
      JekyllFileWriter.write_file_with_dir(test_file_path, new_content, 'TestLogger')
      
      expect(File.read(test_file_path)).to eq(new_content)
    end

    it 'handles directory creation errors gracefully with logger' do
      # Mock FileUtils.mkdir_p to raise an error
      allow(FileUtils).to receive(:mkdir_p).and_raise(StandardError.new('Permission denied'))
      allow(Jekyll).to receive(:logger).and_return(double('logger', error: nil))
      
      expect(Jekyll.logger).to receive(:error).with('TestLogger', /Error creating directory/)
      
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
    end

    it 'handles file write errors gracefully with logger' do
      # Create directory first
      FileUtils.mkdir_p(File.dirname(test_file_path))
      
      # Mock File.write to raise an error
      allow(File).to receive(:write).and_raise(StandardError.new('Disk full'))
      allow(Jekyll).to receive(:logger).and_return(double('logger', error: nil))
      
      expect(Jekyll.logger).to receive(:error).with('TestLogger', /Error writing/)
      
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
    end

    it 'logs errors when logger tag is provided' do
      # Mock FileUtils.mkdir_p to raise an error
      allow(FileUtils).to receive(:mkdir_p).and_raise(StandardError.new('Permission denied'))
      allow(Jekyll).to receive(:logger).and_return(double('logger', error: nil))
      
      expect(Jekyll.logger).to receive(:error).with('TestLogger', /Error creating directory/)
      
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
    end

    it 'handles file read errors when checking for content changes' do
      # Create the file first
      JekyllFileWriter.write_file_with_dir(test_file_path, test_content, 'TestLogger')
      
      # Mock File.read to raise an error and mock Jekyll logger to suppress output
      allow(File).to receive(:read).and_raise(StandardError.new('Read error'))
      allow(File).to receive(:write).and_call_original
      allow(Jekyll).to receive(:logger).and_return(double('logger', error: nil))
      
      # Should still write the file when read fails
      expect {
        JekyllFileWriter.write_file_with_dir(test_file_path, 'new content', 'TestLogger')
      }.not_to raise_error
    end

    it 'works with minimal parameters' do
      simple_path = File.join(temp_dir, 'simple.txt')
      
      JekyllFileWriter.write_file_with_dir(simple_path, test_content, 'TestLogger')
      
      expect(File.exist?(simple_path)).to be true
      expect(File.read(simple_path)).to eq(test_content)
    end
  end
end
