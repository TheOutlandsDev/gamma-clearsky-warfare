# GAMMA Clear Sky Warfare

An unofficial, source-first companion for a slower Clear Sky Warfare campaign in S.T.A.L.K.E.R. GAMMA, with original optional addons, an experimental xrRazom host/join launcher, and structured playtest feedback.

**First public testing build. Two-PC co-op has not been validated.** This repository supplies source and instructions; players obtain GAMMA, xrRazom, and other dependencies from their authors. It does not install the complete customized solo campaign.

## Choose a testing track

| Track | Available here | Status |
|---|---|---|
| Solo | Optional original carry, starter equipment, allegiance and zombie addons; documented campaign targets | Static checks exist for the original local revision. Public packaging needs gameplay testing. |
| Experimental co-op | Host/join launcher source, one-shot game bridge, isolated-copy preflight, save-backup and content checks | Local/mock validation only; requires independently installed, matching xrRazom copies and a real two-PC test. |
| AlifeTactics | Compatibility plan and fair-AI preset proposal | Not installed or bundled. The proposed solo engine update is not a co-op engine update. |

Start with [installation and safe test setup](docs/INSTALL.md), then follow the [two-PC test checklist](docs/COOP-TESTING.md). Use [Issues](https://github.com/osheaconnor580-creator/gamma-clearsky-warfare/issues/new/choose) for reproducible bugs, balance observations, and feature requests.

## Campaign goals

- Clear Sky membership, permanent All-out Warfare, and dynamic faction relations off.
- Slower strategic expansion and sustained captures, with regular combat encounters.
- A Clear Sky trading hub with weapon buying/selling and measured barter progression.
- Finite 1.5x base carry capacity: 75 kg before normal gear and skill bonuses.
- Expanded practical starter guns/ammunition and more unarmed zombie creatures.
- Direct limb treatment/surgery and gradual corpse searching through upstream dependencies.
- Optional future AI changes emphasizing cover and individual flanking, without added superhuman spotting or aim bonuses.

Some of these goals are documented from a private testing installation but are **not redistributed here**. See [feature and validation status](docs/STATUS.md) and [component permissions](THIRD-PARTY.md). The trading, medical, loot-search and long-Warfare whole-file overrides remain excluded pending separate source/permission review.

## Co-op architecture

The host game owns the world simulation; clients join that running game through xrRazom. The launcher is a frontend, not a standalone server or replication engine. Use a separate co-op game copy and a private Tailscale connection. Both PCs need matching game content and the same co-op engine. The launcher does not create accounts, invite peers, open router ports, or change firewall policy.

Keep the host world save and stable, unique player names. A content fingerprint is a preflight aid, not proof of full asset parity, network reachability, or synchronization. Leave xrRazom's own compatibility check enabled. The current solo Tarkov-style changes and proposed AlifeTactics engine are not automatically compatible with co-op.

## Contributing

Read [CONTRIBUTING.md](CONTRIBUTING.md). Submit small patches with their upstream provenance and license. Report only minimal, redacted error excerpts; never publish saves, full logs, IP addresses, access keys, or local account paths. There are no telemetry or automatic report-upload features.

Original project contributions are licensed under [AGPL-3.0-only](LICENSE). Dependencies keep their own licenses and are linked separately. A repository license does not grant rights to game assets or third-party mods.

S.T.A.L.K.E.R. is developed and owned by GSC Game World. This is a fan-created modification companion, not an official product of or affiliated with GSC Game World, GAMMA's maintainers, xrRazom, Tailscale, or Battlestate Games. The survival/economy inspiration does not include Escape from Tarkov assets. Support the original games through [Steam](https://store.steampowered.com/franchise/stalker) or [GOG](https://www.gog.com/en/game/stalker_call_of_pripyat).
