namespace RiotPrefill.Models
{
    //TODO document
    public sealed class BundleChunk
    {
        public string Id { get; }

        public string BundleId { get; }

        /// <summary>
        /// This is the lower bound of the chunk, the offset from the beginning of the file
        /// </summary>
        public uint OffsetFromStart { get; set; }
        public uint CompressedSize { get; }

        public uint UpperBound => OffsetFromStart + CompressedSize;




        public BundleChunk(ReleaseManifestBundleChunk source, string bundleId)
        {
            BundleId = bundleId;

            Id = BitConverter.GetBytes(source.ID).ToHexString();
            CompressedSize = source.CompressedSize;
        }

        public override string ToString()
        {
            return $"{Id} {BundleId} {CompressedSize} {OffsetFromStart}";
        }
    }
}