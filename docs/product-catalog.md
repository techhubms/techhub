# Product Catalog

The AI returns product and feature names as free text. The catalog is a curated layer on top: it links those names to one product (and one feature), so "Azure AI Foundry" and "Microsoft Foundry" are the same product wherever they appear. This page explains what a product, a child product and a feature are, how to tell them apart, and how names are matched.

## Overview

| Table | Purpose |
|---|---|
| `products` | The curated products, unique ignoring case. A product can have a parent (`parent_product_id`) |
| `product_aliases` | Other names for a product (old names, short forms), unique ignoring case and never equal to a product name |
| `product_features` | The features of a product, unique per product. A feature has an optional description and release date |
| `product_feature_aliases` | Other names for a feature |
| `product_feature_content` | Videos and posts that belong to a feature (one can be the thumbnail) |
| `ignored_product_names` | Names the AI returns that are not products (an event, a program), so they stay out of the review queue |
| `content_item_products` | The product names an article lists in classifier order, with the product each one matches. `product_id` is empty while the name is not in the catalog |
| `content_timeline_events.product_id` and `feature_id` | The product and feature of an event. The raw `product` and `feature` text stays on the event |

An article is about several products, and a product has many articles. An event belongs to one article and one product (and optionally one feature), so the dates of a product always lead back to the article that states them.

## Products, Child Products and Features

### The question

> **Can you use, buy, install or deploy it by itself, or do you only get it as part of something else?**

- If you can use it by itself, it is a **product**.
- If you only get it through another product, it is a **feature** of that product.
- If it is a product that belongs to a family of products, it is a **child product** of the family's product.

### Heuristics

Ask the question, then check these signals. A single signal is a hint; several together decide.

| Signal | Points to a product | Points to a feature |
|---|---|---|
| **Use** | You can use it without the product it appears under | You can only use it through that product |
| **Purchase** | It has its own price, SKU, license or subscription | It is included in, or a switch of, a product that you buy |
| **Install or deploy** | It is installed or provisioned on its own (a service, an app, a CLI, an extension) | It is turned on, configured or selected inside a product |
| **Sentence test** | "My pipeline is built with *X*" makes sense | "*X* is one of the things *Y* can do" makes sense |
| **Documentation** | It has its own documentation, versions and lifecycle | It is a page or a release-note entry of a product |
| **Removal test** | Removing the parent product leaves it working | Removing the parent product removes it |

**When it is unclear, choose the feature.** A feature never splits the articles about a product, while a wrong product pollutes the product list with names that nobody can use by itself. The model is told the same thing ("treat it as a feature of the product it belongs to and name that product"), and the catalog decides afterwards.

### Examples

| Name | Kind | Why |
|---|---|---|
| GitHub Actions | Child product of GitHub | You use it without the rest of GitHub's tools: your pipeline is built with it |
| GitHub CLI, GitHub Codespaces | Child products of GitHub | Installed or provisioned and billed on their own |
| GitHub Copilot app | Child product of GitHub Copilot | A separate application that can be used on its own |
| GitHub-hosted runners | Feature of GitHub Actions | You do not say that a pipeline is built with hosted runners; you build it with GitHub Actions and configure where it runs |
| GitHub Enterprise Importer | Child product of GitHub Enterprise Cloud | A standalone migration tool for importing into GitHub Enterprise Cloud |
| Secret scanning, code scanning | Features of the security product that contains them | Nobody buys secret scanning: they buy GitHub Advanced Security (or its parts GitHub Secret Protection and GitHub Code Security), and secret scanning is one of its features |
| GitHub Copilot CLI | Child product of GitHub Copilot | A separate application that you install and start |
| GitHub Copilot Fusion | Feature of GitHub Copilot | Not bought: you have a GitHub Copilot license and select Fusion when you use Copilot |
| Agent mode, code review | Features of GitHub Copilot | Capabilities that exist inside the product |
| Data Warehouse, Eventhouse, OneLake | Features of Microsoft Fabric | Workloads of Fabric: you buy Fabric capacity and use them inside it |
| Azure Monitor Logs, Azure Monitor Agent | Features of Azure Monitor | Parts of Azure Monitor that nobody runs without it |
| Application Insights | Child product of Azure Monitor | Its own service with its own resource and billing, though part of the monitoring family |
| BitLocker, Windows Hello | Features of Microsoft Windows | Parts of the operating system |
| Premium requests, token-based billing | Features of GitHub Copilot | Not capabilities, but a lot of articles are about them, and they only exist inside the product |
| Azure Functions | Child product of Microsoft Azure | Its own service, deployed on its own |

### Child products

A product gets a **parent** when it is a separate product of the same family, so a parent can show the articles and dates of everything in its family. A parent can have several levels (Microsoft Azure > Azure DevOps > Azure Pipelines).

- A child product follows the same question: it can be used by itself. What it shares with its parent is the family, not the usage.
- Everything that is or was called Azure is part of Microsoft Azure, also when it has been renamed (Microsoft Entra ID, Microsoft Foundry, Microsoft Sentinel, Azure Local).
- A product is not a child of a family only because the names look alike. Microsoft Defender for Cloud stays under Microsoft Defender although it was once Azure Security Center: it is a security product, not a developer tool.
- A parent is optional. A top-level product (Visual Studio Code, PostgreSQL) simply has no parent.
- A product cannot be its own ancestor. Deleting a parent makes its children top-level, and merging a product moves its children and features to the target.

Articles and events keep linking to the most specific product that matches their name. A parent shows the articles and dates of all its descendants, with the specific product named per link, and the counts on the product list include them.

### Features

A feature belongs to one product and also applies to that product's children. A date can be about a feature ("Agent mode becomes generally available"), and a feature can have a description, a release date and linked content (the videos and posts about it).

- A feature is one capability in the vendor's words, as short as the name allows ("Agent mode", not "Refreshed agent mode experience").
- Features that describe the same capability are one feature with aliases: "Agent mode" and "agent-mode".
- A feature name that the AI returns where a product is expected still links to the product of the feature: an article about "GitHub-hosted runners" links to GitHub Actions.
- Billing concepts (premium requests, AI credits, token-based billing) are features: they are no capability, but many articles are about them.

### What is neither

These are not catalogued, because they are rarely reported, written in many spellings, and nothing queries by them. When they matter for an event, the model writes them in the description of the event ("available in Visual Studio Code for Pro and Business").

- **Plans and editions** (Free, Pro, Business, Enterprise, Plan 2). The phrase matching still links "GitHub Copilot Business" to GitHub Copilot.
- **Surfaces**: the IDE, web site or mobile app where something runs.
- **Models, regions, language or runtime versions, plain price changes and policies.** A feature text of `null` means the event is about the whole product.
- **Events and programs** (conferences, security labs, funds): add them to the ignored names so they stay out of the review queue.
- **Other vendors' products**, unless an article is mainly about one.

### Converting a product into a feature

When a product turns out to be a capability, **Convert to feature** (`convert_product_to_feature`, on the product page for a child product) turns it into a feature of its parent. The feature is named like the product without the parent's name ("Coding agent"). The name and aliases of the product become aliases of the **feature**, never of a product, so products and features stay separate. Its children and features move to the parent. Nothing becomes unmatched, because a product name that matches no product is looked up as a feature name or alias.

## Names

- **Official, full and current name** with the vendor prefix the vendor uses, without acronyms ("Azure Kubernetes Service", not "AKS"). When the article gives an old and a new name, the new one is used; when it only gives one, that one is written as it is.
- **The most specific product wins** over its family ("Azure Container Apps", not "Microsoft Azure"), as long as it can be used by itself. Something you only get through a platform is a feature, and the platform is the product.
- **Aliases** hold the old names, short forms and spelling variants. A rename keeps the old name as an alias, and a merge turns the merged product and its aliases into aliases of the target.
- **Ignored names** are names that are not products. They never reach the review queue.

## How the AI Is Asked

The AI never receives the catalog: it is long, it changes, and the model would anchor on it. The prompts explain what the catalog expects, and the matching happens afterwards in the database. The rules live in one shared resource, `content-catalog-rules.md`, inserted into the classify prompt and the timeline and product extraction prompt (`{{CATALOG_RULES}}`), so both always agree. The rules describe what to do and give a few examples. They never list the names of the catalog, because such a list would have to follow the catalog.

Raising `ITimelineEventRepository.CurrentPromptVersion` makes the timeline backfill scan older articles again (see [content-processing.md](content-processing.md#timeline-backfill)). Changing the rules is a prompt-version change.

## Matching

Matching is a text match in the database, in tiers, and never uses AI. No name of a product or feature is written in code or in a prompt: the tiers use the words of the catalog as it is today, so a product or alias added in the admin is matched at once. Each tier forgives more than the one before.

1. **Exact** (`find_product_id`): the name matches a product by name or alias, ignoring case and extra whitespace. A name that matches no product is looked up as a feature name or alias.
2. **Normalized** (`find_product_id_normalized`, `catalog_match_key`): the name without parentheses ("(AKS)"), punctuation, a trailing "Service" and an added vendor prefix "Microsoft" on the AI's name. The prefix is never dropped from catalog names, so "Copilot" does not become "Microsoft Copilot".
3. **Phrase** (`find_product_id_by_phrase`):
   - The name without a trailing release version ("Windows Server 2022").
   - **Words missing at the start**: the name is the end of a name or alias, the part without the vendor ("Surface Laptop Ultra" of "Microsoft Surface Laptop Ultra"). "Active Directory" is not the start of "Microsoft Active Directory Certificate Services".
   - **Extra words**: a name or alias is a phrase inside the name and covers at least 60% of its words ("Microsoft Fabric Eventstream" links to Microsoft Fabric).
   - It only links when one product fits, or when one of the products is the parent of all the others ("Fabric" fits Microsoft Fabric and its children, so it links to Microsoft Fabric). A word that fits several unrelated products ("Copilot", "Server") stays unmatched.
4. **Events** (`resolve_event_catalog`): the product of an event is resolved as above. When the feature text matches no feature by name, the product text and the feature text are flattened into one text ("GitHub Copilot" + "Copilot Fusion" = "github copilot fusion"). The longest name or alias that occurs in it as a whole phrase, inside the product, its child products and the features of the product and its parents, wins. A child product replaces the product of the event, a feature is the feature, and the words after a child product may name one of its features. The phrase must contain a word of the feature text, a name of one word has to cover half of the feature text, and two equally long candidates are ambiguous. This is how "Copilot Chat experience" links to the feature inside it, and how a feature text that is really a child product becomes an event of that product, even when the AI puts a name at the wrong level. The words have to be next to each other: "foundry" and "agent" in different places are not "Foundry Agent".

There is no similarity scoring. A name that fits none of the tiers stays unmatched and goes to the review queue, where one click makes it an alias or a product and the matcher never needs to guess it again.

## Links

- **Links follow the AI output.** Whenever the `ai_metadata` of an article is written (processing, roundup metadata backfill, admin edit) its links are replaced, and event writes set `product_id` and `feature_id` the same way.
- **Catalog changes relink everything.** Adding a product or alias, renaming, merging, converting and deleting re-evaluate all links in the same transaction, so nothing becomes unmatched by accident.
- **Readers.** The roundup uses the canonical names (grouping and ordering by product, "Dates to know") and falls back to the AI's text for unmatched names. The admin products page shows the articles and dates per product.

## Review Queue

Names that match nothing and are not ignored are the products to review, on the Reviews page (`/admin/reviews`): add one as a product, add it as an alias of a product, or mark it as not a product. Feature names of the dates of a product (and its children) that match no feature are listed on the Features tab of the product: add one as a feature or as an alias of a feature. The queue is derived from the links, so unmatched names never pollute the catalog. See [admin.md](admin.md#product-catalog) for the admin pages and endpoints.

## Starting Catalog

One migration, `050_product_catalog.sql`, builds the whole catalog:

- the tables and the matching functions;
- about 340 products with their parents and aliases, chosen by the question above;
- the features of those products and their aliases;
- the names that are not products;
- the features of the old GitHub Copilot features page. The page was replaced by the timeline of the section (the old address redirects to it): every feature with a release date and linked content becomes one event per linked item (a `launch` on the release date, a first of a month is a month), stored with `extracted_from = 'catalog'` so the AI extraction and the edit of an article leave it alone. Features that describe the same capability are merged (Chat in IDE and Chat on Mobile are *Copilot Chat*), what is really a product (GitHub Spark, Copilot Workspace) becomes an event of that product, and the old tables are dropped;
- the links of the existing articles and events.

A parent is only set while a product has none, names that exist are skipped, and the script can run again without changing anything. Until the catalog is released, a change to the catalog is a change to that file, tested on a restored copy of the production database (see [database.md](database.md#migrations)).

Mainstream generic products must be in the catalog, because the phrase tier only accepts a name that is the end of a longer name or alias.

## Checklist for a New Name

1. Is it a program, an event, a plan, an IDE or another vendor's product? Add it to the ignored names, or leave it in the description.
2. Ask the question: can it be used, bought or deployed by itself?
   - **No:** add it as a feature (or as an alias of a feature) of the product it belongs to.
   - **Yes:** add it as a product. Give it a parent when it belongs to a family.
3. Is it another name for something that exists (an old name, a short form)? Add an alias instead.
4. Does it start with, or was it once called, "Azure"? Its parent is Microsoft Azure.

## Related

- [content-processing.md](content-processing.md): how articles are classified and how events are extracted.
- [admin.md](admin.md#product-catalog): the product pages and the review queue.
- [database.md](database.md): the tables and the migration policy.
