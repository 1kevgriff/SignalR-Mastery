// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public class Street    {
        [JsonPropertyName("number")]
        public int Number { get; set; } 

        [JsonPropertyName("name")]
        public string Name { get; set; } 
    }

