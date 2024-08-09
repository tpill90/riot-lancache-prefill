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
            return $"{ID} {BundleId} {CompressedSize} {bundle_offset}";
        }
    }
}