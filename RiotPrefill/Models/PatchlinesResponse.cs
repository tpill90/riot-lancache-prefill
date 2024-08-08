namespace RiotPrefill.Models
{
    public class PatchlinesResponse
    {
        [JsonPropertyName("keystone.products.league_of_legends.patchlines.live")]
        public KeystoneProductsLeague_Of_LegendsPatchlinesLive keystoneproductsleague_of_legendspatchlineslive { get; set; }
    }

    public class KeystoneProductsLeague_Of_LegendsPatchlinesLive
    {
        public Metadata metadata { get; set; }
        public Platforms platforms { get; set; }
        public string version { get; set; }
    }

    public class Metadata
    {
        public Default2 _default { get; set; }
        public Hkg hkg { get; set; }
        public Installer installer { get; set; }
        public Mac mac { get; set; }
        public Twn twn { get; set; }
        public Vnm vnm { get; set; }
    }

    public class Default2
    {
        public Alias alias { get; set; }
        public string[] available_platforms { get; set; }
        public string client_product_type { get; set; }
        public Content_Paths content_paths { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string path_name { get; set; }
        public string rso_client_id { get; set; }
    }

    public class Alias
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Content_Paths
    {
        public string loc { get; set; }
        public string riotstatus { get; set; }
        public string social { get; set; }
    }

    public class Hkg
    {
        public Alias1 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias1
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Installer
    {
        public Alias2 alias { get; set; }
    }

    public class Alias2
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Mac
    {
        public Alias3 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias3
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Twn
    {
        public Alias4 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias4
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Vnm
    {
        public Alias5 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias5
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
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
        public Locale_Data locale_data { get; set; }
        public Metadata1 metadata { get; set; }
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

    public class Locale_Data
    {
        public string[] available_locales { get; set; }
        public string default_locale { get; set; }
    }

    public class Metadata1
    {
        public Alias6 alias { get; set; }
        public string theme_manifest { get; set; }
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
        public bool auto_patch { get; set; }
        public Configuration1[] configurations { get; set; }
        public object dependencies { get; set; }
        public string deprecated_cloudfront_id { get; set; }
        public string icon_path { get; set; }
        public string install_dir { get; set; }
        public string[] rogue_process_checklist { get; set; }
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


    public class Metadata3
    {
        public Default1 _default { get; set; }
        public Hkg1 hkg { get; set; }
        public Installer1 installer { get; set; }
        public Mac2 mac { get; set; }
        public Twn1 twn { get; set; }
        public Vnm1 vnm { get; set; }
    }

    public class Default1
    {
        public Alias8 alias { get; set; }
        public string[] available_platforms { get; set; }
        public string client_product_type { get; set; }
        public Content_Paths1 content_paths { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string path_name { get; set; }
        public string rso_client_id { get; set; }
    }

    public class Alias8
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Content_Paths1
    {
        public string loc { get; set; }
        public string riotstatus { get; set; }
        public string social { get; set; }
    }

    public class Hkg1
    {
        public Alias9 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias9
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Installer1
    {
        public Alias10 alias { get; set; }
    }

    public class Alias10
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Mac2
    {
        public Alias11 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias11
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Twn1
    {
        public Alias12 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias12
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }

    public class Vnm1
    {
        public Alias13 alias { get; set; }
        public string default_theme_manifest { get; set; }
        public string full_name { get; set; }
        public string theme_manifest { get; set; }
    }

    public class Alias13
    {
        public object platforms { get; set; }
        public string product_id { get; set; }
    }
}