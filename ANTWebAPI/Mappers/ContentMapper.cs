using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;

namespace ANTWebAPI.Mappers;

/// <summary>
/// A static class that provides extension methods to convert <see cref="Content"/> into the <see cref="ContentDTO"/> and vice versa.
/// </summary>
public static class ContentMapper
{
    /// <summary>
    /// Converts a <see cref="Content"/> to a <see cref="ContentDTO"/>.
    /// </summary>
    /// <param name="content">The content entity to convert.</param>
    /// <returns>The converted content data transfer object.</returns>
    public static ContentDTO ToDto(this Content content) => new()
    {
        Id = content.Id,
        ArticleId = content.ArticleId,
        Data = content.Data
    };

    /// <summary>
    /// Converts a <see cref="ContentDTO"/> to a <see cref="Content"/>.
    /// </summary>
    /// <param name="contentDto">The content data transfer object to convert.</param>
    /// <returns>The converted content entity.</returns>
    public static Content ToModel(this ContentDTO contentDto) => new()
    {
        Id = contentDto.Id,
        ArticleId = contentDto.ArticleId,
        Data = contentDto.Data
    };
}
