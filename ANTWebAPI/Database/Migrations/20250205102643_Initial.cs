using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANTWebAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ANTCatalogs",
                columns: table => new
                {
                    catalog_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANTCatalogs", x => x.catalog_id);
                });

            migrationBuilder.CreateTable(
                name: "ANTArticles",
                columns: table => new
                {
                    article_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    catalog_id = table.Column<long>(type: "INTEGER", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    date_or_banner = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANTArticles", x => x.article_id);
                    table.ForeignKey(
                        name: "FK_ANTArticles_ANTCatalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "ANTCatalogs",
                        principalColumn: "catalog_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANTContents",
                columns: table => new
                {
                    content_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    article_id = table.Column<long>(type: "INTEGER", nullable: false),
                    data = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANTContents", x => x.content_id);
                    table.ForeignKey(
                        name: "FK_ANTContents_ANTArticles_article_id",
                        column: x => x.article_id,
                        principalTable: "ANTArticles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANTArticles_catalog_id",
                table: "ANTArticles",
                column: "catalog_id");

            migrationBuilder.CreateIndex(
                name: "IX_ANTContents_article_id",
                table: "ANTContents",
                column: "article_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANTContents");

            migrationBuilder.DropTable(
                name: "ANTArticles");

            migrationBuilder.DropTable(
                name: "ANTCatalogs");
        }
    }
}
