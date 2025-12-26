---
layout: "post"
title: "Centering a WPF TreeViewItem in the TreeView ScrollViewer"
description: "Rick Strahl presents a guide for centering a TreeViewItem within a WPF TreeView's viewport, correcting the limitations of the standard BringIntoView method. He shares practical code samples and reusable helper functions to improve usability when re-focusing or reorganizing TreeViews in .NET WPF applications."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-07-16 04:23:03 +00:00
permalink: "/posts/2025-07-16-Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer.html"
categories: ["Coding"]
tags: [".NET", "Application Usability", "BringIntoView", "C#", "Coding", "ContentPresenter", "Helper Methods", "Posts", "ScrollViewer", "TreeView", "UI Development", "Visual Tree", "WPF"]
tags_normalized: ["dotnet", "application usability", "bringintoview", "csharp", "coding", "contentpresenter", "helper methods", "posts", "scrollviewer", "treeview", "ui development", "visual tree", "wpf"]
---

In this article, Rick Strahl addresses a common usability issue with WPF's TreeView, offering .NET code and helper methods to properly center selected items and improve user experience.<!--excerpt_end-->

# Centering a WPF TreeViewItem in the TreeView ScrollViewer

*By Rick Strahl*

![](https://weblog.west-wind.com/images/2025/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer/DigitalTreeBanner.jgp.png)

The TreeView control in WPF can be challenging due to missing conveniences that developers often expect as standard, such as easily keeping a selected item visible and centered within the viewport. By default, the TreeView's `BringIntoView()` method only pulls an item just to the edge of the viewport (top or bottom), resulting in poor visibility and suboptimal UI experiences.

## The Problem with BringIntoView

For example, when filtering and then re-unfiltering a list in a TreeView, you might want the focused item to remain clearly visible, ideally centered:[^1]

![Bad Centering Tree View](https://raw.githubusercontent.com/RickStrahl/ImageDrop/refs/heads/master/BlogPosts/2025/BadCenteringTreeView.gif)

> **Figure 1**: BringIntoView() brings the TreeViewItem into view, but only at the edges (top or bottom), making the item less prominent.

The selected item can easily be missed when it sits at the very bottom, especially if the item is deeply nested. This is particularly problematic when working with documentation or data-tree UIs after a filter is reset and focus should be drawn to a specific item.

## Standard Implementation and Its Drawbacks

The typical workaround involves trying to expand parent nodes, focus, and bring the TreeViewItem into view:

```csharp
public void MakeTopicVisible(DocTopic topic) // bound data item {
    if (topic == null) return;

    var tvi = WindowUtilities.GetNestedTreeviewItem(topic, TreeTopicBrowser);
    if (tvi == null) return;

    // expand all parents
    var tvParent = tvi;
    while (tvParent?.Parent is TreeViewItem parentTvi) {
        tvParent.IsExpanded = true;
        if (tvParent.DataContext is DocTopic dt) dt.IsExpanded = true;
        tvParent = parentTvi;
    }

    tvi.IsSelected = true;
    tvi.Focus();
    tvi.BringIntoView();
}
```

After all of this, the item may still only appear at the viewport's edge. The `BringIntoView()` method does not provide a way to center the target item.

## Custom Centering Solution

To address this, Rick Strahl introduces a helper method to center the selected item manually by adjusting the ScrollViewer that hosts the TreeView control:

```csharp
/// <summary>
/// Centers a TreeViewItem inside of the scrollviewer unlike ScrollIntoView
/// which scrolls the item just into the top or bottom if not already
/// in the view
/// </summary>
/// <param name="treeView">TreeView to scroll</param>
/// <param name="treeViewItem">TreeView Item to scroll to</param>
public static void CenterTreeViewItemInScrollViewer(TreeView treeView, TreeViewItem item) {
    if (item == null) return;

    // Ensure item is visible in layout
    item.IsSelected = true;
    item.BringIntoView();

    treeView.Dispatcher.InvokeAsync(() => {
        var scrollViewer = FindVisualChild<ScrollViewer>(treeView);
        if (scrollViewer == null) return;

        // Find the header content presenter
        var header = FindVisualChild<ContentPresenter>(item);
        if (header == null) return;

        // Get header position relative to ScrollViewer
        var transform = header.TransformToAncestor(scrollViewer);
        var position = transform.Transform(new System.Windows.Point(0, 0));

        double headerHeight = header.ActualHeight;
        double viewportHeight = scrollViewer.ViewportHeight * scrollViewer.ScrollableHeight / scrollViewer.ExtentHeight;

        double targetOffset = scrollViewer.VerticalOffset + position.Y - (viewportHeight / 2) + (headerHeight / 2);
        scrollViewer.ScrollToVerticalOffset(targetOffset);
    }, DispatcherPriority.Loaded);
}
```

This approach calculates the position of the item's content within the ScrollViewer, ensuring that only the visible content (not children or expanded subtrees) is centered. This provides a more intuitive and prominent display.

### Usage

Update your UI event code to use the new function:

```csharp
tvi.IsSelected = true;
tvi.Focus();
WindowUtilities.CenterTreeViewItemInScrollViewer(TreeTopicBrowser, tvi);
```

**Visual Result:**

![Proper Centering Tree View](https://raw.githubusercontent.com/RickStrahl/ImageDrop/refs/heads/master/BlogPosts/2025/ProperCenteringTreeView.gif)

This code produces the desired behavior: the selected item is displayed in the center of the control, improving accessibility and focus.

## Helper Methods

**FindVisualChild**
Finds the first child control of a specified type in the visual tree:

```csharp
public static T FindVisualChild<T>(DependencyObject currentControl) where T : DependencyObject {
    if (currentControl != null) {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(currentControl); i++) {
            var child = VisualTreeHelper.GetChild(currentControl, i);
            if (child is T) {
                return (T)child;
            }
            T childItem = FindVisualChild<T>(child);
            if (childItem != null) return childItem;
        }
    }
    return null;
}
```

**GetNestedTreeViewItem**
Recursively searches for a TreeViewItem associated with a model value:

```csharp
public static TreeViewItem GetNestedTreeviewItem(object item, ItemsControl parent) {
    // look at this level
    var tvi = parent.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
    if (tvi != null) return tvi;

    // otherwise, recurse into each generated TreeViewItem
    foreach (object child in parent.Items) {
        if (parent.ItemContainerGenerator.ContainerFromItem(child) is TreeViewItem childContainer) {
            var result = GetNestedTreeviewItem(item, childContainer);
            if (result != null) return result;
        }
    }
    return null;
}
```

## Additional Considerations

While centering is a common requirement, you may wish to refine this code to skip scrolling if the item is already within the viewport, or to position items at the top or bottom instead. Such enhancements are possible by further adjusting the offset logic.

## Summary

Many aspects of the WPF TreeView control do not intuitively address hierarchical UI needs out-of-the-box. Fortunately, custom helper methods like those presented here provide flexible solutions.

Keep these helpers in your toolkit, and consider refactoring common patterns like these into reusable utilities the next time you build a data-centric .NET desktop app.

---

[^1]: GIF images referenced are omitted in textual descriptions for clarity.

---
*© Rick Strahl, West Wind Technologies, 2005-2025*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer)
