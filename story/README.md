# Experimental Story console and bridges

This folder publishes original source from the Loner Story workshop for review. It does not contain a complete installable GAMMA Story mod.

- `console/` is a local TypeScript event reader with a CLI, dashboard, status API and metrics. It reads a local JSONL event file and binds to loopback. It does not host or synchronize the game world.
- `bridge/modxml_zzz_outlands_session_bridge.script` emits minimal world-load, role and heartbeat events from an independently installed compatible game. The event file is local runtime data and is excluded.
- `bridge/modxml_zzz_story_faction_radio.script` attempts to relay major faction relation changes through xrRazom host chat while preserving the game's PDA notice. Peer delivery is unverified.

The bridges depend on game and xrRazom APIs that users must obtain from their respective authors. The private Story rules, trader patch, third-party mods, binaries and profiles are not supplied. See the [playtest status](../docs/STORY-WORKSHOP.md) for evidence and open gates.
