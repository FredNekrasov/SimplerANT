using System.ComponentModel.DataAnnotations.Schema;

namespace ANTWebAPI.Database.Entities;

/// <summary>
/// Represents a content entity used for storing data such as contacts, images, or any other information
/// related to a specific article.
/// </summary>
/// <remarks>
/// Each content is associated with one article and contains information such as data (e.g. contacts, images, etc.).
/// </remarks>
public class Content
{
    /// <summary>
    /// The ID of the content.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The ID of the article associated with this content.
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// The data associated with this content (e.g. contacts, images, etc.).
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// The article associated with this content.
    /// </summary>
    /// <remarks>
    /// This property is a navigation property and is used to access the article associated with this content.
    /// </remarks>
    [ForeignKey(nameof(ArticleId))]
    public Article Article { get; init; } = null!;
}
