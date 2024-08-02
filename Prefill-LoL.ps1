
$MainFunction =
{
    if (-Not(Get-Command git -ErrorAction SilentlyContinue))
    {
        throw "This script requires git to be installed to continue!  Please install git and try again!"
    }

    EnsureRmanIsDownloaded
    CloneAndUpdateManifests

    # league client
    # $latestManifestLink = Get-ChildItem .\riot-manifests\LoL\NA1\windows\league-client\ | Sort-Object -Descending | Select-Object -First 1 | Get-Content
    # Invoke-WebRequest $latestManifestLink -OutFile currentManifest.txt
    # .\bin\rman-dl.exe .\currentManifest.txt --no-write --no-verify

    # lol-game-client
    $latestManifestLink = Get-ChildItem .\riot-manifests\LoL\NA1\windows\lol-game-client\ | Sort-Object -Descending | Select-Object -First 1 | Get-Content
    # Invoke-WebRequest $latestManifestLink -OutFile currentManifest.txt
    # .\bin\rman-dl.exe .\currentManifest.txt --no-write --no-verify --filter-lang en_US

    # # lol-standalone-client-content
    $latestManifestLink = Get-ChildItem .\riot-manifests\LoL\NA1\windows\lol-standalone-client-content\ | Sort-Object -Descending | Select-Object -First 1 | Get-Content
    Invoke-WebRequest $latestManifestLink -OutFile currentManifest.txt
    .\bin\rman-dl.exe .\currentManifest.txt --no-write --no-verify
}


function EnsureRmanIsDownloaded()
{
    $outputFilePath = "$env:TEMP\rman.zip"

    if (Test-Path ".\bin\rman-dl.exe")
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
    Invoke-WebRequest $asset.browser_download_url -OutFile $outputFilePath

    # Unzipping
    Write-Host -ForegroundColor Yellow "Unzipping..."
    Expand-Archive -Force $outputFilePath -DestinationPath .

    # Cleanup zip
    Remove-Item $outputFilePath
}

function CloneAndUpdateManifests()
{
    if(-Not(Test-Path riot-manifests))
    {
        git clone https://github.com/Morilli/riot-manifests.git
    }

    git pull
}

& $MainFunction