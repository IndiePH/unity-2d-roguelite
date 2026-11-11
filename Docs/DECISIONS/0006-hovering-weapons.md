Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Hovering weapons

Equipped weapons hover on a ring around the sprite and attack on their own. Each weapon has its own profile. The profile's type decides which stats it carries and how it behaves in play.

Detail: [hovering-weapons.md](../FEATURE_SPECS/hovering-weapons.md). Nothing in that spec is built. Attack frames stay empty, and nothing applies damage, until that work starts.

## Choice

The player does not press an attack. The body does not play an attack clip. Weapons sit on a ring, stay upright, and act from their own place on that ring.

A weapon profile is the data for one weapon. Melee, ranged, and later types do not share one stat block. A profile carries the stats that type uses. The stats named for this are attack range, damage, attack speed, reload speed, and fire rate. A type omits a stat it does not use.

## Rejected

The character's attack frames as the attack.

One contact ring as the whole attack.

One shared stat block on the ring for every weapon.

## Why

The ring is the loadout. A melee swing and a ranged shot cannot be the same behavior with the same fields. The profile is what makes a new weapon a new behavior without a new ring.
