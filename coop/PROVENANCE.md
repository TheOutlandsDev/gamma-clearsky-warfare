# Co-op source provenance and publication audit

Reviewed 2026-10-22. Scope: original launcher/adapter glue and new publication helpers, rather than the xrRazom engine or GAMMA assets. No third-party engine implementation, game assets, upstream Lua scripts, binaries, saves or network/account records are included in this directory.

| Public file | Source and publication changes |
| --- | --- |
| `GammaCoopLauncher.cs` | Adapted from the locally authored assistant-assisted host/join launcher. It contains task-specific Windows UI, backup/validation, fingerprint and process-launch glue; no upstream engine implementation or third-party copyright/license header was found. Public changes remove the machine username and private endpoint defaults, strengthen path/INI/executable validation and add synthetic tests. It calls documented/runtime APIs supplied by separately acquired software. |
| `bridge/gamedata/scripts/modxml_zzz_gamma_coop_launcher.script` | Adapted from the locally authored one-shot launch-request bridge. It calls the existing X-Ray/xrRazom networking and callback APIs; it does not embed their implementation. Public changes reject malformed/duplicate fields, require request consumption and avoid printing private endpoints/raw connection errors. |
| `build.ps1` | New portable build wrapper, based on the local custom C# build workflow. Uses the installed Windows compiler. Build and self-test outputs remain local. |
| `Register-CoopCopy.ps1` | New publication helper, not a copy of upstream setup code. Creates one new marker for an explicitly chosen, already prepared independent test installation after user acknowledgement. No downloader, asset installer, automatic mod edits, firewall changes or game launch. |
| `engine-manifest.example.json` | New schema/example only, with deliberately invalid all-zero hash placeholders and no local paths or identifiers. It includes filenames required by the launcher, not binaries. |
| `tests/test_bridge.py` | Adapted from the original custom bridge test harness. Removes the original host address, local runtime paths, installed-profile scanning and writes to local diagnostic folders. All engine/network behavior is mocked. `100.64.0.1` is a synthetic fixture, not a recorded host. |
| `README.md`, `PROVENANCE.md`, `VERIFICATION.md` | New repository documentation and sanitized mock-test summary. Dependency links refer to their respective owners. |

The repository's [LICENSE](../LICENSE) applies to these original contributions. External dependencies retain their own licenses and permissions; that license does not grant rights over separately acquired software. This inspection is evidence of the local files' role and absence of bundled upstream implementation; it is not a claim that an automated audit can prove originality of every expression.

Excluded local files include the compiled launcher, launcher settings, host verification records, firewall repair helper, whole-game package/setup/upload scripts, full-game ZIPs, raw logs, private engine manifests and existing install markers. Nothing in this publication modifies the installed solo game or the existing co-op copy.
