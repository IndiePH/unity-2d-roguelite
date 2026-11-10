Status: proposed
Owner: Vincent Veran
Last updated: 2026-11-11

# Party scaling

Proposed balance for the shared-arena run in [0002-shared-arena-coop.md](0002-shared-arena-coop.md). Not a locked spec. Not accepted. Do not implement it during the baseline gate.

Study: [brotato.md](../REFERENCES/brotato.md).

## Choice

Scale from the number of players actually in the match, including scaling back down when someone leaves. The principle is Monster Hunter: World / Iceborne. The numbers are not that game's numbers.

Enemy count for initial testing:

`1 + 0.30 × (active players − 1)`

That is 1.00, 1.30, 1.60, and 1.90 for one through four players. Apply it to how many enemies the wave already scheduled. Keep the mix and the timing. Do not invent an enemy type to spend the extra count. Spread fractional rounding across repeated spawns.

Enemy HP is an original curve: 1.00, 1.50, 1.80, 2.00. Total HP the party must remove is count times HP: 1.00, 1.95, 2.88, 3.80.

| Active players | Enemy count | HP per enemy | Total HP pool | Material value per enemy |
| --- | --- | --- | --- | --- |
| 1 | 1.00x | 1.00x | 1.00x | 1.00x |
| 2 | 1.30x | 1.50x | 1.95x | 1.43x |
| 3 | 1.60x | 1.80x | 2.88x | 1.70x |
| 4 | 1.90x | 2.00x | 3.80x | 1.88x |

Bosses use the same HP curve. Do not add bosses because more players joined. If a later design has more than one boss, that count comes from the wave or the difficulty, not from party size.

Do not multiply an individual enemy's damage by party size. The extra enemies are already more attacks.

Material value per enemy uses the table for initial testing. Those figures are the Brotato co-op report, not a measurement from this project. XP quantity is not in this proposal. Turn-based credit of materials and XP is the economy reference in 0002, and that decision still leaves the exact rule open.

When someone joins or leaves, later spawns use the new count. What happens to enemies already alive is not chosen: rescale their current HP proportionally, or leave them and scale only new spawns. Do not silently heal or damage them as a side effect of the choice.

Does not scale: wave duration, number of waves, wave schedule, boss count, individual enemy damage, player stats, weapon power.

## Rejected

Brotato's reported +30% HP per additional player. Use the custom HP curve instead.

Brotato's reported +8% damage per additional player. Count and durability go up. Per-hit damage does not, until a playtest shows incoming damage is too low.

Multiplying the number of bosses by party size. Brotato's boss count follows Danger, not the co-op player count.

## Why

More players should make the arena busier and each normal enemy somewhat tougher. Stacking count, HP, and damage at once spikes incoming damage before anyone has played it. The table is a first guess. Adjust it from the test below. Do not treat it as final because the arithmetic is tidy.

## Test before locking

Run the same wave with 1, 2, 3, and 4 players on comparable builds. Record enemies spawned and defeated, HP removed, time to kill, damage taken per player and by the party, downs, materials and XP per player, frame time, and the maximum number of enemies alive.

## Sources

Enemy count and the material-value row are a community analysis of Brotato 1.1.15.4: https://brotatodb.com/co-op

The dynamic party-size principle, including scaling down when players leave: https://news.capcomusa.com/2019/09/04/housekeeping-monster-hunter-world-and-mhw-iceborne-logistics/

Wave structure reference: https://brotato.wiki.spellsandguns.com/Waves
