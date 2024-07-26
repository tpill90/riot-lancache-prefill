# Checking to see if dependencies are installed
if (-Not(Get-Module -Name PSWriteColor))
{
    #TODO consider not installing this package
    Install-Module PSWriteColor -Scope CurrentUser
}

$MainFunction =
{
    EnsureRmanIsDownloaded
}


function EnsureRmanIsDownloaded()
{
    $outputFilePath = "$env:TEMP\rman.zip"

    if (Test-Path "$env:TEMP\rman\rman-dl.exe")
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
    # Invoke-WebRequest $asset.browser_download_url -OutFile $outputFilePath

    # Unzipping
    Write-Host -ForegroundColor Yellow "Unzipping..."
    Expand-Archive -Force "rman.zip" -DestinationPath .
    # Copy-Item "$($asset.name.Replace('.zip', ''))\SteamPrefill.exe"

    # Cleanup
    Remove-Item $asset.name
    Remove-Item -Force -Recurse "$($asset.name.Replace('.zip', ''))"

}



& $MainFunction