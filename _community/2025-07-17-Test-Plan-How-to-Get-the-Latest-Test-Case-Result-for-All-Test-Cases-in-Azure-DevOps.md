---
layout: "post"
title: "Test Plan: How to Get the Latest 'Test Case Result' for All Test Cases in Azure DevOps"
description: "The author seeks an efficient way to extract a report from a master test plan in Azure DevOps showing all test cases with their latest result, including details like case ID, name, and last result status. Current tools fall short of providing status history or dates."
author: "penelope77"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1m26lp7/test_plan_or_test_suite_how_to_get_the_last_test/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-07-17 12:46:30 +00:00
permalink: "/2025-07-17-Test-Plan-How-to-Get-the-Latest-Test-Case-Result-for-All-Test-Cases-in-Azure-DevOps.html"
categories: ["Azure", "DevOps"]
tags: ["Automation", "Azure", "Azure DevOps", "Community", "DevOps", "Execution History", "Reporting", "Software Testing", "Test Case", "Test Case ID", "Test Management", "Test Plan", "Test Results"]
tags_normalized: ["automation", "azure", "azure devops", "community", "devops", "execution history", "reporting", "software testing", "test case", "test case id", "test management", "test plan", "test results"]
---

Author penelope77 describes a challenge in Azure DevOps: extracting all test cases with their latest results from a master test plan, highlighting current limitations.<!--excerpt_end-->

## Summary

penelope77 describes a scenario involving Azure DevOps (ADO) test plans, where a master test plan holds all test cases, and subsets are reused for projects or features. While individual test case execution history is available via the UI, the author wishes to generate a comprehensive report for the master plan, listing each test case along with its latest execution result—specifically, including the test case ID, name, and the latest result status.

The tools currently available in Azure DevOps (the export tab and its options) only display the current status and lack important historical data, such as the last execution date. The author notes that these export features do not provide the required granular control or historical insight they seek.

## Key Points

- **Context:** All test cases are managed in a master test plan; feature-specific test plans reuse subsets.
- **Goal:** Generate a report with every test case and its most recent execution result, including status and date.
- **Current Challenge:**
  - The UI allows viewing individual execution histories but cannot batch-export them.
  - Export tools are insufficient—either lacking data about history/dates or not providing the latest-result snapshot per test case.
  - No apparent built-in feature meets this specific need.

## Practical Implications

- **Test Management Limitation:** Azure DevOps’ standard reporting and export tools may not fit more advanced reporting scenarios out-of-the-box.
- **Potential Workarounds:**
  - Use of Azure DevOps REST API to extract case results programmatically.
  - Custom queries or scripts to aggregate latest test case results and statuses.
- **Team Impact:** Access to efficient reporting would allow teams to identify stale or unrun test cases, improving QA cycles and audit preparedness.

## Conclusion

The post highlights a gap in Azure DevOps for advanced test result reporting and invites solutions from the community. The desired feature would be valuable for teams needing better insights into test coverage, recency, and quality assurance processes.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1m26lp7/test_plan_or_test_suite_how_to_get_the_last_test/)
