---
layout: post
title: 'Why Bugs Survive Continuous Fuzzing: Lessons from OSS-Fuzz Research'
author: Antonio Morales
canonical_url: https://github.blog/security/vulnerability-research/bugs-that-survive-the-heat-of-continuous-fuzzing/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-12-29 22:01:14 +00:00
permalink: /devops/news/Why-Bugs-Survive-Continuous-Fuzzing-Lessons-from-OSS-Fuzz-Research
tags:
- AFL++
- CI/CD
- Code Coverage
- Context Sensitive Coverage
- Evince
- Exiv2
- FRFuzz
- Fuzzing
- GitHub Security Lab
- GStreamer
- Open Source
- OSS Fuzz
- Poppler
- Security Testing
- Value Coverage
- Vulnerability Research
section_names:
- devops
- security
---
Antonio Morales of the GitHub Security Lab delves into why bugs can persist in projects that undergo continuous fuzzing, highlighting case studies and offering a five-step workflow for better vulnerability discovery.<!--excerpt_end-->

# Bugs that Survive the Heat of Continuous Fuzzing

**Author: Antonio Morales**

Even in projects subjected to years of intensive continuous fuzzing through platforms like [OSS-Fuzz](https://github.com/google/oss-fuzz), critical security vulnerabilities can go undetected. Antonio Morales from GitHub Security Lab explores why this is the case, drawing on audits of multiple open-source projects and sharing a detailed workflow to help practitioners find the bugs most likely to slip through the cracks.

## Key Case Studies

### GStreamer

- GStreamer showed that limited numbers of active fuzzers and low code coverage (19%) allowed 29 new vulnerabilities to escape detection, even after seven years of fuzzing. In contrast, projects like OpenSSL benefit from 139 fuzzers and much higher coverage numbers.
- Human involvement is necessary to maintain and expand coverage, as AI tools are not yet sufficient.

### Poppler

- Poppler, a core PDF component in Linux, had vulnerabilities persisting due to gaps in dependency coverage. For instance, critical dependencies like DjVuLibre were neither fuzzed nor well-tracked, despite being included in Linux distributions by default.
- Vulnerabilities in these "unfuzzed" dependencies can be exploited, as seen in reference-count overflow vulnerabilities found by the GitHub Security Lab team.

### Exiv2

- Despite three years of OSS-Fuzz integration, new vulnerabilities still surfaced. Researchers tend to focus fuzzing efforts on decoding rather than encoding logic, leaving less obvious attack surfaces exposed for years.

## The Five-Step Fuzzing Workflow

Antonio outlines a practical workflow to improve fuzzing results:

1. **Code Preparation:** Optimize the target code by removing randomness and making it more suitable for fuzzing.
2. **Improving Code Coverage:** Strive for >90% coverage by writing targeted fuzzers and creating diverse input cases.
3. **Context-Sensitive Coverage:** Go beyond edge coverage by considering function stack contexts and sequence of execution using techniques like AFL++'s context-sensitive branch and N-Gram branch coverage.
4. **Value Coverage:** Guide fuzzers not just down paths, but across the full range of variable values using strategies like bucketing and instrumented coverage.
5. **Triaging and Deep Bug Discovery:** Recognize the types of bugs that fuzzers miss, such as those requiring extremely large inputs or that manifest only after long execution times. Address these with manual review, static analysis, or symbolic testing.

## Edge Cases Fuzzing Misses

- Bugs requiring very large or specially crafted inputs (e.g., >2GB files) are often missed due to practical fuzzing limitations.
- Bugs needing execution times longer than typical (milliseconds) fuzzing runs can also persist unnoticed.
- Manual review, static and concolic analysis are still needed for these cases.

## Conclusion

Continuous fuzzing is powerful but incomplete. Security requires both robust automation and hands-on review. Antonio encourages practitioners to adopt a more nuanced, multi-step fuzzing workflow and offers additional resources like the [Fuzzing 101 course](http://gh.io/fuzzing101).

---

**References:**

- [OSS-Fuzz](https://github.com/google/oss-fuzz)
- [GStreamer Vulnerability Research](https://github.blog/security/vulnerability-research/uncovering-gstreamer-secrets/)
- [Poppler and DjVuLibre Vulnerability](https://github.com/github/securitylab/tree/main/SecurityExploits/DjVuLibre/MMRDecoder_scanruns_CVE-2025-53367)
- [FRFuzz Framework](https://github.com/antonio-morales/frfuzz)
- [Fuzz Introspector statistics](https://introspector.oss-fuzz.com/)

For a deeper dive, check the full [GitHub Blog post](https://github.blog/security/vulnerability-research/bugs-that-survive-the-heat-of-continuous-fuzzing/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/security/vulnerability-research/bugs-that-survive-the-heat-of-continuous-fuzzing/)
