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

    /// <summary>
    /// Gets all catalogs.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>200 (OK) if the catalogs are successfully retrieved.</para>
    /// </returns>
    /// <remarks> This method uses AsNoTracking to improve performance. </remarks>
    private static async Task<IResult> GetAllCatalogs(ANTDbContext db)
    {
        var catalogs = await db.Catalogs.AsNoTracking().ToListAsync();
        return TypedResults.Ok(catalogs.Select(e => e.ToDto()));
    }
    
    /// <summary>
    /// Gets a catalog by ID.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the catalog to get.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>200 (OK) if the catalog with the specified <paramref name="id"/>  is successfully retrieved.</para>
    /// 404 (Not Found) if the catalog with the specified <paramref name="id"/> is not found.
    /// </returns>
    /// <remarks> This method uses AsNoTracking to improve performance. </remarks>
    private static async Task<IResult> GetCatalogById(ANTDbContext db, long id)
    {
        var catalog = await db.Catalogs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return catalog == null ? TypedResults.NotFound() : TypedResults.Ok(catalog.ToDto());
    }

    /// <summary>
    /// Creates a new catalog with the specified <paramref name="catalogDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="catalogDto">The data transfer object containing the catalog data.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>201 (Created) if the catalog is successfully created.</para>
    /// 400 (Bad Request) if the <paramref name="catalogDto"/> is invalid or null, or the name is already taken.
    /// </returns>
    private static async Task<IResult> CreateCatalog(ANTDbContext db, [FromBody] CatalogDTO? catalogDto)
    {
        if (catalogDto == null || !catalogDto.IsDataValid()) return TypedResults.BadRequest();
        if (await db.Catalogs.AnyAsync(e => e.Name == catalogDto.Name)) return TypedResults.BadRequest();
        var catalog = catalogDto.ToModel();
        await db.Catalogs.AddAsync(catalog);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/catalogs/{catalog.Id}", catalog.ToDto());
    }

    /// <summary>
    /// Updates the catalog with the specified <paramref name="id"/> using the provided <paramref name="catalogDto"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the catalog to update.</param>
    /// <param name="catalogDto">The data transfer object containing the updated catalog data.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>204 (No Content) if the catalog is successfully updated.</para>
    /// 400 (Bad Request) if the <paramref name="catalogDto"/> is invalid or the IDs do not match.
    /// <para>404 (Not Found) if the catalog is not found.</para>
    /// 500 (Internal Server) Error if a concurrency exception occurs.
    /// </returns>
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

    /// <summary>
    /// Deletes the catalog with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="id">The ID of the catalog to delete.</param>
    /// <returns>
    /// A result object that indicates the outcome of the operation.
    /// <para>204 (No Content) if the catalog is successfully deleted.</para>
    /// 400 (Bad Request) if the <paramref name="id"/> is invalid or the catalog has dependencies.
    /// <para>404 (Not Found) if the catalog is not found.</para>
    /// 401 (Unauthorized) if the user is not authenticated.
    /// <para>429 (Too Many Requests) if the rate limit is exceeded.</para>
    /// </returns>
    private static async Task<IResult> DeleteCatalog(ANTDbContext db, long id)
    {
        if (id <= 0) return TypedResults.BadRequest();
        var catalog = await db.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
        if (catalog == null) return TypedResults.NotFound();
        var hasArticles = await db.Articles.AsNoTracking().AnyAsync(e => e.CatalogId == id);
        if (hasArticles) return TypedResults.BadRequest("This record is used as a foreign key in other entity(ies).");
        db.Catalogs.Remove(catalog);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}