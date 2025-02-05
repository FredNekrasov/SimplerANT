using System.Text.Json;

namespace ANTConsole;

public static class JsonParser<T>
{
    /// <summary>
    /// Parses JSON data from a specified file and returns a list of objects of type T.
    /// If the file does not exist or an error occurs during parsing, it returns null.
    /// </summary>
    /// <param name="filePath">The path to the JSON file to be parsed.</param>
    /// <param name="jsonSerializerOptions">Options to customize the JSON deserialization process.</param>
    /// <returns>A list of objects of type T, or null if parsing fails.</returns>
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