Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Character profile

A character is a profile asset. The profile holds that character's max hit points and a reference to that character's spritesheet asset. Accepted in [0007-character-roster.md](../DECISIONS/0007-character-roster.md).

## Profiles

`Assets/Settings/Characters/Character100.asset` has 100 hit points and `Character100Sprites`. `Assets/Settings/Characters/Character70.asset` has 70 hit points and `Character70Sprites`.

`CharacterSprite` uses `Character100`. `Character.MaxHp` reads that profile. Nothing spends hit points. `Character70` is not spawned.

## Sheets

A sheet holds six clips: idle, move, dash, hit, attack, and death. Each clip is the frames in playback order and the seconds each frame stays up. `CharacterSpriteAnimation` plays the sheet on the assigned profile.

`Character100Sprites` has the frames that used to live on the prefab. Idle is 0.16 seconds, move is 0.08, dash is 0.06, and hit, attack, and death are 0.08. Dash and attack have no frames. `Character70Sprites` uses those same times and has no frames. Those frames are assigned in the Inspector.

## Not in this spec

The selection screen, a second body, and any rule for spending hit points.
