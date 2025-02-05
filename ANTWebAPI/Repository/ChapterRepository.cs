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
        var contentList = await dbContext.Contents.AsNoTracking().ToListAsync();
        return GetChaptersAsync(articles, contentList);
    }
    
    /// <summary>
    /// Gets a paged list of chapters by catalog ID.
    /// </summary>
    /// <param name="catalogId">ID of the catalog.</param>
    /// <param name="pageNumber">Page number. Defaults to 1.</param>
    /// <param name="pageSize">Page size or number of items. Defaults to 50.</param>
    /// <returns>A paged list of chapters.</returns>
    /// <remarks>
    /// If catalog ID is not found, this method returns an empty list.
    /// </remarks>
    public async Task<List<ChapterDTO>> GetPagedListByCatalogAsync(long catalogId, int pageNumber, int pageSize)
    {
        if (!await dbContext.Catalogs.AnyAsync(e => e.Id == catalogId)) return [];
        var contentList = await dbContext.Contents.AsNoTracking().ToListAsync();
        if (catalogId == 1) return await GetSpecialListAsync(contentList);
        var articles = await dbContext.Articles.AsNoTracking()
            .Where(e => e.CatalogId == catalogId)
            .Include(e => e.Catalog)
            .ToListAsync();
        var chapterList = GetChaptersAsync(articles, contentList);
        int startIndex = (pageNumber - 1) * pageSize, endIndex = startIndex + pageSize;
        return articles.Count < pageSize ? chapterList : chapterList.Take(startIndex..endIndex).ToList();
    }
    
    public async Task<int> GetTotalCountAsync() => await dbContext.Articles.CountAsync();
    
    /// <summary>
    /// Gets main chapters, that is articles count is less than pageSize.
    /// </summary>
    /// <param name="contentList">Content List is used for getting content data (urls, images etc.)</param>
    /// <returns>ChapterDTO List</returns>
    private async Task<List<ChapterDTO>> GetSpecialListAsync(List<Content> contentList)
    {
        var mainArticles = await dbContext.Articles.AsNoTracking()
            .Where(e => e.CatalogId != 2 && e.CatalogId != 5 && e.CatalogId != 7 && e.CatalogId != 8 && e.CatalogId != 13)
            .Include(e => e.Catalog)
            .ToListAsync();
        return GetChaptersAsync(mainArticles, contentList);
    }
    
    private static List<ChapterDTO> GetChaptersAsync(List<Article> articles, List<Content> contentList)
    {
        List<ChapterDTO> chapterList = [];
        chapterList.AddRange(
            from article in articles
            let content = contentList.Where(content => content.ArticleId == article.Id)
                .Select(content => content.Data)
                .ToList()
            select new ChapterDTO
            {
                Id = article.Id,
                Catalog = article.Catalog.ToDto(),
                Title = article.Title,
                Description = article.Description,
                DateOrBanner = article.DateOrBanner,
                Content = content
            }
        );
        return chapterList;
    }
}