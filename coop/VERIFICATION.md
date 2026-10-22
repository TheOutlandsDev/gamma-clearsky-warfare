# Source-publication verification

Checked 2026-10-22 on Windows x64. These checks used synthetic files and mocked game/network APIs.

| Check | Result |
| --- | --- |
| .NET Framework C# compilation | Passed |
| C# launcher self-test | 29 checks passed |
| LuaJIT bridge test | 28 checks passed |
| PowerShell registration syntax | Passed |
| Registration against a synthetic independent installation in Windows PowerShell 5.1 | Passed |
| Public source scan for the original local account name, host address and installation paths | No matches |

The C# checks cover strict IPv4 and player validation, unsafe folder names, path bounds, distinct verified backups, console-key replacement, installation-marker requirements, content fingerprints, marker/mod path traversal, MO profile setting redirection, wrong executable paths, storage overrides and modified engine hashes. The bridge checks cover one-use request handling, host/join roles, expiry, invalid endpoints/ports, duplicate and malformed fields, deletion failures, missing networking, already-loaded worlds and redacted connection errors.

No actual game, MO, network session or firewall change was performed. The only executable launched for tests was the newly built mock/self-test program. Registration read process state and wrote only its synthetic fixture marker. Build outputs, fixture data and raw test reports are outside the publication tree.

**Not verified:** a real second-PC connection or gameplay synchronization, replication of the solo customization set, combat/inventory consistency, Warfare captures, map travel, persistence or reconnecting. Successful preflight checks do not establish any of these.

The existing installed launcher and bridge were read only. Their audited source hashes are:

```text
GammaCoopLauncher.cs
963f0700a9d82d94792c239a24c3f3bc73f369a81c2c25c1f0ba2edf10116f81
modxml_zzz_gamma_coop_launcher.script
248d5d421b9fe231254043601bedbb6c49515e60814b89028a48c579e3cc466e
```

The published copies intentionally differ because of portability, privacy and validation improvements. These hashes describe original glue source, not trusted third-party engine binaries or a multiplayer compatibility fingerprint.
