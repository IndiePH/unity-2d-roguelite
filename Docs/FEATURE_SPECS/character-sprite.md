Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-13

# Character

The player is `Assets/Art/Characters/Character.prefab`. `Run.unity` spawns one instance on an open point: [character-spawn.md](character-spawn.md). The prefab uses the Knight profile. The other profiles are assets only: [character-profile.md](character-profile.md). `CharacterAnimation` plays the animator on that prefab and copies that profile's part sprites onto the rig.

## Move

`CharacterMovement` reads the WASD-only `MoveWasd` action and sets `Rigidbody2D.linearVelocity` in `FixedUpdate`. While moving, the animator plays `Running`. Standing still plays `Idle`.

The body is dynamic, gravity 0, rotation frozen, and interpolated. Its vertical `CapsuleCollider2D` is a 0.17 circle, offset (−0.01, −0.07), on the child `BodyCollider`. The physics root stays at scale 1, so that circle is 0.17 in the world. The circle stays on the Default layer and is the shape that meets `CameraBounds`. `Assets/Settings/CharacterBody.physicsMaterial2D` has friction 0 and bounce 0. The arena is the `Tilemap`. Its tilemaps have renderers only, so they do not stop the body. `CameraBounds` stops the body on a solid edge around its trigger box. Detail: [camera-follow.md](camera-follow.md).

The bones, the sprites, the animator, and `EntityRenderer` sit on the child `Visual`. `CharacterAnimation` sets that child's scale from `Character.Scale` and scales `BodyCollider` and `Hurt` by the same change. The profiles use 0.2. The collider sizes on the prefab were fitted at 0.055, which brought the idle height of about 6.22 down to the hurt capsule's 0.34. The current scale divides by that fit, so the drawing and both colliders grow together. The child's position keeps the idle feet on the bottom of the scaled body circle and the idle center on the circle's x. Facing flips the visual only, so the colliders keep a positive scale.

`EntityRenderer` holds order in layer 10. That value is copied onto the parts when the character enables, and again after the first frame. The parts themselves are also saved at 10. The tilemap is order 0 on the same sorting layer.

A child named `Hurt` carries the combat shape. Its trigger `CapsuleCollider2D` is 0.21 by 0.34, offset (−0.01, 0.015). The child has no rigidbody, so it moves with the body. It is layer `PlayerHurt`. That layer meets no other layer, including Default, so a shot has nothing to overlap yet. The first bullet adds the layer pair that turns this trigger on. Nothing on `Hurt` calls `Hit`.

Walking does not also move the body while a dash push is active, or while `Character.HoldMovement` is set. Facing left flips the visual scale on X. The physics root does not flip.

## Dash

`CharacterDash` is on the character. Left Shift pushes along the current move direction at 16 units/second by setting the same velocity. The push stops after the dash Distance. The prefab sets that to 2.5 units. It does not follow a later change to character scale. The first value was twice the idle height at the fitted size. A wall can shorten the path the body actually moves, and the push still ends after that distance at the dash speed. Standing still dashes the way the character is facing, left or right only, and spends a use. A character that has not turned faces right. Walk and dash both write that velocity today. The next movement change folds them into the one motor in [0005-simulation-authority.md](../DECISIONS/0005-simulation-authority.md), after the play check in [PROJECT_CONTEXT.md](../PROJECT_CONTEXT.md).

While the push is active, the animator plays `Sliding`.

Invulnerability starts with the push and lasts 0.35 seconds. It does not blink. `maxUses` is 2. Each dash spends one. When a push ends, one use recharges after 1 second, then the next missing use if any.

## Hit test

P plays `Hurt` once, then calls `Character.Hit`. That raises `CharacterHit` with this character. `CameraShake` on the `MainCamera` prefab listens and plays the hit profile. That profile defaults to 0.16 seconds, 0.25 units, 1.5 degrees, and frequency 24. The camera eases back to the pose it had when the shake started. Another P during the clip or the blink does not call `Hit`. `stopMovementOnHit` defaults on: walking and dashing wait until that clip ends. When the clip ends, the prefab starts 1.25 seconds of invulnerability and the parts blink every 0.15 seconds. Another P does nothing during the clip and during that blink.

The attack is a ring of hovering weapons, not an animator state: [hovering-weapons.md](hovering-weapons.md). That design is accepted and not built.

## Death test

L plays `Dying` once. `Character.Die` sets death, then raises `CharacterDeath` with this character. `CameraShake` listens and plays the death profile. That profile defaults to 0.36 seconds, 0.4 units, 2.5 degrees, and frequency 16. A death that starts during a hit replaces the hit shake. Every part takes `deathColor` at that moment. The script default is #F3F3F3. The prefab sets its own Death Color. `hideSpriteOnDeath` defaults on in the script: the parts hide when the clip ends. This prefab leaves it off, so the tinted last pose stays. Walking, dashing, and P do nothing after that. Both colliders stay. There is no respawn.

## Not verified in play

The hurt clip, the blink after it, and the L death have not been played on this rig. Play Mode has not spawned this prefab.
