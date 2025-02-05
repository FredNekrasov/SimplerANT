using ANTWebAPI.Database.Entities;
using ANTWebAPI.DTOs;

namespace ANTWebAPI.Mappers;

/// <summary>
/// Catalog mapper defines extension methods to map CatalogDTO into the Catalog model and vice versa
/// </summary>
public static class CatalogMapper
{
    public static CatalogDTO ToDto(this Catalog catalog) => new()
    {
        Id = catalog.Id,
        Name = catalog.Name
    };
    public static Catalog ToModel(this CatalogDTO catalogDto) => new()
    {
        Id = catalogDto.Id,
        Name = catalogDto.Name
    };
}
