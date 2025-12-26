---
layout: "post"
title: "How we solved environment variable chaos for 40+ microservices on ECS/Lambda/Batch with AWS Parameter Store"
description: "The author outlines a practical approach for managing environment variables across 40+ microservices running on AWS ECS, Lambda, and Batch. By centralizing secrets and configurations using AWS Parameter Store, the team improved security and operational efficiency, reducing manual redeployments and eliminating hardcoded secrets from their codebase."
author: "compacompila"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/devops/comments/1mgl9tl/how_we_solved_environment_variable_chaos_for_40/"
viewing_mode: "external"
feed_name: "Reddit DevOps"
feed_url: "https://www.reddit.com/r/devops/.rss"
date: 2025-08-03 14:54:50 +00:00
permalink: "/community/2025-08-03-How-we-solved-environment-variable-chaos-for-40-microservices-on-ECSLambdaBatch-with-AWS-Parameter-Store.html"
categories: ["DevOps", "Security"]
tags: ["AWS Batch", "AWS ECS", "AWS Lambda", "AWS Parameter Store", "Community", "DevOps", "Environment Variables", "Go", "IAM", "Microservices", "Secrets Management", "SecureString", "Security"]
tags_normalized: ["aws batch", "aws ecs", "aws lambda", "aws parameter store", "community", "devops", "environment variables", "go", "iam", "microservices", "secrets management", "securestring", "security"]
---

In this post, compacompila describes how their team centralized environment variable management for over 40 AWS-based microservices using AWS Parameter Store, leading to enhanced security and streamlined operations.<!--excerpt_end-->

## Introduction

Managing environment variables and secrets across a microservice ecosystem can be challenging, especially when services are distributed across multiple AWS compute platforms. compacompila shares their team's strategy for consolidating configuration and secrets management in a secure and maintainable way.

## The Problem

The organization ran over 40 microservices on AWS ECS, Lambda, and Batch. Previously, critical environment variables—including sensitive information like database connection strings and API keys—were hardcoded in configuration files and versioned in Git. This posed notable security risks and operational hardships. Whenever a secret used by multiple services needed to be changed, each service relying on it had to be redeployed individually, resulting in a slow and error-prone process.

## The Solution: Centralizing with AWS Parameter Store

The team evaluated AWS Parameter Store and Secrets Manager. They selected Parameter Store due to the cost-effective standard tier, which provides up to 10,000 parameters and free API usage. In contrast, Secrets Manager incurred ongoing per-secret costs that did not align with their scaling needs.

### Implementation Steps

1. **Parameter Creation**: Configuration values, such as `SENTRY/DSN/API_COMPA_COMPILA`, were stored as `SecureString` parameters within AWS Parameter Store.
2. **Service Configuration Update**: Instead of embedding the actual secret, environment variable definitions in services now referenced the Parameter Store path.
3. **Parameter Fetch On Startup**: Each application, at startup, utilized a small Go service leveraging the AWS SDK. This service retrieved the actual values from Parameter Store, requiring an IAM role with `kms:Decrypt` permissions to access encrypted `SecureString` values.
4. **Runtime Secret Injection**: Fetched secrets were used to configure each service instance, removing the need for hardcoded values.

## Key Benefits

- **Security**: Secrets were stripped from source control; access was managed solely via AWS IAM policies.
- **Operational Simplicity**: Updates to shared secrets could be applied in a single place. Services could refresh secrets at runtime without necessitating redeployments, greatly reducing operational friction.

## Further Information

The author offers a comprehensive article, complete with Go code examples and detailed setup screenshots, available at: [https://compacompila.com/posts/centralyzing-env-variables/](https://compacompila.com/posts/centralyzing-env-variables/)

## Closing

compacompila encourages community discussion on the topic, welcoming questions and sharing of alternative approaches to environment variable and configuration management in microservice architectures.

This post appeared first on "Reddit DevOps". [Read the entire article here](https://www.reddit.com/r/devops/comments/1mgl9tl/how_we_solved_environment_variable_chaos_for_40/)
