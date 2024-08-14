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
        [JsonPropertyName("win")]
        public Win Win { get; set; }
    }

    public class Win
    {
        public Configuration1[] configurations { get; set; }
        public object dependencies { get; set; }
    }

    public class Configuration1
    {
        public string id { get; set; }
        public string patch_url { get; set; }
    }
}