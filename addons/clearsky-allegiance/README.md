# Clear Sky Warfare allegiance

Optional single-player GAMMA addon. The first ordinary new-game menu defaults to Clear Sky with Warfare checked; explicit faction selections and randomization remain available.

For a loaded **true Clear Sky Warfare** character, the runtime wrapper prevents voluntary faction changes and disables disguise identity changes. It does not convert unrelated saves to Clear Sky, rewrite global faction relations, or set goodwill. Other factions and non-Warfare campaigns keep their ordinary behavior.

## Installation and dependencies

Use a backed-up copy of the installation and saves. In that copy's Mod Organizer, create one new mod containing this `gamedata` folder and give it higher priority than conflicting faction/menu overrides. Keep **both scripts** together: the DXML entry point installs the new-game hook, while the allegiance script implements the runtime policy. Launch through that Mod Organizer.

Requires [GAMMA](https://github.com/Grokitach/Stalker_GAMMA), native Warfare, DXML `on_xml_read` callbacks, `UINewGame`, native disguise and faction-change APIs, and GAMMA's simulation board. This addon was checked against the existing solo stack and has no general compatibility guarantee for other faction or disguise overhauls.

## Evidence and test limits

Native-provider LuaJIT tests with engine mocks checked dialogue/effect/API faction transitions, disguise refresh, unrelated campaigns, new-game defaults, repeated DXML reads, callback order, and repeated game starts. An existing tester campaign was already Clear Sky; no save conversion was performed. The menu, faction dialogue, disguise behavior, map travel, and save reload still require in-game verification of this isolated addon.

**Co-op: experimental and unverified.** The launcher does not enable it. This is a solo actor policy, not an implemented multiplayer team-enforcement system. Do not claim that it locks all connected players to one faction. Any trial needs matched host/client files, priorities and settings, plus explicit checks of every player's membership and synchronization.

## Provenance

`zzzz_local_clearsky_allegiance.script` and `modxml_local_clearsky_campaign.script` are new original wrappers for this project. They call and wrap native APIs instead of distributing copies of the upstream menu, disguise, dialogue or simulation-board files.
