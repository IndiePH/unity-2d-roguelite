# Project documentation

This folder is the documentation root for **unity-2d-roguelite**.

It lives in the **game repository** so collaborators who clone this repo get the same design, architecture, and roadmap files. Do not keep a parallel docs tree for this title in a separate docs-only repository.

## Layout

| Path | Role |
| --- | --- |
| [GDD.md](GDD.md) | Mother doc: player-facing design summary + links |
| [TECHNICAL_DESIGN.md](TECHNICAL_DESIGN.md) | Mother doc: architecture summary, owners, settled platform facts + links |
| [ROADMAP.md](ROADMAP.md) | Mother doc: gates and links to active work |
| [TEST_STRATEGY.md](TEST_STRATEGY.md) | Definition of done |
| [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md) | Mother doc: current focus; keep tiny |
| [FEATURE_SPECS/](FEATURE_SPECS/) | Child docs: owned feature detail |
| [DECISIONS/](DECISIONS/) | Child docs: accepted / proposed / superseded choices |
| [REFERENCES/brotato.md](REFERENCES/brotato.md) | Brotato study. Not a feature spec and not a decision. |
| [CHANGELOG.md](CHANGELOG.md) | Shipped player-facing changes only |

Agent index and workflow instructions: [../AGENTS.md](../AGENTS.md).

## Collab model

Heavy detail lives in **child docs** (feature specs and decisions). **Mother docs** stay thin: short summary plus links. That keeps concurrent edits from colliding on large files.

- One `Owner` per feature spec. Others propose changes; they do not silently co-edit the same heavy file.
- Mother updates should be index-sized: status line, summary sentence, and link.
- Cross-cutting topics (multiplayer, meta save, Steam, DLC) use a decision and a short mother section, not only a buried feature note.
- Prefer short-lived branches and small docs PRs over long-lived rewrites of half of `Docs/`.
- Task tracking is Trello (board URL in [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md)). Cards may link to docs; they do not replace `Docs/` or GitHub PRs.

## Authority order

When two docs disagree, fix the loser in the same change that notices the conflict:

1. `accepted` decision in `DECISIONS/`
2. Feature spec
3. Mother summary (`GDD.md`, `TECHNICAL_DESIGN.md`, `ROADMAP.md`, `PROJECT_CONTEXT.md`)

## Status (stable paths)

Do **not** move files between todo / in-progress / done folders. Keep the path stable and set status in frontmatter.

### Feature specs

Required at top of each `FEATURE_SPECS/*.md` (except this README):

```markdown
Status: proposed | accepted | in-progress | done | superseded
Owner: name
Last updated: YYYY-MM-DD
```

No status → treat as draft, not authority. `accepted` on a spec means the design is locked. Implementation may not have started.

Create a feature spec when work starts or a design choice would change implementation. Do not pre-create empty specs for every future idea. Unowned ideas stay as ROADMAP bullets until someone owns a spec.

File names: `kebab-case.md` (example: `shared-run-session.md`).

### Decisions

Required at top of each `DECISIONS/*.md` (except this README):

```markdown
Status: proposed | accepted | superseded
Owner: name
Last updated: YYYY-MM-DD
Supersedes: (optional)
Superseded by: (optional)
```

Body must record: the choice, the alternative rejected, and why. Then update the relevant mother summary in `TECHNICAL_DESIGN.md` and/or `GDD.md`.

File names: `NNNN-kebab-case.md` (example: `0001-host-as-player-mvp.md`).

## Mother-doc size limit

If a section in `GDD.md`, `TECHNICAL_DESIGN.md`, `ROADMAP.md`, or `PROJECT_CONTEXT.md` needs more than about 10–15 lines of detail, move the detail to a child doc and leave a pointer.

## Read path and token guardrails

- Do not create parallel agent-only summary docs that mirror `GDD.md`, feature specs, or decisions.
- Default read path: `PROJECT_CONTEXT.md` → thin mother section → one matching child doc. Do not load all of `Docs/` by default.
- Child docs stay precise (player action, rules, acceptance criteria intact). Do not compress them into telegram-speak to save tokens.
