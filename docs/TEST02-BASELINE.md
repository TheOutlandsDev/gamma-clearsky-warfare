# Confirmed solo baseline: test-02

Recorded 2026-10-23. **Keep `test-02` as the pre-Warfare-pacing baseline.** A separate private copy of its complete manual save triplet is preserved with verified SHA-256 hashes, current profile/settings context, engine and effective repair hashes. It matches the final clean-exit save. The save, raw logs and machine-specific manifest are not published.

## Confirmed functional tests

| Check | Result | Evidence |
|---|---|---|
| Fresh world with expanded starting equipment | PASS | Player confirmation; actor-initialization trace and named save corroborate world entry |
| Two reloads preserve items and progress | PASS | Player explicitly confirmed both reloads |
| Packed bolts unpack and work without the earlier crash | PASS | Player confirmation of tested use |
| Buying guns updates money and inventory correctly | PASS | Player confirmation |
| Selling guns updates money and inventory correctly | PASS | Player confirmation |
| Traders' new stock, economy changes and added items | PASS | Player confirmation during test-02 |
| Medical treatment | PASS | Player confirmed the system was already working |
| Save retained | PASS | Player confirmation plus complete final save copies, hash-verified |
| Normal game shutdown | PASS | Local observer recorded natural native game exit `0x00000000`, separately from logger status |

These are functional passes for the tested private solo revision. Player confirmation is identified separately from captured process/save evidence. Exact full loadout, ammunition counts, before/after money values and comprehensive per-trader/per-medical-item coverage were not recorded. A functional transaction pass does not establish price balance, and a limited bolt-use pass is not proof that every future use is crash-free.

The actual starter selection was larger than the proposed 365-point recipe. Do not describe this save as an exact reproduction of that minimal recipe. See [the solo protocol](SOLO-TESTING.md), [startup findings](STARTUP-INVESTIGATION.md) and [current feature status](STATUS.md).

## Next trial: Warfare pacing

No pacing trial is claimed in this record. When testing begins, load `test-02` and immediately save a separate branch as `warfare-pace-01`, or another fresh unused name. Keep both the original named save and its verified private backup. Keep the same engine, mods, difficulty and Warfare settings during the comparison.

Record starting game clock and map/faction control, then observe 30-60 minutes of normal play. Record elapsed real and game time, dispatches, contested points, captures/losses and whether the player's actions caused them. Note sleep, time acceleration, level travel or settings changes if used. Compare strategic dispatch/capture speed separately from NPC movement speed. Save results under the branch name, not over `test-02`.

Longer-session stability, long-campaign pacing and economy balance remain open. Independent public installation and real two-PC co-op are separate release gates; this solo baseline does not validate them. The public repository still excludes private upstream repair implementations and does not reproduce the complete private game stack.
