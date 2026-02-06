---
external_url: https://weblog.west-wind.com/posts/2025/Mar/14/Making-Html-Input-Controls-Truly-ReadOnly
title: 'Making HTML Input Controls Truly ReadOnly: Best Practices and Workarounds'
author: Rick Strahl
feed_name: Rick Strahl's Blog
date: 2025-03-14 20:26:04 +00:00
tags:
- Accessibility
- Bootstrap
- CSS
- Disabled Attribute
- Form Submission
- Frontend
- HTML
- Input Controls
- Pointer Events
- Razor
- Readonly Attribute
- Tabindex
- User Interface
- Vue.js
- Web Development
- Blogs
- .NET
section_names:
- dotnet
primary_section: dotnet
---
Rick Strahl offers practical solutions for improving HTML readonly input controls, providing tips on CSS, tabindex, and UI design trade-offs for web developers.<!--excerpt_end-->

# Making HTML Input Controls Truly ReadOnly: Best Practices and Workarounds

*By Rick Strahl*

HTML provides both `readonly` and `disabled` attributes for input elements, each with distinct behaviors that impact user experience and form handling. In this article, Rick Strahl discusses the issues developers encounter when trying to create truly readonly fields and offers pragmatic advice for making readonly controls behave closer to what users expect.

## Why 'readonly' May Not Be Enough

- `readonly` inputs display minimal style differences by default but can still receive focus and be selectable via mouse or keyboard.
- Users may mistake a readonly control for something editable due to the focus rectangle and enabled UI.

**Example:**

```html
<input type="text" readonly class="image-quantity" />
```

![ReadOnly Text Box](https://weblog.west-wind.com/images/2025/Making-Html-Input-Controls-Truly-ReadOnly/TextBoxFocusStillShowsFocus.jpg)

Frameworks like Bootstrap add some styling, but they don't change the fundamental UX: readonly controls still act like editable fields (except you can't modify values).

## disabled vs. readonly  

- **disabled**: Input cannot be accessed/focused and is not submitted with the form.
- **readonly**: Input can't be modified but can be focused; its value is submitted with the form.

**Reference:** [MDN Documentation](https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/readonly#attribute_interactions)

> In most practical cases, disabled is preferable unless you need to post the value.
> Only use readonly if server submission is required.

## Fixing ReadOnly UI with CSS and tabindex

If you must use `readonly`, CSS and tabindex help you mimic disabled control UX:

```css
.image-quantity[readonly] {
  border: none;
  opacity: 0.7;
  pointer-events: none;
}
```

- Visually reduces emphasis (opacity)
- Disables mouse UI activation (pointer-events)

Prevent keyboard tab focus:

```html
<input type="text" readonly tabindex="-1" class="image-quantity" />
```

- `tabindex="-1"` removes the control from tab order

For generic use:

```css
input[readonly], textarea[readonly] {
  opacity: 0.7;
  pointer-events: none;
}
```

**Note:** `readonly` only applies to text inputs and textareas.

## Client-Side Frameworks and Razor Usage

The article demonstrates integrating these patterns in Vue.js and Razor TagHelpers:

**Vue.js Example:**

```html
<input v-model="image.quantity" v-on:change="updateQuantity(image)"
  v-bind:readonly="isReadOnly"
  v-bind:tabindex="isReadOnly ? -1 : auto"
  class="number image-quantity" />
```

**Razor TagHelper Example:**

```html
<input type="number"
  asp-for="Fundraiser.EstimatedPlates"
  readonly="@(fundraiser.Status != FundraiserStatus.None)" />
```

## Accessibility Considerations

Community comments highlight accessibility questions:

- Screen readers may treat readonly and disabled differently.
- For WCAG compliance, ensure the HTML matches semantic intent and test UI for all users (keyboard, screen reader, mouse, etc).

## Summary & Recommendations

- Prefer `disabled` unless you require form submission.
- When using `readonly`, enhance UI with CSS and tabindex for a better experience.
- Always consider accessibility when changing native control behavior.

## Resources

- [readonly Html Attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/readonly)
- [disabled Html Attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled)
- [pointer-events style](https://developer.mozilla.org/en-US/docs/Web/CSS/pointer-events)
- [tabindex attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/tabindex)

---

*Discussion and examples adapted from Rick Strahl's original article, community comments included for additional context.*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Mar/14/Making-Html-Input-Controls-Truly-ReadOnly)
