# Initial publication verification

Reviewed 2026-10-22 for a source-only public testing release.

- Four original optional solo addon modules contain seven source/config files; their hashes match the audited local originals. All four Lua files compile in LuaJIT. Historical native behavior checks and remaining gameplay checks are recorded in their READMEs.
- The sanitized original co-op launcher compiles and passes 29 synthetic C# checks. Its bridge passes 28 mocked Lua checks. The registration helper passed a synthetic Windows PowerShell 5.1 test. These tests do not launch the game or exercise a real network connection.
- Five GitHub issue-template YAML files parse and pass structural checks for required metadata, unique field IDs and privacy guidance.
- SOURCE-INVENTORY.json records the source tree's relative paths, lengths and SHA256 hashes, excluding itself. Run `python -B tools/verify_publication.py` to verify packaging, relative documentation links, addon provenance and excluded binary/private-file types.
- No game/mod archives, third-party engine implementation, precompiled executable, save, raw log, account credential or recorded network endpoint is intentionally included. Synthetic address fixtures in tests are not user endpoints. File provenance and component permission boundaries are documented.

This publication does not change the installed game, migrate saves, activate AlifeTactics, enable co-op in the solo installation or claim complete solo/co-op feature parity. The source is a starting point for independent, backed-up testing. Two-PC gameplay and long-campaign balance remain open validation work.

## Dependency-reference update, 2026-10-22

- Added a portable dependency/setup map with official author references, component versions, mod-relative paths, MO2/engine boundaries and private-integration limitations.
- Expanded the per-component publishing ledger. Explicit modpack grants (AT/xlibs) are distinguished from exact-artifact review pending, partially licensed content and reference-only packages. No author permission requests have been sent.
- Added the original 805-byte loot-transfer wrapper as a fifth optional addon, separate from REDUX. Its source hash matches the audited private addition and LuaJIT compile-only verification passed. All 42 installed official REDUX gamedata files matched the pinned source archive; none is published here.
- The refreshed inventory covers five optional addon modules with eight original script/config files, plus launcher/source/docs. Packaging/privacy checks pass; they do not establish runtime or co-op compatibility.
- Repository links use the live canonical owner, `TheOutlandsDev`.

## Historical startup investigation and offline companion correction, 2026-10-22

- At that revision the private full solo stack exited during new-game creation. At that time the README and status guide stated the blocker and linked to the effective-stack investigation. No post-fix startup success was claimed for that revision.
- Private WG/medical integration corrections pass 45 and 22 targeted LuaJIT checks. A compile-only pass also accepted 1,038 effective physical scripts in the private solo stack; archive-only native scripts were outside that pass. These third-party adapted scripts and their full diffs remain unpublished.
- The original public `coop/build.ps1` and `coop/Register-CoopCopy.ps1` now resolve script-relative defaults in the script body. Actual Windows PowerShell 5.1 fixtures reproduce the earlier default-path failures and verify both omitted and explicit output/assembly paths, including paths with spaces. Registration tests stop at the synthetic protected-root guard and do not register a real installation.
- The launcher passes 29 synthetic checks, the bridge passes 28 mocked checks, six published Lua scripts compile, two Python sources compile and five issue templates pass structure checks. These checks do not start a game, host a world or prove network synchronization.
- A long private fixture path exceeded the legacy .NET path limit; the same tests pass from a shorter path. Keep test/build copies in a short local directory. No operating-system path/security setting was changed.
- At the end of that offline update gameplay testing was deferred and the project was paused; the prepared comparison profile has not been run. Raw evidence, saves, dumps and private machine/network details remain local.

## Private startup results and docs-only update, 2026-10-23

- Testing resumed against the installation actually selected by the desktop shortcut. The private three-script lifecycle repair has recorded cleanup/contract checks and a 1,040-effective-script compile pass. Current deployed repair hashes match their reviewed candidates.
- The maintainer reached a fresh world without selected starter items (`test-01`), then reported an equipped-start pass with substantially more selected items (`test-02`). Completed actor initialization and the named save files support these limited startup observations; save copies were hash-verified. The game and MO2 were responding at the read-only follow-up check.
- The exact expanded inventory, ammunition and repeated-reload count were not independently recorded. The new solo protocol is a procedure for future fixed comparisons, not a claim that test-02 used the minimal selection.
- This update changes documentation and the source inventory only. No third-party repair implementation or diff, machine-specific logger, full log, save, dump or personal endpoint is added. No new permissions were granted and no author requests were sent.
- Publication checks validate source inventory, privacy patterns and relative links. A public clean installation, longer campaign and real two-PC co-op remain unverified. Historical counts/results above retain their original revision scope.

## Test-02 closed-session follow-up, 2026-10-23

- The maintainer reports traders' new stock, economy changes and added items functioning during test-02. This is attributed player evidence; exact inventory, trader-by-trader transaction and wallet measurements were not captured.
- The final manual save and session evidence were preserved and hash-verified. Both observer records report the game's natural exit as `0x00000000`. The logger's own return status is a separate result.
- This follow-up changes documentation and source hashes only. It does not establish economy balance, later save/reload reliability, long-campaign stability, public installation reproducibility or multiplayer compatibility. Original permissions and private-evidence exclusions are unchanged.

## Confirmed baseline follow-up, 2026-10-23

- Subsequent player confirmation establishes two successful reloads preserving items/progress, packed-bolt unpacking/use, correct money and inventory changes for gun purchases and sales, and already-working medical treatment. Earlier statements about unrecorded reload counts describe the information available before this follow-up.
- The final test-02 manual save triplet is preserved in a separate private baseline and matches the closed-session snapshot by SHA-256. Profile/settings copies and engine/repair hashes retain the local setup context. Raw evidence and the save remain private.
- README/status, startup findings and solo protocol link the consolidated test-02 matrix. Future manual Warfare pacing work uses a separate branch save; it has not begun in this update. No game, settings, mod or engine files were modified.
- Source inventory, privacy patterns and relative-link checks cover this documentation-only revision. Functional solo passes do not establish quantitative price balance, long-campaign stability, public clean installation or multiplayer synchronization. Component permissions remain unchanged.
