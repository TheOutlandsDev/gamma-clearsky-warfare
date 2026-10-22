# Experimental private co-op launcher

This source-only Windows launcher is a host/join front end for **xrRazom 1.4 Slim** over **Tailscale**. xrRazom supplies multiplayer replication; the host game runs the world. There is no dedicated server, Python backend, SPT backend, or matchmaking service here.

**Status: experimental, mock-tested, not verified in a two-PC gameplay session.** This publication contains no engine, game, GAMMA distribution, mod archive, saves, or precompiled launcher. Current solo economy, medical, looting and Warfare changes have not been jointly validated with this co-op branch. The proposed solo AlifeTactics engine update is a separate experiment; do not replace xrRazom's engine with that engine.

## Dependencies acquired separately

- A working, legally acquired [Anomaly/GAMMA installation](https://github.com/Grokitach/Stalker_GAMMA), prepared according to its own instructions.
- [xrRazom 1.4 Slim, from its author](https://www.moddb.com/mods/stalker-anomaly/addons/xrrazom-stalker-anomaly-co-op-slim). Follow the author's installation and compatibility guidance; these files do not install or redistribute it.
- [Tailscale for Windows](https://tailscale.com/docs/install/windows). Sign in interactively. Grant your partner access using your own tailnet policy or [machine sharing](https://tailscale.com/docs/features/sharing); never exchange account passwords or auth keys.
- Windows x64 and the .NET Framework 4.x C# compiler for building the launcher. Registration requires **Windows PowerShell 5.1** (`powershell.exe`), because it loads the .NET Framework launcher assembly; PowerShell 7 (`pwsh`) cannot load that serializer correctly.
- Optional developer dependency: Python with `lupa` exposing `lupa.luajit21` for Lua mock tests. It is not a game or launcher runtime dependency.

## Prepare a test copy manually

Use a **full independent copy**, with its own `Anomaly`, portable `GAMMA`/MO, `appdata`, mods, profiles and overwrite folders. Never point this launcher at your solo installation. Do not use junctions, symlinks or hardlinks; link guards reject reparse points, but cannot prove that a copy has no hardlinked files. This is a preparation checklist, not an automated game installer.

1. Keep the solo installation and its backups separate. Install matching xrRazom and compatible mods into the test copy using the author instructions. Begin with no campaign saves in the test copy.
2. Configure the copied MO's game path and its **Anomaly (DX11)** executable to this copy's `Anomaly/bin/AnomalyDX11.exe`, with empty launch arguments. Keep default portable MO storage paths; explicit base/mod/profile/overwrite directory overrides are refused.
3. Disable MO profile-specific **saves** and **game settings** for this test profile: its `settings.ini` must contain `LocalSaves=false` and `LocalSettings=false`. This makes the game and launcher agree on the copy's `Anomaly/appdata/user.ltx` and save location.
4. Ensure this copy's `Anomaly/fsgame.ltx` contains `$app_data_root$ = true | false | $fs_root$ | appdata\`. Registering the copy does not rewrite this file.
5. Install the contents of `coop/bridge` as a local MO mod named **Local GAMMA Co-op Launcher Bridge**. In the profile's text `modlist.txt`, its enabled entry must be first and the xrRazom mod second, matching the launcher's overlay order. Both PCs need identical bridge source.
6. Build the launcher below. Copy `engine-manifest.example.json` to a private local file and replace its zero placeholders with genuine SHA256 hashes from the independently obtained, trusted xrRazom engine files. The manifest detects subsequent changes; it does not authenticate an untrusted download. Never automatically bless whatever binaries happen to be installed.
7. Run registration with your local paths. It creates only `coop-install.json`, checks the prepared structure and hashes, and removes that new marker if validation fails. It never changes existing mods, MO configuration, saves, VPN settings or firewall rules. It refuses a pre-existing marker or existing saves.

From the repository root, in **Windows PowerShell 5.1** (open `powershell.exe`):

```powershell
.\coop\build.ps1 -SelfTest
$copy = Read-Host 'Independent co-op copy folder'
$solo = Read-Host 'Protected solo installation folder'
$manifest = Read-Host 'Private engine manifest JSON file'
.\coop\Register-CoopCopy.ps1 -GameRoot $copy -ProtectedSinglePlayerRoot $solo -Profile 'G.A.M.M.A Warfare Co-op' -XrRazomMod 'xrRazom Co-op 1.4 - Slim' -EngineManifest $manifest -AcknowledgeExperimental
```

Use the actual profile and xrRazom mod folder names if yours differ. Registration's structural checks are not a complete compatibility or security audit. Once registered, use a new test campaign or a separately backed-up test copy of an appropriate save; do not migrate a valuable solo campaign into an unverified co-op build.

## Host and join

Open the locally built `coop/out/GAMMA Private Co-op.exe`. Browse to the registered copy. Choose a unique, stable player name; the default `CoopPlayer` should be changed so peers do not share an identity. Connect Tailscale and exit Steam normally on both PCs, as the existing xrRazom direct-IP workflow chooses transport at startup. This launcher never closes Steam for you.

**Host:** press **Check setup**, then **HOST CAMPAIGN**, and load a test Warfare world through the copied MO. The host game runs the session. The launcher verifies a new save backup before every launch. **Copy invite** places your private network endpoint and a content fingerprint on the clipboard; send it only to your intended partner, not to a public issue.

**Join:** use matching game/engine/mod versions and the same port, enter the host's Tailscale IPv4 address, and paste their content fingerprint for a preflight check. Press **JOIN HOST**. A five-minute, one-use request makes the bridge attempt a connection at the main menu. A stale or invalid request does not connect. Do not use clients' mirrored saves as independent campaigns; let the host own campaign saves and save before exiting.

The launcher does not alter firewalls, router ports, tailnet access policies or accounts. A Tailscale ping is only a route check; it does not prove game UDP reachability. If a firewall blocks the game, review a rule scoped to the copied executable, chosen UDP port, Tailscale interface and intended peers rather than disabling the firewall or exposing a public server.

## What is checked

- Registration marker, selected profile, bridge/xrRazom priority and the three manifest engine-file hashes.
- Portable MO paths, matching DX11 executable and local `Anomaly/appdata` settings/saves.
- Bounds-checked paths and symlink/junction rejection during reads and backups.
- New SHA256-verified save backups before launch; previous backups are retained and use disk space.
- Optional content fingerprint: enabled mod order, effective loose scripts/configs except personal `axr_options.ltx`, and engine EXE/DLL files. Assets and archives are not fully covered. xrRazom's own mod compatibility check stays enabled.

## Tests and feedback

```powershell
.\coop\build.ps1 -SelfTest
python -B .\coop\tests\test_bridge.py
```

The C# self-test creates synthetic files only. Lua tests mock X-Ray and networking. Neither test launches a game, connects to a server, changes a firewall or kills an existing process. Their passing results establish launcher/bridge behavior, not synchronized gameplay.

Two PCs must still test connection, player visibility, combat, damage, inventories, purchases/barter, corpse search, Warfare captures, level transitions, host save/reload and reconnecting with the same player identity. Report engine/mod versions, matching fingerprints, reproduction steps and the first relevant **redacted** error. Mark whether a finding is solo, host or client. Do not post saves, full logs, private addresses, tailnet names, account identifiers, personal paths, tokens or keys.

Local `launcher-settings.json`, installation markers, `Launcher Data`, `Save Backups`, generated build files and inspection reports contain machine-specific information and are excluded from publication. Even an error dialog or invite screenshot can contain private paths or network details; review it before posting.
