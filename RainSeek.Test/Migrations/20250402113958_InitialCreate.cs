using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RainSeed.Tests.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tokens_documents",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    token_id = table.Column<long>(type: "INTEGER", nullable: false),
                    document_id = table.Column<string>(type: "TEXT", nullable: false),
                    start_position = table.Column<int>(type: "INTEGER", nullable: false),
                    end_position = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens_documents", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "token_index",
                table: "tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "document_id_token_id",
                table: "tokens_documents",
                columns: new[] { "document_id", "token_id" });

            migrationBuilder.CreateIndex(
                name: "token_id_document_id",
                table: "tokens_documents",
                columns: new[] { "token_id", "document_id" });

            migrationBuilder.CreateIndex(
                name: "unique",
                table: "tokens_documents",
                columns: new[] { "token_id", "document_id", "start_position", "end_position" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "tokens_documents");
        }
    }
}
