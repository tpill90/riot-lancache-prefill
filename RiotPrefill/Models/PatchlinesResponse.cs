namespace RiotPrefill.Models
{
    public sealed class PatchlinesResponse
    {
        [JsonPropertyName("keystone.products.league_of_legends.patchlines.live")]
        public KeystoneProductsLeagueOfLegendsPatchlinesLive KeystoneProducts { get; set; }
    }

    public class KeystoneProductsLeagueOfLegendsPatchlinesLive
    {
        public Platforms platforms { get; set; }
        public string version { get; set; }
    }

    public class Platforms
    {
        public Mac1 mac { get; set; }
        public Win win { get; set; }
    }

    public class Mac1
    {
        public bool auto_patch { get; set; }
        public Configuration[] configurations { get; set; }
        public object dependencies { get; set; }
        public string deprecated_cloudfront_id { get; set; }
        public string install_dir { get; set; }
    }

    public class Configuration
    {
        public string[] allowed_http_fallback_hostnames { get; set; }
        public string bundles_url { get; set; }
        public object[] dependencies { get; set; }
        public object entitlements { get; set; }
        public string[] excluded_paths { get; set; }
        public string id { get; set; }
        public bool launchable_on_update_fail { get; set; }
        public Launcher launcher { get; set; }
        public string patch_notes_url { get; set; }
        public string patch_url { get; set; }
        public Region_Data region_data { get; set; }
        public Secondary_Patchlines[] secondary_patchlines { get; set; }
        public string seed_url { get; set; }
        public object[] tags { get; set; }
    }

    public class Launcher
    {
        public string[] arguments { get; set; }
        public string component_id { get; set; }
        public Executables executables { get; set; }
    }

    public class Executables
    {
        public string app { get; set; }
        public string exe { get; set; }
    }

    public class Alias6
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Region_Data
    {
        public string[] available_regions { get; set; }
        public string default_region { get; set; }
    }

    public class Secondary_Patchlines
    {
        public string[] allowed_http_fallback_hostnames { get; set; }
        public string bundles_url { get; set; }
        public object[] excluded_paths { get; set; }
        public string id { get; set; }
        public string path { get; set; }
        public object[] tags { get; set; }
        public string url { get; set; }
    }

    public class Win
    {
        public Configuration1[] configurations { get; set; }
        public object dependencies { get; set; }
    }

    public class Configuration1
    {
        public string[] allowed_http_fallback_hostnames { get; set; }
        public string bundles_url { get; set; }
        public Dependency[] dependencies { get; set; }
        public object entitlements { get; set; }
        public string[] excluded_paths { get; set; }
        public string id { get; set; }
        public bool launchable_on_update_fail { get; set; }
        public Launcher1 launcher { get; set; }
        public Locale_Data1 locale_data { get; set; }
        public Metadata2 metadata { get; set; }
        public string patch_notes_url { get; set; }
        public string patch_url { get; set; }
        public Region_Data1 region_data { get; set; }
        public Secondary_Patchlines1[] secondary_patchlines { get; set; }
        public string seed_url { get; set; }
        public object[] tags { get; set; }
    }

    public class Launcher1
    {
        public string[] arguments { get; set; }
        public string component_id { get; set; }
        public Executables1 executables { get; set; }
    }

    public class Executables1
    {
        public string app { get; set; }
        public string exe { get; set; }
    }

    public class Locale_Data1
    {
        public string[] available_locales { get; set; }
        public string default_locale { get; set; }
    }

    public class Metadata2
    {
        public Alias7 alias { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias7
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Region_Data1
    {
        public string[] available_regions { get; set; }
        public string default_region { get; set; }
    }

    public class Dependency
    {
        public string[] args { get; set; }
        public bool elevate { get; set; }
        public string hash { get; set; }
        public string id { get; set; }
        public string min_version { get; set; }
        public string url { get; set; }
        public string version { get; set; }
    }

    public class Secondary_Patchlines1
    {
        public string[] allowed_http_fallback_hostnames { get; set; }
        public string bundles_url { get; set; }
        public object[] excluded_paths { get; set; }
        public string id { get; set; }
        public string path { get; set; }
        public object[] tags { get; set; }
        public string url { get; set; }
    }

}