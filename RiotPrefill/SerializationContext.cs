namespace RiotPrefill
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata, WriteIndented = true)]
    [JsonSerializable(typeof(ReleaseApiResponse))]
    internal sealed partial class SerializationContext : JsonSerializerContext
    {
    }
}