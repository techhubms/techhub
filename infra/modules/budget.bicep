// Subscription-scoped monthly cost budget with email alerts at 80% / 100% / 120%.
// Requires the deploying principal to have Microsoft.Consumption/budgets/write at subscription scope.
targetScope = 'subscription'

@description('Budget name')
param budgetName string = 'budget-techhub-monthly'

@description('Monthly budget amount in the billing currency (e.g. EUR)')
param amount int

@description('Email addresses to notify when thresholds are exceeded')
param contactEmails string[]

@description('Start date for the budget (YYYY-MM-01 format; must be first of the month)')
param startDate string

resource budget 'Microsoft.Consumption/budgets@2023-11-01' = {
  name: budgetName
  properties: {
    category: 'Cost'
    amount: amount
    timeGrain: 'Monthly'
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
