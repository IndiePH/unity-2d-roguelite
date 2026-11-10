Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Character sprite

The spritesheet character in `Assets/Prefabs/CharacterSprite.prefab`. The capsule prefab stays a separate body and is named only where the shared mover behaves differently.

## Move

`CharacterMovement` reads the WASD-only `MoveWasd` action. On the sprite it sets `Rigidbody2D.linearVelocity` in `FixedUpdate`. The capsule has no rigidbody, so it still writes its own transform, and LitMotion breath and walk squish stay on.

The sprite body is dynamic, gravity 0, rotation frozen, and interpolated. Its vertical `CapsuleCollider2D` is 0.22 by 0.35, offset (−0.015, 0.01), fitted to the opaque idle frame. `Assets/Settings/CharacterBody.physicsMaterial2D` has friction 0 and bounce 0. In `SampleScene`, the instance scale is 2, and `Grid/Mid` blocks movement with a polygon `CompositeCollider2D`.

Walking does not also move the body while a dash push is active, or while `Character.HoldMovement` is set.

## Dash

`CharacterDash` is on the sprite only. Left Shift pushes along the current move direction at 16 units/second for 0.12 seconds by setting the same velocity. Standing still does not dash and does not spend a use.

Invulnerability starts with the push and lasts 0.35 seconds. It does not blink. `maxUses` is 2. Each dash spends one. When a push ends, one use recharges after 1 second, then the next missing use if any.

The dash frame list is empty, so the move frames stay up during the push.

## Hit test

P plays the hit frames once at full alpha. `stopMovementOnHit` defaults on: walking and dashing wait until that clip ends. When the clip ends, the prefab starts 1.25 seconds of invulnerability and the sprite blinks every 0.15 seconds. Another P does nothing during the clip and during that blink. The script defaults, if the prefab fields are cleared, are 0.5 seconds and 0.08 seconds.

Attack frames are filled from `char_boy` and are preview-only. Nothing applies damage.

## Death test

L plays the death frames once. `hideSpriteOnDeath` defaults on: the sprite hides when the clip ends. Off leaves the last frame visible. `Character.Die` stays set. Walking, dashing, and P do nothing after that. The shared WASD action stays enabled, because the capsule uses the same action. The collider stays. There is no respawn.

## Not verified in play

Walking or dashing into `Grid/Mid` has not been played. The hit clip, the blink after it, and the L death have not been played.
