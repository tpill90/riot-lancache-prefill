namespace RiotPrefill.Models
{
    public sealed class ManifestBundle
    {
        public string ID { get; set; }

        public List<BundleChunk> Chunks { get; set; } = new List<BundleChunk>();

        public ManifestBundle(ReleaseManifestBundle source)
        {
            var bytes = BitConverter.GetBytes(source.ID);
            //bytes.AsSpan(0, 4).Reverse();
            //bytes.AsSpan(4, 4).Reverse();
            //bytes.AsSpan(8, 4).Reverse();
            //bytes.AsSpan(12, 4).Reverse();
            var ID2 = HexMate.Convert.ToHexString(bytes);
            ID =  string.Format("{0:X16}", source.ID);

            foreach (var sourceChunk in source.Chunks)
            {
                Chunks.Add(new BundleChunk(sourceChunk, ID));
            }
            for (var i = 0; i < Chunks.Count; i++)
            {
                var bundle_offset = i == 0 ? 0 : Chunks[i - 1].bundle_offset + Chunks[i - 1].CompressedSize;
                Chunks[i].bundle_offset = bundle_offset;
            }
        }

        public override string ToString()
        {
            return ID;
        }
    }
}