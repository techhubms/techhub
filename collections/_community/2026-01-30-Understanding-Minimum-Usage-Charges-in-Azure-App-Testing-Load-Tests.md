---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/minimum-usage-in-azure-app-testing/ba-p/4490658
title: Understanding Minimum Usage Charges in Azure App Testing Load Tests
author: Nikita_Nallamothu
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-01-30 06:56:43 +00:00
tags:
- AI
- AI Assisted Testing
- Azure
- Azure App Testing
- Azure Load Testing
- Billing
- CI/CD
- Cloud Testing
- Community
- Cost Optimization
- DevOps
- Load Testing
- Microsoft Azure
- Performance Testing
- Test Automation
- Test Configuration
- Test Infrastructure
- Testing Strategies
- Virtual User Hours
section_names:
- azure
- devops
---
Nikita Nallamothu explains Azure App Testing's new minimum Virtual User Hours policy for load tests, including practical examples and tips to help your team avoid unnecessary charges while optimizing test efficiency.<!--excerpt_end-->

# Understanding Minimum Usage Charges in Azure App Testing Load Tests

**Author:** Nikita Nallamothu  

Azure App Testing recently introduced a new minimum usage policy for load test runs, taking effect on **March 1, 2026**. This change is aimed at ensuring efficient utilization of test infrastructure and avoiding unnecessary underutilization of resources.

## What’s Changing?

- All load tests in Azure App Testing will now incur a **minimum Virtual User Hours (VUH)** charge per test run.
- For each load test run, you will be charged for:
  - **10 Virtual Users (VUs) per engine** for the test duration, or
  - **10 VUs per engine for 10 minutes** if the test duration is shorter than 10 minutes.

If your configured test already meets or exceeds this minimum, there will be no impact. This policy does **not** impact Playwright tests in Azure App Testing.

## Rationale

Behind every load test is dedicated infrastructure provisioned specifically for the test. Very short or low-user tests can lead to inefficient usage and higher costs per test for Microsoft, so this policy encourages more meaningful, resource-efficient load tests.

## How the Minimum Usage Policy Works

Here are concrete examples showing how this policy applies:

### Example 1: Low-user, long-duration test

- **Configuration:** 5 VUs, 1 engine, 3 hours
- **Actual usage:** 15 VUH (5 VUs × 3 hours × 1 engine)
- **Minimum usage:** 30 VUH (10 VUs × 3 hours × 1 engine)
- **Charged:** 30 VUH (minimum usage applies)

### Example 2: Low-user, short-duration test

- **Configuration:** 5 VUs, 1 engine, 5 minutes
- **Actual usage:** 0.83 VUH
- **Minimum usage:** 1.67 VUH
- **Charged:** 1.67 VUH (minimum usage applies)

### Example 3: High-user, short-duration test

- **Configuration:** 500 VUs, 2 engines, 5 minutes
- **Actual usage:** 83.34 VUH (usage exceeds minimum)
- **Minimum usage:** 3.33 VUH
- **Charged:** 83.34 VUH (actual usage applies)

## How to Optimize Your Load Test Configurations

To avoid unexpected charges and improve both cost and test effectiveness, consider:

- Reducing engine count in low-user tests.
- Increasing user count or test duration in short or low-user tests.
- Ensuring configurations meet or exceed minimum usage.

A small change in test setup can optimize both results and spending.

## Further Resources and Support

- The [pricing page](https://azure.microsoft.com/pricing/details/load-testing/) and pricing calculator will be updated soon to reflect this new policy.
- For technical help, create a support request via the [Azure portal](https://ms.portal.azure.com/#view/Microsoft_Azure_Support/HelpPane.ReactView/callerName/Microsoft_Azure_CloudNativeTesting%2BCreateTestBlade%2B7c71b563-0dc0-4bc0-bcf6-06f8f0516c7a/issueType/quota/assetId/%2Fsubscriptions%2F7c71b563-0dc0-4bc0-bcf6-06f8f0516c7a%2FresourceGroups%2Fdemo-space-rg%2Fproviders%2FMicrosoft.LoadTestService%2Floadtests%2Fdemo-space/subscriptionId/7c71b563-0dc0-4bc0-bcf6-06f8f0516c7a/productId/06bfd9d3-516b-d5c6-5802-169c800dec89/topicId/1e43cc7f-7ff1-9a9a-2b32-eddc9c67c618/internal_bladeCallId~/1/internal_bladeCallerParams~/%7B%22initialData%22%3A%7B%22v%22%3Anull%2C%22%23fxSerialized%23%22%3Atrue%7D%7D).
- For feedback, use the [Developer Community](https://aka.ms/malt-feedback).

## Related Resources

- [AI-assisted load test authoring in Azure App Testing](https://techcommunity.microsoft.com/blog/appsonazureblog/ai-assisted-load-test-authoring-in-azure-app-testing/4480652)
- [Azure Load Testing documentation](https://learn.microsoft.com/azure/load-testing/)

**Published January 29, 2026**

---

*This policy overview is provided by Nikita Nallamothu to help Azure customers understand and adapt to the new minimum usage requirements for load tests in Azure App Testing.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/minimum-usage-in-azure-app-testing/ba-p/4490658)
