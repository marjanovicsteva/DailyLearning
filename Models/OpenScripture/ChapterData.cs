using System.Text.Json.Serialization;

public class ChapterData
{
    [JsonPropertyName("bookTitle")]
    public string? BookTitle { get; set; }

    [JsonPropertyName("delineation")]
    public string? Delineation { get; set; }

    [JsonPropertyName("number")]
    public int? Number { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("chapterAugmentations")]
    public string[]? ChapterAugmentations { get; set; }

    [JsonPropertyName("book")]
    public Book? Book { get; set; }

    public override string ToString()
    {
        return Summary ?? "NO_SUMMARY";
    }
}