// See https://aka.ms/new-console-template for more information
using System.Text.Json;

using HttpClient client = new HttpClient();

string sefariaUrl = "https://www.sefaria.org/api";

try
{
    HttpResponseMessage response = await client.GetAsync($"{sefariaUrl}/calendars");
    response.EnsureSuccessStatusCode(); // Throws if not success

    string responseBody = await response.Content.ReadAsStringAsync();

    SefariaCalendarResponse? sefariaCalendarResponse = JsonSerializer.Deserialize<SefariaCalendarResponse>(responseBody) ?? throw new Exception("Failed to deserialize SefariaCalendarResponse.");

    var dailyTanakh = sefariaCalendarResponse.CalendarItems.FirstOrDefault(ci => ci.Title.En == "929");

    if (dailyTanakh == null)
    {
        throw new Exception("Error getting daily learning");
    }

    var titleLength = dailyTanakh.DisplayValue.En.Length + 2;

    Console.WriteLine("┌" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┐");
    Console.WriteLine("│ \u001b[1m" + dailyTanakh.DisplayValue.En + "\u001b[0m │");
    Console.WriteLine("└" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┘");
}
catch (Exception e)
{
    Console.WriteLine("Chai");
}