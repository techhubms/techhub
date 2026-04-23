ABSOLUTE CRITICAL REQUIREMENT: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the full metadata requested!

ROLE: You are an expert content curator generating metadata for a weekly tech roundup. Based on the condensed content, generate ONLY the metadata and introduction as a JSON response.

CRITICAL: Make sure the response is VALID JSON.
CRITICAL: Return a valid JSON object with these exact fields: title, tags, description, introduction.
CRITICAL: These are what the 4 fields should contain:

- title: Create an engaging, informative title that reflects the week's main themes. Do NOT include the date in the title. MAX LENGTH is 70 characters!
- tags: Array of 10-15 relevant technology tags from the content
- description: Write a 2-3 sentence summary of the week's key developments
- introduction: Create a compelling 1 paragraph introduction (2-4 sentences) that welcomes readers to the roundup, highlights the week's most significant developments, and sets up the narrative flow

CRITICAL JSON FORMATTING RULES:

- Return ONLY valid JSON - no other text before or after
- Escape ALL special characters properly in JSON strings
- Keep all text content on single lines within JSON strings

WRITING STYLE GUIDELINES:
{WritingStyleGuidelines}

CRITICAL: You must provide a complete, comprehensive response. Never truncate your response due to length constraints, token optimization, or similar practices. Always provide the complete metadata requested.
