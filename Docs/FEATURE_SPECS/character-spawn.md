Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Character spawn

`SampleScene` has no placed character. On play, `CharacterSpawner` creates one `CharacterSprite`. That prefab uses the 100 HP profile. The 70 HP profile is not spawned: [character-profile.md](character-profile.md).

The points under `SpawnPoints` are `SpawnPoint01` through `SpawnPoint04`. Each has a `SpawnPoint`. `CharacterSpawner` uses the points assigned on it. An inactive point is not a choice. A disabled `SpawnPoint` is not a choice. An occupied point is not a choice. The open points are chosen at random. The character copies that point's position and rotation. The prefab keeps its own scale.

One character occupies one point. Another character, when one exists, takes a different open point. A dead character keeps the point. There is no respawn.

`Spawner` is the only `Instantiate`. `CharacterSpawner` passes the character prefab, the point's pose, and itself as the owner, and receives an id. Enemy pacing and loot choice are not this component. When they exist, they call `Spawner`. They do not get a second spawner. Decision: [0005-simulation-authority.md](../DECISIONS/0005-simulation-authority.md).

`CameraFollow` in `SampleScene` has no target in the editor. `CharacterSpawner` assigns the spawned character before the first frame. A later character does not take that camera. Split views stay open in [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md).

## Not verified in play

Play Mode was entered. One character stood on `SpawnPoint04` at (−0.60, −0.60). The other three points were open. The camera followed that character. The console had no errors. The Game view was not watched. The point changes each play.
