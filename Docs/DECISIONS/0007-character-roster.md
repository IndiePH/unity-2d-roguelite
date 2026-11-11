Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Character roster

A run has multiple characters to choose from. Each character has different talents and a different playstyle, and their own spritesheet. A player chooses one.

The reference shape is The Spell Brigade (https://store.steampowered.com/app/2904000/The_Spell_Brigade/) and the Brotato study. The study is not this decision: [brotato.md](../REFERENCES/brotato.md).

The current body is still one prefab. `SampleScene` spawns one `CharacterSprite`, and that prefab uses the 100 HP profile. The 70 HP profile is an asset only. Selection is not built. Detail of that body: [character-sprite.md](../FEATURE_SPECS/character-sprite.md). The profiles: [character-profile.md](../FEATURE_SPECS/character-profile.md). Spawn: [character-spawn.md](../FEATURE_SPECS/character-spawn.md).

## Choice

The game has more than one character to choose from. Choosing one is part of starting a run. Each character is a profile asset. The profile holds that character's max hit points and a reference to that character's spritesheet asset. The sheet is one asset. Inside it, each clip is the frames in playback order plus the seconds each frame stays up: idle, move, dash, hit, attack, and death.

Two profiles exist. `Character100` has 100 hit points. `Character70` has 70. `CharacterSprite` uses `Character100`. `CharacterSpriteAnimation` plays the sheet on that profile. The frames that were on the prefab are on `Character100Sprites`. `Character70Sprites` has the same frame times and no frames yet.

The sheet asset owns those clips. The profile references that asset. A clip does not own the asset, and a clip is not its own asset. Hit lock, the blink, invulnerability, and hiding the sprite on death stay on the character. The capsule stays on the body.

This is separate from co-op. Four players in one arena can still share one character. [0002-shared-arena-coop.md](0002-shared-arena-coop.md) already expects replay from a wider pool of characters, weapons, and items. It does not say those characters are selected, or that each one has different talents and a different playstyle.

## Rejected

One character for every player, with other spritesheets as skins of that same style.

Reading the roster out of the 4-player mode. Co-op does not choose it.

Six frame lists and six durations kept on the component once a second sheet exists. One asset per clip. An asset stored inside the clip.

## Why

Spell Brigade and Brotato both send a player in as a character that changes the run. The change is talents and playstyle. Another sheet on the same talents would only change the drawing. The pool is stated here so the co-op decision is not asked to imply it.

A second character is a second sheet. The component changes which sheet it plays. The frames and the frame time travel together, so they are one clip inside that sheet.

## Still open

- What else a talent or playstyle includes beyond max hit points: stats, starting weapons, items, modifiers, and special rules.
- How many characters a first release needs.
- Whether two players in one match can pick the same character.
- When selection happens, and the screen that does it.
- Body fit. The current capsule is fitted to the current idle frame. A new sheet does not reuse that body until that fit is specified.

## Not in this decision

Selection is not part of the baseline. The profile data is [character-profile.md](../FEATURE_SPECS/character-profile.md). The selection screen has no feature spec until that work starts.
