using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using ANTWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.APIs;

public static class ContentAPI
{
    /// <summary>
    /// Maps the content-related API endpoints to the provided RouteGroupBuilder.
    /// Defines endpoints for CRUD operations on contents.
    /// </summary>
    /// <param name="contentApi">The RouteGroupBuilder to which the endpoints are mapped.</param>
    /// <returns>The RouteGroupBuilder with the mapped endpoints.</returns>
    public static RouteGroupBuilder MapContentAPIEndpoints(this RouteGroupBuilder contentApi)
    {
        contentApi.MapGet("/", GetAllContents).Produces<List<ContentDTO>>()
            .ProducesProblem(401)
            .Produces(429);
        contentApi.MapGet("/{id:long}", GetContentById).Produces<ContentDTO>()
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        contentApi.MapPost("/", CreateContent).Accepts<ContentDTO>("application/json")
            .Produces(201)
            .ProducesProblem(400)
            .ProducesProblem(401)
            .Produces(429);
        contentApi.MapPut("/{id:long}", UpdateContent).Accepts<ContentDTO>("application/json")
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(500)
            .ProducesProblem(401)
            .Produces(429);
        contentApi.MapDelete("/{id:long}", DeleteContent)
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        return contentApi;
    }
    
    /// <summary>
    /// Gets a list of all contents.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>200 (OK) if the contents are successfully retrieved.</para>
    /// </returns>
    /// <remarks> This method uses AsNoTracking to improve performance. </remarks>
    private static async Task<IResult> GetAllContents(ANTDbContext db)
    {
        var contents = await db.Contents.AsNoTracking().ToListAsync();
        return TypedResults.Ok(contents.Select(e => e.ToDto()));
    }
    
    /// <summary>
    /// Gets a content by ID.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the content to get.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>200 (OK) if the content is successfully retrieved.</para>
    /// 404 (Not Found) if the content is not found.
    /// </returns>
    /// <remarks> This method uses AsNoTracking to improve performance. </remarks>
    private static async Task<IResult> GetContentById(ANTDbContext db, long id)
    {
        var content = await db.Contents.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return content == null ? TypedResults.NotFound() : TypedResults.Ok(content.ToDto());
    }

    /// <summary>
    /// Creates a new content with the specified <paramref name="contentDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="contentDto">The data transfer object containing the content data.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>201 (Created) if the content is successfully created.</para>
    /// 400 (Bad Request) if the <paramref name="contentDto"/> is invalid or null.
    /// </returns>
    private static async Task<IResult> CreateContent(ANTDbContext db, [FromBody] ContentDTO? contentDto)
    {
        if (contentDto == null || !contentDto.IsDataValid()) return TypedResults.BadRequest();
        var content = contentDto.ToModel();
        await db.Contents.AddAsync(content);
        await db.SaveChangesAsync();
        return TypedResults.Created();
    }

    /// <summary>
    /// Updates the content with the specified <paramref name="id"/> using the provided <paramref name="contentDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the content to update.</param>
    /// <param name="contentDto">The data transfer object containing the updated content data.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>204 (No Content) if the content is successfully updated.</para>
    /// 400 (Bad Request) if the <paramref name="contentDto"/> is invalid or the IDs do not match.
    /// 404 (Not Found) if the content is not found.
    /// <para>500 (Internal Server) Error if a concurrency exception occurs.</para>
    /// </returns>
    private static async Task<IResult> UpdateContent(ANTDbContext db, long id, [FromBody] ContentDTO? contentDto)
    {
        if (contentDto == null || id < 0 || id != contentDto.Id || !contentDto.IsDataValid()) return TypedResults.BadRequest();
        var content = await db.Contents.FirstOrDefaultAsync(e => e.Id == id);
        if (content == null) return TypedResults.NotFound();
        content.ArticleId = contentDto.ArticleId;
        content.Data = contentDto.Data;
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
    /// Deletes the content with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the content to delete.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>204 (No Content) if the content is successfully deleted.</para>
    /// 400 (Bad Request) if the <paramref name="id"/> is invalid.
    /// <para>404 (Not Found) if the content is not found.</para>
    /// 401 (Unauthorized) if the user is not authenticated.
    /// <para>429 (Too Many Requests) if the rate limit is exceeded.</para>
    /// </returns>
    private static async Task<IResult> DeleteContent(ANTDbContext db, long id)
    {
        if (id <= 0) return TypedResults.BadRequest();
        var content = await db.Contents.FirstOrDefaultAsync(e => e.Id == id);
        if (content == null) return TypedResults.NotFound();
        db.Contents.Remove(content);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}