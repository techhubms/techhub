Describe "Get-BalancedHtmlContent" {
    BeforeAll {
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }
    
    Context "Parameter Validation" {
        It "Should handle null InputHtml gracefully" {
            $result = Get-BalancedHtmlContent -InputHtml $null -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle null TagName gracefully" {
            $result = Get-BalancedHtmlContent -InputHtml "<div>test</div>" -TagName $null -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle null TagPattern gracefully" {
            $result = Get-BalancedHtmlContent -InputHtml "<div>test</div>" -TagName "div" -TagPattern $null
            $result | Should -Be ""
        }
        
        It "Should handle empty InputHtml" {
            $result = Get-BalancedHtmlContent -InputHtml "" -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle empty TagName" {
            $result = Get-BalancedHtmlContent -InputHtml "<div>test</div>" -TagName "" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle empty TagPattern" {
            $result = Get-BalancedHtmlContent -InputHtml "<div>test</div>" -TagName "div" -TagPattern ""
            $result | Should -Be ""
        }
    }
    
    Context "Basic Tag Extraction" {
        It "Should extract simple div content" {
            $html = '<div class="test">Simple content</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be "Simple content"
        }
        
        It "Should extract article content" {
            $html = '<article id="post-123">Article content here</article>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "article" -TagPattern '<article[^>]*\s+id="post-[^"]*"[^>]*>'
            $result | Should -Be "Article content here"
        }
        
        It "Should extract section content" {
            $html = '<section class="content-block">Section content</section>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "section" -TagPattern '<section[^>]*\s+class="[^"]*content[^"]*"[^>]*>'
            $result | Should -Be "Section content"
        }
        
        It "Should handle multiline content" {
            $html = @"
<div class="test">
    <p>First paragraph</p>
    <p>Second paragraph</p>
</div>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Match "First paragraph"
            $result | Should -Match "Second paragraph"
        }
    }
    
    Context "Nested Tag Handling" {
        It "Should handle nested divs correctly" {
            $html = @"
<div class="outer">
    <p>Before nested</p>
    <div class="inner">
        <p>Nested content</p>
    </div>
    <p>After nested</p>
</div>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+class="outer"[^>]*>'
            $result | Should -Match "Before nested"
            $result | Should -Match "Nested content"
            $result | Should -Match "After nested"
        }
        
        It "Should handle deeply nested structures" {
            $html = @"
<div class="level1">
    <p>Level 1 content</p>
    <div class="level2">
        <p>Level 2 content</p>
        <div class="level3">
            <p>Level 3 content</p>
        </div>
        <p>More level 2</p>
    </div>
    <p>Back to level 1</p>
</div>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+class="level1"[^>]*>'
            $result | Should -Match "Level 1 content"
            $result | Should -Match "Level 2 content"
            $result | Should -Match "Level 3 content"
            $result | Should -Match "More level 2"
            $result | Should -Match "Back to level 1"
        }
        
        It "Should handle multiple nested tags of same type" {
            $html = @"
<article class="main">
    <p>Main article</p>
    <article class="quote">
        <p>Quoted article</p>
    </article>
    <article class="reference">
        <p>Reference article</p>
    </article>
    <p>Main article continues</p>
</article>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "article" -TagPattern '<article[^>]*\s+class="main"[^>]*>'
            $result | Should -Match "Main article"
            $result | Should -Match "Quoted article"
            $result | Should -Match "Reference article"
            $result | Should -Match "Main article continues"
        }
    }
    
    Context "Case Sensitivity" {
        It "Should handle uppercase tag names" {
            $html = '<DIV class="test">Content</DIV>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be "Content"
        }
        
        It "Should handle mixed case tag names" {
            $html = '<Div class="test">Content</Div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be "Content"
        }
        
        It "Should handle uppercase opening and lowercase closing" {
            $html = '<DIV class="test">Content</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be "Content"
        }
        
        It "Should handle nested tags with mixed case" {
            $html = @"
<DIV class="outer">
    <p>Before</p>
    <div class="inner">Nested</div>
    <p>After</p>
</DIV>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Match "Before"
            $result | Should -Match "Nested"
            $result | Should -Match "After"
        }
    }
    
    Context "Microsoft ArticleBody Specific Tests" {
        It "Should extract articleBody content with itemprop" {
            $html = '<div class="single-post__content block-content content-container" itemprop="articleBody">Article body content</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+itemprop=["'']articleBody["''][^>]*>'
            $result | Should -Be "Article body content"
        }
        
        It "Should handle articleBody with nested divs" {
            $html = @"
<div class="content" itemprop="articleBody">
    <p>Introduction text</p>
    <div class="nested-content">
        <p>This is inside a nested div</p>
    </div>
    <p>More content after the nested div</p>
</div>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+itemprop=["'']articleBody["''][^>]*>'
            $result | Should -Match "Introduction text"
            $result | Should -Match "This is inside a nested div"
            $result | Should -Match "More content after the nested div"
        }
        
        It "Should handle articleBody with single quotes" {
            $html = "<div class='content' itemprop='articleBody'>Content with single quotes</div>"
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+itemprop=["'']articleBody["''][^>]*>'
            $result | Should -Be "Content with single quotes"
        }
        
        It "Should handle articleBody with double quotes" {
            $html = '<div class="content" itemprop="articleBody">Content with double quotes</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+itemprop=["'']articleBody["''][^>]*>'
            $result | Should -Be "Content with double quotes"
        }
    }
    
    Context "Edge Cases and Error Handling" {
        It "Should return empty string when opening tag not found" {
            $html = '<div class="test">Content</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+id="notfound"[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle malformed HTML gracefully" {
            $html = '<div class="test">Content without closing tag'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle self-closing tags in content" {
            $html = '<div class="test">Content with <img src="test.jpg" /> self-closing tag</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Match "Content with"
            $result | Should -Match "self-closing tag"
        }
        
        It "Should handle tags that look like search terms but aren't" {
            $html = '<div class="test">This text contains the word divider but not div tags</div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be "This text contains the word divider but not div tags"
        }
        
        It "Should handle empty tag content" {
            $html = '<div class="test"></div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle whitespace-only content" {
            $html = '<div class="test">   
            
            </div>'
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Be ""
        }
        
        It "Should handle multiple matching tags and extract first one" {
            $html = @"
<div class="test">First div content</div>
<div class="test">Second div content</div>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+class="test"[^>]*>'
            $result | Should -Be "First div content"
        }
    }
    
    Context "Performance and Large Content" {
        It "Should handle large content efficiently" {
            $largeContent = "Large content " * 1000
            $html = "<div class='test'>$largeContent</div>"
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*>'
            $result | Should -Match "Large content"
            $result.Length | Should -BeGreaterThan 10000
        }
        
        It "Should handle many nested levels" {
            # Create a simpler nested structure for testing
            $html = @"
<div class="outer">
    <div class="level1">Level 1 content 
        <div class="level2">Level 2 content 
            <div class="level3">Level 3 content</div>
        </div>
    </div>
    <div class="another">Level 20 content</div>
</div>
"@
            
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "div" -TagPattern '<div[^>]*\s+class="outer"[^>]*>'
            $result | Should -Match "Level 1 content"
            $result | Should -Match "Level 20 content"
        }
    }
    
    Context "Real-world Microsoft Patterns" {
        It "Should extract devblogs.microsoft.com article content" {
            $html = @"
<article class="post" id="post-12345" data-permalink="/article">
    <header>Article Header</header>
    <div class="content">
        <p>This is the main article content</p>
        <div class="code-block">
            <pre>Code example</pre>
        </div>
        <p>More article content</p>
    </div>
</article>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "article" -TagPattern '<article[^>]*\s+id="post-[^"]*"[^>]*>'
            $result | Should -Match "Article Header"
            $result | Should -Match "main article content"
            $result | Should -Match "Code example"
            $result | Should -Match "More article content"
        }
        
        It "Should extract news.microsoft.com section content" {
            $html = @"
<section class="mssrc-block-content-block main-content">
    <h1>News Article Title</h1>
    <p>News article content here</p>
    <section class="quote-section">
        <blockquote>Nested quote section</blockquote>
    </section>
    <p>More news content</p>
</section>
"@
            $result = Get-BalancedHtmlContent -InputHtml $html -TagName "section" -TagPattern '<section[^>]*\s+class="[^"]*\bmssrc-block-content-block\b[^"]*"[^>]*>'
            $result | Should -Match "News Article Title"
            $result | Should -Match "News article content"
            $result | Should -Match "Nested quote section"
            $result | Should -Match "More news content"
        }
    }
}
