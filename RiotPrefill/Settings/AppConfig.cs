namespace RiotPrefill.Settings
{
    public static class AppConfig
    {
        static AppConfig()
        {
            // Create required folders
            Directory.CreateDirectory(ConfigDir);
            Directory.CreateDirectory(CacheDir);
        }

        private static bool _verboseLogs;
        public static bool VerboseLogs
        {
            get => _verboseLogs;
            set
            {
                _verboseLogs = value;
                AnsiConsoleExtensions.WriteVerboseLogs = value;
            }
        }

        /// <summary>
        /// Downloaded manifests, as well as other metadata, are saved into this directory to speedup future prefill runs.
        /// All data in here should be able to be deleted safely.
        /// </summary>
        public static readonly string CacheDir = CacheDirUtils.GetCacheDirBaseDirectories("RiotPrefill", CacheDirVersion);

        /// <summary>
        /// Increment when there is a breaking change made to the files in the cache directory
        /// </summary>
        private const string CacheDirVersion = "v1";

        /// <summary>
        /// Contains user configuration.  Should not be deleted, doing so will reset the app back to defaults.
        /// </summary>
        private static readonly string ConfigDir = Path.Combine(AppContext.BaseDirectory, "Config");

        #region Serialization file paths

        public static readonly string UserSelectedAppsPath = Path.Combine(ConfigDir, "selectedAppsToPrefill.json");

        /// <summary>
        /// Keeps track of which depots have been previously downloaded.  Is used to determine whether or not a game is up to date,
        /// based on whether all of the depots being downloaded are up to date.
        /// </summary>
        //TODO implement
        public static readonly string SuccessfullyDownloadedDepotsPath = Path.Combine(ConfigDir, "successfullyDownloadedDepots.json");

        #endregion

        #region Debugging

        public static readonly string DebugOutputDir = Path.Combine(CacheDir, "Debugging");

        /// <summary>
        /// Skips using locally cached manifests. Saves disk space, at the expense of slower subsequent runs.  Intended for debugging.
        /// </summary>
        public static bool NoLocalCache { get; set; }

        /// <summary>
        /// Will skip over downloading chunks, but will still download manifests and build the chunk download list.  Useful for testing
        /// core logic of SteamPrefill without having to wait for downloads to finish.
        /// </summary>
        public static bool SkipDownloads { get; set; }

        private static bool _debugLogs;
        public static bool DebugLogs
        {
            get => _debugLogs;
            set
            {
                _debugLogs = value;

                // Enable verbose logs as well
                VerboseLogs = true;
            }
        }

        #endregion
    }
}