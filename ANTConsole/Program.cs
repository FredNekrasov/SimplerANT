// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ANTConsole;
using ANTConsole.DTOs;

Console.WriteLine("Hello, World!");

// file names 
const string CATALOGS = "PreviouslyParsedCatalogs.json";
const string ARTICLES = "PreviouslyParsedArticles.json";
const string CONTENTS = "PreviouslyParsedContents.json";

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var directory = Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net9.0", "") + @"\ParsedData\";
var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5066/")
};

await DataSender.SendAsync<CatalogDTO>(client, directory + CATALOGS, jsonOptions, "api/catalogs/");

await DataSender.SendAsync<ArticleDTO>(client, directory + ARTICLES, jsonOptions, "api/articles/");

await DataSender.SendAsync<CatalogDTO>(client, directory + CONTENTS, jsonOptions, "api/contents/");