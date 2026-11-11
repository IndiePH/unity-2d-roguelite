Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Character roster

A run has a roster of characters. A player chooses one, and that character is the style they bring: how they play, and their own spritesheet.

The reference shape is The Spell Brigade (https://store.steampowered.com/app/2904000/The_Spell_Brigade/) and the Brotato study. The study is not this decision: [brotato.md](../REFERENCES/brotato.md).

The current body is still one prefab. `CharacterSprite` is the only character in `SampleScene`. Selection, a second character, and a second sheet are not built. Detail of that body: [character-sprite.md](../FEATURE_SPECS/character-sprite.md).

## Choice

The game has more than one character. Choosing one is part of starting a run. The choice is a style, not a new drawing of the same style. The spritesheet belongs to that character. The sheet is one asset. `CharacterSpriteAnimation` holds that one reference. Inside the asset, each clip is the frames in playback order plus the seconds each frame stays up: idle, move, dash, hit, attack, and death.

The asset owns those clips. A clip does not own the asset, and a clip is not its own asset. Hit lock, the blink, invulnerability, and hiding the sprite on death stay on the character. The capsule stays on the body.

The prefab still stores those lists on the component. The asset is not built.

This is separate from co-op. Four players in one arena can still share one character. [0002-shared-arena-coop.md](0002-shared-arena-coop.md) already expects replay from a wider pool of characters, weapons, and items. It does not say those characters are selected, or that each one plays differently.

## Rejected

One character for every player, with other spritesheets as skins of that same style.

Reading the roster out of the 4-player mode. Co-op does not choose it.

Six frame lists and six durations kept on the component once a second sheet exists. One asset per clip. An asset stored inside the clip.

## Why

Spell Brigade and Brotato both send a player in as a character that changes the run. Another sheet on one style would only change the drawing. The pool is stated here so the co-op decision is not asked to imply it.

A second character is a second sheet. The component changes which sheet it plays. The frames and the frame time travel together, so they are one clip inside that sheet.

## Still open

- What a style contains beyond its spritesheet: stats, starting weapons, items, modifiers, and special rules.
- How many characters a first release needs.
- Whether two players in one match can pick the same character.
- When selection happens, and the screen that does it.
- Body fit. The current capsule is fitted to the current idle frame. A new sheet does not reuse that body until that fit is specified.

## Not in this decision

Selection is not part of the baseline. No feature spec until that work starts.
