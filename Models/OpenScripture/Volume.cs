using System.Text.Json.Serialization;

public class Volume
{
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("titleShort")]
    public required string TitleShort { get; set; }

    [JsonPropertyName("titleOfficial")]
    public required string TitleOfficial { get; set; }

    [JsonPropertyName("books")]
    public List<Book>? Books { get; set; }
}