-- ============================================================
-- 060: Update the subscription tiers of the GitHub Copilot features page
-- Sources: https://github.com/features/copilot/plans and https://docs.github.com/en/copilot/get-started/plans
-- Copilot moved from premium requests to usage-based billing with GitHub AI Credits (1 credit = $0.01 USD), dropped the yearly
-- prices and added the Max plan. The tiers are replaced as a whole, so running this again gives the same result.
-- Order: Free, Student, Pro, Pro+, Max, Business, Enterprise.
-- ============================================================

UPDATE custom_page_data
SET json_data = jsonb_pretty(jsonb_set(
        json_data::jsonb,
        '{subscriptionTiers}',
        $tiers$[
            {
                "id": "free",
                "name": "Free",
                "tagline": "For getting started with GitHub Copilot",
                "price": null,
                "features": [
                    "2,000 inline suggestions per month",
                    "Limited chat and agent usage from an allowance of GitHub AI Credits",
                    "Auto model selection",
                    "Copilot CLI and Copilot Fusion Standard",
                    "No credit card required"
                ],
                "videoAnchor": "videos-free"
            },
            {
                "id": "student",
                "name": "Student",
                "tagline": "Free for verified students",
                "price": null,
                "features": [
                    "Everything in Free",
                    "An allowance of GitHub AI Credits",
                    "Agents, except third-party agents"
                ],
                "videoAnchor": "videos-pro"
            },
            {
                "id": "pro",
                "name": "Pro",
                "tagline": "For everyday coding with agents ($10 USD per month)",
                "price": {
                    "monthly": 10,
                    "currency": "USD"
                },
                "features": [
                    "Everything in Free",
                    "Unlimited code completions and next edit suggestions",
                    "Copilot in the cloud and code review",
                    "Model selection",
                    "1,500 AI credits per month ($15), with the option to buy more",
                    "Free for verified teachers and maintainers of popular open source projects"
                ],
                "videoAnchor": "videos-pro"
            },
            {
                "id": "proplus",
                "name": "Pro+",
                "tagline": "For more complex development with premium models ($39 USD per month)",
                "price": {
                    "monthly": 39,
                    "currency": "USD"
                },
                "features": [
                    "Everything in Pro",
                    "Access to premium models, including Opus",
                    "7,000 AI credits per month ($70), with the option to buy more",
                    "Delegate tasks to third-party coding agents",
                    "Copilot Fusion Standard, High and Max",
                    "Audit logs"
                ],
                "videoAnchor": "videos-proplus"
            },
            {
                "id": "max",
                "name": "Max",
                "tagline": "For sustained, high-volume agent workflows ($100 USD per month)",
                "price": {
                    "monthly": 100,
                    "currency": "USD"
                },
                "features": [
                    "Everything in Pro+",
                    "Priority access to new models and features",
                    "20,000 AI credits per month ($200), with the option to buy more"
                ],
                "videoAnchor": "videos-proplus"
            },
            {
                "id": "business",
                "name": "Business",
                "tagline": "For teams and organizations ($19 USD per seat per month)",
                "price": {
                    "monthly": 19,
                    "perUser": true,
                    "currency": "USD"
                },
                "features": [
                    "Centralized management and Copilot policy control",
                    "1,900 AI credits per user per month, pooled across the organization",
                    "Budgets and spending controls for additional usage",
                    "Access to premium models",
                    "Data excluded from training by default"
                ],
                "videoAnchor": "videos-pro"
            },
            {
                "id": "enterprise",
                "name": "Enterprise",
                "tagline": "For enterprises on GitHub Enterprise Cloud ($39 USD per seat per month)",
                "price": {
                    "monthly": 39,
                    "perUser": true,
                    "currency": "USD"
                },
                "features": [
                    "Everything in Business",
                    "3,900 AI credits per user per month, pooled across the enterprise",
                    "Priority access to premium models",
                    "Enterprise-grade capabilities"
                ],
                "videoAnchor": "videos-proplus"
            }
        ]$tiers$::jsonb)),
    updated_at = NOW()
WHERE key = 'githubcopilot-features';
