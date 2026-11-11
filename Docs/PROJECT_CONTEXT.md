# Project Context

Date: 2026-11-11.

## What the project is

Unity 6 (`6000.6.8f1`) URP 2D template named `unity-2d-roguelite`. The high concept is not settled. The run shape is [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md).

## Current gate

Baseline — in progress. No project test assembly. The player is `CharacterSprite`, the only character prefab and the only character in `SampleScene`. The sprite moves by rigidbody velocity against `Grid/Mid`. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).

## Next step

Press P and confirm the camera kicks and settles back to rest, then press L and confirm the longer shake, the death frames once, the sprite hides, the body stops, and WASD, Shift, and P do nothing afterward. Hide Sprite On Death defaults on. The wall stop and the hit blink are still unplayed. `MainCamera` already listens to `CharacterHit` and `CharacterDeath`.

## Task board

Trello (workspace INDIE): https://trello.com/b/wfh43UJ3/2d-roguelite  
Lists: `To-Do` → `In-Progress` → `Done`, plus `References` for inspirations. Tasks live here; design truth stays in `Docs/`.

## Read next

| Path | Why |
| --- | --- |
| [README.md](README.md) | Collab rules, authority order, read path |
| [ROADMAP.md](ROADMAP.md) | Gates |
| [GDD.md](GDD.md) | Design status |
| [REFERENCES/brotato.md](REFERENCES/brotato.md) | Brotato study. Not design authority. |
| [TECHNICAL_DESIGN.md](TECHNICAL_DESIGN.md) | Settled platform facts |
| [DECISIONS/0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md) | Who may write gameplay state |
| [DECISIONS/0004-event-channels.md](DECISIONS/0004-event-channels.md) | How another prefab hears a committed fact |
| [../AGENTS.md](../AGENTS.md) | Agent index |
