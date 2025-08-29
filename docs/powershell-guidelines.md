---
applyTo: "**/*.ps1"
---

# Instructions for AI models when generating PowerShell scripts and commands

## PowerShell Syntax Rules

**CRITICAL**: Use backticks (\`) to escape special characters in PowerShell. Never use backslashes (`\`) for escaping in PowerShell.

- ✅ **CORRECT**:

  ```powershell
  Write-Host "Value is $variable"
  ```

- ❌ **WRONG**:

  ```powershell
  Write-Host "Value is `$variable"
  ```

- ✅ **CORRECT**:

  ```powershell
  Write-Host "Value is `"$variable`""
  ```

- ❌ **WRONG**:

  ```powershell
  Write-Host "Value is \"\$variable\""
  ```

- ✅ **CORRECT**:

  ```powershell
  $variable = "some value"
  ```

- ❌ **WRONG**:

  ```powershell
  `$variable = "some value"
  ```

- ✅ **CORRECT**:

  ```powershell
  $variable = "a $($object.with.dottednotation) value"  
  ```

- ❌ **WRONG**:

  ```powershell
  $variable = "a $object.with.dottednotation value"
  ```

- ✅ **CORRECT**:

  ```powershell
  $variable = "a $($object['key']) value"  
  ```

- ❌ **WRONG**:

  ```powershell
  $variable = "a $object['key'] value"
  ```

- ✅ **CORRECT**:

  ```powershell
  $variable = "value:type = $($type):string"  
  ```

- ❌ **WRONG**:

  ```powershell
  $variable = "value:type = $type:string"  
  ```

## Best Practices

1. **Always use full cmdlet names** in scripts for clarity (e.g., `Get-ChildItem` instead of `ls`)
2. **Use proper parameter names** (e.g., `-Path` instead of positional parameters)
3. **Handle errors appropriately** with try/catch blocks where needed
4. **Use meaningful variable names** with proper PowerShell naming conventions
5. **Add comments** to explain complex logic
6. **Use proper formatting** with consistent indentation and spacing
7. **Test scripts** before providing them to users

## Common PowerShell Patterns

### Error Handling

```powershell
try {
    # Command that might fail
    Get-Content "file.txt"
} catch {
    Write-Host "Error: $($_.Exception.Message)"
}
```

### Parameter Validation

```powershell
param(
    [Parameter(Mandatory=$true)]
    [string]$InputPath,
    
    [Parameter(Mandatory=$false)]
    [switch]$Force
)
```

### Pipeline Operations

```powershell
Get-ChildItem -Path "*.txt" |
    Where-Object { $_.Length -gt 1KB } |
    Select-Object Name, Length |
    Sort-Object Length -Descending
```

## PowerShell Testing Standards

### Testing Framework

Use **Pester v5** for all PowerShell script testing. Pester provides comprehensive testing capabilities with modern syntax and excellent PowerShell integration.

### Test File Organization

```text
spec/powershell/
├── [ScriptName].Tests.ps1          # Test files named after the script being tested
└── test-data/                      # Test data files (if needed)
```

### Test File Naming Convention

- Test files should follow the pattern: `[ScriptName].Tests.ps1`
- Use exact script name with `.Tests.ps1` suffix
- Example: `Get-FilteredTags.ps1` → `Get-FilteredTags.Tests.ps1`

### Test Structure Standards

#### Standard Test Setup Pattern

**All PowerShell tests should follow this standardized initialization pattern**:

```powershell
Describe "Function-Name" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        
        # Custom BeforeAll setup specific to this test can be added here
        # Example: Mock external dependencies, set environment variables, etc.
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Custom BeforeEach setup specific to this test can be added here
        # Example: Reset variables, clear arrays, etc.
    }
    
    Context "Parameter Validation" {
        It "Should throw when required parameter is null" {
            { Function-Name -Parameter $null } | Should -Throw "*Parameter cannot be null*"
        }
        
        It "Should throw when required parameter is empty" {
            { Function-Name -Parameter @() } | Should -Throw "*Parameter cannot be empty*"
        }
    }
    
    Context "Core Functionality" {
        It "Should process valid input correctly" {
            # Arrange
            $input = @("test-value")
            $expected = @("test value")
            
            # Act
            $result = Function-Name -Parameter $input
            
            # Assert
            $result | Should -Contain "test value"
        }
    }
    
    Context "Edge Cases" {
        It "Should handle special characters properly" {
            # Test special character handling
            $input = @("test-tag_with-special@chars")
            $result = Function-Name -Parameter $input
            $result | Should -Not -BeNullOrEmpty
        }
    }
}
```

#### Initialization Scripts

**Initialize-BeforeAll.ps1**: Contains common setup logic that runs once per `Describe` block:

- Function loading from `.github/scripts/functions/`
- Common variable initialization
- Mock setup that applies to all tests
- Error handling configuration

**Initialize-BeforeEach.ps1**: Contains setup logic that runs before each individual test:

- Variable reset/cleanup
- Test isolation setup
- Fresh state initialization

**Benefits of Standardized Setup**:

- **Consistency**: All tests follow the same initialization pattern
- **Maintainability**: Common setup logic is centralized and reusable
- **Reliability**: Ensures proper function loading and test isolation
- **Extensibility**: Custom setup can be added after the standard initialization

**Example with Custom Setup**:

```powershell
Describe "ConvertTo-SafeFilename" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        
        # Custom setup for this specific test
        $script:TestDataPath = Join-Path $TestDrive "test-data"
        New-Item -Path $script:TestDataPath -ItemType Directory -Force
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        
        # Custom per-test setup
        $script:TestCounter = 0
        Clear-Variable -Name "TestResults" -Scope Script -ErrorAction SilentlyContinue
    }
```

### Test Data Management

**Prefer In-Memory Test Data**: Create test data within test functions rather than external files when possible.

```powershell
# Good: In-memory test data

It "Should process markdown frontmatter" {
    $testContent = @"
---
title: Test Post
tags: ["ai", "github copilot"]
categories: ["AI"]
---
Test content here
"@
    
    $result = Format-FrontMatterValue -Content $testContent -Key "tags"
    $result | Should -Contain "ai"
}
```

### Parameter Testing Patterns

**Always test parameter validation thoroughly**:

```powershell
Context "Parameter Validation" {
    It "Should validate required parameters" {
        { Get-FilteredTags -Tags $null } | Should -Throw
        { Get-FilteredTags -Tags @() } | Should -Throw
    }
    
    It "Should validate parameter types" {
        { Get-FilteredTags -Tags "string-instead-of-array" } | Should -Throw
    }
    
    It "Should handle optional parameters" {
        { Get-FilteredTags -Tags @("test") } | Should -Not -Throw
    }
}
```

### Assertion Standards

**Use descriptive Should assertions**:

```powershell
# Good: Specific assertions

$result.tags | Should -Contain "expected tag"
$result.tags | Should -HaveCount 3
$result.tags | Should -Not -BeNullOrEmpty

# Good: Pattern matching

$result | Should -Match "^processed-.*"

# Good: Type checking

$result | Should -BeOfType [System.Array]
```

### Mock Usage (When Needed)

**Use mocks sparingly** - prefer testing actual function logic:

```powershell
BeforeAll {
    # Mock external dependencies, not the function under test
    Mock Write-Host { }
    Mock Get-Content { return "mocked file content" }
}

It "Should call external dependency correctly" {
    Get-FilteredTags -Tags @("test") -Categories @("AI") -Collection "news"
    
    Should -Invoke Get-Content -Exactly 1 -ParameterFilter { $Path -eq "expected-file.txt" }
}
```

### Performance Considerations

**Keep tests fast and focused**:

```powershell
It "Should process large tag arrays efficiently" {
    $largeTags = 1..1000 | ForEach-Object { "tag-$_" }
    
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    $result = Get-FilteredTags -Tags $largeTags -Categories @("AI") -Collection "news"
    $stopwatch.Stop()
    
    $result | Should -Not -BeNullOrEmpty
    $stopwatch.ElapsedMilliseconds | Should -BeLessThan 1000 # Should complete within 1 second
}
```

### Error Testing Patterns

**Test both success and failure scenarios**:

```powershell
Context "Error Handling" {
    It "Should throw descriptive error for invalid input" {
        $invalidInput = @("", $null, "   ")
        
        { Get-FilteredTags -Tags $invalidInput } | Should -Throw "*Tags cannot contain empty values*"
    }
    
    It "Should handle file processing errors gracefully" {
        Mock Get-Content { throw "File not found" }
        
        { Process-MarkdownFile -Path "nonexistent.md" } | Should -Throw "*Failed to process file*"
    }
}
```

### Documentation in Tests

**Use descriptive test names that serve as documentation**:

```powershell
# Good: Test name explains expected behavior

It "Should convert dash-separated tags to space-separated format" { }
It "Should preserve categories and collection in final tag list" { }
It "Should remove duplicate tags while maintaining order" { }

# Avoid: Generic test names

It "Should work correctly" { }
It "Should return result" { }
```

### Running Tests

**Use the centralized test runner for modern PowerShell testing**:

```powershell
# Run all PowerShell tests using wrapper script

pwsh /workspaces/techhub/run-powershell-tests.ps1

# Run specific test file

pwsh /workspaces/techhub/run-powershell-tests.ps1 -TestFile "spec/powershell/Get-FilteredTags.Tests.ps1"

# Run with coverage analysis

pwsh /workspaces/techhub/run-powershell-tests.ps1 -Coverage

# Run with detailed output

pwsh /workspaces/techhub/run-powershell-tests.ps1 -Detailed

# Run tests matching name pattern

pwsh /workspaces/techhub/run-powershell-tests.ps1 -TestName "tag normalization"
```

### Test Coverage and CI/CD Integration

**PowerShell tests in the testing pipeline**:

1. **Fast Execution**: PowerShell tests run first for immediate feedback
2. **Dependency Free**: Tests should not require external services or files
3. **Isolated**: Each test should be independent and order-agnostic
4. **Comprehensive**: Cover all public function interfaces and edge cases

**Coverage Requirements**:

- **Tag Processing**: Normalization, filtering, and mapping logic
- **Frontmatter Parsing**: YAML extraction and modification
- **String Manipulation**: Text processing and formatting functions  
- **Parameter Validation**: Input validation and error conditions
- **Edge Cases**: Empty inputs, malformed data, special characters

**Should NOT cover**:

- File I/O operations (use mocks instead)
- Jekyll integration (belongs in Ruby tests)
- Browser behavior (belongs in Playwright tests)
- Performance optimization (focus on correctness)

**CI execution example**:

```powershell
# CI pipeline step

pwsh /workspaces/techhub/run-powershell-tests.ps1 -Coverage -Detailed
```

This testing approach ensures PowerShell preprocessing scripts are thoroughly validated before the Jekyll build process, providing confidence in tag normalization and content processing functionality.
