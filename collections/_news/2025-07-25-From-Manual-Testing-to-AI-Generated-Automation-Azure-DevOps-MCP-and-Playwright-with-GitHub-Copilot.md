---
external_url: https://devblogs.microsoft.com/devops/from-manual-testing-to-ai-generated-automation-our-azure-devops-mcp-playwright-success-story/
title: 'From Manual Testing to AI-Generated Automation: Azure DevOps MCP & Playwright with GitHub Copilot'
author: Igor Najdenovski
feed_name: Microsoft DevBlog
date: 2025-07-25 06:47:36 +00:00
tags:
- AI Generated Code
- Automated Testing
- Azure DevOps
- CI/CD
- End To End Testing
- MCP Server
- Playwright
- Prompt Engineering
- Software Quality
- Test
- Test Automation
- Test Pipelines
- Test Scripts
- TypeScript
- AI
- Azure
- Coding
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- azure
- coding
- devops
- github-copilot
primary_section: github-copilot
---
Igor Najdenovski shares insights on transforming manual test suites into automated, AI-generated scripts using Azure DevOps MCP server, GitHub Copilot, and Playwright. This post covers practical steps, challenges, and recommendations based on real team experience.<!--excerpt_end-->

## From Manual Testing to AI-Generated Automation: Azure DevOps MCP & Playwright Success Story

**By Igor Najdenovski**

In today’s fast-paced software development cycles, manual testing can quickly become a significant bottleneck. Our team faced a growing backlog of test cases that demanded repetitive, manual execution—re-running entire test suites every sprint. This approach consumed valuable time better spent on exploratory testing and higher-value engineering tasks.

### Why We Needed Change

Manual regression testing was unsustainable as our codebase and test suite grew. Recognizing that freeing up tester time and ensuring broader, more reliable coverage was essential, we sought to automate our testing pipeline.

### The Solution: Integrating AI and DevOps

We leveraged Azure DevOps’ new **MCP server** integration with **GitHub Copilot** to automatically generate and execute end-to-end tests using [Playwright](https://playwright.dev/). This combination transformed our QA process:

- **Faster test creation** with AI-assisted code generation using Copilot
- **Wider test coverage** across all critical flows
- **Seamless CI/CD integration**, enabling automatic execution of hundreds of tests
- **On-demand test runs** from Azure Test Plans (full association of Playwright JS/TS tests with manual cases coming soon; see [release notes](https://learn.microsoft.com/en-us/azure/devops/release-notes/2025/testplans/sprint-258-update))

By automating our testing with Copilot and MCP, we dramatically reduced manual effort, improved reliability, and accelerated release cycles.

---

## How We Turn Test Cases Into Automated Scripts (Step-by-Step)

The transition to AI-driven automation required a coordinated workflow:

1. **Start with manual test cases in Azure DevOps.**
2. **Use MCP server and GitHub Copilot to generate Playwright tests.**
3. **Review, adjust, and bulk-convert suites using Copilot prompts.**
4. **Integrate the generated scripts with CI/CD for automated execution.**
5. **Associate tests with Azure Test Plans for traceability.**

> By iteratively applying this flow, we transitioned hundreds of cases into maintainable automated tests. MCP and Copilot handled code generation, while we reviewed and refined the scripts.

_"It felt almost like magic—describing a test in plain English and receiving a runnable Playwright script!"_

---

## Challenges and Lessons Learned

### 1. Prompt Engineering Is Critical

- Clear, specific prompts yield better code.
- Breaking down complex requirements into two prompts (“fetch test case” and then “generate script”) produced more reliable results than using a single prompt.
- Experimenting with phrasing—such as explicitly stating, “convert the above test case steps to Playwright script”—led to improved outcomes.
- Providing Copilot with access to relevant code and context (existing test files, configuration) increased the accuracy of generated scripts.

### 2. Test Case Quality Matters

- Investing effort in making manual test cases clear and detailed either upfront or after script generation is essential.
  - **Option 1:** Improve clarity and details of test steps in Azure DevOps
  - **Option 2:** Spend extra time fixing scripts if test cases are vague
- **Example:**
  - _Vague:_
    - "Click through login process"
  - _Specific:_
    - "Enter username in the 'User' field and password in the 'Password' field, then click 'Sign In' button"

### 3. Non-Textual Step Handling

- Some test scenarios involve graphics or images (e.g., verifying charts or screenshots). Copilot currently cannot interpret or assert on images.
- **Workarounds:**
  - Adjust the steps to verify via DOM or data.
  - Use Playwright’s screenshot assertions with baseline images for visual tests.
  - Handle highly visual checks manually.

_For the majority of cases involving UI interactions (click, enter data, assert text), AI-driven automation works smoothly._

---

## Appendix: Example Copilot Prompts

#### **Prompt 1: Retrieve Test Details**

```plaintext
Get me the details of the test cases (do not action anything yet, just give me the details of each test case).
Test Information:
* ADO Organization: Org_Name
* Project: Project_Name
* Test Plan ID: Test_Plan_ID
* Test Suite ID: Test_Suite_ID
```

#### **Prompt 2: Generate Playwright Scripts**

```plaintext
Imagine you are an experienced Software Engineer helping me write high-quality Playwright test scripts in TypeScript based on the test cases I provided. Please go over the task twice to make sure the scripts are accurate and reliable. Avoid making things up and do not hallucinate. Use all the extra information outlined below, to write the best possible scripts, tailored for my project.

# Project Context

Look at the "Project_name" folder, for more insights (add references to specific folders/files for even better script accuracy).

# Test Structure Requirements

1. Use 'test.describe()' for clear test grouping
2. Set up authentication before navigation
3. Robust selector strategies (multiple fallbacks)
4. Detailed logging for debugging
5. Screenshots at verification points
6. Proper error handling
7. Appropriate timeouts/waits
8. Assertions meeting acceptance criteria

# Robustness Requirements

1. Retry for flaky UI
2. Multiple selector strategies
3. Explicit waits on navigation
4. Step-by-step logging
5. Screenshot/error reporting on failure
6. Handling unexpected dialogs
7. Clear timeout errors

# Environmental Considerations

- CI/CD environments (headless, network latency, viewport variations)

# Example Usage

- Helper functions for common operations
- Fully implemented tests per case
- Comments for complex logic
- Guidance on execution

# Authentication Approach

- (Clarify steps or reference if existing)

# Configuration Reference

- (Reference specific config files if needed)

I want these tests to be maintainable, reliable, and provide clear feedback when they fail.
```

---

## Key Takeaways

- Integrating AI via GitHub Copilot and Azure DevOps MCP can accelerate test automation.
- Invest in clear, detailed test cases for best results.
- Expect iterative improvements as you refine your prompt engineering.
- Visual checks may still require manual strategies or Playwright’s baseline screenshots.

This approach allowed our team to realize tangible gains in both speed and quality in our software delivery lifecycle.

---

**References:**

- [Azure DevOps MCP Server Public Preview](https://devblogs.microsoft.com/devops/azure-devops-mcp-server-public-preview/)
- [Playwright Testing Documentation](https://learn.microsoft.com/en-us/azure/playwright-testing/quickstart-automate-end-to-end-testing)
- [Azure Test Plans Release Notes](https://learn.microsoft.com/en-us/azure/devops/release-notes/2025/testplans/sprint-258-update)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/from-manual-testing-to-ai-generated-automation-our-azure-devops-mcp-playwright-success-story/)
