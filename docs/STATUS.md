# Feature and validation status

Status recorded 2026-10-23. Observations concern the private tested revision, not a fresh installation of the public source companion.

**`test-02` is the confirmed private solo baseline before Warfare pacing tests.** The player confirms expanded-equipment startup, two reloads with items/progress intact, packed-bolt unpacking/use, correct money/inventory changes when buying and selling guns, new trader stock/economy/items and medical treatment. The final save triplet is backed up and hash-verified; the observer recorded a normal native game exit of `0x00000000`.

See the [confirmed test matrix and baseline rules](TEST02-BASELINE.md). Functional results are player-confirmed; startup, save and exit evidence additionally corroborate the session. Exact loadout/ammunition, wallet amounts and full per-item coverage were not recorded. Warfare pacing, long-campaign stability and quantitative economy balance remain untested. The next manual trial must use a separate save branch and preserve `test-02`.

This is evidence about the private tested stack. The public source companion does not reproduce its private upstream repairs, and independent clean installation and real two-PC co-op remain unverified. See [startup findings](STARTUP-INVESTIGATION.md), [solo testing](SOLO-TESTING.md) and [publishing boundaries](../THIRD-PARTY.md).

| Feature | Public delivery | Evidence and remaining test |
|---|---|---|
| Carry 1.5x | Original optional addon | Local native LuaJIT checks covered saved-base reset, callback order, nonstacking, normal bonuses and removal of the old key. Public packaging needs an in-game carry check. |
| Starter guns/ammunition | Original DLTX configuration | Private equipped new-game startup passed with an expanded selection. Exact selected items and ammunition counts remain unrecorded; public-install testing is still required. |
| Clear Sky allegiance | Original wrappers | Local tests scoped locking to true Clear Sky Warfare campaigns. Save/load and companion behavior need independent testing. |
| Unarmed zombies | Original wrapper/config | Additional newly spawned zombie-creature squads; armed Zombified faction remains separate. Combat/population balance needs playtesting. |
| Clear Sky hub / barter / parts pricing | Design documented; adapted implementations excluded | Player confirms new stock/economy/items and correct money/inventory changes for gun purchases and sales in test-02. Exact price measurements and comprehensive trader coverage are unrecorded; long-term balance remains unverified. |
| Original loot-transfer guard | Original optional addon; separately acquired REDUX required | Wraps inventory transfer to block unrevealed corpse items during Take All/stack moves. All 42 official installed REDUX gamedata files matched the pinned source archive; only our wrapper is added. Compile-only check passed; no new in-game or co-op validation. |
| Medical / Western Goods / REDUX implementation and assets | Upstream references only | Player confirms medical treatment works and added goods are present. Every medical/loot-search path has not been tested. Upstream files remain excluded; co-op compatibility is unverified. |
| Slow strategic Warfare and crash fixes | Design documented; whole-file overrides excluded | Private lifecycle repairs passed recorded offline checks and initial fresh-world tests. Long-campaign pacing and the complete repaired revision are not reproduced here. |
| Co-op host/join frontend and bridge | Original source | Original local setup had preflight/mock validation; public sanitization receives fresh static tests. Real two-PC join, AI/item synchronization, Warfare captures, transitions and persistence are unverified. |
| AlifeTactics | Proposed plan | Requires a separate engine trial and replacement review. No runtime validation. |

The [dependency guide](DEPENDENCIES.md) documents the private test setup and missing pieces; it does not turn it into a reproducible public installer. Both solo and co-op remain V1 product requirements. An unresolved permission or gameplay synchronization issue remains a release gate; a source upload does not establish completion.

## Suggested balance observations

Record ordinary combat/loot/resupply loops: money before and after, ammo/medicine consumed, sale proceeds, time spent looting, and whether the next trip requires excessive grinding. Compare damaged and healthy gun sale quotes with actual wallet changes only when you have independently installed a reviewed trading implementation.

For Warfare, note contested locations, elapsed in-game time, faction losses/gains and whether the front changes without the player's presence. Separate strategic dispatch/capture pacing from NPC running speed. For AI, record rank/weapon, light, approximate range, line-of-sight breaks and whether a flank completed. Anecdotes are useful when labeled as observations rather than established causes.
