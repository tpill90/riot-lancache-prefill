namespace RiotPrefill.Models
{
    //TODO document
    public sealed class Bundle
    {
        /// <summary>
        /// The bundle identifier which will be referenced when making a CDN request
        /// </summary>
        public string Id { get; }

        public List<BundleChunk> Chunks { get; } = new List<BundleChunk>();

        public Bundle(ReleaseManifestBundle source)
        {
            Id =  string.Format("{0:X16}", source.ID);

            for (var i = 0; i < source.Chunks.Count; i++)
            {
                var currentChunk = source.Chunks[i];

                var newChunk = new BundleChunk
                {
                    Id = BitConverter.GetBytes(currentChunk.ID).ToHexString(),
                    BundleId = Id,

                    CompressedSize = currentChunk.CompressedSize
                };
                Chunks.Add(newChunk);

                if (i == 0)
                {
                    Chunks[i].OffsetFromStart = 0;
                }
                else
                {
                    Chunks[i].OffsetFromStart = Chunks[i - 1].OffsetFromStart + Chunks[i - 1].CompressedSize;
                }
            }
        }

        public override string ToString()
        {
            return Id;
        }
    }
}