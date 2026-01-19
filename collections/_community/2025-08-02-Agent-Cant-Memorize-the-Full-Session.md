---
external_url: https://www.reddit.com/r/GithubCopilot/comments/1mfu87w/agent_cant_memorize_the_full_session/
title: Agent Can't Memorize the Full Session?
author: Own-Dark14
viewing_mode: external
feed_name: Reddit Github Copilot
date: 2025-08-02 16:05:40 +00:00
tags:
- Agent Behavior
- Claude
- Code Execution
- Context Retention
- Contextual Consistency
- Docker Compose
- Python Execution
- Session Memory
- User Experience
section_names:
- ai
- github-copilot
---
Own-Dark14 highlights a limitation in GitHub Copilot's session memory, where the agent fails to retain context consistently during code execution.<!--excerpt_end-->

### Summary

Own-Dark14 discusses a user experience issue encountered with GitHub Copilot's agent during code execution within a single session. Initially, when prompted, the agent correctly executed code using `docker-compose`. Upon updating the code and requesting another run, the agent unexpectedly switched to running it with plain `python`, seemingly forgetting the previously used method.

### Observations

- **Inconsistency Noted**: The agent failed to remember its earlier context (execution via `docker-compose`) and changed behavior mid-session.
- **Expected Behavior**: Ideally, the agent should recall the context of execution style previously used in the session for consistency.
- **Comparison**: The author references Claude, noting its stronger session memory. Claude informs users not to re-upload files as it remembers within a session, benefiting user consistency.
- **User Recommendation**: The post advocates for improved contextual consistency in GitHub Copilot, suggesting that session memory enhancement would make for a more user-friendly experience.

### Conclusion

Retaining execution context, such as the method used to run code, is valuable for maintaining a seamless and predictable workflow in tools like GitHub Copilot.

This post appeared first on "Reddit Github Copilot". [Read the entire article here](https://www.reddit.com/r/GithubCopilot/comments/1mfu87w/agent_cant_memorize_the_full_session/)
