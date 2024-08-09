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

            foreach (var sourceChunk in source.Chunks)
            {
                Chunks.Add(new BundleChunk(sourceChunk, Id));
            }
            for (var i = 0; i < Chunks.Count; i++)
            {
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