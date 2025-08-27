---
layout: "post"
title: "How to Keep key.properties Private in a Public GitHub Repository"
description: "The author seeks advice on making a sensitive file (key.properties) private within a public GitHub repository used for Android app development. They wonder if it's possible to keep the file hidden without making the whole repository private or duplicating the repository."
author: "el_psy_kongree"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/github/comments/1mgrtc3/private_file_in_github_repo/"
viewing_mode: "external"
feed_name: "Reddit GitHub"
feed_url: "https://www.reddit.com/r/github/.rss"
date: 2025-08-03 19:16:38 +00:00
permalink: "/2025-08-03-How-to-Keep-keyproperties-Private-in-a-Public-GitHub-Repository.html"
categories: ["DevOps"]
tags: [".gitignore", "Android Development", "Community", "DevOps", "GitHub", "Key.properties", "Private Files", "Public Repository", "Repository Privacy", "Secrets Management", "Sensitive Data", "Version Control"]
tags_normalized: ["dotgitignore", "android development", "community", "devops", "github", "keydotproperties", "private files", "public repository", "repository privacy", "secrets management", "sensitive data", "version control"]
---

Author el_psy_kongree discusses strategies for keeping the key.properties file private in a public GitHub repo for Android app development.<!--excerpt_end-->

In the process of publishing an Android mobile app, the author encounters a common challenge: safeguarding sensitive files—such as the key.properties file—within a public GitHub repository. The key.properties file contains important security credentials and should not be exposed publicly.

The author asks whether it is possible to keep this file private while leaving the rest of the repository public, expressing reluctance to maintain two separate repositories (one public without secrets, another private containing production keys).

**Considerations and Common Practices:**

- **.gitignore**: The typical solution is to add `key.properties` to the `.gitignore` file. This ensures the file is never committed to the repository. Developers working on the project should generate their own `key.properties` or receive it securely through another channel.

- **Environment Variables**: Some build systems can be configured to read sensitive information from environment variables instead of files, reducing the risk of accidentally including secrets in version control.

- **Secret Management**: Tools and platforms (such as GitHub Actions Secrets or encrypted files) can manage sensitive credentials during CI/CD without storing them in the repository.

- **Documentation**: Include an example (e.g., `key.properties.example`) with placeholder values to show collaborators the required format, and document secure procedures to obtain or generate the real file.

- **Avoid Public Exposure**: Never commit sensitive files to any public repository. If such a file has been committed, use GitHub's history-rewriting tools or BFG Repo-Cleaner to ensure secrets aren't exposed in previous commits.

By following these practices, you can keep a single public repository without compromising the security of sensitive files.

This post appeared first on "Reddit GitHub". [Read the entire article here](https://www.reddit.com/r/github/comments/1mgrtc3/private_file_in_github_repo/)
