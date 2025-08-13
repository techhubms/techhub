
<img src="./images/Promptception.jpg" width="900">

# AI Promptception - Iterating GitHub Copilot Prompts for Maximum Impact

* By Randy Pagels

---

GitHub Copilot is an impressive AI coding assistant, but it’s only as good as the prompts you give it. The difference between a **useful output** and a **frustrating mess** often comes down to how well you structure your request.

This article explores **how to improve prompt engineering for GitHub Copilot** using **real-world examples**, focusing on Playwright test automation.

---

## User Story: The Frustration of Incomplete Tests

<div style="display: flex; align-items: flex-start; margin-bottom: 20px;">
  <img src="./images/The Frustration of Incomplete Tests.jpg" width="300" style="margin-right: 15px;">
  <div>
  </div>
</div>

Imagine you’re working on a critical project with tight deadlines. You’ve just finished building a React component, `PlaneList.tsx`, and now it’s time to write tests using Playwright. Instead of manually writing each test, you decide to save time and use **GitHub Copilot**.

You type a simple prompt:

> _"Create the remaining tests for `#file:PlaneList.tsx` based on `#file:PlaneList.spec.tsx`."_

The results? **Incomplete, generic tests** that miss key scenarios like **API failures, edge cases, and user interactions**. Frustrated, you realize that the problem isn’t GitHub Copilot—it’s the prompt.

This is where **Promptception** begins: using GitHub Copilot to **write better prompts for GitHub Copilot**, turning frustration into a powerful feedback loop for maximum impact.

### User Story on a Kanban Board

Example

**Title:** _Write Playwright Tests for `PlaneList.tsx`_

- _**As a** developer **I want to** generate comprehensive Playwright tests for `PlaneList.tsx` using GitHub Copilot **so that** I can ensure robust coverage for UI rendering, user interactions, and API edge cases._

**Acceptance Criteria:**
- ✅ Tests cover all major UI components and their states
- ✅ Includes edge cases like empty lists, large datasets, and API failures
- ✅ Follows Playwright best practices with proper assertions and API mocking
- ✅ Accessible tests with basic a11y checks

---

## The Initial Prompt

I needed to generate Playwright tests for a React component. I started with a straightforward request:

> _"Create the remaining tests for `#file:PlaneList.tsx` based on `#file:PlaneList.spec.tsx`."_

At first, this seemed reasonable, but the results were **incomplete and generic**. GitHub Copilot lacked the right **context and direction**, leading to tests that:

- ❌ Didn’t fully align with Playwright’s structure
- ❌ Missed key testing scenarios
- ❌ Contained redundant or irrelevant assertions

This prompted a closer look at what was missing.

---

## Prompt Review

After analyzing the output, I found **three key weaknesses** in my original prompt:

- ✅ **No mention of Playwright** – GitHub Copilot supports multiple frameworks. Without specifying Playwright, it wasn’t clear what syntax to use.
- ✅ **Unclear test coverage requirements** – The generated tests lacked **important cases** like **empty lists, filtering, and API failures**.
- ✅ **No mention of best practices** – Playwright tests involve **event handling, API mocking, and accessibility checks**. Without guidance, Copilot didn’t generate them.

At this point, I needed to **refine my prompt** to guide Copilot toward a better solution.

---

## How I Used GitHub Copilot to Improve My Prompt
<div style="display: flex; align-items: flex-start; margin-bottom: 20px;">
  <img src="./images/Improve My Prompt.jpg" width="300" style="margin-right: 15px;">
  <div>
  </div>
</div>

Sometimes, the best way to **write a better prompt** is to use **GitHub Copilot itself**. Since Copilot is trained on a vast amount of **coding patterns and best practices**, it can **help structure a well-formed prompt** that follows clear engineering principles.

### Step 1: Started with a Simple Inquiry

I first asked Copilot:

> _"What are the key elements of a good AI prompt for generating Playwright tests?"_

Copilot responded with an outline of best practices, including:

- **Be specific about the testing framework.**
- **Clearly define expected test scenarios.**
- **Use structured bullet points for clarity.**

This confirmed that my original prompt was **too vague** and lacked the **details necessary for an accurate output**.

### Step 2: Asked for a Better Prompt Structure

To get more specific guidance, I refined my request:

> _"How can I improve this prompt: 'Create the remaining tests for PlaneList.tsx based on PlaneList.spec.tsx'?"_

Copilot’s response **suggested adding:**

- ✅ The **test framework** (Playwright)
- ✅ The **type of tests** (end-to-end, component tests, UI interactions)
- ✅ A **list of required test cases**

### Step 3: Iterating with Refinements

Now that I had a clearer structure, I asked Copilot to **expand on missing elements**:

- ✅ _"Make sure it includes accessibility and performance checks."_
- ✅ _"Reword it to be clearer but still concise."_

**Result:** I ended up with a **precise, structured, and effective prompt** that GitHub Copilot could **easily understand and execute**.

---

## Prompt Refinement

Using Copilot’s feedback, I restructured my prompt:

> _"Create the remaining end-to-end (E2E) and component tests for `#file:PlaneList.tsx` using Playwright in `#file:PlaneList.spec.tsx`. Ensure broad test coverage, including:_
> - **UI Rendering**: Verify layout and default states
> - **User Interactions**: Test clicks, filtering, and list updates
> - **Edge Cases**: Handle empty lists, large datasets, and API failures
> - **Performance & Accessibility**: Ensure fast rendering and basic a11y checks
> - **Mocking APIs & Network Requests**: Simulate responses for dynamic content"

This small adjustment had a **huge impact**. GitHub Copilot **generated Playwright tests** that:

- ✅ Correctly used **Playwright’s test methods**
- ✅ Included **real-world scenarios**
- ✅ Followed **best practices**

---

## Breaking Down the Anatomy of a Good Prompt
<div style="display: flex; align-items: flex-start; margin-bottom: 20px;">
  <img src="./images/Anatomy of a Good Prompt.jpg" width="300" style="margin-right: 15px;">
  <div>
  </div>
</div>

A great prompt follows a **clear structure**:

- 🔹 **Clarity** – State exactly what you need. Avoid vague or open-ended requests.
- 🔹 **Specificity** – Mention the **framework** (Playwright), the **file** (`PlaneList.tsx`), and the **test type** (E2E, component tests).
- 🔹 **Intent** – Clearly define the **goal** (e.g., ensure broad test coverage, mock APIs, etc.).
- 🔹 **Structure** – Use bullet points or numbered lists to break down different test scenarios.

---

## Iterating for Perfection

Even with a refined prompt, GitHub Copilot might need **small adjustments**. Instead of rewriting everything, **follow-up prompts** help fine-tune the output.

Example:

- ✅ _"Refactor tests to use `test.use()` for shared setup."_
- ✅ _"Optimize API mocking to improve test speed."_
- ✅ _"Add accessibility assertions for buttons and inputs."_

These **micro-adjustments** allow **GitHub Copilot to polish the output** without requiring a complete rewrite.

---

## Avoiding Common Pitfalls

Even experienced developers make mistakes when prompting. Here are some **pitfalls to avoid**:

- 🚫 **Too vague:** _"Write tests for this file."_
  - ✅ **Better:** _"Write Playwright E2E tests for `#file:PlaneList.tsx`, covering filtering, UI rendering, and API handling."_

- 🚫 **Too broad:** _"Generate all tests needed."_
  - ✅ **Better:** _"Generate tests for missing scenarios, including empty lists, large datasets, and API failures."_

- 🚫 **Too rigid:** _"Write a test exactly like this one."_
  - ✅ **Better:** _"Write a test that follows the same structure but covers user interactions."_

---

## Why Prompt Engineering Matters for Developers

GitHub Copilot is more than just an autocomplete tool. Used correctly, it **accelerates development, improves test coverage, and reduces manual effort**.

However, it’s not magic—**it needs guidance**. Prompt engineering is the skill that separates a **good developer** from a **great one** when using AI-assisted coding.

---

## Final Thoughts & Best Practices

Here’s a **quick reference guide** for writing better prompts:

- ✅ **Be specific** – Mention the **framework, file, and test type**.
- ✅ **Use lists** – Break down test cases instead of lumping them together.
- ✅ **Refine iteratively** – Use follow-up prompts to fine-tune the output.
- ✅ **Avoid vague requests** – Clearly state what’s missing or needs improvement.

With these techniques, you can **turn GitHub Copilot into a precision tool** for generating **cleaner, more reliable code**.

Try GitHub Copilot for refining your prompts today and see the difference!

