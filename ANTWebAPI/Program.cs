using System.Text.Json.Serialization;
using ANTWebAPI.APIs;
using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Repository;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddDbContext<ANTDbContext>();
builder.Services.AddScoped<ChapterRepository>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var sampleTodos = new Todo[]
{
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

// Configure the HTTP request pipeline.
var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id:int}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.MapGroup("/api/catalogs")
    .MapCatalogAPIEndpoints()
    .WithTags("Catalogs");

app.MapGroup("/api/articles")
    .MapArticleAPIEndpoints()
    .WithTags("Articles");

app.MapGroup("/api/contents")
    .MapContentAPIEndpoints()
    .WithTags("Contents");

app.MapGroup("/api/chapters")
    .MapChapterAPIEndpoints()
    .WithTags("Chapters");

app.Run();

internal record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

/// <summary>
/// Custom JSON serializer context to include custom types in the JSON output of the API.
/// </summary>
[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(List<CatalogDTO>))]
[JsonSerializable(typeof(List<ArticleDTO>))]
[JsonSerializable(typeof(List<ContentDTO>))]
[JsonSerializable(typeof(List<ChapterDTO>))]
[JsonSerializable(typeof(PagedResponse<ChapterDTO>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;