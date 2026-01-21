# Writing Style Guidelines

This document defines the writing style and tone requirements for all content in the Tech Hub. These guidelines apply to all output: code comments, documentation, chat responses, generated content, articles, descriptions, and user-facing text.

## Core Writing Principles

### Tone and Voice

- **Down-to-earth and authentic**: Avoid exaggerated language, hype, and marketing buzzwords
- **Professional yet approachable**: Clear and authoritative without being overly formal
- **Direct and honest**: Get to the point without unnecessary embellishment
- **Respectful and inclusive**: Use language that welcomes all readers

### Language Standards

- **Clear, professional English**: Write for international audiences with varying English proficiency
- **Concise and actionable**: Every sentence should serve a purpose
- **Active voice preferred**: "The system processes data" instead of "Data is processed by the system"
- **Consistent terminology**: Use the same terms throughout (see [terminology.md](terminology.md))

### Character and Typography Standards

**Use plain ASCII characters for consistency and to avoid encoding issues.**

| Use This | Not This | Why |
| -------- | -------- | --- |
| `-` (hyphen) | `—` (em-dash) or `–` (en-dash) | ASCII compatibility, searchability |
| `"` (straight quotes) | `"` `"` (curly quotes) | Prevents encoding mismatches |
| `'` (straight apostrophe) | `'` `'` (curly apostrophes) | Consistent rendering |
| `...` (three periods) | `…` (ellipsis character) | Easier to type and match |

**Punctuation rules:**

- **Use parentheses for asides**: Write `GitHub Actions (the CI/CD platform) is popular` instead of using em-dashes
- **Minimize semicolons**: Use periods and start new sentences instead - semicolons can make text feel formal and choppy
- **Avoid excessive parentheses**: Incorporate information into the main sentence flow when possible

**Example:**

- Good: `Everything an AI processes (words, images, concepts) gets converted into vectors, which are simply lists of numbers that capture meaning.`
- Avoid: `Everything an AI processes—words, images, concepts—gets converted into vectors. Which are simply lists of numbers; they capture meaning.`

**Rationale**: Plain ASCII characters are easier to search, replace, and process programmatically. Special Unicode characters can cause issues with text matching tools and may render inconsistently across different systems.

### Punctuation and Sentence Flow

Create smooth, readable prose that connects ideas naturally. Aim for sentences that guide readers forward without unnecessary stops or interruptions. Good writing flows like conversation - each sentence leads logically to the next.

**Techniques for flowing prose:**

- **Connect related ideas**: Use commas with coordinating conjunctions (and, but, so) rather than breaking into separate sentences
- **Vary sentence length**: Mix shorter declarative sentences with longer explanatory ones to create rhythm
- **Use transitional phrases sparingly**: Words like "however" and "therefore" work best at the start of paragraphs, not every sentence
- **Lead with the main point**: Put the most important information first, then add supporting details

## Specific Guidelines for AI Models

### Common AI Writing Patterns to Avoid

- **Generic openings**: Never start with "In today's digital landscape", "In an era of", "In a world where"
- **Transition sentence starters**: Limit sentences beginning with "Moreover", "Furthermore", "Additionally", "However" to one per paragraph maximum
- **Qualification overuse**: Avoid starting sentences with "It's worth noting", "It's important to understand", "It should be mentioned"
- **Abstract positioning**: Don't use phrases like "stands at the forefront", "represents a paradigm shift", "marks a new era"
- **Temporal vagueness**: Avoid "moving at this speed", "continues to move fast", "develops quickly" without specific context
- **Generic trend language**: Replace "clear uptick", "remains a focus", "across the board" with specific facts

### Positive Patterns to Embrace

- **Direct statements**: Start with the main point: "GitHub Copilot suggests code completions" not "GitHub Copilot offers innovative code completion capabilities"
- **Specific benefits**: Use measurable outcomes: "reduces coding time by 30%" not "significantly improves productivity", but only if you have data to back it up
- **Concrete examples**: Illustrate points with real scenarios: "You can filter issues by label using the dropdown" not "offers advanced filtering options"
- **Active voice**: "The system processes data" not "Data is processed by the system"
- **Human-centered language**: Focus on what users can do, not what technology enables

### Writing Quality Checks for AI

Before finishing any piece of writing, check:

1. **Count transition words**: No more than one "Moreover/Furthermore/Additionally" per paragraph
2. **Scan for buzzwords**: Remove any words from the "avoid" lists above
3. **Check sentence starters**: Vary how sentences begin - avoid repetitive patterns
4. **Verify specificity**: Each claim should be concrete and actionable
5. **Read for flow**: Text should sound natural when read aloud

## What to Avoid

**CRITICAL**: Avoid these types of words and phrases that create hype or overstate importance:

### Exaggerated Language

- **Buzzwords**: "groundbreaking", "revolutionary", "game-changing", "cutting-edge", "paradigm shift"
- **Superlatives**: "the best", "amazing", "incredible", "the ultimate solution"
- **Marketing speak**: "delve into", "leverage", "synergize", "transformative", "unprecedented"
- **AI-generated tells**: Starting with "In an era", "In a world", excessive use of "fundamentally", "Moreover", "Furthermore", "Additionally" at sentence starts, overuse of "seamlessly", "effortlessly", "streamlined"
- **Impact qualifiers**: "pivotal", "significant", "major", "substantial", "dramatic", "massive"
- **Intensity words**: "breakthrough", "cutting-edge", "world-class", "industry-leading", "paradigm-shifting"
- **Speed and movement clichés**: "move fast", "moving at this speed", "continue to advance rapidly", "develop quickly"
- **Vague growth patterns**: "clear uptick", "clear trend", "obvious shift", "dramatic increase"

### Vague or Generic Language

- **Filler phrases**: "In this article", "In this post", "It's worth noting that", "As we dive into", "Let's explore"
- **Generic adjectives**: "nice", "great", "awesome" without specific context
- **Hedge words**: Excessive use of "basically", "essentially", "obviously", "simply", "just"
- **Redundant expressions**: "future plans", "past history", "final conclusion"
- **Transition overuse**: Starting multiple sentences with "Moreover", "Furthermore", "Additionally", "However"
- **Vague trend language**: "remains a focus", "continues to be", "across the board", "with plenty of"
- **Generic emphasis**: "as well", "too", "also" when used excessively for emphasis
- **Unclear quantifiers**: "plenty of practical news", "clear uptick", "obvious trend"
- **Speed qualifiers**: "moving at this speed", "continues to move fast", "develops quickly"
- **Generic conclusions**: "With technology advancing rapidly", "As the industry evolves"

### Technical Jargon Without Context

- Define technical terms when first introduced
- Provide context for acronyms and abbreviations
- Explain concepts before diving into implementation details

## What to Embrace

### Authentic Language

- **Specific examples**: Use concrete examples rather than abstract concepts
- **Practical benefits**: Focus on what users can actually accomplish
- **Real-world context**: Explain how features solve actual problems
- **Honest limitations**: Acknowledge when something has constraints or trade-offs
- **Neutral descriptors**: Use "new", "updated", "improved", "enhanced", "additional", "latest" instead of flashy qualifiers
- **Capability-focused**: Describe what something "enables", "provides", "supports", or "includes" rather than how impressive it is

### Clear Structure

- **Logical flow**: Organize information from general to specific
- **Scannable format**: Use headings, lists, and short paragraphs
- **Action-oriented**: Tell readers what they can do with the information
- **Result-focused**: Explain outcomes and benefits clearly

## Content-Specific Guidelines

### Technical Documentation

- **Start with the problem**: Explain what users need to accomplish
- **Provide step-by-step instructions**: Break complex tasks into manageable steps
- **Include examples**: Show real code, commands, or configurations
- **Test all instructions**: Verify that examples work as written

### Article Excerpts and Descriptions

- **Lead with value**: Start with the main benefit or key insight
- **Mention the author**: Include author names in excerpts where appropriate
- **Avoid repetition**: Don't repeat the title in different words
- **Set clear expectations**: Help readers understand what they'll learn

### User Interface Text

- **Be helpful**: Guide users toward successful completion
- **Use familiar terms**: Choose words users already understand
- **Provide context**: Explain what will happen when users take actions
- **Be consistent**: Use the same terms for the same concepts throughout the interface

## Quality Checks

### Before Publishing

Ask yourself these questions about your content:

1. **Clarity**: Would someone new to this topic understand it?
2. **Accuracy**: Are all facts and examples correct and current?
3. **Usefulness**: Can readers accomplish something concrete with this information?
4. **Authenticity**: Does this sound like genuine human insight rather than marketing copy?
5. **Respect**: Is the language inclusive and welcoming to all readers?

### Content Review Process

- **Read aloud**: Content should flow naturally when spoken
- **Check for buzzwords**: Remove or replace exaggerated language
- **Verify examples**: Ensure all code, links, and instructions work
- **Test with others**: Have someone else review for clarity and usefulness

## Examples

### Good vs. Poor Writing

#### ✅ Good: Direct and Helpful

> GitHub Copilot helps developers write code faster by suggesting completions as they type. You can accept suggestions with Tab or modify them to fit your specific needs.

#### ❌ Poor: Exaggerated and Vague

> GitHub Copilot revolutionizes the development experience by leveraging cutting-edge AI to fundamentally transform how developers approach coding, delivering unprecedented productivity gains.

#### ✅ Good: Specific and Actionable

> The new activity report shows which features your team uses most, helping you identify training opportunities and measure adoption success.

or

> GitHub Copilot now uses OpenAI's GPT-5 across Visual Studio, VS Code, and JetBrains IDEs, providing improved code completion and debugging assistance.

#### ❌ Poor: Generic and Buzzword-Heavy

> This groundbreaking report delivers game-changing insights that will transform your understanding of user engagement and drive synergistic adoption strategies.

or

> AI integration, cloud platforms, and developer tools continue to move fast, with plenty of practical news across the board.

### Content Type Examples

#### Technical Instructions

✅ **Good**:
> To filter content by date, click the "Last 30 days" button. The page will update to show only recent articles without refreshing.

❌ **Poor**:
> Leverage the revolutionary date filtering paradigm to fundamentally transform your content discovery experience through our cutting-edge temporal navigation system.

#### Feature Descriptions

✅ **Good**:
> Tag filters help you find content about specific technologies. Select multiple tags to see articles that cover all your selected topics.

❌ **Poor**:
> Our innovative tag-based filtering solution empowers users to delve into highly targeted content discovery through an unprecedented multi-dimensional taxonomy system.

#### News Roundups

✅ **Good**:
> GitHub Copilot now supports GPT-5 across major IDEs. Microsoft announced new security features for Azure. Three companies released updates to their development tools this week.

❌ **Poor**:
> Technology continues to move fast across the board. There's a clear uptick in adoption of cutting-edge solutions. Meanwhile, the industry remains focused on innovation at unprecedented speed.
