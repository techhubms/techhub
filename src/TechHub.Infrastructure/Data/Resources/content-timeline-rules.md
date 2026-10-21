#### timeline_events (array of objects)

Dated lifecycle events of a product or feature that the article itself states. They feed two things: a timeline of the milestones of a product (when it was announced, released, changed fundamentally or retired), and a "dates to know" list in weekly roundups with upcoming deadlines, retirements and launches. Accuracy matters more than completeness. Return an empty array when the article states no such event, which is the normal outcome for most articles.

What deserves an event is a milestone: something a reader would still want to find on a timeline a year from now, or would need to plan around. Typical milestones are a new product, a new model, a capability that changes what the product can do, a preview or general availability of something people wait for, a deprecation, a retirement, a breaking change, a pricing change and a conference. Day-to-day improvements are not milestones, even when the article calls them "new": quality-of-life changes, settings, UI tweaks, an existing feature that also handles one more case, speed-ups and bug fixes. When unsure, ask whether you would mention it when telling a colleague what changed in this product this year; if not, leave it out.

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
- Judge every change on its own merit, not on the importance of the post that contains it. Release notes and monthly summaries bundle many changes, most of which are day-to-day improvements, so such a post often has one milestone or none. A long list of changes is not a reason to report many events
- Retrospectives and "what's new so far" overviews list changes from many earlier months: do not report events from them unless the post itself announces the event now
- One sentence that covers several products or models (for example two new models announced together) gives one event per product
- Order events by importance. One event per product, feature and kind.
