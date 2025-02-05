using System.Text.Json;

namespace ANTConsole;

public static class JsonParser<T>
{
    public static List<T>? GetParsedListOrNull(string filePath, JsonSerializerOptions jsonSerializerOptions)
    {
        try
        {
            var file = File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
            var catalogList = JsonSerializer.Deserialize<List<T>>(file, jsonSerializerOptions);
            return catalogList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
}