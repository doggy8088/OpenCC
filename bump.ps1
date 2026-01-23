[CmdletBinding()]
param(
    [ValidateSet("patch", "minor", "major")]
    [string]$Bump = "patch",
    [string]$Project = "src/OpenCC/OpenCC.csproj"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $Project)) {
    throw "Project file not found: $Project"
}

[xml]$xml = Get-Content -Path $Project
$versionNode = $xml.SelectSingleNode("//Project/PropertyGroup/Version")
if (-not $versionNode -or -not $versionNode.InnerText) {
    throw "Version element not found in $Project."
}

$versionText = $versionNode.InnerText.Trim()
if ($versionText -notmatch '^(\d+)\.(\d+)\.(\d+)$') {
    throw "Version '$versionText' is not in Major.Minor.Patch format."
}

$major = [int]$Matches[1]
$minor = [int]$Matches[2]
$patch = [int]$Matches[3]

switch ($Bump) {
    "major" {
        $major++
        $minor = 0
        $patch = 0
    }
    "minor" {
        $minor++
        $patch = 0
    }
    default {
        $patch++
    }
}

$newVersion = "$major.$minor.$patch"
$versionNode.InnerText = $newVersion
$xml.Save($Project)

Write-Host "Version bumped: $versionText -> $newVersion"
