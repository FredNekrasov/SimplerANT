using ANTWebAPI.DTOs;
using ANTWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ANTWebAPI.APIs;

public static class ChapterAPI
{
    /// <summary>
    /// Maps the chapter-related API endpoints to the provided RouteGroupBuilder.
    /// Defines endpoints for getting chapters.
    /// </summary>
    /// <param name="chapterApi">The RouteGroupBuilder to which the endpoints are mapped.</param>
    /// <returns>The RouteGroupBuilder with the mapped endpoints.</returns>
    public static RouteGroupBuilder MapChapterAPIEndpoints(this RouteGroupBuilder chapterApi)
    {
        chapterApi.MapGet("/", GetAllChapters).Produces<List<ChapterDTO>>()
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        chapterApi.MapGet("/{catalogId:long}", GetPagedChapters).Produces<PagedResponse<ChapterDTO>>()
            .ProducesProblem(404)
            .ProducesProblem(401)
            .Produces(429);
        return chapterApi;
    }

    /// <summary>
    /// Retrieves a list of all chapters.
    /// </summary>
    /// <param name="chapterRepository">The chapter repository.</param>
    /// <returns>
    /// A result object indicating the outcome of the operation.
    /// <para>200 (OK) if the chapters are successfully retrieved.</para>
    /// 404 (Not Found) if returns an empty list.
    /// </returns>
    private static async Task<IResult> GetAllChapters(ChapterRepository chapterRepository)
    {
        var chapterList = await chapterRepository.GetListAsync();
        return chapterList is null or [] ? Results.NotFound() : Results.Ok(chapterList);
    }

    /// <summary>
    /// Gets a paged list of chapters by catalog ID.
    /// </summary>
    /// <param name="chapterRepository">Chapter repository.</param>
    /// <param name="catalogId">ID of the catalog.</param>
    /// <param name="pageNumber">Page number. Defaults to 1.</param>
    /// <param name="pageSize">Page size or number of items. Defaults to 50.</param>
    /// <returns>A paged list of chapters.</returns>
    /// <remarks>
    /// If no chapters are found, this method returns 404.
    /// </remarks>
    private static async Task<IResult> GetPagedChapters(ChapterRepository chapterRepository, long catalogId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var chapterList = await chapterRepository.GetPagedListByCatalogAsync(catalogId, pageNumber, pageSize);
        if (chapterList is []) return Results.NotFound();
        var totalRecords = await chapterRepository.GetTotalCountAsync();
        var response = new PagedResponse<ChapterDTO>(pageNumber, chapterList.Count, totalRecords, chapterList);
        return Results.Ok(response);
    }
}