using System.Text.Json;
using System.Text.Json.Serialization;

public class Chapter
{
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("nextChapterId")]
    public string? NextChapterId { get; set; }

    [JsonPropertyName("prevChapterId")]
    public string? PreviousChapterId { get; set; }

    [JsonPropertyName("volume")]
    public Volume? Volume { get; set; }

    [JsonPropertyName("book")]
    public Book? Book { get; set; }

    [JsonPropertyName("chapter")]
    public ChapterData? Data { get; set; }

    public override string ToString()
    {
        return Id;
    }

    public async Task<bool> Load()
    {
        try
        {
            OpenScripture os = new OpenScripture();
            string chapterData = await os.Call($"chapter/{Id}");

            Chapter chapter = JsonSerializer.Deserialize<Chapter>(chapterData) ?? throw new Exception("Failed to deserialize Chapter");

            NextChapterId = chapter.NextChapterId;
            PreviousChapterId = chapter.PreviousChapterId;
            Summary = chapter.Summary;
            // Volume = new Volume(chapter.Volume);
            // Book = chapter.Book;

            if (chapter.Data == null) throw new Exception("No chapter Data loaded");
            Data = chapter.Data;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}