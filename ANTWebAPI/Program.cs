using System.Text.Json;
using System.Text.Json.Serialization;
using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using ANTWebAPI.Repository;
using ANTWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddDbContext<ANTDbContext>();
builder.Services.AddScoped<ChapterRepository>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    // options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    // options.SerializerOptions.PropertyNameCaseInsensitive = true;
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

// Configure the Catalog API endpoints
var catalogApi = app.MapGroup("/catalogs");

catalogApi.MapGet("/catalogs", async (ANTDbContext db) =>
{
    var catalogs = await db.Catalogs.AsNoTracking().ToListAsync();
    return Results.Ok(catalogs.Select(e => e.ToDto()));
});

catalogApi.MapGet("/{id:long}", async (ANTDbContext db, long id) =>
{
    var catalog = await db.Catalogs.AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);
    return catalog == null ? Results.NotFound() : Results.Ok(catalog.ToDto());
});

catalogApi.MapPost("/", async (ANTDbContext db, CatalogDTO? catalogDto) =>
{
    if (catalogDto == null || !catalogDto.IsDataValid()) return Results.BadRequest();
    if (await db.Catalogs.AnyAsync(e => e.Name == catalogDto.Name)) return Results.BadRequest();
    var catalog = catalogDto.ToModel();
    await db.Catalogs.AddAsync(catalog);
    await db.SaveChangesAsync();
    return Results.Created($"/catalog/{catalog.Id}", catalog.ToDto());
});

catalogApi.MapPut("/{id:long}", async (ANTDbContext db, long id, CatalogDTO? catalogDto) =>
{
    if (catalogDto == null || id != catalogDto.Id || !catalogDto.IsDataValid()) return Results.BadRequest();
    var catalog = await db.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
    if (catalog == null) return Results.NotFound();
    catalog.Name = catalogDto.Name;
    try
    {
        await db.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return Results.BadRequest();
    }
    return Results.Ok(catalog.ToDto());
});

catalogApi.MapDelete("/{id:long}", async (ANTDbContext db, long id) =>
{
    if (id <= 0) return Results.BadRequest();
    var catalog = await db.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
    if (catalog == null) return Results.NotFound();
    var hasArticles = await db.Articles.AnyAsync(e => e.CatalogId == id);
    if (hasArticles) return Results.BadRequest("This record is used as a foreign key in other entity(ies).");
    db.Catalogs.Remove(catalog);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Configure the Article API endpoints
var articleApi = app.MapGroup("/articles");

articleApi.MapGet("/", async (ANTDbContext db) =>
{
    var articles = await db.Articles.AsNoTracking().ToListAsync();
    return Results.Ok(articles.Select(e => e.ToDto()));
});

articleApi.MapGet("/{id:long}", async (ANTDbContext db, long id) =>
{
    var article = await db.Articles.AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);
    return article == null ? Results.NotFound() : Results.Ok(article.ToDto());
});

articleApi.MapPost("/", async (ANTDbContext db, ArticleDTO? articleDto) =>
{
    if (articleDto == null || !articleDto.IsDataValid()) return Results.BadRequest();
    var article = articleDto.ToModel();
    await db.Articles.AddAsync(article);
    await db.SaveChangesAsync();
    return Results.Created($"/article/{article.Id}", article.ToDto());
});

articleApi.MapPut("/{id:long}", async (ANTDbContext db, long id, ArticleDTO? articleDto) =>
{
    if (articleDto == null || id != articleDto.Id || !articleDto.IsDataValid()) return Results.BadRequest();
    var article = await db.Articles.FirstOrDefaultAsync(e => e.Id == id);
    if (article == null) return Results.NotFound();
    article.CatalogId = articleDto.Catalog.Id;
    article.Title = articleDto.Title;
    article.Description = articleDto.Description;
    article.DateOrBanner = articleDto.DateOrBanner;
    try
    {
        await db.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return Results.BadRequest();
    }
    return Results.Ok(articleDto);
});

articleApi.MapDelete("/{id:long}", async (ANTDbContext db, long id) =>
{
    if (id <= 0) return Results.BadRequest();
    var hasContents = await db.Contents.AnyAsync(e => e.ArticleId == id);
    if (hasContents) return Results.BadRequest("This record is used as a foreign key in other entity(ies).");
    var article = await db.Articles.FirstOrDefaultAsync(e => e.Id == id);
    if (article == null) return Results.NotFound();
    db.Articles.Remove(article);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Configure the Content API endpoints
var contentApi = app.MapGroup("/contents");

contentApi.MapGet("/", async (ANTDbContext db) =>
{
    var contents = await db.Contents.AsNoTracking().ToListAsync();
    return contents is [] ? Results.NotFound() : Results.Ok(contents.Select(e => e.ToDto()));
});

contentApi.MapGet("/{id:long}", async (ANTDbContext db, long id) =>
{
    var content = await db.Contents.AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);
    return content == null ? Results.NotFound() : Results.Ok(content.ToDto());
});

contentApi.MapPost("/", async (ANTDbContext db, ContentDTO? contentDto) =>
{
    if (contentDto == null || !contentDto.IsDataValid()) return Results.BadRequest();
    var content = contentDto.ToModel();
    await db.Contents.AddAsync(content);
    await db.SaveChangesAsync();
    return Results.Created($"/content/{contentDto.Id}", contentDto);
});

contentApi.MapPut("/{id:long}", async (ANTDbContext db, long id, ContentDTO? contentDto) =>
{
    if (contentDto == null || id != contentDto.Id || !contentDto.IsDataValid()) return Results.BadRequest();
    var content = await db.Contents.FirstOrDefaultAsync(e => e.Id == id);
    if (content == null) return Results.NotFound();
    content.ArticleId = contentDto.ArticleId;
    content.Data = contentDto.Data;
    try
    {
        await db.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return Results.BadRequest();
    }
    return Results.Ok(contentDto);
});

contentApi.MapDelete("/{id:long}", async (ANTDbContext db, long id) =>
{
    if (id <= 0) return Results.BadRequest();
    var content = await db.Contents.FirstOrDefaultAsync(e => e.Id == id);
    if (content == null) return Results.NotFound();
    db.Contents.Remove(content);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Configure the Chapter API endpoints
var chapterApi = app.MapGroup("/chapters");

chapterApi.MapGet("/", async (ChapterRepository chapterRepository) =>
{
    var chapterList = await chapterRepository.GetListAsync();
    return chapterList is [] ? Results.NotFound() : Results.Ok(chapterList);
});

chapterApi.MapGet("/{catalogId:long}", async (ChapterRepository chapterRepository, long catalogId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50) =>
{
    var chapterList = await chapterRepository.GetPagedListByCatalogAsync(catalogId, pageNumber, pageSize);
    if (chapterList is []) return Results.NotFound();
    var totalRecords = await chapterRepository.GetTotalCountAsync();
    var response = new PagedResponse<ChapterDTO>(pageNumber, chapterList.Count, totalRecords, chapterList);
    return Results.Ok(response);
});

app.Run();

internal record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}