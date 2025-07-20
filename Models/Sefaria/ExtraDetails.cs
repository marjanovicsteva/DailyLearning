using System.Text.Json.Serialization;

public class ExtraDetails
{
    [JsonPropertyName("aliyot")]
    public List<string>? Aliyot { get; set; }
}