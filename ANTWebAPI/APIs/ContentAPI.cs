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
    
    private static async Task<IResult> GetAllContents(ANTDbContext db)
    {
        var contents = await db.Contents.AsNoTracking().ToListAsync();
        return TypedResults.Ok(contents.Select(e => e.ToDto()));
    }
    
    private static async Task<IResult> GetContentById(ANTDbContext db, long id)
    {
        var content = await db.Contents.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return content == null ? TypedResults.NotFound() : TypedResults.Ok(content.ToDto());
    }

    private static async Task<IResult> CreateContent(ANTDbContext db, [FromBody] ContentDTO? contentDto)
    {
        if (contentDto == null || !contentDto.IsDataValid()) return TypedResults.BadRequest();
        var content = contentDto.ToModel();
        await db.Contents.AddAsync(content);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/content/{contentDto.Id}", contentDto);
    }

    private static async Task<IResult> UpdateContent(ANTDbContext db, long id, [FromBody] ContentDTO? contentDto)
    {
        if (contentDto == null || id != contentDto.Id || !contentDto.IsDataValid()) return TypedResults.BadRequest();
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