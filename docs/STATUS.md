# Feature and validation status

Status recorded 2026-10-23. Observations concern the private tested revision, not a fresh installation of the public source companion.

**Initial private solo startup tests now pass.** The maintainer created `test-01` without selected starting items, then reported `test-02` passing with substantially more selected items. Both runs reached completed actor initialization; the named save files were preserved and hash-verified. A read-only check found the game and Mod Organizer responding after test-02. The exact expanded loadout, ammunition counts and number of reloads have not been independently recorded.

**Follow-up test-02 gameplay and shutdown:** the maintainer reports the traders' new stock, economy changes and added items working in game. The final manual save was written and backed up with verified hashes. The observer independently recorded a natural game exit of `0x00000000`, distinct from the logger's own success status. Exact trader-by-trader transactions, wallet figures and inventory counts were not captured, so this is a reported functional pass rather than quantified economy-balance validation.

These observations clear the immediate startup test on this PC. They do not establish long-campaign stability or reproduce the complete private stack from this repository. The startup repairs remain private third-party adaptations; see [the investigation](STARTUP-INVESTIGATION.md), [solo testing protocol](SOLO-TESTING.md) and [publishing boundaries](../THIRD-PARTY.md). Co-op validation is unchanged.

| Feature | Public delivery | Evidence and remaining test |
|---|---|---|
| Carry 1.5x | Original optional addon | Local native LuaJIT checks covered saved-base reset, callback order, nonstacking, normal bonuses and removal of the old key. Public packaging needs an in-game carry check. |
| Starter guns/ammunition | Original DLTX configuration | Private equipped new-game startup passed with an expanded selection. Exact selected items and ammunition counts remain unrecorded; public-install testing is still required. |
| Clear Sky allegiance | Original wrappers | Local tests scoped locking to true Clear Sky Warfare campaigns. Save/load and companion behavior need independent testing. |
| Unarmed zombies | Original wrapper/config | Additional newly spawned zombie-creature squads; armed Zombified faction remains separate. Combat/population balance needs playtesting. |
| Clear Sky hub / barter / parts pricing | Design documented; adapted implementations excluded | In test-02 the maintainer reports traders, new stock and economy changes working in game. Exact transaction/wallet figures and a trader-by-trader matrix were not recorded; long-term balance remains unverified. |
| Original loot-transfer guard | Original optional addon; separately acquired REDUX required | Wraps inventory transfer to block unrevealed corpse items during Take All/stack moves. All 42 official installed REDUX gamedata files matched the pinned source archive; only our wrapper is added. Compile-only check passed; no new in-game or co-op validation. |
| Medical / Western Goods / REDUX implementation and assets | Upstream references only | The maintainer reports added goods present in test-02. This does not validate each medical or loot-search behavior. Upstream files remain excluded; co-op compatibility is unverified. |
| Slow strategic Warfare and crash fixes | Design documented; whole-file overrides excluded | Private lifecycle repairs passed recorded offline checks and initial fresh-world tests. Long-campaign pacing and the complete repaired revision are not reproduced here. |
| Co-op host/join frontend and bridge | Original source | Original local setup had preflight/mock validation; public sanitization receives fresh static tests. Real two-PC join, AI/item synchronization, Warfare captures, transitions and persistence are unverified. |
| AlifeTactics | Proposed plan | Requires a separate engine trial and replacement review. No runtime validation. |

The [dependency guide](DEPENDENCIES.md) documents the private test setup and missing pieces; it does not turn it into a reproducible public installer. Both solo and co-op remain V1 product requirements. An unresolved permission or gameplay synchronization issue remains a release gate; a source upload does not establish completion.

## Suggested balance observations

Record ordinary combat/loot/resupply loops: money before and after, ammo/medicine consumed, sale proceeds, time spent looting, and whether the next trip requires excessive grinding. Compare damaged and healthy gun sale quotes with actual wallet changes only when you have independently installed a reviewed trading implementation.

For Warfare, note contested locations, elapsed in-game time, faction losses/gains and whether the front changes without the player's presence. Separate strategic dispatch/capture pacing from NPC running speed. For AI, record rank/weapon, light, approximate range, line-of-sight breaks and whether a flank completed. Anecdotes are useful when labeled as observations rather than established causes.
