# Technical Design

No gameplay architecture is settled. The sprite character that exists is specified in [character-sprite.md](FEATURE_SPECS/character-sprite.md).

## Settled facts

- UI: uGUI chosen; no Canvas in a scene yet. Package `com.unity.ugui`. Rule: `.cursor/rules/unity-ugui.mdc`.
- VCS: Git + `.gitignore`. Remote: GitHub (`https://github.com/IndiePH/unity-2d-roguelite.git`).
- Tasks: Trello board https://trello.com/b/wfh43UJ3/2d-roguelite (`To-Do` / `In-Progress` / `Done`, plus `References` for inspirations). Not a second design docs store.
- Docs: in-repo under `Docs/` only. Collab rules: [README.md](README.md).
- Input: new Input System (`activeInputHandler` 1). Package `com.unity.inputsystem` 1.20.1. Asset: `Assets/Settings/InputSystem_Actions.inputactions`.
- Component wiring: known references are `[SerializeField]` on the prefab. Decision: [0001-serialized-references.md](DECISIONS/0001-serialized-references.md). Rule: `.cursor/rules/unity-serialized-references.mdc`.

## Current owners

- Character: `Assets/Scripts/Character.cs` on both character prefabs. Invulnerability, movement hold, and death. No damage. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CharacterMovement: `Assets/Scripts/CharacterMovement.cs` on both prefabs. WASD. The capsule writes its transform. The sprite sets `linearVelocity`. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CharacterDash: `Assets/Scripts/CharacterDash.cs` on the sprite prefab only. Left Shift velocity push, two stored uses. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).
- CharacterSpriteAnimation: `Assets/Scripts/CharacterSpriteAnimation.cs` on the sprite prefab only. Frame lists, the P hit test, and the L death test. Detail: [character-sprite.md](FEATURE_SPECS/character-sprite.md).

## Absent

- `*.asmdef`, `Assets/Tests`, EditMode assembly, gameplay rules.

## Active children

- [0001-serialized-references.md](DECISIONS/0001-serialized-references.md) — accepted. Inspector references for known components and assets.
- [character-sprite.md](FEATURE_SPECS/character-sprite.md) — in progress. Sprite movement, dash, hit test, death test, and the tilemap collider.
- [0002-shared-arena-coop.md](DECISIONS/0002-shared-arena-coop.md) — proposed. Not architecture.
- [0003-party-scaling.md](DECISIONS/0003-party-scaling.md) — proposed balance. Not architecture.
