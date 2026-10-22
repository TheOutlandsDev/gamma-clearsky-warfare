# Install and choose a test track

This is a source companion, not a complete modpack installer. Downloading this repository alone does not reproduce the private customized campaign.

Read the [dependency and setup map](DEPENDENCIES.md) for official acquisition links, portable folder roots, exact mod-relative integration paths and the distinction between MO2 overlays and root engine files. Check [publishing decisions](../THIRD-PARTY.md) before redistributing any upstream files. A documented private path is a reference, not a supplied package or permission grant.

## Solo optional addons

1. Obtain and verify your own regular single-player GAMMA installation.
2. Close the game and MO2. Back up saves, modlist and personal settings; verify the backup before making changes.
3. Make a separate test profile with independent saves, or a separate game copy. A profile alone does not isolate engine binaries, root gamedata, appdata or shader cache.
4. Follow the README inside each optional addon folder. Install it as a separate MO2 mod and check the actual winning files. Add one feature at a time.
5. Test carry/gear bonuses, new-game starter selections, faction behavior and new zombie spawns as relevant. Preserve a pre-addon save.

The finite carry addon deliberately restores a normal 50 kg difficulty baseline before applying 1.5x. Existing item masses and ordinary equipment bonuses are retained. It supersedes the old unlimited addon; do not enable both.

The optional [loot-transfer guard](../addons/loot-transfer-guard/README.md) requires a separately obtained, matching REDUX installation and compatible GAMMA inventory APIs. It supplies only an original wrapper. Do not enable it without those dependencies, and do not install a second copy if your private integration already has the same script.

## Experimental co-op

Start with [coop/README.md](../coop/README.md). Prepare an independent copy using xrRazom's own supported engine and installation instructions. Both machines obtain upstream dependencies separately and match their enabled content. This project does not copy your working solo installation or install xrRazom for you.

Build the original launcher locally from source. Use its setup/preflight helper only against an independently prepared co-op copy, then configure a unique stable player name and host endpoint locally. Keep addresses/settings out of public issues. Exit Steam before the direct-IP path, as instructed by xrRazom. Establish private Tailscale device access yourself; no router-forwarding instructions or automatic firewall policy are included.

Run the [two-PC checklist](COOP-TESTING.md). Keep xrRazom's compatibility check enabled. Add the solo optional addons to both co-op copies only as separate experimental cases after the clean co-op baseline passes. The host's authoritative world save is the campaign; mirrored client saves are not standalone world backups.

## AlifeTactics future solo trial

Follow [ALIFETACTICS.md](ALIFETACTICS.md). It is a proposed replacement plan, not an installed addon in this repository. Keep its engine change separate from xrRazom.

## Undo a trial

Close the game/MO2, preserve any new saves, then restore the matched profile/config backup and pre-addon save. Never remove item-content dependencies such as Western Goods blindly from a save containing their items; follow their authors' removal instructions. Keep a verified complete engine backup before any future engine update.
