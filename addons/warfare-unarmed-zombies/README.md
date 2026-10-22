# Unarmed zombies in Warfare

Optional single-player GAMMA addon. When native Warfare selects a new ambient mutant squad, this wrapper substitutes an unarmed zombie-creature group with 35% probability. Groups contain 4-6 of the installed `zombie_weak`, `zombie_normal` and `zombie_strong` creatures. Existing population and mutant-squad caps still apply.

This replaces some new mutant selections; it does not multiply the total population or convert existing NPCs. The armed Zombified faction is unchanged. Story-mode selection follows its original path.

## Installation and dependencies

Use a backed-up copy of the installation and saves. In that copy's Mod Organizer, create one new mod containing this `gamedata` folder and give it higher priority than conflicting Warfare spawn hooks. Keep the script and DLTX squad definition together. Launch through that Mod Organizer.

Requires [GAMMA](https://github.com/Grokitach/Stalker_GAMMA), its native `faction_expansions.get_section` selection API, `IsWarfare()`, its DLTX-capable engine and the installed unarmed zombie-creature definitions. The local target profile had the native zombie mutant population option enabled; check your own population settings. No creature models, sounds, animations or upstream spawner files are bundled.

## Evidence and test limits

A seeded 20,000-selection test produced a 35.4% replacement rate. It checked all 13 armed-faction paths, unchanged story-mode selections, and repeated startup without hook stacking. This tests the selection logic, not live spawning or animation. Squad spawning, encounter pacing, save/load, performance and campaign balance still need gameplay checks.

**Co-op: experimental and unverified.** The launcher does not enable this addon. Match files, versions, priorities and settings on host and clients if trialed. The co-op engine's ownership and synchronization of spawn selection must be checked; this wrapper is not a dedicated-server or multiplayer spawn implementation.

## Provenance

The script is a new original API wrapper, and the LTX file adds a new squad section referencing existing creature identifiers. Both were generated for this project. Neither file copies the upstream `faction_expansions.script` or redistributes creature assets.
