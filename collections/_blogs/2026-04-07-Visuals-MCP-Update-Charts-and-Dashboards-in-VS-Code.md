---
author: Harald Binkle
title: 'Visuals MCP Update: Charts and Dashboards in VS Code'
section_names:
- ai
- github-copilot
tags:
- AI
- AI Agents
- Bar Chart
- Blogs
- Chart Rendering
- CSV Export
- Dashboards
- Data Visualization
- Dual Axis
- GitHub Copilot
- Interactive Charts
- Legends
- Line Chart
- MCP
- MCP Server
- Pie Chart
- Scatter Chart
- Storybook
- SVG Export
- Tooltips
- Visuals MCP
- VS Code
- VS Code Copilot Chat
feed_name: Harald Binkle's blog
primary_section: github-copilot
date: 2026-04-07 11:25:35 +00:00
external_url: https://harrybin.de/posts/visuals-mcp-charts-dashboard-update/
---

Harald Binkle explains the latest Visuals MCP update, adding a chart tool that lets AI agents render single charts and full dashboards directly inside GitHub Copilot Chat in VS Code, with Storybook examples and export options for turning analysis into shareable visuals.<!--excerpt_end-->

# Visuals MCP Update: Charts and Dashboards in VS Code

Posted on: April 7, 2026 (12:55 PM)

If you read the earlier post about Visuals MCP, you saw how an MCP server can render real UI instead of only text responses. Since then, the project has moved forward in a big way.

The most important update is **chart support**: you can now ask your AI agent to render **single charts** or **complete dashboards** on the fly **inside the Copilot chat in VS Code**.

## What Changed Since the First Post

Visuals MCP originally supported:

- Table
- List
- Tree
- Image
- Master-detail experiences

The current version adds:

- A **dedicated chart tool**
- A broader **Storybook surface** for validating payloads and layouts

### Supported chart families

1. Line
2. Bar
3. Area
4. Pie
5. Scatter
6. Composed

### Dashboard layouts

Dashboard-style layouts can render multiple charts together in:

- Vertical mode
- Horizontal mode
- Grid mode

## Why This Matters

When agents return plain text summaries, trend analysis is harder than it should be. A chart view gives instant visual context for:

- Changes over time
- Outliers
- Correlations

For everyday workflows, this enables you to:

1. Compare metrics over time without rewriting markdown tables
2. Inspect multiple dimensions in one panel
3. Export chart data and visuals for reports
4. Keep the interaction inside your editor

## Single Charts and Dashboard Views

The chart tool is designed for both:

- **Focused analysis** (one clear question, e.g., monthly growth or category distribution)
- **Overview screens** (system-level view with multiple chart types side by side)

![Visuals MCP chart dashboard grid screenshot](/assets/visuals-mcp/chart-grid.webp)

### Validate layouts in Storybook

In Storybook, you can validate both patterns before wiring prompts into your MCP workflow:

- Storybook: https://harrybin.de/visuals-mcp
- Single chart example: `mcp-visuals-chart--line-chart`
- Multi chart grid example: `mcp-visuals-chart--multiple-charts-grid`

## Practical Capabilities You Can Use Right Away

The chart implementation includes:

1. Multiple chart layouts in one render
2. Optional legends, tooltips, and grid lines
3. Dual axis configurations
4. Export options for JSON, CSV, JPG, and SVG
5. Theme-aware rendering that matches VS Code

The intent is to make the output useful beyond demos, so you can move from exploration to documentation without leaving the editor.

## Prompt Pattern for On-the-Fly Dashboards

A simple pattern is to request one dashboard with a fixed set of cards:

```text
Build a dashboard with 4 charts in a grid:
1) line chart for weekly active users
2) bar chart for feature usage
3) pie chart for traffic source share
4) composed chart for revenue and cost
Include legends and tooltips and exportable JSON.
```

This keeps intent explicit and gives the model clear structure for chart type and placement.

## Conclusion

If you already use MCP tools in your daily flow, this update shifts analysis from text-heavy output to visual interaction. Use the chart features to get visual insights from any data your AI agent can access, compare multiple metrics in one view, and export results for sharing or reporting.


[Read the entire article](https://harrybin.de/posts/visuals-mcp-charts-dashboard-update/)

