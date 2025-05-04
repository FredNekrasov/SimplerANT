// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ANTConsole;
using ANTConsole.DTOs;

Console.WriteLine("Hello, World!");

const string CATALOGS = "PreviouslyParsedCatalogs.json"; // Path to catalogs JSON file
const string ARTICLES = "PreviouslyParsedArticles.json"; // Path to articles JSON file
const string CONTENTS = "PreviouslyParsedContents.json"; // Path to contents JSON file

// Configuration for JSON deserialization to ignore case sensitivity in property names
var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

// Determine the base directory for JSON files, adjusting for current directory structure
var directory = Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net9.0", "") + @"\ParsedData\";

// Initialize HttpClient with the base address of the server
var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5066/")
};

// Send the catalogs data to the specified API endpoint
await DataSender.SendAsync<CatalogDTO>(client, directory + CATALOGS, jsonOptions, "api/catalogs/");

// Send the articles data to the specified API endpoint
await DataSender.SendAsync<ArticleDTO>(client, directory + ARTICLES, jsonOptions, "api/articles/");

// Send the content data to the specified API endpoint
await DataSender.SendAsync<ContentDTO>(client, directory + CONTENTS, jsonOptions, "api/contents/");