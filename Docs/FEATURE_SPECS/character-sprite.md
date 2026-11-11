Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Character sprite

The player is the spritesheet character in `Assets/Prefabs/CharacterSprite.prefab`. That is the only character prefab, and the only character in `SampleScene`. A roster, where a chosen character brings different talents, a different playstyle, and their own spritesheet, is accepted and not built: [0007-character-roster.md](../DECISIONS/0007-character-roster.md). The frame lists below are the fields on this prefab today. That decision makes the sheet one asset of clips, and this component holds the asset. That move has not been made.

## Move

`CharacterMovement` reads the WASD-only `MoveWasd` action and sets `Rigidbody2D.linearVelocity` in `FixedUpdate`.

The sprite body is dynamic, gravity 0, rotation frozen, and interpolated. Its vertical `CapsuleCollider2D` is 0.22 by 0.35, offset (−0.015, 0.01), fitted to the opaque idle frame. `Assets/Settings/CharacterBody.physicsMaterial2D` has friction 0 and bounce 0. In `SampleScene`, the instance scale is 2. The arena is the `Tilemap`. Its tilemaps have renderers only, so they do not stop the body. `CameraBounds` stops the body on a solid edge around its trigger box. Detail: [camera-follow.md](camera-follow.md).

Walking does not also move the body while a dash push is active, or while `Character.HoldMovement` is set.

## Dash

`CharacterDash` is on the sprite. Left Shift pushes along the current move direction at 16 units/second for 0.12 seconds by setting the same velocity. Standing still does not dash and does not spend a use. Walk and dash both write that velocity today. The next movement change folds them into the one motor in [0005-simulation-authority.md](../DECISIONS/0005-simulation-authority.md), after the play check in [PROJECT_CONTEXT.md](../PROJECT_CONTEXT.md).

Invulnerability starts with the push and lasts 0.35 seconds. It does not blink. `maxUses` is 2. Each dash spends one. When a push ends, one use recharges after 1 second, then the next missing use if any.

The dash frame list is empty, so the move frames stay up during the push.

## Hit test

P plays the hit frames once at full alpha, then calls `Character.Hit`. That raises `CharacterHit` with this character. `CameraShake` on the `MainCamera` prefab listens and plays the hit profile. That profile defaults to 0.16 seconds, 0.25 units, 1.5 degrees, and frequency 24. The camera eases back to the pose it had when the shake started. Another P during the clip or the blink does not call `Hit`. `stopMovementOnHit` defaults on: walking and dashing wait until that clip ends. When the clip ends, the prefab starts 1.25 seconds of invulnerability and the sprite blinks every 0.15 seconds. Another P does nothing during the clip and during that blink. The script defaults, if the prefab fields are cleared, are 0.5 seconds and 0.08 seconds.

Attack frames are empty. Nothing plays them, and nothing applies damage. The attack is a ring of hovering weapons, not these frames: [hovering-weapons.md](hovering-weapons.md). That design is accepted and not built.

## Death test

L plays the death frames once. `Character.Die` sets death, then raises `CharacterDeath` with this character. `CameraShake` listens and plays the death profile. That profile defaults to 0.36 seconds, 0.4 units, 2.5 degrees, and frequency 16. A death that starts during a hit replaces the hit shake. `hideSpriteOnDeath` defaults on: the sprite hides when the clip ends. Off leaves the last frame visible. Walking, dashing, and P do nothing after that. The collider stays. There is no respawn.

## Not verified in play

The hit clip, the blink after it, and the L death have not been played. In Play Mode, calling `Hit` and `Die` moved the camera through the channels. The Game view was not watched.
