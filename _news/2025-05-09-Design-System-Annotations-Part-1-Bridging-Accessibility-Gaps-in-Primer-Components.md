---
layout: "post"
title: "Design System Annotations, Part 1: Bridging Accessibility Gaps in Primer Components"
description: "This article by Jan Maarten explores the limitations of relying on design system components for accessibility. It introduces GitHub’s annotation toolkit and Primer A11y Preset annotations, which aim to close accessibility gaps in digital product development by streamlining the annotation process within Figma and enhancing component usability for all users."
author: "Jan Maarten"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/user-experience/design-system-annotations-part-1-how-accessibility-gets-left-out-of-components/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-05-09 16:56:41 +00:00
permalink: "/2025-05-09-Design-System-Annotations-Part-1-Bridging-Accessibility-Gaps-in-Primer-Components.html"
categories: ["DevOps"]
tags: ["A11y", "Accessibility", "Annotations", "ARIA", "Components", "Design", "Design Systems", "DevOps", "Engineering", "Figma", "GitHub", "News", "Primer", "User Experience", "Web Standards"]
tags_normalized: ["a11y", "accessibility", "annotations", "aria", "components", "design", "design systems", "devops", "engineering", "figma", "github", "news", "primer", "user experience", "web standards"]
---

Authored by Jan Maarten, this article discusses how GitHub’s Accessibility Design team developed a new system of annotations to address persistent accessibility challenges in design system components and streamline the collaborative product development process.<!--excerpt_end-->

# Design System Annotations, Part 1: Bridging Accessibility Gaps in Primer Components

_Article by Jan Maarten_

When organizations build and scale digital experiences, design systems are often core to delivering consistency and quality. However, accessibility isn’t always guaranteed at the component level—even with a mature design system. In response, the Accessibility Design team at GitHub has devised a framework of annotations to bridge accessibility gaps left by standard component libraries, especially within Primer.

## The Role and Need for Accessibility Annotations

Despite diligent work on accessible design systems, access barriers can persist into production. Many teams believe that mature, 'accessible' components will always yield accessible designs, yet this assumption too often proves false. Design systems drive adoption of standards but cannot account for every design or usage context. To address this, many organizations rely on accessibility annotations—visual notes incorporated in design files that clarify intent and highlight unseen requirements.

Annotations help answer vital questions, such as:

- How should assistive technologies navigate the page?
- What is the alternative text for images and unlabeled buttons?
- How does the layout adapt to viewport changes?
- Which virtual keyboard should be triggered for form inputs on mobile?
- What is the correct management of focus in complex interactions?

By clarifying these points, annotations enhance developer understanding and reduce communication gaps, ultimately preventing audit issues and reducing rework costs. Various public annotation kits exist for these purposes, helping teams document application controls, headings, decorative and informative images, and focus orders.

## GitHub’s Annotation Toolkit

To enable all designers—not just accessibility experts—to add essential context, GitHub began creating an internal Figma library: the GitHub Annotation Toolkit. Building on open-source kits from CVS Health, the toolkit is designed for clarity, ease-of-use, and direct integration with component documentation.

Improvements over previous kits include:

- Inline documentation
- Reduced annotation overhead
- Accessibility information accessible for all designers

Design system audits, annotation integration, and documentation all serve to surface technical semantics and expert knowledge, embedding accessibility deeply into the development process.

## Shortcomings: Why Accessibility Still Gets Left Out

### Accessibility is Non-Binary

While tools like the Web Content Accessibility Guidelines (WCAG) offer foundational direction, ensuring all real-world scenarios are covered is challenging. Design systems might address known issues, but unique user needs or context-specific requirements often go missed if teams don’t seek guidance from actual users—particularly those using assistive technology.

### Accessible Components ≠ Accessible Designs

Even when using accessible components, the arrangement and context may introduce hidden issues. For example:

- Inadequate heading hierarchy
- Missing context for assistive technology
- Insufficient annotation for dynamic or responsive layouts

Annotations, or a lack thereof, in Figma components can result in confusion for developers regarding component usage, required HTML/ARIA attributes, and behavioral nuances.

## Primer Example: Common Gaps in Component Annotation

For instance, a seemingly accessible Primer button in Figma may lack vital details about its function, its accessible label, or data submission requirements. Leaving these unanswered can introduce accessibility risks and confusion downstream in the development pipeline.

## Streamlining the Annotation Process: Preset Annotations

GitHub developed Primer A11y Preset annotations by selecting frequently used Primer components and creating detailed, context-aware notes alongside them. Key benefits include:

- Built-in documentation of complex elements (e.g., heading levels, semantic tags, landmarks)
- Reduced repetitive annotation for new component instances
- Pre-set prompts that guide designers to fill critical information specific to each use case

By simplifying and standardizing this process, teams better communicate both requirements and intent, significantly reducing the introduction of access barriers.

## Primer A11y Preset Annotations: Proof of Concept

Ten commonly used Primer components now come with preset annotations that link directly to documentation and Storybook demos. These annotations clarify requirements such as:

- Proper ARIA attributes
- Component usage guidelines
- Validation and error messages
- How to fill out each annotation for maximum accessibility

The toolkit aims to help designers without specialist knowledge document accessible usage inline, without leaving Figma.

## Next Steps: Building Your Own Annotation Systems

As noted, preset annotations are only meaningful in conjunction with the specific components they are designed for. For teams leveraging other design systems (Material Design, Polaris, Carbon, etc.), the approach and method can be adapted to those systems. The forthcoming Part 2 will guide readers on constructing custom preset annotations and documenting accessibility decisions before development commences.

## Additional Resources

- [Design system annotations, part 2: Advanced methods of annotating components](https://github.blog/engineering/user-experience/design-system-annotations-part-2-advanced-methods-of-annotating-components/)
- [Web Accessibility Annotation Kit](https://github.com/cvs-health/annotations)
- [Accessibility audit guidance](https://www.w3.org/WAI/test-evaluate/)
- [Primer documentation](https://primer.style/)

For a visual exploration of these concepts, watch Alexis Lucio present during GitHub’s Dev Community Event for Figma’s Config 2024.

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/user-experience/design-system-annotations-part-1-how-accessibility-gets-left-out-of-components/)
