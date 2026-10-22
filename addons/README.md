# Optional campaign addons

These small original source addons were developed for regular single-player GAMMA. They are separate from the co-op launcher, and the launcher does not install or enable them.

Use a backed-up **copy** of your installation and saves for testing. Create a separate Mod Organizer mod for each desired addon, copying its `gamedata` folder into that mod. Give the addon higher priority than the stock files it adjusts. Check the conflict view instead of relying on a particular visual position in the mod list. Launch through the copied installation's Mod Organizer.

| Addon | Behavior | Evidence |
| --- | --- | --- |
| `carry-weight-1.5x` | Restore the normal 50 kg base and apply a nonstacking 1.5 multiplier. | Eight native LuaJIT callback/load cases passed; this revision has not been checked in game. |
| `practical-starters` | Add 12 selectable guns and increase matching starting ammunition. | Item references, all 13 faction loadouts, and native ammo grants for 52 weapon entries checked; new-campaign gameplay still needs testing. |
| `clearsky-allegiance` | Default the new-game menu to Clear Sky/Warfare and lock true Clear Sky membership in that campaign type. | Native-provider tests with engine mocks passed; menu and transition gameplay need testing. |
| `warfare-unarmed-zombies` | Replace 35% of new ambient Warfare mutant squad selections with groups of 4-6 unarmed zombie creatures. | Seeded 20,000-selection test passed; encounter pacing needs gameplay testing. |

These checks are not end-to-end engine or multiplayer validation. A previous combined single-player build loaded an existing save and opened Spore's trade window, according to the tester; that does not verify every addon listed here.

## Co-op boundary

Every addon here is **unverified in co-op**. Start co-op testing with a matched upstream setup and the launcher only. Add an optional addon only as a separate experiment, using identical files, versions, priorities, and settings on host and clients. Local UI, actor callbacks, persistence, and server-owned spawning may differ in the co-op engine. This repository does not claim these solo callbacks run correctly or synchronously on every peer.

No automatic installer, engine binaries, game assets, copied upstream framework, or save files are included. Dependencies and their licenses remain with their respective authors. See each addon README for required APIs and behavior details.
