The project documentation root is `Docs/` in this repository.

Documentation for this title is kept in the game repo so collaborators share one source of truth. Do not maintain a parallel docs tree for this project in a separate docs-only repository. Docs conventions live in `Docs/README.md` (do not duplicate them here).

| Path | Contents | When to read |
| --- | --- | --- |
| `Docs/README.md` | Docs root, collab conventions, authority order, read path | When changing docs, or when authority/read path is unclear |
| `Docs/PROJECT_CONTEXT.md` | Current state, next step, Trello board URL | Every relevant session start |
| `.cursor/rules/*.mdc` | Reusable instructions and scoped coding constraints | Automatically, according to rule configuration |
| `AGENTS.md` | Project-wide agent instructions and documentation index | Every relevant agent session |
| `Docs/GDD.md` | Game design summary + links | When designing or implementing gameplay |
| `Docs/TECHNICAL_DESIGN.md` | Architecture summary + links | When designing or changing systems |
| `Docs/ROADMAP.md` | Milestones and gate status | When planning or choosing work |
| `Docs/TEST_STRATEGY.md` | Testing requirements and definition of done | When implementing or verifying features |
| `Docs/FEATURE_SPECS/*.md` | Owned feature detail | When working on that feature |
| `Docs/DECISIONS/*.md` | Architectural/design decisions | When making or checking related decisions |
| `Docs/CHANGELOG.md` | Shipped player-facing changes | When recording a release or verifying what shipped |
| `.cursor/rules/unity-ugui.mdc` | uGUI Canvas standards | When authoring or editing uGUI |

# Project Instructions

## Before Working
- Inspect the existing Unity project and relevant code before proposing changes.
- Read Docs/PROJECT_CONTEXT.md for the current project state.
- Default docs read path: PROJECT_CONTEXT → thin mother section → one matching child. Do not load all of Docs/ by default; do not create parallel agent-only summary docs.
- Before proposing gameplay or architecture, read matching `Docs/DECISIONS/` entries and any `Status: in-progress` feature spec. Do not reopen an `accepted` decision without new facts.
- Read Docs/GDD.md when a task affects gameplay or game design.
- Read Docs/TECHNICAL_DESIGN.md when a task affects architecture or system boundaries.
- Read the relevant feature specification before implementing a substantial feature.
- Read Docs/README.md when changing docs, or when authority/read path is unclear.

## Implementation
- Follow the applicable rules in .cursor/rules/.
- Prefer incremental changes over broad rewrites.
- Do not invent gameplay requirements or silently change approved decisions.
- Do not modify unrelated systems or assets without justification.
- Ask for clarification when a missing decision materially affects the implementation.

## Verification
- Follow Docs/TEST_STRATEGY.md.
- Stay inside the current ROADMAP.md gate. Do not start the next gate until its exit criteria are met.
- A passing EditMode suite does not mean a scene flow was played. A class existing does not mean the feature works.
- Report what was changed, what was tested, and what remains unverified.
- Never claim that Unity runtime behavior was verified unless it was actually tested.

## Documentation
- Keep all project documentation under `Docs/` in this repository. Follow Docs/README.md.
- Authority order: accepted decision > feature spec > mother summary. Fix the loser in the same change.
- Child docs hold detail; mother updates stay index-sized. New decisions use required frontmatter, then update the mother summary.
- Keep ROADMAP.md and PROJECT_CONTEXT.md consistent with actual progress.
