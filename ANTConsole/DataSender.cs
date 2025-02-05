using System.Text;
using System.Text.Json;

namespace ANTConsole;

public static class DataSender
{
    /// <summary>
    /// Asynchronously sends a list of objects of type T to a specified HTTP endpoint.
    /// Retrieves the list from a JSON file and checks for valid data before sending.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance used to send requests.</param>
    /// <param name="filePath">The path to the JSON file containing the list to send.</param>
    /// <param name="options">Serialization options for parsing the JSON file.</param>
    /// <param name="endpoint">The endpoint URL to which the data will be sent.</param>
    /// <typeparam name="T">The type of objects in the list being sent.</typeparam>
    public static async Task SendAsync<T>(HttpClient httpClient, string filePath, JsonSerializerOptions options, string endpoint)
    {
        var dtoList = JsonParser<T>.GetParsedListOrNull(filePath, options);
        if (dtoList is null or [] || dtoList.Any(dto => dto == null))
        {
            Console.WriteLine($"{typeof(T).Name} list is empty or null.");
            return;
        }

        foreach (var dto in dtoList)
        {
            Console.WriteLine(dto.ToString());
            var response = await httpClient.PostAsync(endpoint, GetStringContent(dto));
            Console.WriteLine(response.StatusCode);
        }
    }
    /// <summary>
    /// Converts an object to a StringContent instance with JSON format.
    /// </summary>
    /// <param name="obj">The object to serialize into JSON.</param>
    /// <returns>A StringContent object containing the serialized JSON.</returns>
    private static StringContent GetStringContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}