---
external_url: https://weblog.west-wind.com/posts/2024/Nov/08/Getting-the-Current-TabItem-when-the-Tab-is-not-selected-in-WPF
title: Getting the Current TabItem When the Tab Is Not Selected in WPF
author: Rick Strahl
viewing_mode: external
feed_name: Rick Strahl's Blog
date: 2024-11-09 09:10:44 +00:00
tags:
- C#
- Context Menu
- Event Handling
- FindAncestor
- Hover Detection
- MahApps.Metro
- MetroTabItem
- SelectedItem
- TabControl
- TabItem
- Tooltip
- UI Automation
- UI Development
- Visual Tree
- WPF
section_names:
- coding
---
In this article, Rick Strahl shares insights into handling hover detection in WPF TabControls, demonstrating how to access non-selected TabItems for context-sensitive operations.<!--excerpt_end-->

# Getting the Current TabItem When the Tab Is Not Selected in WPF

**Author:** Rick Strahl

Hovering over a tab in a WPF TabControl doesn’t make it “selected”, and WPF’s data binding and event APIs mainly work with the selected state. This presents a challenge when you wish to display a context-sensitive UI element – like a context menu or tooltip – for the item under the mouse rather than the currently selected item.

## Problem: Selected vs. Hovered TabItem

WPF’s TabControl (and similar list-based controls), easily provides information about the selected item through properties such as `SelectedItem`, `SelectedValue`, and `SelectedIndex`. However, these properties are not helpful if you want to respond to the tab the user is simply hovering over with their cursor.

**Common Scenarios Requiring Hover Detection:**

- Displaying a context menu for the hovered tab, regardless of selection
- Showing tooltips for tabs the mouse hovers over

Most of the time you want context-aware features for the hovered item, not just the selected one. This scenario often arises in applications with sidebar panels, icon-only tabs, or advanced UI interactions, such as in Rick’s application [Markdown Monster](https://markdownmonster.west-wind.com/), where tabs can trigger context menus even when not active.

![Markdown Monster Sidebar Tab Context Menu](https://raw.githubusercontent.com/RickStrahl/ImageDrop/refs/heads/master/MarkdownMonster/LeftSidebarSplitPanels.gif)

*Figure 1: Sidebar tabs display context menus for hovered (not just selected) tabs in Markdown Monster.*

## Attempt 1: Relying Only on SelectedItem

A naïve event handler for opening a context menu might look like this:

```csharp
private void LeftSidebar_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
    var src = sender as TabControl;
    var tab = src?.SelectedItem;
    if (tab == null) return;
    // Show context menu, but only for the selected tab
}
```

**Limitation:** This works if you only care about the selected tab, but fails when you want context for the hovered (but not selected) tab. For example, if you right-click on a different tab, the context menu would still show options for the selected tab.

## Solution: Using OriginalSource and Traversing the Visual Tree

To accurately respond to the hovered tab, you need to:

1. Use `e.OriginalSource` from the event to find the originating UI element.
2. Walk up (traverse) the parent hierarchy from that source until you reach the relevant `TabItem` (or your custom `MetroTabItem` if using MahApps.Metro).

### Example: Event Handler to Detect the Hovered TabItem

```csharp
private async void LeftSidebar_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
    var src = e.OriginalSource as FrameworkElement;
    if (src == null) return;

    var tab = WindowUtilities.FindAncestor<MetroTabItem>(src);
    if (tab == null) return;

    // Now you have the hovered tab item (not just the selected one)
    var context = new LeftSidebarContextMenu(tab);
    await context.OpenContextMenu();
    e.Handled = true;
}
```

This pattern works by:

- Grabbing the control the context menu was invoked on.
- Walking up the control’s parent chain to match the type you’re looking for.

### Utility Function: FindAncestor<T>()

A reusable helper, such as `FindAncestor<T>`, traverses the visual tree:

```csharp
/// <summary>
/// Finds a type of element in the parent chain of an element
/// </summary>
public static T FindAncestor<T>(DependencyObject currentControl) where T : DependencyObject {
    do {
        if (currentControl is T)
            return (T) currentControl;
        currentControl = VisualTreeHelper.GetParent(currentControl);
    } while (currentControl != null);
    return null;
}
```

This method returns the first ancestor in the parent chain matching the specified type.

**Use Case:**

- Can be applied to any ItemsControl, such as TreeView or ListView, not just TabControl.

## Community Improvements and Ideas

### Enumerable Traversal with LINQ (Stephen Cleary)

Rather than a one-off search, you can define an enumerable that produces the current control and all its ancestors:

```csharp
public static IEnumerable<DependencyObject> SelfAndAncestors(this DependencyObject currentControl) {
    while (currentControl != null) {
        yield return currentControl;
        currentControl = VisualTreeHelper.GetParent(currentControl);
    }
}

// Usage:
var tab = src.SelfAndAncestors().OfType<MetroTabItem>().FirstOrDefault();
```

**Benefits:** Leads to more expressive and flexible code using LINQ.

### Challenge: Multiple TabControls and Focus

A reader, Felix Strauss, points out a scenario with multiple TabControls where the selected tab may not reflect the ‘active’ tab—defined as the tab containing the control with focus. This opens up further possibilities for leveraging visual/logical tree traversal to detect active states.

## Summary

The main technique is:

- Start with the event’s `OriginalSource`
- Traverse up the visual tree for the parent TabItem
- Respond with context-sensitive UI (menus, tooltips, etc.) for the appropriate tab

This approach is a handy reminder for WPF developers who want their UIs to be responsive not just to selection, but to the user’s context—as indicated by the mouse or focus location.

---

*Content adapted from Rick Strahl’s blog post, with additional insights from the community and practical code references.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2024/Nov/08/Getting-the-Current-TabItem-when-the-Tab-is-not-selected-in-WPF)
