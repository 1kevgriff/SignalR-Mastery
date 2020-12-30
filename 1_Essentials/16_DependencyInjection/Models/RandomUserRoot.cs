// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class RandomUserRoot
{
    [JsonPropertyName("results")]
    public List<RandomUser> Results { get; set; }

    [JsonPropertyName("info")]
    public Info Info { get; set; }
}

