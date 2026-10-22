# Feature and validation status

Status recorded 2026-10-22. Historical local testing is evidence about that revision, not a guarantee for a fresh installation or the public packaging.

| Feature | Public delivery | Evidence and remaining test |
|---|---|---|
| Carry 1.5x | Original optional addon | Local native LuaJIT checks covered saved-base reset, callback order, nonstacking, normal bonuses and removal of the old key. Public packaging needs an in-game carry check. |
| Starter guns/ammunition | Original DLTX configuration | Practical starter selection and additional ammunition were configured locally. New-game selections require public-install testing. |
| Clear Sky allegiance | Original wrappers | Local tests scoped locking to true Clear Sky Warfare campaigns. Save/load and companion behavior need independent testing. |
| Unarmed zombies | Original wrapper/config | Additional newly spawned zombie-creature squads; armed Zombified faction remains separate. Combat/population balance needs playtesting. |
| Clear Sky hub / barter / parts pricing | Design documented; adapted implementations excluded | A prior local revision loaded a save and opened Spore's trade window. Later pricing/charge changes had static/native tests only, not real wallet-transaction confirmation. |
| Medical / Western Goods / loot search | Upstream links only | Local adaptations are not included or licensed by this repo. Co-op compatibility is unverified. |
| Slow strategic Warfare and crash fixes | Design documented; whole-file overrides excluded | Local Lua/config checks existed. Long-campaign pacing and the full private revision are not reproduced here. |
| Co-op host/join frontend and bridge | Original source | Original local setup had preflight/mock validation; public sanitization receives fresh static tests. Real two-PC join, AI/item synchronization, Warfare captures, transitions and persistence are unverified. |
| AlifeTactics | Proposed plan | Requires a separate engine trial and replacement review. No runtime validation. |

## Suggested balance observations

Record ordinary combat/loot/resupply loops: money before and after, ammo/medicine consumed, sale proceeds, time spent looting, and whether the next trip requires excessive grinding. Compare damaged and healthy gun sale quotes with actual wallet changes only when you have independently installed a reviewed trading implementation.

For Warfare, note contested locations, elapsed in-game time, faction losses/gains and whether the front changes without the player's presence. Separate strategic dispatch/capture pacing from NPC running speed. For AI, record rank/weapon, light, approximate range, line-of-sight breaks and whether a flank completed. Anecdotes are useful when labeled as observations rather than established causes.
