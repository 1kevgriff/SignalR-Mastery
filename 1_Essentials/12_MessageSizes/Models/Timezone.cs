// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public class Timezone
{
    [JsonPropertyName("offset")]
    public string Offset { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

