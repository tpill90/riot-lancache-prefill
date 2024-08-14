namespace RiotPrefill
{
    public static class Program
    {
        private static IAnsiConsole _ansiConsole = AnsiConsole.Console;

        public static async Task Main()
        {
            //AppConfig.CompareAgainstRealRequests = true;

            // Downloading manifest
            var parseTimer = Stopwatch.StartNew();
            var manifestHandler = new ManifestHandler(_ansiConsole);
            var manifestUrl = await manifestHandler.FindPatchlineReleaseAsync();
            var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(manifestUrl);

            // Standalone
            //var releaseInfo = await manifestHandler.FindLatestProductReleaseAsync(ArtifactType.LolStandaloneClientContent);
            //var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(releaseInfo);

            //LolGameClient
            //var releaseInfo = await manifestHandler.FindLatestProductReleaseAsync(ArtifactType.LolGameClient);
            //var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(releaseInfo);

            // Parsing manifest
            ReleaseManifest manifest = new ReleaseManifest(manifestPathOnDisk);
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

            // Combining requests to the same bundle into a single request
            var combinedRequests = new List<Request>();

            var groupedByBundle = downloadQueue.GroupBy(e => e.BundleKey).ToList();
            foreach (var bundle in groupedByBundle)
            {
                var chunked = bundle.Chunk(5).ToList();
                foreach (var chunk in chunked)
                {
                    var ranges = chunk.Select(e => new ByteRange(e.LowerByteRange, e.UpperByteRange)).ToList();
                    combinedRequests.Add(new Request(bundle.Key, ranges));
                }
            }

            using var downloader = new DownloadHandler(_ansiConsole);
            //await downloader.DownloadQueuedChunksAsync(downloadQueue);


            await downloader.DownloadQueuedChunksAsync(combinedRequests);
        }


    }
}
