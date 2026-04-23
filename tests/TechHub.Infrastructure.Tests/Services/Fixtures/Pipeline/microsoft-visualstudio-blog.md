April 1st, 2026

![heart](https://devblogs.microsoft.com/visualstudio/wp-content/themes/devblogs-evo/images/emojis/heart.svg)![like](https://devblogs.microsoft.com/visualstudio/wp-content/themes/devblogs-evo/images/emojis/like.svg)5 reactions

 

![Mads Kristensen](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2021/09/Mads-glasses-square-96x96.jpg)

Principal Product Manager

 

Bookmarks in Visual Studio have always been a simple, reliable feature. Many developers use them regularly, and over the years we’ve heard consistent feedback from those users. Bookmarks were useful, but there were a few core gaps that kept them from being as effective and relevant as they could be.

Navigation was one of the biggest pain points. You could move between bookmarks, but there was no easy way to jump directly to a specific bookmark using the keyboard. That made bookmarks harder to rely on once you had more than a few. Another common request was sharing. Bookmarks worked well for personal, local navigation, but there was no good way to share them with teammates or reuse them across repos, branches, or pull requests.

That feedback is what led to [Bookmark Studio](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BookmarkStudio), a new experimental Visual Studio extension that builds on the existing bookmark experience by filling in those missing pieces, without changing how bookmarks fundamentally work.

## Faster, more intentional navigation

One of the core additions in Bookmark Studio is **slot‑based navigation**.

Bookmarks can be assigned to slots 1 through 9 and jumped to directly using simple keyboard shortcuts like Alt+Shift+1 through Alt+Shift+9. This makes bookmarks feel more deliberate and easier to rely on when you want fast access to a handful of important locations.

![Toolbar](https://madskristensen.gallerycdn.vsassets.io/extensions/madskristensen/bookmarkstudio/1.0.58/1774373928867/art/toolbar.png)

New bookmarks are automatically assigned the next available slot when possible, so fast navigation often works without any extra setup. Bookmark Studio also integrates with Visual Studio’s existing bookmark commands, which means your current shortcuts and muscle memory continue to work as expected.

## A single place to work with bookmarks

Bookmark Studio also adds a dedicated **Bookmark Manager** tool window.

![Tool window](https://madskristensen.gallerycdn.vsassets.io/extensions/madskristensen/bookmarkstudio/1.0.58/1774373928867/art/bookmark-manager.png)

The manager shows all bookmarks in one place and makes it easy to browse, search, and navigate between them. You can filter by name, file, location, color, or slot, and jump directly to a bookmark with a double‑click or keyboard navigation. It’s designed to make bookmarks easier to revisit, especially when switching context or coming back to code later.

## Optional structure, when you need it

Another piece of feedback we heard was the need for just a bit more organization.

With Bookmark Studio, bookmarks can have labels, colors, and folders. None of this is required, and you can keep using bookmarks exactly as you do today. But when you’re debugging, refactoring, reviewing code, or exploring unfamiliar areas of a codebase, that extra context can make bookmarks more useful and easier to reason about.

![Glyphs](https://madskristensen.gallerycdn.vsassets.io/extensions/madskristensen/bookmarkstudio/1.0.58/1774373928867/art/set-color.png)

All bookmark metadata is stored per solution, so it stays with your work across sessions.

## Bookmarks you can share and reuse

Bookmarks are often most valuable when they capture intent, not just location.

Bookmark Studio makes it easy to export bookmarks as plain text, Markdown, or CSV. That means you can include bookmarks in pull requests, share investigation paths with teammates, or move useful bookmark sets between repos. Instead of being a purely personal tool, bookmarks can become a lightweight way to communicate context and decisions.

## Bookmarks that stay put as code changes

Bookmark Studio tracks bookmarks as text moves during editing, so they stay attached to the relevant code instead of drifting to the wrong line. This makes bookmarks more dependable during active development, especially when files are changing frequently.

![Drag and drop of glyph](https://madskristensen.gallerycdn.vsassets.io/extensions/madskristensen/bookmarkstudio/1.0.58/1774373928867/art/glyph-drag-drop.gif)

## A focused improvement, not a reinvention

Bookmark Studio doesn’t try to replace tasks, TODO comments, or issue tracking. It doesn’t introduce a new workflow you have to learn. Instead, it fills in the gaps that many bookmark users have pointed out over time, making bookmarks easier to navigate, easier to share, and more useful as part of everyday development.

If you already use bookmarks in Visual Studio, Bookmark Studio should feel familiar within minutes. And if you’ve ever wished bookmarks could do just a little more, this extension is worth a look.

You can download **Bookmark Studio** today from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BookmarkStudio). As always, feedback and pull requests are welcome on the [GitHub repo](https://github.com/madskristensen/BookmarkStudio).

 
 

Category

Topics

## Author

![Mads Kristensen](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2021/09/Mads-glasses-square-96x96.jpg)

Principal Product Manager

Mads Kristensen is a Principal Product Manager at Microsoft, working to enhance productivity and usability in Visual Studio. He’s behind popular extensions like Web Essentials and File Nesting and is active in the open-source community. A frequent speaker, Mads is dedicated to making Visual Studio the most enjoyable IDE for developers.