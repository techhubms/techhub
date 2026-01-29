---
external_url: https://weblog.west-wind.com/posts/2025/Feb/26/Inline-Confirmations-in-JavaScript-UI
title: Implementing Inline Confirmation UI in JavaScript and ASP.NET Apps
author: Rick Strahl
feed_name: Rick Strahl's Blog
date: 2025-02-27 09:02:10 +00:00
tags:
- ASP.NET
- ASP.NET MVC
- Button State Management
- Client Side Scripting
- CodePen
- Dynamic UI
- Event Handling
- Front End Design
- HTML
- Inline Confirmation
- JavaScript
- Reactive Frameworks
- UI/UX Patterns
- Vue.js
- Web Development
- Coding
- Blogs
section_names:
- coding
primary_section: coding
---
Rick Strahl shares a simple and effective technique for handling confirmation prompts inline in JavaScript and ASP.NET web applications, providing code examples and UI best practices.<!--excerpt_end-->

# Implementing Inline Confirmation UI in JavaScript and ASP.NET Apps

*Author: Rick Strahl*

## Overview

Traditional confirmation dialogs or modal popups can be intrusive and disrupt user workflow in HTML and web applications. This post introduces a modern alternative: an inline UI confirmation for operations (like deletes) that can be achieved with minimal code and a few binding directives.

## Scenario

While managing image uploads in a web application (built with ASP.NET and Vue.js), Rick adopted an inline confirmation button approach for deletion instead of using JavaScript's `confirm()` dialog or modal boxes. This design enhances usability and maintains workflow continuity.

### Benefits of Inline Confirmation

- Reduces user disruption compared to modal dialogs
- Faster and more intuitive interactions
- Flexible state management for custom confirmation logic

**Demo:** [Inline Delete Button Confirmation CodePen](https://codepen.io/rstrahl/pen/yyLVweL)

### UI Behavior

- First click: Button changes to 'Confirm?' state
- User has ~5 seconds to confirm (button reverts if not confirmed)
- Confirmation click triggers the operation (e.g., delete image)

## Implementation Details

### Using Vue.js (Reactive Framework)

**Server-rendered Data:**

```html
@{ // C# Razor: client variables setup }
<script>
// Example: page = { imageList: [...], fundRaiserId: "xxxxx" }
@scriptVars.ToHtmlString();
var vueData = {};
</script>
```

**Button Markup:**

```html
<div v-for="image in images" :key="image.id" class="flex-item">
  <img :src="..." />
  <div class="action-box">
    <button
      type="button"
      v-on:click="deleteImage(image)"
      class="btn btn-sm float-end"
      v-bind:class="{ 'btn-danger': image.deleteConfirm, 'btn-secondary': !image.deleteConfirm }">
      <i class="fad fa-trash-alt me-1 fa-beat"></i>
      <span v-if="!image.deleteConfirm">Delete</span>
      <span v-if="image.deleteConfirm">Confirm?</span>
    </button>
  </div>
</div>
```

**State Management:**

- `deleteConfirm` flag toggles UI and styling
- Initially `false` (or not set), updates to `true` on first click
- Timeout restores original state

**JavaScript (Vue method):**

```js
// image instance is passed from HTML
function deleteImage(image) {
  if (!image.deleteConfirm) {
    image.deleteConfirm = true; // Prompt confirmation
    setTimeout(() => (image.deleteConfirm = false), 5000); // Revert in 5s
    return;
  }
  // ... perform deletion
}
```

### Pure JS / No Framework Approach

For apps not using frameworks, the pattern works with two buttons and some event handling:

```html
<button id="btnSubmitDelete" class="btn btn-secondary mt-3" type="button">
  <i class="fa fa-xmark-circle"></i> Delete Fundraiser
</button>

<button name="btnSubmitDelete" id="btnSubmitDeleteConfirm" class="btn btn-danger mt-3 hidden" type="submit">
  <i class="fad fa-trash-circle fa-beat"></i> Are you sure?
</button>

<script>
const el = document.getElementById("btnSubmitDelete");
const el2 = document.getElementById("btnSubmitDeleteConfirm");
el.addEventListener("click", () => {
  el.classList.add("hidden");
  el2.classList.remove("hidden");
  setTimeout(() => {
    el2.classList.add("hidden");
    el.classList.remove("hidden");
  }, 5000);
});
</script>
```

**Key Points:**

- UI swaps between delete and confirm buttons
- Timeout automatically restores the initial state

### Design Tips

- Modify the confirmation UI for longer messages or custom styling
- Easily adapts for other frameworks or vanilla JavaScript
- Consolidate to a single button for a cleaner look, or use two for accessibility

## Summary

Inline confirmation UIs streamline user experience during critical actions such as deletion. Rick Strahl’s approach demonstrates minimal coding requirements, flexibility for frameworks or plain JavaScript, and improved workflow for ASP.NET web apps. Future usage can include more advanced widgets and animation cues for confirmation timeouts.

---

**Related Resources:**

- [CodePen Demo](https://codepen.io/rstrahl/pen/yyLVweL)
- [Minimal OWIN Identity Authentication in ASP.NET MVC](https://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application)
- [Map Physical Paths with HttpContext.MapPath in ASP.NET](https://weblog.west-wind.com/posts/2023/Aug/15/Map-Physical-Paths-with-an-HttpContextMapPath-Extension-Method-in-ASPNET)

---

**Author:** Rick Strahl ([website](https://west-wind.com))

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Feb/26/Inline-Confirmations-in-JavaScript-UI)
