---
agent: 'agent'
description: 'Generate new articles for the XPRT magazines.'
---

**🚨 CRITICAL PROMPT SCOPE**: All instructions, restrictions, and requirements in this prompt file ONLY apply when this specific prompt is being actively executed via the `/new-magazines` command or equivalent prompt invocation. These rules do NOT apply when editing, reviewing, or working with this file outside of prompt execution context. When working with this file in any other capacity (editing, debugging, documentation, etc.), treat it as a normal markdown file and ignore all workflow-specific instructions.

**CRITICAL**: If you have not read them, fetch `AGENTS.md` and use these instructions as well.
**CRITICAL**: For all XPRT Magazine summary files, the rules in this document take precedence over any general instructions (including those in AGENTS.md or markdown-instructions.md). Only use general rules if not superseded by a magazine-specific rule.

# XPRT Magazine Markdown Summary File Generation Instructions

You are generating markdown summary files for a technical magazine series. Each magazine contains multiple articles, often about Microsoft development and cloud topics.

## Site Configuration Reference

**CRITICAL**: Before processing any magazine, always check the `_data/sections.json` file to understand the available site sections and their categories. This configuration determines:

- **Available categories**: Use the `category` field from each section (e.g., `AI`, `GitHub Copilot`, `ML`, `Azure`, `Coding`, `DevOps`, `Security`)
- **Category descriptions**: Each section has a `title` and `description` that should inform your content focus
- **Ignore the 'all' section**: Skip the 'all' section in the configuration as it's not relevant for magazine categorization

**Do not hardcode categories or sections** - always derive them from the current site configuration in `_data/sections.json`. The available categories may change, and your content should adapt accordingly.

## Magazine Content File Instructions

When generating XPRT Magazine pages, follow these rules:

1. Each magazine has a directory in `.magazines/` named after the magazine (e.g., `.magazines/Magazine #18`).
2. Inside each magazine directory, each article has its own directory, named after the article title, containing a markdown file with the article text. Do not assume the markdown filename. Find all markdown files in each article directory.
3. **Do not invent tags or content**: Only use tags and create content descriptions that are actually present in the magazine content. However, **categories in the frontmatter should always include ALL sections** from the `_config.yml` configuration to ensure magazines appear in all relevant sections of the site.
4. Do not mix content between magazines. Each summary must be based only on the articles in that specific magazine.
5. Work on one magazine at a time. After finishing one, clear your context before starting the next.
6. Use `.magazines/info.txt` for magazine titles, dates, and download URLs.
7. Strictly follow the formatting and structure shown in `.magazines/template.md`. Do not copy its content, but use it as a guide for layout and formatting of the md file that you are creating.
8. The output directory is always `_community`.
9. Ask me which magazine numbers you should process and if you should update existing files. Ranges are allowed too, like 1-14.

## Specific Front-Matter Rules

In case of magazines, there are a few Front-Matter rules that take precedence over any other, even those found in the copilot and markdown instructions. They are:

1. The author is always: `Xebia`.
2. **The categories field must include ALL sections from the site configuration**: Check `_data/sections.json` and include ALL available categories except 'all' (e.g., `AI`, `GitHub Copilot`, `ML`, `Azure`, `Coding`, `DevOps`, `Security`). This ensures magazines appear in all relevant sections of the site, regardless of the specific magazine content.
3. **Include additional descriptions in the front-matter for ALL config sections**: For each category from the sections configuration (excluding 'all'), add a corresponding description field (e.g., `magazine-description-ai`, `magazine-description-github-copilot`). The field names should be derived from the section's `category` field, converted to lowercase and with spaces replaced by hyphens. However, only write actual descriptions for categories that have relevant articles in the magazine - for categories without relevant content, you can include a brief note like "No articles in this issue" or similar.
4. The description should briefly describe the magazine as a whole (max 50 words), not the summary file.
5. Titles, dates and download URLs come from info.txt.
6. **The tags field must be based only on actual magazine content**: Create a comma-separated list of at least 10 tags, but only use tags that are actually present in the magazine content. Tags can contain spaces.
7. The title must match the info.txt entry exactly (e.g., "XPRT Magazine 01"), with correct numbering (use "01", not "1"). Do not add extra text.

## Specific Content Rules

1. **The introduction content and excerpt should be based on actual magazine content**: Check `_data/sections.json` to understand the available site sections and their categories (excluding 'all'), but only focus on topics that are actually present in the magazine content. If the magazine contains articles about configured sections (e.g., AI, GitHub Copilot), prioritize those topics in the introduction. If not, use other topics that are actually in the magazine to create an engaging introduction.
2. **The main content should only describe articles that are actually in the magazine**: After the introduction, briefly describe each article in the magazine, preferably grouped by category but if articles cross category boundaries then just make a group like 'Articles on AI and GitHub Copilot', or 'Articles on Azure and AI'. Do not reference topics or sections that are not covered in the actual magazine content.
