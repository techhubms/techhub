require 'fileutils'

# Generic utility for safe file writing with directory creation
module JekyllFileWriter
  def self.write_file_with_dir(file_path, content, logger_tag)
    dir_path = File.dirname(file_path)
    begin
      FileUtils.mkdir_p(dir_path) unless Dir.exist?(dir_path)
    rescue => e
      Jekyll.logger.error logger_tag, "Error creating directory #{dir_path} for #{file_path}: #{e.message}"
      return
    end
    begin
      if !File.exist?(file_path) || File.read(file_path) != content
        File.write(file_path, content)
        Jekyll.logger.info logger_tag, "Updated #{file_path}"
      end
    rescue => e
      Jekyll.logger.error logger_tag, "Error writing #{file_path}: #{e.message}"
    end
  end
end
