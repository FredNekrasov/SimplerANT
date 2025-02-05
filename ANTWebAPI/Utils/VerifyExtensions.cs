using ANTWebAPI.DTOs;

namespace ANTWebAPI.Utils;

public static class VerifyExtensions //Only extenstion functions that check the data
{
    //simplification of conditions
    /// <summary>
    /// Checks if a string is null, empty, or whitespace only.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>true if the string is null, empty, or whitespace only; false otherwise.</returns>
    private static bool IsBlank(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    
    private static bool IsNegative(this long l) => l < 0;

    //Validate data
    public static bool IsDataValid(this CatalogDTO catalog) => !(catalog.Id.IsNegative() || catalog.Name.IsBlank());
    
    public static bool IsDataValid(this ArticleDTO article) => !(article.Id.IsNegative() || article.Title.IsBlank() || article.Catalog.Id.IsNegative());

    public static bool IsDataValid(this ContentDTO content) => !(content.Id.IsNegative() || content.Data.IsBlank() || content.ArticleId.IsNegative());
}
