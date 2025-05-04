using System.ComponentModel.DataAnnotations.Schema;

namespace ANTWebAPI.Database.Entities;


/// <summary>
/// Represents an article entity used for storing article data.
/// </summary>
/// <remarks>
/// An Article is associated with a Catalog and can have multiple pieces of content. 
/// This class maps to the database schema where each article is linked to a catalog and
/// contains information such as title, description, and a date or banner.
/// </remarks>
public class Article
{
    /// <summary>
    /// Gets the unique identifier for the article.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets or sets the catalog identifier to which this article belongs.
    /// </summary>
    public long CatalogId { get; set; }

    /// <summary>
    /// Gets or sets the title of the article.
    /// </summary>
    /// <remarks>
    /// This is a required field and should not be empty.
    /// </remarks>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the article.
    /// </summary>
    /// <remarks>
    /// This is a required field and should not be empty.
    /// </remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date or banner associated with the article.
    /// </summary>
    /// <remarks>
    /// This is a required field and should not be empty.
    /// </remarks>
    public string DateOrBanner { get; set; } = string.Empty;

    /// <summary>
    /// Gets the catalog to which this article belongs.
    /// </summary>
    /// <remarks>
    /// This is a navigation property used by Entity Framework to establish a relationship
    /// between the Article and Catalog entities.
    /// </remarks>
    [ForeignKey(nameof(CatalogId))]
    public Catalog Catalog { get; init; } = null!;

    /// <summary>
    /// Gets the collection of content items associated with this article.
    /// </summary>
    /// <remarks>
    /// This is a navigation property used by Entity Framework to establish a one-to-many
    /// relationship between Article and Content entities.
    /// </remarks>
    public ICollection<Content> Contents { get; init; } = null!;
}
