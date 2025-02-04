using ANTWebAPI.DTOs;
using ANTWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ANTWebAPI.APIs;

public static class ChapterAPI
{
    public static RouteGroupBuilder MapChapterAPIEndpoints(this RouteGroupBuilder chapterApi)
    {
        chapterApi.MapGet("/", GetAllChapters);
        chapterApi.MapGet("/{catalogId:long}", GetPagedChapters);
        return chapterApi;
    }

    private static async Task<IResult> GetAllChapters(ChapterRepository chapterRepository)
    {
        var chapterList = await chapterRepository.GetListAsync();
        return chapterList is [] ? Results.NotFound() : Results.Ok(chapterList);
    }

    private static async Task<IResult> GetPagedChapters(ChapterRepository chapterRepository, long catalogId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var chapterList = await chapterRepository.GetPagedListByCatalogAsync(catalogId, pageNumber, pageSize);
        if (chapterList is []) return Results.NotFound();
        var totalRecords = await chapterRepository.GetTotalCountAsync();
        var response = new PagedResponse<ChapterDTO>(pageNumber, chapterList.Count, totalRecords, chapterList);
        return Results.Ok(response);
    }
}