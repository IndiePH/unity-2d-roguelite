Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-13

# Character roster

A run has multiple characters to choose from. Each character has different talents and a different playstyle, and their own body. A player chooses one.

The reference shape is The Spell Brigade (https://store.steampowered.com/app/2904000/The_Spell_Brigade/) and the Brotato study. The study is not this decision: [brotato.md](../REFERENCES/brotato.md).

The current body is one prefab. `Run.unity` spawns `Assets/Art/Characters/Character.prefab`, and that prefab uses the Knight profile. The other profiles are assets only. Selection is not built. The body plays its animator: Idle, Running, Hurt, Dying, and Sliding. Every profile references that prefab and supplies its own part sprites. Detail of that body: [character-sprite.md](../FEATURE_SPECS/character-sprite.md). The profiles: [character-profile.md](../FEATURE_SPECS/character-profile.md). Spawn: [character-spawn.md](../FEATURE_SPECS/character-spawn.md).

## Choice

The game has more than one character to choose from. Choosing one is part of starting a run. Each character is a profile asset. The profile holds that character's max hit points, the visual scale, the body prefab, and the eight part sprites. The spawned body plays animator states. The scale sizes the drawing and both colliders. The colliders stay on the body. Every profile uses scale 0.2 and references `Assets/Art/Characters/Character.prefab`.

Five profiles exist. `BloodAlchemist` has 100 hit points. `Warrior`, `Knight`, `Necromancer`, and `Templar` have 70. The spawned prefab uses `Knight`. `CharacterAnimation` plays the animator on that prefab and applies the profile's part sprites.

Hit lock, the blink, invulnerability, and hiding the parts on death stay on the character. The capsule stays on the body.

This is separate from co-op. Four players in one arena can still share one character. [0002-shared-arena-coop.md](0002-shared-arena-coop.md) already expects replay from a wider pool of characters, weapons, and items. It does not say those characters are selected, or that each one has different talents and a different playstyle.

## Rejected

One character for every player, with other spritesheets as skins of that same style.

Reading the roster out of the 4-player mode. Co-op does not choose it.

Six frame lists and six durations kept on the component once a second sheet exists. One asset per clip. An asset stored inside the clip.

## Why

Spell Brigade and Brotato both send a player in as a character that changes the run. The change is talents and playstyle. Another sheet on the same talents would only change the drawing. The pool is stated here so the co-op decision is not asked to imply it.

A second character is a second profile. It references the shared body prefab and its own part sprites. A different skeleton would reference its own prefab.

## Still open

- What else a talent or playstyle includes beyond max hit points: stats, starting weapons, items, modifiers, and special rules.
- How many characters a first release needs.
- Whether two players in one match can pick the same character.
- When selection happens, and the screen that does it.
- A later character whose drawing does not share this rig sets its own visual scale. The colliders scale with that value.

## Not in this decision

Selection is not part of the baseline. The profile data is [character-profile.md](../FEATURE_SPECS/character-profile.md). The selection screen has no feature spec until that work starts.
