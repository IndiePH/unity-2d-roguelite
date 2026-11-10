Status: proposed
Owner: Vincent Veran
Last updated: 2026-11-11

# Shared-arena co-op

Proposed direction for an original multiplayer arena survivor. Not accepted. [GDD.md](../GDD.md) still says gameplay is not settled. Do not build from this file until its status is accepted.

Study: [brotato.md](../REFERENCES/brotato.md). Balance numbers: [0003-party-scaling.md](0003-party-scaling.md).

## Choice

Co-op for up to 4 players in one shared arena. Players move independently. That does not mean separate maps or separate stages.

The run follows Brotato's wave, then a preparation and shop phase. A 20-wave run is the length reference, and wave 20 is the final encounter in that reference. Endless mode is not part of the initial design.

Difficulty escalates by wave. A selectable difficulty ladder is a reference shape, not a list of tiers.

Each player has an individual build and inventory. The economy reference is Brotato co-op: materials and XP are credited in turn, and each player has a shop. The exact distribution, and whether shop offers are rolled per player, are not chosen. See what is still open below.

Meta progression unlocks weapons, items, and other content through play. A Codex tracks what has been discovered. Permanent account-wide stat upgrades are not the progression model.

## Rejected

Separate maps or stages for each player. The shared run is the point.

Permanent account-wide stat upgrades as the main meta layer. New content should widen future runs. A standing power bonus should not be assumed.

Endless mode as a requirement of the first design. It can be reconsidered later. It is not part of this proposal.

## Why

Build decisions stay with each player, while the fight is one arena. Replay is meant to come from a wider pool of characters, weapons, and items, not from a permanent stat climb. A 4-player shared run is also the shape of The Spell Brigade: https://store.steampowered.com/app/2904000/The_Spell_Brigade/

## Still open

- Exact enemy count, HP, and damage multipliers. The candidate table is [0003-party-scaling.md](0003-party-scaling.md), and it is not locked.
- One scaling curve, or separate curves for count, health, and damage.
- Whether materials and XP are actually dealt out in turn, and the exact rule.
- Shop offers rolled per player, or any shared offers.
- Whether a downed player can be revived, and what fails the run.
- Exact wave durations, and when elites or hordes appear. Reference timing only, from the study: wave 1 is 20 seconds, then +5 seconds per wave until 60 seconds. Wave 20 is 90 seconds.
- Which challenges, Codex discoveries, or clears unlock which content.
- Whether Endless mode is added after the initial design.

Do not add mechanics beyond this file unless a new decision records them.
