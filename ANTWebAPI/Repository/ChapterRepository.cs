using ANTWebAPI.Database;
using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;
using ANTWebAPI.mappers;
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
    
    /*
     * GetPagedListByCatalogAsync method is used for getting ChapterDTO List by Catalog id with pagination
     * 
     * @param catalogId - Catalog id 
     * @param pageNumber - Page number is used to get specific page of data
     * @param pageSize - Page size/number of items
     * 
     * @return List<ChapterDTO> - ChapterDTO List
     */
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
    
    /**
     * GetSpecialListAsync method is used for getting main chapters, that is articles count is less than pageSize
     * 
     * @param contentList - Content List is used for getting content data (urls, images etc.)
     * 
     * @return ChapterDTO List
     */
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