#requires -Version 5.1
#requires -PSEdition Desktop
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)][string]$GameRoot,
    [Parameter(Mandatory=$true)][string]$ProtectedSinglePlayerRoot,
    [Parameter(Mandatory=$true)][string]$Profile,
    [Parameter(Mandatory=$true)][string]$XrRazomMod,
    [Parameter(Mandatory=$true)][string]$EngineManifest,
    [string]$LauncherAssembly = (Join-Path $PSScriptRoot 'out\GAMMA Private Co-op.exe'),
    [switch]$AcknowledgeExperimental
)
$ErrorActionPreference = 'Stop'
if (-not $AcknowledgeExperimental) { throw 'Read coop/README.md, then pass -AcknowledgeExperimental for a separately prepared test copy.' }
Add-Type -AssemblyName System.Web.Extensions
[Reflection.Assembly]::LoadFrom([IO.Path]::GetFullPath($LauncherAssembly)) | Out-Null
$taskRoot = [GammaCoop.Core]::Canonical($GameRoot)
$taskProtected = [GammaCoop.Core]::Canonical($ProtectedSinglePlayerRoot)
if (-not (Test-Path -LiteralPath $taskProtected -PathType Container)) { throw 'The protected single-player root must exist.' }
if ($taskRoot.Equals($taskProtected, [StringComparison]::OrdinalIgnoreCase)) { throw 'The single-player root cannot be registered.' }
[GammaCoop.Core]::CheckPath($taskRoot, $taskRoot)
[GammaCoop.Core]::ValidComponent($Profile)
[GammaCoop.Core]::ValidComponent($XrRazomMod)
[GammaCoop.Core]::NoGameRunning($taskRoot)
$taskMarker = [GammaCoop.Core]::OwnedFile($taskRoot, @('coop-install.json'))
if (Test-Path -LiteralPath $taskMarker) { throw 'An installation marker already exists. Registration never overwrites it.' }
$taskSaves = [GammaCoop.Core]::OwnedFile($taskRoot, @('Anomaly','appdata','savedgames'))
if (@([GammaCoop.Core]::SafeFiles($taskRoot, $taskSaves)).Count -gt 0) { throw 'Register a fresh copy with no campaign saves. Copy test saves only after registration and backup.' }
$taskManifest = Get-Content -LiteralPath $EngineManifest -Raw | ConvertFrom-Json
if ($taskManifest.schema -ne 1 -or $taskManifest.engine -ne 'xrRazom 1.4') { throw 'Use an xrRazom 1.4 engine manifest with schema 1.' }
$taskVerified = @()
foreach ($taskRelative in @('Anomaly/bin/AnomalyDX11.exe','Anomaly/bin/GameNetworkingSockets.dll','Anomaly/bin/steam_api64.dll')) {
    $taskExpected = @($taskManifest.verified_files | Where-Object { $_.file -ceq $taskRelative })
    if ($taskExpected.Count -ne 1 -or $taskExpected[0].sha256 -cnotmatch '^[a-f0-9]{64}$' -or $taskExpected[0].sha256 -eq ('0' * 64)) { throw 'The engine manifest needs one genuine SHA256 for each required file.' }
    $taskPath = [GammaCoop.Core]::OwnedFile($taskRoot, @($taskRelative.Replace('/','\')))
    if (-not (Test-Path -LiteralPath $taskPath -PathType Leaf) -or [GammaCoop.Core]::Hash($taskPath) -cne $taskExpected[0].sha256) { throw 'A required engine file does not match the independently verified manifest.' }
    $taskVerified += @{ file=$taskRelative; sha256=$taskExpected[0].sha256 }
}
$taskInstall = @{schema=1;engine='xrRazom 1.4';profile=$Profile;mod=$XrRazomMod;original_root=$taskProtected;verified_files=$taskVerified}
# This writes only a new marker. It does not install mods, patch MO, copy saves,
# configure Tailscale, or launch the game. Validation rolls back this marker alone.
[GammaCoop.Core]::AtomicWrite($taskMarker, ($taskInstall | ConvertTo-Json -Depth 5), $taskRoot)
try { [GammaCoop.Core]::ValidateInstall($taskRoot) | Out-Null }
catch {
    [GammaCoop.Core]::CheckPath($taskMarker, $taskRoot)
    Remove-Item -LiteralPath $taskMarker
    throw
}
Write-Output 'Co-op test copy registered. This is structural validation, not proof of multiplayer compatibility.'
