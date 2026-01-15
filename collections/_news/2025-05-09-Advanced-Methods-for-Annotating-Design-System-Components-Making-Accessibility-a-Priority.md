---
layout: post
title: 'Advanced Methods for Annotating Design System Components: Making Accessibility a Priority'
author: Jan Maarten
canonical_url: https://github.blog/engineering/user-experience/design-system-annotations-part-2-advanced-methods-of-annotating-components/
viewing_mode: external
feed_name: GitHub Engineering Blog
feed_url: https://github.blog/engineering/feed/
date: 2025-05-09 16:56:57 +00:00
permalink: /devops/news/Advanced-Methods-for-Annotating-Design-System-Components-Making-Accessibility-a-Priority
tags:
- Accessibility
- Annotations
- ARIA
- Code Connect
- Component Properties
- Components
- Design
- Design System Automation
- Design Systems
- Developer Experience
- DevOps
- Engineering
- Figma
- GitHub
- News
- Primer
- React
- UI Components
- User Experience
- Visual Regression Testing
section_names:
- devops
---
Jan Maarten discusses advanced methods for annotating design system components, offering insights from GitHub's experience with Primer, custom annotation strategies, and using Figma’s Code Connect to strengthen accessibility before development.<!--excerpt_end-->

## Introduction

In part one of the design system annotation series, we explored how accessibility considerations can be overlooked in design system components as they move from design to implementation. The solution was to use a set of “Preset annotations” for each component within GitHub’s Primer design system, allowing designers to embed accessibility information not inherently expressed in the component’s visual appearance or code.

Not all organizations use Primer, so Jan Maarten details how to construct your own preset annotations regardless of your system, emphasizing accessibility and documentation at every step.

---

## Building Preset Annotations for Your Design System

**1. Assess Components:**

- Audit your component library, prioritizing those needing annotations—start with high-value, widely used, or accessibility-issue-prone components.

**2. Determine Properties:**

- Identify essential but non-visual/non-coded properties, such as ARIA attributes or behavior requirements.

> _Example: ActionBar and Autocomplete components in Primer need specific documentation for their accessibility traits._

---

### Prioritizing Components

To manage extensive component sets, GitHub used a tool called Primer Query to:

- Track component implementation frequency
- Cross-reference against accessibility audit issues

**Main prioritization criteria:**

- Alignment with organizational priorities (high-value or high-traffic products)
- History of accessibility audit findings
- React-based implementations
- General usage frequency

---

### Mapping Out Properties

Comprehensive property mapping involves referencing several sources:

- **Component Documentation:** Usage and accessibility guidance for both code and Figma design assets
- **Coded Demos (Storybook):** Implementation and code-specific accessibility attributes
- **Figma Asset Libraries:** Available text layers, image fills, and variant controls
- **Team Expertise and Audit Findings:** Leverage institutional memory and previous accessibility audit recommendations

_Note: Avoid duplicating info already present in either the design asset or code._

---

## Lessons from Creating Preset Annotations

- Preset annotations are especially useful in inexperienced or rapidly evolving teams.
- Annotation maintenance is essential for mature systems; otherwise, mismatches can cause confusion.
- The process can be time-consuming, particularly when property names need to be concise within Figma’s property panels.
- Sometimes it’s unclear whether to use design system–specific or general annotations (e.g., Primer `<TextArea>` preset vs. generic `<textarea>` annotation), but either is typically better than none.
- Combining system-specific and general annotation types (e.g., adding toggles for system-specific properties) can reduce confusion.

---

## Unlocking Automation Potential

Automating annotation (e.g., with Figma plugins) is complex:

- Off-the-shelf tools often generate many false positives because they’re design-system agnostic.
- Accurate automation requires mapping hidden component properties that aren’t visually obvious or reflected directly in code.
- Systematically building your own preset annotations sets the stage for future automation by establishing the necessary component property knowledge base.

---

## Leveraging Figma’s Code Connect

**Code Connect** enhances the integration of design and development:

- Allows key accessibility details (like `aria-label` values) to be included as hidden properties in Figma components.
- When code is exported, these properties are embedded directly (e.g., a hidden Figma layer for an IconButton component’s `aria-label` flows into the exported React implementation).
- Reduces time spent managing manual annotations and streamlines the process for both designers and developers.
- Ensures visual cleanliness (hidden layers) while embedding essential accessibility properties.

**Tips for Success:**

- Maintain consistency in hidden property practices
- Use development branches for experimentation
- Incorporate visual regression testing to prevent regressions when adding complexity

---

## Broader Accessibility and Documentation Practices

The article concludes that proactive, well-documented annotations set teams up for success both in current design/development and future automation or innovation efforts. Planned releases such as the GitHub Annotation Toolkit, and further reading from experts like Eric Bailey, are offered as next steps for teams looking to enhance their own annotation workflows.

---

### Further Reading

- [Eric Bailey’s exploration of annotation kits and structural accessibility issues](https://ericwbailey.website/published/accessibility-annotation-kits-only-annotate/?li)

## References

- [Primer Design System](https://primer.style)
- [Figma’s Code Connect](https://www.figma.com/blog/unlocking-the-power-of-code-connect/)
- [GitHub Blog: Series Part 1](https://github.blog/engineering/user-experience/design-system-annotations-part-1-how-accessibility-gets-left-out-of-components/)

---

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/user-experience/design-system-annotations-part-2-advanced-methods-of-annotating-components/)
