Copilot usage metrics now indicate which users have Copilot code review (CCR) activity, and whether that activity was active or passive. Enterprise and organization admins can see how users engage with Copilot code review on daily and 28-day user-level reports, enabling a clearer picture of CCR adoption and engagement.

Within the [API response](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10), CCR usage is represented by two fields at the user level:

- **`used_copilot_code_review_active`**: Set to `true` when a user intentionally engaged with Copilot code review by:

Assigning Copilot as a reviewer on a pull request.

- Requesting Copilot reviews again.

- Applying a CCR suggestion.

- **`used_copilot_code_review_passive`**: Set to `true` when Copilot code review automatically ran on a user’s pull request (via a repo-level policy), but the user did not interact with the review.

If a user has both active and passive CCR events on the same day, the active signal takes precedence.

You can learn more in [our documentation about Copilot usage metrics](https://docs.github.com/copilot/concepts/copilot-usage-metrics/copilot-metrics).

### [Why this matters](#why-this-matters)

With this update, you can:

- **Measure real engagement, not just coverage.** Distinguish between users who actively interact with Copilot code review and those whose pull requests are reviewed automatically but without engagement.

- **Track adoption maturity.** An admin can now say “100% of my repositories are covered by CCR, and 60% of my users engage with those reviews actively” instead of a single, undifferentiated active-user count.

- **Distinguish across Copilot surfaces.** `used_copilot_code_review_active` / `used_copilot_code_review_passive` sit alongside `used_agent` (IDE agent mode) and `used_copilot_coding_agent` (CCA) for a complete view of adoption.

Visit our [API documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10) to learn more.

Join the discussion within [GitHub Community](https://github.com/orgs/community/discussions/categories/announcements).