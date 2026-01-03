# spec/spec_helper.rb
require 'rspec'
require 'jekyll'
require 'liquid'

# Initialize Jekyll environment for plugin testing
Jekyll.logger.log_level = :warn

RSpec.configure do |config|
  # Use the expect syntax
  config.expect_with :rspec do |expectations|
    expectations.include_chain_clauses_in_custom_matcher_descriptions = true
  end

  # Use the new syntax for mocking
  config.mock_with :rspec do |mocks|
    mocks.verify_partial_doubles = true
  end

  # This option will default to `true` in RSpec 4.
  config.shared_context_metadata_behavior = :apply_to_host_groups

  # Allow more verbose output when running specs
  config.filter_run_when_matching :focus

  # Allow using `fit`, `fdescribe`, and `fcontext` to focus specs
  config.run_all_when_everything_filtered = true

  # Disable monkey patching of Object class
  config.disable_monkey_patching!

  # Print the slowest examples
  config.profile_examples = 10 if ENV['PROFILE']

  # Randomize the order tests are run
  config.order = :random

  # Seed global randomization in this process using the `--seed` CLI option.
  Kernel.srand config.seed
end
