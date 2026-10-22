# Dependencies, install locations and integration

This guide explains the private testing installation and the files supplied by this public repository. **The repository is a source companion; downloading it does not reproduce the complete customized campaign.** Referenced dependencies and private overrides are not included unless their files appear in the published source inventory.

The V1 target is one free, unofficial setup and launch system supporting both solo and private host/join co-op. That target is not a statement that the complete installer or synchronized gameplay has been completed. The current launcher is source-only and experimentally supports independently prepared xrRazom copies. See [status](STATUS.md), [installation](INSTALL.md), [co-op setup](../coop/README.md) and [the two-PC test checklist](COOP-TESTING.md).

## Path key

These placeholders refer to folders chosen by the player; they are not literal folder names to create:

| Placeholder | Meaning |
|---|---|
| `<SOLO_ROOT>` | The independent single-player installation containing `Anomaly/` and portable `GAMMA/`. |
| `<COOP_ROOT>` | A separate co-op installation containing its own `Anomaly/`, portable `GAMMA/`, profiles, mods, appdata and overwrite folder. |
| `<MO_ROOT>` | The portable MO2 folder for the mode being prepared: `<SOLO_ROOT>/GAMMA` or `<COOP_ROOT>/GAMMA`. |
| `<PROFILE>` | The selected MO2 profile. The audited private names are `G.A.M.M.A` for solo and `G.A.M.M.A Warfare Co-op` for co-op. |

The relevant layout is:

```text
<SOLO_ROOT> or <COOP_ROOT>/
  Anomaly/
    bin/                         engine EXE and required DLLs
    fsgame.ltx                   engine path configuration
    appdata/
      user.ltx                  settings for the isolated co-op layout
      savedgames/               campaign saves in that layout
      logs/                     local diagnostics; review before sharing
  GAMMA/                        <MO_ROOT>, portable Mod Organizer
    ModOrganizer.exe
    ModOrganizer.ini
    portable.txt
    mods/<MOD_NAME>/gamedata/    mod-owned scripts/configs/assets
    profiles/<PROFILE>/
      modlist.txt               enabled mods and priority order
      settings.ini              profile save/settings isolation flags
    overwrite/                  generated MO output; review conflicts
```

Install script/config/asset addons through MO2 with `gamedata` directly inside the mod folder. Do not accidentally produce `mods/<MOD_NAME>/<ARCHIVE_NAME>/gamedata`. MO2 presents the enabled files to the game virtually when it launches the configured executable. Files in `Anomaly/bin/` and other root locations are a different installation layer; placing an engine EXE inside an ordinary `gamedata` mod does not install it.

Use the upstream package's own installation instructions to decide which files belong in each layer. No engine archive is provided here. A separate MO2 profile alone does not isolate root binaries, root loose files, appdata or shader cache. Keep solo and co-op roots independent rather than sharing them through filesystem links.

## Acquisition and redistribution ledger

The linked author pages provide sources, documentation and applicable terms. A public download or visible source repository does not by itself grant permission to redistribute, modify, mirror or automate downloading. Automated retrieval, a user-local repair cache and a public mirror are separate questions. This repository currently implements no dependency downloader.

| Component / audited version | Author source | Current publication handling |
|---|---|---|
| GAMMA / Anomaly base | [GAMMA instructions and credits](https://github.com/Grokitach/Stalker_GAMMA#credits), [GSC guidelines](https://www.gsc-game.com/guidelines/) | Acquire independently through the supported route. No full game/modpack copy is provided. GAMMA's own license does not relicense every numbered third-party addon. |
| xrRazom 1.4 Slim | [Author's Slim addon page](https://www.moddb.com/mods/stalker-anomaly/addons/xrrazom-stalker-anomaly-co-op-slim) | Reference only. Upstream multiplayer files, engine EXE and networking DLLs are excluded pending redistribution clearance, including individual bundled components. |
| Looting Takes Time REDUX Update 2, source `cf7572ac3175` | [Author's addon page](https://www.moddb.com/mods/stalker-anomaly/addons/looting-takes-time-redux-tarkov-like-looting), [source repository](https://github.com/Priler/tarkov_looting) | Upstream source/assets remain reference only; the independent [transfer guard](../addons/loot-transfer-guard/README.md) is published separately. No upstream modification is present in the audited installed REDUX files. A permission request is drafted, not sent. |
| anomaly-barter, commit `df04ef88442e3fc796b9028e2e90982878193eba` | [Pinned author source](https://github.com/ahuyn/anomaly-barter/tree/df04ef88442e3fc796b9028e2e90982878193eba) | Reference only. Framework scripts, UI assets and modified framework files remain excluded pending permission clarification. |
| Weapon Parts Overhaul, installed folder `140- Weapon Parts Overhaul - arti` | [Author repository](https://github.com/ahuyn/anomaly-wpo); its README directs downloads to [Releases](https://github.com/ahuyn/anomaly-wpo/releases). The locally installed metadata identifies `anomaly-wpo.zip` but does not establish its exact upstream revision. | Reference and observed file locations only. The reviewed upstream repository has no license file establishing redistribution permission. Neither WPO nor the adapted `a_wpo_parts.script` is provided. Confirm the covered local revision and permission before redistribution; current upstream must not silently substitute for it. |
| Western Goods 3.1.1 | [Tagged source, credits and terms](https://github.com/nltp-ashes/Western-Goods/tree/v3.1.1#license), [author release](https://github.com/nltp-ashes/Western-Goods/releases/tag/v3.1.1) | Reference only here. Author-owned work is CC BY-NC-SA 4.0; external contributions have their own terms, many proprietary. The whole archive is not treated as cleared. The author includes donations within prohibited commercial use. |
| Mirrowel BHSRO 0.87 | [Author repository](https://github.com/Mirrowel/Mirrowels-BHS-Edits), [current license](https://github.com/Mirrowel/Mirrowels-BHS-Edits/blob/master/LICENSE.MD) | Reference only. Confirm coverage of the exact 0.87 archive and outside assets. A current branch license alone does not establish every archived file's rights. |
| AlifeTactics 1.2.1 | [Author release](https://github.com/damiansirbu-stalker/AlifeTactics/releases/tag/1.2.1), [tagged license](https://github.com/damiansirbu-stalker/AlifeTactics/blob/1.2.1/LICENSE) | Proposed solo trial, not installed or bundled here. The custom license explicitly permits modpack inclusion with its conditions, including retained documentation/license and attribution. Exact shipped archive review remains necessary. |
| xlibs 1.9.1 | [Author release](https://github.com/damiansirbu-stalker/xlibs/releases/tag/1.9.1), [tagged license](https://github.com/damiansirbu-stalker/xlibs/blob/1.9.1/LICENSE) | Proposed complete AT dependency. The custom license permits conditional modpack inclusion. Preserve its own license and documentation; a metadata-only folder is not the complete library. |
| X-Ray Monolith 2026.9.12 | [Author release](https://github.com/themrdemonized/xray-monolith/releases/tag/2026.9.12), [license](https://github.com/themrdemonized/xray-monolith/blob/2026.9.12/License.txt) | Proposed separate solo engine trial only. Conditional noncommercial redistribution and notices apply; included third-party components need their own audit. Do not install over xrRazom. |
| Tailscale | [Official Windows setup](https://tailscale.com/docs/install/windows), [machine sharing](https://tailscale.com/docs/features/sharing) | User installs and signs in separately. No installer, account data, access keys or automatic access-policy changes are included. |

AlifeTactics by Damian Sirbu and xlibs by Damian Sirbu: https://github.com/damiansirbu-stalker . Their own licenses apply; they are not relicensed by this project.

See [THIRD-PARTY.md](../THIRD-PARTY.md) for the broader rights boundary. Providing these references and locations does not clear redistribution of an adapted upstream file. Calling such a file a patch, compatibility fix or local mod does not change its authorship or licensing.

## How xrRazom and the original launcher fit together

xrRazom supplies the multiplayer engine and replication. **The host's running game owns and simulates the world.** The project's C# launcher starts MO2 and writes a short-lived launch request; its original Lua bridge reads that request at the main menu. There is no dedicated world server, Python backend, SPT backend or public matchmaking service in this project.

The source checks these exact locations in the independent copy:

| Location relative to `<COOP_ROOT>` | Purpose |
|---|---|
| `Anomaly/bin/AnomalyDX11.exe` | Independently obtained xrRazom engine executable. |
| `Anomaly/bin/GameNetworkingSockets.dll` | Independently obtained networking runtime; part of the manifest check. |
| `Anomaly/bin/steam_api64.dll` | Independently obtained xrRazom dependency; part of the manifest check. Its presence is not a claim of redistribution permission. |
| `GAMMA/mods/xrRazom Co-op 1.4 - Slim/gamedata/` | Audited private upstream MO overlay, including `scripts/xrr_*.script` and `scripts/modxml_xrr_*.script`. Follow the author's complete package instructions rather than copying only these examples. |
| `GAMMA/mods/Local GAMMA Co-op Launcher Bridge/gamedata/scripts/modxml_zzz_gamma_coop_launcher.script` | Original bridge supplied in `coop/bridge/` in this repository. |
| `coop-install.json` | Local registration marker containing the selected profile and verified engine hashes; generated by the registration helper, not a distributable example installation. |
| `Anomaly/appdata/gamma_coop_request.txt` | One-use launch instruction, normally expiring after five minutes. The bridge consumes it before acting. Do not publish this local session file. |
| `Anomaly/appdata/user.ltx` | Player identity and xrRazom settings, including port and the enabled upstream mod check. |
| `Launcher Data/content-manifest.txt` | Locally generated content fingerprint details. Assets and archives are not fully covered by that fingerprint. Review before sharing. |
| `Anomaly/appdata/savedgames/` | Host campaign save location under the required isolated layout. The launcher makes verified save backups before launch. |

The source's default game port is `27015`; supported configured ports are `1024` through `65535`. Join requests are restricted to Tailscale IPv4 addresses. A route check does not prove game UDP connectivity. Each player needs a unique stable identity, matching engine/content and the same port. Leave xrRazom's compatibility check enabled. Follow the upstream direct-IP requirement to close Steam normally before launching.

For the public source launcher, the copied MO executable must point at this copy's `Anomaly/bin/AnomalyDX11.exe` with empty arguments. Its profile must have **both `LocalSaves=false` and `LocalSettings=false`**, and `fsgame.ltx` must map `$app_data_root$` to the copy's `appdata` folder. These are prerequisites checked by the helper, not changes made automatically by this documentation.

The private copy inspected on 2026-10-22 still had `LocalSettings=true`, despite `LocalSaves=false`; it does not meet that public launcher prerequisite unchanged. This documents an outstanding preparation difference rather than claiming that the old local launcher and the current public source are identical.

Build and registration steps, link protections, backup behavior and the private engine-manifest process are in [coop/README.md](../coop/README.md). The example manifest contains all-zero example hashes, not authenticated engine hashes, and must never be treated as a verified engine manifest.

## How the solo gameplay dependencies are connected

All paths below are **relative to the mod folder** under `<SOLO_ROOT>/GAMMA/mods/`. They describe the audited private setup; they do not imply that those mod folders or upstream-derived files are available in this repository.

| Private MO mod folder | Relevant files and integration |
|---|---|
| `Looting Takes Time REDUX - 2026-10-22` | `gamedata/scripts/zzzz_loot_searching.script`, `zzzz_loot_searching_mcm.script` and upstream UI/spinner assets provide gradual corpse searching. The separately published original [loot transfer guard](../addons/loot-transfer-guard/README.md), `gamedata/scripts/zzzzz_local_loot_transfer_guard.script`, wraps inventory movement so Take All/stack actions cannot move undiscovered items. The 42 upstream REDUX files in the private module were verified unchanged; they remain excluded from this repository. Corpse search is used; experimental container search is disabled. The guard requires independently obtained REDUX and does not grant rights to redistribute it. |
| `Local Trader Barter and Supplies - 2026-10-22` | Upstream `gamedata/scripts/barter_core.script`, `barter_ui.script`, `barter_cellcont.script`, `barter_mcm.script`, plus `gamedata/configs/ui/ui_barter.xml` and associated assets form the exchange interface/framework. Custom `gamedata/configs/barter/barter_local_campaign.ltx` and trader supply addons define campaign recipes and stock. The local core contains compatibility edits. The framework and modified whole files remain excluded. |
| `140- Weapon Parts Overhaul - arti` | Upstream `gamedata/scripts/a_wpo_parts.script` supplies weapon-part behavior. The private trading hub also contains an adapted file with this same relative path; the higher-priority balance module overrides that adapted file again. Changing a sale payout must not repair the actual weapon parts. No version of this upstream script ships here. |
| `Local Clear Sky Trading Hub - 2026-10-22` | `gamedata/configs/items/trade/mod_trade_csky_spore_zzzzzz_local_hub.ltx` defines the Spore gun hub, and `mod_trade_csky_spore_zzzzzzz_local_western_goods.ltx` adds selected goods. `gamedata/scripts/zzzzzz_local_csky_buyback.script` enforces the hub's 5% item-condition floor. `a_wpo_parts.script` integrates weapon-part resale. Separate original allegiance scripts keep the solo actor Clear Sky in the intended Warfare campaign; that is not multiplayer team enforcement. The allegiance addon is public; the hub's stock, buyback and WPO integration are not included. |
| `Western Goods - Selected Modules - 2026-10-22` | Upstream `gamedata/scripts/western_goods_core.script`, module scripts for food/drinks/devices/currencies, `western_goods_loot.script`, `western_goods_trade_eur_usd.script`, configs and assets provide selected items/currency exchanges. The private `zzzzz_local_csky_wg_trade.script` connects stock to the hub. Enabling selected modules does not establish rights to redistribute the rest of the archive. |
| `Local Tarkov Style Medical - BHSRO 0.87 - 2026-10-22` | `gamedata/scripts/zzz_player_injuries.script`, its MCM script, `eftmeds_util.script`, treatment configs/UI and assets implement the private direct limb-treatment/surgery setup. Additional GAMMA healing/damage compatibility files are present. It is a player medical adaptation; native NPC medical behavior and co-op medical synchronization require separate checks. None of this adapted module is provided here. |
| `Local Economy Testing Balance - 2026-10-22` | Winning `gamedata/scripts/a_wpo_parts.script` scales weapon resale using actual internal part condition. Winning `western_goods_loot.script` repairs a tier gate while retaining the upstream loot algorithm. Winning `gamedata/configs/barter/barter_local_campaign.ltx` corrects multi-use treatment/water quantities. These files override the earlier modules; this module is not a standalone economy or publicly reproduced here. |
| `Local Long Warfare Fronts - 2026-10-22` | `gamedata/scripts/zzz_local_long_warfronts.script` supplies occupation/regroup state; adapted `smart_terrain_warfare.script` calls it from strategic control. Target private rules are 90/60/30 game-minute base/resource/territory occupation and 180 game-minute regrouping. This slows strategic ownership and orders; it does not slow actor/NPC walking animations. The controller override remains excluded pending provenance clearance. |

The private hub uses low-condition buyback, part-aware payouts and controlled stock/recipes to make looted guns useful without treating stripped weapons as intact. It does not implement Escape from Tarkov's online flea market, use its backend, or make the entire economy an exact Tarkov simulation. Private barter recipes depend on both the framework and the item definitions they reference; importing only the recipe file cannot supply missing items or UI behavior.

The original public optional addons are individually documented under [addons/](../addons/README.md): 1.5x carry weight, practical starting equipment, Clear Sky allegiance, extra unarmed zombie creatures and the independent REDUX transfer guard. Their publication does not imply that the private hub, medical, looting or Warfare controllers are bundled with them.

## Observed priority and winning files

For the inspected profile's **text `modlist.txt`, the first enabled mod is the highest priority among listed mods**. Do not reverse this order by interpreting it as the normal ascending-priority MO2 screen. In MO2, inspect the actual conflict/winning-file view and account for root loose files and `overwrite/` as well.

The current private solo text order starts as follows, highest listed priority first:

```text
+Local Economy Testing Balance - 2026-10-22
+Local Carry Weight 1.5x - 2026-10-22
+Local Clear Sky Trading Hub - 2026-10-22
+Looting Takes Time REDUX - 2026-10-22
+Western Goods - Selected Modules - 2026-10-22
+Local Long Warfare Fronts - 2026-10-22
+Local Practical Starting Weapons - 2026-10-22
+Local Trader Barter and Supplies - 2026-10-22
+Local Tarkov Style Medical - BHSRO 0.87 - 2026-10-22
+Local Warfare Unarmed Zombies - 2026-10-22
-Local Unlimited Carry Weight - 2026-10-22
+Local GAMMA Startup Compatibility Fixes - 2026-10-11
+Local Warfare Ownership Crash Fix - 2026-10-11
```

`+` means enabled; `-` means disabled. This excerpt is an explanation of private integration, **not an importable complete profile**. Several named modules remain unpublished. The disabled unlimited-carry addon must not be enabled together with finite carry. Priority examples: the balance module's `a_wpo_parts.script` wins over the hub/WPO copies; its `western_goods_loot.script` wins over Western Goods; the long-front controller wins over the older ownership-fix controller.

The isolated co-op text order instead begins:

```text
+Local GAMMA Co-op Launcher Bridge
+xrRazom Co-op 1.4 - Slim
+Local GAMMA Startup Compatibility Fixes - 2026-10-11
+Local Warfare Ownership Crash Fix - 2026-10-11
```

The public launcher requires the bridge and xrRazom entries first and second, respectively, for its checked overlay order. Do not copy the solo priority excerpt wholesale into that profile. The later solo economy/medical/looting/long-front modules have not been jointly verified on this co-op branch. Both PCs must match any separately approved experimental addition, then repeat the relevant two-PC tests.

## Proposed tactical-AI trial

The intended behavior is more cover use and committed individual flanking with fair spotting/aim. [ALIFETACTICS.md](ALIFETACTICS.md) records the proposed pinned solo engine/library pairing, replacements for overlapping AI mods, native NPC medical preservation and conservative settings. It is a trial plan rather than an installed feature. The current private solo engine is the May 15, 2026 build; the proposed September engine has not been promoted into that game installation.

AlifeTactics/xlibs redistribution terms permit modpack inclusion with conditions, but the exact archives, notices and integration still need review. Their licenses do not establish compatibility with xrRazom. Keep the multiplayer engine independent. V1 targets tactical behavior in both modes; the exact co-op implementation remains unresolved.

## What a reader can reproduce today

Readers can acquire their base through supported upstream routes, inspect/build the original source here, install each public optional addon as documented, and prepare an independent matching co-op baseline using upstream instructions and the original bridge. The current repository does not supply the private full campaign or a complete one-click installer.

The private integration locations above are provided to make dependencies and conflicts reviewable, not to direct users to obtain withheld modified files from unofficial mirrors. References cannot substitute for missing permissions, dependencies or validation. Before distributing any newly cleared component, record its precise version/source, hash, license, required notices, included third-party rights and the files changed by this project.

Before claiming V1 readiness, complete a clean Windows installation test, a solo campaign loop and a real two-PC session covering joining, AI/combat, allegiance, inventory/loot, medical treatment, purchases/barter, Warfare captures, travel, host save/reload and reconnect. Launcher mocks and fingerprints cannot establish these outcomes. See [COOP-TESTING.md](COOP-TESTING.md).

Paths, addresses, registration markers, saves, raw logs and local configuration stay private. Post only deliberately reviewed/redacted diagnostics. No author permission request or automated report is sent by this guide.
