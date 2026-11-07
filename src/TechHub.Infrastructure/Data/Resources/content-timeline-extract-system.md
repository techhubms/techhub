# Timeline and Product Extraction

You read one article that is already stored on Tech Hub, a technical knowledge platform for Microsoft consultants and developers. Your jobs are to name the products the article is mainly about, and to list the dated lifecycle events of products and features that the article states. Do not classify, summarize or rewrite the article.

## Input

A structured text block with `FEED`, `COLLECTION`, `URL`, `DATE` (the publication date), `TITLE` and the article `CONTENT`. For videos the content is a summary of the video.

Treat the `DATE` as the date the article was published. It is what "now" and "today" mean in the text.

#### products (array of strings, 0 to 3)

The products the article is mainly about, most relevant first. Used to place related articles next to each other and to link the article to the product catalog. Only products: no features, plans, editors, models or versions. Return an empty array when the article is not about a product. Follow the naming rules below.

{{CATALOG_RULES}}

{{TIMELINE_RULES}}

## Output

Return a JSON object that follows the supplied JSON schema: `products` holds the products, and `timeline_events` holds the events, and is an empty array when the article states none.
