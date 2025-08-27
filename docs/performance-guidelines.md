# Performance Guidelines

## Performance Optimization Overview

### Core Performance Principles

#### Performance-First Architecture

- **Server-side rendering before JavaScript enhancement**: All content must load and be functional before JavaScript executes
- **Client-side performance is paramount after initial render**: Optimize for post-load interactions and responsiveness
- **Follow content limiting rules for performance**: Limit initial content loads to maintain fast page loading (see [Filtering System](filtering-system.md))

### Server-Side Performance Optimizations

#### Content Limiting Strategy

- **Server-side Content Limiting**: Implemented via filtering system to reduce processing load (see [Filtering System](filtering-system.md))
- **Limited Dataset Processing**: Reduces server processing load during Jekyll builds
- **Pre-calculated Counts**: Tag counts computed during build, not runtime
- **Efficient Data Structures**: Use JSON lookup tables for fast data access

#### Jekyll Build Optimizations

- **Cache expensive calculations**: Store computed values to avoid repeated processing

### Client-Side Performance Optimizations

For comprehensive JavaScript performance guidelines, see [JavaScript Guidelines](javascript-guidelines.md).

#### General Client-Side Principles

- **Progressive Enhancement**: JavaScript enhances server-rendered content
- **Graceful Degradation**: Ensure functionality without JavaScript
- **Monitor memory usage**: Check for memory leaks in long-running sessions

### Performance Testing Requirements

#### Mobile Performance Testing

- **Mobile Performance**: Test loading speeds on mobile networks
- **Network conditions**: Verify performance across various connection speeds
- **Touch optimization**: Ensure touch interactions are responsive

#### Browser Performance

- **Browser Testing**: Test across different browsers and versions for consistent performance
- **JavaScript execution monitoring**: Track client-side performance impact
- **Performance profiling**: Use browser dev tools to identify bottlenecks

### Image and Asset Optimization

#### Image Performance

- **Lazy Loading**: Load content as needed to improve initial page performance
- **Format Optimization**: Use modern image formats when supported
- **Responsive Images**: Implement proper srcset and sizes attributes

#### Asset Loading

- **Progressive Enhancement**: Load core functionality first, enhance progressively
- **Asset bundling**: Optimize file loading for reduced HTTP requests

### Performance Troubleshooting

#### Common Performance Issues

**Symptom**: Slow filtering or page loading

**Common Causes**:

- Too much client-side processing
- Inefficient DOM queries  
- Large datasets without optimization

**Solutions**:

1. Move processing to server-side (Liquid)
2. Optimize JavaScript queries
3. Implement proper caching
4. Use content limiting rules correctly (see [Filtering System](filtering-system.md))

#### Performance Monitoring

- **Monitor client-side performance**: Keep an eye on JavaScript execution times
- **Track loading metrics**: Measure page load times across different devices
- **Identify bottlenecks**: Use performance profiling tools to find slow operations

### Critical Performance Requirements

- **Prioritize client-side performance**: After initial server-side render, all interactions must be responsive
- **Cache expensive calculations**: Store computed values to avoid repeated processing
- **Optimize for mobile**: Ensure performance standards are met on mobile devices
- **Maintain performance during updates**: Ensure code changes don't impact site performance

## Performance Priorities

**Core Principles:**

1. **Server-side first**: All content must be rendered server-side for initial page load (exception: assets/js/sections.js only)
2. **Client-side performance is paramount** after initial render
3. **Never sacrifice functionality, content amount, or visual elements**
4. **Server-side processing can be slower** if it benefits the client
5. **Optimize for user experience over build time**
6. **JavaScript enhances, never creates initial content**

**Requirements:**

- Content must never be rendered or altered by JavaScript on initial page load.
- All visible content must be fully rendered server-side by Jekyll/Liquid
- Users must see complete, functional content immediately upon page load
- JavaScript should only enhance interactivity, never create initial content
- This ensures optimal SEO, performance, and accessibility

**The ONLY Exception:**

- `assets/js/sections.js` - Allowed to modify section collections state on page load
- This file handles section collections activation based on URL parameters
- All other JavaScript must wait for user interaction

## Code Quality Standards

### Liquid Template Processing

**Critical Understanding**: Liquid code (between `{%` or `{%-` tags) is executed server-side during the Jekyll build process only. This means:

- Liquid code runs once when the website is built
- It does NOT run in the browser
- Server-side code can be slower if it improves client performance
- Focus on client-side performance without sacrificing functionality

### JavaScript Guidelines

#### Server-Side First Principle

**Rule**: JavaScript must never render initial page content (exception: assets/js/sections.js only)

**Correct Approach:**

1. Jekyll/Liquid renders all initial content server-side
2. JavaScript adds event listeners and interactivity
3. JavaScript responds to user interactions
4. Content is immediately visible and functional

**Examples:**

```javascript
// ✅ CORRECT: Enhance existing server-rendered content
document.querySelectorAll('.tag-filter-btn').forEach(btn => {
  btn.addEventListener('click', handleFilterClick);
});

// ❌ WRONG: Create content on page load
window.addEventListener('load', () => {
  createFilterButtons(); // This should be done server-side!
});
```

**The assets/js/sections.js Exception:**

- Only `assets/js/sections.js` may modify content on initial load
- It handles section collections state based on URL parameters
- This is necessary for proper navigation highlighting
- All other JavaScript must wait for user interaction

For comprehensive JavaScript performance guidelines, see [JavaScript Guidelines](javascript-guidelines.md).

### Formatting Requirements

- Add proper indentation wherever possible
- Never place conditions and actions on the same line
- **Bad**:

  ```liquid
  {% if true %} then dosomething {% else %} bla {% endif %}
  ```

- **Good**:

  ```liquid
  {%- if condition -%}
    {%- assign result = value -%}
  {%- else -%}
    {%- assign result = alternative -%}
  {%- endif -%}
  ```

### Data Passing Conventions

When data is explicitly passed to included files:

- Access it using the `include.` prefix
- Example: If passing `tags` and `posts`, reference them as `include.tags` and `include.posts`

### Content Types

Articles are written in Markdown but can contain:

- Liquid template code
- HTML
- CSS  
- JavaScript
- Mixed content types

## Jekyll Documentation References

When working with Jekyll/Liquid, reference these official documentation sources:

- **Liquid Syntax**: [Jekyll Liquid Documentation](https://jekyllrb.com/docs/liquid/)
- **Includes**: [Jekyll Includes Documentation](https://jekyllrb.com/docs/includes/)
- **Front Matter**: [Jekyll Front Matter Documentation](https://jekyllrb.com/docs/front-matter/)
- **Variables**: [Jekyll Variables Documentation](https://jekyllrb.com/docs/variables/)
