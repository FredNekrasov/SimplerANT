using ANTWebAPI.Database;
using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;
using ANTWebAPI.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Repository;

public class ChapterRepository(ANTDbContext dbContext)
{
    public async Task<List<ChapterDTO>> GetListAsync()
    {
        var articles = await dbContext.Articles.AsNoTracking()
            .Include(e => e.Catalog)
            .ToListAsync();
        return await GetChaptersAsync(articles);
    }
    
    /// <summary>
    /// Gets a list of chapters by catalog ID, with optional pagination.
    /// </summary>
    /// <param name="catalogId">The ID of the catalog to get chapters from.</param>
    /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The page size or number of items. Defaults to 50.</param>
    /// <returns>A list of chapters.</returns>
    /// <remarks>
    /// If the catalog is not found, this method returns an empty list.
    /// If the catalog ID is 1, this method returns the special list of chapters.
    /// </remarks>
    public async Task<List<ChapterDTO>> GetPagedListByCatalogAsync(long catalogId, int pageNumber, int pageSize)
    {
        if (!await dbContext.Catalogs.AnyAsync(e => e.Id == catalogId)) return [];
        if (catalogId == 1) return await GetSpecialListAsync();
        var articles = await dbContext.Articles.AsNoTracking()
            .Where(e => e.CatalogId == catalogId)
            .Include(e => e.Catalog)
            .ToListAsync();
        var chapterList = await GetChaptersAsync(articles);
        int startIndex = (pageNumber - 1) * pageSize, endIndex = startIndex + pageSize;
        return articles.Count < pageSize ? chapterList : chapterList.Take(startIndex..endIndex).ToList();
    }
    
    public Task<int> GetTotalCountAsync() => dbContext.Articles.CountAsync();
    private readonly long[] excludedCatalogIds = [2, 5, 7, 8, 13];
    
    /// <summary>
    /// Retrieves a special list of chapters, excluding those from specific catalog IDs.
    /// </summary>
    /// <returns>A list of ChapterDTO objects, representing the chapters from articles
    /// not associated with the excluded catalog IDs.</returns>
    /// <remarks>
    /// This method filters articles based on their CatalogId, excluding those that
    /// belong to the predefined excluded catalog IDs, and returns the corresponding chapters.
    /// </remarks>
    private async Task<List<ChapterDTO>> GetSpecialListAsync()
    {
        var mainArticles = await dbContext.Articles.AsNoTracking()
            .Where(e => !excludedCatalogIds.Contains(e.CatalogId))
            .Include(e => e.Catalog)
            .ToListAsync();
        return await GetChaptersAsync(mainArticles);
    }
    
    /// <summary>
    /// Retrieves a list of chapters, given a list of articles.
    /// </summary>
    /// <param name="articles">The list of articles.</param>
    /// <returns>A list of ChapterDTO objects, representing the chapters from the given articles.</returns>
    /// <remarks>
    /// This method groups the contents of the given articles by article ID and
    /// uses this grouped content to create a ChapterDTO for each article.
    /// </remarks>
    private async Task<List<ChapterDTO>> GetChaptersAsync(List<Article> articles)
    {
        var articleIds = articles.Select(a => a.Id).ToList();
        var groupedContent = await dbContext.Contents.AsNoTracking()
            .Where(c => articleIds.Contains(c.ArticleId))
            .GroupBy(e => e.ArticleId)
            .ToDictionaryAsync(g => g.Key, g => g.Select(c => c.Data).ToList());
        var listChapters = articles.Select(article => 
            CreateChapter(article, groupedContent.TryGetValue(article.Id, out var cList) ? cList : [])
        ).ToList();
        return listChapters;
    }

    private static ChapterDTO CreateChapter(Article article, List<string> contentList) => new()
    {
        Id = article.Id,
        Catalog = article.Catalog.ToDto(),
        Title = article.Title,
        Description = article.Description,
        DateOrBanner = article.DateOrBanner,
        Content = contentList
    };
}
