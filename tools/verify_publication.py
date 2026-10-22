"""Check the source-only publication tree without touching any game installation.

AGPL-3.0-only. Python standard library only; run from any working directory.
This validates inventory/packaging, not multiplayer behavior or legal ownership.
"""
from __future__ import annotations

import hashlib
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
INVENTORY = ROOT / "SOURCE-INVENTORY.json"
ALLOWED_SUFFIXES = {".md", ".json", ".yml", ".yaml", ".py", ".ps1", ".cs", ".script", ".ltx"}
SPECIAL_FILES = {"LICENSE", ".gitignore"}
PRIVATE_NAMES = {"launcher-settings.json", "coop-install.json", "inspection.json", "self-test-results.txt"}


def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def tracked_files() -> list[Path]:
    result = []
    for path in sorted(ROOT.rglob("*")):
        if ".git" in path.relative_to(ROOT).parts:
            continue
        if path.is_symlink():
            raise ValueError("Linked content cannot be published: " + path.relative_to(ROOT).as_posix())
        if path.is_file():
            result.append(path)
    return result


def verify() -> None:
    files = tracked_files()
    expected = json.loads(INVENTORY.read_text(encoding="utf-8"))
    records = expected["files"]
    expected_paths = {entry["path"] for entry in records}
    actual_paths = {p.relative_to(ROOT).as_posix() for p in files if p != INVENTORY}
    if len(records) != len(expected_paths) or expected_paths != actual_paths:
        raise ValueError("Publication inventory differs from the source tree; review additions/removals before regenerating it.")
    for record in records:
        relative = record["path"]
        path = ROOT / relative
        if Path(relative).is_absolute() or ".." in Path(relative).parts:
            raise ValueError("Inventory path escaped the repository.")
        if path.name in PRIVATE_NAMES or (path.suffix.lower() not in ALLOWED_SUFFIXES and path.name not in SPECIAL_FILES):
            raise ValueError("Private/generated or unexpected file type: " + relative)
        data = path.read_bytes()
        if len(data) != record["bytes"] or digest(path) != record["sha256"]:
            raise ValueError("Content hash mismatch: " + relative)
        if len(data) > 1024 * 1024 or b"\0" in data:
            raise ValueError("Binary or oversized content: " + relative)
        text = data.decode("utf-8-sig")
        if re.search(r"[A-Za-z]:[\\/]Users[\\/][^\s]+", text, re.IGNORECASE):
            raise ValueError("Personal Windows path in: " + relative)
        if re.search(r"(?:gh[pousr]_[A-Za-z0-9]{25,}|github_pat_[A-Za-z0-9_]{25,}|tskey-[A-Za-z0-9_-]{16,})", text):
            raise ValueError("Potential access token in: " + relative)
        if path.suffix == ".json":
            json.loads(text)
        if path.suffix == ".md":
            for target in re.findall(r"\]\(([^\s)]+)(?:\s+[^)]*)?\)", text):
                if ":" in target or target.startswith("#"):
                    continue
                target = target.split("#", 1)[0]
                if target and not (path.parent / target).resolve().exists():
                    raise ValueError("Broken relative Markdown link in " + relative + ": " + target)
    provenance = json.loads((ROOT / "addons/provenance.json").read_text(encoding="utf-8-sig"))
    for entry in provenance["files"]:
        if digest(ROOT / "addons" / entry["path"]) != entry["sha256"]:
            raise ValueError("Original addon provenance hash mismatch.")
    for name in ("solo_bug.yml", "coop_bug.yml", "balance_feedback.yml", "feature_request.yml", "config.yml"):
        if not (ROOT / ".github/ISSUE_TEMPLATE" / name).is_file():
            raise ValueError("Missing feedback form: " + name)
    print(json.dumps({"status": "PASS", "source_files": len(records), "bytes": sum(r["bytes"] for r in records), "binary_payloads": 0, "gameplay_validation": False}))


if __name__ == "__main__":
    try:
        verify()
    except (ValueError, OSError, KeyError, UnicodeError) as error:
        print("Publication check failed: " + str(error), file=sys.stderr)
        raise SystemExit(1)
