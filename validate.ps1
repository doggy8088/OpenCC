[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "Validating formatting..."
dotnet format --verify-no-changes --no-restore OpenCC.sln
