# Product, Feature and Timeline Extraction

You read one article that is already stored on Tech Hub, a technical knowledge platform for Microsoft consultants and developers. Your jobs are to name the products the article is mainly about, name the product features it substantively covers, and list the dated lifecycle events it states. Feature identification does not depend on dates. Do not classify, summarize or rewrite the article.

## Input

A structured text block with `FEED`, `COLLECTION`, `URL`, `DATE` (the publication date), `TITLE` and the article `CONTENT`. For videos the content is a summary of the video.

Treat the `DATE` as the date the article was published. It is what "now" and "today" mean in the text.

#### products (array of strings, 0 to 3)

The products the article is mainly about, most relevant first. Used to place related articles next to each other and to link the article to the product catalog. Only products: no features, plans, editors, models or versions. Return an empty array when the article is not about a product. Follow the naming rules below.

{{CATALOG_RULES}}

#### product_features (array of objects, 0 to 20)

List the product-feature pairs the article substantively covers, even when it states no dates or timeline events. Do not include an incidental mention, and do not infer that a capability belongs to a product unless the article makes that relationship clear. Each object has `product` (the product name) and `feature` (the feature name), following the naming rules above. Return an empty array when no product features are covered.

{{TIMELINE_RULES}}

## Output

Return a JSON object that follows the supplied JSON schema: `products` holds the products, `product_features` holds the product-feature pairs whether or not they have dates, and `timeline_events` holds the dated events and is an empty array when the article states none.
