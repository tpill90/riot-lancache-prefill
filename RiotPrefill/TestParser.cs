using RiotPrefill.Models;

namespace RiotPrefill
{
    public static class TestParser
    {
        public static Manifest ParseManifestData(byte[] data)
        {
            if (Encoding.ASCII.GetString(data, 0, 4) != "RMAN")
            {
                throw new Exception("Not a valid RMAN file! Missing magic bytes.");
            }

            if (data[4] == 2 && data[5] != 0)
            {
                throw new Exception($"Info: Untested manifest version {data[4]}.{data[5]} detected. Everything should still work though.");
            }
            else if (data[4] != 2)
            {
                throw new Exception($"Warning: Probably unsupported manifest version {data[4]}.{data[5]} detected. Will continue, but it might not work.");
            }

            var manifest = new Manifest();
            uint contentOffset = BitConverter.ToUInt32(data, 8);
            uint compressedSize = BitConverter.ToUInt32(data, 12);
            manifest.ManifestId = BitConverter.ToUInt64(data, 16);
            uint uncompressedSize = BitConverter.ToUInt32(data, 24);

            var decompressedBody = new byte[uncompressedSize];
            //using (var decompressor = new Decompressor())
            //{
            //    decompressor.Unwrap(decompressedBody, data.Skip((int)contentOffset).Take((int)compressedSize).ToArray());
            //}

            ParseBody(manifest, decompressedBody);
            return manifest;
        }

        public static bool ChunkValid(BinaryData chunk, ulong chunkId)
        {
            using (var sha256 = SHA256.Create())
            {
                var key = sha256.ComputeHash(chunk.Data);
                var ipad = key.Select(b => (byte)(b ^ 0x36)).ToArray();
                var opad = key.Select(b => (byte)(b ^ 0x5C)).ToArray();

                var buffer = new byte[32];
                var index = new byte[] { 0, 0, 0, 1 };

                buffer = sha256.ComputeHash(ipad.Concat(index).ToArray());
                buffer = sha256.ComputeHash(opad.Concat(buffer).ToArray());

                var result = new byte[8];
                Array.Copy(buffer, result, 8);

                for (int i = 0; i < 31; i++)
                {
                    buffer = sha256.ComputeHash(ipad.Concat(buffer).ToArray());
                    buffer = sha256.ComputeHash(opad.Concat(buffer).ToArray());

                    for (int j = 0; j < 8; j++)
                    {
                        result[j] ^= buffer[j];
                    }
                }

                return result.SequenceEqual(BitConverter.GetBytes(chunkId).Reverse().Take(8));
            }
        }

        public static List<Bundle> GroupByBundles(List<Chunk> chunks)
        {
            var uniqueBundles = new List<Bundle>();
            foreach (var chunk in chunks)
            {
                var bundle = uniqueBundles.FirstOrDefault(b => b.BundleId == chunk.BundleId);
                if (bundle == null)
                {
                    bundle = new Bundle { BundleId = chunk.BundleId };
                    uniqueBundles.Add(bundle);
                }
                bundle.Chunks.Add(chunk);
            }
            return uniqueBundles;
        }

        public static void FreeManifest(Manifest manifest)
        {
            manifest.Chunks.Clear();
            foreach (var bundle in manifest.Bundles)
            {
                bundle.Chunks.Clear();
            }
            manifest.Bundles.Clear();
            foreach (var file in manifest.Files)
            {
                file.Languages.Clear();
                file.Chunks.Clear();
            }
            manifest.Files.Clear();
            manifest.Languages.Clear();
        }

        public static string DuplicateString(string str)
        {
            return string.Copy(str);
        }

        public static int ParseBody(Manifest manifest, byte[] body)
        {
            //FlatBufferObject rootObject = FlatBufferObject_of(body);
            // bundles (and their chunks)
            //var bundleOffsets = rootObject.GetOffsetVector(0);
            //manifest.Bundles = new List<Bundle>(bundleOffsets.Length);
            //uint totalChunks = 0;

            //for (int i = 0; i < bundleOffsets.Length; i++)
            //{
            //    var bundleObject = new FlatBufferObject(bundleOffsets[i]);
            //    var newBundle = new Bundle
            //    {
            //        BundleId = bundleObject.GetUInt64(0),
            //        Chunks = new List<Chunk>()
            //    };

            //    var chunkOffsets = bundleObject.GetOffsetVector(1);
            //    for (int j = 0; j < chunkOffsets.Length; j++)
            //    {
            //        var chunkObject = new FlatBufferObject(chunkOffsets[j]);
            //        var newChunk = new Chunk
            //        {
            //            CompressedSize = chunkObject.GetUInt32(1),
            //            UncompressedSize = chunkObject.GetUInt32(2),
            //            ChunkId = chunkObject.GetUInt64(0),
            //            BundleOffset = (uint)(j == 0 ? 0 : newBundle.Chunks[j - 1].BundleOffset + newBundle.Chunks[j - 1].CompressedSize),
            //            BundleId = newBundle.BundleId
            //        };
            //        newBundle.Chunks.Add(newChunk);
            //    }
            //    manifest.Bundles.Add(newBundle);
            //    totalChunks += (uint)chunkOffsets.Length;
            //}

            //manifest.Chunks = new List<Chunk>((int)totalChunks);
            //foreach (var bundle in manifest.Bundles)
            //{
            //    manifest.Chunks.AddRange(bundle.Chunks);
            //}

            //manifest.Chunks.Sort((x, y) => x.ChunkId.CompareTo(y.ChunkId));

            //// languages
            //var languageOffsets = rootObject.GetOffsetVector(1);
            //manifest.Languages = new List<Language>(languageOffsets.Length);
            //for (int i = 0; i < languageOffsets.Length; i++)
            //{
            //    var languageObject = new FlatBufferObject(languageOffsets[i]);
            //    var newLanguage = new Language
            //    {
            //        LanguageId = languageObject.GetByte(0),
            //        Name = languageObject.GetString(1)
            //    };
            //    manifest.Languages.Add(newLanguage);
            //}

            //// file entries
            //var fileEntryOffsets = rootObject.GetOffsetVector(2);
            //var fileEntries = new List<FileEntry>(fileEntryOffsets.Length);
            //for (int i = 0; i < fileEntryOffsets.Length; i++)
            //{
            //    var fileEntryObject = new FlatBufferObject(fileEntryOffsets[i]);
            //    var newFileEntry = new FileEntry
            //    {
            //        FileEntryId = fileEntryObject.GetUInt64(0),
            //        DirectoryId = fileEntryObject.GetOptionalUInt64(1),
            //        FileSize = fileEntryObject.GetUInt32(2),
            //        Name = fileEntryObject.GetString(3),
            //        Link = fileEntryObject.GetString(9),
            //        ChunkIds = fileEntryObject.GetOffsetVector(7).Select(fo => fo.GetUInt64(0)).ToList()
            //    };

            //    ulong languageMask = fileEntryObject.GetOptionalUInt64(4);
            //    newFileEntry.LanguageIds = new List<byte>();
            //    for (int j = 0; j < 64; j++)
            //    {
            //        if ((languageMask & (1UL << j)) != 0)
            //        {
            //            newFileEntry.LanguageIds.Add((byte)(j + 1));
            //        }
            //    }
            //    fileEntries.Add(newFileEntry);
            //}

            //// directories
            //var directoryOffsets = rootObject.GetOffsetVector(3);
            //var directories = new List<Directory>(directoryOffsets.Length);
            //for (int i = 0; i < directoryOffsets.Length; i++)
            //{
            //    var directoryObject = new FlatBufferObject(directoryOffsets[i]);
            //    var newDirectory = new Directory
            //    {
            //        DirectoryId = directoryObject.GetUInt64(0),
            //        ParentId = directoryObject.GetOptionalUInt64(1),
            //        Name = directoryObject.GetString(2)
            //    };
            //    directories.Add(newDirectory);
            //}

            //// merge directories and file_entries together to a list of files
            //manifest.Files = new List<File>(fileEntries.Count);
            //foreach (var fileEntry in fileEntries)
            //{
            //    var newFile = new File
            //    {
            //        FileSize = fileEntry.FileSize,
            //        Link = fileEntry.Link,
            //        Languages = fileEntry.LanguageIds.Select(id => manifest.Languages.First(lang => lang.LanguageId == id)).ToList()
            //    };

            //    ulong directoryId = fileEntry.DirectoryId;
            //    string tempName = fileEntry.Name;
            //    while (directoryId != 0)
            //    {
            //        var directory = directories.First(d => d.DirectoryId == directoryId);
            //        tempName = $"{directory.Name}/{tempName}";
            //        directoryId = directory.ParentId;
            //    }
            //    newFile.Name = tempName;

            //    newFile.Chunks = new List<Chunk>(fileEntry.ChunkIds.Count);
            //    uint fileOffset = 0;
            //    foreach (var chunkId in fileEntry.ChunkIds)
            //    {
            //        var chunk = manifest.Chunks.First(c => c.ChunkId == chunkId);
            //        chunk.FileOffset = fileOffset;
            //        newFile.Chunks.Add(chunk);
            //        fileOffset += chunk.UncompressedSize;
            //    }

            //    manifest.Files.Add(newFile);
            //}

            //Console.WriteLine($"amount of chunks in this manifest: {totalChunks}");

            return 0;
        }


    }
}
