---
external_url: https://www.reddit.com/r/devops/comments/1mfq8ri/long_running_celery_tasks_with_zero_downtime/
title: Long Running Celery Tasks with Zero Downtime Updates
author: Lazy_Economy_6851
feed_name: Reddit DevOps
date: 2025-08-02 13:13:29 +00:00
tags:
- Background Jobs
- Blue Green Deployment
- Celery
- Coolify
- Deployment Strategies
- Docker
- Mysql
- Redis
- Task Management
- Zero Downtime
section_names:
- devops
primary_section: devops
---
Lazy_Economy_6851 discusses challenges in redeploying an app with Celery-backed long-running jobs, aiming for zero downtime. The post explores risks to running tasks and seeks peer advice on deployment strategies.<!--excerpt_end-->

## Summary

Lazy_Economy_6851 describes a DevOps challenge involving an application that processes user-submitted "validation tasks." The backend leverages Celery (for distributed task management), Redis (as a broker), and MySQL (to track state), supporting jobs that can last up to an hour. This architecture is containerized using Docker and orchestrated through Coolify, which offers built-in blue-green deployment features.

The core concern is deploying updates or upgrades to the Celery worker service without any downtime or interruption to in-flight tasks. The author specifically does not want any currently running jobs to be lost or restarted during a deployment or environment switch.

### Deployment Environment

- **Task runner:** Celery
- **Broker:** Redis
- **Tracking:** MySQL
- **Orchestration:** Docker (via Coolify)
- **Deployment Methods Considered:** Blue-green or environment switching

## Key Challenges

- **Long-lived tasks risk disruption:** Stopping or redeploying Celery workers without proper handling can terminate or lose running tasks.
- **Blue-green deployments**: While these reduce downtime for API/web layers, they may not safeguard long-running background jobs.
- **Desire for zero downtime:** The target is seamless upgrades and no job loss.

## Sought Advice

- Strategies or real-world solutions for redeploying Celery workers that do not disrupt active jobs.
- Approaches to orchestrate rolling upgrades, worker draining, or handoff gracefully within Coolify or Docker setups.

## Potential Next Steps

The author is aware of general deployment concepts (like blue-green deployments) but is searching for nuanced experiences or best practices relating to Celery and similar stack deployments.

### Ideas Often Considered for Similar Scenarios

- **Worker Draining:** Configure Celery workers to finish current tasks before shutting down (`--no-force-shutdown` / `--graceful` options), or signal with `SIGTERM` to allow for graceful exit after current jobs complete.
- **Rolling Deployments:** Upgrade workers one at a time so no job is left unhandled.
- **Sticky Queues:** Use persistent state-tracking in MySQL/Redis, to potentially resume or retry failed/incomplete jobs.
- **Instance Overlap:** Spin up new worker containers before decommissioning old ones.

## Conclusion

This community post is a request for input on zero-downtime deployment strategies tailored to Celery-based long-running job systems, sharing pain points and looking for practical guidance from peers who have solved similar issues.

This post appeared first on "Reddit DevOps". [Read the entire article here](https://www.reddit.com/r/devops/comments/1mfq8ri/long_running_celery_tasks_with_zero_downtime/)
