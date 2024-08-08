namespace RiotPrefill.Models
{
    //TODO this whole thing is awful, need to trim whats not needed and rename what is
    public class ReleaseInfo
    {
        public Release[] releases { get; set; }
    }

    public class Release
    {
        public Release1 release { get; set; }
        public Compat_Version compat_version { get; set; }
        public Download download { get; set; }
    }

    public class Release1
    {
        public string product { get; set; }
        public string id { get; set; }
        public Labels labels { get; set; }

        public string ArtifactTypeId => labels.riotartifact_type_id.values.First();

        private Version _version;
        public Version Version
        {
            get
            {
                if (_version == null)
                {
                    var artifactVersionIdAsString = labels.riotartifact_version_id.values.First();
                    var split = artifactVersionIdAsString.Split("+");
                    _version = Version.Parse(split[0]);
                }
                return _version;
            }
        }
    }

    public class Labels
    {
        public Buildtracker_Config buildtracker_config { get; set; }
        public Code code { get; set; }
        public Content content { get; set; }
        public Cpuarch cpuarch { get; set; }
        public Platform platform { get; set; }


        public RiotAnticheat_Option riotanticheat_option { get; set; }

        [JsonPropertyName("riot:artifact_type_id")]
        public RiotArtifact_Type_Id riotartifact_type_id { get; set; }

        [JsonPropertyName("riot:artifact_version_id")]
        public RiotArtifact_Version_Id riotartifact_version_id { get; set; }

        public RiotCpu_Arch riotcpu_arch { get; set; }
        public RiotPlatform riotplatform { get; set; }
        public RiotRevision riotrevision { get; set; }
    }

    public class Buildtracker_Config
    {
        public string[] values { get; set; }
    }

    public class Code
    {
        public string[] values { get; set; }
    }

    public class Content
    {
        public string[] values { get; set; }
    }

    public class Cpuarch
    {
        public string[] values { get; set; }
    }

    public class Platform
    {
        public string[] values { get; set; }
    }

    public class RiotAnticheat_Option
    {
        public string[] values { get; set; }
    }

    public class RiotArtifact_Type_Id
    {
        public string[] values { get; set; }
    }

    public class RiotArtifact_Version_Id
    {
        public string[] values { get; set; }
    }

    public class RiotCpu_Arch
    {
        public string[] values { get; set; }
    }

    public class RiotPlatform
    {
        public string[] values { get; set; }
    }

    public class RiotRevision
    {
        public string[] values { get; set; }
    }

    public class Compat_Version
    {
        public string id { get; set; }
    }

    public class Download
    {
        public string url { get; set; }
        public bool scd_required { get; set; }
    }

}