class OpenScripture
{
    private readonly HttpClient client = new HttpClient();
    private readonly string openScriptureUrl = "https://openscriptureapi.org/api/scriptures/v1/lds/en";

    public async Task<string> Call(string endpoint)
    {
        var response = await client.GetAsync($"{openScriptureUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}