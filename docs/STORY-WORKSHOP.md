# Loner Story workshop: playtest status

This is a separate, private-installation test track for a normal Story campaign with Warfare off. It does not alter the confirmed Clear Sky Warfare `test-02` baseline. The public repository includes original console and event-bridge source only; it does not include a game installation, a GAMMA profile, an xrRazom engine, third-party trader mods, or the private generated overrides.

## Current local evidence

- A fresh new-game menu showed only Loner and Rookie Village in Cordon. Story was checked, Warfare was off, both difficulty settings showed Easy, and the starter loadout budget was 3000 points. Randomize retained Rookie Village.
- A new Loner Story character entered Rookie Village. The local console reported a loaded solo world, one player, Loner faction and a Story policy match. The match is observational: the console does not enforce client settings.
- Source checks passed for the local rule builder, event bridge and host-only faction chat relay. The console's synthetic integration test passed event ingestion, status API, metrics and dashboard behavior. The isolated install preflight passed.
- A combined-launch script was corrected to recognize the Loner policy before deciding whether to start a console. This script is part of the private installation and is not distributed here.

## Tonight's solo playtest

1. Start a **new** Loner Story game at Rookie Village with both difficulty choices on Easy. Confirm the loadout budget is 3000 points.
2. Check that the local console shows the loaded solo Story world and one player.
3. Make a named save, quit, reload and compare quest state, inventory and money.
4. At Sidorovich, buy and sell a gun, armor, ammunition and supplies. Record item condition, quoted price, actual money change and any refused transaction. Check a later trader as well.
5. Note frame rate, stutter, shader behavior, graphics, sound, animations, crashes and any unexpected faction message.

Trader transactions, save/reload persistence, economy balance and long-session stability have **not** passed on this combined Story build. The Rookie Village gun market has static checks but still needs the in-game transaction test. A successful local startup does not establish these results.

## Co-op and server boundary

The intended first multiplayer trial uses a running Windows game as the xrRazom world host and a second matching Windows client. Test join, inventory and combat synchronization, trade, faction chat, level travel, host save/load, disconnect and reconnect. Five players is a target, not a verified capacity. The console listens locally and observes events; it does not host a headless world, provide profiles or enforce join rules. Linux headless hosting remains research and development work.

Do not package or distribute the private game copy until the relevant gameplay and multiplayer gates pass and all third-party redistribution rights are reviewed. Keep saves, logs, profiles, network details and game assets out of public reports.
