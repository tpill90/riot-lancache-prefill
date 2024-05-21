using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotPrefill.Models
{
    public class BinaryData
    {
        public byte[] Data;
        public int Length;
    }

    public class Chunk
    {
        public uint CompressedSize;
        public uint UncompressedSize;
        public ulong ChunkId;
        public ulong BundleId;
        public int BundleOffset;
        public int FileOffset;
    }

    public class Bundle
    {
        public ulong BundleId;
        public List<Chunk> Chunks = new List<Chunk>();
    }

    public class Manifest
    {
        public List<Chunk> Chunks = new List<Chunk>();
        public List<Bundle> Bundles = new List<Bundle>();
        public List<RiotFile> Files = new List<RiotFile>();
        public List<Language> Languages = new List<Language>();
        public ulong ManifestId;
    }

    public class RiotFile
    {
        public uint FileSize;
        public string Link;
        public string Name;
        public List<Language> Languages = new List<Language>();
        public List<Chunk> Chunks = new List<Chunk>();
    }

    public class Language
    {
        public byte LanguageId;
        public string Name;
    }
}
