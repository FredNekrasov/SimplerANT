using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;

namespace ANTWebAPI.Mappers;

/// <summary>
/// Article mapper defines extension methods to map ArticleDTO into the Article model and vice versa
/// </summary>
public static class ArticleMapper
{
    public static ArticleDTO ToDto(this Article article) => new()
    {
        Id = article.Id,
        Title = article.Title,
        Catalog = article.Catalog.ToDto(),
        Description = article.Description,
        DateOrBanner = article.DateOrBanner
    };
    public static Article ToModel(this ArticleDTO articleDto) => new()
    {
        Id = articleDto.Id,
        Title = articleDto.Title,
        CatalogId = articleDto.Catalog.Id,
        Description = articleDto.Description,
        DateOrBanner = articleDto.DateOrBanner
    };
}
