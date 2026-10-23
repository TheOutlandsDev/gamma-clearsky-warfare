# Repeatable solo testing

This protocol separates new-world startup, saving/reloading and later gameplay. It requires an independently prepared matching GAMMA installation. Downloading this repository alone does not supply the private upstream compatibility repairs; see [installation boundaries](INSTALL.md), [dependencies](DEPENDENCIES.md) and [startup findings](STARTUP-INVESTIGATION.md).

## Preserve a baseline

Before any mod/config changes, close the game and MO2 normally, back up saves, profile/modlist, settings and affected files, then verify copied content hashes. Record engine version, effective file providers and relevant source hashes locally. A profile alone does not isolate the engine, root gamedata or appdata. Do not change the stack during a running test.

Keep a successful no-selected-equipment save separate from an equipped-start save. Preserve all companion files produced for each save; do not overwrite the baseline. Use a fresh save name if one already exists.

## Fixed equipped-start recipe

1. Start a new game with Clear Sky, Hidden Base and Warfare. Keep combat/economy difficulties and other toggles identical to the baseline and record their actual values. Do not change difficulties between comparison cases.
2. With the matching practical-starter and medical configuration, select exactly one each: Glock 17 (225 points), Clear Sky Leather Jacket (50), AI-2 Medkit (75) and Bandage (15). Total: 365 points. Each medicine entry is one item, not a bundle. Leave mandatory default equipment as supplied and choose no optional extras for this case.
3. Record the final selection before starting. After entering the world, check the four items and matching ammunition. The audited private configuration grants 180 loose 9x19 FMJ; record loose rounds and the loaded magazine separately rather than assuming a combined total.
4. Wait 60 seconds near spawn. Check movement and inventory, then make the equipped-start save. Avoid combat, trades, packed bolts and treatment until this baseline is saved.
5. Return to the menu and load that save twice. Check spawn/location, selected equipment and ammunition after each reload. Record each outcome separately.

The private maintainer's `test-02` passed with a larger selection, not this exact minimal recipe. Its full item list and repeated-reload count were not captured, so that report must not be presented as execution of this fixed case. Future expanded cases should list every optional item and change one category at a time after the fixed case passes.

## Record and extend

Record new-world result, selected items/counts, ammunition loose/loaded, 60-second idle result, save result and reloads 1/2. Also record settings, elapsed game time and the last deliberate action. Keep raw evidence local. If it crashes, preserve that attempt before relaunching; never publish full logs, saves, machine paths or endpoints in an issue.

After save/reload checks pass, test one feature at a time from a new named save: packed-bolt use, limb treatment, Spore purchases/sales with actual wallet changes, then combat and Warfare dispatch/capture pacing. Opening a trade window alone is not a transaction test. Initial world loading is not proof of a stable long campaign. Repeatability here means a recorded manual procedure, not deterministic simulation or a fixed world seed.

Solo results do not validate xrRazom networking. Use the separate [two-PC protocol](COOP-TESTING.md) for host/client synchronization, transitions and persistence.
