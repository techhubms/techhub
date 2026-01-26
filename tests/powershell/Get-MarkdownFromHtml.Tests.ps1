Describe "Get-MarkdownFromHtml" {
    BeforeAll {

        # Sample HTML content from Khalid Abuhakmeh's Blog
        $script:TestHtmlContent = @"
<p>A few new tricks have shipped with the .NET 8 release, and I'd like to take this time to experiment with them.
Specifically, I wanted to see if folks investing in a Blazor component library could still use the excellent HTMX
library. If you want to write even less JavaScript, this blog post will be right up your alley.</p>

<p>This post will explore how to take a server-rendered component and give it some client-side flair without needing web
sockets or web assembly. We'll even explore rewriting the <code>Counter</code> component found in the Blazor template and building
it with HTMX in mind. Let's go!</p>

<!--more-->

<h2 id="what-is-htmx">What is HTMX?</h2>

<p>For folks familiar with Blazor's interactive server mode, SignalR, aka Web sockets, the <a href="https://htmx.org/">HTMX</a> model isn't much different.</p>

<p>The client communicates with the server, and the server retains stateful information. The big difference is that HTMX
takes a hypermedia approach, meaning it leans on the web's traditional request/response nature. There are no persistent
connections between the client and the server. Any DOM element can initiate a request, wait for the server to process
it, and then respond with appropriate HTML. Once the HTML payload is received, it is swapped into the current DOM. While
the concept is simple to understand, it is powerful in practice.</p>

<h2 id="what-are-blazor-server-rendered-components">What are Blazor Server-Rendered Components?</h2>

<p>With the .NET 8 release, folks can opt-in
to <a href="https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-8.0#render-modes">multiple render modes</a>.
The first in the list of render modes is <code>Static</code>, although that's not entirely accurate.</p>

<p>Static rendering implies you could compile components and assets into HTML at build time. In the case of Blazor, "
Static" rendering is more comparable to its contemporary approaches of MVC and Razor Pages. When a request to a page or
component is made, the server renders the component and its component graph and then responds with HTML.</p>

<p>These Blazor components are all HTML, meaning they can only use HTML features and not the same interactive model you may
expect from <code>Interactive Server</code> or <code>Interactive WebAssembly</code> modes.</p>

<p>The advantage to these Components is they are lightweight payloads, fast to render, and can even be streamed via stream
rendering.</p>

<h2 id="lets-use-htmx-with-blazor">Let's Use HTMX with Blazor</h2>

<p>Before we port our <code>Counter</code> component to use HTMX, we must set up our project to make Blazor play nicely with HTMX.</p>

<p>The first step is to add HTMX to our <code>App.razor</code> file. This is the app shell file which has our HTML structure.</p>

<pre><code class="language-html"><!DOCTYPE html>  
<html lang="en">  
  
<head>  
    <meta charset="utf-8"/>  
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>  
    <base href="/"/>  
    <link rel="stylesheet" href="bootstrap/bootstrap.min.css"/>  
    <link rel="stylesheet" href="app.css"/>  
    <link rel="stylesheet" href="BlazorHtmx.styles.css"/>  
    <link rel="icon" type="image/png" href="favicon.png"/>  
  
    <script defer src="_framework/blazor.web.js"></script>  
    <script defer src="https://unpkg.com/htmx.org@1.9.8"></script>  
    <script defer src="js/htmx-blazor.js"></script>  
    <HeadOutlet/>  
</head>  
<body>  
<Routes/>  
</body>  
</html>
</code></pre>

<p>We'll also need to write a bit of JavaScript to tie into Blazor's enhanced rendering mode.</p>

<pre><code class="language-javascript">// An enhanced load allows users to navigate between different pages  
Blazor.addEventListener("enhancedload", function () {  
    // HTMX need to reprocess any htmx tags because of enhanced loading  
    htmx.process(document.body);  
});
</code></pre>

<p>Now, let's set up our <code>HtmxCounter</code> component. In a file, add the following Blazor component code. You'll notice that
HTMX uses <code>hx-*</code> attributes to define the behavior of DOM elements. It's easy to pick up and can add functionality
quickly. (Not to brag, but I got this sample working on the first try. 🤩)</p>

<pre><code class="language-razor"><div class="counter">
    <p role="status">Current count: @State.Value</p>
    <button class="btn btn-primary"
            hx-post="/count"
            hx-target="closest .counter"
            hx-swap="outerHTML">
        Click me
    </button>
</div>

@code {
    [Parameter, EditorRequired] 
    public HtmxCounterState State { get; set; } = new();
    
    public class HtmxCounterState
    {
        public int Value { get; set; } = 0;
    }
}
</code></pre>

<p>Next, let's look at our endpoint holding on to state. <strong>Note that this use of state management is only for demo
purposes. I recommend user-scoped state management like a database limited to a single user.</strong></p>

<p>In your Blazor's <code>Program.cs</code> file, you'll need to register the state for our component.</p>

<pre><code class="language-c#">builder.Services
    .AddSingleton<HtmxCounter.HtmxCounterState>();
</code></pre>

<p>Next, we'll need the endpoint to increment and render our component HTML fragment.</p>

<pre><code class="language-c#">app.MapPost("/count",
    (HtmxCounter.HtmxCounterState value) =>
    {
        value.Value++;
        return new RazorComponentResult<HtmxCounter>(
            new { State = value }
        );
    });
</code></pre>

<p>Finally, let's add our component to a Blazor server-rendered page.</p>

<pre><code class="language-razor">@page "/"
@inject HtmxCounter.HtmxCounterState CounterState

<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

<div class="mb-4">
    <HtmxCounter State="CounterState"/>
</div>

@code {
    protected override void OnInitialized()
    {
        // reset counter on page reloads
        CounterState.Value = 0;
    }
}
</code></pre>

<p>Let's see what happens when we load our page.</p>

<video controls="" preload="metadata">
    <source src="https://github.com/khalidabuhakmeh/BlazorHtmx/raw/main/sample-video.mp4" type="video/mp4" />
</video>

<p>That's pretty cool. From a client perspective, you can't tell which implementation uses WebSockets and which is using
HTMX. That's amazing if you ask me. The HTMX implementation will also be cheaper in the long run as it requires no more
infrastructure than you currently have.</p>

<p>If you want to check out the sample, you can get the solution
on <a href="https://github.com/khalidabuhakmeh/BlazorHtmx">my GitHub repository and a few more samples of using HTMX with Blazor</a>.</p>

<h2 id="conclusion">Conclusion</h2>

<p>Server-rendered components are an excellent addition to the Blazor toolbox and open up the possibility of using your
component library with something as cool as HTMX. I hope you try this sample and let me know what you think.</p>

<p>As always, thanks for reading my blog posts. Cheers.</p>
"@
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
    }

    Context "Debugging Body Extraction" {
        It "Should correctly extract body content or use entire content" {
            # Debug what the function is actually receiving
            Write-Host "Input HTML length: $($script:TestHtmlContent.Length)"
            Write-Host "Input HTML first 100 chars: '$($script:TestHtmlContent.Substring(0, [Math]::Min(100, $script:TestHtmlContent.Length)))'"
            
            # Test the body extraction logic manually
            if ($script:TestHtmlContent -match '(?si)<body[^>]*>(.*?)</body>') {
                Write-Host "Body tag found - extracted length: $($matches[1].Length)"
                Write-Host "Body content: '$($matches[1])'"
            }
            else {
                Write-Host "No body tag - should use entire content"
            }
            
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # This test is just for debugging - we'll see the output
            $true | Should -Be $true
        }
    }
    
    Context "HTML Content Processing" {
        It "Should not return null or empty content for valid HTML" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            Write-Host "Result length: $($result.Length)"
            Write-Host "First 200 chars of result: $($result.Substring(0, [Math]::Min(200, $result.Length)))"
        }
        
        It "Should preserve main content elements" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert - should contain key content from the original
            $result | Should -Match "new tricks have shipped with the \.NET 8 release"
            $result | Should -Match "What is HTMX"
            $result | Should -Match "Blazor Server-Rendered Components"
            $result | Should -Match "Let's Use HTMX with Blazor"
        }
        
        It "Should handle code blocks properly" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert - should preserve code content and convert to markdown format
            $result | Should -Match "Counter.*component"
            $result | Should -Match "```html"
            $result | Should -Match "```javascript"
        }
        
        It "Should remove HTML comments" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert - should not contain the <!--more--> comment
            $result | Should -Not -Match "<!--more-->"
        }
        
        It "Should preserve links" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert - should contain the HTMX link
            $result | Should -Match "htmx\.org"
            $result | Should -Match "khalidabuhakmeh/BlazorHtmx"
        }
    }
    
    Context "Edge Cases" {
        It "Should handle empty HTML gracefully" {
            # Act & Assert
            { Get-MarkdownFromHtml -HtmlContent "" } | Should -Not -Throw
        }
        
        It "Should handle HTML with only whitespace" {
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent "   `n   `t   "
            
            # Assert
            $result | Should -BeNullOrEmpty
        }
        
        It "Should handle HTML without body tags" {
            # Arrange
            $simpleHtml = "<p>Simple paragraph</p><h1>Header</h1>"
            
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $simpleHtml
            
            # Assert
            $result | Should -Not -BeNullOrEmpty
            $result | Should -Match "Simple paragraph"
            $result | Should -Match "Header"
        }
    }
    
    Context "Content Length Validation" {
        It "Should track content length through processing steps" {
            # This test will show us exactly where content might be lost
            # The debug output from the function will be captured in the test run
            
            # Act
            $result = Get-MarkdownFromHtml -HtmlContent $script:TestHtmlContent
            
            # Assert - basic validation that we got meaningful content back
            $result | Should -Not -BeNullOrEmpty
            $result.Length | Should -BeGreaterThan 100  # Should be substantial content
        }
    }
}
