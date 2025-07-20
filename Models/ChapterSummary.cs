using System.Text.Json.Serialization;

class ChapterSummary
{
    [JsonPropertyName("chapter")]
    public required string Chapter { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
}