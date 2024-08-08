namespace RiotPrefill
{
    public static class Program
    {
        public static IAnsiConsole _ansiConsole = AnsiConsole.Console;

        public static async Task Main()
        {
            AppConfig.CompareAgainstRealRequests = true;

            var manifestHandler = new ManifestHandler(_ansiConsole);
            //var latestRelease = await manifestHandler.FindLatestProductReleaseAsync(ArtifactType.LolGameClient);
            //var manifestBytes = await manifestHandler.DownloadManifestAsync(latestRelease);
            var manifestUrl = await manifestHandler.FindPatchlineReleaseAsync();
            var manifestBytes = await manifestHandler.DownloadManifestAsync(manifestUrl);

            var manifestParseTimer = Stopwatch.StartNew();
            var parsedManifest = new ReleaseManifest(manifestBytes);
            _ansiConsole.LogMarkupLine("Finished parsing manifest", manifestParseTimer);

            var downloadQueue = BuildDownloadQueue(parsedManifest);

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

        //TODO improve performance
        private static List<Request> BuildDownloadQueue(ReleaseManifest manifest)
        {
            var timer = Stopwatch.StartNew();

            var bundles = new Dictionary<string, ManifestBundle>();
            for (var index = 0; index < manifest.Bundles.Count; index++)
            {
                var originalBundle = manifest.Bundles[index];

                var bundle = new ManifestBundle(originalBundle);
                bundles.Add(bundle.ID, bundle);
            }

            var allChunksLookup = new Dictionary<string, BundleChunk>();
            var dupes = new List<BundleChunk>();
            foreach (var chunk in bundles.Values.SelectMany(e => e.Chunks).ToList())
            {
                if (!allChunksLookup.ContainsKey(chunk.ID))
                {
                    allChunksLookup.Add(chunk.ID, chunk);
                }
                else
                {
                    dupes.Add(chunk);
                }
            }

            ulong bitMask = 1 | (2 << 9);
            var filteredFiles = manifest.Files.Where(e => e.LanguageFlags == 0 || ((e.LanguageFlags & bitMask) != 0)).ToList();
            //var filteredFiles = manifest.Files.ToList();

            //TODO figure out language flags
            var fileChunks = filteredFiles.SelectMany(e => e.ChunkIDs)
                                          .Select(e => BitConverter.GetBytes(e).ToHexString())
                                          .ToList();
            _ansiConsole.LogMarkupLine($"Filtered down to {LightYellow(filteredFiles.Count)} files and {Cyan(fileChunks.Count)} chunks");

            var chunksToDownload = new List<BundleChunk>();
            foreach (var chunk in fileChunks)
            {
                if (allChunksLookup.ContainsKey(chunk))
                {
                    chunksToDownload.Add(allChunksLookup[chunk]);
                }
            }
            var chunksToDownloadDeduped = chunksToDownload.DistinctBy(e => e.ID).ToList();

            var requests = chunksToDownloadDeduped
                                     .Select(e => new Request(e.BundleId, e.bundle_offset, e.bundle_offset + e.CompressedSize))
                                     .ToList();
            //TODO these need to be combined into multiple ranges in the same request for a single bundle
            var coalesced = RequestUtils.CoalesceRequests(requests);

            var totalSize = ByteSize.FromBytes(coalesced.Sum(e => e.TotalBytes));
            _ansiConsole.LogMarkupLine($"Total download size : {Magenta(totalSize.ToDecimalString())}, with {LightYellow(coalesced.Count)} requests");

            _ansiConsole.LogMarkupLine("Download queue built", timer);
            return coalesced;
        }
    }
}
