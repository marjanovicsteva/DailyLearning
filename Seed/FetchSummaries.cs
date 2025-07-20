using Microsoft.Data.Sqlite;
using System.Text.Json;

public class FetchSummaries
{
    private readonly string volumeId = "oldtestament";

    private async Task<bool> FetchChapterSummaries()
    {
        // Create Data forlder if it doesnt exits
        var buildPath = AppContext.BaseDirectory;
        Directory.CreateDirectory(Path.Combine(buildPath, "Data"));

        // Connect to local database
        var databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.sqlite");
        var connectionString = $"Data Source={databasePath}";

        // Load all books from volume
        OpenScripture os = new OpenScripture();
        string responseBody = await os.Call($"volume/{volumeId}");
        Volume volume = JsonSerializer.Deserialize<Volume>(responseBody) ?? throw new Exception("Failed to deserialize Volume");
        var books = volume.Books;

        if (books == null) throw new Exception($"No books in Volume {volume.Title}");

        Console.WriteLine("Books acquired. Fetch chapters.");
        foreach (Book book in books)
        {
            if (book.Chapters == null)
            {
                Console.WriteLine($"Loading book {book.Id}");
                await book.Load();
                Console.WriteLine($"Book {book.Title} ({book.Id}) is successfully loaded.");

                if (book.Chapters == null) throw new Exception("No Chapters");

                foreach (Chapter chapter in book.Chapters)
                {
                    Console.WriteLine($"Loading chapter {chapter.Id}");
                    await chapter.Load();

                    if (chapter.Data == null) throw new Exception("No chapter Data loaded");
                    if (chapter.Data.Summary == null) throw new Exception("No chapter Data Summary loaded");

                    string chapterTitle = $"{book.FormatTitle()} {chapter.Data.Number}";
                    string chapterSummary = chapter.Data.Summary;

                    Console.WriteLine($"Chapter {chapterTitle} ({chapter.Id}) is successfully loaded.");

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        var createTableCmd = connection.CreateCommand();
                        createTableCmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS summaries (
                                chapter TEXT NOT NULL,
                                summary TEXT
                            );";
                        createTableCmd.ExecuteNonQuery();

                        var command = connection.CreateCommand();
                        command.CommandText =
                        @"
                            INSERT INTO summaries (chapter, summary)
                            VALUES ($chapter, $summary);
                        ";
                        command.Parameters.AddWithValue("$chapter", chapterTitle);
                        command.Parameters.AddWithValue("$summary", chapterSummary);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }

            Console.WriteLine("-------------------------------");
            Console.WriteLine("");
        }

        return false;
    }

    public async Task<bool> GetChapterSummaries(bool fetchFromApi = false)
    {
        if (fetchFromApi)
        {
            return await FetchChapterSummaries();
        }
        else
        {
            // Create Data forlder if it doesnt exits
            var buildPath = AppContext.BaseDirectory;
            Directory.CreateDirectory(Path.Combine(buildPath, "Data"));

            // Connect to local database
            var databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.sqlite");
            var connectionString = $"Data Source={databasePath}";

            var jsonDataPath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.json");

            string jsonData = File.ReadAllText(jsonDataPath);
            var summaries = JsonSerializer.Deserialize<List<ChapterSummary>>(jsonData) ?? throw new Exception("Failed to deserialize ChapterSummary");

            var connection = new SqliteConnection(connectionString);
            connection.Open();
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS summaries (
                    chapter TEXT NOT NULL,
                    summary TEXT
                );";
            createTableCmd.ExecuteNonQuery();

            var truncateTableCommand = connection.CreateCommand();
            truncateTableCommand.CommandText = "DELETE FROM summaries; VACUUM;";
            truncateTableCommand.ExecuteNonQuery();
            connection.Close();

            foreach (var summary in summaries)
            {
                connection.Open();
                Console.WriteLine($"Importing {summary.Chapter}...");

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO summaries (chapter, summary)
                    VALUES ($chapter, $summary);
                ";
                command.Parameters.AddWithValue("$chapter", summary.Chapter);
                command.Parameters.AddWithValue("$summary", summary.Summary);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Successfully imported {summary.Chapter}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();
                }
            }

            return false;
        }
    }
}