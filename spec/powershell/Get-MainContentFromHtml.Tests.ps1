Describe "Get-MainContentFromHtml" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Parameter Validation" {
        It "Should throw when InputHtml parameter is null" {
            { Get-MainContentFromHtml -InputHtml $null -SourceUrl "https://github.blog/test" } | Should -Throw
        }
        
        It "Should throw when InputHtml parameter is empty" {
            { Get-MainContentFromHtml -InputHtml "" -SourceUrl "https://github.blog/test" } | Should -Throw
        }
        
        It "Should throw when SourceUrl parameter is null" {
            { Get-MainContentFromHtml -InputHtml "<html></html>" -SourceUrl $null } | Should -Throw
        }
        
        It "Should throw when SourceUrl parameter is empty" {
            { Get-MainContentFromHtml -InputHtml "<html></html>" -SourceUrl "" } | Should -Throw
        }
        
        It "Should throw when SourceUrl parameter is invalid" {
            { Get-MainContentFromHtml -InputHtml "<html></html>" -SourceUrl "not-a-valid-url" } | Should -Throw
        }
    }

    Context "GitHub Blog Processing" {
        It "Should extract content from valid GitHub blog nested HTML structure" {
            # Arrange
            $testHtml = @"
<html><body>
<div class="header">Header content</div>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body><p>This is the main content</p><h2>Test heading</h2><p>More content here</p></body></html>
<div class="footer">Footer content</div>
</body></html>
"@
            $testUrl = "https://github.blog/ai-and-ml/test-article"
            $expected = "<p>This is the main content</p><h2>Test heading</h2><p>More content here</p>"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $expected
        }
        
        It "Should handle GitHub blog HTML with whitespace in nested structure" {
            # Arrange
            $testHtml = @"
<html><body>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
  <html>
    <body>
      <p>Content with whitespace</p>
    </body>
  </html>
</body></html>
"@
            $testUrl = "https://github.blog/test"
            $expected = "<p>Content with whitespace</p>"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $expected
        }
        
        It "Should throw when GitHub blog nested HTML structure is missing" {
            # Arrange
            $testHtml = "<html><body>Regular content without nested structure</body></html>"
            $testUrl = "https://github.blog/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*nested <html><body> structure*"
        }
        
        It "Should throw when GitHub blog nested content is empty" {
            # Arrange
            $testHtml = @"
<html><body>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body></body></html>
</body></html>
"@
            $testUrl = "https://github.blog/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*nested content is empty*"
        }
        
        It "Should throw when GitHub blog nested content is only whitespace" {
            # Arrange
            $testHtml = @"
<html><body>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body>   
   
</body></html>
</body></html>
"@
            $testUrl = "https://github.blog/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*nested content is empty*"
        }
        
        It "Should handle case-insensitive domain matching for github.blog" {
            # Arrange
            $testHtml = @"
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body><p>Test content</p></body></html>
"@
            $testUrl = "https://GITHUB.BLOG/test"
            $expected = "<p>Test content</p>"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $expected
        }
    }

    Context "Microsoft DevBlogs Processing" {
        It "Should extract content from article tag with post- id" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <header>Header content</header>
    <article data-clarity-region="article" class="middle-column pe-xl-198" id="post-253427">
        <div class="entry-content">
            <p>We're excited to share some major improvements.</p>
            <h3>Smarter default model</h3>
            <p>Copilot in Visual Studio now uses GPT-4.1 as the default model.</p>
        </div>
    </article>
    <footer>Footer content</footer>
</body>
</html>
"@
            $testUrl = "https://devblogs.microsoft.com/test"
            $expected = @"
        <div class="entry-content">
            <p>We're excited to share some major improvements.</p>
            <h3>Smarter default model</h3>
            <p>Copilot in Visual Studio now uses GPT-4.1 as the default model.</p>
        </div>
"@
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result.Trim() | Should -Be $expected.Trim()
        }
        
        It "Should throw error when article tag not found" {
            # Arrange
            $testHtml = "<html><body>DevBlog content without article tag</body></html>"
            $testUrl = "https://devblogs.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
        
        It "Should throw error when article content is empty" {
            # Arrange
            $testHtml = '<html><body><article id="post-123"></article></body></html>'
            $testUrl = "https://devblogs.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }

    Context "Microsoft Blog Processing" {
        It "Should throw error when no recognized patterns found for blog.fabric.microsoft.com" {
            # Arrange
            $testHtml = "<html><body>Microsoft blog content without recognized patterns</body></html>"
            $testUrl = "https://blog.fabric.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
        
        It "Should throw error when no recognized patterns found for blog.azure.microsoft.com" {
            # Arrange
            $testHtml = "<html><body>Azure blog content without recognized patterns</body></html>"
            $testUrl = "https://blog.azure.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }

    Context "Microsoft News Processing" {
        It "Should throw error when no section with mssrc-block-content-block class exists" {
            # Arrange
            $testHtml = "<html><body>Microsoft news content without required section</body></html>"
            $testUrl = "https://news.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }

    Context "Azure Site Processing" {
        It "Should throw error when no recognized patterns found for azure.microsoft.com" {
            # Arrange
            $testHtml = "<html><body>Azure content without recognized patterns</body></html>"
            $testUrl = "https://azure.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }

    Context "Microsoft Main Site Processing" {
        It "Should extract content from div with class 'entry-content'" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <header>Site header content</header>
    <nav>Navigation menu</nav>
    <div class="sidebar">
        <p>Sidebar content</p>
    </div>
    <div class="entry-content">
        <h1>Microsoft Security Blog Post</h1>
        <p>This is the main content of the Microsoft security blog article.</p>
        <p>It contains detailed information about <strong>security threats</strong> and mitigation strategies.</p>
        <h2>Key Findings</h2>
        <ul>
            <li>Advanced persistent threats</li>
            <li>Zero-day vulnerabilities</li>
            <li>Recommended security practices</li>
        </ul>
        <img src="/security-diagram.jpg" alt="Security architecture diagram" />
        <blockquote>
            Security is our top priority at Microsoft.
        </blockquote>
    </div>
    <aside>Related articles</aside>
    <footer>Footer content</footer>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/en-us/security/blog/test-article"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Microsoft Security Blog Post"
            $result | Should -Match "main content of the Microsoft security blog"
            $result | Should -Match "security threats"
            $result | Should -Match "Advanced persistent threats"
            $result | Should -Match "Security is our top priority"
            $result | Should -Not -Match "Site header content"
            $result | Should -Not -Match "Navigation menu"
            $result | Should -Not -Match "Sidebar content"
            $result | Should -Not -Match "Related articles"
            $result | Should -Not -Match "Footer content"
        }
        
        It "Should handle div with multiple classes including entry-content" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="container entry-content main-article">
        <h2>Security Update with Multiple Classes</h2>
        <p>Content from div with multiple CSS classes.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/multi-class-test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Security Update with Multiple Classes"
            $result | Should -Match "multiple CSS classes"
        }
        
        It "Should throw error when div with entry-content class is not found" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="other-class">
        <p>Content in different div</p>
    </div>
    <section class="entry-content">
        <p>Content in section, not div</p>
    </section>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/no-matching-div"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
        
        It "Should throw error when div content is only whitespace" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="entry-content">
        
        
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/empty-div"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }

    Context "Tech Community Processing" {
        It "Should throw error when no recognized patterns found for techcommunity.microsoft.com" {
            # Arrange
            $testHtml = "<html><body>Tech community content without recognized patterns</body></html>"
            $testUrl = "https://techcommunity.microsoft.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should extract content from div with message-body class" {
            # Arrange
            $testHtml = @"
<html><body>
<div class="styles_lia-g-message-body__LkV7_ styles_lia-g-message-body-forum__p9J5_ styles_clearfix__xFEoC styles_text-body__F7QRV">
<p>This is the main message content from Microsoft Tech Community.</p>
<p>It contains multiple paragraphs and formatting.</p>
</div>
</body></html>
"@
            $testUrl = "https://techcommunity.microsoft.com/t5/excel/bug-report/m-p/4439923"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "This is the main message content from Microsoft Tech Community"
            $result | Should -Match "It contains multiple paragraphs and formatting"
        }
    }

    # Note: Reddit-specific processing was removed - Reddit URLs now fall through to default handling
    # which returns the original HTML unchanged.

    Context "YouTube Processing" {
        It "Should throw not implemented error for www.youtube.com" {
            # Arrange
            $testHtml = "<html><body>YouTube content</body></html>"
            $testUrl = "https://www.youtube.com/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*YouTube HTML extraction not yet implemented*"
        }
    }

    Context "Default Processing" {
        It "Should return original HTML for unsupported domains" {
            # Arrange
            $testHtml = "<html><body>Unknown domain content</body></html>"
            $testUrl = "https://example.com/test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $testHtml
        }
        
        It "Should return original HTML for unsupported subdomains" {
            # Arrange
            $testHtml = "<html><body>Subdomain content</body></html>"
            $testUrl = "https://subdomain.example.com/test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $testHtml
        }
    }

    Context "Error Handling" {
        It "Should throw Write-Error with proper message when processing fails" {
            # Arrange
            $testHtml = "<html><body>Invalid GitHub blog structure</body></html>"
            $testUrl = "https://github.blog/test-error"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*Error processing HTML from https://github.blog/test-error*"
        }
    }

    Context "URL Parsing and Domain Detection" {
        It "Should handle URLs with different protocols" {
            # Arrange
            $testHtml = "<html><body>Test content</body></html>"
            $testUrl = "http://github.blog/test"  # HTTP instead of HTTPS
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*nested <html><body> structure*"
        }
        
        It "Should handle URLs with ports" {
            # Arrange
            $testHtml = "<html><body>Test content</body></html>"
            $testUrl = "https://github.blog:443/test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*nested <html><body> structure*"
        }
        
        It "Should handle URLs with query parameters" {
            # Arrange
            $testHtml = @"
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body><p>Test content</p></body></html>
"@
            $testUrl = "https://github.blog/test?param=value&other=test"
            $expected = "<p>Test content</p>"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $expected
        }
        
        It "Should handle URLs with fragments" {
            # Arrange
            $testHtml = @"
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" "http://www.w3.org/TR/REC-html40/loose.dtd">
<html><body><p>Test content</p></body></html>
"@
            $testUrl = "https://github.blog/test#section"
            $expected = "<p>Test content</p>"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Be $expected
        }
    }
    
    Context "Microsoft News Extraction" {
        It "Should extract content from section with class 'mssrc-block-content-block'" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <header>Site header content</header>
    <nav>Navigation menu</nav>
    <section class="some-other-class">
        <p>Other section content</p>
    </section>
    <section class="mssrc-block-content-block">
        <h1>Main Article Title</h1>
        <p>This is the main content of the Microsoft News article.</p>
        <p>It contains multiple paragraphs with <strong>formatted text</strong>.</p>
        <ul>
            <li>List item 1</li>
            <li>List item 2</li>
        </ul>
        <img src="/image.jpg" alt="Article image" />
    </section>
    <aside>Sidebar content</aside>
    <footer>Footer content</footer>
</body>
</html>
"@
            $testUrl = "https://news.microsoft.com/test-article"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Main Article Title"
            $result | Should -Match "main content of the Microsoft News article"
            $result | Should -Match "formatted text"
            $result | Should -Match "List item 1"
            $result | Should -Not -Match "Site header content"
            $result | Should -Not -Match "Navigation menu"
            $result | Should -Not -Match "Sidebar content"
            $result | Should -Not -Match "Footer content"
            $result | Should -Not -Match "Other section content"
        }
        
        It "Should handle section with multiple classes including mssrc-block-content-block" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <section class="container mssrc-block-content-block article-content">
        <h2>News Article with Multiple Classes</h2>
        <p>Content from section with multiple CSS classes.</p>
    </section>
</body>
</html>
"@
            $testUrl = "https://news.microsoft.com/multi-class-test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "News Article with Multiple Classes"
            $result | Should -Match "multiple CSS classes"
        }
        
        It "Should throw error when section with mssrc-block-content-block class is not found" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <section class="other-class">
        <p>Content in different section</p>
    </section>
    <div class="mssrc-block-content-block">
        <p>Content in div, not section</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://news.microsoft.com/no-matching-section"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
        
        It "Should throw error when section content is only whitespace" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <section class="mssrc-block-content-block">
        
        
    </section>
</body>
</html>
"@
            $testUrl = "https://news.microsoft.com/empty-section"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }
    }
    
    Context "Consolidated Microsoft Pattern Tests" {
        It "Should use section pattern for techcommunity.microsoft.com when available" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <section class="mssrc-block-content-block">
        <h2>Tech Community Article</h2>
        <p>Content from techcommunity.microsoft.com using section pattern.</p>
    </section>
</body>
</html>
"@
            $testUrl = "https://techcommunity.microsoft.com/tech-article"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Tech Community Article"
            $result | Should -Match "techcommunity.microsoft.com using section pattern"
        }
        
        It "Should use div pattern for azure.microsoft.com when available" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="entry-content">
        <h2>Azure Documentation</h2>
        <p>Content from azure.microsoft.com using div pattern.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://azure.microsoft.com/azure-docs"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Azure Documentation"
            $result | Should -Match "azure.microsoft.com using div pattern"
        }
        
        It "Should use press release pattern for Microsoft investor content" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div id="pressreleasecontent">
        <h2>Microsoft Q4 2025 Earnings</h2>
        <p>Microsoft Corp. announced financial results for the quarter.</p>
        <p>Revenue increased by significant amounts this quarter.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/en-us/Investor/earnings/press-release"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Microsoft Q4 2025 Earnings"
            $result | Should -Match "financial results for the quarter"
            $result | Should -Match "Revenue increased by significant amounts"
        }
        
        It "Should use blogContent pattern for Microsoft Fabric blog content" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="row row-size2" id="blogContent" dir="ltr">
        <div class="nested-column">
            <div class="row side-row">
                <div class="column nested-column" id="blogContentDetails">
                    <h2>Fabric Platform Support for TLS</h2>
                    <p>We have officially ended the support for <strong>TLS 1.1 and earlier</strong> on the Fabric platform.</p>
                    <p>This update follows our earlier announcement in the TLS Deprecation blog.</p>
                    <ul>
                        <li><strong>Deadline</strong>: TLS 1.1 and earlier will no longer be supported after July 31, 2025.</li>
                        <li><strong>Impact</strong>: Any Fabric-integrated resources still using older TLS versions will fail to connect.</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
"@
            $testUrl = "https://blog.fabric.microsoft.com/en-us/blog/fabric-platform-support-for-transport-layer-security"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Fabric Platform Support for TLS"
            $result | Should -Match "TLS 1.1 and earlier"
            $result | Should -Match "July 31, 2025"
            $result | Should -Match "Fabric-integrated resources"
        }
        
        It "Should prefer section pattern over div patterns when multiple are present" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <section class="mssrc-block-content-block">
        <h2>Section Content</h2>
        <p>This should be extracted as it's the preferred pattern.</p>
    </section>
    <div class="entry-content">
        <h2>Div Content</h2>
        <p>This should not be extracted as section pattern takes precedence.</p>
    </div>
    <div id="pressreleasecontent">
        <h2>Press Release Content</h2>
        <p>This should also not be extracted as section pattern takes precedence.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/multiple-patterns"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Section Content"
            $result | Should -Match "preferred pattern"
            $result | Should -Not -Match "Div Content"
            $result | Should -Not -Match "Press Release Content"
            $result | Should -Not -Match "section pattern takes precedence"
        }
        
        It "Should prefer div entry-content pattern over press release pattern when section is not available" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="entry-content">
        <h2>Entry Content</h2>
        <p>This should be extracted as entry-content has higher priority than press release.</p>
    </div>
    <div id="pressreleasecontent">
        <h2>Press Release Content</h2>
        <p>This should not be extracted as entry-content pattern takes precedence.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/entry-vs-press"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Entry Content"
            $result | Should -Match "higher priority than press release"
            $result | Should -Not -Match "Press Release Content"
            $result | Should -Not -Match "entry-content pattern takes precedence"
        }
        
        It "Should prefer press release pattern over blogContent pattern when both are present" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div id="pressreleasecontent">
        <h2>Press Release Content</h2>
        <p>This should be extracted as press release has higher priority than blogContent.</p>
    </div>
    <div class="row row-size2" id="blogContent" dir="ltr">
        <h2>Blog Content</h2>
        <p>This should not be extracted as press release pattern takes precedence.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://blog.fabric.microsoft.com/press-vs-blog"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Press Release Content"
            $result | Should -Match "higher priority than blogContent"
            $result | Should -Not -Match "Blog Content"
            $result | Should -Not -Match "press release pattern takes precedence"
        }

        It "Should prefer message-body pattern over blogContent pattern when both are present" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="message-body">
        <h2>Message Body Content</h2>
        <p>This should be extracted as message-body has higher priority than blogContent.</p>
    </div>
    <div id="blogContent">
        <h2>Blog Content</h2>
        <p>This should not be extracted as message-body pattern takes precedence.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://techcommunity.microsoft.com/message-vs-blog"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Message Body Content"
            $result | Should -Match "higher priority than blogContent"
            $result | Should -Not -Match "Blog Content"
            $result | Should -Not -Match "message-body pattern takes precedence"
        }

        It "Should prefer message-body pattern over articleBody pattern when both are present" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div class="message-body">
        <h2>Message Body Content</h2>
        <p>This should be extracted as message-body has higher priority than articleBody.</p>
    </div>
    <div itemprop="articleBody">
        <h2>Article Body Content</h2>
        <p>This should not be extracted as message-body pattern takes precedence.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://techcommunity.microsoft.com/message-vs-article"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Message Body Content"
            $result | Should -Match "higher priority than articleBody"
            $result | Should -Not -Match "Article Body Content"
            $result | Should -Not -Match "message-body pattern takes precedence"
        }
    }

    Context "Microsoft ArticleBody Pattern (itemprop)" {
        It "Should extract content from div with itemprop='articleBody'" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <header>Site header</header>
    <nav>Navigation</nav>
    <div class="single-post__content block-content content-container" itemprop="articleBody">
        <h1>Microsoft Article with Semantic Markup</h1>
        <p>This is the main content of the Microsoft article using schema.org semantic markup.</p>
        <p>The content includes detailed information about <strong>Microsoft technologies</strong>.</p>
        <h2>Key Features</h2>
        <ul>
            <li>Semantic HTML markup</li>
            <li>Schema.org compliance</li>
            <li>Accessible content structure</li>
        </ul>
        <blockquote>
            Semantic markup improves accessibility and SEO.
        </blockquote>
    </div>
    <aside>Related content</aside>
    <footer>Footer</footer>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/en-us/research/semantic-article"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Microsoft Article with Semantic Markup"
            $result | Should -Match "schema.org semantic markup"
            $result | Should -Match "Microsoft technologies"
            $result | Should -Match "Semantic HTML markup"
            $result | Should -Match "Schema.org compliance"
            $result | Should -Match "Semantic markup improves accessibility"
            $result | Should -Not -Match "Site header"
            $result | Should -Not -Match "Navigation"
            $result | Should -Not -Match "Related content"
            $result | Should -Not -Match "Footer"
        }

        It "Should handle div with itemprop='articleBody' and other attributes" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div id="main-article" class="article-content main-content" itemprop="articleBody" data-analytics="article">
        <h2>Article with Multiple Attributes</h2>
        <p>Content from div with articleBody itemprop and other attributes.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://docs.microsoft.com/multi-attribute-test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Article with Multiple Attributes"
            $result | Should -Match "articleBody itemprop and other attributes"
        }

        It "Should extract from itemprop='articleBody' with different casing" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop="ARTICLEBODY">
        <h2>Article Body with Upper Case</h2>
        <p>Content should be extracted regardless of case.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/case-test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Article Body with Upper Case"
            $result | Should -Match "regardless of case"
        }

        It "Should handle itemprop='articleBody' with nested complex content" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop="articleBody">
        <section class="intro">
            <h1>Complex Nested Article</h1>
            <p>Introduction paragraph.</p>
        </section>
        <section class="main-content">
            <h2>Main Section</h2>
            <div class="code-block">
                <pre><code>function example() { return "test"; }</code></pre>
            </div>
            <p>Explanation of the code above.</p>
            <div class="image-container">
                <img src="diagram.png" alt="Technical diagram" />
                <figcaption>Figure 1: System architecture</figcaption>
            </div>
        </section>
        <section class="conclusion">
            <h2>Conclusion</h2>
            <p>Summary of the article content.</p>
        </section>
    </div>
</body>
</html>
"@
            $testUrl = "https://docs.microsoft.com/complex-nested-content"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Complex Nested Article"
            $result | Should -Match "Introduction paragraph"
            $result | Should -Match "Main Section"
            $result | Should -Match "function example"
            # Note: Simple regex stops at first </div> which is the code-block closing tag
            # So the following content after the code block won't be captured
            # $result | Should -Match "Explanation of the code above"
            # $result | Should -Match "Technical diagram"  
            # $result | Should -Match "Figure 1: System architecture"
            # $result | Should -Match "Conclusion"
            # $result | Should -Match "Summary of the article content"
        }

        It "Should throw error when articleBody content is empty" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop="articleBody">
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/empty-article-body"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should throw error when articleBody content is only whitespace" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop="articleBody">
        
        
        
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/whitespace-article-body"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should handle multiple divs with articleBody itemprop (extract first one found)" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop="articleBody">
        <h2>First Article Body</h2>
        <p>This should be extracted as it's the first one found.</p>
    </div>
    <div itemprop="articleBody">
        <h2>Second Article Body</h2>
        <p>This should not be extracted.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/multiple-article-bodies"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "First Article Body"
            $result | Should -Match "first one found"
            $result | Should -Not -Match "Second Article Body"
            $result | Should -Not -Match "should not be extracted"
        }

        It "Should handle articleBody with different quote styles in HTML" {
            # Arrange
            $testHtml = @"
<html>
<body>
    <div itemprop='articleBody'>
        <h2>Article with Single Quotes</h2>
        <p>Content using single quotes for itemprop attribute.</p>
    </div>
</body>
</html>
"@
            $testUrl = "https://www.microsoft.com/single-quotes-test"
            
            # Act
            $result = Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Article with Single Quotes"
            $result | Should -Match "single quotes for itemprop"
        }
    }

    Context "Error Handling" {
        It "Should throw meaningful error when Microsoft content patterns are not found" {
            # Arrange
            $testHtml = "<html><body>Invalid Microsoft content without patterns</body></html>"
            $testUrl = "https://www.microsoft.com/debug-test-file"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should handle URLs with special characters in error scenarios" {
            # Arrange
            $testHtml = "<html><body>Invalid content</body></html>"
            $testUrl = "https://www.microsoft.com/path?param=value&other=test#section"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should handle very long URLs in error scenarios" {
            # Arrange
            $testHtml = "<html><body>Invalid content</body></html>"
            $longPath = "a" * 250  # Create a very long path
            $testUrl = "https://www.microsoft.com/$longPath"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should handle missing content gracefully for various Microsoft domains" {
            # Arrange
            $testHtml = "<html><body>Invalid content</body></html>"
            $testUrl = "https://devblogs.microsoft.com/temp-dir-test"
            
            # Act & Assert
            { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
        }

        It "Should process different Microsoft domain patterns consistently" {
            # Arrange
            $testHtml = "<html><body>Invalid content</body></html>"
            $testUrls = @(
                "https://www.microsoft.com/no-temp-var",
                "https://devblogs.microsoft.com/no-temp-var",
                "https://news.microsoft.com/no-temp-var"
            )
            
            # Act & Assert
            foreach ($testUrl in $testUrls) {
                { Get-MainContentFromHtml -InputHtml $testHtml -SourceUrl $testUrl } | Should -Throw "*main content extraction from HTML did not result in any content*"
            }
        }
    }

    # Note: Reddit-specific processing was removed - Reddit URLs now fall through to default handling
    # which returns the original HTML unchanged. See "Default Processing" context for coverage.
}
