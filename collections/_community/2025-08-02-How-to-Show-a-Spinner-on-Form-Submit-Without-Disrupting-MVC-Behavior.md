---
external_url: https://www.reddit.com/r/dotnet/comments/1mfpq2m/how_do_i_show_a_spinner_btn_on_form_submit/
title: How to Show a Spinner on Form Submit Without Disrupting MVC Behavior
author: Fragrant_Ride_29
feed_name: Reddit DotNet
date: 2025-08-02 12:47:54 +00:00
tags:
- .NET
- AJAX
- ASP.NET MVC
- Client Side Validation
- Double Requests
- Form Submission
- JavaScript
- MVC Forms
- Spinner Button
- UI Feedback
- UX
section_names:
- coding
---
Fragrant_Ride_29 discusses the challenges of displaying a spinner inside a submit button in ASP.NET MVC forms without causing double server requests or breaking the framework's default submission behavior.<!--excerpt_end-->

Fragrant_Ride_29 is looking for an effective way to show a loading spinner within a submit button in an ASP.NET MVC application. The primary concern is to provide real-time feedback to users by displaying a spinner while the form is submitting—without interfering with the default form submission workflow or inadvertently triggering duplicate server actions.

**Key challenges highlighted in the post:**

- Using an explicit AJAX call to manually handle spinner timing results in two server requests—one from the form submission and one from the AJAX request.
- Not using `e.preventDefault()` is crucial, as it would disrupt the standard ASP.NET MVC post-back mechanism, including validation and model binding.
- Double server requests can cause serious issues such as duplicate emails, PDFs, or records.
- Client-side tricks to show the spinner on unload do not always work as expected, especially when client-side validation blocks submission.

**Context:**

- The author wants a solution that works within conventional MVC forms, providing user feedback via a spinner without breaking built-in behaviors.

**Common approaches and pitfalls:**

- Handling the spinner in JavaScript without disrupting form submission can be tricky, as browser and framework-level behaviors sometimes interfere or require special handling for events like validation and page reloads.
- Ensuring only a single submission event is processed is critical, especially in applications with sensitive or transactional server-side actions.

**Takeaway:**

- The problem addresses a practical but nuanced UI/UX challenge common in .NET web development, emphasizing the need for solutions that respect both front-end and back-end behaviors.

Developers facing similar frustrations may need to look for a JavaScript approach that toggles the spinner state immediately before submission, possibly using event hooks that only fire when the form passes validation and just before the submit action completes. Care must be taken to avoid duplicate requests and maintain expected MVC form submission flow.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mfpq2m/how_do_i_show_a_spinner_btn_on_form_submit/)
