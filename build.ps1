[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Output = "artifacts",
    [switch]$SkipTests
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "Restoring..."
dotnet restore OpenCC.sln

Write-Host "Formatting..."
dotnet format OpenCC.sln --no-restore

Write-Host "Building ($Configuration)..."
dotnet build OpenCC.sln -c $Configuration --no-restore

if (-not $SkipTests) {
    Write-Host "Testing ($Configuration)..."
    dotnet test tests/OpenCC.Tests/OpenCC.Tests.csproj -c $Configuration --no-build
}

New-Item -ItemType Directory -Force -Path $Output | Out-Null

Write-Host "Packing ($Configuration)..."
dotnet pack src/OpenCC/OpenCC.csproj -c $Configuration -o $Output --no-build

Write-Host "Done. Packages are in: $Output"
