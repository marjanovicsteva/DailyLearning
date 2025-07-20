using System.Text.Json.Serialization;

class VerseData
{
    [JsonPropertyName("start")]
    public required int Start { get; set; }

    [JsonPropertyName("end")]
    public required int End { get; set; }

    [JsonPropertyName("footnote")]
    public string? footnote { get; set; }
}