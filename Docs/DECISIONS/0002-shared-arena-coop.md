Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Shared-arena co-op

Co-op for up to 4 players in one shared arena. Who writes gameplay state, including a later host, is [0005-simulation-authority.md](0005-simulation-authority.md).

Study: [brotato.md](../REFERENCES/brotato.md). Player-count balance: [0003-party-scaling.md](0003-party-scaling.md).

## Choice

Players move independently in one arena. That does not mean separate maps or separate stages.

The run is waves, then a preparation and shop phase. How many waves is `waveCount` on `Assets/Settings/GameConfig.asset` (`GameConfig`). The default is 20. The last wave is that value, and it is the final encounter. A run of 50, 100, or 1000 waves is the same rule with another value. Code does not branch on 20. The wave runner holds this asset in the Inspector. It does not look the asset up.

[0003-party-scaling.md](0003-party-scaling.md) scales from the players in the match. It does not take the wave total as an input. There is no difficulty formula that turns wave 20 into wave 1000. Per-wave duration, elites, and difficulty stay open below. The study's timing is a 20-wave reference only: wave 1 is 20 seconds, then +5 seconds per wave until 60 seconds, and that reference's wave 20 is 90 seconds. It is not the curve for a configured length.

Endless mode is not part of this design. A large finite `waveCount` is not endless mode.

Difficulty escalates by wave. A selectable difficulty ladder is a reference shape, not a list of tiers.

Each player has an individual build and inventory. The economy reference is Brotato co-op: materials and XP are credited in turn, and each player has a shop. The exact distribution, and whether shop offers are rolled per player, are not chosen. See what is still open below.

Meta progression unlocks weapons, items, and other content through play. A Codex tracks what has been discovered. Permanent account-wide stat upgrades are not the progression model.

## View

Each player has their own camera, and the screen is split so those views sit side by side. The camera follows that player's character and keeps the character at the center. It does not zoom out to frame the whole party.

Further camera work waits. This file does not set lens, shake sharing, split layout, or what a remote player's view shows.

## Rejected

Separate maps or stages for each player. The shared run is the point.

One shared camera that zooms out until every character fits. Each player watches their own character.

Permanent account-wide stat upgrades as the main meta layer. New content should widen future runs. A standing power bonus should not be assumed.

Endless mode as a requirement of the first design. It can be reconsidered later. It is not part of this decision.

A hardcoded run of 20 waves. Twenty is the default value on the config asset.

## Why

Build decisions stay with each player, while the fight is one arena. Replay is meant to come from a wider pool of characters, weapons, and items, not from a permanent stat climb. A 4-player shared run is also the shape of The Spell Brigade: https://store.steampowered.com/app/2904000/The_Spell_Brigade/ The view is each character centered on that player's own camera. Camera research is not part of this decision.

The wave total is data so a different length does not require a new rule. Player-count scaling already ignores that total. Inventing a per-wave curve in the same step would pretend a formula exists.

## Still open

- Whether materials and XP are actually dealt out in turn, and the exact rule.
- Shop offers rolled per player, or any shared offers.
- Whether a downed player can be revived, and what fails the run.
- Per-wave duration, difficulty, and when elites or hordes appear.
- Which challenges, Codex discoveries, or clears unlock which content.
- Whether Endless mode is added after the initial design.
- Camera layout and behavior beyond one centered character per player.
- What happens to enemies already alive when the party size changes. See 0003.

Do not add mechanics beyond this file unless a new decision records them.
