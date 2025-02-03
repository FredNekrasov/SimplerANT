using ANTWebAPI.DTOs;

namespace ANTWebAPI.Utils;

public static class VerifyExtensions //Only extenstion functions that check the data
{
    //simplification of conditions
    /**
     * IsBlank - check if string is null or empty or white space only
     * @param string str - string to check for blank
     * @return bool - true if string is null or empty or white space only false otherwise
     */
    private static bool IsBlank(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    private static bool IsNegative(this long l) => l < 0;

    //Validate data
    public static bool IsDataValid(this CatalogDTO catalog) => !(catalog.Id.IsNegative() || catalog.Name.IsBlank());
    public static bool IsDataValid(this ArticleDTO article) => !(article.Id.IsNegative() || article.Title.IsBlank() || article.Catalog.Id.IsNegative());
    public static bool IsDataValid(this ContentDTO content) => !(content.Id.IsNegative() || content.Data.IsBlank() || content.ArticleId.IsNegative());
}
