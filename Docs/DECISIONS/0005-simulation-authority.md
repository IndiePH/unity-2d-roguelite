Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Simulation authority

Gameplay state has one writer: a simulation that runs in the game process. Presentation reads that state and plays what it sees. This is the boundary a later host would take over. It is not a networking implementation.

The co-op mode is [0002-shared-arena-coop.md](0002-shared-arena-coop.md). Player-count balance is [0003-party-scaling.md](0003-party-scaling.md). The attack is [0006-hovering-weapons.md](0006-hovering-weapons.md). This decision does not settle the open rules inside those files.

## Choice

The simulation owns position, velocity, health, alive, cooldowns, and which spawned things exist. Rules step on a fixed timestep. `Update` samples input and runs presentation.

One motor is the only code that sets `Rigidbody2D.linearVelocity` on a gameplay body. Walk, dash, hit-stop, and death submit requests. The motor applies the winner in this order: dead, hit-stop, dash, walk.

Input produces a command for the tick: a move vector, and whether dash was pressed. The command does not move a body. Animation events and UI callbacks do not apply damage, death, or movement.

Presentation plays sprites, motion, particles, and UI from what the simulation already decided. A presentation script does not change health, alive, position, or velocity. A reaction on another prefab uses an `EventChannel<T>` asset, raised after that state has changed. Same-prefab facts stay on the owner. Decision: [0004-event-channels.md](0004-event-channels.md).

Objects created during play come from one spawner. The caller passes a kind, a position, and an owner, and receives an id. Wave logic, weapons, and pickups do not call `Instantiate`. The pool, once one exists, lives inside that spawner. Gameplay addresses the id. Objects placed in the scene, including the player, stay Inspector references under [0001-serialized-references.md](0001-serialized-references.md).

Enemy behavior is a record the simulation steps: position, velocity, health, target, state. A pooled view copies that record onto a sprite. Health bars, particles, and sounds are shared players with a budget. An enemy record does not own a canvas, a particle system, or an audio source.

Single player is this simulation running locally. It checks cooldowns and computes damage from weapon data. A caller does not declare its own position or its own damage total.

If co-op is later accepted, the host runs this same simulation. Clients send commands and apply snapshots. The host's physics is the result that counts. Clients interpolate sprites. Prediction, if it is added, covers the local player only.

## Rejected

A second writer of the same body. That includes gameplay code adding to `transform.position`, and a dash script setting velocity beside the walker.

A netcode package, a headless server, client prediction, or a snapshot format during the baseline gate.

Lockstep that needs Unity physics to match on two machines.

Each spawned enemy carrying its own world-space UI, particle system, and audio source.

Gameplay scripts calling `Instantiate`, or finding a spawned object with a scene search.

A second listener bus beside [0004-event-channels.md](0004-event-channels.md).

A client telling the host where it stands or how much damage a hit dealt.

## Why

Movement wrote `transform.position` after a rigidbody existed, so gameplay and physics both owned the body. `CharacterMovement` and `CharacterDash` still both set `linearVelocity`, and `CharacterMovement` still steps `transform.position` when no rigidbody is assigned. Those leftovers are named below. New movement does not add another writer.

That single writer is also what a host needs. The command a local player already produces is the command a remote player would send. Building the network before one local wave has been played would freeze a protocol around combat that is not designed yet.

Unity physics is not a shared clock across machines. The host simulates. Clients show the result.

## Leftover on the current sprite

The player is the sprite in [character-sprite.md](../FEATURE_SPECS/character-sprite.md). `CharacterMovement` and `CharacterDash` both set `linearVelocity`. `CharacterMovement` still contains a `transform.position` step for a missing rigidbody. The next movement change replaces both with the motor and removes that step. It waits until the play check in [PROJECT_CONTEXT.md](../PROJECT_CONTEXT.md) is done.

## Still not chosen

- The open rules in 0002: shops, revive, what fails the run, per-wave difficulty and timing, and camera work beyond one centered character per player. Wave count is `GameConfig`, default 20.
- What happens to enemies already alive when the party size changes, in 0003.
- The ranged shot, in 0006. The ring and the per-weapon profile are accepted. They are not built.
- Which netcode library, tick rate, snapshot layout, and interest management.
- Whether the local player predicts.

## New work follows this

A new moving body goes through the motor. A new spawned thing goes through the spawner, even when the pool size is one. Damage, death, and cooldowns are simulation methods. A cosmetic on another prefab subscribes with an event channel to a result the simulation already committed.
