using ANTWebAPI.DTOs;

namespace ANTWebAPI.Utils;

public static class VerifyExtensions
{
    /// <summary>
    /// Checks if a string is null, empty, or whitespace only.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>
    /// true if the string is null, empty, or whitespace only; false otherwise.
    /// </returns>
    private static bool IsBlank(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    
    /// <summary>
    /// Determines if a given long integer is negative.
    /// </summary>
    /// <param name="l">The long integer to evaluate.</param>
    /// <returns>
    /// true if the long integer is less than zero; otherwise, false.
    /// </returns>
    private static bool IsNegative(this long l) => l < 0;

    /// <summary>
    /// Determines if a given <see cref="CatalogDTO"/> instance is valid.
    /// </summary>
    /// <param name="catalog">The <see cref="CatalogDTO"/> instance to evaluate.</param>
    /// <returns>
    /// true if the <see cref="CatalogDTO"/> instance is valid; otherwise, false.
    /// <para>A <see cref="CatalogDTO"/> is considered valid if its ID is not negative and its name is not null, empty, or whitespace only.</para>
    /// </returns>
    public static bool IsDataValid(this CatalogDTO catalog) => !(catalog.Id.IsNegative() || catalog.Name.IsBlank());
    
    /// <summary>
    /// Determines if a given <see cref="ArticleDTO"/> instance is valid.
    /// </summary>
    /// <param name="article">The <see cref="ArticleDTO"/> instance to evaluate.</param>
    /// <returns>
    /// true if the <see cref="ArticleDTO"/> instance is valid; otherwise, false.
    /// <para>
    /// An <see cref="ArticleDTO"/> is considered valid if its ID, catalog's ID is not negative,
    /// and its title is not null, empty, or whitespace only.
    /// </para>
    /// </returns>
    public static bool IsDataValid(this ArticleDTO article) => !(article.Id.IsNegative() || article.Title.IsBlank() || article.Catalog.Id.IsNegative());

    /// <summary>
    /// Determines if a given <see cref="ContentDTO"/> instance is valid.
    /// </summary>
    /// <param name="content">The <see cref="ContentDTO"/> instance to evaluate.</param>
    /// <returns>
    /// true if the <see cref="ContentDTO"/> instance is valid; otherwise, false.
    /// <para>
    /// A <see cref="ContentDTO"/> is considered valid if its ID, its article ID is not negative,
    /// and its data is not null, empty, or whitespace only.
    /// </para>
    /// </returns>
    public static bool IsDataValid(this ContentDTO content) => !(content.Id.IsNegative() || content.Data.IsBlank() || content.ArticleId.IsNegative());
}
