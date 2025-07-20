﻿// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Text.Json;
using Microsoft.Data.Sqlite;

using HttpClient client = new HttpClient();

bool updateDatabase = false;
bool fetchFromApi = false;

foreach (var arg in args)
{
    switch (arg)
    {
        case "update":
            updateDatabase = true;
            break;

        case "fetch":
            fetchFromApi = true;
            updateDatabase = true;
            break;
    }
}

if (updateDatabase)
{
    try
    {
        var fs = new FetchSummaries();
        await fs.GetChapterSummaries(fetchFromApi);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    
    return 0;
}

string sefariaUrl = "https://www.sefaria.org/api";

try
{
    // Getting Chapter title
    HttpResponseMessage response = await client.GetAsync($"{sefariaUrl}/calendars");
    response.EnsureSuccessStatusCode(); // Throws if not success

    string responseBody = await response.Content.ReadAsStringAsync();

    SefariaCalendarResponse? sefariaCalendarResponse = JsonSerializer.Deserialize<SefariaCalendarResponse>(responseBody) ?? throw new Exception("Failed to deserialize SefariaCalendarResponse.");

    var dailyTanakh = sefariaCalendarResponse.CalendarItems.FirstOrDefault(ci => ci.Title.En == "929");

    if (dailyTanakh == null)
    {
        throw new Exception("Error getting daily learning");
    }

    // Getting chapter summary
    var databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.sqlite");
    var connectionString = $"Data Source={databasePath}";

    var connection = new SqliteConnection(connectionString);
    connection.Open();
    var readCommand = connection.CreateCommand();
    readCommand.CommandText = "SELECT summary FROM summaries WHERE chapter = $chapter LIMIT 1";
    readCommand.Parameters.AddWithValue("$chapter", dailyTanakh.Ref);

    string summaryOneLine = "";
    char[] summary = [];

    using var reader = readCommand.ExecuteReader();
    if (reader.Read())
    {
        summaryOneLine = reader.GetString(0);

        // All the code below is to break the summary at the last space in every 80 characters
        summary = summaryOneLine.ToCharArray();

        var lineSize = 80;
        var lines = summaryOneLine.Length / lineSize;
        var margin = 0;

        for (var line = 0; line < lines; line++)
        {
            char[] reversedLine = summaryOneLine
                .Substring(line * lineSize - margin, lineSize)
                .ToCharArray()
                .Reverse()
                .ToArray();

            var spaceIndex = 0;
            for (int i = 0; i < reversedLine.Length; i++)
            {
                if (reversedLine[i] == ' ')
                {
                    spaceIndex = i;
                    break;
                }
            }

            summary[(line + 1) * lineSize - spaceIndex - 1] = '\n';
            margin = spaceIndex;
        }
    }

    // Displaying all the data
    var titleLength = dailyTanakh.DisplayValue.En.Length + 2;

    Console.WriteLine("┌" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┐");
    Console.WriteLine("│ \u001b[1m" + dailyTanakh.DisplayValue.En + "\u001b[0m │");
    Console.WriteLine("└" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┘");
    Console.WriteLine(new string(summary).Replace("Lord", "Lᴏʀᴅ"));
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine("Chai");
}

return 0;