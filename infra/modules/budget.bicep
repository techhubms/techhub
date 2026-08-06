// Subscription-scoped budget aligned to the billing cycle, with email alerts at 80% / 100% / 120%.
// Uses BillingMonth so the reset period follows the invoice billing period (e.g. Apr 7 – May 6)
// instead of calendar months.
// Requires the deploying principal to have Microsoft.Consumption/budgets/write at subscription scope.
targetScope = 'subscription'

@description('Budget name')
param budgetName string = 'budget-techhub-billing-month'

@description('Budget amount per billing period in the billing currency (e.g. EUR)')
param amount int

@description('Email addresses to notify when thresholds are exceeded')
param contactEmails string[]

@description('Start date for the budget (YYYY-MM-DD format, aligned to billing period start)')
param startDate string

// Azure rejects updates to an existing budget's timePeriod.startDate ("Start date of budgets
// cannot be updated. Please delete and create a new budget."), so the start date is baked into
// the resource name. Changing startDate then deploys a brand-new budget resource instead of
// trying to mutate the immutable field on the old one. The previous budget (with the old name)
// is orphaned in Azure and must be deleted manually — Azure does not auto-remove it.
var uniqueBudgetName = '${budgetName}-${replace(startDate, '-', '')}'

resource budget 'Microsoft.Consumption/budgets@2024-08-01' = {
  name: uniqueBudgetName
  properties: {
    category: 'Cost'
    amount: amount
    timeGrain: 'BillingMonth'
    timePeriod: {
      startDate: startDate
    }
    notifications: {
      Actual_80_Percent: {
        enabled: true
        operator: 'GreaterThan'
        threshold: 80
        thresholdType: 'Actual'
        contactEmails: contactEmails
        locale: 'en-us'
      }
      Actual_100_Percent: {
        enabled: true
        operator: 'GreaterThan'
        threshold: 100
        thresholdType: 'Actual'
        contactEmails: contactEmails
        locale: 'en-us'
      }
      Forecasted_120_Percent: {
        enabled: true
        operator: 'GreaterThan'
        threshold: 120
        thresholdType: 'Forecasted'
        contactEmails: contactEmails
        locale: 'en-us'
      }
    }
  }
}

output budgetId string = budget.id
