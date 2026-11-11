# Technical Design

The run shape, the per-player view, party scaling, the weapon ring, the character roster, and who may write gameplay state are settled. The high concept and the core loop are not. The player is the sprite spawned in `SampleScene`: [character-spawn.md](FEATURE_SPECS/character-spawn.md). Two profiles exist. Selection is not built: [0007-character-roster.md](DECISIONS/0007-character-roster.md).

## Settled facts

- UI: uGUI chosen; no Canvas in a scene yet. Package `com.unity.ugui`. Rule: `.cursor/rules/unity-ugui.mdc`.
- VCS: Git + `.gitignore`. Remote: GitHub (`https://github.com/IndiePH/unity-2d-roguelite.git`).
- Tasks: Trello board https://trello.com/b/wfh43UJ3/2d-roguelite (`To-Do` / `In-Progress` / `Done`, plus `References` for inspirations). Not a second design docs store.
- Docs: in-repo under `Docs/` only. Collab rules: [README.md](README.md).
- Input: new Input System (`activeInputHandler` 1). Package `com.unity.inputsystem` 1.20.1. Asset: `Assets/Settings/InputSystem_Actions.inputactions`.
- Component wiring: known references are `[SerializeField]` on the prefab. Decision: [0001-serialized-references.md](DECISIONS/0001-serialized-references.md). Rule: `.cursor/rules/unity-serialized-references.mdc`.
- Simulation: one local authority writes gameplay state on a fixed step. One motor writes a rigidbody. Runtime spawns go through a spawner and are addressed by id. Presentation listens. A later host would run that same simulation. Decision: [0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md).
- Events: the simulation raises one `EventChannel<T>` asset per fact another prefab must hear. Listeners share that asset. Same-prefab reads stay on the owner. Hit is `Assets/Settings/Events/CharacterHit.asset`. Death is `Assets/Settings/Events/CharacterDeath.asset`. Decision: [0004-event-channels.md](DECISIONS/0004-event-channels.md).
- Run length: `GameConfig.waveCount` on `Assets/Settings/GameConfig.asset`. Default 20. The last wave is that value. Decision: [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md).
- Assemblies: runtime scripts are `Game` (`Assets/Scripts/Game.asmdef`). It references Input System, LitMotion, and LitMotion.Extensions. EditMode tests are `Assets/Tests/EditMode`.

## Current owners

- Character: `Assets/Scripts/Character.cs` on the sprite prefab. Reads `MaxHp` from the assigned profile. Invulnerability, movement hold, and death. `Hit` raises `CharacterHit`. `Die` raises `CharacterDeath`. No damage. There is no capsule prefab. A `Hurt` child on layer `PlayerHurt` is the combat trigger. That layer meets nothing yet. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md), [character-profile.md](FEATURE_SPECS/character-profile.md).
- CharacterMovement: `Assets/Scripts/CharacterMovement.cs` on the sprite prefab. WASD sets `linearVelocity`. It still steps `transform.position` when no rigidbody is assigned. Both this and dash write velocity until the motor in [0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md). Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CharacterDash: `Assets/Scripts/CharacterDash.cs` on the sprite prefab. Left Shift velocity push, two stored uses. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CharacterProfile: `Assets/Scripts/CharacterProfile.cs`. Assets: `Assets/Settings/Characters/Character100.asset` (100 hit points) and `Character70.asset` (70). Each references its own `CharacterSpriteSheet`. The prefab uses `Character100`. Detail: [character-profile.md](FEATURE_SPECS/character-profile.md).
- CharacterSpriteSheet: `Assets/Scripts/CharacterSpriteSheet.cs`. `Character100Sprites` holds the frames that were on the prefab. `Character70Sprites` has the same frame times and no frames. Detail: [character-profile.md](FEATURE_SPECS/character-profile.md).
- CharacterSpriteAnimation: `Assets/Scripts/CharacterSpriteAnimation.cs` on the sprite prefab only. Plays the sheet on the assigned profile. The P hit test and the L death test. P calls `Character.Hit` after the hit clip starts. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CameraFollow: `Assets/Scripts/CameraFollow.cs` on the `MainCamera` prefab. Follows the assigned character in `LateUpdate` and keeps the view inside the assigned confinement collider. `SampleScene` leaves the target empty. `CharacterSpawner` assigns it at play. The box is 33 by 21, offset (0.5, 0.5). Detail: [camera-follow.md](FEATURE_SPECS/camera-follow.md).
- CameraBoundsBarrier: `Assets/Scripts/CameraBoundsBarrier.cs` on `CameraBounds`. Keeps a solid edge on the outside of that trigger box so the body cannot cross it. The top edge has an offset. Detail: [camera-follow.md](FEATURE_SPECS/camera-follow.md).
- CameraShake: `Assets/Scripts/CameraShake.cs` on the `MainCamera` prefab. Listens to `CharacterHit` and `CharacterDeath`. Hit and death profiles stay on this component. It runs after `CameraFollow` and offsets that pose. The `SampleScene` instance is named Main Camera. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- EventChannel: `Assets/Scripts/EventChannel.cs`, with `CharacterHitChannel` and `CharacterDeathChannel`. Assets: `Assets/Settings/Events/`. Decision: [0004-event-channels.md](DECISIONS/0004-event-channels.md).
- GameConfig: `Assets/Scripts/GameConfig.cs`. Asset: `Assets/Settings/GameConfig.asset`. Wave count only. The wave runner does not exist yet. Decision: [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md).
- Spawner: `Assets/Scripts/Spawner.cs`. The only `Instantiate`. A caller passes a prefab, a pose, and an owner, and receives an id. `Release` enqueues that instance. The next rent dequeues the one that has waited longest. The id is not reused. Detail: [spawner-pool.md](FEATURE_SPECS/spawner-pool.md). Decision: [0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md).
- CharacterSpawner: `Assets/Scripts/CharacterSpawner.cs` on `Spawner` in `SampleScene`. Spawns one `CharacterSprite` on a random open `SpawnPoint`, then assigns that transform to `CameraFollow`. Detail: [character-spawn.md](FEATURE_SPECS/character-spawn.md).
- SpawnPoint: `Assets/Scripts/SpawnPoint.cs` on `SpawnPoint01` through `SpawnPoint04`. One occupant. An inactive or disabled point is not a choice.

## Absent

- Motor, wave runner, weapon ring, enemy spawn, loot spawn, and any network code. The boundaries are the accepted decisions. Those objects are not built.

## Active children

- [0001-serialized-references.md](DECISIONS/0001-serialized-references.md) — accepted. Inspector references for known components and assets.
- [0004-event-channels.md](DECISIONS/0004-event-channels.md) — accepted. One asset per fact. Listeners share it. Same-prefab reads stay on the owner.
- [character-sprite.md](FEATURE_SPECS/character-sprite.md) — in progress. Sprite movement, dash, hit test, and death test. The tilemap does not block the body. The arena edge does.
- [character-profile.md](FEATURE_SPECS/character-profile.md) — in progress. Two profiles, 100 and 70 hit points, each with a spritesheet.
- [character-spawn.md](FEATURE_SPECS/character-spawn.md) — in progress. One character on a random open spawn point.
- [spawner-pool.md](FEATURE_SPECS/spawner-pool.md) — in progress. One queue inside `Spawner`. A returned instance keeps no id.
- [hovering-weapons.md](FEATURE_SPECS/hovering-weapons.md) — accepted design, not built. Detail for [0006-hovering-weapons.md](DECISIONS/0006-hovering-weapons.md).
- [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md) — accepted mode. Per-player camera, character centered until the arena edge. Wave count is config, default 20.
- [camera-follow.md](FEATURE_SPECS/camera-follow.md) — in progress. Follow, confinement, and the body edge.
- [0003-party-scaling.md](DECISIONS/0003-party-scaling.md) — accepted starting balance. Not a wave-difficulty curve.
- [0005-simulation-authority.md](DECISIONS/0005-simulation-authority.md) — accepted. Single writer, spawner, host later.
- [0006-hovering-weapons.md](DECISIONS/0006-hovering-weapons.md) — accepted. Ring, and a profile per weapon type.
- [0007-character-roster.md](DECISIONS/0007-character-roster.md) — accepted. A chosen character has different talents and a different playstyle. Max hit points are the first field. The spritesheet is a separate asset.
