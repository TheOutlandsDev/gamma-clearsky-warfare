# GAMMA Clear Sky Warfare

An unofficial, source-first companion for a slower Clear Sky Warfare campaign in S.T.A.L.K.E.R. GAMMA, with original optional addons, an experimental xrRazom host/join launcher, and structured playtest feedback.

**Testing source only. Initial private solo startup tests passed on 2026-10-23, including an expanded starting loadout. Long-campaign stability, independent public installation and two-PC co-op remain unverified.** This repository supplies source and instructions; players obtain GAMMA, xrRazom, and other dependencies from their authors. It does not install the complete customized solo campaign. See the [startup investigation](docs/STARTUP-INVESTIGATION.md) before attempting a fresh campaign.

## Choose a testing track

| Track | Available here | Status |
|---|---|---|
| Solo | Optional original carry, starter equipment, allegiance, zombie and loot-transfer addons; documented campaign targets | Private fresh-world startup passed with and without selected equipment. Public packaging and longer gameplay need testing. |
| Experimental co-op | Host/join launcher source, one-shot game bridge, isolated-copy preflight, save-backup and content checks | Local/mock validation only; requires independently installed, matching xrRazom copies and a real two-PC test. |
| AlifeTactics | Compatibility plan and fair-AI preset proposal | Not installed or bundled. The proposed solo engine update is not a co-op engine update. |

Start with [installation and safe test setup](docs/INSTALL.md). The [dependency and setup map](docs/DEPENDENCIES.md) lists author sources, versions, relative install locations, load priority and how the private integrations work. The [publishing decision record](THIRD-PARTY.md) distinguishes original source we can publish, licensed dependencies awaiting artifact review and components that remain reference-only. Use the [solo test protocol](docs/SOLO-TESTING.md) for fresh-world and reload checks, or the [two-PC test checklist](docs/COOP-TESTING.md) for co-op. Use [Issues](https://github.com/TheOutlandsDev/gamma-clearsky-warfare/issues/new/choose) for reproducible bugs, balance observations, and feature requests.

## Campaign goals

- Clear Sky membership, permanent All-out Warfare, and dynamic faction relations off.
- Slower strategic expansion and sustained captures, with regular combat encounters.
- A Clear Sky trading hub with weapon buying/selling and measured barter progression.
- Finite 1.5x base carry capacity: 75 kg before normal gear and skill bonuses.
- Expanded practical starter guns/ammunition and more unarmed zombie creatures.
- Direct limb treatment/surgery and gradual corpse searching through upstream dependencies.
- Optional future AI changes emphasizing cover and individual flanking, without added superhuman spotting or aim bonuses.

Some of these goals are documented from a private testing installation but are **not redistributed here**. See [feature and validation status](docs/STATUS.md) and [component permissions](THIRD-PARTY.md). The trading, medical, Western Goods and long-Warfare whole-file overrides remain excluded pending separate source/permission review.

The independent [loot-transfer guard](addons/loot-transfer-guard/README.md) is now available as an optional addon. It requires separately obtained REDUX and compatible GAMMA inventory APIs; no REDUX files or assets are included. The currently installed REDUX source/assets are unchanged; earlier references to REDUX adaptations should be read as this separate integration wrapper.

## V1 product requirements

V1 targets one guided installer/launcher for **both solo and private host/join co-op**, retaining the campaign goals above. It should acquire dependencies through cleared distribution routes and provide verification/repair, save backups and reversible updates. The host game runs the co-op world. Permission clearance, independent clean installation, representative solo gameplay and real two-PC synchronization tests are release gates. These are requirements, not claims about the current source companion; see the [setup and integration boundaries](docs/DEPENDENCIES.md).

## Co-op architecture

The host game owns the world simulation; clients join that running game through xrRazom. The launcher is a frontend, not a standalone server or replication engine. Use a separate co-op game copy and a private Tailscale connection. Both PCs need matching game content and the same co-op engine. The launcher does not create accounts, invite peers, open router ports, or change firewall policy.

Keep the host world save and stable, unique player names. A content fingerprint is a preflight aid, not proof of full asset parity, network reachability, or synchronization. Leave xrRazom's own compatibility check enabled. The current solo Tarkov-style changes and proposed AlifeTactics engine are not automatically compatible with co-op.

## Contributing

Read [CONTRIBUTING.md](CONTRIBUTING.md). Submit small patches with their upstream provenance and license. Report only minimal, redacted error excerpts; never publish saves, full logs, IP addresses, access keys, or local account paths. There are no telemetry or automatic report-upload features.

Original project contributions are licensed under [AGPL-3.0-only](LICENSE). Dependencies keep their own licenses and are linked separately. A repository license does not grant rights to game assets or third-party mods.

S.T.A.L.K.E.R. is developed and owned by GSC Game World. This is a fan-created modification companion, not an official product of or affiliated with GSC Game World, GAMMA's maintainers, xrRazom, Tailscale, or Battlestate Games. The survival/economy inspiration does not include Escape from Tarkov assets. Support the original games through [Steam](https://store.steampowered.com/franchise/stalker) or [GOG](https://www.gog.com/en/game/stalker_call_of_pripyat).
