namespace RiotPrefill.Models
{
    public class BundleChunk
    {
        public string BundleId { get; set; }
        public string ID { get; set; }
        public uint CompressedSize { get; set; }
        public uint UncompressedSize { get; set; }

        public uint bundle_offset { get; set; }

        public BundleChunk(ReleaseManifestBundleChunk source, string bundleId)
        {
            BundleId = bundleId;
            ID = BitConverter.GetBytes(source.ID).ToHexString();
            CompressedSize = source.CompressedSize;
            UncompressedSize = source.UncompressedSize;
        }

        public override string ToString()
        {
            return ID;
        }
    }

    public class ManifestFile
    {
        //public ManifestFile(ReleaseManifestFile source)
        //{
        //    ID = source.ID;
        //    ParentID = source.ParentID;
        //    Size = source.Size;
        //    Name = source.Name;
        //    LanguageFlags = source.LanguageFlags;
        //    Unknown5 = source.Unknown5;
        //    Unknown6 = source.Unknown6;
        //    ChunkIDs = source.ChunkIDs;
        //    Unk8 = source.Unk8;
        //    Link = source.Link;
        //    Unknown10 = source.Unknown10;
        //    ChunkingParametersIndex = source.ChunkingParametersIndex;
        //    Permissions = source.Permissions;
        //}

        public ulong ID { get; set; }

        public ulong ParentID { get; set; }

        public uint Size { get; set; }

        public string? Name { get; set; }

        public ulong LanguageFlags { get; set; }

        public byte Unknown5 { get; set; }

        public byte Unknown6 { get; set; }

        public IList<ulong>? ChunkIDs { get; set; }

        public byte Unk8 { get; set; }

        public string? Link { get; set; }

        public byte Unknown10 { get; set; }

        public byte ChunkingParametersIndex { get; set; }

        public byte Permissions { get; set; }
    }
}