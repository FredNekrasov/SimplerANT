using ANTWebAPI.Database;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using ANTWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.APIs;

public static class CatalogAPI
{
    /// <summary>
    /// Maps the catalog-related API endpoints to the provided RouteGroupBuilder.
    /// Defines endpoints for CRUD operations on catalogs.
    /// </summary>
    /// <param name="catalogApi">The RouteGroupBuilder to which the endpoints are mapped.</param>
    /// <returns>The RouteGroupBuilder with the mapped endpoints.</returns>
    public static RouteGroupBuilder MapCatalogAPIEndpoints(this RouteGroupBuilder catalogApi)
    {
        catalogApi.MapGet("/", GetAllCatalogs).Produces<List<CatalogDTO>>()
            .ProducesProblem(401)
            .Produces(429);
        catalogApi.MapGet("/{id:long}", GetCatalogById).Produces<CatalogDTO>()
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        catalogApi.MapPost("/", CreateCatalog).Accepts<CatalogDTO>("application/json")
            .Produces(201)
            .ProducesProblem(400)
            .ProducesProblem(401)
            .Produces(429);
        catalogApi.MapPut("/{id:long}", UpdateCatalog).Accepts<CatalogDTO>("application/json")
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(500)
            .ProducesProblem(401)
            .Produces(429);
        catalogApi.MapDelete("/{id:long}", DeleteCatalog)
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        return catalogApi;
    }

    private static async Task<IResult> GetAllCatalogs(ANTDbContext db)
    {
        var catalogs = await db.Catalogs.AsNoTracking().ToListAsync();
        return TypedResults.Ok(catalogs.Select(e => e.ToDto()));
    }
    
    private static async Task<IResult> GetCatalogById(ANTDbContext db, long id)
    {
        var catalog = await db.Catalogs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return catalog == null ? TypedResults.NotFound() : TypedResults.Ok(catalog.ToDto());
    }

    private static async Task<IResult> CreateCatalog(ANTDbContext db, [FromBody] CatalogDTO? catalogDto)
    {
        if (catalogDto == null || !catalogDto.IsDataValid()) return TypedResults.BadRequest();
        if (await db.Catalogs.AnyAsync(e => e.Name == catalogDto.Name)) return TypedResults.BadRequest();
        var catalog = catalogDto.ToModel();
        await db.Catalogs.AddAsync(catalog);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/catalogs/{catalog.Id}", catalog.ToDto());
    }

    private static async Task<IResult> UpdateCatalog(ANTDbContext db, long id, [FromBody] CatalogDTO? catalogDto)
    {
        if (catalogDto == null || id < 0 || id != catalogDto.Id || !catalogDto.IsDataValid()) return TypedResults.BadRequest();
        var catalog = await db.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
        if (catalog == null) return TypedResults.NotFound();
        catalog.Name = catalogDto.Name;
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

    private static async Task<IResult> DeleteCatalog(ANTDbContext db, long id)
    {
        if (id <= 0) return TypedResults.BadRequest();
        var catalog = await db.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
        if (catalog == null) return TypedResults.NotFound();
        var hasArticles = await db.Articles.AnyAsync(e => e.CatalogId == id);
        if (hasArticles) return TypedResults.BadRequest("This record is used as a foreign key in other entity(ies).");
        db.Catalogs.Remove(catalog);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}