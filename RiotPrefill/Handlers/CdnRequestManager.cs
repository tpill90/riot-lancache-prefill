using System.Net.Http;
using System.Threading;

namespace RiotPrefill.Handlers
{
    public sealed class DownloadHandler : IDisposable
    {
        private readonly IAnsiConsole _ansiConsole;
        private string _currentCdn => "lol.dyn.riotcdn.net";
        private readonly HttpClient _client;

        /// <summary>
        /// The URL/IP Address where the Lancache has been detected.
        /// </summary>
        private string _lancacheAddress;

        public DownloadHandler(IAnsiConsole ansiConsole)
        {
            _ansiConsole = ansiConsole;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "RiotNetwork/1.0.0");
        }

        public async Task InitializeAsync()
        {
            if (_lancacheAddress == null)
            {
                _lancacheAddress = await LancacheIpResolver.ResolveLancacheIpAsync(_ansiConsole, _currentCdn);
            }
        }

        /// <summary>
        /// Attempts to download all queued requests.  If all downloads are successful, will return true.
        /// In the case of any failed downloads, the failed downloads will be retried up to 3 times.  If the downloads fail 3 times, then
        /// false will be returned
        /// </summary>
        /// <returns>True if all downloads succeeded.  False if any downloads failed 3 times in a row.</returns>
        public async Task<bool> DownloadQueuedChunksAsync(List<Request> queuedRequests)
        {
            await InitializeAsync();

            int retryCount = 0;
            var failedRequests = new ConcurrentBag<Request>();
            await _ansiConsole.CreateSpectreProgress(TransferSpeedUnit.Bits).StartAsync(async ctx =>
            {
                // Run the initial download
                failedRequests = await AttemptDownloadAsync(ctx, "Downloading..", queuedRequests);

                // Handle any failed requests
                while (failedRequests.Any() && retryCount < 2)
                {
                    retryCount++;
                    failedRequests = await AttemptDownloadAsync(ctx, $"Retrying  {retryCount}..", failedRequests.ToList(), forceRecache: true);
                }
            });

            // Handling final failed requests
            if (failedRequests.IsEmpty)
            {
                return true;
            }

            _ansiConsole.LogMarkupError($"Download failed! {LightYellow(failedRequests.Count)} requests failed unexpectedly, see {LightYellow("app.log")} for more details.");
            _ansiConsole.WriteLine();

            return false;
        }

        //TODO I don't like the number of parameters here, should maybe rethink the way this is written.
        /// <summary>
        /// Attempts to download the specified requests.  Returns a list of any requests that have failed for any reason.
        /// </summary>
        /// <param name="forceRecache">When specified, will cause the cache to delete the existing cached data for a request, and re-download it again.</param>
        /// <returns>A list of failed requests</returns>
        public async Task<ConcurrentBag<Request>> AttemptDownloadAsync(ProgressContext ctx, string taskTitle, List<Request> requestsToDownload,
                                                                                bool forceRecache = false)
        {
            double requestTotalSize = requestsToDownload.Sum(e => e.TotalBytes);
            var progressTask = ctx.AddTask(taskTitle, new ProgressTaskSettings { MaxValue = requestTotalSize });

            var failedRequests = new ConcurrentBag<Request>();

            await Parallel.ForEachAsync(requestsToDownload, new ParallelOptions { MaxDegreeOfParallelism = 25 }, body: async (request, _) =>
            {
                try
                {
                    var url = $"http://{_currentCdn}/channels/public/bundles/{request.BundleKey.ToUpper()}.bundle";
                    if (forceRecache)
                    {
                        url += "?nocache=1";
                    }
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    requestMessage.Headers.Host = _currentCdn;
                    requestMessage.Headers.Range = new RangeHeaderValue(request.LowerByteRange, request.UpperByteRange);

                    using var cts = new CancellationTokenSource();
                    using var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                    using Stream responseStream = await response.Content.ReadAsStreamAsync(cts.Token);
                    response.EnsureSuccessStatusCode();

                    // Don't save the data anywhere, so we don't have to waste time writing it to disk.
                    var buffer = new byte[4096];
                    while (await responseStream.ReadAsync(buffer, cts.Token) != 0)
                    {
                    }
                }
                catch (Exception e)
                {
                    failedRequests.Add(request);
                }
                progressTask.Increment(request.TotalBytes);
            });


            // Making sure the progress bar is always set to its max value, in-case some unexpected error leaves the progress bar showing as unfinished
            progressTask.Increment(progressTask.MaxValue);
            return failedRequests;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}