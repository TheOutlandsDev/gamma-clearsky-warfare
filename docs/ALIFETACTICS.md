# Proposed fair-AI solo trial

Not installed or bundled. The target is better cover use, reload positioning and completed individual flanks with fair spotting and aim. Coordinated squad encirclement is not a verified feature of published AlifeTactics 1.2.1.

Proposed dependency combination: [AlifeTactics 1.2.1](https://github.com/damiansirbu-stalker/AlifeTactics/releases/tag/1.2.1), complete [xlibs 1.9.1](https://github.com/damiansirbu-stalker/xlibs/releases/tag/1.9.1), and a separate [X-Ray Monolith 2026.9.12](https://github.com/themrdemonized/xray-monolith/releases/tag/2026.9.12) solo engine trial. The pinned AT code identifies September 12 as its full-status engine target; current main has later features and must not be substituted silently.

The local May 15, 2026 engine meets AT's minimum but lacks hooks needed for commitment, reload-to-cover and spacing. Missing-feature fallback is not an adequate test of the intended tactical changes. The existing SSS 23.5 / Atmospherics GAMMA 2.68.93 hotfix3 / Enhanced Shaders 1.10 setup is a source-audited candidate: the engine's shader payload was unchanged between May and September, but renderer code changed. Shader compilation and gameplay are still required. A separate MO profile cannot isolate shared root engine/appdata files.

## Replacement plan

- Disable GAMMA AI Rework, NPCs Faster Reactions, No NPC Friendly Fire and Useful Idiots in the trial only.
- Guard the Voiced Actor Refined Useful Idiots adapter before its removed-library require calls. Retire obsolete AI Rework delegates while preserving Vices quest behavior.
- Review noncombat world-job definitions rather than copying whole combat overrides. Added guard sections were found, but their active use was not established.
- Keep native NPC healing available for AT. Preserve the separate player medical system and winning Atmospherics visual-memory protections.
- Expect Useful Idiots companion modes/commands to be unavailable. Test recruited companions through save/load.

## Conservative first preset

Enable tactical maneuvers, commitment, posture and spacing. Initially disable AT's additional vision, aim tracking, prediction/lead, rank accuracy and moving-accuracy bonuses. Set extra player selection bias to zero and gear-derived combat/spotting bonuses off. These remove added AT boosts; native accuracy and perception still need testing.

Compare similar-rank NPCs/weapons at approximately 25, 75 and 125 metres; test darkness/crouching, breaking sight and repositioning, sounds behind a wall, moving aim, NPC-versus-NPC fighting and Clear Sky defense. Test healing consumption, reload stalls, save/load, travel and heavy-Warfare performance. AT's unoccluded sound investigation must not be mistaken for confirmed visual detection.

**Do not copy this solo engine over xrRazom.** Co-op needs its own matching multiplayer fork. AT-on-co-op is a separate unverified compatibility project, not part of this release.

AlifeTactics and xlibs by Damian Sirbu: https://github.com/damiansirbu-stalker . Their custom licenses remain in force; see [THIRD-PARTY.md](../THIRD-PARTY.md).
