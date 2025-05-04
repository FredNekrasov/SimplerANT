using System.Text.Json;

namespace ANTConsole;

public static class JsonParser
{
    /// <summary>
    /// Parses JSON data from a specified file and returns a list of objects of type T.
    /// If the file does not exist or an error occurs during parsing, it returns null.
    /// </summary>
    /// <param name="filePath">The path to the JSON file to be parsed.</param>
    /// <param name="jsonSerializerOptions">Options to customize the JSON deserialization process.</param>
    /// <returns>A list of objects of type T, or null if parsing fails.</returns>
    public static List<T>? GetParsedListOrNull<T>(string filePath, JsonSerializerOptions jsonSerializerOptions)
    {
        try
        {
            var fileContent = File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
            var parsedList = JsonSerializer.Deserialize<List<T>>(fileContent, jsonSerializerOptions);
            return parsedList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
}
