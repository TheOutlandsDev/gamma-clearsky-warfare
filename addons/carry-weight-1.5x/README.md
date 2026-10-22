# Carry weight: normal base x 1.5

Optional single-player addon for GAMMA. The normal 50 kg base becomes 75 kg before ordinary gear and skill bonuses; the native walking limit becomes 105 kg before bonuses. Item masses and equipment definitions are unchanged.

On load, this addon resets a saved custom difficulty weight to 50, then uses GAMMA's keyed weight API after native first-update callbacks. It also removes the former local unlimited-carry key. Repeated callbacks do not stack the multiplier. Its saved modifier identifiers are retained for compatibility with the local testing build.

## Installation and dependencies

Use a backed-up copy of the installation and saves. In that copy's Mod Organizer, create a new mod containing this `gamedata` folder, disable the previous unlimited-carry addon, and give this addon higher priority than other carry-weight adjustments. Launch through that Mod Organizer. Do not combine it with another custom base-weight or multiplier override during the first test.

Requires [GAMMA](https://github.com/Grokitach/Stalker_GAMMA), including the native `weight`, `game_difficulties`, script-callback and time-event APIs. It is not a standalone Anomaly addon.

## Evidence and test limits

Eight LuaJIT cases using native GAMMA script bodies passed: saved bases of 20, 35, 50 and 1000 kg, each with both callback orders. Repeated callbacks, skill modifiers, and removal of the unlimited key were checked. The finite-carry revision has not been launched for an in-game check. Confirm the inventory limit and ordinary movement with your character's bonuses.

**Co-op: experimental and unverified.** The launcher does not enable this addon. If trialed, host and clients must use identical files, priorities and settings; verify each player's capacity and save/load behavior separately.

## Provenance

`zzz_local_carry_15x.script` is a new original callback wrapper written for this project. It calls existing APIs and does not contain a replacement copy of `weight.script` or `game_difficulties.script`. Dependency names and game identifiers do not imply ownership of the dependencies.
