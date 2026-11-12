Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-13

# Character profile

A character is a profile asset. The profile holds that character's max hit points, the visual scale, the shared body prefab, and the eight part sprites. The animator on that prefab is the drawing. Accepted in [0007-character-roster.md](../DECISIONS/0007-character-roster.md).

## Profiles

Five profiles live in `Assets/Settings/Characters`. `BloodAlchemist` has 100 hit points. `Warrior`, `Knight`, `Necromancer`, and `Templar` have 70. Each uses visual scale 0.2 and references `Assets/Art/Characters/Character.prefab`. Each has its own eight part sprites.

`Character.prefab` uses `Knight`. `Character.MaxHp` and `Character.Scale` read that profile. Nothing spends hit points. The scale sizes the drawing and both colliders. The colliders stay on the body. Detail: [character-sprite.md](character-sprite.md).

## Sprites

Assign Head, Body, Left Arm, Right Arm, Left Hand, Right Hand, Left Leg, and Right Leg on the profile. `CharacterAnimation` copies the assigned sprites onto the matching parts when the character enables. An empty slot keeps the sprite already on the prefab. A new character is a new profile with those eight sprites assigned. It uses the same prefab.

## Not in this spec

The selection screen, a second body, and any rule for spending hit points.
