using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using ANTWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.APIs;

public static class ArticleAPI
{
    public static RouteGroupBuilder MapArticleAPIEndpoints(this RouteGroupBuilder articleApi)
    {
        articleApi.MapGet("/", GetAllArticles).Produces<List<ArticleDTO>>()
            .ProducesProblem(401)
            .Produces(429);
        articleApi.MapGet("/{id:long}", GetArticleById).Produces<ArticleDTO>()
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        articleApi.MapPost("/", CreateArticle).Accepts<ArticleDTO>("application/json")
            .Produces(201)
            .ProducesProblem(400)
            .ProducesProblem(401)
            .Produces(429);
        articleApi.MapPut("/{id:long}", UpdateArticle).Accepts<ArticleDTO>("application/json")
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(500)
            .ProducesProblem(401)
            .Produces(429);
        articleApi.MapDelete("/{id:long}", DeleteArticle)
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        return articleApi;
    }

    private static async Task<IResult> GetAllArticles(ANTDbContext db)
    {
        var articles = await db.Articles.AsNoTracking().ToListAsync();
        return TypedResults.Ok(articles.Select(e => e.ToDto()));
    }

    private static async Task<IResult> GetArticleById(ANTDbContext db, long id)
    {
        var article = await db.Articles.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return article == null ? TypedResults.NotFound() : TypedResults.Ok(article.ToDto());
    }

    private static async Task<IResult> CreateArticle(ANTDbContext db, [FromBody] ArticleDTO? articleDto)
    {
        if (articleDto == null || !articleDto.IsDataValid()) return TypedResults.BadRequest();
        var article = articleDto.ToModel();
        await db.Articles.AddAsync(article);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/article/{article.Id}", article.ToDto());
    }

    private static async Task<IResult> UpdateArticle(ANTDbContext db, long id, [FromBody] ArticleDTO? articleDto)
    {
        if (articleDto == null || id != articleDto.Id || !articleDto.IsDataValid()) return TypedResults.BadRequest();
        var article = await db.Articles.FirstOrDefaultAsync(e => e.Id == id);
        if (article == null) return TypedResults.NotFound();
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
            return TypedResults.InternalServerError();
        }
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteArticle(ANTDbContext db, long id)
    {
        if (id <= 0) return TypedResults.BadRequest();
        var hasContents = await db.Contents.AnyAsync(e => e.ArticleId == id);
        if (hasContents) return TypedResults.BadRequest("This record is used as a foreign key in other entity(ies).");
        var article = await db.Articles.FirstOrDefaultAsync(e => e.Id == id);
        if (article == null) return TypedResults.NotFound();
        db.Articles.Remove(article);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}