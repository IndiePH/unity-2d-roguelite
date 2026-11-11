# Brotato mechanics reference

Study of Brotato for an original arena-survival roguelite. This is not a specification, and it is not design authority.

Transferred 2026-11-11. Statements here are simplified design observations until someone checks them against a Brotato version. When adding a fact, label it: verified feature, observed behavior, secondary-source claim, interpretation, or original proposal.

The project plan stays in [ROADMAP.md](../ROADMAP.md). The high concept and the core loop are not settled: [GDD.md](../GDD.md).

Decisions taken from this study:

- [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md) — accepted. This study is not that decision.
- [0003-party-scaling.md](../DECISIONS/0003-party-scaling.md) — accepted starting table. This study is not that decision.

Steam: https://store.steampowered.com/app/1942280/Brotato/

Waves: https://brotato.wiki.spellsandguns.com/Waves

Shop: https://brotato.wiki.spellsandguns.com/Shop

Co-op figures below are a community analysis of Brotato 1.1.15.4, not an official spec: https://brotatodb.com/co-op

Monster Hunter party-size principle cited by the scaling decision: https://news.capcomusa.com/2019/09/04/housekeeping-monster-hunter-world-and-mhw-iceborne-logistics/

## Core pattern

Short, movement-focused survival waves alternate with randomized build decisions. Combat inputs stay simple. Depth comes from character modifiers, weapon loadouts, item synergies, trade-offs, and economy.

Layers:

1. Moment to moment: move, dodge, position, attack automatically, collect drops.
2. Wave: spawn patterns, enemy mix, timer.
3. Build: weapons, items, and attributes.
4. Economy: spend or hold.
5. Meta: unlocks and challenges across runs.

Combat tests execution. The shop lowers pressure and asks for a plan. The alternation is the rhythm. Do not let preparation take longer than the fight.

## High-level structure

Top-down arena survival roguelite. The objective is to survive a sequence of timed waves in a bounded space where enemies approach from several directions. The player mostly moves, evades, positions, and collects. Weapons attack automatically by default.

A run is combat, then rewards and progression, then shop and loadout, then the next wave. The standard Brotato run is 20 waves and ends in a final encounter. Replay comes from characters, random shops, item combinations, weapon builds, difficulty, unlocks, and endless play.

Reference timing, not a locked original-game value: wave 1 lasts 20 seconds and each following wave adds 5 seconds until the cap of 60 seconds. Wave 20 lasts 90 seconds.

Choose a target run length early. Decide whether every wave uses the same shape or special waves break it. Make the combat-to-shop transition obvious and fast. A run needs a beginning, an escalation, a climax, and an end.

## Combat loop

Movement is the continuous input. Attacks are usually automatic, so the player is not pressing an attack for every weapon. Optional manual aim exists in Brotato and adds precision. Positioning decides which threats connect and which targets a weapon can reach. Collecting drops competes with staying on a safe route.

Sequence:

1. The wave starts and the timer starts.
2. Enemies spawn from the wave schedule.
3. Enemies move, attack, and interact with the player or other enemies.
4. Weapons pick targets and attack.
5. Damage, status, knockback, and death resolve.
6. Defeated enemies may drop pickups.
7. The player collects while still evading.
8. The wave ends when its survival condition is met.

Target rules to consider, per weapon: nearest, lowest health, highest threat, random valid target, a forward arc or line, or weapon-specific logic. Account for range, line of sight, cooldown, and whether the target is still valid. Do not scan every target every frame. Use a timed scan, a spatial partition, or a nearby-target cache.

Feedback the combat needs: hits, crits, blocked or reduced damage, player damage and invulnerability, enemy death, status effects, pickups, wave completion, and incoming threats.

Pressure comes from density, spawn direction, projectile patterns, movement limits, elite modifiers, and time. Raising health alone does not create variety.

Current player work is [character-sprite.md](../FEATURE_SPECS/character-sprite.md): move, dash, a hit test, a death test that hides the sprite, and attack frames that are unused. The hovering-weapon attack is accepted and not built: [hovering-weapons.md](../FEATURE_SPECS/hovering-weapons.md). Do not treat this combat model as a replacement for that spec.

Still open for our game: manual aim or not, acceleration versus instant movement, whether enemies push each other, which defensive actions exist besides dash, combat speed, and average density.

## Characters

A character is a run identity, not a skin. A definition can hold name, description, portrait, unlock condition, base health and speed, starting weapon, starting items, positive and negative modifiers, special rules, a character challenge, and selection UI.

The character should change which weapons, stats, and items look good. A flat stat bonus is not enough. Archetype examples, not a required roster:

| Direction | What it changes |
| --- | --- |
| Single-weapon specialist | Fewer weapons, more investment in the one equipped. Brotato example: One-Armed. |
| Luck specialist | Better access to random or rare outcomes. Example: Lucky. |
| Elemental specialist | Elemental damage and status. Example: Mage. |
| Melee specialist | Close-range damage or defense. Example: Brawler. |
| Survival specialist | Rewards evasion, time survived, or non-kill play. Example: Pacifist. |
| Collision specialist | Contact, knockback, or impact is the weapon. Example: Bull. |

Modifiers should show up in combat or in the shop, interact with more than one item or weapon, avoid making one strategy mandatory, and carry a real drawback when the advantage is large.

Unlocks can ask for a weapon class, a stat or economy milestone, a boss approached a certain way, a restricted survival challenge, or a difficulty clear. Do not use a grind that teaches nothing.

Judge a character in the early, middle, and late run, and across more than one build. Early power with bad scaling, or a weak start that dominates later, are both failures if they are accidental.

Still open: equal power versus intentional difficulty, whether every character can use every weapon and item, additive versus multiplicative versus rule-changing modifiers, whether a character mechanic can be upgraded mid-run, and how many characters a first release needs. The player in the scene is one spritesheet character. The roster is accepted in [0007-character-roster.md](../DECISIONS/0007-character-roster.md) and not built. Each character there has different talents and a different playstyle. See [character-sprite.md](../FEATURE_SPECS/character-sprite.md).

## Weapons and loadout

A weapon definition can hold identity, icon, description, rarity, class, tier, damage and damage type, attack interval, range and targeting, projectile speed, lifetime, penetration, crit behavior, status application, scaling attributes, attack behavior, and presentation.

Families to consider: melee (arcs, thrusts, orbits, repeated strikes), ranged (bullets, beams, thrown objects), elemental (burn, freeze, shock, and similar), explosive (delay or area), and summon or deployable (turret, companion, trap, temporary entity).

Brotato's reference loadout is six slots. An original game picks its own count and also decides:

- Whether duplicates occupy separate slots.
- Whether every family shares the same slots.
- Whether a weapon can be replaced only in the shop.
- Whether an empty slot does anything.
- How the UI shows tier and class.

Merging two identical weapons of the same tier into the next tier trades coverage for one stronger weapon. Before implementing it, define whether the merge is automatic or confirmed, what happens when the inventory is full, whether item bonuses change the result, whether weapon-specific upgrades survive, and how the lost slot is shown before the player commits.

Class bonuses reward specialization. Show the class in the inventory and the shop. The bonus should matter, and a mixed loadout should still be allowed to work.

Keep weapon data separate from execution. Targeting, cooldown, projectile spawn, hit resolution, and effect application should be testable on their own.

Still open: slot limit, which families fit this game, tier count, whether classes are required, and whether a weapon can evolve into a different variant instead of only a stronger tier.

## Items

Items are passive upgrades and special effects beyond the weapons.

| Category | Examples |
| --- | --- |
| Offense | Damage, attack speed, crit chance, range, penetration. |
| Defense | Maximum health, armor, dodge, damage reduction. |
| Recovery | Regeneration, lifesteal, healing on events. |
| Utility | Move speed, pickup radius, luck, cooldown reduction. |
| Economy | Harvesting, discounts, bonus materials, shop manipulation. |
| Special | Explosions, chains, extra projectiles, turrets, pets, conditional effects. |

An item definition can hold name, description, icon, rarity, tags, stat modifiers, triggers, magnitude, duration, stack rules and caps, positive and negative modifiers, exclusions, unlock requirement, shop weight, and price.

A drawback makes the buy depend on the build. The drawback has to be readable before purchase. Do not hide a penalty.

Synergies can key off weapon class, damage type, crits, enemy status, health thresholds, kills, pickups, summons, or shop actions. Several combinations should work. One exact item set should not be the only viable build.

Rarity can change appearance rate, cost, and strength. A common item that completes a synergy can be worth more than a rare item that does not fit.

Use one modifier pipeline. Define additive and multiplicative order, duplicate stacking, and when temporary effects expire. The description has to match the math.

Still open: whether items last the whole run, stack caps, selling or removing items, how many rarity tiers, and whether items mostly change stats, add triggers, or change rules.

## Attributes

Candidate groups. An original game does not need all of them.

Offense: general damage, melee, ranged, elemental, attack speed, crit chance, crit damage, range, projectile speed, penetration, area size.

Defense: maximum health, armor or flat reduction, dodge, regeneration, lifesteal, status resistance, shields.

Utility and economy: move speed, pickup radius, luck, harvesting or end-of-wave income, engineering or deployable scaling, cooldown reduction.

Each weapon or effect names the attributes that change it. A firearm can scale with ranged damage and attack speed. A spell can scale with elemental damage and cooldown reduction. A turret can scale with engineering and general damage. A melee strike can scale with melee damage and attack speed. The player should be able to predict that before buying.

Archetypes for testing whether the stats actually diverge: rapid-fire ranged, slow high damage, elemental damage over time, engineering and deployables, high-health tank, crits, economy.

Consider soft caps where a stat explodes, diminishing returns on dodge, crit chance, or damage reduction, and hard caps only for stability. Show the effective value after modifiers. Later upgrades should still be worth taking.

Write down the order of base value, flat modifiers, additive percents, multiplicative bonuses, and temporary effects. A silent order change is a balance bug.

Still open: which stats are essential, which are derived, the combine order, which stats cap, and how the UI explains scaling.

## Materials, experience, and the shop

In Brotato, materials collected in the fight pay for the shop and also feed leveling. That makes spending now compete with holding for later. An original game still has to choose one of these:

- One currency for both levels and the shop.
- Separate experience and currency.
- A combat drop plus a separate end-of-wave reward.
- Several specialized resources.

Sources to consider: enemy drops, wave clear, harvesting, elites and bosses, objectives, environment pickups, item and character effects.

Level-up flow: cross a threshold, present a short list, the player picks one, apply it, set the next threshold. Decide whether that happens during combat or only between waves. That choice changes pacing and risk. Rerolls need a cost, a limit, or both.

If one material pays for both, every purchase has an opportunity cost. Separate resources are easier to tune and easier to make confusing.

Accounting: prefer integers, do not grant the same reward twice during a transition, distinguish balance from lifetime earned and from unspent, save every run resource, and keep reward math deterministic if a seed will be replayed.

Shop flow: wave ends, rewards and progression resolve, stock is generated, the player buys, rerolls, locks, or skips, loadout and resources update, the player readies the next wave.

Stock rules to choose among: fixed slot count, weighted random, rarity rates, character or build weighting, unlock pools, limited or unique stock, separate pools for weapons, items, and consumables. Rerolls and locks are the agency that keeps a random shop from feeling arbitrary.

Shop actions in the reference: buy, reroll, lock for a later shop, sell or recycle where supported, ban from future pools where supported, compare with current gear, inspect stats.

Economy numbers that have to exist before balance means anything: starting money, average income per wave, prices, reroll cost and how it scales, discounts, end-of-wave income, carryover, and whether price follows the wave, the rarity, or both.

Viable spending patterns that should all sometimes be right: spend to survive the next wave, save for a expensive piece, invest in income, reroll toward a build, buy defense instead of damage. One pattern should not always win.

The shop UI shows effect, price, rarity, current resources, owned synergies, and the result of a replace or merge before the player commits.

Still open: one resource or several, when leveling happens, how many choices per level, reroll limits, whether a lock survives more than one wave, selling or dismantling, and what carries to the next wave.

## Enemies

| Role | Purpose |
| --- | --- |
| Chaser | Constant close pressure. |
| Charger | A burst that forces a reposition. |
| Ranged attacker | Distance and line of sight matter. |
| Buffer | Nearby enemies get worse. |
| Healer | The group lasts until the healer is dealt with. |
| Spawner | Count and pacing change if it is ignored. |
| Elite | A familiar enemy with a higher threat. |
| Boss | A distinct pattern that tests movement and the combat rules. |

An enemy definition can hold health, damage, speed, size, contact radius, detection and engagement range, movement, attack type, cooldown, telegraph, specials, resistances, spawn weight, which waves it may appear in, drops, and elite or boss phase data.

Build behavior from parts: move toward a target, hold a distance, charge, circle or flank, shoot, support nearby enemies, spawn, retreat. New variants should be assemblies of those parts.

Mix roles on purpose. Chasers plus ranged punish a predictable path. Buffers punish clumps. Healers and spawners become priorities. Chargers cut safe routes. Do not stack every high-pressure behavior before the player has an answer.

Telegraph dangerous attacks with animation, sound, a ground mark, and wind-up. The player should know why they were hit and have had a chance to avoid it.

Still open: how many behaviors the first prototype needs, whether enemies collide, whether they can target summons, whether elites are modified normals, and how boss phases are authored.

## Difficulty, clear, and endless

Scale one dimension at a time when tuning: health, damage, spawn rate, count, move and attack speed, composition, elite frequency, boss complexity, hazards, or fewer shop opportunities. Moving several at once makes spikes that are hard to read.

Brotato uses Danger levels. An original game can use names, numbers, modifiers, or presets. Say what the tier changes. Do not hide the whole change inside one multiplier.

A standard run needs a wave count, milestone waves for elites or bosses, varied duration and composition, a final wave with a victory condition, and a defeat condition with a result.

Endless continues past the standard ending. Decide rewards after the standard clear, how stats and spawns keep rising, whether new mixes appear, whether there is a best, and whether it simply runs until defeat. Endless is not part of the accepted design. See [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md).

Accessibility and custom challenge can cover enemy health and damage, move speed, game speed, aim assist, and clarity. If records or leaderboards exist, keep accessibility settings off those records.

Track clear rate, average wave, time to death, damage taken, and build results by character and difficulty. A clear should not depend only on a lucky shop.

Still open: how many tiers, whether they unlock in order, and whether custom modifiers count for achievements. Run length is `GameConfig`, default 20. Endless is not part of the initial design.

## Meta progression and supporting features

Unlock categories to consider: characters, weapons, items, starting equipment, difficulty, challenge modes, cosmetics if any.

Condition patterns: finish a run with a character, reach a wave, beat a boss, win with a weapon class, hit a stat threshold, finish under a restriction, repeat a specific action.

Unlocks should push experimentation, teach a system, expand later runs, and show progress. Do not grind for its own sake. Do not lock a balance fix or a core function behind a long playtime.

Keep profile data separate from the run. Profile: unlocks, achievements, highest wave or difficulty, lifetime stats, settings. Run: character, loadout, temporary stats and items, currency, wave and timer, seed if used.

Show a lock, its requirement, and progress. On unlock, say what arrived and where it is.

The reference also mentions pause and resume, wave retry on some versions, accessibility and custom difficulty, stat tracking, a build summary, controller and keyboard, rebinding, save and resume, localization, and audio, visual, and gameplay settings. Platform and option names vary. Verify before treating any of them as requirements.

Co-op is accepted in [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md). Each player has a camera with their character at the center until the arena edge. That decision still leaves open: shared versus separate currency, revive, and split layout. Character selection is [0007-character-roster.md](../DECISIONS/0007-character-roster.md). Whether two players can pick the same character stays open there.

Accessibility to consider later: game speed, less shake and flashing, colorblind indicators, readable and scalable text, aim assist, remapping, audio cues that also have a visual, and less clutter.

Quality of life after the loop exists: stat compare, optional cooldown indicators, optional damage numbers, a run recap, quick restart, a pause screen that shows the build, tooltips.

Still open: what is locked at the start, whether unlocks are per character, whether a new unlock enters the random pool immediately, achievements separate from unlocks, local versus cloud save, platforms, which accessibility is required at launch, which stats to record, and whether a run can be resumed.

## Loop summary

Run start: pick a character, apply starting gear and modifiers, initialize waves, difficulty, and seed, enter the arena.

Combat: start the timer, spawn from the schedule, the player moves, weapons attack by their rules, enemies damage and apply effects, drops appear, the player collects until the wave condition is met.

Progression: resolve rewards, apply harvesting, check level thresholds, present choices, apply the pick.

Shop: generate stock, buy or reroll or lock or skip, update currency and equipment, recalculate stats and synergies, confirm ready.

Then repeat, escalate, hit milestones and bosses. Victory and defeat both show results. Rewards on a win can unlock content.

High-level states worth naming before anyone codes them: main menu, character select, run init, wave preparation, combat, wave resolution, level-up, shop, boss or elite as a combat substate, victory, defeat, results. Define the legal transitions. Grant each reward once, on the transition that owns it.

The player should always know whether they are in combat or in preparation. Show the next objective and the timer. Keep resource totals readable. Show a shop result before commit. Say what a win or a loss paid out.

## Design lessons

1. Few real-time inputs can leave room for positioning. Test whether weapons and items supply the strategy before adding buttons.
2. A character should change a build question, not only a starting number.
3. Random offers need a limited way to steer them. Reroll, lock, and merge are the reference tools.
4. A benefit plus a drawback is interesting only if it is not the same choice for every build.
5. Write a small synergy matrix and check that more than one combination can win.
6. Keep the wave transition short. The shop is a decision, not a second game.
7. Prefer a few readable enemy roles over many enemies that differ only in health and damage.
8. An economy build needs a payoff and a survival cost.
9. Tie unlocks to trying a system, not only to volume.

For any mechanic considered for this game, ask what decision it creates, what it rewards, what else it touches, whether it is understandable, whether it adds variety, what it costs to build and balance, and whether it makes an existing mechanic pointless.

## Suggested prototype order

A suggestion from the study. It is not the project roadmap. Gates stay in [ROADMAP.md](../ROADMAP.md). Do not build a catalog before the loop is fun. Start with a few weapons, items, and enemies that interact differently.

1. Combat foundation. Movement, arena bounds, health, one chasing enemy, one automatic weapon, enemy health and death, a timed wave, basic spawn pacing. Exit: survive, move, attack automatically, kill, and finish a timed wave.
2. Reward loop. Drops, pickups, a counter, a between-wave transition, a small shop, purchase that changes a stat. Exit: a clear wave can be spent to make the next wave easier.
3. Build variety. Several weapon types, slots, item modifiers, attribute scaling, a random shop, rerolls, a basic synergy. Exit: two runs can feel different and ask for different buys.
4. Run shape. Several waves with harder schedules, more enemy roles, level-up choices, an elite or boss, win and lose, a result screen. Exit: a short run has a start, a rise, and an end.
5. Replay. Several characters, unlocks, more weapons and items, difficulty tiers, endless if it is still wanted, run stats and save data. Exit: there is a reason to start again as someone else.

Workstreams named, not scheduled: run state, data-driven weapons and items, enemy behavior parts, stat math, a spawn director, shop and economy, UI feedback, save and unlocks, balance notes.

Metrics worth recording once a loop exists: average wave reached, run duration, damage dealt and taken, time spent near enemies, currency earned and spent, purchases and rerolls, weapon performance, character win rates, death causes.

## Co-op reference, not the decision

Brotato co-op, as reported for 1.1.15.4 and not verified here:

- One shared arena. Players move independently inside it.
- Enemy count +30% per additional player.
- Enemy HP +30% per additional player.
- Enemy damage +8% per additional player.
- Materials and XP are credited to players in turn, not only to whoever picked the drop up.
- Each player keeps their own character, build, inventory, level-up choices, and shop.

The accepted decisions do not adopt the HP or damage coefficients. They do use the count curve and a different HP curve, and they treat turn-based loot plus individual shops as the economy reference while leaving the exact implementation open. Numbers and the open list are in the two decision files, not here.

## Open questions

These have to be answered before this study can become a GDD. Unanswered items are not implied decisions.

Identity: player fantasy, setting, theme, visual identity, what is not Brotato, and tone.

Camera and arena: each player has their own camera with their character at the center until the arena edge, in [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md). Split layout and the arena layout are not chosen.

Combat: movement as the only active input or not, manual aim, dashes and other defenses, speed and density, targeting per weapon family. The sprite character already dashes. Hit and death are tests, and attack frames are unused. Auto-fire from a ring, with a profile per weapon, is accepted and not built: [hovering-weapons.md](../FEATURE_SPECS/hovering-weapons.md). Detail: [character-sprite.md](../FEATURE_SPECS/character-sprite.md).

Run: wave count is `GameConfig`, default 20, in [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md). This study's 20-wave timing is a reference, not that config. Exact timings and the special-wave schedule are not chosen. Endless at launch is not part of the design.

Build: slot count, tier count, merge or evolve, classes, which attributes, negative item modifiers, how synergies are shown.

Economy: the study points at turn-based material and XP credit with individual builds and shops, and still calls the exact distribution a technical choice. Also open: when level-ups happen, how many choices, reroll and lock rules, selling, and what carries between waves.

Content: launch roster size, what is unlocked, whether unlocks are per character, achievements, the first enemy roster.

Production: platforms, controller support, localization, save and resume, and the smallest shippable version. Co-op scope is accepted in [0002-shared-arena-coop.md](../DECISIONS/0002-shared-arena-coop.md). These production items are not.

## Verification

Check these against a current Brotato build before using them as facts: weapon slot limit and merge rules, class bonuses, stat names and formulas, item rarity and stacking, how level-up relates to currency, shop reroll, lock, and ban, wave count, timing, and boss schedule, what Danger levels change, endless rules, co-op player count and platforms, accessibility and custom difficulty, unlock conditions.

Record the version, the platform, the date, and the source. Note DLC or platform differences.

The point of the study is to learn the structure, then make a different game.
