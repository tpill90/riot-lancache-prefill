namespace RiotPrefill
{
    public static class Program
    {
        public static async Task Main()
        {
            var rootPath = @"C:\Users\Tim\AppData\Local\Temp\RiotPrefill\downloaded-manifests\";
            var allFiles = Directory.GetFiles($"{rootPath}", "*.*", SearchOption.AllDirectories);

            foreach (var manifestPath in allFiles)
            {
                await DownloadManifestAsync(manifestPath);
            }
        }

        private static async Task DownloadManifestAsync(string manifestPath)
        {
            var manifestParseTimer = Stopwatch.StartNew();
            var manifest = new ReleaseManifest(manifestPath);
            AnsiConsole.Console.LogMarkupLine("Finished parsing manifest", manifestParseTimer);

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
            //var filteredFiles = manifest.Files.Where(e => e.LanguageFlags == 0 || ((e.LanguageFlags & bitMask) != 0)).ToList();
            var filteredFiles = manifest.Files.ToList();

            //TODO figure out language flags
            var fileChunks = filteredFiles.SelectMany(e => e.ChunkIDs)
                                          .Select(e => BitConverter.GetBytes(e).ToHexString())
                                          .ToList();
            AnsiConsole.Console.MarkupLine($"Filtered down to {LightYellow(filteredFiles.Count)} files and {Cyan(fileChunks.Count)} chunks");

            var chunksToDownload = new List<BundleChunk>();
            foreach (var chunk in fileChunks)
            {
                if (allChunksLookup.ContainsKey(chunk))
                {
                    chunksToDownload.Add(allChunksLookup[chunk]);
                }
            }
            chunksToDownload = chunksToDownload.DistinctBy(e => e.ID).ToList();

            var totalSize = ByteSize.FromBytes(chunksToDownload.Sum(e => e.CompressedSize));

            AnsiConsole.Console.MarkupLine($"Total download size : {Magenta(totalSize.ToDecimalString())}");

            using var downloader = new DownloadHandler(AnsiConsole.Console);

            var requests = chunksToDownload
                           .Select(e => new Request(e.BundleId, e.bundle_offset, e.bundle_offset + e.CompressedSize))
                           .ToList();
            var coalesced = RequestUtils.CoalesceRequests(requests);

            AnsiConsole.Console.MarkupLine($"Combined to {LightYellow(coalesced.Count)} requests");
            await downloader.DownloadQueuedChunksAsync(coalesced);
            Debugger.Break();
        }
    }
}
