# Private mod-stack startup investigation

Recorded 2026-10-22. **Fresh-campaign startup is blocked; the current crash is unresolved.** These findings describe an isolated private GAMMA installation. They do not establish that an individual public addon or upstream author caused the failure.

## What changed in the evidence

An earlier private revision loaded an existing Clear Sky save and opened Spore's trade window. Later attempts to create a fresh Warfare campaign failed. Save loading and new-world generation exercise different paths, so the earlier observation cannot clear the current stack.

The first captured Lua failure followed Warfare initialization through `sim_board`, ZCP's `smr_pop` and `sim_squad_scripted.remove_npc` into native `unregister_member`. A local helper redundantly inspected/released the squad after native member removal, which already owns empty-squad cleanup. The redundant release was removed in the private test copy, and two optional mutant-variant replacements were disabled. Ordinary mutant and zombie settings were retained. LuaJIT compilation and mocked lifecycle/variant regressions pass. Subsequent desktop exits still occur, so this is a correction to an observed defect rather than a verified startup fix.

The latest engine log does not contain a completed Lua error stack or explicit fatal message. Its final buffered sound-initialization line cannot identify the corrupting component. A callback BEGIN/END trace was added privately; a path-alias guard defect in that diagnostic was corrected offline. The corrected trace has not received a new runtime test.

## Effective stack audit

The full solo profile contains 590 enabled mods. An audit resolved the actual Mod Organizer file providers, rather than treating every overwritten file as simultaneous execution:

- Twelve local/recent addons supply 267 script, LTX and XML paths, including 45 exact-path replacements. The earlier compatibility addon wins 21 scripts. Its recorded base files and candidate revisions were checked for drift.
- Economy balancing intentionally replaces the Clear Sky WPO adapter and two Western Goods loot expressions. The surviving WPO adapter retains the Spore exception. Long Warfare's controller retains the earlier ownership-recursion fix. These shadows alone do not establish a conflict.
- Loot-search transfer guards delegate through the existing inventory wrappers. Starter ammunition and trader DLTX overlaps are intentional declarations, though actual engine merging and transactions still require gameplay checks.
- Western Goods' selected item/economy package also installs unconditional AI method overrides over Stealth Overhaul and AI Rework. This is a real integration boundary to resolve; it has not been established as the startup cause.
- The medical adaptation bypasses a footstep voice helper expected by Voiced Actor Refined and has an invalid local-variable reference when legacy pain sounds are selected. This is a real later-gameplay defect, not evidence for the pre-world exit.
- Medical HUD additions preserve the prior nodes and texture entries. Sixteen shader files are text-identical to their lower-priority providers. Identically named audio DLLs were hash-identical to the active base DLL; no binary replacement was warranted.
- Ninety-one winning loose OGG files fully decoded, including 14 prior replacement music files and 73 Western Goods sounds. Two packed base motor sounds were not extracted or verified. Successful decoding does not establish engine compatibility.

## Confirmed integration corrections applied privately

Two narrow corrections are installed only in the isolated solo test copy, with original bytes retained and deployed hashes verified:

- Western Goods keeps its item/economy helpers and all 11 callbacks, but no longer replaces the five existing GAMMA combat-ignore/camper methods. Its required animation state is added only when absent. Forty-five LuaJIT checks pass, including an original-version negative control, function/state identity, callback registration and preserved save/load behavior.
- The medical adaptation exposes the zero-argument pain-voice helper expected by Voiced Actor Refined and selects a valid sound index in both voice settings. Twenty-two LuaJIT cases pass, including original-error reproduction, the actual voice wrapper, sound bounds, chance/cooldown edges and unchanged damage behavior.

These corrections address demonstrated integration defects. Neither is a verified fix for the current startup exit; gameplay was deferred at the maintainer's request. The complete third-party adapted scripts and diffs remain private.

## Controlled comparison prepared

A separate 580-mod comparison profile disables only the ten recent economy, carry, hub, loot-search, goods, strategic-Warfare, starter, barter, medical and unarmed-zombie addons as a group. It retains the underlying GAMMA stack and earlier compatibility/ownership fixes. The full profile and selected profile were unchanged during preparation; settings were backed up and hash-verified.

This comparison is **not runtime-tested**. It shares the isolated test installation's settings and appdata, so it is for a separately named **new game only**. Loading an existing full-stack save with those addons disabled would not be a valid comparison. No save was deleted or migrated.

When gameplay testing resumes, use Clear Sky, Hidden Base, Warfare and a legal default starter loadout. Record whether the world loads, then preserve the engine log and callback trace before another run. A successful comparison would narrow the fault to the removed group; a failed comparison would require investigating the retained layers. Neither outcome has been observed yet.

## Publication boundary

This report publishes findings and reproducible test intent. It does not distribute the private whole-file compatibility patches, medical/WG source or assets, raw logs, dumps, saves, local account paths or network endpoints. Obtain upstream packages through [their documented author routes](DEPENDENCIES.md); [component permissions](../THIRD-PARTY.md) remain unchanged. Offline checks and prepared profiles do not constitute a finished solo or co-op release.
