namespace RiotPrefill.Models
{
    //TODO document
    public sealed class BundleChunk
    {
        public string Id { get; set; }

        public string BundleId { get; set; }

        /// <summary>
        /// This is the lower bound of the chunk, the offset from the beginning of the file
        /// </summary>
        public uint OffsetFromStart { get; set; } = 0;
        public uint CompressedSize { get; set; }

        public uint UpperBound => OffsetFromStart + CompressedSize;

        public override string ToString()
        {
            return $"{Id} {BundleId} {CompressedSize} {OffsetFromStart}";
        }
    }
}