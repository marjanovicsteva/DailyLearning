using System.Text.Json.Serialization;

class Verse
{
    [JsonPropertyName("bookTitle")]
    public required string Text { get; set; }

    [JsonPropertyName("footnotes")]
    public required List<VerseData> Footnotes { get; set; }

    [JsonPropertyName("associatedContent")]
    public required List<string> AssociatedContent { get; set; }
    
    [JsonPropertyName("italics")]
    public required List<VerseData> Italics { get; set; }
}