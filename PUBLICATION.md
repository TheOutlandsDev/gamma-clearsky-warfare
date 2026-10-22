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
