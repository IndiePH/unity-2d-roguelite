# Game Design Document

## Status and date

Status: early template. The high concept and the core loop are not settled. The run shape is.
Date: 2026-11-11.

## Direction

The high concept and the core loop below are still not decided. These are accepted. A line marked open is not.

- Up to 4 players in one shared arena, waves, then a shop, each player with their own build. Each player has a camera that keeps their character at the center until the view would leave the arena, then the camera stops. The body stops at that same edge. The view is split. It does not zoom out to frame everyone. Accepted: [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md). Still open there: shops, revive, what fails the run, per-wave difficulty and timing, lens, and split layout.
- A run has `waveCount` waves, default 20, edited on `GameConfig`. The last wave is that number. Player-count scaling does not change with that total, and there is no per-wave difficulty formula yet.
- Enemy count and health scale with the players actually in the match. Accepted starting table: [0003-party-scaling.md](DECISIONS/0003-party-scaling.md). Still open: enemies already alive when someone joins or leaves.
- Equipped weapons hover on a ring and attack on their own. Each weapon has a profile. The type decides its stats and behavior. Accepted: [0006-hovering-weapons.md](DECISIONS/0006-hovering-weapons.md). Not built. Nothing applies damage yet.
- A run has a roster of characters. A player chooses one, and that character brings their own style, including their own spritesheet. That sheet is one asset of clips. Selection and the asset are not built. `SampleScene` still has one character. Accepted: [0007-character-roster.md](DECISIONS/0007-character-roster.md). The Spell Brigade and [brotato.md](REFERENCES/brotato.md) are the reference shape, not the definition of a character.
- The fight is a local simulation that alone writes gameplay state. Presentation listens. Across prefabs, the simulation raises an event channel after the fact is committed. Accepted: [0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md), [0004-event-channels.md](DECISIONS/0004-event-channels.md). Netcode is not part of the baseline.

## High concept

Not decided.

## Core loop

Not decided.

## MVP

Scene contents are in [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md). The core loop is not settled.

Smallest missing piece that would make a session playable: Not decided.

## Out of scope

Not recorded.

## Proposals

None. Direction above is the index. [brotato.md](REFERENCES/brotato.md) is a study, not a specification.

## Active children

- [character-sprite.md](FEATURE_SPECS/character-sprite.md) — in progress.
- [camera-follow.md](FEATURE_SPECS/camera-follow.md) — in progress. Centered follow. The body stops at the arena edge.
- [hovering-weapons.md](FEATURE_SPECS/hovering-weapons.md) — accepted design. Not built.
