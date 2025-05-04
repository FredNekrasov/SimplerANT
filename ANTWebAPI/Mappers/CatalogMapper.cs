using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;

namespace ANTWebAPI.Mappers;

/// <summary>
/// A static class that provides extension methods to convert <see cref="Catalog"/> into the <see cref="CatalogDTO"/> and vice versa.
/// </summary>
public static class CatalogMapper
{
    /// <summary>
    /// Converts a <see cref="Catalog"/> into the <see cref="CatalogDTO"/>.
    /// </summary>
    /// <param name="catalog">The catalog to convert.</param>
    /// <returns>The converted catalog data transfer object.</returns>
    public static CatalogDTO ToDto(this Catalog catalog) => new()
    {
        Id = catalog.Id,
        Name = catalog.Name
    };
    /// <summary>
    /// Converts a <see cref="CatalogDTO"/> into the <see cref="Catalog"/>.
    /// </summary>
    /// <param name="catalogDto">The catalog data transfer object to convert.</param>
    /// <returns>The converted catalog entity.</returns>
    public static Catalog ToModel(this CatalogDTO catalogDto) => new()
    {
        Id = catalogDto.Id,
        Name = catalogDto.Name
    };
}
