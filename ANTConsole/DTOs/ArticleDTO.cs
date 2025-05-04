namespace ANTConsole.DTOs;

public record ArticleDTO(long Id, CatalogDTO Catalog, string Title, string Description, string DateOrBanner);
