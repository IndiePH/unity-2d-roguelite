Status: in-progress
Owner: Vincent Veran
Last updated: 2026-11-11

# Spawner pool

`Spawner` is the only `Instantiate`. A caller passes a prefab, a pose, and an owner, and receives an id. `Release` returns that instance to the pool for that prefab. Decision: [0005-simulation-authority.md](../DECISIONS/0005-simulation-authority.md).

The id covers that use. After `Release`, `TryGet` fails and `GetOwner` throws. A returned instance is deactivated and enqueued. The next rent of that prefab dequeues the instance that has been waiting longest, then gives it a new id, the new pose, the new owner, and the prefab's scale. A different prefab does not take that instance. Two live rents of one prefab are two instances.

A live instance is a child of the spawner. A returned instance stays there, inactive. The first rent instantiates. A later rent runs `OnEnable` again and does not run `Awake` again. A returned instance that was destroyed is skipped, and the next rent instantiates.

A rent or a return does not raise an event channel. A listener on another prefab hears a fact the simulation already committed, then calls `Spawner`. Decision: [0004-event-channels.md](../DECISIONS/0004-event-channels.md).

Character spawn rents one character and does not return it. Detail: [character-spawn.md](character-spawn.md). Enemies, bullets, and effects are not built. When they exist, they call this spawner. There is no second spawner, and no pre-made count.

## Not verified in play

EditMode covers rent, return, id expiry, pose, owner, scale, queue order, a second live rent, a separate prefab, and a destroyed return. Play Mode was not entered for this change.
