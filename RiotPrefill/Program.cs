namespace RiotPrefill
{
    public static class Program
    {
        public static IAnsiConsole _ansiConsole = AnsiConsole.Console;

        public static async Task Main()
        {
            //AppConfig.CompareAgainstRealRequests = true;

            // Downloading manifest
            var parseTimer = Stopwatch.StartNew();
            var manifestHandler = new ManifestHandler(_ansiConsole);
            var manifestUrl = await manifestHandler.FindPatchlineReleaseAsync();
            var manifestBytes = await manifestHandler.DownloadManifestAsync(manifestUrl);

            // Parsing manifest
            ReleaseManifest manifest = new ReleaseManifest(manifestBytes);
            _ansiConsole.LogMarkupLine("Finished parsing manifest", parseTimer);

            // Building out queue
            var downloadQueue = manifestHandler.BuildDownloadQueue(manifest);

            if (AppConfig.CompareAgainstRealRequests)
            {
                var comparisonUtil = new ComparisonUtil();
                await comparisonUtil.CompareAgainstRealRequestsAsync(downloadQueue);
                return;
            }
            if (AppConfig.SkipDownloads)
            {
                return;
            }

            using var downloader = new DownloadHandler(_ansiConsole);
            await downloader.DownloadQueuedChunksAsync(downloadQueue);
        }


    }
}
