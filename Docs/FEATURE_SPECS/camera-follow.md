Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Camera follow

`CameraFollow` on the `MainCamera` prefab follows the assigned character and keeps that character at the center of the orthographic view. In `SampleScene` the target is empty in the editor. `CharacterSpawner` assigns the spawned character before the first frame: [character-spawn.md](character-spawn.md).

`CameraBounds` in `SampleScene` has a trigger `BoxCollider2D` on the water background, `Layer 0 - BG Color`. Those tiles are 64 pixels at 64 pixels per unit, so each tile is one unit. The layer is a filled 33 by 20 tiles, from cell (−16, −10) through cell (16, 9). The box is one tile taller: size (33, 21), offset (0.5, 0.5). The camera is assigned that box. Both counts are odd, so the center is the middle of cell (0, 0), at (0.5, 0.5). The extra row is the empty cell row y = 10, above the painted water. `CameraBoundsBarrier` on the same object keeps a solid `EdgeCollider2D` just outside the box, and updates that edge when the box changes. The edge uses the body's frictionless material. Its radius is 0.25, so the inner face sits on the box. `topOffset` moves that top edge in the same local units. Positive is up. Zero leaves it on the box. Half a tile is 0.5. `SampleScene` uses −0.5, so the edge sits half a tile lower. The body cannot cross that edge. The trigger box does not block it.

The follow runs in `LateUpdate`, before `CameraShake`. It copies the character's x and y and keeps the camera's z. It does not smooth.

Each axis clamps the camera so the view stays inside the collider. Near an edge, the camera stops and the character moves off center. When the view is larger than the bounds on an axis, the camera sits on the bounds center for that axis.

`CameraShake` adds its offset after that pose. Hit and death still use the profiles on `CameraShake`. The scene camera's orthographic size is unchanged.

## Not verified in play

Play Mode was entered against an earlier 35 by 24 box. With walking and dashing turned off, a fast push into each side stopped the capsule about 0.005 inside that side, and the outward speed fell to zero. A push back toward the middle moved the body. The box is now 33 by 21 at offset (0.5, 0.5). That size was not pushed. WASD was not used. The Game view was not watched for a walk into the edge.
