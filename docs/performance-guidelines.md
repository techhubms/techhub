# Performance Guidelines

Performance optimization for Tech Hub follows a server-side-first architecture where all content is rendered during the Jekyll build, and JavaScript only enhances interactivity after page load.

## Core Principles

1. **Server-side first** - All content must be rendered server-side for initial page load
2. **Client-side performance is paramount** - After initial render, all interactions must be responsive
3. **Never sacrifice functionality** - Don't reduce content amount or visual elements for performance
4. **Server-side can be slower** - Build-time processing can take longer if it benefits the client
5. **JavaScript enhances, never creates** - Initial content must never depend on JavaScript

### The Server-Side First Rule

All visible content must be fully rendered server-side by Jekyll/Liquid. Users must see complete, functional content immediately upon page load. This ensures optimal SEO, performance, and accessibility.

**The ONLY exception**: `assets/js/sections.js` is allowed to modify section collections state on page load based on URL parameters. All other JavaScript must wait for user interaction.

## Server-Side Optimizations

### Jekyll Build Performance

Server-side processing happens during the Jekyll build. Liquid code (between `{%` or `{%-` tags) runs once when the website is built, not in the browser.

**Optimization strategies:**

- **Cache expensive calculations** - Store computed values to avoid repeated processing
- **Pre-calculate counts** - Tag counts are computed during build, not runtime
- **Use efficient data structures** - JSON lookup tables enable fast data access
- **Limit dataset processing** - Use the [Filtering System](filtering-system.md) to reduce processing load

### Data Passing in Templates

When data is passed to included files, access it using the `include.` prefix:

```liquid
{% include component.html tags=page.tags posts=site.posts %}
```

Reference as `include.tags` and `include.posts` within the included file.

## Client-Side Optimizations

For comprehensive JavaScript guidelines, see [assets/js/AGENTS.md](../assets/js/AGENTS.md).

### Progressive Enhancement

JavaScript enhances server-rendered content rather than creating it:

- Ensure core functionality works without JavaScript
- Monitor memory usage for leaks in long-running sessions
- Load core functionality first, then enhance progressively

### Image and Asset Performance

- **Lazy loading** - Load images as needed to improve initial page performance
- **Modern formats** - Use WebP and other modern image formats when supported
- **Responsive images** - Implement proper `srcset` and `sizes` attributes
- **Asset bundling** - Optimize file loading to reduce HTTP requests

## Testing Requirements

### Mobile Performance

- Test loading speeds on mobile networks and various connection speeds
- Ensure touch interactions are responsive
- Verify performance standards are met on mobile devices

### Browser Performance

- Test across different browsers and versions
- Track JavaScript execution times
- Use browser dev tools to profile and identify bottlenecks

## Troubleshooting

### Slow Filtering or Page Loading

**Common causes:**

- Too much client-side processing
- Inefficient DOM queries
- Large datasets without optimization

**Solutions:**

1. Move processing to server-side (Liquid)
2. Optimize JavaScript queries
3. Implement proper caching
4. Use content limiting rules correctly (see [Filtering System](filtering-system.md))

### Performance Monitoring

- Monitor JavaScript execution times
- Measure page load times across devices
- Use profiling tools to find slow operations
- Ensure code changes don't regress performance

## Related Documentation

- **JavaScript guidelines**: [assets/js/AGENTS.md](../assets/js/AGENTS.md)
- **Liquid template formatting**: [_plugins/AGENTS.md](../_plugins/AGENTS.md#formatting-requirements)
- **Filtering system**: [filtering-system.md](filtering-system.md)

### Jekyll Official Documentation

- [Liquid Syntax](https://jekyllrb.com/docs/liquid/)
- [Includes](https://jekyllrb.com/docs/includes/)
- [Front Matter](https://jekyllrb.com/docs/front-matter/)
- [Variables](https://jekyllrb.com/docs/variables/)
