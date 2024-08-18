using CommunityToolkit.HighPerformance;

namespace RiotPrefill
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            try
            {
                // Checking to see if the user double clicked the exe in Windows, and display a message on how to use the app
                OperatingSystemUtils.DetectDoubleClickOnWindows("SteamPrefill");

                var cliArgs = ParseHiddenFlags();
                var description = "Automatically fills a Lancache with games from Riot, so that subsequent downloads will be \n" +
                                  "  served from the Lancache, improving speeds and reducing load on your internet connection. \n";

                return await new CliApplicationBuilder()
                             .AddCommandsFromThisAssembly()
                             .SetTitle("RiotPrefill")
                             .SetExecutableNamePlatformAware("RiotPrefill")
                             .SetDescription(description)
                             .SetVersion($"v{ThisAssembly.Info.InformationalVersion}")
                             .Build()
                             .RunAsync(cliArgs);
            }
            catch (Exception e)
            {
                AnsiConsole.Console.LogException(e);
            }

            // Return failed status code, since you can only get to this line if an exception was handled
            return 1;
        }

        /// <summary>
        /// Adds hidden flags that may be useful for debugging/development, but shouldn't be displayed to users in the help text
        /// </summary>
        private static List<string> ParseHiddenFlags()
        {
            // Have to skip the first argument, since its the path to the executable
            var args = Environment.GetCommandLineArgs().Skip(1).ToList();

            // Enables SteamKit2 debugging as well as SteamPrefill verbose logs
            if (args.Any(e => e.Contains("--debug")))
            {
                AnsiConsole.Console.LogMarkupLine($"Using {LightYellow("--debug")} flag.  Displaying debug only logging...");
                AnsiConsole.Console.LogMarkupLine($"Additional debugging files will be output to {Magenta(AppConfig.DebugOutputDir)}");
                AppConfig.DebugLogs = true;
                args.Remove("--debug");
            }

            // Will skip over downloading logic.  Will only download manifests
            if (args.Any(e => e.Contains("--no-download")))
            {
                AnsiConsole.Console.LogMarkupLine($"Using {LightYellow("--no-download")} flag.  Will skip downloading chunks...");
                AppConfig.SkipDownloads = true;
                args.Remove("--no-download");
            }

            // Skips using locally cached manifests. Saves disk space, at the expense of slower subsequent runs.
            // Useful for debugging since the manifests will always be re-downloaded.
            if (args.Any(e => e.Contains("--nocache")) || args.Any(e => e.Contains("--no-cache")))
            {
                AnsiConsole.Console.LogMarkupLine($"Using {LightYellow("--nocache")} flag.  Will always re-download manifests...");
                AppConfig.NoLocalCache = true;
                args.Remove("--nocache");
                args.Remove("--no-cache");
            }

            // Adding some formatting to logging to make it more readable + clear that these flags are enabled
            if (AppConfig.DebugLogs || AppConfig.SkipDownloads || AppConfig.NoLocalCache)
            {
                AnsiConsole.Console.WriteLine();
                AnsiConsole.Console.Write(new Rule());
            }

            return args;
        }


    }
}
