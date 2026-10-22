# Initial publication verification

Reviewed 2026-10-22 for a source-only public testing release.

- Four original optional solo addon modules contain seven source/config files; their hashes match the audited local originals. All four Lua files compile in LuaJIT. Historical native behavior checks and remaining gameplay checks are recorded in their READMEs.
- The sanitized original co-op launcher compiles and passes 29 synthetic C# checks. Its bridge passes 28 mocked Lua checks. The registration helper passed a synthetic Windows PowerShell 5.1 test. These tests do not launch the game or exercise a real network connection.
- Five GitHub issue-template YAML files parse and pass structural checks for required metadata, unique field IDs and privacy guidance.
- SOURCE-INVENTORY.json records the source tree's relative paths, lengths and SHA256 hashes, excluding itself. Run `python -B tools/verify_publication.py` to verify packaging, relative documentation links, addon provenance and excluded binary/private-file types.
- No game/mod archives, third-party engine implementation, precompiled executable, save, raw log, account credential or recorded network endpoint is intentionally included. Synthetic address fixtures in tests are not user endpoints. File provenance and component permission boundaries are documented.

This publication does not change the installed game, migrate saves, activate AlifeTactics, enable co-op in the solo installation or claim complete solo/co-op feature parity. The source is a starting point for independent, backed-up testing. Two-PC gameplay and long-campaign balance remain open validation work.
