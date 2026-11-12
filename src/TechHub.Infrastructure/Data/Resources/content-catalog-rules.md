#### Naming products and features

Tech Hub keeps a catalog of Microsoft and GitHub products, with their features. You do not receive the catalog. A program compares your names with it afterwards, without asking you again, so write every name the way the vendor's own documentation would: complete, official and current. A shortened or invented name, or one with extra words around it, is harder to match.

Two kinds of things. Put each in its own field and do not mix them. The test: **can you use, buy, install or deploy it by itself, or do you only get it as part of something else?**

- **Product**: you can use, buy, install, provision or subscribe to it by itself, and it has its own documentation, versions or lifecycle. A product can belong to a family and still be a product: "GitHub Actions", "GitHub CLI" and "GitHub Codespaces" each work without the others. Other examples: "Azure Functions", "Visual Studio Code".
- **Feature**: a named capability, mode, part of a bundle or billing concept that you only get as part of one product, and that shows up in its release notes. Nobody buys secret scanning: they buy a security product, and secret scanning is one of its features. A pipeline is not built with hosted runners but with GitHub Actions, so hosted runners are a feature of it. Something you only choose or switch on inside a product (a routing mode, an experimental option) is a feature of it, not a product. Examples: "Agent mode", "Scheduled backups".

A plan or edition of a product ("Pro", "Enterprise", "Plan 2"), the place it runs (an IDE, a web site, a mobile app), a model, a region, a language or runtime version, a plain price change and a policy are neither. Mention them in the `description`, the `key_facts` or the `tags` instead: "generally available for Business and Enterprise plans in Visual Studio Code" belongs in the description of the event.

Rules for product names:

- Use the official, full and current name, with the vendor prefix the vendor uses: "GitHub Dependabot", not "Dependabot"; "Azure Kubernetes Service", not "Kubernetes Service". When the article gives an old and a new name, use the new one. When it only gives one, write that one as it is: never guess a rename.
- No acronyms, nicknames or product-family shorthand: "Azure Kubernetes Service", not "AKS"; "Visual Studio Code", not "VS Code".
- No plan, edition, surface, release version or feature in the name. "GitHub Copilot Business" is the product "GitHub Copilot" (the plan goes in the description); "GitHub Copilot in Visual Studio Code" is the product "GitHub Copilot" (the editor goes in the description). Put a version in `key_facts`. A number that is part of the product's own name stays.
- Name the most specific product the article is about. A tool or service of a platform that you can use by itself is a product of its own: "GitHub Dependabot", not "GitHub"; "Azure Container Apps", not "Microsoft Azure". Something you only get through the platform is a feature: name the platform and put the thing in the feature field. Use the platform name only when the article is about the platform as a whole.
- When you cannot tell whether a named thing can be used by itself, treat it as a feature of the product it belongs to and name that product. The catalog decides afterwards.
- List Microsoft and GitHub products and the open-source projects around them. List a third-party product only when the article is mainly about it.

Rules for feature names:

- One capability or concept in the words the vendor uses, as short as that name allows: a noun phrase of at most 5 words, without a version, a plan, an editor, a verb or a status ("generally available", "new", "preview"). Do not wrap descriptive words around the name: "Agent mode", not "Refreshed agent mode experience".
- Use the same name for the same thing every time, with the capitalization of the vendor's own documentation.
- In `product_features`, name the features the article substantively covers, whether or not it gives dates or describes a lifecycle event. Include only features clearly presented as part of the named product; do not infer a relationship from mere co-occurrence.
- In `timeline_events`, use `null` when the event is about the whole product, or when the thing is a model, region, version, a plain price change or a policy.
