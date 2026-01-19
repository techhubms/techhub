---
layout: post
title: 'Security Risks from Deleted GitHub Commits: Admin Access to Istio Exposed'
author: Tim Anderson
canonical_url: https://devclass.com/2025/07/03/security-researcher-exploits-github-gotcha-gets-admin-access-to-all-istio-repositories-and-more/
viewing_mode: external
feed_name: DevClass
feed_url: https://devclass.com/feed/
date: 2025-07-03 14:48:30 +00:00
permalink: /devops/blogs/Security-Risks-from-Deleted-GitHub-Commits-Admin-Access-to-Istio-Exposed
tags:
- Credential Leaks
- DevOps Security
- DevSecOps
- GH Archive
- Git
- Git Filter Repo
- GitHub
- Infosec
- Istio
- Kubernetes
- Personal Access Token
- Repository Hygiene
- Secret Management
- Service Mesh
- Source Control
- Token Revocation
- Truffle Security
section_names:
- devops
- security
---
Tim Anderson's article explores how lingering commit history on GitHub enabled a researcher to find secrets—including admin tokens for Istio—highlighting security risks and mitigation strategies for developers.<!--excerpt_end-->

# Security Risks from Deleted GitHub Commits: Admin Access to Istio Exposed

A recent investigation by security researcher Sharon Brizinov has brought attention to a lesser-known threat in source control use: secrets disclosed in 'deleted' GitHub commits can persist in public archives, exposing critical credentials long after a developer tries to remove them.

## Key Incident Details

- Brizinov searched public GitHub commit archives for forgotten or 'deleted' commits.
- He uncovered a Personal Access Token (PAT) for an Istio project developer, which granted **admin access to all Istio repositories**. Istio is a major open-source service mesh solution used alongside Kubernetes in many enterprises.
- The exposed token was revoked after discovery, but the event illustrates the real risk posed by improper secret management in code repositories and the permanence of Git history.

## Why Are Secrets Hard to Delete?

- When sensitive information is accidentally committed, developers often try to 'fix' the issue by deleting or force-pushing history.
- **Force-pushing** only re-aligns the branch pointer; the actual commit still exists and can be retrieved by SHA1 hash or via repository archives like [GH Archive](https://www.gharchive.org).
- Complete removal typically requires tools like [git-filter-repo](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/removing-sensitive-data-from-a-repository) and manual intervention from GitHub support, including pull request purging and garbage collection.

## Exploitation and Tooling

- Brizinov used GH Archive (which logs all public repo activity in BigQuery) and a custom script to find orphaned commits and search for secrets such as PATs and AWS credentials.
- The most common leak involved `.env` files, representing over half of all discovered sensitive disclosures.
- Other tools, like Truffle Security, automate searching for leaked secrets but are less effective once exposure is public.

## Mitigation and Best Practices

- **Immediate Response**: Revoke any exposed tokens or credentials and change relevant passwords.
- **Prevention**: Use pre-commit hooks (e.g., [AWS git-secrets](https://github.com/awslabs/git-secrets)) to scan for sensitive data before pushing.
- **Cleaning History**: For actual deletion, employ advanced tools and reach out to platform support for deep clean-up.
- **Awareness**: Educate development teams on Git's immutable nature and on risk factors like GH Archive/other public mirrors.

## Further Reading

- [GitHub Support Article: Removing Sensitive Data](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/removing-sensitive-data-from-a-repository)
- [StackOverflow: How can I remove a commit on GitHub?](https://stackoverflow.com/questions/448919/how-can-i-remove-a-commit-on-github)
- [GH Archive Project](https://www.gharchive.org)

Staying vigilant with secrets in source control isn't just best practice—it's vital to preventing wide-reaching security incidents.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/07/03/security-researcher-exploits-github-gotcha-gets-admin-access-to-all-istio-repositories-and-more/)
