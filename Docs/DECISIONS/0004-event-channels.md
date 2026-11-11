Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Event channels

Cross-prefab presentation listens through an `EventChannel<T>` asset. The simulation in [0005-simulation-authority.md](0005-simulation-authority.md) commits the fact first. Same-prefab reads stay on the owner component.

## Choice

The simulation method that changes the state raises the channel after that change. The payload is the committed fact: who, how much, where. A subscriber is a component on the listening prefab. It holds the asset in a `[SerializeField]`, registers in `OnEnable`, and unregisters in `OnDisable`. The reaction stays in that script. It does not apply damage, death, or movement.

`EventChannel<T>` keeps its listener list in memory. `Raise` copies the list before calling. `Register` ignores a duplicate of the same delegate. `OnDisable`, and exiting Play Mode, clear the list so the asset cannot keep callers after play ends. A concrete channel is an empty subclass with `[CreateAssetMenu]`. It does not declare `OnEnable` or `OnDisable`.

Create a channel asset when another object must react. `CharacterHit` and `CharacterDeath` are those assets, under `Assets/Settings/Events/`. Per-player channel instances wait until [0002-shared-arena-coop.md](0002-shared-arena-coop.md) is accepted. Prefab assets reference channel assets. Scene objects subscribe from their own prefab.

Same-prefab siblings keep a `[SerializeField]` to the owner and read its state, or subscribe to a C# event on that owner. They do not get a channel.

## Rejected

A static event bus. The subscription would not be on the prefab, which fights [0001-serialized-references.md](0001-serialized-references.md).

A root component that registers for every feature on the prefab. Each new feature would edit the root.

`UnityEvent` lists as the place the reaction lives. The feature script owns the reaction.

Listener order as a sequence. When order matters, the simulation does those steps before it raises.

A shared channel for a fact that belongs to one character, weapon, or enemy. Every instance would join the same asset.

A listener that applies damage, death, or movement. That write belongs to the simulation in [0005-simulation-authority.md](0005-simulation-authority.md).

## Why

[0005-simulation-authority.md](0005-simulation-authority.md) says presentation listens to a result the simulation already committed. The channel asset is that listen when the listener is another prefab, and the Inspector shows the link. The list is not part of the asset file, so Play Mode cannot write listeners back to disk.
