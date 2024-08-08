namespace RiotPrefill
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata, WriteIndented = true)]
    [JsonSerializable(typeof(ReleaseInfo))]
    internal sealed partial class SerializationContext : JsonSerializerContext
    {
    }
}