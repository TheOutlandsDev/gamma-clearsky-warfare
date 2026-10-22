# First two-PC co-op test

Experimental. The launcher can prepare a connection request; only xrRazom replicates the game. A private VPN path or open host port is not a successful game session.

## Before the session

- Use two separately acquired, matching co-op installations. Record GAMMA, xrRazom and repository versions. Match enabled mod order and fingerprints; keep upstream compatibility checking enabled.
- Back up the host world and player state. Keep stable, unique player names across reconnects. Avoid reusing another player's name.
- Verify private Tailscale access on both machines without publishing their addresses or credentials. Exit Steam for the direct-IP route.
- Start with the clean co-op baseline. Do not add the newer solo AlifeTactics engine or the complete private solo mod stack.

## Run in this order

1. **Host and join:** the host opens its game and loads the world. A client connects once. Confirm both players can move and see each other.
2. **Combat:** fight a small AI group. Confirm positions, deaths, damage and corpses agree. Note who observes any disagreement.
3. **Items:** pick up, drop and exchange a disposable item. Check both inventories and the world for duplication or loss.
4. **Medical:** test only the installed baseline treatment first. Add direct-limb/surgery changes later as an explicitly separate compatibility case.
5. **Warfare:** contest one location. Check ownership, squads and the host's simulation against both clients' views.
6. **Travel:** cross a level boundary as the upstream co-op documentation instructs. Confirm players and essential state persist.
7. **Save/reconnect:** save on the host; leave and reconnect with the same player name. Check position, inventory and campaign state. Restart the host and repeat.
8. **Load:** repeat a larger fight and a longer session, recording stutters or failures separately from network desynchronization.

Stop the affected test if state diverges or items duplicate. Preserve the host backup and a private local copy of the relevant errors. Submit minimal redacted text through the co-op issue form. State which numbered test failed, host/client role, exact versions and whether matching preflight passed. Do not upload full saves, full logs, IPs, tailnet identifiers or account paths.

Only after steps 1-7 pass should you test optional campaign addons one at a time on both copies. No two-PC result is recorded as passed in this repository's initial release.
