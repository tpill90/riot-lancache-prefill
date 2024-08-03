# TODO comment
function EnsureRmanIsDownloaded()
{
    $downloadedZipPath = "$env:TEMP\RiotPrefill\rman.zip"

    if (Test-Path "$env:TEMP\RiotPrefill\bin\rman-dl.exe")
    {
        return
    }

    # rman will be saved to the user's temp dir
    Write-Color "Required dependency ", "Rman ", "not found.  Downloading now.." -Color Yellow, Cyan, Yellow

    # Finding latest release
    $versions = Invoke-RestMethod -Uri "https://api.github.com/repos/moonshadow565/rman/releases"
    $latestVersion = $versions[0].name
    $asset = $versions[0].assets[0]
    Write-Host "Found latest version : " -NoNewline
    Write-Host -ForegroundColor Cyan $latestVersion

    # Downloading
    Invoke-WebRequest $asset.browser_download_url -OutFile $downloadedZipPath

    # Unzipping
    Write-Host -ForegroundColor Yellow "Unzipping..."
    Expand-Archive -Force $downloadedZipPath -DestinationPath "$env:TEMP\RiotPrefill\"

    # Cleanup zip
    Remove-Item $downloadedZipPath
}

function CloneAndUpdateManifests()
{
    if (-Not(Test-Path "$env:TEMP\RiotPrefill\riot-manifests"))
    {
        git clone https://github.com/Morilli/riot-manifests.git "$env:TEMP\RiotPrefill\riot-manifests"
    }

    git pull
}

function DownloadManifest([string] $productName, [string] $assetName)
{
    $outputDir = "$env:TEMP\RiotPrefill\downloaded-manifests\$assetName"
    if(-Not(Test-Path $outputDir))
    {
        New-Item $outputDir -ItemType Directory | Out-Null
    }

    $latestManifest = Get-ChildItem "$env:TEMP\RiotPrefill\riot-manifests\$productName\NA1\windows\$assetName\" `
        | Sort-Object -Descending `
        | Select-Object -First 1
    $versionNumber = $latestManifest.Name.Replace(".txt", "")

    if (Test-Path "$outputDir\$versionNumber.txt")
    {
        return "$outputDir\$versionNumber.txt"
    }
    $latestManifestLink = $latestManifest | Get-Content

    Invoke-WebRequest $latestManifestLink -OutFile "$outputDir\$versionNumber.txt"

    return "$outputDir\$versionNumber.txt"
}