param(
    [Parameter()][Switch]$PackageOnly,
    [Parameter()][string]$LocalNugetPath
)

# This script is used to create a distribution folder that can be packaged into a zip file for release.
$projectDir = Join-Path $PSScriptRoot "Ookii.BinarySize"
$publishDir = Join-Path $projectDir "bin" "publish"
$zipDir = Join-Path $publishDir "zip"
New-Item $publishDir -ItemType Directory -Force | Out-Null
Remove-Item "$publishDir/*" -Recurse -Force
New-Item $zipDir -ItemType Directory -Force | Out-Null

[xml]$project = Get-Content (Join-Path $PSScriptRoot "Ookii.BinarySize/Ookii.BinarySize.csproj")
$frameworks = $project.Project.PropertyGroup.TargetFrameworks -split ";"
[xml]$props = Get-Content (Join-Path $PSScriptRoot "Directory.Build.Props")
$versionPrefix = $props.Project.PropertyGroup.VersionPrefix
$versionSuffix = $props.Project.PropertyGroup.VersionSuffix
if ($versionSuffix) {
    $version = "$versionPrefix-$versionSuffix"
} else {
    $version = $versionPrefix
}


# Clean and build deterministic.
dotnet clean "$PSScriptRoot" --configuration release
dotnet build "$PSScriptRoot" --configuration release /p:ContinuousIntegrationBuild=true

if (-not $PackageOnly) {
    # Publish each version of the library.
    foreach ($framework in $frameworks) {
        if ($framework) {
            dotnet publish --no-build "$PSScriptRoot/Ookii.BinarySize" --configuration Release --framework $framework --output "$zipDir/$framework"
        }
    }

    # Publish each sample
    $samples = Get-ChildItem -Directory "$PSScriptRoot/Samples"
    foreach ($sample in $samples) {
        $name = $sample.Name
        $publishArgs = "publish","--no-build",$sample,"--configuration","release","--output","$zipDir/Samples/$name"
        dotnet @publishArgs
    }

    # Copy global items.
    Copy-Item "$PSScriptRoot/../LICENSE.txt" $zipDir

    # Create readme.txt files.
    $url = "https://github.com/SvenGroot/Ookii.BinarySize"
    "For documentation and other information, see:",$url | Set-Content "$zipDir/readme.txt"
    "For descriptions of each sample, see:",$url | Set-Content "$zipDir/Samples/readme.txt"

    Compress-Archive "$zipDir/*" "$publishDir/Ookii.BinarySize-$version.zip"
}

# Copy packages
dotnet pack "$projectDir" --no-build --configuration Release --output "$publishDir"

if ($LocalNugetPath) {
    Copy-Item "$publishDir\*.nupkg" $LocalNugetPath
    Copy-Item "$publishDir\*.snupkg" $LocalNugetPath
    Remove-Item -Recurse "~/.nuget/packages/ookii.binarysize/*"
}
