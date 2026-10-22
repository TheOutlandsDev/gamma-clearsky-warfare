[CmdletBinding()]
param([string]$OutputDirectory, [switch]$SelfTest)
$ErrorActionPreference = 'Stop'
# Resolve script-relative defaults after Windows PowerShell 5.1 binds parameters.
if (-not $PSBoundParameters.ContainsKey('OutputDirectory')) {
    $OutputDirectory = Join-Path -Path $PSScriptRoot -ChildPath 'out'
}
$taskCompiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $taskCompiler -PathType Leaf)) { throw 'Windows .NET Framework 4.x C# compiler is required.' }
$taskOutput = [IO.Path]::GetFullPath($OutputDirectory)
[IO.Directory]::CreateDirectory($taskOutput) | Out-Null
$taskExecutable = Join-Path $taskOutput 'GAMMA Private Co-op.exe'
& $taskCompiler /nologo /target:winexe /optimize+ /platform:x64 "/out:$taskExecutable" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Web.Extensions.dll (Join-Path $PSScriptRoot 'GammaCoopLauncher.cs')
if ($LASTEXITCODE -ne 0) { throw 'Launcher compilation failed.' }
if ($SelfTest) {
    # Only synthetic files are used. No game, VPN, Steam or firewall APIs run.
    $taskProcess = Start-Process -FilePath $taskExecutable -ArgumentList '--self-test' -WindowStyle Hidden -PassThru -Wait
    if ($taskProcess.ExitCode -ne 0) { throw 'Mock self-test failed; inspect self-test-results.txt in the output directory.' }
    Get-Content -LiteralPath (Join-Path $taskOutput 'self-test-results.txt')
}
Write-Output 'Build complete. Generated binaries and test fixtures are local build products; do not commit them.'
