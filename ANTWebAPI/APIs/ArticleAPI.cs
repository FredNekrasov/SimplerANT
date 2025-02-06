using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using ANTWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.APIs;

public static class ArticleAPI
{
    /// <summary>
    /// Configures the API endpoints for managing articles.
    /// </summary>
    /// <param name="articleApi">The RouteGroupBuilder to which the endpoints are mapped.</param>
    /// <returns>The RouteGroupBuilder with the mapped endpoints.</returns>
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

    /// <summary>
    /// Retrieves a list of all articles.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>200 (OK) if the articles are successfully retrieved.</para>
    /// </returns>
    /// <remarks> This method uses AsNoTracking for improved performance. </remarks>
    private static async Task<IResult> GetAllArticles(ANTDbContext db)
    {
        var articles = await db.Articles.AsNoTracking().Include(e => e.Catalog).ToListAsync();
        return TypedResults.Ok(articles.Select(e => e.ToDto()));
    }

    /// <summary>
    /// Retrieves an article by the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the article to retrieve.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>200 (OK) if the article is successfully retrieved.</para>
    /// 404 (Not Found) if the article is not found.
    /// </returns>
    /// <remarks> This method uses AsNoTracking for improved performance. </remarks>
    private static async Task<IResult> GetArticleById(ANTDbContext db, long id)
    {
        var article = await db.Articles.AsNoTracking().Include(e => e.Catalog).FirstOrDefaultAsync(e => e.Id == id);
        return article == null ? TypedResults.NotFound() : TypedResults.Ok(article.ToDto());
    }

    /// <summary>
    /// Creates a new article with the specified <paramref name="articleDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="articleDto">The data transfer object containing the article data.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>201 (Created) if the article is successfully created.</para>
    /// 400 (Bad Request) if the <paramref name="articleDto"/> is invalid or null.
    /// </returns>
    private static async Task<IResult> CreateArticle(ANTDbContext db, [FromBody] ArticleDTO? articleDto)
    {
        if (articleDto == null || !articleDto.IsDataValid()) return TypedResults.BadRequest();
        var article = articleDto.ToModel();
        await db.Articles.AddAsync(article);
        await db.SaveChangesAsync();
        return TypedResults.Created();
    }

    /// <summary>
    /// Updates an article with the specified <paramref name="id"/> using the provided <paramref name="articleDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the article to update.</param>
    /// <param name="articleDto">The data transfer object containing the updated article data.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>204 (No Content) if the article is successfully updated.</para>
    /// 400 (Bad Request) if the <paramref name="articleDto"/> is invalid or the IDs do not match.
    /// <para>404 (Not Found) if the article with the specified <paramref name="id"/> is not found.</para>
    /// 500 (Internal Server) Error if a concurrency exception occurs.
    /// </returns>
    private static async Task<IResult> UpdateArticle(ANTDbContext db, long id, [FromBody] ArticleDTO? articleDto)
    {
        if (articleDto == null || id < 0 || id != articleDto.Id || !articleDto.IsDataValid()) return TypedResults.BadRequest();
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

    /// <summary>
    /// Deletes an article with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the article to delete.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>204 (No Content) if the article is successfully deleted.</para>
    /// 400 (Bad Request) if the article has dependencies or id is invalid.
    /// <para>404 (Not Found) if the article is not found.</para>
    /// 401 (Unauthorized) if the user is not authenticated.
    /// <para>429 (Too Many Requests) if the rate limit is exceeded.</para>
    /// </returns>
    private static async Task<IResult> DeleteArticle(ANTDbContext db, long id)
    {
        if (id <= 0) return TypedResults.BadRequest();
        var hasContents = await db.Contents.AsNoTracking().AnyAsync(e => e.ArticleId == id);
        if (hasContents) return TypedResults.BadRequest("This record is used as a foreign key in other entity(ies).");
        var article = await db.Articles.FirstOrDefaultAsync(e => e.Id == id);
        if (article == null) return TypedResults.NotFound();
        db.Articles.Remove(article);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}