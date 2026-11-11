Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-11

# Hovering weapons

The attack for the sprite player. Accepted in [0006-hovering-weapons.md](../DECISIONS/0006-hovering-weapons.md). Not built. Until it is, [character-sprite.md](character-sprite.md) still holds: attack frames are empty, nothing plays them, and nothing applies damage. The Brotato study is not authority: [brotato.md](../REFERENCES/brotato.md).

## Choice

Equipped weapons hover on a ring around `Assets/Prefabs/CharacterSprite.prefab` and attack on their own. The player does not press an attack. The body does not play an attack clip.

The ring is how the loadout is carried, which is the Brotato approach named here. Each weapon attacks from its own place on that ring. A weapon whose hitbox is the orbit itself is a later family, not the ring.

## Hover

Weapons follow the player's position. They are not rigidbodies, and they do not collide with the player or with arena tiles. `SampleScene` has no blocking tilemap.

They are spaced evenly on a circle in the player's local space. One weapon starts at a fixed angle. A second sits opposite it. Further weapons split the circle by the same step. The circle turns continuously, so the weapons travel around the body and keep that spacing.

Weapon sprites stay upright while they hover. They do not flip with the player's `flipX`.

Starting values, not measurements from Brotato: local radius 0.55, turn rate 30 degrees per second. The scene instance scale is 2, so a child weapon inherits that scale.

The ring is assigned on the sprite prefab. It is not looked up at runtime.

While the player walks, dashes, or is held by a hit, the weapons stay on the ring and may still attack. On death they stop attacking and hide with the sprite. Nothing attacks while `Character.IsDead` is set.

## Weapon profile

Each weapon has its own profile. The profile is that weapon's data. The ring does not hold one stat block for every weapon.

The type decides which stats the profile carries and what it does in play. Melee and ranged are the first types. Later types follow the same rule. Stats a type can use are attack range, damage, attack speed, reload speed, and fire rate. A type leaves out a stat it does not use. A melee swing uses range, damage, and attack speed. A ranged weapon uses the stats its shot needs, including fire rate and reload. The ranged shot itself is not specified yet.

The starting melee numbers below are that first profile's starting values. Radius and turn rate stay on the ring.

No profile asset exists until the first weapon is built.

## First weapon

One melee weapon is assigned on the sprite. That is enough to see the ring. Slot count, shop, merge, tiers, and classes stay open in the study and are not this spec. Brotato's reference loadout is six slots. This game has not picked its own count.

The weapon swings from its current place on the ring toward the nearest target in range, then returns to that place. The swing is the weapon sprite. It does not use `CharacterSpriteAnimation` attack frames.

A target is chosen when a swing starts. If nothing is in range, the weapon keeps hovering and does not swing.

Starting values: range 2 world units, interval 0.9 seconds, swing 0.12 seconds.

No enemy exists yet. Until one does, the swing has no target and does not play. This spec does not add enemies, health, or damage.

## Rejected

The character's attack frames as the attack. The body stays on idle, move, hit, and death. The weapon is what attacks.

One contact ring as the whole attack. That only covers weapons whose damage volume is the orbit. It drops the loadout ring Brotato uses for the rest of the weapons.

A shop, merge, tier, or class system in this spec.

One stat block shared by melee and ranged weapons.

## Still open

- The slot cap.
- The ranged weapon's shot.
- Radius, turn rate, and the first melee profile's numbers after one play check.
- Manual aim. Off for this design.

## Not verified

Nothing in this spec has been played.
