---
date: 2026-04-20 16:43:57 +00:00
section_names:
- devops
tags:
- Commit Graph
- Config Based Hooks
- Core.hookspath
- DevOps
- Diff Algorithms
- G)
- Geometric Repacking
- Git
- Git 2.53
- Git 2.54
- Git Add  P
- Git Backfill
- Git Blame
- Git History
- Git Hooks
- Git Log  L
- Git Maintenance
- Git Rebase
- Git Replay
- GitHub Blog
- GPG Signatures
- History Rewriting
- HTTP 429 Retry
- Http.maxretries
- Http.retryafter
- Interactive Staging
- Multi Pack Index (midx)
- News
- Open Source
- Packfiles
- Partial Clone
- Pickaxe Search ( S
- Pre Commit Hooks
- Reflog
- Repository Maintenance
- Signed Commits
- Version Control
primary_section: devops
title: Highlights from Git 2.54
external_url: https://github.blog/open-source/git/highlights-from-git-2-54/
author: Taylor Blau
feed_name: The GitHub Blog
---

Taylor Blau summarizes Git 2.53 and 2.54 highlights, including a new experimental `git history` command for targeted history rewrites, config-based hooks, improved maintenance defaults, and a set of smaller updates across staging, replay, networking, and diff/log tooling.<!--excerpt_end-->

## Highlights from Git 2.54

Taylor Blau (GitHub) reviews notable changes across the Git 2.53 and 2.54 releases, based on work from 137+ contributors.

> Since the last Git release covered was 2.52, this post includes highlights from both Git 2.53 and 2.54.

## Rewrite history with `git history` (experimental)

Git 2.54 introduces an experimental command aimed at simpler history rewrites than an interactive rebase:

- `git history reword <commit>`
  - Opens an editor for the specified commit message and rewrites it in place.
  - Updates descendant branches.
  - Does **not** touch the working tree or index.
  - Can operate in a bare repository.

- `git history split <commit>`
  - Interactively splits a commit into two by selecting hunks to carve out into a new parent commit.
  - Uses an interface similar to `git add -p`.

```shell
$ git history split HEAD
diff --git a/bar b/bar
new file mode 100644
index 0000000..50810a5
--- /dev/null
+++ b/bar
@@ -0,0 +1 @@
+bar
(1/1) Stage addition [y,n,q,a,d,p,?]? y
```

Limitations (by design):

- Does not support histories containing merge commits.
- Refuses operations that would result in merge conflicts.
- Intended for targeted, non-open-ended rewrites (not a replacement for `git rebase -i`).

Implementation note:

- Built on top of `git replay` machinery, benefiting from the ability to operate without touching the working tree (useful for scripting/automation).

Sources:

- https://github.com/git/git/compare/3e0db84c88c57e70ac8be8c196dfa92c5d656fbc...d205234cb05a5e330c0f7f5b3ea764533a74d69e
- https://github.com/git/git/compare/aa95f87c740011f7d21555c5ad7f0870faf4b5c8...1278a26544e81dddf564fd7730890a7e023ed367
- https://github.com/git/git/compare/2eec0f51156ea872174bbd08f355155f381a568e...d1f33c753de68f63c945c3213f439081ed11c27b
- https://github.com/git/git/compare/dd33e738a469cb7841a4a6132bdce1809d0772aa...d563ecec2845467880f5742e178a9723afef495a
- https://github.com/git/git/compare/f1743ad69a492d1ca3773bfdddf7f5ffd278c19b...26b9946dd756a2efc29f898e53327676a22adc3e

## Config-based hooks

Historically, hooks lived as executable scripts under `$GIT_DIR/hooks` (or via `core.hooksPath`), which made sharing hooks across many repos awkward.

Git 2.54 adds the ability to define hooks in Git configuration files:

```ini
[hook "linter"]
event = pre-commit
command = ~/bin/linter --cpp20
```

Key points:

- `hook.<name>.event` chooses the hook event (for example, `pre-commit`).
- `hook.<name>.command` defines the command to run.
- Can be placed in `~/.gitconfig`, `/etc/gitconfig`, or repo-local config.
- Multiple hooks can run for the same event:

```ini
[hook "linter"]
event = pre-commit
command = ~/bin/linter --cpp20

[hook "no-leaks"]
event = pre-commit
command = ~/bin/leak-detector
```

Ordering and compatibility:

- Git runs hooks in the order encountered in configuration.
- Traditional hook scripts in `$GIT_DIR/hooks` still run and run **last**.

Discoverability and control:

- List configured hooks (and origin) with `git hook list`:

```shell
$ git hook list
pre-commit global linter ~/bin/linter --cpp20
pre-commit local no-leaks ~/bin/leak-detector
```

- Disable a hook without removing it:
  - `hook.<name>.enabled = false`

Internal modernization:

- Various built-in hooks have been migrated to use the new hook API.

Sources:

- https://github.com/git/git/compare/4aa72ea1f64e8ddcd1865c76b24591c0916c0b5d...005f3fbe07a20dd5f7dea57f6f46cd797387e56a
- https://github.com/git/git/compare/9a8aebae972de22ecd5adb92fec9d77147949c8a...ec1c4d974ac74afb4f0574d29f7bbb30c1c46431
- https://github.com/git/git/compare/4e5821732e684f21a35288d8e67f453ca2595083...5c58dbc887a1f3530cb29c995f63675beebb22e9

## Geometric repacking during maintenance by default

Git 2.52 introduced a `geometric` strategy for `git maintenance`, which incrementally repacks repositories based on whether packfiles can form a geometric progression (by object count).

Change in 2.54:

- `geometric` becomes the default for manual maintenance when running:
  - `git maintenance run`

Implications:

- More efficient maintenance out-of-the-box.
- Avoids expensive all-into-one repacks typical of `gc`.
- Falls back to a full `gc` only when it would consolidate the entire repository into a single pack.
- Keeps auxiliary structures updated (commit-graph, reflogs, etc.).

You can still choose the old behavior:

- `maintenance.strategy = gc`

Source:

- https://github.com/git/git/compare/1ebfc2171310ed5ca2bcd8c1255d45f03e56dda7...452b12c2e0fe7a18f9487f8a090ce46bef207177

## Other updates (selected)

- `git add -p` usability improvements
  - `J`/`K` navigation now shows whether each hunk was accepted or skipped.
  - New `--no-auto-advance` flag keeps the session on the same file after deciding on the last hunk, letting you move between files with `<` and `>`.
  - Sources:
    - https://github.com/git/git/compare/2b53e8b3ee7f143af785e2a39ce4e1614ff6c66e...8cafc305e22a59efb92472d4132616e24d3184c6
    - https://github.com/git/git/compare/f19f1b6cf37d22cf317b5c3b52a11eede1abe267...417b181f99ce53f50dea6541430cfe1f1f359a6a

- `git replay` continues to mature (experimental)
  - Atomic reference updates by default (instead of printing `update-ref` commands).
  - New `--revert` mode for reversing changes from a range of commits.
  - Can drop commits that become empty during replay.
  - Supports replaying down to the root commit.
  - Sources:
    - https://github.com/git/git/compare/7bf3785d0973d229fa21a76122c7e4735a2b1ffb...0ee71f4bd035db61342c2c5a25984e4545347c11
    - https://github.com/git/git/compare/05ddb9ee8a4c619fbb0e7309fe291bff5cd7c987...2760ee49834953c0860fa5d7983a6af4d27cb6a9
    - https://github.com/git/git/compare/03311dca7f91f69e9e0c532fce1c1e3c0a9fa34d...e8b79a96ebaa2113391d14bfcdabe239f6ff8611
    - https://github.com/git/git/compare/d8c553bbed21761a8af3fa40a20518e210e78a0d...23d83f8ddbef9adcb87671358b473e55cf90c90b

- HTTP transport: retry on HTTP 429
  - Git can now retry requests on 429 “Too Many Requests”.
  - Honors `Retry-After` header when present.
  - New config: `http.retryAfter`, `http.maxRetries`, `http.maxRetryTime`.
  - Source:
    - https://github.com/git/git/compare/270e10ad6dda3379ea0da7efd11e4fbf2cd7a325...640657ffd06999ec1ec3b1d030b7f5aac6b7f57b

- `git log -L` routed through standard diff machinery
  - Now compatible with patch formatting options and pickaxe searches (`-S`, `-G`) that were previously ignored.
  - Example shown: trace `strbuf_addstr()` history and filter to commits where `len` changed.
  - Source:
    - https://github.com/git/git/compare/fb5516997ef3f882d8e53ce70ba6077533683621...512536a09ea2964e93226f219898ee0a09d85a70

- Incremental multi-pack index (MIDX) compaction
  - Adds compaction to merge smaller MIDX layers (and reachability bitmaps), helping keep layer chains manageable.
  - Source:
    - https://github.com/git/git/compare/ce74208c2fa13943fffa58f168ac27a76d0eb789...d54da84bd9de09fc339accff553f1fc8a5539154

- `git status` enhanced branch comparison
  - New `status.compareBranches` option to compare against upstream and/or push remote.

```ini
[status]
compareBranches = @{upstream} @{push}
```

- Source:
  - https://github.com/git/git/compare/d79fff4a11a5...68791d7506aa

- `git rebase --trailer`
  - New `--trailer` option to append a trailer to every rebased commit via `interpret-trailers`.
  - Source:
    - https://github.com/git/git/compare/a7a079c2c4bc7b269229a6ea6c147b6b2d5b2684...e4f9d6b0ab2e1903765258991a6265599d0007ce

- Commit signing display improvement
  - Git now treats signatures made with since-expired GPG keys as good signatures (previously shown in scary red).
  - Source:
    - https://github.com/git/git/compare/9eb5b3b999cb89d4a09dcf1012784e74154026de...90695bbdaea86064398c26eb259043cadcf99a86

- `git blame --diff-algorithm`
  - Choose diff algorithm for blame computation (`histogram`, `patience`, `minimal`, etc.).
  - Source:
    - https://github.com/git/git/compare/716e871d50dc63a6f436442b127571b9268a75a3...ffffb987fcd3b3d6b88aceed87000ef4a5b6114e

- Object database (ODB) internal refactor
  - Refactored to a pluggable backend design (via function pointers per source), laying groundwork for alternative storage backends.
  - Note: one provided source URL in the input appears malformed (`https://ttps//...`).
  - Sources (as provided):
    - https://ttps//github.com/git/git/compare/2cc71917514657b93014134350864f4849edfc83...3565faf28c2059c6260d53ac71a303b1c04b0a7b
    - https://github.com/git/git/compare/d0413b31ddcce6ae6ffaff0a30a67ffbd1a7c648...d6fc6fe6f8b74e663d6013f830b535f50bfc1414
    - https://github.com/git/git/compare/2f8c3f6a5a6d6a3de205be709e1a598b9d4b0b3e...83869e15fa9ef3b0ea2adbfe2fe68a309f95b856
    - https://github.com/git/git/compare/2e3028a58c1f1fbf08538443fc30a48ac4f6bacf...109bcb7d1d2f0d2f0514beec15779190c0b89575

- `git backfill` improvements (partial clone)
  - Now accepts revision ranges and pathspecs (including wildcards), making it more practical for large partial clones.
  - Source:
    - https://github.com/git/git/compare/cd79c76a51f776bf46a849db04ce2cc45c5c5d6d...46d1f4cf4dcb8aaf799f78410af829e149086f36

- Alias names can now use non-ASCII characters (new subsection-based syntax)

```ini
[alias "hämta"]
command = fetch
```

- Traditional `[alias] co = checkout` still works for ASCII names.
- Source:
  - https://github.com/git/git/compare/ac78c5804e080aa8f0307155eda85465a2a1b1dd...edd8ad18a643d47dd92b08ab865bf7f4a26f50bc
  - https://github.com/git/git/compare/08c36099359e6a5c694f9abb97e630a247bc8dfb...73cc549559398626f33063f64ece9e558e654c95

- Histogram diff algorithm output quality fix
  - Improves compaction behavior so output isn’t visually redundant when change groups move across histogram anchors.
  - Source:
    - https://github.com/git/git/compare/7f13e5c8c744ec8da268b6f774d16f2ea729f48e...e417277ae99687b576e48cb477a7a50241ea0096

## Release notes

- Git 2.53 release notes: https://github.com/git/git/blob/v2.54.0/Documentation/RelNotes/2.53.0.adoc
- Git 2.54 release notes: https://github.com/git/git/blob/v2.54.0/Documentation/RelNotes/2.54.0.adoc
- More versions: https://github.com/git/git/tree/v2.54.0/Documentation/RelNotes
- Git repository: https://github.com/git/git


[Read the entire article](https://github.blog/open-source/git/highlights-from-git-2-54/)

