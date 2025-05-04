using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;

namespace ANTWebAPI.Mappers;

/// <summary>
/// A static class that provides extension methods to convert an <see cref="Article"/> to an <see cref="ArticleDTO"/> and vice versa.
/// </summary>
public static class ArticleMapper
{
    /// <summary>
    /// Converts an <see cref="Article"/> to an <see cref="ArticleDTO"/>.
    /// </summary>
    /// <param name="article">The article to convert.</param>
    /// <returns>The converted article data transfer object.</returns>
    public static ArticleDTO ToDto(this Article article) => new()
    {
        Id = article.Id,
        Title = article.Title,
        Catalog = article.Catalog.ToDto(),
        Description = article.Description,
        DateOrBanner = article.DateOrBanner
    };
    /// <summary>
    /// Converts an <see cref="ArticleDTO"/> to an <see cref="Article"/>.
    /// </summary>
    /// <param name="articleDto">The article data transfer object to convert.</param>
    /// <returns>The converted article entity.</returns>
    public static Article ToModel(this ArticleDTO articleDto) => new()
    {
        Id = articleDto.Id,
        Title = articleDto.Title,
        CatalogId = articleDto.Catalog.Id,
        Description = articleDto.Description,
        DateOrBanner = articleDto.DateOrBanner
    };
}
