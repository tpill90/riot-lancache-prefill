namespace RiotPrefill
{
    public static class Program
    {
        private static IAnsiConsole _ansiConsole = AnsiConsole.Console;

        public static async Task Main()
        {
            AppConfig.CompareAgainstRealRequests = true;

            // Standalone
            //var releaseInfo = await manifestHandler.FindLatestProductReleaseAsync(ArtifactType.LolStandaloneClientContent);
            //var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(releaseInfo);

            //LolGameClient
            //var releaseInfo = await manifestHandler.FindLatestProductReleaseAsync(ArtifactType.LolGameClient);
            //var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(releaseInfo);

            // Downloading manifest
            var manifestHandler = new ManifestHandler(_ansiConsole);
            var manifestUrl = await manifestHandler.FindPatchlineReleaseAsync(Patchline.Valorant);
            var manifestPathOnDisk = await manifestHandler.DownloadManifestAsync(manifestUrl);

            // Parsing manifest
            var parseTimer = Stopwatch.StartNew();
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

            //TODO this needs to be moved over to the manifest handler  at some point
            // Combining requests to the same bundle into a single request
            var combinedRequests = new List<Request>();

            var groupedByBundle = downloadQueue.GroupBy(e => e.BundleKey).ToList();
            foreach (var bundle in groupedByBundle)
            {
                var ranges = bundle.OrderBy(e => e.LowerByteRange)
                                               .Select(e => new ByteRange(e.LowerByteRange, e.UpperByteRange))
                                               .ToList();
                combinedRequests.Add(new Request(bundle.Key, ranges));
            }


            var filteredToRangedOnly = combinedRequests.Where(e => e.ByteRanges.Count > 1).ToList();
            using var downloader = new DownloadHandler(_ansiConsole);
            await downloader.DownloadQueuedChunksAsync(combinedRequests);
        }
    }
}
