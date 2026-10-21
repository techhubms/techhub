#### timeline_events (array of objects)

Dated lifecycle events of a product or feature that the article itself states. They feed a timeline of when features were announced, released or retired, so accuracy matters more than completeness. Return an empty array when the article states no such event.

Each event has these fields:

- `kind`: one of
  - `announced`: a plan or roadmap item is announced for the future
  - `preview`: becomes available in public or private preview
  - `ga`: becomes generally available
  - `launch`: becomes available now, without a preview or GA label
  - `release`: a versioned release (e.g. a new SDK or runtime version)
  - `deprecation`: is declared deprecated
  - `retirement`: is shut down or removed
  - `end-of-support`: support ends
  - `breaking-change`: a change that breaks existing usage
  - `pricing-change`: pricing or licensing changes
  - `event`: a conference or other dated event
- `product`: official product name, with the same naming rules as `products`
- `feature`: official feature name within the product, or `null` when the event is about the whole product; no version numbers
- `description`: one line saying what happens
- `date_source`: `explicit` when the article states the date, or `publication` when the article says it happens now (for example "is now generally available" or "starting today")
- `date`: ISO date (`yyyy-MM-dd`) for `explicit`; `null` for `publication`. For a month use the first day of the month; for a quarter use the first day of the quarter.
- `date_precision`: `day`, `month` or `quarter`; use `day` for `publication`
- `quote`: the short sentence from the article that states the event, copied from the source

Rules:

- Only report events the article states as fact. Never infer a date from the article's publication date unless the article says the event happens now.
- Relative dates ("next week", "in Q3") count only when the article gives enough information to determine the date; otherwise skip the event.
- Report events about Microsoft and GitHub products, and about the open-source ecosystem around them. Skip events that are only about third-party products.
- Do not report events from opinion pieces, tutorials or retrospectives unless they announce something new.
- Report at most 10 events, the most important first. One event per product, feature and kind.
