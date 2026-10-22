# Corpse-search transfer guard

Optional compatibility addon for GAMMA with Priler's separately installed **Looting Takes Time REDUX Update 2**, pinned to source commit `cf7572ac3175a2fa46fc0d2f4645e200ab5449fe`. It wraps GAMMA's inventory move method so Take All and stack transfers leave unrevealed items on the corpse until their search completes. Revealed items remain transferable while other items are being searched.

Obtain REDUX from [Priler's author page](https://www.moddb.com/mods/stalker-anomaly/addons/looting-takes-time-redux-tarkov-like-looting); the [pinned upstream source](https://github.com/Priler/tarkov_looting/tree/cf7572ac3175a2fa46fc0d2f4645e200ab5449fe) identifies the version used for integration. This repository does not redistribute REDUX's scripts, UI, sounds or textures, or grant rights to them.

## Installation and dependencies

Requires [GAMMA](https://github.com/Grokitach/Stalker_GAMMA), the installed `ui_inventory.UIInventory.Action_Move` method, and REDUX's `zzzz_loot_searching.is_item_hidden(npc_id, item_id)` API. **Do not enable this addon without REDUX and these APIs.** The original integration has no missing-dependency fallback. It is not a standalone looting system.

1. Back up the installation and saves used for testing.
2. Install the required REDUX version separately into that installation's Mod Organizer.
3. Create a distinct Mod Organizer mod containing this addon's `gamedata` folder. Give it higher priority than REDUX and the inventory providers, and launch through that Mod Organizer.
4. If using the existing private Clear Sky testing build, its REDUX mod already includes `zzzzz_local_loot_transfer_guard.script`. Keep only one copy enabled; do not add a duplicate guard.

The guard preserves the previous inventory move method for allowed transfers. It does not replace medical behavior, the barter framework or the packed-bolts startup fix. Dependency versions and inventory wrappers can affect compatibility; test against the installed load order.

## Evidence and test limits

Publication verification compiles the original script with LuaJIT without executing game APIs. No game was launched for this publication check. During earlier local integration testing, LuaJIT tests exercised native Take All and stack movement, hidden/revealed item handling and ordinary trade/container passthrough. Those tests are not in-game confirmation.

Gameplay check: open a corpse, wait for one item to reveal, use Take All while other items are hidden, then close and reopen the inventory. Confirm that hidden items stay on the corpse, revealed items transfer, and ordinary inventory/trader actions still work.

**Co-op: experimental and unverified.** The launcher does not enable this addon. If trialed, host and clients must use matching dependencies, files and priorities; verify search visibility, transfers and reconnect behavior on both PCs.

## Provenance

`zzzzz_local_loot_transfer_guard.script` is the project's independent 805-byte API wrapper, copied byte-for-byte from the existing solo testing build. It does not contain a replacement copy of a REDUX or GAMMA implementation. Its SHA256 is `e2f65f12b1c761620ca0df9e30ce92d36281917e5819f0287cf80d35d4fd4252`.

All 42 official REDUX `gamedata` files in that private integration were verified unchanged against the pinned source archive. Only this independent wrapper is published here. The original project contribution is covered by this repository's license; upstream dependencies retain their own terms and attribution.
