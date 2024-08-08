namespace RiotPrefill.Handlers
{
    public sealed class ManifestHandler
    {
        private readonly IAnsiConsole _ansiConsole;
        private readonly HttpClient _httpClient;

        public ManifestHandler(IAnsiConsole ansiConsole)
        {
            _ansiConsole = ansiConsole;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "RiotNetwork/1.0.0");
        }

        public async Task<ReleaseInfo> FindLatestProductReleaseAsync(ArtifactType artifactType)
        {
            //TODO parameterize
            var apiUrl = $"https://sieve.services.riotcdn.net/api/v1/products/lol/version-sets/NA1?q[platform]=windows";
            using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            // Send request
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var releaseApiResponse = await JsonSerializer.DeserializeAsync(responseStream, SerializationContext.Default.ReleaseApiResponse);
            var releases = releaseApiResponse.releases;

            var latestVersion = releases.Where(e => e.Platform.Contains("windows"))
                                                    .Where(e => e.ArtifactTypeId == artifactType.Value)
                                                    .OrderByDescending(e => e.Version)
                                                    .First();

            _ansiConsole.LogMarkupLine($"Found latest version {LightYellow(latestVersion.Version)} for artifact {Cyan(latestVersion.ArtifactTypeId)}");
            return latestVersion;
        }

        public async Task<byte[]> DownloadManifestAsync(ReleaseInfo release)
        {
            // Load from disk if manifest already exists
            var cachedFileName = Path.Combine(AppConfig.CacheDir, $"{release._Release.product}-{release.ArtifactTypeId}-{release.Version}.manifest");
            if (ManifestIsCached(cachedFileName))
            {
                return await File.ReadAllBytesAsync(cachedFileName);
            }

            byte[] responseAsBytes = null;
            await _ansiConsole.StatusSpinner().StartAsync("Downloading manifest", async ctx =>
            {
                var timer = Stopwatch.StartNew();
                using var request = new HttpRequestMessage(HttpMethod.Get, release.DownloadUrl);

                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                responseAsBytes = await response.Content.ReadAsByteArrayAsync();
                // Cache to disk
                await File.WriteAllBytesAsync(cachedFileName, responseAsBytes);

                _ansiConsole.LogMarkupLine("Downloaded manifest", timer);
            });
            return responseAsBytes;
        }

        private bool ManifestIsCached(string manifestFileName)
        {
            return !AppConfig.NoLocalCache && File.Exists(manifestFileName);
        }

        public async Task<string> FindPatchlineReleaseAsync()
        {
            //TODO parameterize
            var apiUrl = $"https://clientconfig.rpg.riotgames.com/api/v1/config/public?namespace=keystone.products.league_of_legends.patchlines";
            using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            // Send request
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var releaseApiResponse = await JsonSerializer.DeserializeAsync(responseStream, SerializationContext.Default.PatchlinesResponse);
            var manifestUrl = releaseApiResponse.keystoneproductsleague_of_legendspatchlineslive.platforms.win.configurations.First(e => e.id == "NA").patch_url;


            return manifestUrl;
        }

        public async Task<byte[]> DownloadManifestAsync(string url)
        {
            // Load from disk if manifest already exists
            var cachedFileName = url.Split("/").Last();
            if (ManifestIsCached(cachedFileName))
            {
                return await File.ReadAllBytesAsync(cachedFileName);
            }

            byte[] responseAsBytes = null;
            await _ansiConsole.StatusSpinner().StartAsync("Downloading manifest", async ctx =>
            {
                var timer = Stopwatch.StartNew();
                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                responseAsBytes = await response.Content.ReadAsByteArrayAsync();
                // Cache to disk
                await File.WriteAllBytesAsync(cachedFileName, responseAsBytes);

                _ansiConsole.LogMarkupLine("Downloaded manifest", timer);
            });
            return responseAsBytes;
        }
    }
}