# Project Context

Date: 2026-11-13.

## What the project is

Unity 6 (`6000.6.8f1`) URP 2D template named `unity-2d-roguelite`. The high concept is not settled. The run shape is [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md).

## Current gate

Baseline — in progress. EditMode covers spawn-point choice, the character profiles, and the spawner pool. The player is `Assets/Art/Characters/Character.prefab`. `Run.unity` spawns one of it on a random open point under `SpawnPoints`. `CharacterAnimation` plays Idle, Running, Hurt, Dying, and Sliding on that animator and applies the profile's part sprites. Five profiles share that prefab at scale 0.2. Blood Alchemist has 100 hit points. Warrior, Knight, Necromancer, and Templar have 70. The prefab uses Knight. Selection is not built: [0007-character-roster.md](DECISIONS/0007-character-roster.md). Early-access weapons, armor, accessories, and attributes are recorded and not built: [0008-gear-and-attributes.md](DECISIONS/0008-gear-and-attributes.md). The sprite moves by rigidbody velocity. The arena is the `Tilemap`. `CameraBounds` is 33 by 21, offset (0.5, 0.5), centered on cell (0, 0). `CameraFollow` keeps the character centered until the view would leave that box, and the feet stop on the barrier around it. The scene top of that barrier sits half a tile below the box. A `Hurt` trigger covers the head and meets no layer yet. The drawing is the `Visual` child. The profile scale sizes the drawing and both colliders. The profiles use 0.2. The parts draw at order 10. A standing dash follows facing. The dash stops after the Distance on the prefab, currently 2.5. Death tints the parts with the prefab's Death Color. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md), [character-profile.md](FEATURE_SPECS/character-profile.md), [character-spawn.md](FEATURE_SPECS/character-spawn.md), [spawner-pool.md](FEATURE_SPECS/spawner-pool.md), [camera-follow.md](FEATURE_SPECS/camera-follow.md).

## Next step

Walk into an edge of `CameraBounds`, including the top, where the barrier sits half a tile below the box. The character now starts on a spawn point. It stays centered until the view meets the box, then the camera stops and the feet stop. That walk is unplayed. The earlier push test used the previous box. The P hit and L death play check are still unplayed. Play Mode did spawn one character on an open point. The Game view was not watched.

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
