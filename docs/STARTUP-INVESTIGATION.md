# Private mod-stack startup investigation

Updated 2026-10-23. **Initial fresh-world tests now pass on the repaired private solo installation.** This is a limited startup result, not confirmation of long-campaign or co-op stability. These findings concern a combined mod stack and do not establish fault by an individual public addon or upstream author.

## Current runtime evidence

| Test | Observed result | Limits |
|---|---|---|
| `test-01`, no selected starting items | Maintainer reached the world; trace completed actor initialization; manual save triplet preserved and hash-verified | One startup observation |
| `test-02`, expanded selected equipment | Maintainer reported a pass with substantially more items; a later actor-initialization completion and manual/autosave files were captured; game and MO2 were responding | Exact loadout, ammo and repeated reload count were not independently recorded |

The planned small starter recipe was not the actual expanded test-02 loadout. Follow [the solo test protocol](SOLO-TESTING.md) to record a fixed selection and repeat reloads. A previous Spore trade-window observation still does not establish successful purchases, sales or economy balance.

## Installation and failure evidence

Earlier checks used a separate comparison installation. Inspection of the desktop shortcut established that the actual launch used a different solo installation. Repairs and diagnostic hashes were then matched to that launched root and its selected MO2 profile. A successful check in one copy cannot clear a different copy.

The actual failed run returned native exit code `0xC0000374`. Its callback and native-boundary trace reached mutant-squad cleanup after module initialization. During cleanup of a rodent squad, the same member ID appeared twice at removal; the final breadcrumb entered native member unregistering and did not return.

This strongly implicated mutation of the live squad-member container during iteration. Native member removal can change that container and retire an empty squad. It is evidence for a lifecycle defect, not a native crash-dump proof of the exact corrupting instruction. The final buffered sound-initialization log line was not a reliable attribution to sound or memory exhaustion.

## Lifecycle corrections applied privately

The private repair changes three cooperating paths:

- Population cleanup/replacement/downsize paths snapshot member IDs before removal, rather than iterating a container that native removal changes. Cleanup avoids duplicate removals and querying a retired squad.
- The simulation-board caller respects the retired result and optional replacement, stopping later operations on a squad that no longer exists.
- The squad-creation helper accepts the absence of a surviving squad instead of dereferencing it after cleanup.

Original files and pre-repair saves were backed up and verified. The three deployed script hashes match their reviewed candidates. The active profile still contains 590 enabled real mods; the engine and mod list were not replaced as part of these lifecycle corrections. Existing optional mutant choices were retained.

Recorded offline checks cover 70 targeted cleanup cases and 11 contract cases. A compile-only pass accepted 1,040 effective physical Lua scripts, including the new loose helper override; this is not a claim that every archived script was extracted or gameplay-tested. Initial post-repair runtime observations are listed above. No repaired upstream script or full diff is distributed here.

## Effective stack and other integration corrections

The audit resolved actual winning MO2 providers rather than treating overwritten files as simultaneous execution. Intentional starter-ammunition, economy and trading declarations were distinguished from script-hook ownership collisions.

Two earlier private corrections remain in place:

- Western Goods retains its item/economy helpers and callbacks while preserving GAMMA's existing combat-ignore/camper methods. Its required animation state is added only when absent. The recorded 45 targeted checks cover hook identity, callbacks and save/load behavior.
- The medical adaptation exposes the pain-voice helper expected by Voiced Actor Refined and uses a valid sound index in both voice settings. The recorded 22 targeted checks cover reproduction of the prior error, wrapper behavior, bounds and cooldown edges.

These address demonstrated integration defects; the successful startup does not validate every affected gameplay feature. Earlier shader/audio identity and loose-sound decoding checks did not establish runtime compatibility. The 580-mod comparison profile was prepared in the separate installation and remains untested; no successful reduced-stack comparison is claimed.

## Local diagnostics

A private logging console backs up saves, records the selected profile and effective repair hashes, observes the exact launched game process and preserves native exit evidence. Callback/native-boundary breadcrumbs flush during startup and stop after the first actor update. The console reports administrator requirements explicitly; it does not silently change security settings. It distinguishes the game's exit code from successful evidence collection and never treats closing/detaching the console as a request to terminate the game.

The deployed logger is currently specific to the maintainer's PC and is not published as a portable launcher. Recorded offline launcher checks passed 40 cases. Logging does not replace manual gameplay or prove the absence of later faults.

## Publication boundary

This report publishes findings and test methods. It excludes private whole-file compatibility patches and diffs, medical/WG implementations or assets, the machine-specific logger, raw logs, dumps, saves, personal paths and network endpoints. Acquire upstream packages through [their author routes](DEPENDENCIES.md); [component permissions](../THIRD-PARTY.md) remain unchanged. The public source companion does not reproduce the complete repaired private installation. Independent installation, longer solo play and real two-PC tests remain release gates.
