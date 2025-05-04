namespace ANTWebAPI.Database.Entities;

/// <summary>
/// Represents a catalog entity in the database.
/// </summary>
/// <remarks>
/// A catalog serves as a collection of articles.
/// Each catalog has a unique identifier and a name.
/// Catalogs have a one-to-many relationship with articles,
/// meaning that a catalog can contain multiple articles.
/// </remarks>
public class Catalog
{
    /// <summary>
    /// Gets the unique identifier for the catalog.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets or sets the name of the catalog.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of articles associated with this catalog.
    /// </summary>
    public ICollection<Article> Articles { get; init; } = null!;
}
