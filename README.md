## Required Software
- [Unity 5](https://unity3d.com/)
- [Gimp](http://www.gimp.org/)
- [Git](http://git-scm.com/)
- [MyPaint](http://mypaint.intilinux.com/)
- [Blender](http://www.blender.org/)

## Optional Software
- [SourceTree](http://www.sourcetreeapp.com/)

## Command notes
How to serve repository:

	git daemon --reuseaddr --base-path=. --export-all --verbose

To clone/pull from served repo:

	git clone git://<ipaddress>/ <target>


## How not to be annoyed with Git cmdline when it keeps asking for password
Enable the password caching:

	git config --global credential.helper cache

Set the cache timeout to 1 hour:

	git config --global credential.helper 'cache --timeout=3600'
	
## High-Level Game Design

We are building an endless side-scrolling dungeon crawler using a Gothic aesthetic (Diablo, Castlevania) that focuses on player skill (combat and platforming) and strategy (optimizing weapon power loadouts and which enemy types/colors to attack/avoid). 

Levels will be built dynamically from a set of chunks or "rooms", each presenting connection points on entry and exit to connect other chunks. These chunks will contain platforming components as well as spawners (can be used for enemies, player, power-ups, etc.)

Weapons will come in 3 basic varieties: melee, short-ranged and long-ranged. Each will present tradeoffs in damage output, target count and range. Any weapons using ammunition will have an infinite supply.

Enemies will come in multiple different types with different attack styles (fliers, tanks, jumpers, runners, shooters). These enemies will be of one particular color: red, green or blue. When killed, they will drop an energy orb of their color. This can be picked up by the player and charged into one of their weapons, which will increase the damage output of that weapon against enemies of that color.

Each weapon can contain energy from the 3 different colors. Whenever the weapon is [discharged/connects with an enemy], a finite amount of this energy on all charged streams will be consumed, regardless of the target color. The amount of energy stored in the weapon when attacking determines its damage output against each enemy color.

TODO: Discuss character life/death
whether or not he has hit points or is a single-hit death, checkpoints with a few lives, etc.

## Additional Features

- Vertical connections between level chunks
- Additional dynamic level elements (moving platforms, traps, horizontal or vertical boosters)
- Additional weapon types
- Advanced character controls (dash, air dash, double jump)
- Consumable Power-Ups
- Consumable splash weapons (Grenades, splashing potions)
- Biomes for changing up scenery
