Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-13

# Gear and attributes

Early access has three weapon archetypes, eight weapons, and two passive tags. A passive stacks. A weapon tiers by combining two copies. There is no skill button.

The ring is [0006-hovering-weapons.md](0006-hovering-weapons.md). The five profiles stay [0007-character-roster.md](0007-character-roster.md). This decision does not assign those profiles to an archetype. The study is not this decision: [brotato.md](../REFERENCES/brotato.md). Notion page 21, sections 8 and 9, reviewed 2026-11-12, is the passive-item note. It is not this decision.

Nothing here is built. No gear asset exists. The baseline next step is unchanged.

## Choice

### Attributes

An attribute is a number a run can change. Early access uses these.

| Attribute | Base today | Kind | What may change it |
| --- | --- | --- | --- |
| Max HP | The character profile | Flat | Armor stacks |
| Move speed | `CharacterMovement.speed` on the prefab, currently 5 | Flat | Accessory stacks |
| Damage | The weapon profile | Flat | That weapon's tier, and accessory stacks |
| Attack speed | The weapon profile | Weapon | That weapon's tier |
| Attack range | The weapon profile | Weapon | That weapon's tier |
| Fire rate | The weapon profile | Weapon | That weapon's tier |
| Reload speed | The weapon profile, when that weapon reloads | Weapon | That weapon's tier |

A flat stack adds. The total is the base plus the item's amount times its count. Armor and accessories have no count cap. Each player keeps their own counts.

Attack speed, fire rate, reload speed, dodge, and damage reduction are not bought as stacking items. A later item that changes a rate or a chance needs a cap, or diminishing returns, before it can be offered. Copies of a rate or a chance do not add the same amount forever.

Nothing spends hit points yet. Nothing applies a bonus yet. An armor stack does not edit the profile asset.

### Weapons

| Weapon | Archetype | Behavior | Attributes it uses |
| --- | --- | --- | --- |
| Sword | Melee | Swing | Range, damage, attack speed |
| Hammer | Melee | Swing | Range, damage, attack speed |
| Staff | Magic | Shot, no reload | Range, damage, fire rate |
| Wand | Magic | Shot, no reload | Range, damage, fire rate |
| Bow | Ranged | Shot | Range, damage, fire rate. Reload only if this bow uses one |
| Crossbow | Ranged | Shot | Range, damage, fire rate, reload |
| Handgun | Ranged | Shot | Range, damage, fire rate, reload |
| Machine gun | Ranged | Shot | Range, damage, fire rate, reload |

Magic is a weapon type under the ring rule. It uses the shot and leaves reload out. The shot itself is still unspecified in [hovering-weapons.md](../FEATURE_SPECS/hovering-weapons.md).

A weapon starts at tier 1. Two copies of the same weapon at the same tier combine into one copy of the next tier. Those two copies are removed. Two of tier 2 become one of tier 3, and the same rule continues. The new tier raises the attributes that weapon uses. The tier cap is not set.

The two copies have to be held at the same time. How many weapon slots a character has stays open in 0006. This decision does not pick that number.

### Armor and accessories

Armor and accessories are passives. They never sit in a weapon slot. They have no tier. Two copies do not combine into a higher tier.

The same item stacks. Each copy adds its attribute again. There is no inventory limit.

Early access has three passive effects. Names for the items are not chosen.

| Tag | Effect | Adds |
| --- | --- | --- |
| Armor | One vitality effect | Max HP |
| Accessory | One force effect | Damage |
| Accessory | One pace effect | Move speed |

Any character can buy any of these. The shop rules in [0002-shared-arena-coop.md](0002-shared-arena-coop.md) still decide offers, prices, and rerolls.

### Skills

Skills are not a system in this game. Attacks stay on the ring. Stat changes stay on attributes, weapon tiers, and passive stacks.

## Rejected

A skill slot, a hotbar, or a button that fires a skill. Page 21 of the Notion study says Brotato has no skill roster. Skill-like effects there sit on a character rule, a weapon, a passive, or a stat. [0006-hovering-weapons.md](0006-hovering-weapons.md) already says the player does not press an attack.

Brotato's 16-stat level-up track, and its 17 weapon classes, as the early-access set.

A passive that merges two-for-one into a higher tier.

A paper-doll of chest, ring, or accessory slots. A worn-slot limit would cap a pile the player can stack without a limit.

Stacking attack speed, dodge, or damage reduction by a flat percent with no cap.

Assigning Blood Alchemist, Warrior, Knight, Necromancer, or Templar to melee, magic, or ranged. Those profiles stay as 0007 wrote them.

## Why

The ring and the passive pile answer different questions. The ring is which attacks are out. The pile is which flat numbers grew. Combining two weapon copies is how one attack gets stronger. Stacking a flat amount is how armor and accessories get stronger without taking a weapon slot.

## Still open

- The weapon slot count.
- The tier cap, and the amount a tier adds.
- Prices, and whether each shop shows weapons and passives as separate offers.
- Whether a character's shop is limited to one archetype.
- Which of the five profiles starts with which weapon.
- Whether the bow uses reload.
- Damage reduction as an armor attribute, after a hit spends health.
- The cap a later rate or chance item uses.

## Not in this decision

Level-up drafts that offer the same attributes again. Shop, revive, and what fails the run, which stay in 0002. Selection, which stays in 0007.
