namespace RiotPrefill.Models
{
    /// <summary>
    /// Model that represents a request that could be made to a CDN.
    /// </summary>
    public sealed class Request
    {
        public Request()
        {

        }

        public Request(string bundleKey, long? startBytes = null, long? endBytes = null)
        {
            BundleKey = bundleKey;

            LowerByteRange = startBytes.Value;
            UpperByteRange = endBytes.Value;
        }

        public string BundleKey { get; set; }

        public long LowerByteRange { get; set; }
        public long UpperByteRange { get; set; }

        // Bytes are an inclusive range.  Ex bytes 0->9 == 10 bytes
        public long TotalBytes => UpperByteRange - LowerByteRange + 1;

        public override string ToString()
        {
            return $"{LowerByteRange}-{UpperByteRange}";
        }

        public bool Overlaps(Request request2)
        {
            int overlap = 4096 * 1;

            if (LowerByteRange <= request2.LowerByteRange)
            {
                // Checks to see if ranges are overlapping ex 0-100 and 50-200
                var areOverlapping = UpperByteRange >= request2.LowerByteRange;
                if (!areOverlapping)
                {
                    // Seeing if adjacent ranges can be combined, ex 0-100 and 101-200
                    if (UpperByteRange + overlap >= request2.LowerByteRange)
                    {
                        return true;
                    }
                }
                return areOverlapping;
            }
            return request2.UpperByteRange >= LowerByteRange;
        }

        public void MergeWith(Request request2)
        {
            LowerByteRange = Math.Min(LowerByteRange, request2.LowerByteRange);
            UpperByteRange = Math.Max(UpperByteRange, request2.UpperByteRange);
        }
    }
}
